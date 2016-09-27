// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountManager.cs" company="MPR INC">
//   Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace EvalEngine.UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Profile;
    using System.Web.Security;
    using Newtonsoft.Json;
    using EvalEngine.Domain.Abstract;
    using EvalEngine.Domain.Entities;
    using EvalEngine.Infrastructure.Abstract;

    /// <summary>
    /// A class doing CRUD on user accounts.
    /// A user account has up to four components:
    /// 1] Membership: credentials.
    /// 2] Profile.
    /// 3] Roles.
    /// 4] When applicable, state assignments.
    /// </summary>
    public class AccountManager
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The user account info repository.
        /// </summary>
        private readonly IUserAccountInfoRepository userAccountInfoRepository;

        /// <summary>
        /// The state assignment repository
        /// </summary>
        private readonly IStateAssignmentRepository stateAssignmentRepository;

        /// <summary>
        /// The password history repository
        /// </summary>
        private readonly IPasswordHistoryRepository passwordHistoryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountManager" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="userAccountInfoRepository">The user account info repository.</param>
        /// <param name="stateAssignmentRepository">The state assignment repository.</param>
        /// <param name="passwordHistoryRepository">The password history repository.</param>
        public AccountManager(ILogger logger, IUserAccountInfoRepository userAccountInfoRepository, IStateAssignmentRepository stateAssignmentRepository, IPasswordHistoryRepository passwordHistoryRepository)
        {
            this.logger = logger;
            this.userAccountInfoRepository = userAccountInfoRepository;
            this.stateAssignmentRepository = stateAssignmentRepository;
            this.passwordHistoryRepository = passwordHistoryRepository;
        }

        /// <summary>
        /// An enumeration of the two application we manage.
        /// </summary>
        public enum Application
        {
            /// <summary>
            /// The EvalEngine application
            /// </summary>
            EvalEngine
        }

        /// <summary>
        /// Gets the application provider names.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <returns>A struct containing the application provider names for membership and roles.</returns>
        public static ProviderNames GetProviderNamesFor(Application application)
        {
            ProviderNames pn;
            if (application.Equals(Application.EvalEngine))
            {
                pn.Membership = "AspNetSqlMembershipProvider";
                pn.Role = "AspNetSqlRoleProvider";
            }
            else
            {
                pn.Membership = "AspNetSqlMembershipProvider";
                pn.Role = "AspNetSqlRoleProvider";
            }

            return pn;
        }

        /// <summary>
        /// Accounts the transaction was successful.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns><c>true</c> if all the components of a user account were updated successfully. <c>false</c> otherwise.</returns>
        public static bool AccountTransactionWasSuccessful(AccountTransactionStatus status)
        {
            return status.HasUpdatedMembership && status.HasUpdatedProfile && status.HasUpdatedRoles && status.HasUpdatedStateAssignments;
        }

        /// <summary>
        /// Populates the user add model with account container.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="container">The container.</param>
        public static void PopulateUserAddModelWithAccountContainer(UserAddModel model, AccountContainer container)
        {
            // update user information
            model.UserName = container.User.UserName;

            // update profile information
            MapProfileToUserAddModel(container.Profile, model);

            // update roles
            model.RoleSelections.ForEach(x => x.IsChecked = container.Roles.Contains(x.RoleName));

            // update state assignments
            var selectedStates = container.StateAssignments.Select(x => x.StateId);
            model.CheckboxStateSelections.ForEach(x => x.IsChecked = selectedStates.Contains(x.StateId));
        }

        /// <summary>
        /// Maps the user add model to profile.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="profile">The profile.</param>
        public static void MapUserAddModelToProfile(UserAddModel model, UserProfile profile)
        {
            profile.FirstName = model.FirstName;
            profile.LastName = model.LastName;
            profile.AccountType = model.AccountType;
            profile.Title = model.Title;
            profile.Organization = model.Organization;
        }

        /// <summary>
        /// Maps the profile to user add model.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="model">The model.</param>
        public static void MapProfileToUserAddModel(UserProfile profile, UserAddModel model)
        { 
            model.FirstName = profile.FirstName;
            model.LastName = profile.LastName;
            model.AccountType = profile.AccountType;
            model.Title = profile.Title;
            model.Organization = profile.Organization;
        }

        /// <summary>
        /// Creates the user account.
        /// An account has up to four components:
        /// 1] Membership: credentials.
        /// 2] Profile.
        /// 3] Roles.
        /// 4] When applicable, state assignments.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="isApproved">if set to <c>true</c> [is approved].</param>
        /// <returns>
        /// An AccountTransactionStatus struct.
        /// </returns>
        public AccountTransactionStatus CreateUserAccount(UserAddModel model, bool isApproved = true)
        {
            var application = Application.EvalEngine;

            AccountTransactionStatus status = new AccountTransactionStatus { HasUpdatedMembership = false, HasUpdatedStateAssignments = false, HasUpdatedRoles = false, HasUpdatedProfile = false };

            var providersNames = GetProviderNamesFor(application);
            var userName = this.CreateUser(model, providersNames.Membership, isApproved);
            if (userName != null)
            {
                status.HasUpdatedMembership = true;
                status.HasUpdatedProfile = this.UpdateUserProfile(userName, model);
                status.HasUpdatedRoles = this.UpdateUserRoles(userName, model.RoleSelections, providersNames.Role);
                status.HasUpdatedStateAssignments = this.UpdateUserStateAssignments(userName, model.CheckboxStateSelections, model.RadioSelectedState, providersNames.Role);
            }

            return status;
        }

        /// <summary>
        /// Updates the user account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="isApproved">if set to <c>true</c> [is approved].</param>
        /// <returns>
        /// An AccountTransactionStatus struct.
        /// </returns>
        public AccountTransactionStatus UpdateUserAccount(UserAddModel model, bool isApproved = true)
        {
            // Try to find user.
            AccountTransactionStatus status = new AccountTransactionStatus { HasUpdatedMembership = false, HasUpdatedStateAssignments = false, HasUpdatedRoles = false, HasUpdatedProfile = false };
            var container = this.FindUser(model.UserName);
            if (container.IsEmpty)
            {
                var logInfo = new LogInformation
                {
                    ClassName = "AccountManager",
                    MethodName = "UpdateUserAccount",
                    ArgumentsPassedIn = new string[] { JsonConvert.SerializeObject(model), isApproved.ToString() },
                    ErrorMessage = "User was not found or does not exist." 
                };

                this.logger.Error(JsonConvert.SerializeObject(logInfo));
            }
            else
            {
                // perform updates
                status.HasUpdatedMembership = this.UpdateUser(container.User, model, container.Providers.Membership, isApproved);
                status.HasUpdatedProfile = this.UpdateUserProfile(model.UserName, model);
                status.HasUpdatedRoles = this.UpdateUserRoles(model.UserName, model.RoleSelections, container.Providers.Role);
                status.HasUpdatedStateAssignments = this.UpdateUserStateAssignments(model.UserName, model.CheckboxStateSelections, model.RadioSelectedState, container.Providers.Role);
            }

            return status;
        }

        /// <summary>
        /// Deletes the user account.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="adminName">Name of the admin deleting the user.</param>
        /// <returns>An AccountTransactionStatus struct.</returns>
        public AccountTransactionStatus DeleteUserAccount(string userName, string adminName)
        {
            AccountTransactionStatus status = new AccountTransactionStatus { HasUpdatedMembership = false, HasUpdatedStateAssignments = false, HasUpdatedRoles = false, HasUpdatedProfile = false };
            if (userName.Equals(adminName))
            {
                var logInfo = new LogInformation
                {
                    ClassName = "AccountManager",
                    MethodName = "UpdateUserAccount",
                    ArgumentsPassedIn = new string[] { userName, adminName },
                    ErrorMessage = "Administrator tried to delete self." 
                };
                this.logger.Error(JsonConvert.SerializeObject(logInfo));
                return status;
            }

            var container = this.FindUser(userName);
            if (container.IsEmpty)
            {
                var logInfo = new LogInformation
                {
                    ClassName = "AccountManager",
                    MethodName = "UpdateUserAccount",
                    ArgumentsPassedIn = new string[] { userName, adminName },
                    ErrorMessage = "User was not found or does not exist." 
                };
                this.logger.Error(JsonConvert.SerializeObject(logInfo));
            }
            else
            {
                status.HasUpdatedStateAssignments = true;
                status.HasUpdatedRoles = this.DeleteUserRoles(userName, container.Providers.Role);
                status.HasUpdatedProfile = this.DeleteUserProfile(userName);
                status.HasUpdatedMembership = this.DeleteUser(container.User, container.Providers);
            }
            
            return status;
        }

        /// <summary>
        /// Gets the user account.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>An account container.</returns>
        public AccountContainer GetUserAccountContainer(string userName)
        {
            var account = new AccountContainer { IsEmpty = true, Profile = null, Roles = null, User = null };
            var container = this.FindUser(userName);
            if (container.IsEmpty)
            {
                return account;
            }

            account.User = container.User;
            try
            {
                account.Profile = UserProfile.GetUserProfile(userName);
                account.Roles = Roles.Providers[container.Providers.Role].GetRolesForUser(userName);
                account.StateAssignments = this.stateAssignmentRepository.SearchFor(x => x.UserName.Equals(userName)).ToList();
                account.IsEmpty = false;
            }
            catch (Exception e)
            {
                account.IsEmpty = true;
                var logInfo = new LogInformation
                {
                    ClassName = "AccountManager",
                    MethodName = "GetUserAccountContainer",
                    ArgumentsPassedIn = new string[] { userName },
                    ErrorMessage = e.Message
                };
                this.logger.Error(JsonConvert.SerializeObject(logInfo));
            }

            return account;
        }

        /// <summary>
        /// Creates the user.
        /// It also populates the userAccountInfo table.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="membershipProviderName">Name of the membership provider.</param>
        /// <param name="isApproved">if set to <c>true</c> [is approved].</param>
        /// <returns>
        /// The username that the application assigns to the user.
        /// </returns>
        private string CreateUser(UserAddModel model, string membershipProviderName, bool isApproved = true)
        {
            var userId = Guid.NewGuid();
            model.UserName = userId.ToString();

            try
            {
                var password = Membership.GeneratePassword(12, 5);
                MembershipCreateStatus createStatus;
                Membership.Providers[membershipProviderName].CreateUser(model.UserName, password, model.Email, null, null, isApproved, userId, out createStatus);
                this.userAccountInfoRepository.CreateUserAccountInfo(userId);
                if (createStatus == MembershipCreateStatus.Success)
                {
                    return model.UserName;
                }
            }
            catch (Exception e)
            {
                var logInfo = new LogInformation
                {
                    ClassName = "AccountManager",
                    MethodName = "CreateUser",
                    ArgumentsPassedIn = new string[] { JsonConvert.SerializeObject(model), membershipProviderName },
                    ErrorMessage = e.Message
                };
                this.logger.Error(JsonConvert.SerializeObject(logInfo));
            }

            return null;
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="model">The model.</param>
        /// <param name="membershipProviderName">Name of the membership provider.</param>
        /// <param name="isApproved">if set to <c>true</c> [is approved].</param>
        /// <returns>
        ///   <c>true</c> if the update was successful; <c>false</c>, otherwise.
        /// </returns>
        private bool UpdateUser(MembershipUser user, UserAddModel model, string membershipProviderName, bool isApproved = true)
        {
            try
            {
                user.IsApproved = isApproved;
                user.Email = model.Email;
                Membership.Providers[membershipProviderName].UpdateUser(user);
                return true;
            }
            catch (Exception e)
            {
                var logInfo = new LogInformation
                {
                    ClassName = "AccountManager",
                    MethodName = "UpdateUser",
                    ArgumentsPassedIn = new string[] { JsonConvert.SerializeObject(user), JsonConvert.SerializeObject(model), membershipProviderName },
                    ErrorMessage = e.Message
                };
                this.logger.Error(JsonConvert.SerializeObject(logInfo));
                return false;
            }
        }

        /// <summary>
        /// Deletes the user, UserAccountInfo, and PasswordHistory.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="providersNames">The providers names.</param>
        /// <returns><c>true</c> if the user was succesffully deleted; <c>false</c> otherwise.</returns>
        private bool DeleteUser(MembershipUser user, ProviderNames providersNames)
        {
            try
            {
                var userId = new Guid(user.ProviderUserKey.ToString());

                // delete userAccountInfo and passwordHistory data
                this.userAccountInfoRepository.DeleteUserAccountInfoByUserId(userId);
                this.passwordHistoryRepository.DeleteByUserId(userId);

                // delete user
                Membership.Providers[providersNames.Membership].DeleteUser(user.UserName, true);
                return true;
            }
            catch (Exception e)
            {
                var logInfo = new LogInformation
                {
                    ClassName = "AccountManager",
                    MethodName = "DeleteUser",
                    ArgumentsPassedIn = new string[] { JsonConvert.SerializeObject(user), JsonConvert.SerializeObject(providersNames) },
                    ErrorMessage = e.Message
                };
                this.logger.Error(JsonConvert.SerializeObject(logInfo));
                return false;
            }
        }

        /// <summary>
        /// Creates the user profile.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="model">The model.</param>
        /// <returns>True if the user profile is created successfully; false otherwise.</returns>
        private bool UpdateUserProfile(string userName, UserAddModel model)
        {
            try
            {
                // add profile information
                var profile = UserProfile.GetUserProfile(userName);
                MapUserAddModelToProfile(model, profile);
                profile.Save();
                return true;
            }
            catch (Exception e)
            {
                var logInfo = new LogInformation
                {
                    ClassName = "AccountManager",
                    MethodName = "UpdateUserProfile",
                    ArgumentsPassedIn = new string[] { userName, JsonConvert.SerializeObject(model) },
                    ErrorMessage = e.Message
                };
                this.logger.Error(JsonConvert.SerializeObject(logInfo));
                return false;
            }
        }

        /// <summary>
        /// Deletes the user profile.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns><c>true</c> if the profile was deleted successfully; <c>false</c> otherwise.</returns>
        private bool DeleteUserProfile(string userName)
        {
            try
            {
                ProfileManager.DeleteProfile(userName);
                return true;
            }
            catch (Exception e)
            {
                var logInfo = new LogInformation
                {
                    ClassName = "AccountManager",
                    MethodName = "DeleteUserProfile",
                    ArgumentsPassedIn = new string[] { userName },
                    ErrorMessage = e.Message
                };
                this.logger.Error(JsonConvert.SerializeObject(logInfo));
                return false;
            }
        }

        /// <summary>
        /// Updates the user roles.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userRoles">The user roles.</param>
        private void UpdateUserRoles(string userName, List<RoleSelection> userRoles)
        {
            var application = Application.EvalEngine;
            var providersNames = GetProviderNamesFor(application);
            this.UpdateUserRoles(userName, userRoles, providersNames.Role);
        }

        /// <summary>
        /// Updates the user roles.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userRoles">The user roles.</param>
        /// <param name="roleProviderName">Name of the role provider.</param>
        /// <returns><c>true</c> if the role updates was successful; <c>false</c> otherwise.</returns>
        private bool UpdateUserRoles(string userName, List<RoleSelection> userRoles, string roleProviderName)
        {
            try
            {
                var oldRoles = Roles.Providers[roleProviderName].GetRolesForUser(userName);
                var newRoles = userRoles.Where(x => x.IsChecked).Select(x => x.RoleName);
                var rolesToKeep = newRoles.Intersect(oldRoles);
                var rolesToDelete = oldRoles.Except(rolesToKeep);
                var rolesToAdd = newRoles.Except(rolesToKeep);

                var usersArray = new string[] { userName };
                if (rolesToDelete.Count() > 0)
                {
                    Roles.Providers[roleProviderName].RemoveUsersFromRoles(usersArray, rolesToDelete.ToArray());
                }

                if (rolesToAdd.Count() > 0)
                {
                    Roles.Providers[roleProviderName].AddUsersToRoles(usersArray, rolesToAdd.ToArray());
                }

                return true;
            }
            catch (Exception e)
            {
                var logInfo = new LogInformation
                {
                    ClassName = "AccountManager",
                    MethodName = "UpdateUserRoles",
                    ArgumentsPassedIn = new string[] { userName, JsonConvert.SerializeObject(userRoles), roleProviderName },
                    ErrorMessage = e.Message
                };
                this.logger.Error(JsonConvert.SerializeObject(logInfo));
                return false;
            }
        }

        /// <summary>
        /// Deletes the user roles.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="roleProviderName">Name of the role provider.</param>
        /// <returns><c>true</c> if the roles are deleted successfully; <c>false</c> otherwise.</returns>
        private bool DeleteUserRoles(string userName, string roleProviderName)
        {
            try
            {
                var userRoles = Roles.Providers[roleProviderName].GetRolesForUser(userName);
                if (userRoles.Count() == 0)
                {
                    return true;
                }

                var usersArray = new string[] { userName };
                Roles.Providers[roleProviderName].RemoveUsersFromRoles(usersArray, userRoles);
                return true;
            }
            catch (Exception e)
            {
                var logInfo = new LogInformation
                {
                    ClassName = "AccountManager",
                    MethodName = "DeleteUserRoles",
                    ArgumentsPassedIn = new string[] { userName, roleProviderName },
                    ErrorMessage = e.Message
                };
                this.logger.Error(JsonConvert.SerializeObject(logInfo));
                return false;
            }
        }

        /// <summary>
        /// Updates the user state assignment for the <c>current year</c>.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="stateSelections">The state selections.</param>
        /// <param name="roleProviderName">The role provider name.</param>
        /// <returns><c>true</c> if the state assignments update was successful; <c>false</c> otherwise.</returns>
        private bool UpdateUserStateAssignments(string userName, List<StateSelection> checkboxStateSelections, int radioSelectedState, string roleProviderName)
        {
            try
            {
                var assignements = this.stateAssignmentRepository.SearchFor(x => x.UserName.Equals(userName));
                this.stateAssignmentRepository.DeleteAll(assignements.AsEnumerable());

                // assign state specific roles
                var stateRoles = new string[] { Constants.ProjectAdminRole, Constants.ProjectUserRole, Constants.MultipleStateUserRole, Constants.SiteAdministratorRole };
                foreach (var state in checkboxStateSelections.Where(x => x.IsChecked))
                {
                    foreach (var role in stateRoles)
                    {
                        if (Roles.Providers[roleProviderName].IsUserInRole(userName, role))
                        {
                            this.stateAssignmentRepository.Update(
                                        new StateAssignment
                                            {
                                                UserName = userName,
                                                StateId = state.StateId,
                                                Role = role
                                            });
                        }
                    }
                }

                var stateUserRoles = new string[] { Constants.StateAdminRole, Constants.StateUserRole };
                    foreach (var role in stateUserRoles)
                    {
                        if (Roles.Providers[roleProviderName].IsUserInRole(userName, role))
                        {
                            this.stateAssignmentRepository.Update(
                                        new StateAssignment
                                            {
                                                UserName = userName,
                                                StateId = radioSelectedState,
                                                Role = role
                                            });
                        }
                    }
                   
                return true;
            }
            catch (Exception e)
            {
                var logInfo = new LogInformation
                {
                    ClassName = "AccountManager",
                    MethodName = "UpdateUserStateAssignments",
                    ArgumentsPassedIn = new string[] { userName, JsonConvert.SerializeObject(checkboxStateSelections), JsonConvert.SerializeObject(radioSelectedState), roleProviderName },
                    ErrorMessage = e.Message
                };
                this.logger.Error(JsonConvert.SerializeObject(logInfo));
                return false;
            }
        }

        /// <summary>
        /// Deletes the user state assignments.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns><c>true</c> if the user state assignments were deleted successfully; <c>false</c> otherwise.</returns>
        private bool DeleteUserStateAssignments(string userName)
        {
            try
            {
                var stateAssignments = this.stateAssignmentRepository.SearchFor(x => x.UserName.Equals(userName)).ToList();
                this.stateAssignmentRepository.DeleteAll(stateAssignments);
                return true;
            }
            catch (Exception e)
            {
                var logInfo = new LogInformation
                {
                    ClassName = "AccountManager",
                    MethodName = "DeleteUserStateAssignments",
                    ArgumentsPassedIn = new string[] { userName },
                    ErrorMessage = e.Message
                };
                this.logger.Error(JsonConvert.SerializeObject(logInfo));
                return false;
            }
        }

        /// <summary>
        /// Finds the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>A UserContainer struct.</returns>
        private UserContainter FindUser(string userName)
        {
            var providersNames = GetProviderNamesFor(Application.EvalEngine);

            UserContainter container;
            container.IsEmpty = true;
            container.User = null;
            container.Providers = providersNames;

            try
            {
                var user = Membership.GetUser(userName);
                if (user == null)
                {
                    providersNames = GetProviderNamesFor(Application.EvalEngine);
                    user = Membership.Providers[providersNames.Membership].GetUser(userName, false);
                }

                if (user != null)
                {
                    container.IsEmpty = false;
                    container.User = user;
                    container.Providers = providersNames;
                }
            }
            catch (Exception e)
            {
                var logInfo = new LogInformation
                {
                    ClassName = "AccountManager",
                    MethodName = "FindUser",
                    ArgumentsPassedIn = new string[] { userName },
                    ErrorMessage = e.Message
                };
                this.logger.Error(JsonConvert.SerializeObject(logInfo));
            }

            return container;
        }

        /// <summary>
        /// Stores the membership and role provider names.
        /// </summary>
        public struct ProviderNames
        {
            /// <summary>
            /// The name of the membership provider
            /// </summary>
            public string Membership;
            
            /// <summary>
            /// The name of the role provider
            /// </summary>
            public string Role; 
        }

        /// <summary>
        /// Stores information specifying whether memberhip, roles, profile and state assignments were updated successfully.
        /// </summary>
        public struct AccountTransactionStatus
        {
            /// <summary>
            /// The transaction has updated membership.
            /// </summary>
            public bool HasUpdatedMembership;

            /// <summary>
            /// The transaction has updated profile
            /// </summary>
            public bool HasUpdatedProfile;

            /// <summary>
            /// The transaction has updated roles
            /// </summary>
            public bool HasUpdatedRoles;

            /// <summary>
            /// The transaction has updated state assignments
            /// </summary>
            public bool HasUpdatedStateAssignments;
        }

        /// <summary>
        /// A container to store user and the providers to modify that user.
        /// </summary>
        public struct UserContainter
        {
            /// <summary>
            /// A flag indicating whether the container actually contains a user.
            /// </summary>
            public bool IsEmpty;

            /// <summary>
            /// The user
            /// </summary>
            public MembershipUser User;

            /// <summary>
            /// The providers
            /// </summary>
            public ProviderNames Providers;
        }

        /// <summary>
        /// A container to store the components of an account:
        /// 1] Membership
        /// 2] Profile
        /// 3] Roles
        /// 4] State assigments
        /// </summary>
        public struct AccountContainer
        {
            /// <summary>
            /// A flag indicating whether the container actually contains an account.
            /// </summary>
            public bool IsEmpty;

            /// <summary>
            /// The user
            /// </summary>
            public MembershipUser User;

            /// <summary>
            /// The profile
            /// </summary>
            public UserProfile Profile;

            /// <summary>
            /// The roles
            /// </summary>
            public string[] Roles;

            /// <summary>
            /// The state assignments
            /// </summary>
            public List<StateAssignment> StateAssignments;
        }
    }
}