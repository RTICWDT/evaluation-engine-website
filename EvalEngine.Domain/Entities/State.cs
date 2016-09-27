// -----------------------------------------------------------------------
// <copyright file="State.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Domain.Entities
{
    #region

    using System.Data.Linq.Mapping;
    using EvalEngine.Domain.Abstract;

    #endregion

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    [Table(Name = "States")]
    public class State : IEntity
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the state abbreviation.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string StateAbbrev { get; set; }

        /// <summary>
        /// Gets or sets the state id text.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string StateIdText { get; set; }

        /// <summary>
        /// Gets or sets the state helpful text.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string HelpfulText { get; set; }

        /// <summary>
        /// Gets or sets the state abbreviation.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public bool IncludedInProject { get; set; }

        /// <summary>
        /// Gets or sets the state_ id.
        /// </summary>
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int StateId { get; set; }

        /// <summary>
        /// Gets or sets the portion of the data source pertaining to the state.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string DataSource { get; set; }

        /// <summary>
        /// Gets or sets the state data text for output.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string DataDescription { get; set; }

        #endregion
    }
}
