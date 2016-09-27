// -----------------------------------------------------------------------
// <copyright file="IStateAssignmentRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Domain.Abstract
{
    #region

    using System.Collections.Generic;
    using System.Linq;
    using EvalEngine.Domain.Entities;

    #endregion

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public interface IStateAssignmentRepository : IRepository<StateAssignment>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The delete state assignment.
        /// </summary>
        /// <param name="assignment">
        /// The assignment.
        /// </param>
        void DeleteStateAssignment(StateAssignment assignment);

        /// <summary>
        /// The delete state assignment for all states and roles.
        /// </summary>
        /// <param name="assignment">
        /// The assignment.
        /// </param>
        void DeleteStateAssignmentForAllStatesAndRoles(StateAssignment assignment);

        /// <summary>
        /// The delete state assignment for all states and roles by username.
        /// </summary>
        /// <param name="userName">
        /// The userName to delete.
        /// </param>
        void DeleteStateAssignmentsByUserName(string userName);

        /// <summary>
        /// The get state assignment by role.
        /// </summary>
        /// <param name="role">
        /// The role.
        /// </param>
        /// <param name="stateid">
        /// The state id.
        /// </param>
        /// <returns>
        /// State assignment for state id and role.
        /// </returns>
        StateAssignment GetStateAssignmentByRole(string role, int stateid);

        /// <summary>
        /// Gets state assignments by username as List
        /// </summary>
        /// <param name="userName">Username to get states for</param>
        /// <returns>List of states</returns>
        List<State> GetStatesByUserName(string userName);

        /// <summary>
        /// The get state assignments by user name.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>
        /// A collection of state assignments.
        /// </returns>
        IQueryable<StateAssignment> GetStateAssignmentsByUserName(string userName);

        /// <summary>
        /// The get state assignment names by user name.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>
        /// List of strings
        /// </returns>
        List<string> GetStateNamesByUserName(string userName);

        /// <summary>
        /// The get state abbreviations names by user name.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>
        /// List of strings
        /// </returns>
        List<string> GetStateAbbrevsByUserName(string userName);

        /// <summary>
        /// The is user name and role assigned to state.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="role">
        /// The role.
        /// </param>
        /// <param name="stateid">
        /// The state id.
        /// </param>
        /// <returns>
        /// true if username and role are assigned to state, false otherwise.
        /// </returns>
        bool IsUserNameAndRoleAssignedToState(string username, string role, int stateid);

        /// <summary>
        /// The is user name assigned to state.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="stateid">
        /// The state id.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        bool IsUserNameAssignedToState(string username, int stateid);

        /// <summary>
        /// The set state assignment.
        /// </summary>
        /// <param name="assignment">
        /// The assignment.
        /// </param>
        void SetStateAssignment(StateAssignment assignment);

        /// <summary>
        /// The submit changes.
        /// </summary>
        void SubmitChanges();

        #endregion
    }
}