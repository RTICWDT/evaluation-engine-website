// -----------------------------------------------------------------------
// <copyright file="SqlAnalysesRepository.cs" company="RTI, Inc.">
// The analyses repository.
// </copyright>
// -----------------------------------------------------------------------
namespace EvalEngine.Domain.Concrete
{
    using System;
    using System.Data.Linq;
    using System.Data.SqlTypes;
    using System.Linq;
    using EvalEngine.Domain.Abstract;
    using EvalEngine.Domain.Entities;
    using System.Collections.Generic;

    /// <summary>
    /// The analyses repository.
    /// </summary>
    public class SqlAnalysesRepository : Repository<Analysis>, IAnalysesRepository
    {
        /// <summary>
        /// The analyses table.
        /// </summary>
        private readonly Table<Analysis> analysesTable;

        /// <summary>
        /// The years table.
        /// </summary>
        private readonly Table<YearOfInterest> yearsTable;

        /// <summary>
        /// The user option table.
        /// </summary>
        private readonly Table<UserOption> userOptionTable;

        /// <summary>
        /// The state table.
        /// </summary>
        private readonly Table<State> stateTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlAnalysesRepository"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        public SqlAnalysesRepository(string connectionString)
            : base(connectionString)
        {
            DataContext dc = new DataContext(connectionString);
            this.analysesTable = dc.GetTable<Analysis>();
            this.yearsTable= dc.GetTable<YearOfInterest>();
            this.userOptionTable = dc.GetTable<UserOption>();
            this.stateTable = dc.GetTable<State>();
        }

        /// <summary>
        /// The create user account info
        /// </summary>
        /// <param name="username">
        /// The username to create analysis for
        /// </param>
        /// <returns>the id of the newly created analysis record</returns>
        public int CreateAnalysis(string username, string useremail)
        {
            Analysis analysis = new Analysis
            {
                UseIdentifier = "state",
                IdentifierList = null,
                IdentifierFile = null,
                StudyName = null,
                StudyDescription = null,
                AnalysisName = null,
                AnalysisDescription = null,
                InterventionStartDate = (DateTime)SqlDateTime.MinValue,
                InterventionEndDate = (DateTime)SqlDateTime.MinValue,
                InterventionGradeLevels = null,
                InterventionAreas = null,
                OutcomeAreas = "",
                SubgroupAnalyses = "",
                UserName = username,
                GeneratedOn = (DateTime)SqlDateTime.MinValue,
                StatusId = 1,
                CreatedOn = DateTime.Now,
                JobGUID = Guid.NewGuid(),
                State = null,
                YearsOfInterest = "",
                UserEmail = useremail,
                DistrictMatch = 0
            };

            base.Update(analysis);
            return analysis.Id;
        }

        /// <summary>
        /// The get user account info
        /// </summary>
        /// <param name="userName">
        /// The username to get analysis for
        /// </param>
        /// <returns>most recent Analysis object for username</returns>
        public Analysis GetCurrentAnalysisByUserName(string userName)
        {
            var analysis = this.analysesTable.FirstOrDefault(x => x.UserName.Equals(userName));
            return analysis;
        }

        /// <summary>
        /// Check if user is owner of analysis
        /// </summary>
        /// <param name="userName">username to check</param>
        /// <param name="id">id to check</param>
        /// <returns>true if username owns Analysis indicated by id</returns>
        public bool IsOwnerOfAnalysis(string userName, int id)
        {
            var analysis = from a in this.analysesTable
                           where a.Id == id && a.UserName == userName
                           select a;
            return analysis.Count() > 0 ? true : false;
        }

        /// <summary>
        /// Gets years of interest for state.
        /// </summary>
        /// <param name="state">state</param>
        /// <returns>Collection of years</returns>
        public IQueryable<YearOfInterest> GetYearsOfInterest(string state)
        {
            return from s in this.yearsTable where s.StateAbbv == state select s;
        }

        /// <summary>
        /// Gets earliest start date for state.
        /// </summary>
        /// <param name="state">state</param>
        /// <returns>Collection of years</returns>
        public DateTime GetDefaultStartDate(string state)
        {
            var date = from s in this.yearsTable where s.StateAbbv == state orderby s.Value ascending select s.DefaultStartDate;

            return date.First();
        }

        /// <summary>
        /// Tests if analysis contains year.
        /// </summary>
        /// <param name="year">year</param>
        /// <param name="id">id</param>
        /// <returns>true if analysis contains year</returns>
        public bool DoesAnalysisContainYear(string year, int id)
        {
            var analysis = from a in this.analysesTable
                           where a.Id == id && a.YearsOfInterest.Contains(year)
                           select a;
            return analysis.Count() > 0 ? true : false;
        }

        /// <summary>
        /// Get Top Done Analyses by Username
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="n">
        /// number of analyses to return
        /// </param>
        /// <returns>Collection of analyses</returns>
        public List<Analysis> GetTopDoneAnalysesByUserName(string userName, int n)
        {
            var jobs = (from a in this.analysesTable
                        where a.UserName == userName && a.StatusId == 3
                        select a).Take(n).ToList();

            return jobs;
        }

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
        public List<Analysis> GetTopDoneAnalysesByJobGUIDs(List<Guid> jobGUIDs, int n)
        {
            var jobs = (from a in this.analysesTable
                        where jobGUIDs.Contains(a.JobGUID)
                        orderby a.CreatedOn descending
                        select a).Take(n).ToList();

            return jobs;
        }

        /// <summary>
        /// Get Guids by Username
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>Collection of analyses</returns>
        public List<Guid> GetGUIDsByUserName(string userName)
        {
            var jobs = (from a in this.analysesTable
                        where a.UserName == userName
                        select a.JobGUID).ToList();

            return jobs;
        }

        /// <summary>
        /// Gets user options for state, by section.
        /// </summary>
        /// <param name="state">state</param>
        /// <returns>Collection of years</returns>
        public IQueryable<UserOption> GetUserOptionsBySection(int stateId, string section)
        {
             var options = from s in this.userOptionTable orderby s.Rank ascending where s.StateId == stateId && s.Section == section select s;

             return options;
        }

        /// <summary>
        /// Gets the figure title.
        /// </summary>
        /// <param name="stateid">stateId</param>
        /// <param name="value">value</param>
        /// <returns>the figure title</returns>
        public string GetFigureTitleByStateAndValue(int stateId, string value)
        {
            string title = (from s in this.userOptionTable orderby s.Rank ascending where s.StateId == stateId && s.Value == value select s.FigureTitle).SingleOrDefault();

            return (title == null) ? "" : title;
        }

        /// <summary>
        /// Gets the outcome title.
        /// </summary>
        /// <param name="stateId">The state identifier.</param>
        /// <param name="value">value</param>
        /// <returns>the outcome title</returns>
        public string GetOutcomeTitleByStateAndValue(int stateId, string value)
        {
            string title = (from s in this.userOptionTable orderby s.Rank ascending where s.StateId == stateId && s.Value == value select s.OutcomeTitle).SingleOrDefault();

            return (title == null) ? "" : title;
        }

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <param name="stateId">The state id</param>
        /// <param name="value">value</param>
        /// <returns>the label</returns>
        public string GetLabelByStateAndValue(int stateId, string value)
        {
            string title = (from s in this.userOptionTable orderby s.Rank ascending where s.StateId == stateId && s.Value == value select s.Label).SingleOrDefault();

            return (title == null) ? "" : title;
        }

        /// <summary>
        /// Gets the subject
        /// </summary>
        /// <param name="stateid">stateId</param>
        /// <param name="value">value</param>
        /// <returns>the subject</returns>
        public string GetSubjectByStateAndValue(int stateId, string value)
        {
            string title = (from s in this.userOptionTable orderby s.Rank ascending where s.StateId == stateId && s.Value == value select s.Subject).SingleOrDefault();

            return (title == null) ? "" : title.Trim();
        }

        /// <summary>
        /// Gets the datasource related to the outcome
        /// </summary>
        /// <param name="stateId">The state identifier.</param>
        /// <param name="value">value</param>
        /// <returns>the datasource</returns>
        public string GetOutcomeDataSourceByStateAndValue(int stateId, string value)
        {
            string title = (from s in this.userOptionTable orderby s.Rank ascending where s.StateId == stateId && s.Value == value select s.DataSource).SingleOrDefault();

            return (title == null) ? "" : ", " + title;
        }

        /// <summary>
        /// Gets the IsPercent field.
        /// </summary>
        /// <param name="stateId">The state identifier.</param>
        /// <param name="value">value</param>
        /// <returns>true if the outcome is formatted as a percent, false otherwise.</returns>
        public bool IsOutcomePercentByStateAndValue(int stateId, string value)
        {
            var result = (from s in this.userOptionTable orderby s.Rank ascending where s.StateId == stateId && s.Value == value && s.IsPercent == true select s);

            return (result.Count() != 1) ? false : true;
        }

        /// <summary>
        /// Gets the datasource related to the outcome
        /// </summary>
        /// <param name="stateId">The state identifier.</param>
        /// <returns>the datasource</returns>
        public string GetStateDataSourceByStateAndValue(int stateId)
        {
            string title = (from s in this.stateTable where s.StateId == stateId  select s.DataSource).SingleOrDefault();

            return (title == null) ? "" : title;
        }

        /// <summary>
        /// Gets the datasource related to the outcome
        /// </summary>
        /// <param name="stateId">The state identifier.</param>
        /// <returns>the datasource</returns>
        public List<Analysis> GetAnalysesByUserId(Guid userId)
        {
            List<Analysis> things = (from s in this.analysesTable
                                     where s.UserName == userId.ToString()
                                     orderby s.CreatedOn descending
                                     select s).ToList();
            return things;
        }

        /// <summary>
        /// Gets the analysis id by JobGUID.
        /// </summary>
        /// <param name="jobGUID">The job guid.</param>
        /// <returns>the analysis id</returns>
        public string GetAnalysisIdByJobGUID(Guid jobGUID)
        {
            Analysis a = (from s in this.analysesTable
                                     where s.JobGUID == jobGUID
                                     select s).First();
            return a.Id.ToString();
        }

        /// <summary>
        /// Gets the analysis id by JobGUID.
        /// </summary>
        /// <param name="jobGUID">The job guid.</param>
        /// <returns>the analysis id</returns>
        public string GetAnalysisNameByJobGUID(Guid jobGUID)
        {
            Analysis a = (from s in this.analysesTable
                          where s.JobGUID == jobGUID
                          select s).First();
            return a.AnalysisName.ToString();
        }

        /// <summary>
        /// Gets the analysis id by JobGUID.
        /// </summary>
        /// <param name="jobGUID">The job guid.</param>
        /// <returns>the analysis id</returns>
        public Analysis GetAnalysisByJobGUID(Guid jobGUID)
        {
            Analysis a = (from s in this.analysesTable
                          where s.JobGUID == jobGUID
                          select s).First();
            return a;
        }
    }
}
