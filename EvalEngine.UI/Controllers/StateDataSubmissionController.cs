// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StateDataSubmissionController.cs" company="RTI, Inc.">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// <summary>
//   The state data submission controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace EvalEngine.UI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// The state data submission controller.
    /// </summary>
    public class StateDataSubmissionController : Controller
    {
        /// <summary>
        /// The index view method
        /// </summary>
        /// <returns>The index view</returns>
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
