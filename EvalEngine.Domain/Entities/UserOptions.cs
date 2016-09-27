// -----------------------------------------------------------------------
// <copyright file="UserOptions.cs" company="Microsoft">
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
    [Table(Name = "UserOptions")]
    public class UserOption : IEntity
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
        /// Gets or sets the state id.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int StateId { get; set; }

        /// <summary>
        /// Gets or sets the rank.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int Rank { get; set; }

        /// <summary>
        /// Gets or sets the section name.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Section { get; set; }

        /// <summary>
        /// Gets or sets the label bit.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public bool IsLabel { get; set; }

        /// <summary>
        /// Gets or sets the header bit.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public bool IsHeader { get; set; }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int ParentId { get; set; }

        /// <summary>
        /// Gets or sets the figure title.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string FigureTitle { get; set; }

        /// <summary>
        /// Gets or sets the outcome title.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string OutcomeTitle { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the portion of the data source pertaining to the outcome.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string DataSource { get; set; }

        /// <summary>
        /// Gets or sets the isPercent field, for formatting.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public bool IsPercent { get; set; }
        #endregion
    }
}
