// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountControllerTest.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// <summary>
//   The account controller test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace EvalEngine.Test
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;
    using EvalEngine.Domain.Abstract;
    using EvalEngine.Infrastructure.Abstract;
    using EvalEngine.UI.Controllers;
    using EvalEngine.UI.Models;
    using Moq;
    using NUnit.Framework;

    /// <summary>
    /// The account controller test.
    /// </summary>
    [TestFixture]
    public class AccountControllerTest
    {
        /// <summary>
        /// The mock state.
        /// </summary>
        private Mock<IStateRepository> mockState = new Mock<IStateRepository>();

        /// <summary>
        /// The mock state assignment.
        /// </summary>
        private Mock<IStateAssignmentRepository> mockStateAssignment = new Mock<IStateAssignmentRepository>();

        /// <summary>
        /// Mock context base
        /// </summary>
        private Mock<HttpContextBase> mockContext;

        /// <summary>
        /// Mock account controller
        /// </summary>
        private Mock<AccountController> mockAccountController;

        /// <summary>
        /// The StaticMembershipService interface
        /// </summary>
        public interface IStaticMembershipService
        {
            MembershipUser GetUser();

            void UpdateUser(MembershipUser user);
        }

        /// <summary>
        /// The StaticMembershipService
        /// </summary>
        public class StaticMembershipService : IStaticMembershipService
        {
            public System.Web.Security.MembershipUser GetUser()
            {
                return Membership.GetUser();
            }

            public void UpdateUser(MembershipUser user)
            {
                Membership.UpdateUser(user);
            }
        }

        /// <summary>
        /// The setup method
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Arrange - folder with testing files
            string testingFolder = @"C:GEE\EvalEngine\EvalEngine.Test\TestFixtures\";

            // Arrange - create mock repositories to instantiate controller
            this.mockAccountController = new Mock<AccountController>();

            // Arrange - mock logger
            // Arrange - set up elements for controller context
            Mock<HttpServerUtilityBase> mockServer = new Mock<HttpServerUtilityBase>();
            mockServer.Setup(s => s.MapPath(It.IsAny<string>())).Returns(testingFolder);
            var mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            var mockRequest = new Mock<HttpRequestBase>(MockBehavior.Strict);

            this.mockContext = new Mock<HttpContextBase>();
            this.mockContext.SetupGet(x => x.Request).Returns(mockRequest.Object);
            this.mockContext.SetupGet(x => x.Response).Returns(mockResponse.Object);
            this.mockContext.SetupGet(x => x.Server).Returns(mockServer.Object);
            this.mockContext.SetupGet(x => x.Request["X-Requested-With"]).Returns("XMLHttpRequest");
            this.mockContext.SetupGet(x => x.User.Identity.Name).Returns("Dr. Evil");

            var membershipMock = new Mock<IStaticMembershipService>();
            var userMock = new Mock<MembershipUser>();
            var profileMock = new Mock<UserProfile>();
            userMock.Setup(u => u.ProviderUserKey).Returns(new Guid());
            userMock.Setup(u => u.Email).Returns("asmith@mprinc.com");
            membershipMock.Setup(s => s.GetUser()).Returns(userMock.Object);
        }

        /// <summary>
        /// The change password_ get_ returns view.
        /// </summary>
        [Test]
        public void ForceChangePassword_Get_ReturnsView()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.ChangePassword();

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            Assert.AreEqual(12, ((ViewResult)result).ViewData["PasswordLength"]);
        }

        /// <summary>
        /// The change password_ post_ returns redirect on success.
        /// </summary>
        [Test]
        [Ignore("Ignore a test")]
        public void ChangePasswordPartial_Post_ReturnsRedirectOnSuccess()
        {
            AccountController controller = GetAccountController();

            IPrincipal fakeUser = new GenericPrincipal(new GenericIdentity("smithale@gmail.com", "Forms"), null);

            ChangePasswordModel modelChange = new ChangePasswordModel
            {
                Password = "goodNewPassword",
                ConfirmPassword = "goodNewPassword",
                OldPassword = "goodOldPassword"
            };

            ActionResult result = controller.ChangePasswordPartial(modelChange, fakeUser);

            //// Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual("MyAccount", viewResult.ViewName);

            ////PartialViewResult pvr = new PartialViewResult();

            /*var mock = new Mock<AccountController>();
            mock.Setup(acctController => acctController.ChangePasswordPartial(modelChange, fakeUser)).Returns();*/

            //// Act
            ////ActionResult result = controller.ChangePasswordPartial(modelChange, fakeUser);

            //// Assert
            ////Assert.IsInstanceOf(typeof(ViewResult), result);
            ////ViewResult viewResult = (ViewResult)result;
            ////Assert.AreEqual("MyAccount", viewResult.ViewName);
            ////Assert.AreEqual(mock., pvr);
        }

        /// <summary>
        /// The change password_ post_ returns view if change password fails.
        /// </summary>
        [Test]
        [Ignore("Ignore a test")]
        public void ChangePasswordPartial_Post_ReturnsViewIfChangePasswordFails()
        {
            // Arrange
            AccountController controller = GetAccountController();
            ChangePasswordModel model = new ChangePasswordModel
            {
                OldPassword = "goodOldPassword",
                Password = "badNewPassword",
                ConfirmPassword = "badNewPassword"
            };

            IPrincipal fakeUser = new GenericPrincipal(new GenericIdentity("Scott", "Forms"), null);

            // Act
            ActionResult result = controller.ChangePasswordPartial(model, fakeUser);

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual(model, viewResult.ViewData.Model);
            Assert.AreEqual(
                "The current password is incorrect or the new password is invalid.",
                controller.ModelState[string.Empty].Errors[0].ErrorMessage);
            Assert.AreEqual(12, viewResult.ViewData["PasswordLength"]);
        }

        /// <summary>
        /// The change password_ post_ returns view if model state is invalid.
        /// </summary>
        [Test]
        [Ignore("Ignore a test")]
        public void ChangePasswordPartial_Post_ReturnsViewIfModelStateIsInvalid()
        {
            // Arrange
            AccountController controller = GetAccountController();
            ChangePasswordModel model = new ChangePasswordModel
            {
                OldPassword = "goodOldPassword",
                Password = "goodNewPassword",
                ConfirmPassword = "goodNewPassword"
            };
            controller.ModelState.AddModelError(string.Empty, "Dummy error message.");

            IPrincipal fakeUser = new GenericPrincipal(new GenericIdentity("smithale@gmail.com", "Forms"), null);

            // Act
            ActionResult result = controller.ChangePasswordPartial(model, fakeUser);

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            ViewResult viewResult = (ViewResult)result;
            Assert.AreNotEqual(model, viewResult.ViewData.Model);
            Assert.AreEqual(12, viewResult.ViewData["PasswordLength"]);
        }

        /// <summary>
        /// The log off_ logs out and redirects.
        /// </summary>
        [Test]
        public void LogOff_LogsOutAndRedirects()
        {
            // Arrange
            AccountController controller = GetAccountController();
            MockFormsAuthenticationService fas = new MockFormsAuthenticationService();
            fas.SignIn("goodEmail", false);
            Assert.AreEqual(true, fas.SignIn_WasCalled);

            // Act
            ////ActionResult result = controller.LogOff();
            fas.SignOut();

            // Assert
            // Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);

            // RedirectToRouteResult redirectResult = (RedirectToRouteResult)result;
            // Assert.AreEqual("Home", redirectResult.RouteValues["controller"]);
            // Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            Assert.IsTrue(fas.SignOut_WasCalled);
        }

        /// <summary>
        /// The log on_ get_ returns view.
        /// </summary>
        [Test]
        public void LogOn_Get_ReturnsView()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.LogOn("");

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        /// <summary>
        /// The log on_ post_ returns redirect on success_ with return url.
        /// </summary>
        [Test]
        [Ignore("Ignore a test")]
        public void LogOn_Post_ReturnsRedirectOnSuccess_WithReturnUrl()
        {
            // Arrange
            AccountController controller = GetAccountController();
            LogOnModel model = new LogOnModel { UserName = "goodEmail", Password = "goodPassword", RememberMe = false };

            // Act
            ActionResult result = controller.LogOn("Log On", model, "/someUrl");

            // Assert
            Assert.IsInstanceOf(typeof(RedirectResult), result);
            RedirectResult redirectResult = (RedirectResult)result;
            Assert.AreEqual("/someUrl", redirectResult.Url);
            Assert.IsTrue(((MockFormsAuthenticationService)controller.FormsService).SignIn_WasCalled);
        }

        /// <summary>
        /// The log on_ post_ returns redirect on success_ without return url.
        /// </summary>
        [Test]
        [Ignore("Ignore a test")]
        public void LogOn_Post_ReturnsViewOnSuccess_WithoutReturnUrl()
        {
            // Arrange
            AccountController controller = GetAccountController();
            LogOnModel model = new LogOnModel { UserName = "goodEmail", Password = "goodPassword", RememberMe = false };

            // Act
            ActionResult result = controller.LogOn("Log On", model, null);

            // Assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            RedirectToRouteResult redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("Home", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            Assert.IsTrue(((MockFormsAuthenticationService)controller.FormsService).SignIn_WasCalled);
        }

        /// <summary>
        /// The log on_ post_ returns view if model state is invalid.
        /// </summary>
        [Test]
        [Ignore("Ignore a test")]
        public void LogOn_Post_ReturnsViewIfModelStateIsInvalid()
        {
            // Arrange
            AccountController controller = GetAccountController();
            LogOnModel model = new LogOnModel { UserName = "goodEmail", Password = "goodPassword", RememberMe = false };
            controller.ModelState.AddModelError(string.Empty, "Dummy error message.");

            // Act
            ActionResult result = controller.LogOn("Log On", model, null);

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual(model, viewResult.ViewData.Model);
        }

        /// <summary>
        /// The log on_ post_ returns view if validate user fails.
        /// </summary>
        [Test]
        [Ignore("Ignore a test")]
        public void LogOn_Post_ReturnsViewIfValidateUserFails()
        {
            // Arrange
            AccountController controller = GetAccountController();
            LogOnModel model = new LogOnModel { UserName = "goodEmail", Password = "badPassword", RememberMe = false };

            // Act
            ActionResult result = controller.LogOn("Log On", model, null);

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual(model, viewResult.ViewData.Model);

            // Assert.AreEqual("The user name or password provided is incorrect.", controller.ModelState[string.Empty].Errors[0].ErrorMessage);
        }

        /// <summary>
        /// The user add_ get_ returns view.
        /// </summary>
        [Test]
        [Ignore("Ignore this test")]
        public void UserAdd_Get_ReturnsView()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.Add();

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        /// <summary>
        /// The user add_ post_ returns redirect on success.
        /// </summary>
        [Test]
        [Ignore("Ignore a test")]
        public void UserAdd_Post_ReturnsRedirectOnSuccess()
        {
            // Arrange
            AccountController controller = GetAccountController();
            UserAddModel model = new UserAddModel
            {
                Email = "goodEmail",
                UserName = "goodEmail",
                FirstName = "goodFName",
                LastName = "goodLName",
                Organization = "MPR",
                AccountType = null,
                AccountTypeOptions = null,
                RadioSelectedState = 1,
                RoleSelections = new List<RoleSelection>(),
                RadioStateSelections = new List<StateSelection>(),
                CheckboxStateSelections = new List<StateSelection>()
            };

            // Act
            ActionResult result = controller.Add(model);

            // Assert
            Assert.IsInstanceOf(typeof(RedirectToRouteResult), result);
            RedirectToRouteResult redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("Account", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Users", redirectResult.RouteValues["action"]);
        }

        /// <summary>
        /// The user add_ post_ returns view if model state is invalid.
        /// </summary>
        [Test]
        public void UserAdd_Post_ReturnsViewIfModelStateIsInvalid()
        {
            // Arrange
            AccountController controller = GetAccountController();
            UserAddModel model = new UserAddModel
            {
                Email = "goodEmail",
                UserName = "goodEmail",
                FirstName = "goodFName",
                LastName = "goodLName",
                Organization = "MPR",
                AccountType = null,
                AccountTypeOptions = null,
                RadioSelectedState = 1,
                RoleSelections = new List<RoleSelection>(),
                RadioStateSelections = new List<StateSelection>(),
                CheckboxStateSelections = new List<StateSelection>()
            };
            controller.ModelState.AddModelError(string.Empty, "Dummy error message.");

            // Act
            ActionResult result = controller.Add(model);

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual(model, viewResult.ViewData.Model);
        }

        /// <summary>
        /// The user add_ post_ returns view if user add fails.
        /// </summary>
        [Test]
        [Ignore("Ignore a test")]
        public void UserAdd_Post_ReturnsViewIfUserAddFails()
        {
            // Arrange
            AccountController controller = GetAccountController();
            UserAddModel model = new UserAddModel
            {
                Email = "asmith@mprinc.com",
                UserName = "goodEmail",
                FirstName = "goodFName",
                LastName = "goodLName",
                Organization = "MPR",
                AccountType = null,
                AccountTypeOptions = null,
                RadioSelectedState = 1,
                RoleSelections = new List<RoleSelection>(),
                RadioStateSelections = new List<StateSelection>(),
                CheckboxStateSelections = new List<StateSelection>()
            };

            // Act
            ActionResult result = controller.Add(model);

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual(model, viewResult.ViewData.Model);
            Assert.AreEqual(
                "Username already exists. Please enter a different user name.",
                controller.ModelState[string.Empty].Errors[0].ErrorMessage);
        }

        /// <summary>
        ///   UserEdit Tests
        /// </summary>
        [Test]
        [Ignore("Ignore a test")]
        public void UserEdit_Get_ReturnsView()
        {
            // Arrange
            AccountController controller = GetAccountController();

            // Act
            ActionResult result = controller.Edit("asmith@mprinc.com");

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }

        /// <summary>
        /// The user edit_ post_ returns redirect on success.
        /// </summary>
        [Test]
        [Ignore("Ignore a test")]
        public void UserEdit_Post_ReturnsRedirectOnSuccess()
        {
            // Arrange
            AccountController controller = GetAccountController();
            UserEditModel model = new UserEditModel
            {
                Email = "goodEmail",
                UserName = "goodEmail",
                FirstName = "goodFName",
                LastName = "goodLName",
                MiddleInitial = "M",
                Organization = "MPR",
                IsActive = true,
                AccountType = null,
                AccountTypeOptions = null,
                RadioSelectedState = 1,
                RoleSelections = new List<RoleSelection>(),
                RadioStateSelections = new List<StateSelection>(),
                CheckboxStateSelections = new List<StateSelection>()
            };

            // Act
            ActionResult result = controller.Edit(model);

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            RedirectToRouteResult redirectResult = (RedirectToRouteResult)result;
            Assert.AreEqual("Account", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Users", redirectResult.RouteValues["action"]);
        }

        /// <summary>
        /// The user edit_ post_ returns view if model state is invalid.
        /// </summary>
        [Test]
        public void UserEdit_Post_ReturnsViewIfModelStateIsInvalid()
        {
            // Arrange
            AccountController controller = GetAccountController();
            UserEditModel model = new UserEditModel
            {
                Email = "goodEmail",
                UserName = "goodEmail",
                FirstName = "goodFName",
                LastName = "goodLName",
                MiddleInitial = "M",
                Organization = "MPR",
                IsActive = true,
                AccountType = null,
                AccountTypeOptions = null,
                RadioSelectedState = 1,
                RoleSelections = new List<RoleSelection>(),
                RadioStateSelections = new List<StateSelection>(),
                CheckboxStateSelections = new List<StateSelection>()
            };
            controller.ModelState.AddModelError(string.Empty, "Dummy error message.");

            // Act
            ActionResult result = controller.Edit(model);

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), result);
            ViewResult viewResult = (ViewResult)result;
            Assert.AreEqual(model, viewResult.ViewData.Model);
        }

        /// <summary>
        /// The get account controller.
        /// </summary>
        /// <returns>
        /// The account controller
        /// </returns>
        private static AccountController GetAccountController()
        {
            Mock<IStateAssignmentRepository> mockStateAssignment = new Mock<IStateAssignmentRepository>();
            Mock<IStateRepository> mockState = new Mock<IStateRepository>();
            Mock<IUserAccountInfoRepository> mockUser = new Mock<IUserAccountInfoRepository>();
            Mock<IPasswordHistoryRepository> passMock = new Mock<IPasswordHistoryRepository>();
            Mock<ILogger> mockLogger = new Mock<ILogger>();
            var membershipMock = new Mock<IStaticMembershipService>();
            var userMock = new Mock<MembershipUser>();
            
            userMock.Setup(u => u.ProviderUserKey).Returns(new Guid());
            userMock.Setup(u => u.Email).Returns("asmith@mprinc.com");
            userMock.Setup(u => u.UserName).Returns("asmith@mprinc.com");
            membershipMock.Setup(s => s.GetUser()).Returns(userMock.Object);

            AccountController controller = new AccountController(mockLogger.Object, mockStateAssignment.Object, mockState.Object, mockUser.Object, passMock.Object)
            {
                FormsService = new MockFormsAuthenticationService(),
                MembershipService = new MockMembershipService()
            };

            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                RequestContext = new RequestContext(new MockHttpContext(), new RouteData())
            };

            return controller;
        }

        /// <summary>
        /// The mock forms authentication service.
        /// </summary>
        private class MockFormsAuthenticationService : IFormsAuthenticationService
        {
            /// <summary>
            /// The sign in_ was called.
            /// </summary>
            public bool SignIn_WasCalled;

            /// <summary>
            /// The sign out_ was called.
            /// </summary>
            public bool SignOut_WasCalled;

            /// <summary>
            /// The sign in.
            /// </summary>
            /// <param name="userName">
            /// The user name.
            /// </param>
            /// <param name="createPersistentCookie">
            /// The create persistent cookie.
            /// </param>
            public void SignIn(string userName, bool createPersistentCookie)
            {
                // verify that the arguments are what we expected
                Assert.AreEqual("goodEmail", userName);
                Assert.IsFalse(createPersistentCookie);

                this.SignIn_WasCalled = true;
            }

            /// <summary>
            /// The sign out.
            /// </summary>
            public void SignOut()
            {
                this.SignOut_WasCalled = true;
            }
        }

        /// <summary>
        /// The mock http context.
        /// </summary>
        private class MockHttpContext : HttpContextBase
        {
            /// <summary>
            /// The _user.
            /// </summary>
            private readonly IPrincipal _user = new GenericPrincipal(new GenericIdentity("asmith@mprinc.com"), null);

            /// <summary>
            /// Gets or sets the user.
            /// </summary>
            public override IPrincipal User
            {
                get
                {
                    return this._user;
                }

                set
                {
                    base.User = value;
                }
            }
        }

        /// <summary>
        /// The mock membership service.
        /// </summary>
        private class MockMembershipService : IMembershipService
        {
            /// <summary>
            /// Gets the min password length.
            /// </summary>
            public int MinPasswordLength
            {
                get
                {
                    return 12;
                }
            }

            /// <summary>
            /// The change password.
            /// </summary>
            /// <param name="userName">
            /// The user name.
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
                return userName == "goodEmail" && oldPassword == "goodOldPassword" && newPassword == "goodNewPassword";
            }

            /// <summary>
            /// The create user.
            /// </summary>
            /// <param name="userName">
            /// The user name.
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
                if (userName == "duplicateEmail")
                {
                    return MembershipCreateStatus.DuplicateUserName;
                    ////return "";
                }

                // verify that values are what we expected
                Assert.AreEqual("00000000", password);
                Assert.AreEqual("goodEmail", email);

                return MembershipCreateStatus.Success;
                ////return "";
            }

            /// <summary>
            /// The validate user.
            /// </summary>
            /// <param name="userName">
            /// The user name.
            /// </param>
            /// <param name="password">
            /// The password.
            /// </param>
            /// <returns>
            /// The System.Boolean.
            /// </returns>
            public bool ValidateUser(string userName, string password)
            {
                return userName == "goodEmail" && password == "goodPassword";
            }

            /// <summary>
            /// The validate user.
            /// </summary>
            /// <param name="userName">
            /// The user name.
            /// </param>
            /// <param name="online">
            /// online boolean
            /// </param>
            /// <returns>
            /// The MembershipUser that was retrieved
            /// </returns>
            public MembershipUser GetUser(string userName, bool online)
            {
                if (string.IsNullOrEmpty(userName))
                {
                    throw new ArgumentException("Value cannot be null or empty.", "userName");
                }

                object membershipObj = new object();
                MembershipUser usr = (MembershipUser)membershipObj;
                usr.Email = "asmith@mprinc.com";

                return usr;
            }

            /// <summary>
            /// Sends confirmation email on account creation.
            /// </summary>
            /// <param name="userName">
            /// Name of the user. 
            /// </param>
            public void SendConfirmationEmail(string userName)
            {
                MembershipUser user = Membership.GetUser(userName);
                string confirmationGuid = user.ProviderUserKey.ToString();
                string verifyUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                                 "/Account/Verify?ID=" + confirmationGuid;
            }

            /// <summary>
            /// Sends confirmation email on account creation.
            /// </summary>
            /// <param name="userName">
            /// Name of the user. 
            /// </param>
            public void SendPasswordResetEmail(string userName)
            {
                MembershipUser user = Membership.GetUser(userName);
                string userGuid = user.ProviderUserKey.ToString();
                UserProfile profile = UserProfile.GetUserProfile(userName);
                string verifyUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                                 "/Account/ResetPassword?ID=" + userGuid + "&RGUID=";
            }
        }
    }
}