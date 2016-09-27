// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalysisModels.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// <summary>
//   The analysis view models.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace EvalEngine.UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using System.Globalization;
    using System.Web;

    /// <summary>
    /// The Step1 model
    /// </summary>
    public class Step1Model
    {
        /// <summary>
        /// Gets or sets a value indicating whether the user has state ids to provide
        /// </summary>
        [Required(ErrorMessage = "State student IDs are required.")]
        [Display(Name = "Has State ID")]
        public bool HasStateID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has district ids to provide
        /// </summary>
        [Display(Name = "Has District ID")]
        public bool HasDistrictID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has school ids to provide
        /// </summary>
        [Display(Name = "Has School ID")]
        public bool HasSchoolID { get; set; }

        /// <summary>
        /// Gets or sets the state
        /// </summary>
        [Required(ErrorMessage = "Please select a state.")]
        [Display(Name = "State")]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the list of states.
        /// </summary>
        [Display(Name = "RadioState")]
        public List<CheckboxItem> RadioStates { get; set; }

        /// <summary>
        /// gets or sets the helpful information
        /// </summary>
        [Display(Name = "Helpful Information")]
        public string HelpfulInfo { get; set; }
    }

    /// <summary>
    /// The Step1B model
    /// </summary>
    [ChooseThisOrThat("File", "ParticipantText", ErrorMessage = "Please upload a file or paste information into the box.")]
    public class Step1BModel
    {
        /// <summary>
        /// Gets or sets the participant file (intervention group)
        /// </summary>
        [Display(Name = "Participant File")]
        public HttpPostedFileBase File { get; set; }

        /// <summary>
        /// Gets or sets the participant file (intervention group)
        /// </summary>
        [Display(Name = "Participant File")]
        public string ParticipantFile { get; set; }

        /// <summary>
        /// gets or sets the participant text (intervention group)
        /// </summary>
        [Display(Name = "Participant Text")]
        public string ParticipantText { get; set; }
    }

    /// <summary>
    /// The Step2 model
    /// </summary>
    public class Step2Model
    {
        /// <summary>
        /// Gets or sets the study name
        /// </summary>
        [Required(ErrorMessage = "Intervention name is required.")]
        [Display(Name = "Study Name")]
        public string StudyName { get; set; }

        /// <summary>
        /// Gets or sets the study description
        /// </summary>
        [Display(Name = "Study Description")]
        public string StudyDescription { get; set; }
    }

    /// <summary>
    /// The Step2B model
    /// </summary>
    public class Step2BModel
    {
        /// <summary>
        /// Gets or sets the analysis name
        /// </summary>
        [Required(ErrorMessage = "Analysis name is required.")]
        [Display(Name = "Analysis Name")]
        public string AnalysisName { get; set; }

        /// <summary>
        /// Gets or sets the analysis description
        /// </summary>
        [Display(Name = "Analysis Description")]
        public string AnalysisDescription { get; set; }

        /// <summary>
        ///   Gets or sets the grade levels
        /// </summary>
        /// <value> The grade levels. </value>
        [Display(Name = "Grade Levels")]
        [CheckList(1, false, "At least one grade level is required.")]
        public List<CheckboxItem> GradeLevels { get; set; }

        /// <summary>
        ///   Gets or sets the grade level
        /// </summary>
        /// <value> The grade level. </value>
        [Display(Name = "Grade Level")]
        public string GradeLevel { get; set; }

        /// <summary>
        ///   Gets or sets the intervention areas
        /// </summary>
        /// <value> The intervention areas. </value>
        //[Display(Name = "Intervention Areas")]
        //public List<CheckboxItem> InterventionAreas { get; set; }

        /// <summary>
        /// Gets or sets the intervention start date
        /// </summary>
        [Display(Name = "Program start date")]
        public DateTime InterventionStartDate { get; set; }

        /// <summary>
        /// Gets or sets the intervention end date
        /// </summary>
        [Display(Name = "Program end date")]
        public DateTime InterventionEndDate { get; set; }

        /// <summary>
        ///   Gets or sets the outcome measures
        /// </summary>
        /// <value> The outcome measures. </value>
        [CheckList(1, false, "At least one outcome measure is required.")]
        [Display(Name = "Outcome Measures")]
        public List<CheckboxItem> OutcomeMeasures { get; set; }

        /// <summary>
        ///   Gets or sets the outcome measures
        /// </summary>
        /// <value> The outcome measures. </value>
        [Display(Name = "Outcome Measures")]
        public List<UserOutcomeOption> OutcomeItems { get; set; }

        /// <summary>
        ///   Gets or sets the years of interest
        /// </summary>
        /// <value> The years of interest. </value>
        [CheckList(1, false, "At least one year of interest is required.")]
        [Display(Name = "Year of Interest")]
        public List<CheckboxItem> YearsOfInterest { get; set; }

        /// <summary>
        ///   Gets or sets the subgroup analyses
        /// </summary>
        /// <value> The subgroup analyses. </value>
        [Display(Name = "Subgroup Analyses")]
        public List<CheckboxItem> SubgroupAnalyses { get; set; }

        /// <summary>
        ///   Gets or sets the district matching preference
        /// </summary>
        /// <value> The district matching preference value. </value>
        [Display(Name = "District Matching Preference")]
        public int DistrictMatch { get; set; }
    }

    /// <summary>
    /// The Step2B model
    /// </summary>
    public class Step2CModel
    {
        /// <summary>
        ///   Gets or sets the district matching preference
        /// </summary>
        /// <value> The district matching preference value. </value>
        [Display(Name = "District Matching Preference")]
        public int DistrictMatch { get; set; }
    }

    public class Step4Model
    {
        /// <summary>
        ///   Gets or sets the report model.
        /// </summary>
        /// <value> The report to display. </value>
        [Display(Name = "Report")]
        public ReportModel Report { get; set; }
    }

    public class LibraryItem
    {
        /// <summary>
        ///   Gets or sets the report model.
        /// </summary>
        /// <value> The report to display. </value>
        [Display(Name = "State")]
        public string State{ get; set; }

        /// <summary>
        ///   Gets or sets the report model.
        /// </summary>
        /// <value> The report to display. </value>
        [Display(Name = "JobGUID")]
        public string JobGUID { get; set; }

        /// <summary>
        ///   Gets or sets the report model.
        /// </summary>
        /// <value> The report to display. </value>
        [Display(Name = "Status")]
        public string Status { get; set; }

        /// <summary>
        ///   Gets or sets the report model.
        /// </summary>
        /// <value> The report to display. </value>
        [Display(Name = "Link")]
        public string Link { get; set; }

        /// <summary>
        ///   Gets or sets the report model.
        /// </summary>
        /// <value> The report to display. </value>
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets the params for the report.
        /// </summary>
        /// <value> The parameters to display. </value>
        [Display(Name = "Params")]
        public string Params { get; set; }
    }

    public class ErrorModel
    {
        /// <summary>
        /// Gets or sets the list of reports.
        /// </summary>
        [Display(Name = "Error")]
        public string Error { get; set; }
    }

    public class LibraryModel
    {
        /// <summary>
        /// Gets or sets the list of reports.
        /// </summary>
        [Display(Name = "Reports")]
        public List<LibraryItem> Reports { get; set; }
    }

    /// <summary>
    ///   CheckboxItem Class
    /// </summary>
    public class CheckboxItem
    {
        /// <summary>
        ///   Gets or sets a value indicating whether this instance is checked.
        /// </summary>
        /// <value> <c>true</c> if this instance is checked; otherwise, <c>false</c> . </value>
        public bool Checked { get; set; }

        /// <summary>
        ///   Gets or sets the checkbox value.
        /// </summary>
        /// <value> The checkbox value. </value>
        public string Value { get; set; }

        /// <summary>
        ///   Gets or sets the checkbox label.
        /// </summary>
        /// <value> The checkbox label. </value>
        public string Label { get; set; }
    }

    /// <summary>
    ///   UserOutcomeOption Class
    /// </summary>
    public class UserOutcomeOption
    {
        /// <summary>
        ///   Gets or sets the id.
        /// </summary>
        /// <value> The id.</value>
        public int Id { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance is checked.
        /// </summary>
        /// <value> <c>true</c> if this instance is checked; otherwise, <c>false</c> . </value>
        public bool Checked { get; set; }

        /// <summary>
        ///   Gets or sets the outcome value.
        /// </summary>
        /// <value> The outcome value. </value>
        public string Value { get; set; }

        /// <summary>
        ///   Gets or sets the outcome label.
        /// </summary>
        /// <value> The outcome label. </value>
        public string Label { get; set; }

        /// <summary>
        ///   Gets or sets the outcome section.
        /// </summary>
        /// <value> The outcome section. </value>
        public string Section { get; set; }

        /// <summary>
        ///   Gets or sets the rank.
        /// </summary>
        /// <value> The rank. </value>
        public int Rank { get; set; }

        /// <summary>
        ///   Gets or sets the islabel bit.
        /// </summary>
        /// <value> The isLabel bool. </value>
        public bool isLabel{ get; set; }

        /// <summary>
        ///   Gets or sets the isHeader bit.
        /// </summary>
        /// <value> The isHeader bool. </value>
        public bool isHeader { get; set; }

        /// <summary>
        ///   Gets or sets the parentId.
        /// </summary>
        /// <value> The parentId. </value>
        public int parentId { get; set; }
    }

    /// <summary>
    ///   PropertiesMustMatchAttribute, forces proerties to match.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class ChooseThisOrThatAttribute : ValidationAttribute
    {
        /// <summary>
        /// The default error message.
        /// </summary>
        private const string DefaultErrorMessage = "'{0}' or '{1}' has not been selected.";

        /// <summary>
        /// The _type id.
        /// </summary>
        private readonly object typeId = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesMustMatchAttribute"/> class.
        /// </summary>
        /// <param name="originalProperty">
        /// The original property. 
        /// </param>
        /// <param name="confirmProperty">
        /// The confirm property. 
        /// </param>
        public ChooseThisOrThatAttribute(string originalProperty, string confirmProperty)
            : base(DefaultErrorMessage)
        {
            this.OriginalProperty = originalProperty;
            this.ConfirmProperty = confirmProperty;
        }

        /// <summary>
        ///   Gets the confirm property.
        /// </summary>
        public string ConfirmProperty { get; private set; }

        /// <summary>
        ///   Gets the original property.
        /// </summary>
        public string OriginalProperty { get; private set; }

        /// <summary>
        ///   Gets the original property.
        /// </summary>
        public string customErrorMessage { get; private set; }

        /// <summary>
        ///   When implemented in a derived class, gets a unique identifier for this <see cref="T:System.Attribute" />.
        /// </summary>
        /// <returns> An <see cref="T:System.Object" /> that is a unique identifier for the attribute. </returns>
        public override object TypeId
        {
            get
            {
                return this.typeId;
            }
        }

        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">
        /// The name to include in the formatted message. 
        /// </param>
        /// <returns>
        /// An instance of the formatted error message. 
        /// </returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(
                CultureInfo.CurrentUICulture, this.ErrorMessageString, this.OriginalProperty, this.ConfirmProperty);
        }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">
        /// The value of the object to validate. 
        /// </param>
        /// <returns>
        /// true if the specified value is valid; otherwise, false. 
        /// </returns>
        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(this.OriginalProperty, true /* ignoreCase */).GetValue(value);
            object confirmValue = properties.Find(this.ConfirmProperty, true /* ignoreCase */).GetValue(value);
            if (originalValue != null && confirmValue == null) 
                return true;
            else if (originalValue == null && confirmValue != null)
                return true;
            else
                return false;
        }
    }
}