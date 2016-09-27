// -----------------------------------------------------------------------
// <copyright file="IEntity.cs" company="RTI, Inc.">
// Standard entity interface.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Domain.Abstract
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Entity interface
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets the id of entity.
        /// </summary>
        int Id { get; }
    }
}