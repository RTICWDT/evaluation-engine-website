// -----------------------------------------------------------------------
// <copyright file="EntitiesFactory.cs" company="RTI, Inc.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Test
{
    using System.Security.Principal;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EntitiesFactory
    {
        /// <summary>
        /// Get admin IPrincipal user
        /// </summary>
        /// <returns>The admin IPrincipal user</returns>
        public static IPrincipal GetAdmin()
        {
            var identity = new GenericIdentity("Admin");
            var roles = new string[] { "Project Administrator" };
            var user = new GenericPrincipal(identity, roles);
            return user;
        }
    }
}