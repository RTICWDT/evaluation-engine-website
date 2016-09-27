// -----------------------------------------------------------------------
// <copyright file="JobResults_Outcomes.cs" company="RTI, Inc.">
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
    [Table(Name = "JobResults")]
    public class JobResults : IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the job GUID.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public Guid JobGUID { get; set; }

        /// <summary>
        /// Gets or sets the job YAML.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string YAML { get; set; }

        /// <summary>
        /// Gets or sets the job image
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the job title.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the job intro text.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string IntroText { get; set; }

        /// <summary>
        /// Gets or sets the treatment count.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int TreatmentCount { get; set; }

        /// <summary>
        /// Gets or sets the treatment excluded count.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int? TreatmentExcludedCount { get; set; }

        /// <summary>
        /// Gets or sets the control count.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int ControlCount { get; set; }

        /// <summary>
        /// Gets or sets the balance table.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string BalanceTable { get; set; }

        /// <summary>
        /// Gets or sets the school model box plot.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string SchoolModelBoxPlot { get; set; }

        /// <summary>
        /// Gets or sets the inclusive main box plot.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string InclusiveMainBoxPlot { get; set; }

        /// <summary>
        /// Gets or sets the main box plot.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string MainBoxPlot { get; set; }

        /// <summary>
        /// Gets or sets the histogram.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Histogram { get; set; }

        /// <summary>
        /// Gets or sets the intervention plot.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string InterventionPlot { get; set; }

        /// <summary>
        /// Gets or sets the control plot.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ControlPlot { get; set; }

        /// <summary>
        /// Gets or sets the active plot.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ActivePlot { get; set; }

        /// <summary>
        /// Gets or sets the within district matches plot.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string WithinDistrictMatchesPct { get; set; }


        /// <summary>
        /// Gets or sets the balance main P value.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string BalanceMainPval { get; set; }

        /// <summary>
        /// Gets or sets the balance inclusive P value.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string BalanceInclusivePval { get; set; }

        /// <summary>
        /// Gets or sets the supplemental information.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string SupplementalInformation { get; set; }

        /// <summary>
        /// Gets or sets the active plot.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string BalancePlotSensitivity { get; set; }
    }
}
