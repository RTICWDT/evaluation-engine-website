// -----------------------------------------------------------------------
// <copyright file="SqlStateAssignmentRepository.cs" company="RTI, Inc.">
// TODO: Update copyright text.
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
    ///   TODO: Update summary.
    /// </summary>
    public class SqlStateAssignmentRepository : Repository<StateAssignment>, IStateAssignmentRepository
    {
        #region Fields

        /// <summary>
        /// The state assignment repository.
        /// </summary>
        private readonly Table<StateAssignment> stateAssignmentRepository;

        /// <summary>
        /// The state repository.
        /// </summary>
        private readonly Table<State> stateRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlStateAssignmentRepository"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        public SqlStateAssignmentRepository(string connectionString) : base(connectionString)
        {
            DataContext dc = new DataContext(connectionString);
            this.stateAssignmentRepository = dc.GetTable<StateAssignment>();
            this.stateRepository = dc.GetTable<State>();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The delete state assignment.
        /// </summary>
        /// <param name="assignment">The assignment.</param>
        public void DeleteStateAssignment(StateAssignment assignment)
        {
            var deletestateassingment = from d in this.stateAssignmentRepository
                                        where
                                            d.UserName == assignment.UserName && d.StateId == assignment.StateId
                                            && d.Role == assignment.Role && d.Year == assignment.Year
                                        select d;
            this.stateAssignmentRepository.DeleteAllOnSubmit(deletestateassingment);
        }

        /// <summary>
        /// The delete state assignment for all states and roles.
        /// </summary>
        /// <param name="userName">The userName to delete.</param>
        public void DeleteStateAssignmentsByUserName(string userName)
        {
            var deletestateassingment = from d in this.stateAssignmentRepository
                                        where d.UserName == userName
                                        select d;
            this.stateAssignmentRepository.DeleteAllOnSubmit(deletestateassingment);
        }

        /// <summary>
        /// The delete state assignment for all states and roles.
        /// </summary>
        /// <param name="assignment">The assignment.</param>
        public void DeleteStateAssignmentForAllStatesAndRoles(StateAssignment assignment)
        {
            var deletestateassingment = from d in this.stateAssignmentRepository
                                        where d.UserName == assignment.UserName
                                        select d;
            this.stateAssignmentRepository.DeleteAllOnSubmit(deletestateassingment);
        }

        /// <summary>
        /// The get state assignment by role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="stateid">The state id.</param>
        /// <returns>The PIMS.Domain.Entities.StateAssignment.</returns>
        public StateAssignment GetStateAssignmentByRole(string role, int stateid)
        {
            var getstateassingments = from s in this.stateAssignmentRepository
                                      where s.StateId == stateid && s.Role == role
                                      select s;

            if (getstateassingments.Count() == 1)
            {
                return new StateAssignment
                {
                    Role = role,
                    StateId = stateid,
                    UserName = getstateassingments.FirstOrDefault().UserName
                };
            }
            else
            {
                return new StateAssignment { Role = role, StateId = stateid, UserName = string.Empty };
            }
        }

        /// <summary>
        /// The get states by username
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <returns>The list of states by username</returns>
        public List<State> GetStatesByUserName(string userName)
        {
            var stateAssignments = (from s in this.stateAssignmentRepository
                                    join t in this.stateRepository on s.StateId equals t.StateId
                                    where s.UserName == userName
                                    select new State
                                    {
                                        Id = t.Id,
                                        FullName = t.FullName,
                                        StateAbbrev = t.StateAbbrev,
                                        StateId = t.StateId
                                    }).ToList();

            return stateAssignments;
        }

        /// <summary>
        /// The get state assignments by user name.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>A collection of state assignments.</returns>
        public IQueryable<StateAssignment> GetStateAssignmentsByUserName(string userName)
        {
            var stateAssignments = from s in this.stateAssignmentRepository
                                   where s.UserName == userName
                                   select s;

            return stateAssignments;
        }

        /// <summary>
        /// The get state assignments by user name.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>
        /// List of states
        /// </returns>
        public List<string> GetStateNamesByUserName(string userName)
        {
            var stateAssignments = (from s in this.stateAssignmentRepository
                                    join t in this.stateRepository on s.StateId equals t.StateId 
                                   where s.UserName == userName
                                   select t.FullName).ToList();

            return stateAssignments;
        }

        /// <summary>
        /// The get state assignments by user name.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>
        /// List of state abbreviations
        /// </returns>
        public List<string> GetStateAbbrevsByUserName(string userName)
        {
            var stateAssignments = (from s in this.stateAssignmentRepository
                                    join t in this.stateRepository on s.StateId equals t.StateId
                                    where s.UserName == userName
                                    select t.StateAbbrev).ToList();

            return stateAssignments;
        }

        /// <summary>
        /// Is user name and role assigned to state.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="role">The role.</param>
        /// <param name="stateid">The state id.</param>
        /// <returns>The System.Boolean.</returns>
        public bool IsUserNameAndRoleAssignedToState(string username, string role, int stateid)
        {
            var isuserassignedtostate = from s in this.stateAssignmentRepository
                                        where
                                            s.UserName == username && s.StateId == stateid && s.Role == role
                                        select s;
            return isuserassignedtostate.Count() > 0 ? true : false;
        }

        /// <summary>
        /// Is user name and role assigned to state.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="stateid">The state id.</param>
        /// <returns>The System.Boolean.</returns>
        public bool IsUserNameAssignedToState(string username, int stateid)
        {
            var isuserassignedtostate = from s in this.stateAssignmentRepository
                                        where s.UserName == username && s.StateId == stateid
                                        select s;
            return isuserassignedtostate.Count() > 0 ? true : false;
        }

        /// <summary>
        /// The set state assignment.
        /// </summary>
        /// <param name="assignment">The assignment.</param>
        public void SetStateAssignment(StateAssignment assignment)
        {
            this.stateAssignmentRepository.InsertOnSubmit(assignment);
        }

        /// <summary>
        /// The submit changes.
        /// </summary>
        public void SubmitChanges()
        {
            this.stateAssignmentRepository.Context.SubmitChanges();
        }

        #endregion
    }
}
