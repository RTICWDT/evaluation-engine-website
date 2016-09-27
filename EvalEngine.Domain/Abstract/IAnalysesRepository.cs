// -----------------------------------------------------------------------
// <copyright file="IAnalysesRepository.cs" company="RTI, Inc.">
// The interface for the Analyses repository,
// </copyright>
// -----------------------------------------------------------------------
namespace EvalEngine.Domain.Abstract
{
    using EvalEngine.Domain.Entities;
    using System.Linq;
    using System.Collections.Generic;
    using System;

    /// <summary>
    /// The interface for the Analyses repository
    /// </summary>
    public interface IAnalysesRepository : IRepository<Analysis>
    {
        /// <summary>
        /// The create analysis
        /// </summary>
        /// <param name="userName">
        /// The analysis record to create
        /// </param>
        /// <returns>the id of the created Analysis</returns>
        int CreateAnalysis(string userName, string useremail);

        /// <summary>
        /// The get analysis by username
        /// </summary>
        /// <param name="userName">
        /// The analysis record to get
        /// </param>
        /// <returns>Current Analysis for userName</returns>
        Analysis GetCurrentAnalysisByUserName(string userName);

        /// <summary>
        /// Check if user is owner of analysis
        /// </summary>
        /// <param name="userName">
        /// The username to test
        /// </param>
        /// <param name="id">
        /// The analysis ID to test
        /// </param> 
        /// <returns>true if userName is the owner of Analysis</returns>
        bool IsOwnerOfAnalysis(string userName, int id);

        /// <summary>
        /// Get years of interest for state
        /// </summary>
        /// <param name="stateAbbv">
        /// The state to get years for.
        /// </param>
        /// <returns>true if userName is the owner of Analysis</returns>
        IQueryable<YearOfInterest> GetYearsOfInterest(string stateAbbv);

        /// <summary>
        /// Gets earliest start date for state.
        /// </summary>
        /// <param name="state">state</param>
        /// <returns>Collection of years</returns>
        DateTime GetDefaultStartDate(string state);

        /// <summary>
        /// Tests if analysis contains year.
        /// </summary>
        /// <param name="year">year</param>
        /// <param name="id">id</param>
        /// <returns>true if analysis contains year</returns>
        bool DoesAnalysisContainYear(string year, int id);

        /// <summary>
        /// Get Top Done Analyses by Username
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="n">
        /// number of analyses to return
        /// </param>
        /// <returns>IQuerybable of analyses</returns>
        List<Analysis> GetTopDoneAnalysesByUserName(string userName, int n);

        /// <summary>
        /// Get Top Done Analyses by job GUIDs
        /// </summary>
        /// <param name="jobGUIDs">
        /// The job guids
        /// </param>
        /// <param name="n">
        /// number of analyses to return
        /// </param>
        /// <returns>IQuerybable of analyses</returns>
        List<Analysis> GetTopDoneAnalysesByJobGUIDs(List<Guid> jobGUIDs, int n);

        /// <summary>
        /// Get Guids by Username
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>IQuerybable of analyses</returns>
        List<Guid> GetGUIDsByUserName(string userName);

        /// <summary>
        /// Gets user options for state, by section.
        /// </summary>
        /// <param name="state">state</param>
        /// <returns>IQueryable of years</returns>
        IQueryable<UserOption> GetUserOptionsBySection(int stateId, string section);

        /// <summary>
        /// Gets the figure title portion.
        /// </summary>
        /// <param name="state">state</param>
        /// <param name="value">value</param>
        /// <returns>the figure title</returns>
        string GetFigureTitleByStateAndValue(int stateId, string value);

        /// <summary>
        /// Gets the outcome title portion.
        /// </summary>
        /// <param name="state">state</param>
        /// <param name="value">value</param>
        /// <returns>the outcome title</returns>
        string GetOutcomeTitleByStateAndValue(int stateId, string value);

        /// <summary>
        /// Gets the subject.
        /// </summary>
        /// <param name="state">state</param>
        /// <param name="value">value</param>
        /// <returns>the subject</returns>
        string GetSubjectByStateAndValue(int stateId, string value);

        /// <summary>
        /// Gets the outcome data source.
        /// </summary>
        /// <param name="state">state</param>
        /// <param name="value">value</param>
        /// <returns>the outcome data source</returns>
        string GetOutcomeDataSourceByStateAndValue(int stateId, string value);

        /// <summary>
        /// Gets the state data source
        /// </summary>
        /// <param name="state">state</param>
        /// <param name="value">value</param>
        /// <returns>the state data source</returns>
        string GetStateDataSourceByStateAndValue(int stateId);

        /// <summary>
        /// Gets the label from user options.
        /// </summary>
        /// <param name="stateId">The state id</param>
        /// <param name="value">The value</param>
        /// <returns></returns>
        string GetLabelByStateAndValue(int stateId, string value);

        /// <summary>
        /// Gets the IsPercent field.
        /// </summary>
        /// <param name="stateId">The state identifier.</param>
        /// <param name="value">value</param>
        /// <returns>true if the outcome is formatted as a percent, false otherwise.</returns>
        bool IsOutcomePercentByStateAndValue(int stateId, string value);

        /// <summary>
        /// Gets the datasource related to the outcome
        /// </summary>
        /// <param name="stateId">The state identifier.</param>
        /// <returns>the datasource</returns>
        List<Analysis> GetAnalysesByUserId(Guid userId);

        /// <summary>
        /// Gets the analysis id by JobGUID.
        /// </summary>
        /// <param name="jobGUID">The job guid.</param>
        /// <returns>the analysis id</returns>
        string GetAnalysisIdByJobGUID(Guid JobGUID);

        /// <summary>
        /// Gets the analysis id by JobGUID.
        /// </summary>
        /// <param name="jobGUID">The job guid.</param>
        /// <returns>the analysis id</returns>
        string GetAnalysisNameByJobGUID(Guid JobGUID);

        /// <summary>
        /// Gets the analysis by JobGUID.
        /// </summary>
        /// <param name="jobGUID">The job guid.</param>
        /// <returns>the analysis</returns>
        Analysis GetAnalysisByJobGUID(Guid jobGUID);
    }
}
