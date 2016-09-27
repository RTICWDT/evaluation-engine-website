// -----------------------------------------------------------------------
// <copyright file="StateAssignment.cs" company="Microsoft">
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
    [Table(Name = "StateAssignments")]
    public class StateAssignment : IEntity
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the state id.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int StateId { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Year { get; set; }

        #endregion
    }
}
