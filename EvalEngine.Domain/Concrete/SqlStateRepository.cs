// -----------------------------------------------------------------------
// <copyright file="SqlStateRepository.cs" company="RTI, Inc.">
// State repository
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Domain.Concrete
{
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using EvalEngine.Domain.Abstract;
    using EvalEngine.Domain.Entities;

    /// <summary>
    /// State repository class
    /// </summary>
    public class SqlStateRepository : Repository<State>, IStateRepository
    {
        #region Fields

        /// <summary>
        /// The state table.
        /// </summary>
        private readonly Table<State> stateRepository;

        #endregion

        #region Public Methods and Operators
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlStateRepository"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        public SqlStateRepository(string connectionString) : base(connectionString)
        {
            DataContext dc = new DataContext(connectionString);
            this.stateRepository = dc.GetTable<State>();
        }

        /// <summary>
        /// The get list of states.
        /// </summary>
        /// <returns>The list of states.</returns>
        public List<string> GetListofStates()
        {
            return (from s in this.stateRepository select s.FullName).ToList();
        }

        /// <summary>
        /// The get states.
        /// </summary>
        /// <returns>Collection of states.</returns>
        public IQueryable<State> GetStates()
        {
            return from s in this.stateRepository where s.IncludedInProject == true select s;
        }

        /// <summary>
        /// The get states.
        /// </summary>
        /// <param name="stateName">Name of the state.</param>
        /// <returns>Text for State Id</returns>
        public string GetStateIdText(string stateName)
        {
            return (from s in this.stateRepository where s.StateAbbrev == stateName select s.StateIdText).Single().ToString();
        }

        /// <summary>
        /// get state helpful text for step 1
        /// </summary>
        /// <param name="stateName">Name of the state.</param>
        /// <returns>the text in Helpful Information on Step 1</returns>
        public string GetStateHelpfulText(string stateName)
        {
            return (from s in this.stateRepository where s.StateAbbrev == stateName select s.HelpfulText).Single().ToString();
        }

        /// <summary>
        /// get state data text for output
        /// </summary>
        /// <param name="stateName">Name of the state.</param>
        /// <returns>the state data text for output</returns>
        public string GetStateDataText(string stateName)
        {
            return (from s in this.stateRepository where s.StateAbbrev == stateName select s.DataDescription).Single().ToString();
        }

        /// <summary>
        /// Gets the state identifier.
        /// </summary>
        /// <param name="stateName">Name of the state.</param>
        /// <returns>System.Int32.</returns>
        public int GetStateId(string stateName)
        {
            return (from s in this.stateRepository where s.StateAbbrev == stateName select s.StateId).Single();
        }

        #endregion
    }
}
