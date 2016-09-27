﻿// -----------------------------------------------------------------------
// <copyright file="DefaultPermissions.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.UI.Infrastructure.Concrete
{
    using EvalEngine.UI.Infrastructure.Abstract;

    /// <summary>
    /// A class modeling the default permissions for any section.
    /// </summary>
    public class DefaultPermissions : IPermissions
    {
        /// <summary>
        /// Can the user view the contents of folder?
        /// </summary>
        /// <returns>Always false.</returns>
        public bool CanView()
        {
            return false;
        }

        /// <summary>
        /// Can the user edit folders (i.e., edit their names, change their parent)?
        /// </summary>
        /// <returns>Always false.</returns>
        public bool CanEdit()
        {
            return false;
        }

        /// <summary>
        /// Can the user add new files to the folder?
        /// </summary>
        /// <returns>Always false.</returns>
        public bool CanCreate()
        {
            return false;
        }

        /// <summary>
        /// Can the user delete elements in the folder?
        /// </summary>
        /// <returns>Always false.</returns>
        public bool CanDelete()
        {
            return false;
        }
    }
}