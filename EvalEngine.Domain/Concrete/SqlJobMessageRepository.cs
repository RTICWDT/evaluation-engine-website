// -----------------------------------------------------------------------
// <copyright file="SqlJobMessageRepository.cs" company="RTI, Inc.">
// Job Message repository
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
    /// Job message repository
    /// </summary>
    public class SqlJobMessageRepository : Repository<Job>, IJobMessageRepository
    {
        /// <summary>
        /// The job table
        /// </summary>
        private readonly Table<Job> jobTable;

        /// <summary>
        /// The job status table
        /// </summary>
        private readonly Table<JobStatus> jobStatusTable;

        /// <summary>
        /// The job study id table
        /// </summary>
        private readonly Table<JobStudyId> jobStudyIdTable;

        /// <summary>
        /// The job student id table
        /// </summary>
        private readonly Table<JobStudentId> jobStudentIdTable;


        /// <summary>
        /// Initializes a new instance of the <see cref="SqlJobMessageRepository"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        public SqlJobMessageRepository(string connectionString)
            : base(connectionString)
        {
            DataContext dc = new DataContext(connectionString);
            this.jobTable = dc.GetTable<Job>();
            this.jobStatusTable = dc.GetTable<JobStatus>();
            this.jobStudyIdTable = dc.GetTable<JobStudyId>();
            this.jobStudentIdTable = dc.GetTable<JobStudentId>();
        }

        /// <summary>
        /// The create job and ids
        /// </summary>
        /// <param name="analysis">Analysis objet to create in database</param>
        /// <param name="jobStudyIds">Study Ids to add to database</param>
        public void CreateJobAndIds(Analysis analysis, string[] jobStudentIds)
        {
            Job job = new Job();
            if (this.jobTable.Where(m => m.JobGUID == analysis.JobGUID).Count() > 0)
            {
                job = this.jobTable.Where(m => m.JobGUID == analysis.JobGUID).Single();
            }
            else
            {
                job = new Job
                {
                    StatusId = analysis.StatusId,
                    InterventionStartDate = analysis.InterventionStartDate,
                    //InterventionEndDate = analysis.InterventionEndDate,
                    InterventionGradeLevels = analysis.InterventionGradeLevels,
                    InterventionAreas = analysis.InterventionAreas,
                    OutcomeMeasures = analysis.OutcomeAreas,
                    SubgroupAnalyses = analysis.SubgroupAnalyses,
                    GeneratedOn = analysis.GeneratedOn,
                    CreatedOn = analysis.CreatedOn,
                    JobGUID = analysis.JobGUID,
                    State = analysis.State,
                    OutcomeYears = analysis.YearsOfInterest,
                    DistrictMatch = analysis.DistrictMatch
                };
            }

            if (this.jobStudentIdTable.Where(m => m.JobGUID == analysis.JobGUID).Count() > 0)
            {
                IQueryable<JobStudentId> currentJobIds =    from c in this.jobStudentIdTable
                                 where c.JobGUID == analysis.JobGUID
                                 select c;
                this.jobStudentIdTable.DeleteAllOnSubmit(currentJobIds);
                this.jobStudentIdTable.Context.SubmitChanges();
            }

            //split on commas
            //for each, trim
            List<JobStudentId> listJobStudentIds = jobStudentIds.Select(item => new JobStudentId() { JobGUID = analysis.JobGUID, StudentId = item }).ToList();

            this.jobStudentIdTable.InsertAllOnSubmit(listJobStudentIds);
            this.jobStudentIdTable.Context.SubmitChanges();

            base.Update(job);
        }
    }
}
