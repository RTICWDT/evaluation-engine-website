// -----------------------------------------------------------------------
// <copyright file="IJobResultsRepository.cs" company="RTI, Inc.">
// The job message repository interface.
// </copyright>
// -----------------------------------------------------------------------
namespace EvalEngine.Domain.Abstract
{
    using EvalEngine.Domain.Entities;
    using System.Collections.Generic;
    using System;
    using System.Linq;

    /// <summary>
    /// The job message repository interface.
    /// </summary>
    public interface IJobResultsRepository
    {
        /// <summary>
        /// The get outcomes by Job GUID
        /// </summary>
        /// <param name="jobGUID">
        /// The job GUID.
        /// </param>
        /// <returns>
        /// The list of outcomes by jobGUID
        /// </returns>
        List<JobResults_Outcomes> GetOutcomesByJobGUID(Guid jobGUID);

        /// <summary>
        /// The get job by Job GUID
        /// </summary>
        /// <param name="jobGUID">
        /// The job GUID.
        /// </param>
        /// <returns>
        /// The job by jobGUID
        /// </returns>
        JobResults GetJobResultByJobGUID(Guid jobGUID);

        /// <summary>
        /// The get job by Job GUID
        /// </summary>
        /// <param name="jobGUID">
        /// The job GUID.
        /// </param>
        /// <returns>
        /// The job by jobGUID
        /// </returns>
        Job GetJobByJobGUID(Guid jobGUID);

        /// <summary>
        /// The update notes by job GUID
        /// </summary>
        /// <param name="jobGUID">
        /// The job GUID.
        /// </param>
        /// <param name="rank">
        /// rank order of outcome 
        /// </param>
        /// <param name="note">
        /// chart note
        /// </param>
        void UpdateChartNoteByJobGUID(Guid jobGUID, int rank, string note, string type = "ss");

        /// <summary>
        /// The update notes by job GUID
        /// </summary>
        /// <param name="jobGUID">
        /// The job GUID.
        /// </param>
        /// <param name="rank">
        /// rank order of outcome 
        /// </param>
        /// <param name="note">
        /// table note
        /// </param>
        void UpdateTableNoteByJobGUID(Guid jobGUID, int rank, string note, string type = "ss");

        /// <summary>
        /// Returns status name
        /// </summary>
        /// <param name="id">The id of the job status.</param>
        /// <returns></returns>
        string GetJobStatusNameById(int id);

        /// <summary>
        /// The update notes by job GUID
        /// </summary>
        /// <param name="jobGUID">
        /// The job GUID.
        /// </param>
        /// <param name="rank">
        /// rank order of outcome 
        /// </param>
        /// <param name="note">
        /// chart note
        /// </param>
        string GetChartByJobGUIDAndRank(Guid jobGUID, int rank, string type = "ss");

        /// <summary>
        /// Get balance chart for job.
        /// </summary>
        /// <param name="jobGUID">
        /// <param name="second"></param>
        /// The job GUID.
        /// </param>
        string GetBalanceChartByJobGUID(Guid jobGUID, bool second = false);

        /// <summary>
        /// Inserts mock results into DB.
        /// </summary>
        /// <param name="jobGUID">the jobGUID of the results to insert</param>
        void InsertFakeResults(Guid jobGUID, string stateAbbv);

        /// <summary>
        /// The get jobs by userName
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>
        /// List of jobs by userName
        /// </returns>
        List<Guid> GetJobGUIDsByJobGUIDsAndStatus(List<Guid> guids, int status);

        /// <summary>
        /// The get job results by user ID
        /// </summary>
        /// <param name="analyses">the list of Analyses.</param>
        /// <returns>The job reslts by user ID</returns>
        List<Job> GetJobsByAnalyses(List<Analysis> analyses);

        /// <summary>
        /// Submits changes.
        /// </summary>
        void SubmitChanges();
    }
}
