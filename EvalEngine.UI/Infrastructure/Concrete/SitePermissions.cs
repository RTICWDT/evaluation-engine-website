// -----------------------------------------------------------------------
// <copyright file="SitePermissions.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.UI.Infrastructure.Concrete
{
    using System.Collections.Generic;
    using System.Linq;
    using EvalEngine.Infrastructure.Abstract;
    using EvalEngine.Domain.Entities;
    using EvalEngine.UI.Infrastructure.Abstract;

    /// <summary>
    /// A class in charge of handling permissions for most sections of the site.
    /// </summary>
    public class SitePermissions : IPermissions
    {
        /// <summary>
        /// The inner permissions
        /// </summary>
        private readonly IPermissions innerPermissions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitePermissions"/> class.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="userRoles">The user roles.</param>
        public SitePermissions(string sectionName, string[] userRoles)
        {
            this.innerPermissions = this.SetInnerPermissions(sectionName, userRoles);
        }

        /// <summary>
        /// Can the user view the section?
        /// </summary>
        /// <returns>
        ///   <c>True</c> if the user has permission to view the object; <c>false</c> otherwise.
        /// </returns>
        public bool CanView()
        {
            return this.innerPermissions.CanView();
        }

        /// <summary>
        /// Can the user edit items in the section?
        /// </summary>
        /// <returns>
        ///   <c>True</c> if the user has permission to edit the object; <c>false</c> otherwise.
        /// </returns>
        public bool CanEdit()
        {
            return this.innerPermissions.CanEdit();
        }

        /// <summary>
        /// Can the user create items in the section?
        /// </summary>
        /// <returns>
        ///   <c>True</c> if the user has permission to create the object; <c>false</c> otherwise.
        /// </returns>
        public bool CanCreate()
        {
            return this.innerPermissions.CanCreate();
        }

        /// <summary>
        /// Can the user delete items from the section?
        /// </summary>
        /// <returns>
        ///   <c>True</c> if the user has permission to delete the object; <c>false</c> otherwise.
        /// </returns>
        public bool CanDelete()
        {
            return this.innerPermissions.CanDelete();
        }

        /// <summary>
        /// Sets the inner permissions.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="userRoles">The user roles.</param>
        /// <returns>An object that knows how to handle permissions for the given section.</returns>
        private IPermissions SetInnerPermissions(string sectionName, string[] userRoles)
        {
            var permissions = new DefaultPermissions();
            if (string.IsNullOrEmpty(sectionName) || userRoles == null || userRoles.Length == 0)
            {
                return permissions;
            }

            switch (sectionName)
            {
                /*case Constants.StaffCalendarSection:
                    return new StaffCalendarPermissions(userRoles);
                case Constants.ContactsDatabaseSection:
                    return new ContactsDatabasePermissions(userRoles);
                case Constants.EmailSection:
                    return new EmailPermissions(userRoles);
                case Constants.RegistrationListsSection:
                    return new RegistrationListsPermissions(userRoles);
                case Constants.StateAssignmentsSection:
                    return new StateAssignmentsPermissions(userRoles);
                case Constants.MonitoringSection:
                    return new MonitoringPermissions(userRoles);
                case Constants.AuditFindingsSection:
                    return new AuditFindingsPermissions(userRoles);
                case Constants.WorkPlanSection:
                    return new WorkPlanPermissions(userRoles);*/
                default:
                    return new DefaultPermissions();
            }
        }
    }
}