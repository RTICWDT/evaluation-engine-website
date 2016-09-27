//-----------------------------------------------------------------------
// <copyright file="AccountModels.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// The account models.
// </copyright>
//-----------------------------------------------------------------------
namespace EvalEngine.UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Web.Mvc;
    using System.Web.Security;

    /// <summary>
    /// The FormsAuthentication type is sealed and contains static members, so it is difficult to
    /// unit test code that calls its members. The interface and helper class below demonstrate
    /// how to create an abstract wrapper around such a type in order to make the AccountController
    /// code unit testable.
    /// </summary>
    public interface IFormsAuthenticationService
    {
        /// <summary>
        /// Signs user in.
        /// </summary>
        /// <param name="userName">User to sign in</param>
        /// <param name="createPersistentCookie">bool, set cookie?</param>
        void SignIn(string userName, bool createPersistentCookie);

        /// <summary>
        /// Signs user out.
        /// </summary>
        void SignOut();
    }

    /// <summary>
    ///   Membership Interface
    /// </summary>
    public interface IMembershipService
    {
        /// <summary>
        ///   Gets the length of the min password.
        /// </summary>
        /// <value> The length of the min password. </value>
        int MinPasswordLength { get; }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="userName">
        /// Name of the user. 
        /// </param>
        /// <param name="oldPassword">
        /// The old password. 
        /// </param>
        /// <param name="newPassword">
        /// The new password. 
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        bool ChangePassword(string userName, string oldPassword, string newPassword);

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="userName">
        /// Name of the user. 
        /// </param>
        /// <param name="password">
        /// The password. 
        /// </param>
        /// <param name="email">
        /// The email. 
        /// </param>
        /// <returns>
        /// The System.Web.Security.MembershipCreateStatus.
        /// </returns>
        MembershipCreateStatus CreateUser(string userName, string password, string email);

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="userName">
        /// Name of the user. 
        /// </param>
        /// <param name="online">
        /// Is user online? 
        /// </param>
        /// <returns>
        /// The System.Web.Security.MembershipCreateStatus.
        /// </returns>
        MembershipUser GetUser(string userName, bool online);

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="userName">
        /// Name of the user. 
        /// </param>
        /// <param name="password">
        /// The password. 
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        bool ValidateUser(string userName, string password);
    }

    /// <summary>
    ///   Account Validation Class
    /// </summary>
    public static class AccountValidation
    {
        /// <summary>
        /// Errors the code to string.
        /// </summary>
        /// <param name="createStatus">
        /// The create status. 
        /// </param>
        /// <returns>
        /// Error String 
        /// </returns>
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return
                        "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }

    /// <summary>
    /// Performs signin, signout for forms authentication service.
    /// </summary>
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        /// <summary>
        /// SignIn method
        /// </summary>
        /// <param name="userName">Username to sign in</param>
        /// <param name="createPersistentCookie">bool, set cookie?</param>
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "userName");
            }

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        /// <summary>
        /// Signs user out via FormsAuthentication.
        /// </summary>
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    /// <summary>
    ///   Membership Service
    /// </summary>
    public class AccountMembershipService : IMembershipService
    {
        /// <summary>
        ///   Membership Porvider
        /// </summary>
        private readonly MembershipProvider provider;

        /// <summary>
        ///   Initializes a new instance of the <see cref="AccountMembershipService" /> class.
        /// </summary>
        public AccountMembershipService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountMembershipService"/> class.
        /// </summary>
        /// <param name="provider">
        /// The provider. 
        /// </param>
        public AccountMembershipService(MembershipProvider provider)
        {
            this.provider = provider ?? Membership.Provider;
        }

        /// <summary>
        ///   Gets the length of the min password.
        /// </summary>
        /// <value> The length of the min password. </value>
        public int MinPasswordLength
        {
            get
            {
                return this.provider.MinRequiredPasswordLength;
            }
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="userName">
        /// Name of the user. 
        /// </param>
        /// <param name="oldPassword">
        /// The old password. 
        /// </param>
        /// <param name="newPassword">
        /// The new password. 
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "userName");
            }

            if (string.IsNullOrEmpty(oldPassword))
            {
                throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentException("Value cannot be null or empty.", "newPassword");
            }

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = this.provider.GetUser(userName, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="userName">
        /// Name of the user. 
        /// </param>
        /// <param name="password">
        /// The password. 
        /// </param>
        /// <param name="email">
        /// The email. 
        /// </param>
        /// <returns>
        /// The System.Web.Security.MembershipCreateStatus.
        /// </returns>
        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "userName");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Value cannot be null or empty.", "password");
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Value cannot be null or empty.", "email");
            }

            MembershipCreateStatus status;
            this.provider.CreateUser(userName, password, email, null, null, false, null, out status);
            return status;
        }

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="userName">
        /// Name of the user. 
        /// </param>
        /// <param name="password">
        /// The password. 
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public bool ValidateUser(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "userName");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Value cannot be null or empty.", "password");
            }

            return this.provider.ValidateUser(userName, password);
        }

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="userName">
        /// Name of the user. 
        /// </param>
        /// <param name="online">Is user online?</param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public MembershipUser GetUser(string userName, bool online)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "userName");
            }

            return this.provider.GetUser(userName, online);
        }
    }

    /// <summary>
    /// The layout model, used for displaying username in template.
    /// </summary>
    public class LayoutModel
    {
        /// <summary>
        /// Gets or sets the name for template display.
        /// </summary>
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// The send reset url model.
    /// </summary>
    public class SendResetUrlModel
    {
        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        [Required]
        [Display(Name = "Email address")]
        public string UserEmail { get; set; }
    }

    /// <summary>
    /// The reset password model.
    /// </summary>
    [PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public class ResetPasswordModel
    {
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value> The password. </value>
        [ValidatePasswordLength]
        [RegularExpression(@"(?=^.{12,}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(.*[^0-9a-zA-Z].*)(?!.*\s).*$",
            ErrorMessage =
                "Password must contain at least 1 uppercase letter, at least 1 lowercase letter, at least 1 number, at least 1 special character, and be 12 characters in length.")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        /// <value> The confirm password. </value>
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// The change password model.
    /// </summary>
    [PropertiesMustMatch("Password", "ConfirmPassword",
    ErrorMessage = "The password and confirmation password do not match.")]
    public class ChangePasswordModel
    {
        /// <summary>
        ///   Gets or sets the password.
        /// </summary>
        /// <value> The password. </value>
        [ValidatePasswordLength]
        [RegularExpression(@"(?=^.{12,}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(.*[^0-9a-zA-Z].*)(?!.*\s).*$",
            ErrorMessage =
                "Password must contain at least 1 uppercase letter, at least 1 lowercase letter, at least 1 number, at least 1 sepcial character, and be 12 characters in length.")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        /// <summary>
        ///   Gets or sets the confirm password.
        /// </summary>
        /// <value> The confirm password. </value>
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the previous password.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }
    }

    /// <summary>
    /// The Change exipred password model.
    /// </summary>
    public class ChangeExpiredPasswordModel : ChangePasswordModel
    {
        /// <summary>
        /// Gets or sets the user name
        /// </summary>
        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }
    }

    /// <summary>
    /// The force change password model.
    /// </summary>
    public class ForceChangePasswordModel
    {
        /// <summary>
        ///   Gets or sets the password.
        /// </summary>
        /// <value> The password. </value>
        [ValidatePasswordLength]
        [RegularExpression(@"(?=^.{12,}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(.*[^0-9a-zA-Z].*)(?!.*\s).*$",
            ErrorMessage =
                "Password must contain at least 1 uppercase letter, at least 1 lowercase letter, at least 1 number, at least 1 sepcial character, and be 12 characters in length.")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        /// <summary>
        ///   Gets or sets the confirm password.
        /// </summary>
        /// <value> The confirm password. </value>
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// The logon model.
    /// </summary>
    public class LogOnModel
    {
        /// <summary>
        ///   Gets or sets the email.
        /// </summary>
        /// <value> The email. </value>
        [DisplayName("Email")]
        [Required]
        public string Email { get; set; }

        /// <summary>
        ///   Gets or sets the password.
        /// </summary>
        /// <value> The password. </value>
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether [remember me].
        /// </summary>
        /// <value> <c>true</c> if [remember me]; otherwise, <c>false</c> . </value>
        [DisplayName("Remember me?")]
        public bool RememberMe { get; set; }

        /// <summary>
        ///   Gets or sets the name of the user.
        /// </summary>
        /// <value> The name of the user. </value>
        [DisplayName("User name")]
        public string UserName { get; set; }
    }

    /// <summary>
    /// The register model.
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email address
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets user password
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets Confirm password
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    ///   User Add Model
    /// </summary>
    public class UserAddModel
    {
        /// <summary>
        ///   Gets or sets the account type Id.
        /// </summary>
        /// <value> The account type Id. </value>
        [Display(Name = "Account Type")]
        public string AccountType { get; set; }

        /// <summary>
        ///   Gets or sets the email.
        /// </summary>
        /// <value> The email. </value>
        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email address")]
        public string Email { get; set; }

        /// <summary>
        ///   Gets or sets the first name.
        /// </summary>
        /// <value> The first name. </value>
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        /// <summary>
        ///   Gets or sets the organziation.
        /// </summary>
        /// <value> The organization. </value>
        [Required]
        [Display(Name = "Organization")]
        public string Organization { get; set; }

        /// <summary>
        ///   Gets or sets the last name.
        /// </summary>
        /// <value> The last name. </value>
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        /// <summary>
        ///   Gets or sets the title.
        /// </summary>
        /// <value> The title. </value>
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        ///   Gets or sets the name of the user.
        /// </summary>
        /// <value> The name of the user. </value>
        [DisplayName("User name")]
        public string UserName { get; set; }

        /// <summary>
        ///   Gets or sets the user roles CSV.
        /// </summary>
        /// <value> The user roles CSV. </value>
        [DataType(DataType.Text)]
        [DisplayName("UserRoles")]
        public string UserRolesCsv { get; set; }

        /// <summary>
        ///   Gets or sets the account type options.
        /// </summary>
        /// <value> The account type options. </value>
        public List<SelectListItem> AccountTypeOptions { get; set; }

        /// <summary>
        ///   Gets or sets the role selections.
        /// </summary>
        /// <value> The role selections. </value>
        [Display(Name = "Role")]
        [Required]
        public List<RoleSelection> RoleSelections { get; set; }

        /// <summary>
        ///   Gets or sets the state selections from radio buttons in Add form.
        /// </summary>
        /// <value> The state selections. </value>
        [Display(Name = "State Assignment")]
        public List<StateSelection> RadioStateSelections { get; set; }

        /// <summary>
        ///   Gets or sets the state selections from checkboxes in Add form.
        /// </summary>
        /// <value> The state selections. </value>
        [Display(Name = "State Assignment")]
        public List<StateSelection> CheckboxStateSelections { get; set; }

        /// <summary>
        ///   Gets or sets the state selections from radio buttons in Add form.
        /// </summary>
        /// <value> The state selections. </value>
        [Display(Name = "Radio State Assignment, Single")]
        public int RadioSelectedState { get; set; }
    }

    /// <summary>
    ///   User View Model
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        ///   Gets or sets the account type.
        /// </summary>
        /// <value> The account type. </value>
        [Display(Name = "Account Type")]
        public string AccountType { get; set; }

        /// <summary>
        ///   Gets or sets the email.
        /// </summary>
        /// <value> The email. </value>
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email address")]
        public string Email { get; set; }

        /// <summary>
        ///   Gets or sets the first name.
        /// </summary>
        /// <value> The first name. </value>
        [DisplayName("First Name")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        /// <summary>
        ///   Gets or sets the organziation.
        /// </summary>
        /// <value> The organization. </value>
        [Display(Name = "Organization")]
        public string Organization { get; set; }

        /// <summary>
        ///   Gets or sets the last name.
        /// </summary>
        /// <value> The last name. </value>
        [DisplayName("Last Name")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        /// <summary>
        ///   Gets or sets the middle initial.
        /// </summary>
        /// <value> The middle initial. </value>
        [DataType(DataType.Text)]
        [Display(Name = "Middle Initial")]
        public string MiddleInitial { get; set; }

        /// <summary>
        ///   Gets or sets the title.
        /// </summary>
        /// <value> The title. </value>
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        ///   Gets or sets the name of the user.
        /// </summary>
        /// <value> The name of the user. </value>
        [DisplayName("User name")]
        public string UserName { get; set; }

        /// <summary>
        ///   Gets or sets the user roles CSV.
        /// </summary>
        /// <value> The user roles CSV. </value>
        [DataType(DataType.Text)]
        [DisplayName("UserRoles")]
        public string UserRolesCsv { get; set; }

        /// <summary>
        ///   Gets or sets the user state CSV.
        /// </summary>
        /// <value> The user state CSV. </value>
        [DataType(DataType.Text)]
        [DisplayName("UserStates")]
        public string UserStatesCsv { get; set; }
    }

    /// <summary>
    ///   MyAccount model
    /// </summary>
    public class MyAccountModel
    {
        /// <summary>
        ///   Gets or sets the email.
        /// </summary>
        /// <value> The email. </value>
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email address")]
        public string Email { get; set; }

        /// <summary>
        ///   Gets or sets the first name.
        /// </summary>
        /// <value> The first name. </value>
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        /// <summary>
        ///   Gets or sets the organization.
        /// </summary>
        /// <value> The organization. </value>
        [Required]
        [Display(Name = "Organization")]
        public string Organization { get; set; }

        /// <summary>
        ///   Gets or sets the last name.
        /// </summary>
        /// <value> The last name. </value>
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        /// <summary>
        ///   Gets or sets the middle initial.
        /// </summary>
        /// <value> The middle initial. </value>
        [Display(Name = "Middle Initial")]
        public string MiddleInitial { get; set; }

        /// <summary>
        ///   Gets or sets the title.
        /// </summary>
        /// <value> The title. </value>
        [Display(Name = "Title")]
        public string Title { get; set; }
    }

    /// <summary>
    ///   UserEdit model
    /// </summary>
    public class UserEditModel
    {
        /// <summary>
        ///   Gets or sets a value indicating whether the activity status is true or false.
        /// </summary>
        /// <value> The activity status </value>
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        /// <summary>Gets or sets a value indicating whether the lock status is set or not.</summary>
        /// <value> The lock status </value>
        [Display(Name = "Locked?")]
        public bool IsLocked { get; set; }

        /// <summary>
        ///   Gets or sets the account type Id.
        /// </summary>
        /// <value> The account type Id. </value>
        [Display(Name = "Account Type")]
        public string AccountType { get; set; }

        /// <summary>
        ///   Gets or sets the account type options.
        /// </summary>
        /// <value> The account type options. </value>
        public List<SelectListItem> AccountTypeOptions { get; set; }

        /// <summary>
        ///   Gets or sets the email.
        /// </summary>
        /// <value> The email. </value>
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email address")]
        public string Email { get; set; }

        /// <summary>
        ///   Gets or sets the first name.
        /// </summary>
        /// <value> The first name. </value>
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        /// <summary>
        ///   Gets or sets the organization.
        /// </summary>
        /// <value> The organization. </value>
        [Required]
        [Display(Name = "Organization")]
        public string Organization { get; set; }

        /// <summary>
        ///   Gets or sets the last name.
        /// </summary>
        /// <value> The last name. </value>
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        /// <summary>
        ///   Gets or sets the middle initial.
        /// </summary>
        /// <value> The middle initial. </value>
        [Display(Name = "Middle Initial")]
        public string MiddleInitial { get; set; }

        /// <summary>
        ///   Gets or sets the title.
        /// </summary>
        /// <value> The title. </value>
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        ///   Gets or sets the name of the user.
        /// </summary>
        /// <value> The name of the user. </value>
        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Username")]
        public string UserName { get; set; }

        /// <summary>
        ///   Gets or sets the role selections.
        /// </summary>
        /// <value> The role selections. </value>
        [Display(Name = "Role")]
        public List<RoleSelection> RoleSelections { get; set; }

        /// <summary>
        ///   Gets or sets the state selections from radio buttons in edit form.
        /// </summary>
        /// <value> The state selections. </value>
        [Display(Name = "Radio State Assignment")]
        public List<StateSelection> RadioStateSelections { get; set; }

        /// <summary>
        ///   Gets or sets the state selections from checkboxes in edit form.
        /// </summary>
        /// <value> The state selections. </value>
        [Display(Name = "Checkbox State Assignment")]
        public List<StateSelection> CheckboxStateSelections { get; set; }

        /// <summary>
        ///   Gets or sets the state selections from radio buttons in edit form.
        /// </summary>
        /// <value> The state selections. </value>
        [Display(Name = "Radio State Assignment, Single")]
        public int RadioSelectedState { get; set; } 
    }

    /// <summary>
    ///   Role Selection Class
    /// </summary>
    public class RoleSelection
    {
        /// <summary>
        ///   Gets or sets a value indicating whether this instance is checked.
        /// </summary>
        /// <value> <c>true</c> if this instance is checked; otherwise, <c>false</c> . </value>
        public bool IsChecked { get; set; }

        /// <summary>
        ///   Gets or sets the name of the role.
        /// </summary>
        /// <value> The name of the role. </value>
        public string RoleName { get; set; }
    }

    /// <summary>
    ///   StateSelection Class
    /// </summary>
    public class StateSelection
    {
        /// <summary>
        ///   Gets or sets a value indicating whether this instance is checked.
        /// </summary>
        /// <value> <c>true</c> if this instance is checked; otherwise, <c>false</c> . </value>
        public bool IsChecked { get; set; }

        /// <summary>
        ///   Gets or sets the state value.
        /// </summary>
        /// <value> The state value. </value>
        public int StateId { get; set; }

        /// <summary>
        ///   Gets or sets the name of the state.
        /// </summary>
        /// <value> The name of the state. </value>
        public string StateName { get; set; }
    }

    /// <summary>
    ///   PropertiesMustMatchAttribute, forces proerties to match.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        /// <summary>
        /// The default error message.
        /// </summary>
        private const string DefaultErrorMessage = "'{0}' and '{1}' do not match.";

        /// <summary>
        /// The _type id.
        /// </summary>
        private readonly object typeId = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesMustMatchAttribute"/> class.
        /// </summary>
        /// <param name="originalProperty">
        /// The original property. 
        /// </param>
        /// <param name="confirmProperty">
        /// The confirm property. 
        /// </param>
        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(DefaultErrorMessage)
        {
            this.OriginalProperty = originalProperty;
            this.ConfirmProperty = confirmProperty;
        }

        /// <summary>
        ///   Gets the confirm property.
        /// </summary>
        public string ConfirmProperty { get; private set; }

        /// <summary>
        ///   Gets the original property.
        /// </summary>
        public string OriginalProperty { get; private set; }

        /// <summary>
        ///   When implemented in a derived class, gets a unique identifier for this <see cref="T:System.Attribute" />.
        /// </summary>
        /// <returns> An <see cref="T:System.Object" /> that is a unique identifier for the attribute. </returns>
        public override object TypeId
        {
            get
            {
                return this.typeId;
            }
        }

        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">
        /// The name to include in the formatted message. 
        /// </param>
        /// <returns>
        /// An instance of the formatted error message. 
        /// </returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(
                CultureInfo.CurrentUICulture, this.ErrorMessageString, this.OriginalProperty, this.ConfirmProperty);
        }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">
        /// The value of the object to validate. 
        /// </param>
        /// <returns>
        /// true if the specified value is valid; otherwise, false. 
        /// </returns>
        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(this.OriginalProperty, true /* ignoreCase */).GetValue(value);
            object confirmValue = properties.Find(this.ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return object.Equals(originalValue, confirmValue);
        }
    }

    /// <summary>
    ///   ValidatePasswordLengthAttribute class
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        /// <summary>
        /// The default error message.
        /// </summary>
        private const string DefaultErrorMessage = "'{0}' must be at least {1} characters long.";

        /// <summary>
        /// The _min characters.
        /// </summary>
        private readonly int minCharacters = Membership.Provider.MinRequiredPasswordLength;

        /// <summary>
        ///   Initializes a new instance of the <see cref="ValidatePasswordLengthAttribute" /> class.
        /// </summary>
        public ValidatePasswordLengthAttribute()
            : base(DefaultErrorMessage)
        {
        }

        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">
        /// The name to include in the formatted message. 
        /// </param>
        /// <returns>
        /// An instance of the formatted error message. 
        /// </returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentUICulture, this.ErrorMessageString, name, this.minCharacters);
        }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">
        /// The value of the object to validate. 
        /// </param>
        /// <returns>
        /// true if the specified value is valid; otherwise, false. 
        /// </returns>
        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return valueAsString != null && valueAsString.Length >= this.minCharacters;
        }
    }
}
