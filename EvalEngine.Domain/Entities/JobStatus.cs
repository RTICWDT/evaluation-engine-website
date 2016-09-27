// -----------------------------------------------------------------------
// <copyright file="JobStatus.cs" company="RTI, Inc.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Domain.Entities
{
    #region
    using System;
    using System.Configuration;
    using System.Data.Linq.Mapping;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using EvalEngine.Domain.Abstract;

    #endregion

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    [Table(Name = "JobStatuses")]
    public class JobStatus : IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the status id.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Name { get; set; }
    }
}
