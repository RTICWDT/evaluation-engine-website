// -----------------------------------------------------------------------
// <copyright file="IJobMessageRepository.cs" company="RTI, Inc.">
// The job message repository interface.
// </copyright>
// -----------------------------------------------------------------------
namespace EvalEngine.Domain.Abstract
{
    using EvalEngine.Domain.Entities;

    /// <summary>
    /// The job message repository interface.
    /// </summary>
    public interface IJobMessageRepository : IRepository<Job>
    {
        /// <summary>
        /// Creates job from Analysis and adds jobStudyIds
        /// </summary>
        /// <param name="analysis">Analysis to send to message database</param>
        /// <param name="jobStudyIds">List of Ids to send to message database</param>
        void CreateJobAndIds(Analysis analysis, string[] jobStudentIds);
    }
}
