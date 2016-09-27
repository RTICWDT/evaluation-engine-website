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
    [Table(Name = "JobResults_Outcomes")]
    public class JobResults_Outcomes : IEntity
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
        /// Gets or sets the outcome YAML.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string OutcomeYAML { get; set; }

        /// <summary>
        /// Gets or sets the outcome name.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string OutcomeName { get; set; }

        /// <summary>
        /// Gets or sets the outcome header.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string OutcomeHeader { get; set; }

        /// <summary>
        /// Gets or sets the outcome year.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string OutcomeYear { get; set; }

        /// <summary>
        /// Gets or sets the rank (order) for outcomes in the report.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public int Rank { get; set; }

        /// <summary>
        /// Gets or sets the analyst data note.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string DataNote_ScaleScore { get; set; }

        /// <summary>
        /// Gets or sets the analyst data note.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string DataNote_PercentProficient { get; set; }

        /// <summary>
        /// Gets or sets the analyst table note.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string TableNote_ScaleScore { get; set; }

        /// <summary>
        /// Gets or sets the analyst table note.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string TableNote_PercentProficient { get; set; }

        /// <summary>
        /// Gets or sets the chart showing scale score.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Chart_ScaleScore { get; set; }

        /// <summary>
        /// Gets or sets the chart showing percent proficency.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Chart_PercentProficient { get; set; }

        /// <summary>
        /// Gets or sets the table showing scale score.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string YAML_ScaleScore { get; set; }

        /// <summary>
        /// Gets or sets the table showing percent proficency.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string YAML_PercentProficient { get; set; }

        /// <summary>
        /// Gets or sets the outcome note.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string OutcomeNote { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string Title { get; set; }

         /// <summary>  
         /// Gets or sets the chart values for scale score.  
         /// </summary>  
        [Column(UpdateCheck = UpdateCheck.Never)]  
        public string ChartValues_ScaleScore_Intervention { get; set; }

        /// <summary>  
        /// Gets or sets the chart values for scale score.  
        /// </summary>  
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ChartValues_ScaleScore_Control { get; set; }

        /// <summary>  
        /// Gets or sets the chart values for percent proficient.
        /// </summary>  
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ChartValues_PercentProficient_Intervention { get; set; }

        /// <summary>  
        /// Gets or sets the chart values for percent proficient.
        /// </summary>  
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ChartValues_PercentProficient_Control { get; set; }

        /// <summary>  
        /// Gets or sets the per grade averages YAML.
        /// </summary>  
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string PerGrade_Averages_YAML { get; set; }

        /// <summary>  
        /// Gets or sets the difference in chart values for main effect.
        /// </summary>  
        [Column(UpdateCheck = UpdateCheck.Never)]
        public double? ChartValues_Main_Difference { get; set; }

        /// <summary>  
        /// Gets or sets the difference in chart values for proficiency.
        /// </summary>  
        [Column(UpdateCheck = UpdateCheck.Never)]
        public double? ChartValues_Proficiency_Difference { get; set; }

        /// <summary>  
        /// Gets or sets the ordinal averages yaml field.
        /// </summary>  
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string OrdinalAverages { get; set; }

        /// <summary>  
        /// Gets or sets the subgroup note field.
        /// </summary>  
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string SubgroupNote { get; set; }

        /// <summary>
        /// Gets or sets the data note.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string DataNote { get; set; }

        /// <summary>
        /// Gets or sets the error messages.
        /// </summary>
        [Column(UpdateCheck = UpdateCheck.Never)]
        public string ErrorMessages { get; set; }

    }
}
