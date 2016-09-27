// -----------------------------------------------------------------------
// <copyright file="IStateRepository.cs" company="Microsoft">
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
    /// TODO: Update summary.
    /// </summary>
    public interface IStateRepository : IRepository<State>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get list of states.
        /// </summary>
        /// <returns>
        /// The list of states.
        /// </returns>
        List<string> GetListofStates();

        /// <summary>
        /// The get states.
        /// </summary>
        /// <returns>
        /// Collection of states.
        /// </returns>
        IQueryable<State> GetStates();

        /// <summary>
        /// Get ID text for state.
        /// </summary>
        /// <returns>
        /// Text for State Id
        /// </returns>
        string GetStateIdText(string stateName);

        /// <summary>
        /// get state helpful text for step 1
        /// </summary>
        /// <returns>
        /// the text in Helpful Information on Step 1
        /// </returns>
        string GetStateHelpfulText(string stateName);

        /// The get states.
        /// </summary>
        /// <returns>
        /// The System.Linq.IQueryable`1[T -&gt; PIMS.Domain.Entities.State].
        /// </returns>
        int GetStateId(string stateName);

        /// <summary>
        /// get state data text for output
        /// </summary>
        /// <returns>
        /// the state data text for output
        /// </returns>
        string GetStateDataText(string stateName);

        #endregion
    }
}
