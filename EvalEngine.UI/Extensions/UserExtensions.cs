//-----------------------------------------------------------------------
// <copyright file="UserExtensions.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace EvalEngine.UI.Extensions
{
    using System.Linq;
    using System.Security.Principal;
    using System.Web.Mvc;
    using System.Web.Security;
    using EvalEngine.Domain.Abstract;
    using EvalEngine.Domain.Entities;
    using EvalEngine.UI.Infrastructure;
    using EvalEngine.UI.Infrastructure.Abstract;
    using EvalEngine.UI.Infrastructure.Concrete;
    using EvalEngine.UI.Models;
    using Ninject;


    /// <summary>
    /// Ane
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// The state assignment repository
        /// </summary>
        [Inject]
        private static IStateAssignmentRepository stateAssignmentRepository;

        /// <summary>
        /// Initializes the <see cref="UserExtensions"/> class.
        /// </summary>
        static UserExtensions()
        {
            NinjectDependencyResolver ndr = DependencyResolver.Current as NinjectDependencyResolver;

            if (ndr == null)
            {
                ndr = new NinjectDependencyResolver();

            }

            stateAssignmentRepository = ndr.GetService(typeof(IStateAssignmentRepository)) as IStateAssignmentRepository;
        }

        /// <summary>
        /// Gets the state assignments.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="year">The year.</param>
        /// <returns></returns>
        public static IQueryable<StateAssignment> GetStateAssignments(this IPrincipal user)
        {
            return stateAssignmentRepository.GetStateAssignmentsByUserName(user.Identity.Name);
        }

        public static IPermissions GetPermissionsForSection(this IPrincipal user, string sectionName)
        {
            return new SitePermissions(sectionName, user.GetRoles());
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public static string[] GetRoles(this IPrincipal user)
        {
            return Roles.GetRolesForUser(user.Identity.Name);
        }
    }
}