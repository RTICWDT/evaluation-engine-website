// -----------------------------------------------------------------------
// <copyright file="Analysis.cs" company="RTI, Inc.">
// The analysis entity.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Domain.Entities
{
    using System;
    using System.Data.Linq.Mapping;
    using EvalEngine.Domain.Abstract;

    /// <summary>
    ///  The analysis table
    /// </summary>
    [Table(Name = "Analyses")]
    public class Analysis : IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier to use
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string UseIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the identifier list
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string IdentifierList { get; set; }

        /// <summary>
        /// Gets or sets the identifier file
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string IdentifierFile { get; set; }

        /// <summary>
        /// Gets or sets the study name
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string StudyName { get; set; }

        /// <summary>
        /// Gets or sets the study description
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string StudyDescription { get; set; }

        /// <summary>
        /// Gets or sets the analysis name
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string AnalysisName { get; set; }

        /// <summary>
        /// Gets or sets the analysis description
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string AnalysisDescription { get; set; }

        /// <summary>
        /// Gets or sets intervention start date
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime InterventionStartDate { get; set; }

        /// <summary>
        /// Gets or sets intervention end date
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime InterventionEndDate { get; set; }

        /// <summary>
        /// Gets or sets the intervention grade levels
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
        public string OutcomeAreas { get; set; }

        /// <summary>
        /// Gets or sets the list of years of interest.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string YearsOfInterest { get; set; }

        /// <summary>
        /// Gets or sets the list of subgroup analyses.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string SubgroupAnalyses { get; set; }

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
        /// Gets or sets the state abbreviation
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the status of the analysis
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int StatusId { get; set; }

        /// <summary>
        /// Gets or sets the created on date
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string UserEmail { get; set; }

        /// <summary>
        /// Gets or sets the district match enum.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int DistrictMatch { get; set; }
    }
}
