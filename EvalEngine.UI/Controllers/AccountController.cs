// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// <summary>
//   The account controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using EvalEngine.Domain.Abstract;
using EvalEngine.Domain.Entities;
using EvalEngine.Infrastructure.Abstract;
using EvalEngine.UI.Infrastructure.Filters;
using EvalEngine.UI.Models;
using Resources;

namespace EvalEngine.UI.Controllers
{
    /// <summary>
    /// The account controller.
    /// </summary>
    [HandleError]
    public class AccountController : Controller
    {
        /// <summary>
        /// The state assignment repository.
        /// </summary>
        private readonly IStateAssignmentRepository _stateAssignmentRepository;

        /// <summary>
        /// The state repository.
        /// </summary>
        private readonly IStateRepository _stateRepository;

        /// <summary>
        /// The user account info repository.
        /// </summary>
        private readonly IUserAccountInfoRepository _userAccountInfoRepository;

        /// <summary>
        /// The user account info repository.
        /// </summary>
        private readonly IPasswordHistoryRepository _passwordHistoryRepository;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>Initializes a new instance of the <see cref="AccountController"/> class.</summary>
        /// <param name="logger">The logger</param>
        /// <param name="stateAssignmentRepository">The state assignment repository. </param>
        /// <param name="stateRepository">The state repository.</param>
        /// <param name="userAccountInfoRepository">The user account info repository</param>
        /// <param name="passwordHistoryRepository">The password history repository</param>
        public AccountController(ILogger logger, IStateAssignmentRepository stateAssignmentRepository, IStateRepository stateRepository, IUserAccountInfoRepository userAccountInfoRepository, IPasswordHistoryRepository passwordHistoryRepository)
        {
            _logger = logger;
            _stateAssignmentRepository = stateAssignmentRepository;
            _stateRepository = stateRepository;
            _userAccountInfoRepository = userAccountInfoRepository;
            _passwordHistoryRepository = passwordHistoryRepository;
        }

        /// <summary>
        ///   Gets or sets the membership service.
        /// </summary>
        /// <value> The membership service. </value>
        public IMembershipService MembershipService { get; set; }

        /// <summary>
        ///   Gets or sets the forms authentication service.
        /// </summary>
        /// <value> The forms authentication service. </value>
        public IFormsAuthenticationService FormsService { get; set; }

        /// <summary>
        /// The LogOn view
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult LogOn(string returnUrl)
        {
            if (TempData["logonMessage"] == null) 
            {
                TempData["logonMessage"] = string.Empty;
            }

            if (Server.UrlDecode(returnUrl) == "/")
                returnUrl = ConfigurationManager.AppSettings["environment"];

            if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
                returnUrl = ConfigurationManager.AppSettings["environment"] + Server.UrlEncode(Request.UrlReferrer.PathAndQuery);


            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.ReturnURL = returnUrl;
            }

            return View();
        }

        /// <summary>
        /// The LogOn action
        /// </summary>
        /// <param name="logIn">The submit button text</param>
        /// <param name="model">
        /// The LogOn model.
        /// </param>
        /// <param name="returnUrl">
        /// The URL to return to after logging on.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOn(string logIn, LogOnModel model, string returnUrl)
        {
            if (logIn.Equals("Forgot Password"))
            {
                return RedirectToAction("SendResetUrl");
            }
            //// Validate the model
            if (!ModelState.IsValid)
            {
                return View("LogOn", model);
            }

            model.UserName = Membership.GetUserNameByEmail(model.Email);
            if (string.IsNullOrEmpty(model.UserName))
            {
                TempData["message"] = UserFeedbackMessages.InvalidCredentials;
                return RedirectToAction("LogOn", new
                {
                    returnUrl = returnUrl
                });
            }

            var accountManager = new AccountManager(_logger, _userAccountInfoRepository, _stateAssignmentRepository, _passwordHistoryRepository);
            var accountContainer = accountManager.GetUserAccountContainer(model.UserName);
            var user = accountContainer.User;
            var userId = new Guid(user.ProviderUserKey.ToString());

            // Ensure every user has a record in the UserAccountInfo table.
            var account = _userAccountInfoRepository.SearchForFirstOrDefault(x => x.UserId.Equals(userId));
            if (account == null)
            {
                _userAccountInfoRepository.CreateUserAccountInfo(userId);
            }

            //// Validate credentials AND redirect to appropriate page
            var messageIfPwdExpired = UserFeedbackMessages.ExpiredPassword;
            Tuple<bool, string> validation = ValidateLogOnPassword(user, model, messageIfPwdExpired);
            if (validation.Item1)
            {
                FormsService.SignIn(user.UserName, model.RememberMe);
                UserProfile profile = UserProfile.GetUserProfile(model.UserName);
                HttpContext.Session["fullName"] = Server.HtmlEncode(profile.GetUserFullName());
                TempData["message"] = validation.Item2;
                _logger.Info(string.Format("INFO c: AccountController, a: LogOn, p: {0}, t: {1}", model.Email, DateTime.Now.ToShortDateString()));

                string decodedUrl = "";
                if (!string.IsNullOrEmpty(returnUrl))
                    decodedUrl = Server.UrlDecode(returnUrl);

                if (Url.IsLocalUrl(decodedUrl))
                    return Redirect(decodedUrl);
                else
                    return RedirectToAction("Index", "Home");
            }

            //// Redirect if password expired
            TempData["message"] = validation.Item2;
            if (validation.Item2.Equals(messageIfPwdExpired))
            {
                Membership.UpdateUser(user);
                var accountMembershipService = new AccountMembershipService();
                FormsService.SignIn(user.UserName, false);

                return RedirectToAction("ChangePassword");
            }

            //// Plain fail
            _logger.Info("Invalid login " + model.Email);
            return View("LogOn", model);
        }

        /// <summary>
        /// The Password Requirements view
        /// </summary> 
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ViewResult PasswordRequirements()
        {
            return View();
        }


        /// <summary>
        /// The LogOff action
        /// </summary> 
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("LogOn", "Account");
        }

        /// <summary>
        /// The ChangePasswordPartial action
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.PartialViewResult.
        /// </returns>
        [Authorize]
        public PartialViewResult ChangePasswordPartial()
        {
            ViewData["PasswordLength"] = 12;
            return PartialView();
        }

        /// <summary>
        /// The ChangePasswordPartial action
        /// </summary>
        /// <param name="model">
        /// The ChangePasswordModel model.
        /// </param>
        /// <param name="user">
        /// The IPrincipal user, optional
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.PartialViewResult.
        /// </returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult ChangePasswordPartial(ChangePasswordModel model, IPrincipal user)
        {
            ViewData["PasswordLength"] = 12;
            if (ModelState.IsValid)
            {
                //// ChangePassword will throw an exception rather
                //// than return false in certain failure scenarios.
                MembershipUser thisUser = Membership.GetUser(user.Identity.Name);
                var userId = new Guid(thisUser.ProviderUserKey.ToString());
                var account = _userAccountInfoRepository.SearchForFirstOrDefault(x => x.UserId.Equals(userId));
                bool changePasswordSucceeded;
                try
                {
                    if (_passwordHistoryRepository.CanUsePassword(account.UserId, model.Password, account.Salt))
                    {
                        MembershipUser currentUser = Membership.GetUser(user.Identity.Name, true /* userIsOnline */);
                        changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.Password);
                        _passwordHistoryRepository.UpdatePasswordHistory(account.UserId, model.Password, account.Salt);
                        TempData["pwd_message"] = UserFeedbackMessages.PasswordUpdateSuccess;
                    }
                    else
                    {
                        changePasswordSucceeded = false;
                        TempData["pwd_message"] = UserFeedbackMessages.PasswordInHistory;
                        ModelState.AddModelError("changePasswordError", UserFeedbackMessages.PasswordInHistory);
                    }
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                    TempData["pwd_message"] = UserFeedbackMessages.PasswordUpdateFail;
                    ModelState.AddModelError("changePasswordError", UserFeedbackMessages.PasswordUpdateFail);
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("MyAccount", "Account");
                }
            }

            MyAccountModel e = new MyAccountModel();
            MembershipUser cuser = Membership.GetUser(user.Identity.Name);
            UserProfile profile = UserProfile.GetUserProfile(user.Identity.Name);

            ////e.UserName = user.UserName;
            e.Email = cuser.Email;
            e.FirstName = profile.FirstName;
            e.LastName = profile.LastName;
            e.Title = profile.Title;
            e.Organization = profile.Organization;
            ViewData["MyAccountModel"] = e;
            ViewData["ChangePasswordModel"] = new ChangePasswordModel();
            TempData["pwd_message"] = UserFeedbackMessages.PasswordUpdateFail;
            ModelState.AddModelError(string.Empty, string.Empty);

            //// If we got this far, something failed, redisplay form
            return View("MyAccount");
        }

        /// <summary>
        ///   Changes the password.
        /// </summary>
        /// <returns> Change Password View </returns>
        public ActionResult ChangePassword()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="model">
        /// The model. 
        /// </param>
        /// <returns>
        /// Change Password View 
        /// </returns>
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            // Validate the data submitted by the user.
            if (!ModelState.IsValid)
            {
                ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
                return View(model);
            }
            MembershipUser thisUser = Membership.GetUser(User.Identity.Name);
            var userId = new Guid(thisUser.ProviderUserKey.ToString());
            var account = _userAccountInfoRepository.SearchForFirstOrDefault(x => x.UserId.Equals(userId));
            MembershipUser user = Membership.GetUser(account.UserId);
            var tempPassword = user.ResetPassword();
            model.OldPassword = tempPassword;

            return HandleChangePassword(model, User.Identity.Name);
        }

        /// <summary>
        /// Displays ChangeExpiredPassowrd view.
        /// </summary>
        /// <returns>The ChangeExpiredPassowrd view</returns>
        public ActionResult ChangeExpiredPassword(string userName)
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            ChangeExpiredPasswordModel model = new ChangeExpiredPasswordModel();
            model.UserName = userName;
            return View(model);
        }

        /// <summary>
        /// Dispatches HandleChangePassword after form is submitted.
        /// </summary>
        /// <param name="model">The ChangeExpiredPassword view model.</param>
        /// <returns>Redirects user to Home or LogOn</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeExpiredPassword(ChangeExpiredPasswordModel model)
        {
            // Validate the data submitted by the user.
            if (!ModelState.IsValid)
            {
                ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
                return View(model);
            }

            return HandleChangePassword(model, model.UserName, "LogOn");
        }

        /// <summary>
        /// The ChangePasswordSuccess view
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        /// <summary>
        ///   Adds a new User
        /// </summary>
        /// <returns> User Add View </returns>
        [Authorize]
        [RoleFilter(Action = "Index", Controller = "Home", RolesWithAccess = new[] { Constants.SiteAdministratorRole, Constants.ProjectAdminRole, Constants.StateAdminRole }, Message = "Only Administrators have access to this section.")]
        public ActionResult Add()
        {
            UserAddModel n = new UserAddModel();
            n.RoleSelections = Roles.GetAllRoles().Select(x => new RoleSelection { RoleName = x, IsChecked = false }).ToList();
            var stateslist = _stateRepository.GetStates();
            n.CheckboxStateSelections = stateslist.Select(
                    i => new StateSelection { StateName = i.FullName, StateId = i.StateId, IsChecked = false }).ToList();
            n.RadioStateSelections = stateslist.Select(
                    i => new StateSelection { StateName = i.FullName, StateId = i.StateId, IsChecked = false }).ToList();

            return View(n);
        }

        /// <summary>
        /// Adds a new user
        /// </summary>
        /// <param name="model">
        /// The model. 
        /// </param>
        /// <returns>
        /// User Add View 
        /// </returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleFilter(Action = "Index", Controller = "Home", RolesWithAccess = new[] { Constants.SiteAdministratorRole, Constants.ProjectAdminRole, Constants.StateAdminRole }, Message = "Only Administrators have access to this section.")]
        public ActionResult Add(UserAddModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                        var accountManager = new AccountManager(_logger, _userAccountInfoRepository, _stateAssignmentRepository, _passwordHistoryRepository);
                        
                        var createStatus = accountManager.CreateUserAccount(model);
                        if (AccountManager.AccountTransactionWasSuccessful(createStatus))
                        {
                            // Get user info
                            var userName = Membership.GetUserNameByEmail(model.Email);
                            var user = Membership.GetUser(userName);
                            var userId = user.ProviderUserKey.ToString();
                            var account = _userAccountInfoRepository.SearchForFirstOrDefault(x => x.UserId.Equals(userId));
                            UserProfile profile = UserProfile.GetUserProfile(model.UserName);
                            //this.AssignStates(profile, model.RadioSelectedState, model.CheckboxStateSelections);
                            List<string> assignedStates = _stateAssignmentRepository.GetStateNamesByUserName(model.UserName);
                            string csv = string.Join(",", assignedStates);
                            profile.UserStatesCsv = csv;
                            profile.Save();
                            // Send confirmation email
                            SendAccountVerificationEmail(model.Email, account.VerifyToken);
                            TempData["message"] = UserFeedbackMessages.UserAccountCreatedSuccessfully;
                            return RedirectToAction("Users", "Account");
                        }
            }

            var stateslist = _stateRepository.GetStates();
            model.RoleSelections = Roles.GetAllRoles().Select(x => new RoleSelection { RoleName = x, IsChecked = false }).ToList();
            model.RadioStateSelections = stateslist.Select(
                    i => new StateSelection { StateName = i.FullName, StateId = i.StateId, IsChecked = false }).ToList();
            model.CheckboxStateSelections = stateslist.Select(
                    i => new StateSelection { StateName = i.FullName, StateId = i.StateId, IsChecked = false }).ToList();

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        /// <summary>
        /// The Edit action
        /// </summary>
        /// <param name="model">
        /// The UserEditModel model
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleFilter(Action = "Index", Controller = "Home", RolesWithAccess = new[] { Constants.SiteAdministratorRole, Constants.ProjectAdminRole, Constants.StateAdminRole }, Message = "Only Administrators have access to this section.")]
        public ActionResult Edit(UserEditModel model)
        {
            if (ModelState.IsValid)
            {
                //// ChangePassword will throw an exception rather
                //// than return false in certain failure scenarios.
                bool editMyAccountSucceeded;
                try
                {
                    var user = Membership.GetUser(model.UserName);

                    user.IsApproved = model.IsActive;
                    if (model.IsLocked == false)
                    {
                        user.UnlockUser();
                    }

                    if (model.IsLocked)
                    {
                        ////do nothing
                    }

                    Membership.UpdateUser(user);

                    //// add profile information
                    UserProfile profile = UserProfile.GetUserProfile(model.UserName);
                    
                    profile.FirstName = model.FirstName;
                    profile.LastName = model.LastName;
                    profile.Title = model.Title;
                    profile.Organization = model.Organization;
                    profile.AccountType = model.AccountType;
                    
                    _stateAssignmentRepository.DeleteStateAssignmentsByUserName(model.UserName);
                    _stateAssignmentRepository.SubmitChanges();

                    string[] userRoles = Roles.GetRolesForUser(model.UserName);
                    List<string> list = new List<string>(userRoles);
                    if (list.Contains(Constants.SiteAdministratorRole))
                    {
                        list.RemoveAt(list.IndexOf(Constants.SiteAdministratorRole));
                    }

                    userRoles = list.ToArray();
                    if (userRoles.Length != 0)
                    {
                        Roles.RemoveUserFromRoles(model.UserName, userRoles);
                    }
                    Roles.AddUserToRole(model.UserName, model.AccountType);
                    string csv = string.Empty;

                    AssignStates(profile, model.RadioSelectedState, model.CheckboxStateSelections);

                    List<string> assignedStates = _stateAssignmentRepository.GetStateNamesByUserName(model.UserName);

                    csv = string.Join(",", assignedStates);
                    profile.UserStatesCsv = csv;
                    profile.Save();
                    editMyAccountSucceeded = true;
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.Message);
                    editMyAccountSucceeded = false;
                }

                if (editMyAccountSucceeded)
                {
                    return RedirectToAction("Users", "Account", string.Empty);
                }
                ModelState.AddModelError(string.Empty, "There was an error editing your account. Please check the validation messages and try again.");
            }

            var stateslist = _stateRepository.GetStates();

            List<StateSelection> stateselectionlistcb =
                stateslist.Select(
                    i => new StateSelection { StateName = i.FullName, StateId = i.StateId, IsChecked = false }).ToList();
            List<StateSelection> stateselectionlistrb =
                stateslist.Select(
                    i => new StateSelection { StateName = i.FullName, StateId = i.StateId, IsChecked = false }).ToList();
            model.RadioStateSelections = stateselectionlistrb;
            model.CheckboxStateSelections = stateselectionlistcb;

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// The Edit view
        /// </summary>
        /// <param name="userName">
        /// The string userName
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        [RoleFilter(Action = "Index", Controller = "Home", RolesWithAccess = new[] { Constants.SiteAdministratorRole, Constants.ProjectAdminRole, Constants.StateAdminRole }, Message = "Only Administrators have access to this section.")]
        public ActionResult Edit(string userName)
        {

            UserEditModel m = new UserEditModel();
            UserProfile profile = UserProfile.GetUserProfile(userName);

            var user = MembershipService.GetUser(userName, false);
            m.UserName = user.UserName;
            m.Email = user.Email;
            m.FirstName = profile.FirstName;
            m.LastName = profile.LastName;
            m.Title = profile.Title;
            m.Organization = profile.Organization;
            m.AccountType = profile.AccountType;
            m.IsActive = user.IsApproved;
            m.IsLocked = user.IsLockedOut;
            var stateslist = _stateRepository.GetStates();
            List<StateSelection> stateselectionlistcb =
               stateslist.Select(
                   i => new StateSelection { StateName = i.FullName, StateId = i.StateId, IsChecked = false }).ToList();

            if (profile.AccountType.Equals(Constants.MultipleStateUserRole))
            {
                stateselectionlistcb =
               stateslist.Select(i => new StateSelection { StateName = i.FullName, StateId = i.StateId, IsChecked = _stateAssignmentRepository.IsUserNameAssignedToState(user.UserName, i.StateId) }).ToList();
            }
            else if (profile.AccountType.Equals(Constants.StateUserRole) || profile.AccountType.Equals(Constants.StateAdminRole))
            {
                m.RadioSelectedState = _stateAssignmentRepository.GetStateAssignmentsByUserName(user.UserName).First().StateId;
            }

            List<StateSelection> stateselectionlistrb =
                stateslist.Select(
                    i => new StateSelection { StateName = i.FullName, StateId = i.StateId, IsChecked = false }).ToList();
            m.CheckboxStateSelections = stateselectionlistcb;
            m.RadioStateSelections = stateselectionlistrb;
            List<RoleSelection> rolelist = Roles.GetAllRoles().Select(roleloop => new RoleSelection {RoleName = roleloop, IsChecked = Roles.IsUserInRole(roleloop)}).ToList();

            m.RoleSelections = rolelist;
            return View(m);
        }

        /// <summary>
        /// The MyAccount view
        /// </summary>
        /// <param name="user">
        /// The IPrincipal user
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public ActionResult MyAccount(IPrincipal user)
        {
            string userName = user.Identity.Name;
            MyAccountModel e = new MyAccountModel();
            UserProfile profile = UserProfile.GetUserProfile(userName);
            var currentUser = Membership.GetUser(userName);
            ////e.UserName = user.UserName;
            e.Email = currentUser.Email;
            e.FirstName = profile.FirstName;
            e.LastName = profile.LastName;
            e.Title = profile.Title;
            e.Organization = profile.Organization;
            ViewData["MyAccountModel"] = e;
            ViewData["ChangePasswordModel"] = new ChangePasswordModel();
            return View();
        }

        /// <summary>
        /// The MyAccountPartial view
        /// </summary>
        /// <param name="user">
        /// The IPrincipal user
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        public PartialViewResult MyAccountPartial(IPrincipal user)
        {
            string userName = user.Identity.Name;
            MyAccountModel e = new MyAccountModel();
            UserProfile profile = UserProfile.GetUserProfile(userName);
            var currentUser = Membership.GetUser(userName);
            e.Email = currentUser.Email;
            e.FirstName = profile.FirstName;
            e.LastName = profile.LastName;
            e.Title = profile.Title;
            e.Organization = profile.Organization;
            return PartialView(e);
        }

        /// <summary>
        /// The MyAccountPartial action
        /// </summary>
        /// <param name="model">
        /// The MyAccountModel model
        /// </param>
        /// <param name="user">
        /// The IPrincipal user
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyAccountPartial(MyAccountModel model, IPrincipal user)
        {
            if (ModelState.IsValid)
            {
                bool editMyAccountSucceeded;

                try
                {
                    string userName = user.Identity.Name;
                    var currentUser = Membership.GetUser(userName);
                    var profile = UserProfile.GetUserProfile(userName);
                    profile.FirstName = model.FirstName;
                    profile.LastName = model.LastName;
                    profile.Title = model.Title;
                    profile.Organization = model.Organization;
                    profile.Save();
                    editMyAccountSucceeded = true;
                    TempData["acct_message"] = UserFeedbackMessages.UserUpdateSuccess;
                }
                catch (Exception ex)
                {
                    editMyAccountSucceeded = false;
                    TempData["acct_message"] = UserFeedbackMessages.UserUpdateFail;
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                if (editMyAccountSucceeded)
                {
                    return RedirectToAction("MyAccount", "Account");
                }
            }

            string cuserName = user.Identity.Name;
            MyAccountModel e = new MyAccountModel();
            UserProfile cprofile = UserProfile.GetUserProfile(cuserName);
            var cuser = Membership.GetUser(cuserName);
            e.Email = cuser.Email;
            e.FirstName = cprofile.FirstName;
            e.LastName = cprofile.LastName;
            e.Title = cprofile.Title;
            e.Organization = cprofile.Organization;
            ViewData["MyAccountModel"] = e;
            TempData["acct_message"] = UserFeedbackMessages.UserUpdateFail;
            return View("MyAccount");
        }

        /// <summary>
        /// The SendResetUrl view
        /// </summary>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        public ActionResult SendResetUrl()
        {
            return View();
        }

        /// <summary>
        /// Sends a URL to the user to click to reset his password. 
        /// The email if not sent if:
        /// 1] No user has the given Model.UserEmail.
        /// 2] The user does not have a record in the UserAccountInfoRepository.
        /// 3] The user does not have a profile.
        /// In each of these scenarios, an exception is thrown, then caught and logged.
        /// No matter what the user is redirected to LogOn page.
        /// </summary>
        /// <param name="model">A SendRestUrlModel; contains only one field: UserEmail</param>
        /// <returns>A LogOn View.</returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SendResetUrl(SendResetUrlModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure this is actually a user.
                    string userName = Membership.GetUserNameByEmail(model.UserEmail);
                    var user = Membership.GetUser(userName);
                    var userId = new Guid(user.ProviderUserKey.ToString());

                    // Update user account
                    UserAccountInfo account = _userAccountInfoRepository.SearchForFirstOrDefault(x => x.UserId.Equals(userId));
                    if (account == null)
                    {
                        _userAccountInfoRepository.CreateUserAccountInfo(userId);
                        account = _userAccountInfoRepository.SearchForFirstOrDefault(x => x.UserId.Equals(userId));
                    }

                    account.GenerateTokens();
                    _userAccountInfoRepository.Update(account);

                    // Update profile
                    var profile = UserProfile.GetUserProfile(userName);
                    profile.ResetFlag = "true";
                    profile.Save();

                    // Send email
                    SendPasswordResetEmail(user.UserName, account.ResetToken);
                }
                catch (Exception exception)
                {
                    _logger.Info(string.Format("ERROR c: AccountController, a: SendResetUrl, p: {0}, t: {1}, e: {2}", model.UserEmail, DateTime.Now.ToShortDateString(), exception.Message));
                }
                finally
                {
                    TempData["message"] = UserFeedbackMessages.ConfirmPasswordResetRequest;
                }

                return RedirectToAction("LogOn", "Account", string.Empty);
            }

            // If we got this far, something failed, redisplay form
            TempData["message"] = UserFeedbackMessages.SomethingWentWrong;
            return View(model);
        }

        /// <summary>
        /// Sends new confirmation email
        /// </summary>
        /// <param name="userName">
        /// Name of the user. 
        /// </param>
        /// <param name="user">
        /// The IPrincipal user 
        /// </param>/// 
        /// <returns>
        /// Users View 
        /// </returns>
        [Authorize]
        public ActionResult ResendVerify(string userName, IPrincipal user)
        {
            if (user.IsInRole(Constants.SiteAdministratorRole))
            {
                MembershipUser thisUser = Membership.GetUser(user.Identity.Name);
                var userId = new Guid(thisUser.ProviderUserKey.ToString());
                var account = _userAccountInfoRepository.SearchForFirstOrDefault(x => x.UserId.Equals(userId));
                account.GenerateVerifyToken();
                _userAccountInfoRepository.Update(account);
                account = _userAccountInfoRepository.SearchForFirstOrDefault(x => x.UserId.Equals(userId));
                MembershipUser recipient = Membership.GetUser(userName);
                SendAccountVerificationEmail(recipient.Email, account.VerifyToken);
                return RedirectToAction("Users", "Account");
            }
            return RedirectToAction("MyAccount", "Account");
        }

        /// <summary>
        /// Deletes the User
        /// </summary>
        /// <param name="userName">
        /// Name of the user. 
        /// </param>
        /// <param name="user">
        /// The IPrincipal user 
        /// </param>/// 
        /// <returns>
        /// Users View 
        /// </returns>
        [Authorize]
        public ActionResult Delete(string userName, IPrincipal user)
        {
            if (user.IsInRole(Constants.SiteAdministratorRole))
            {
                var accountManager = new AccountManager(_logger, _userAccountInfoRepository, _stateAssignmentRepository, _passwordHistoryRepository);
                var status = accountManager.DeleteUserAccount(userName, User.Identity.Name);
                if (AccountManager.AccountTransactionWasSuccessful(status))
                {
                    TempData["message"] = UserFeedbackMessages.UserDeleteSuccess;
                }
                else
                {
                    TempData["message"] = UserFeedbackMessages.UserDeleteFail;
                }
                return RedirectToAction("Users", "Account");
            }
            return RedirectToAction("MyAccount", "Account");
        }

        /// <summary>
        ///   Displays Users
        /// </summary>
        /// <returns> Users View </returns>
        [Authorize]
        [RoleFilter(Action = "Index", Controller = "Home", RolesWithAccess = new[] { Constants.SiteAdministratorRole, Constants.ProjectAdminRole, Constants.StateAdminRole }, Message = "Only Administrators have access to this section.")]
        public ActionResult Users(IPrincipal user)
        {
            if (user.IsInRole(Constants.StateAdminRole))
            {
                var userProfile = UserProfile.GetUserProfile(user.Identity.Name);
                var model = GetUsersForView(userProfile.UserStatesCsv);
                return View(model);
            }
            else
            {
                var model = GetUsersForView(null);
                return View(model);
            }
        }

        /// <summary>
        /// The users.
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <returns>
        /// The System.Web.Mvc.ActionResult.
        /// </returns>
        [Authorize]
        [HttpPost]
        public ActionResult Users(FormCollection collection, IPrincipal user)
        {
            if (user.IsInRole(Constants.StateAdminRole))
            {
                var userProfile = UserProfile.GetUserProfile(user.Identity.Name);
                var model = GetUsersForView(userProfile.UserStatesCsv);
                return View(model);
            }
            else
            {
                var model = GetUsersForView(null);
                return View(model);
            }
        }

        /// <summary>
        /// The Verify action
        /// </summary>
        /// <param name="token">
        /// The token to verify
        /// </param>
        /// <returns>
        /// The change password or logon view.
        /// </returns>
        public ActionResult Verify(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                TempData["message"] = UserFeedbackMessages.FaultyPasswordResetToken;
                return RedirectToAction("LogOn");
            }
            string realId = string.IsNullOrEmpty(token) ? string.Empty : Uri.UnescapeDataString(token);

            //UserAccountInfo account = this.userAccountInfoRepository.GetUserAccountInfoByUserName(username);
            var account = _userAccountInfoRepository.SearchForFirstOrDefault(x => x.VerifyToken.ToString().Equals(realId));

            MembershipUser user = Membership.GetUser(account.UserId);
            var profile = UserProfile.GetUserProfile(user.UserName);

            if (!user.IsApproved && account.VerifyFlag && user.UserName.Length > 0)
            {
                user.IsApproved = true;
                Membership.UpdateUser(user);
                var model = new AccountMembershipService();
                FormsService.SignIn(user.UserName, false);
                    
                account.ResetFlag = false;
                account.VerifyFlag = false;
                _userAccountInfoRepository.Update(account);
                profile.ResetFlag = "false";
                profile.Save();
                return RedirectToAction("ChangePassword");
            }
            FormsService.SignOut();
            TempData["message"] = UserFeedbackMessages.AlreadyConfirmed;
            return RedirectToAction("LogOn");
        }

        /// <summary>
        /// A form where a new user enters the password she wants to use.
        /// </summary>
        /// <param name="token">A token sent to the user via email to identify them.</param>
        /// <returns>A view showing a form to enter a new password.</returns>
        public ActionResult VerifyNewUserAccount(string token)
        {
            return View();
        }

        /// <summary>
        /// Verifies the new user account.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="model">The model.</param>
        /// <returns>The Log On view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyNewUserAccount(string token, ResetPasswordModel model)
        {
            // Validate token.
            string realId = string.IsNullOrEmpty(token) ? string.Empty : Uri.UnescapeDataString(token);
            var account = _userAccountInfoRepository.SearchForFirstOrDefault(x => x.VerifyToken.ToString().Equals(realId));
            if (account == null)
            {
                TempData["message"] = UserFeedbackMessages.FaultyPasswordResetToken;
                return RedirectToAction("LogOn", new
                {
                    returnUrl = ""
                });
            }

            // Validate data. We don't want to rely solely on JavaScript.
            if (!ModelState.IsValid)
            {
                TempData["message"] = UserFeedbackMessages.FaultyClientData;
                return View(model);
            }

            // Ensure the user exists.
            MembershipUser user = Membership.GetUser(account.UserId);
            if (user == null)
            {
                TempData["message"] = UserFeedbackMessages.SomethingWentWrong;
                return RedirectToAction("LogOn", new
                {
                    returnUrl = ""
                });
            }

            // Unlock user and set new password 
            var tempPassword = user.ResetPassword();
            user.ChangePassword(tempPassword, model.NewPassword);
            Membership.Provider.UpdateUser(user);
            var userId = new Guid(user.ProviderUserKey.ToString());

            // Update user account.
            account.GenerateTokens();
            account.ResetFlag = false;
            account.VerifyFlag = false;
            _userAccountInfoRepository.Update(account);

            // Update history.
            _passwordHistoryRepository.UpdatePasswordHistory(userId, model.NewPassword, account.Salt);

            // Send confirmation email.
            SendConfirmationPasswordReset(user.UserName);
            TempData["message"] = UserFeedbackMessages.SuccessNewAccountVerification;

            return RedirectToAction("LogOn", new
            {
                returnUrl = ""
            });
        }

        /// <summary>
        /// A view for allowing users to reset their password when they've forgotten it.
        /// </summary>
        /// <param name="token">A token that has an expiration time.</param>
        /// <returns>A view where user can enter new password.</returns>
        public ActionResult ResetPassword(string token)
        {
            return View();
        }

        /// <summary>
        /// Resets the user password with the one submitted by the user. 
        /// </summary>
        /// <param name="token">The token sent to the user via email.</param>
        /// <param name="model">The model containing the password and confirmation password.</param>
        /// <returns>A view indicating whether the reset was successful or not.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string token, ResetPasswordModel model)
        {
            //// Validate token.
            string realId = string.IsNullOrEmpty(token) ? string.Empty : Uri.UnescapeDataString(token);
            var account = _userAccountInfoRepository.SearchForFirstOrDefault(x => x.ResetToken.ToString().Equals(realId));
            if (account == null)
            {
                TempData["message"] = UserFeedbackMessages.PasswordResetNotValid;
                return View("LogOn");
            }

            //// Validate data. We don't want to rely solely on JavaScript.
            if (!ModelState.IsValid)
            {
                TempData["message"] = UserFeedbackMessages.PasswordResetFail;
                return View(model);
            }

            //// Ensure user and profile exist.
            MembershipUser user = Membership.GetUser(account.UserId);
            UserProfile profile = null;
            var resetFlag = string.Empty;
            if (user != null)
            {
                profile = UserProfile.GetUserProfile(user.UserName);
                if (profile != null)
                {
                    resetFlag = profile.ResetFlag;
                }
            }

            //// Validate password AND request to reset
            Tuple<bool, string> validation = ValidateResetRequestAndPassword(user, account, model.NewPassword, resetFlag);
            if (validation.Item1)
            {
                //// Reset password
                user.UnlockUser();
                var tempPassword = user.ResetPassword();
                user.ChangePassword(tempPassword, model.NewPassword);

                //// Update profile an log in user.
                profile.ResetFlag = "false";
                profile.Save();

                //// Update user account.
                account.GenerateResetToken();
                account.ResetFlag = false;
                _userAccountInfoRepository.Update(account);

                //// update history.
                _passwordHistoryRepository.UpdatePasswordHistory(account.UserId, model.NewPassword, account.Salt);

                SendConfirmationPasswordReset(user.UserName);
                TempData["message"] = validation.Item2;
            }
            else
            {
                TempData["message"] = validation.Item2;
                return View(model);
            }

            return RedirectToAction("LogOn", new
            {
                returnUrl = ""
            });
        }

        /// <summary>
        /// Controls change password actions.
        /// </summary>
        /// <param name="model">The view model from ChangePassword</param>
        /// <param name="userName">THe username to change password for.</param>
        /// <param name="actionToRedirectTo">Where to redirect the user to.</param>
        /// <returns>redirects to ChangePasswordSuccess or failure</returns>
        [ValidateAntiForgeryToken]
        public ActionResult HandleChangePassword(ChangePasswordModel model, string userName, string actionToRedirectTo = "Home")
        {
            //// Are the entered credentials legit?
            if (MembershipService.ValidateUser(userName, model.OldPassword))
            {
                MembershipUser user = Membership.GetUser(userName);
                var userId = new Guid(user.ProviderUserKey.ToString());
                var account = _userAccountInfoRepository.SearchForFirstOrDefault(x => x.UserId.Equals(userId));

                //// Validate new password
                Tuple<bool, string> validation = ValidateChangePassword(user, account, model.OldPassword, model.Password);
                if (validation.Item1 && MembershipService.ChangePassword(user.UserName, model.OldPassword, model.Password))
                {
                    //// Update password history
                    _passwordHistoryRepository.UpdatePasswordHistory(account.UserId, model.Password, account.Salt);
                    TempData["message"] = UserFeedbackMessages.PasswordUpdateSuccess;
                    _logger.Info(user.Email + " Changed Password");
                    return RedirectToAction("Index", "Home");
                }
                TempData["message"] = validation.Item2;
                ModelState.AddModelError(string.Empty, validation.Item2);
                return ChangePassword(model);
            }

            TempData["message"] = UserFeedbackMessages.InvalidCredentials;
            return RedirectToAction("LogOn", new
            {
                returnUrl = ""
            });
        }

        /// <summary>
        /// Validates password reset request AND new password. Calls ValidateResetRequest.
        /// </summary>
        /// <param name="user">The user resetting password</param>
        /// <param name="account">The user account info of this account</param>
        /// <param name="newPassword">The new passowrd</param>
        /// <param name="resetFlag">The reset flag</param>
        /// <returns>pair consisting of (is reset password valid, string to display)</returns>
        [NonAction]
        public Tuple<bool, string> ValidateResetRequestAndPassword(MembershipUser user, UserAccountInfo account, string newPassword, string resetFlag)
        {
            var output = ValidateResetPassword(user, account, newPassword);
            if (!output.Item1)
            {
                return output;
            }

            return ValidateResetRequest(account, resetFlag);
        }

        /// <summary>
        /// Validates password reset request. Results used in view.
        /// </summary>
        /// <param name="account">The account resetting password</param>
        /// <param name="resetFlag">The reset flag.</param>
        /// <returns>pair consisting of (is reset password valid, string to display)</returns>
        [NonAction]
        public Tuple<bool, string> ValidateResetRequest(UserAccountInfo account, string resetFlag)
        {
            //// Reset password: if they requested and the token is valid and has not expired 
            if (resetFlag.Equals("true") && account.ResetFlag && !account.HasResetTokenExpired(GetExpirationResetLinkInMinutes()))
            {
                return new Tuple<bool, string>(true, UserFeedbackMessages.PasswordUpdateSuccess);
            }
            return new Tuple<bool, string>(false, UserFeedbackMessages.ExpiredPasswordResetUrl);
        }

        /// <summary>
        /// Validates user password on login, to check if it needs to be reset. Results used in view.
        /// </summary>
        /// <param name="user">The user logging in</param>
        /// <param name="model">The logOn model</param>
        /// <param name="messageIfPwdExpired">The message to return if password is expired.</param>
        /// <returns>pair consisting of (is password valid, string to display)</returns>
        [NonAction]
        public Tuple<bool, string> ValidateLogOnPassword(MembershipUser user, LogOnModel model, string messageIfPwdExpired)
        {
            if (user.IsLockedOut)
            {
                return new Tuple<bool, string>(false, UserFeedbackMessages.LockedAccount);
            }

            var maxPwdLife = GetMaxPasswordLifeInDays();
            var passwordAgeInDays = DateTime.Now.Subtract(user.LastPasswordChangedDate).TotalDays;

            if (MembershipService.ValidateUser(model.UserName, model.Password))
            {
                //// Password is about to expire.
                if ((maxPwdLife - 5) < passwordAgeInDays && passwordAgeInDays <= maxPwdLife)
                {
                    return new Tuple<bool, string>(true, string.Format(UserFeedbackMessages.PasswordAboutToExpire, Convert.ToInt32(maxPwdLife - passwordAgeInDays)));
                }

                //// Password expired.
                if (passwordAgeInDays > maxPwdLife)
                {
                    return new Tuple<bool, string>(false, messageIfPwdExpired);
                }

                //// Everything checks out.
                return new Tuple<bool, string>(true, null);
            }
            return new Tuple<bool, string>(false, UserFeedbackMessages.InvalidCredentials);
        }

        /// <summary>
        /// Validates a change password request.
        /// </summary>
        /// <param name="user">The user changing their password</param>
        /// <param name="account">The user account info for this user</param>
        /// <param name="oldPassword">The current password</param>
        /// <param name="newPassword">The intended password</param>
        /// <returns>pair consisting of (is change password valid, string to display)</returns>
        [NonAction]
        public Tuple<bool, string> ValidateChangePassword(MembershipUser user, UserAccountInfo account, string oldPassword, string newPassword)
        {
            var hasValidNewPassword = ValidateResetPassword(user, account, newPassword);
            if (hasValidNewPassword.Item1)
            {
                //// Ensure new password is sufficiently different from old one.
                if (PercentageDifference(newPassword, oldPassword) > 0.2)
                {
                    return hasValidNewPassword;
                }

                return new Tuple<bool, string>(false, "The new password is too similar to the old password.");
            }

            return hasValidNewPassword;
        }

        /// <summary>
        /// Validates the password reset request.
        /// </summary>
        /// <param name="user">The user resetting their password</param>
        /// <param name="account">The user account info for this user</param>
        /// <param name="newPassword">The new password to test</param>
        /// <returns>pair consisting of (is reset password valid, string to display)</returns>
        [NonAction]
        public Tuple<bool, string> ValidateResetPassword(MembershipUser user, UserAccountInfo account, string newPassword)
        {
            //// Ensure the password has not been used in the past.
            if (!_passwordHistoryRepository.CanUsePassword(account.UserId, newPassword, account.Salt))
            {
                return new Tuple<bool, string>(false, "You have already used that password in the recent past. Please pick another.");
            }

            //// Password must exist for at least one day.
            var hasValidPasswordLife = DateTime.Now.Subtract(user.LastPasswordChangedDate).TotalDays > GetMinPasswordLifeInDays();
            if (!hasValidPasswordLife)
            {
                return new Tuple<bool, string>(false, "Passwords may only be reset once a day.");
            }

            return new Tuple<bool, string>(true, "Password passed additional validation checks.");
        }

        /// <summary>
        /// Gets the lifetime of the password reset link from web.config.
        /// </summary>
        /// <returns>password reset link lifetime in minutes</returns>
        [NonAction]
        public double GetExpirationResetLinkInMinutes()
        {
            return Convert.ToDouble(ConfigurationManager.AppSettings["ExpirationResetLinkInMinutes"]);
        }

        /// <summary>
        /// Gets the min password lifetime in days from web.config.
        /// </summary>
        /// <returns>min password lifetime in days</returns>
        [NonAction]
        public double GetMinPasswordLifeInDays()
        {
            return Convert.ToDouble(ConfigurationManager.AppSettings["MinPasswordLifeInDays"]);
        }

        /// <summary>
        /// Gets the max password lifetime in days from web.config.
        /// </summary>
        /// <returns>max password lifetime in days</returns>
        [NonAction]
        public double GetMaxPasswordLifeInDays()
        {
            return Convert.ToDouble(ConfigurationManager.AppSettings["MaxPasswordLifeInDays"]);
        }

        /// <summary>
        /// Calculates the percentage difference between string 'first' and string 'second'.
        /// </summary>
        /// <param name="first">The first string to compare</param>
        /// <param name="second">The string to compare to</param>
        /// <returns>The percent difference between two strings</returns>
        [NonAction]
        public static double PercentageDifference(string first, string second)
        {
            return (LevenshteinDistance(first, second) + LevenshteinDistance(first.ToLower(), second.ToLower())) / (2 * Math.Max(first.Length, second.Length));
        }

        /// <summary>
        /// Calculates the number of changes that need to be made to 'first' so that it becomes 'second'.
        /// </summary>
        /// <param name="first">The first string to compare</param>
        /// <param name="second">The string to compare to</param>
        /// <returns>double Levenshtein distance</returns>
        [NonAction]
        public static double LevenshteinDistance(string first, string second)
        {
            int n = first.Length;
            int m = second.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                // Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (second[j - 1] == first[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            // Step 7
            return d[n, m];
        }

        /// <summary>
        /// Assigns the protocol to URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        [NonAction]
        public static string AssignProtocolToUrl(string url)
        {
            var useHTTPS = Convert.ToBoolean(ConfigurationManager.AppSettings["UseHTTPSInAccountEmails"]);
            var http = url.Substring(0, 5).ToLower();
            if (!http.Equals("https") && useHTTPS)
            {
                url = url.Replace("http", "https");
            }

            return url;
        }

        /// <summary>
        /// Sends an email to the user to confirm that the correct email address was entered.
        /// </summary>
        /// <param name="userEmail">The user's email address.</param>
        /// <param name="token">A token to identify the user.  Included in a URL the user has to click.</param>
        [NonAction]
        public void SendAccountVerificationEmail(string userEmail, string token)
        {
            string verifyUrl = AssignProtocolToUrl(Request.Url.GetLeftPart(UriPartial.Authority) + ConfigurationManager.AppSettings["environment"] + "/Account/VerifyNewUserAccount?token=" + Uri.EscapeDataString(token));

            var message = new MailMessage
            {
                Subject = UserFeedbackMessages.SubjectNewAccount,
                Body = string.Format(UserFeedbackMessages.BodyNewAccount, userEmail, verifyUrl)
            };

            message.To.Add(userEmail);
            SendEmail(message);
        }

        /// <summary>
        /// Sends and email with a link the user can click to reset their password.
        /// </summary>
        /// <param name="userName">The name of the user we are contacting.</param>
        /// <param name="token">A token to identify the user passed on the URL.</param>
        [NonAction]
        public void SendPasswordResetEmail(string userName, string token)
        {
            MembershipUser user = Membership.GetUser(userName);

            // prepare url; the paramerter is a guid
            string verifyUrl = Request.Url.GetLeftPart(UriPartial.Authority) + ConfigurationManager.AppSettings["environment"] + "/Account/ResetPassword?token=" + Uri.EscapeDataString(token);
            string expiration = Convert.ToInt32(GetExpirationResetLinkInMinutes()/60).ToString();
            

            // send email
            var message = new MailMessage
            {
                Subject = UserFeedbackMessages.SubjectPasswordResetRequest,
                Body = string.Format(UserFeedbackMessages.BodyPasswordResetRequest, user.Email, expiration, verifyUrl)
            };
            message.To.Add(user.Email);

            SendEmail(message);
        }


        /// <summary>
        /// Sends and email which confirms that the user reset their password.
        /// </summary>
        /// <param name="userName">The name of the user we are contacting.</param>
        [NonAction]
        public void SendConfirmationPasswordReset(string userName)
        {
            MembershipUser user = Membership.GetUser(userName);

            // send email
            var message = new MailMessage
            {
                Subject = UserFeedbackMessages.SubjectConfirmPasswordReset,
                Body = string.Format(UserFeedbackMessages.BodyConfirmPasswordReset, user.Email)
            };
            message.To.Add(user.Email);

            SendEmail(message);
        }

        /// <summary>
        /// Send emails to users.
        /// </summary>
        /// <param name="message">The messsage to be sent.</param>
        [NonAction]
        public void SendEmail(MailMessage message)
        {
            var section = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
            var client = new SmtpClient();
            if (ConfigurationManager.AppSettings["RServerURL"] == "")
            {
                client.EnableSsl = true;
            }
            message.From = new MailAddress(section.From);
            client.Send(message);
        }

        /// <summary>
        /// Initializes data that might not be available when the constructor is called.
        /// </summary>
        /// <param name="requestContext">
        /// The HTTP context and route data. 
        /// </param>
        protected override void Initialize(RequestContext requestContext)
        {
            if (MembershipService == null)
            {
                MembershipService = new AccountMembershipService();
            }

            if (FormsService == null)
            {
                FormsService = new FormsAuthenticationService();
            }

            base.Initialize(requestContext);
        }

        /// <summary>
        /// Assigns States to user
        /// </summary>
        /// <param name="profile">
        /// The UserProfile profile
        /// </param>
        /// <param name="radio">
        /// int radio, selected radio button
        /// </param>
        /// <param name="checkbox">
        /// The lis tof checked checkboxes
        /// </param>
        private void AssignStates(UserProfile profile, int radio, List<StateSelection> checkbox)
        {
            if (profile.AccountType.Equals(Constants.MultipleStateUserRole))
            {
                foreach (StateSelection state in checkbox)
                {
                    if (state.IsChecked)
                    {
                        _stateAssignmentRepository.SetStateAssignment(
                            new StateAssignment
                            {
                                UserName = profile.UserName,
                                StateId = state.StateId,
                                Role = profile.AccountType
                            });
                    }
                }
            }
            else if (profile.AccountType.Equals(Constants.StateAdminRole) || profile.AccountType.Equals(Constants.StateUserRole))
            {
                _stateAssignmentRepository.SetStateAssignment(
                             new StateAssignment
                             {
                                 UserName = profile.UserName,
                                 StateId = radio,
                                 Role = profile.AccountType
                             });
            }

            _stateAssignmentRepository.SubmitChanges();
        }

        /// <summary>
        /// Gets all users and profile information for Users views.
        /// </summary>
        /// <returns>
        /// IEnuberable UserViewModel
        /// </returns>
        private IEnumerable<UserViewModel> GetUsersForView(string state)
        {
            if (state == null)
            {
                var model = Membership.GetAllUsers().Cast<MembershipUser>().Select(
                       x =>
                       {
                           var userProfile = UserProfile.GetUserProfile(x.UserName);
                           return
                           new UserViewModel
                           {
                               UserName = x.UserName,
                               Email = x.Email,
                               FirstName = userProfile.FirstName,
                               LastName = userProfile.LastName,
                               Organization = userProfile.Organization,
                               UserRolesCsv =
                                   string.Join(",", Roles.GetRolesForUser(x.UserName).Select(y => y.ToString()).ToArray()),
                               UserStatesCsv = userProfile.UserStatesCsv,
                               AccountType = userProfile.AccountType
                           };
                       });
                return model;
            }
            else
            {
                var model = Membership.GetAllUsers().Cast<MembershipUser>().Select(
                       x =>
                       {
                           var userProfile = UserProfile.GetUserProfile(x.UserName);
                           return
                           new UserViewModel
                           {
                               UserName = x.UserName,
                               Email = x.Email,
                               FirstName = userProfile.FirstName,
                               LastName = userProfile.LastName,
                               Organization = userProfile.Organization,
                               UserRolesCsv =
                                   string.Join(",", Roles.GetRolesForUser(x.UserName).Select(y => y.ToString()).ToArray()),
                               UserStatesCsv = userProfile.UserStatesCsv,
                               AccountType = userProfile.AccountType
                           };
                       }).Where(x => x.UserStatesCsv.Contains(state));
                return model;
            }
            
        }

        /// <summary>
        /// Grabs SVG from database, returns it as a file to the view.
        /// </summary>
        /// <param name="jobGUID">the jobGUID</param>
        /// <param name="rank">rank order of outcomes</param>
        /// <param name="type">ss for scale score or pp for percent proficient</param>
        /// [NonAction]
        public static string UserName(string userName)
        {
            string fullName = "";
            UserProfile profile = UserProfile.GetUserProfile(userName);
            fullName = profile.GetUserFullName();

            if (fullName != null)
            {
                return fullName;
            }
            return "";
        }
    }
}
