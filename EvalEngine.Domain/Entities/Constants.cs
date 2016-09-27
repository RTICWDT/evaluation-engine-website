// -----------------------------------------------------------------------
// <copyright file="Constants.cs" company="RTI, Inc.">
// Constants used throughout application, mostly for role names.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Constants used for role names
    /// </summary>
    public static class Constants
    {
        /// <summary>Constant for project admin role.</summary>
        public const string ProjectAdminRole = "Project Administrator";

        /// <summary>Constant for project user role.</summary>
        public const string ProjectUserRole = "Project User";

        /// <summary>Constant for state admin role.</summary>
        public const string StateAdminRole = "State Administrator";

        /// <summary>Constant for state user role.</summary>
        public const string StateUserRole = "State User";

        /// <summary>Constant for multiple state user role.</summary>
        public const string MultipleStateUserRole = "Multiple State User";

        /// <summary>Constant for site admin role.</summary>
        public const string SiteAdministratorRole = "Site Administrator";
    }
}