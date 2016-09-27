// -----------------------------------------------------------------------
// <copyright file="IPermissions.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.UI.Infrastructure.Abstract
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An interface that permission objects (i.e., objects that control access to files and folders) should implement.
    /// </summary>
    public interface IPermissions
    {
        /// <summary>
        /// Can the user view the section?
        /// </summary>
        /// <returns><c>True</c> if the user has permission to view the object; <c>false</c> otherwise.</returns>
        bool CanView();

        /// <summary>
        /// Can the user edit items in the section?
        /// </summary>
        /// <returns><c>True</c> if the user has permission to edit the object; <c>false</c> otherwise.</returns>
        bool CanEdit();

        /// <summary>
        /// Can the user create items in the section?
        /// </summary>
        /// <returns><c>True</c> if the user has permission to create the object; <c>false</c> otherwise.</returns>
        bool CanCreate();

        /// <summary>
        /// Can the user delete items from the section?
        /// </summary>
        /// <returns><c>True</c> if the user has permission to delete the object; <c>false</c> otherwise.</returns>
        bool CanDelete();
    }
}
