// -----------------------------------------------------------------------
// <copyright file="PasswordHistory.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Domain.Entities
{
    #region

    using System;
    using System.Data.Linq.Mapping;
    using EvalEngine.Domain.Abstract;

    #endregion

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    [Table(Name = "PasswordHistory")]
    public class PasswordHistory : IEntity
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the hashed password
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the last used DateTime
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime LastUsed { get; set; }

        /// <summary>
        /// Gets or sets username
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public Guid UserId { get; set; }

        #endregion
    }
}
