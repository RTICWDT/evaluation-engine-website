// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorController.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// http://blog.janjonas.net/2011-12-11/asp-net-mvc3-custom-error-pages-non-ajax-requests-jquery-ajax-requests
// <summary>
//   The error controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace EvalEngine.UI.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    /// <summary>
    /// The error controller.
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// The index error page
        /// </summary>
        /// <returns>InternalServerError view</returns>
        public ActionResult Index()
        {
            return this.InternalServerError();
        }

        /// <summary>
        /// The results not found error page
        /// </summary>
        /// <returns>NotFound view</returns>
        public ActionResult NotFound()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return this.View("NotFound");
        }

        /// <summary>
        /// The internal server error page
        /// </summary>
        /// <returns>InternalServerError view</returns>
        public ActionResult InternalServerError()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return this.View("InternalServerError");
        }

        /// <summary>
        /// The permission denied page.
        /// </summary>
        /// <returns>InternalServerError view</returns>
        public ActionResult PermissionDenied()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return this.View("PermissionDenied");
        }

        /// <summary>
        /// The report not ready page.
        /// </summary>
        /// <returns>InternalServerError view</returns>
        public ActionResult ReportNotReady()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return this.View("ReportNotReady");
        }
    }
}
