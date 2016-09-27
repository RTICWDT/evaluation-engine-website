namespace EvalEngine.UI.Models
{
    #region

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///   The view state assignment model.
    /// </summary>
    public class StateAssignmentModel
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        ///   Gets or sets the state_ id.
        /// </summary>
        public int StateId { get; set; }

        #endregion
    }

    /// <summary>
    ///   The user selection.
    /// </summary>
    public class UserSelection
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        #endregion
    }

    /// <summary>
    ///   The edit state assignment model.
    /// </summary>
    public class EditStateAssignmentModel
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the selected year.
        /// </summary>
        public string SelectedYear { get; set; }

        /// <summary>
        ///   Gets or sets the state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        ///   Gets or sets the state_ id.
        /// </summary>
        public int StateId { get; set; }

        #endregion
    }

    /// <summary>
    ///   The view state assignments model.
    /// </summary>
    public class ViewStateAssignmentsModel
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the edit state assignments.
        /// </summary>
        public EditStateAssignmentModel EditStateAssignments { get; set; }

        /// <summary>
        ///   Gets or sets the selected year.
        /// </summary>
        public string SelectedYear { get; set; }

        /// <summary>
        ///   Gets or sets the view state assignmets.
        /// </summary>
        public List<StateAssignmentModel> ViewStateAssignmets { get; set; }

        /// <summary>
        ///   Gets or sets the years.
        /// </summary>
        public IEnumerable<string> Years { get; set; }

        #endregion
    }
}
