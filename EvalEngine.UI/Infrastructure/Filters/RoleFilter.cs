// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleFilter.cs" company="MPR INC">
//   Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace EvalEngine.UI.Infrastructure.Filters
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using EvalEngine.UI.Extensions;

    /// <summary>
    /// A class modeling a filter that restricts access to sections of the application based on the user's roles.
    /// </summary>
    public class RoleFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets the roles with access.
        /// </summary>
        /// <value>
        /// The roles with access.
        /// </value>
        public string[] RolesWithAccess { get; set; }

        /// <summary>
        /// Gets or sets the action to redirect users without access.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the controller to redirect users without access.
        /// </summary>
        /// <value>
        /// The controller.
        /// </value>
        public string Controller { get; set; }

        /// <summary>
        /// Gets or sets the message to be placed in TempData.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the key in resource file.
        /// </summary>
        /// <value>
        /// The key in resource file.
        /// </value>
        public string KeyInResourceFile { get; set; }

        /// <summary>
        /// Called by the ASP.NET MVC framework after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // do nothing
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userRoles = filterContext.HttpContext.User.GetRoles();
            if (userRoles == null)
            {
                userRoles = new string[] { };
            }

            if (string.IsNullOrEmpty(this.Controller) || string.IsNullOrEmpty(this.Action))
            {
                this.Controller = "Home";
                this.Action = "Index";
            }

            var matchOnRolesWithAccess = this.RolesWithAccess.Intersect(userRoles);
            if (matchOnRolesWithAccess.Count() == 0 || userRoles.Length == 0)
            {
                filterContext.Controller.TempData["message"] = this.GetMessage();
                var routeDictionary = new RouteValueDictionary { { "action", this.Action }, { "controller", this.Controller } };
                filterContext.Result = new RedirectToRouteResult(routeDictionary);
            }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <returns>The string to be shown to the user.</returns>
        private string GetMessage()
        {
            var defaultMessage = Resources.UserFeedbackMessages.NotAuthorizedInSiteSection;
            if (!string.IsNullOrEmpty(this.Message))
            {
                defaultMessage = this.Message;
            }

            if (string.IsNullOrEmpty(this.KeyInResourceFile))
            {
                return defaultMessage;
            }
            else
            {
                try
                {
                    var manager = new System.Resources.ResourceManager(typeof(Resources.UserFeedbackMessages));
                    var message = manager.GetString(this.KeyInResourceFile);
                    return message;
                }
                catch (Exception)
                {
                    return defaultMessage;
                }
            }
        }
    }
}