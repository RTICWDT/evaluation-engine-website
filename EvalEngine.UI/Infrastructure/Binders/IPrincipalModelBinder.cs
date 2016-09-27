// -----------------------------------------------------------------------
// <copyright file="IPrincipalModelBinder.cs" company="MPR INC">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.UI.Infrastructure.Binders
{
    using System;
    using System.Security.Principal;
    using System.Web.Mvc;

    /// <summary>
    /// A model binder that makes it possible to pass an IPrincipal object as an argument to the controller actions.
    /// </summary>
    public class IPrincipalModelBinder : IModelBinder
    {
        /// <summary>
        /// See: http://www.hanselman.com/blog/IPrincipalUserModelBinderInASPNETMVCForEasierTesting.aspx
        /// This model binder makes it so that methods in the controller can automatically be passed the user as an argument.
        /// The goal of doing this is to facilitate testing.
        /// </summary>
        /// <param name="controllerContext">A ControllerContext object.</param>
        /// <param name="bindingContext">A ModelBindingContext object.</param>
        /// <returns>An IPrincipal object (the user object).</returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            IPrincipal p = controllerContext.HttpContext.User;
            return p;
        }
    }
}