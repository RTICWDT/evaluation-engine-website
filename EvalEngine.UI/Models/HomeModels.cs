//-----------------------------------------------------------------------
// <copyright file="HomeModels.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// The account models.
// </copyright>
//-----------------------------------------------------------------------
namespace EvalEngine.UI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Web.Mvc;
    using System.Web.Security;
    using EvalEngine.Domain.Entities;

    /// <summary>
    /// The help model.
    /// </summary>
    public class HelpModel
    {
        /// <summary>
        /// Gets or sets the subject for help email.
        /// </summary>
        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the category for help email.
        /// </summary>
        [Required]
        [Display(Name = "Category")]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the description for help email.
        /// </summary>
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// The index model.
    /// </summary>
    public class IndexModel
    {
        /// <summary>
        /// Gets or sets the subject for help email.
        /// </summary>
        [Display(Name = "Reports")]
        public List<ReportDisplay> Reports{ get; set; }
    }

    /// <summary>
    /// The report model.
    /// </summary>
    public class ReportDisplay
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Display(Name = "Id")]
        public string Id { get; set; }
    }
}
