// -----------------------------------------------------------------------
// <copyright file="Job.cs" company="RTI, Inc.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Domain.Entities
{
    #region
    using System;
    using System.Configuration;
    using System.Data.Linq.Mapping;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using EvalEngine.Domain.Abstract;

    #endregion

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    [Table(Name = "Jobs")]
    public class Job : IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the status id.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int StatusId { get; set; }

        /// <summary>
        /// Gets or sets the salt
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime InterventionStartDate { get; set; }

        /// <summary>
        /// Gets or sets the salt
        /// </summary>
        //[Column(UpdateCheck = UpdateCheck.Never)]
        //public DateTime InterventionEndDate { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string InterventionGradeLevels { get; set; }

        /// <summary>
        /// Gets or sets the list of intervention areas.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string InterventionAreas { get; set; }

        /// <summary>
        /// Gets or sets the list of outcome areas.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string OutcomeMeasures { get; set; }

        /// <summary>
        /// Gets or sets the list of subgroup analyses.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string SubgroupAnalyses { get; set; }

        /// <summary>
        /// Gets or sets the created on date
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the generated-on date
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime GeneratedOn { get; set; }

        /// <summary>
        /// Gets or sets the job GUID
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public Guid JobGUID { get; set; }

        /// <summary>
        /// Gets or sets the state name.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the state name.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ServerNotes { get; set; }

        /// <summary>
        /// Gets or sets the list of years for outcomes.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string OutcomeYears { get; set; }

        /// <summary>
        /// Gets or sets the list of years for outcomes.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int DistrictMatch { get; set; }

        /// <summary>
        /// Gets or sets the state name.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ErrorMessages { get; set; }
    }
}
