// -----------------------------------------------------------------------
// <copyright file="StateAssignmentBuilder.cs" company="RTI, Inc.">
// State assignment builder
// </copyright>
// -----------------------------------------------------------------------
namespace EvalEngine.Test.TestObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Domain.Entities;

    /// <summary>
    /// Creates an instance of a user
    /// </summary>
    public class StateAssignmentBuilder
    {
        private int id = 0;

        private string role = Constants.ProjectAdminRole;

        private int stateId = 1;

        private string userName = "Alex";

        public StateAssignmentBuilder WithProjectUserRole()
        {
            this.role = Constants.ProjectUserRole;
            return this;
        }

        public StateAssignmentBuilder WithStateAdminRole()
        {
            this.role = Constants.StateAdminRole;
            return this;
        }

        public StateAssignmentBuilder WithStateId(int stateId)
        {
            this.stateId = stateId;
            return this;
        }

        public StateAssignmentBuilder WithRole(string role)
        {
            this.role = role;
            return this;
        }

        public StateAssignment Build()
        {
            return new StateAssignment { Id = this.id, Role = this.role, StateId = this.stateId, UserName = this.userName };
        }
    }
}
