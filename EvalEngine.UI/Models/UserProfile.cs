// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserProfile.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// <summary>
//   UserProfile Class - Holds full name, organization, title, reset flag and reset GUID
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace EvalEngine.UI.Models
{
    using System.Web.Profile;
    using System.Web.Security;

    /// <summary>
    /// The User Profile interface
    /// </summary>
    public interface IUserProfile
    {
        /// <summary>
        /// Gets or sets user's first name
        /// </summary>
        [SettingsAllowAnonymous(false)]
        string FirstName { get; set; }

        /// <summary>
        /// Gets or sets user's last name
        /// </summary>
        [SettingsAllowAnonymous(false)]
        string LastName { get; set; }

        /// <summary>
        /// Gets or sets user's account type
        /// </summary>
        [SettingsAllowAnonymous(true)]
        string AccountType { get; set; }

        /// <summary>
        /// Gets or sets user's organization
        /// </summary>
        [SettingsAllowAnonymous(false)]
        string Organization { get; set; }

        /// <summary>
        /// Gets or sets user's title
        /// </summary>
        [SettingsAllowAnonymous(false)]
        string Title { get; set; }

        /// <summary>
        /// Gets the UserProfile based on the userName
        /// </summary>
        /// <param name="userName">the userName string</param>
        /// <returns>UserProfile object</returns>
        UserProfile GetUserProfile(string userName);
    }

    /// <summary>
    ///   UserProfile Class - Holds full name, organization, title, reset flag and reset GUID 
    /// </summary>
    public class UserProfile : ProfileBase
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the account type.
        /// </summary>
        /// <value> The account type. </value>
        [SettingsAllowAnonymous(false)]
        public string AccountType
        {
            get
            {
                return base["AccountType"] as string;
            }

            set
            {
                base["AccountType"] = value;
            }
        }

        /// <summary>
        ///   Gets or sets the first name.
        /// </summary>
        /// <value> The first name. </value>
        [SettingsAllowAnonymous(false)]
        public string FirstName
        {
            get
            {
                return base["FirstName"] as string;
            }

            set
            {
                base["FirstName"] = value;
            }
        }

        /// <summary>
        ///   Gets or sets the organization.
        /// </summary>
        /// <value> The organization. </value>
        [SettingsAllowAnonymous(false)]
        public string Organization
        {
            get
            {
                return base["Organization"] as string;
            }

            set
            {
                base["Organization"] = value;
            }
        }

        /// <summary>
        ///   Gets or sets the last name.
        /// </summary>
        /// <value> The last name. </value>
        [SettingsAllowAnonymous(false)]
        public string LastName
        {
            get
            {
                return base["LastName"] as string;
            }

            set
            {
                base["LastName"] = value;
            }
        }

        /// <summary>
        ///   Gets or sets the title.
        /// </summary>
        /// <value> The title. </value>
        [SettingsAllowAnonymous(false)]
        public string Title
        {
            get
            {
                return base["Title"] as string;
            }

            set
            {
                base["Title"] = value;
            }
        }

        /// <summary>
        ///   Gets or sets the title.
        /// </summary>
        /// <value> The title. </value>
        [SettingsAllowAnonymous(false)]
        public string UserStatesCsv
        {
            get
            {
                return base["UserStatesCsv"] as string;
            }

            set
            {
                base["UserStatesCsv"] = value;
            }
        }

        /// <summary>
        ///   Gets or sets the reset flag.
        /// </summary>
        /// <value> The reset flag. </value>
        [SettingsAllowAnonymous(false)]
        public string ResetFlag
        {
            get
            {
                return base["ResetFlag"] as string;
            }

            set
            {
                base["ResetFlag"] = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the user profile.
        /// </summary>
        /// <param name="username">
        /// The username. 
        /// </param>
        /// <returns>
        /// The EvalEngine.Models.UserProfile.
        /// </returns>
        public static UserProfile GetUserProfile(string username)
        {
            return Create(username) as UserProfile;
        }

        /// <summary>
        /// Gets the user profile.
        /// </summary>
        /// <returns>
        /// The EvalEngine.Models.UserProfile.
        /// </returns>
        public static UserProfile GetUserProfile()
        {
            return Create(Membership.GetUser().UserName) as UserProfile;
        }

        /// <summary>
        /// The get user full name.
        /// </summary>
        /// <returns>
        /// The System.String.
        /// </returns>
        public string GetUserFullName()
        {
            return this.FirstName + " " + this.LastName;
        }

        #endregion
    }
}