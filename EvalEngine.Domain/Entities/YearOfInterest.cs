// -----------------------------------------------------------------------
// <copyright file="YearOfInterest.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Domain.Entities
{
    #region

    using System.Data.Linq.Mapping;
    using EvalEngine.Domain.Abstract;
    using System;

    #endregion

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    [Table(Name = "YearsOfInterest")]
    public class YearOfInterest : IEntity
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the state abbreviation.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string StateAbbv { get; set; }

        /// <summary>
        /// Gets or sets the state abbreviation.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime DefaultStartDate { get; set; }

        #endregion
    }
}
