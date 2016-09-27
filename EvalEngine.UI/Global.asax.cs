// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// <summary>
//   The Global.asax.cs file
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EvalEngine.UI
{
    using System.Security.Principal;
    using System.Web.Mvc;
    using System.Web.Routing;
    using EvalEngine.Infrastructure;
    using EvalEngine.UI.Infrastructure;
    using EvalEngine.UI.Infrastructure.Binders;
    using EvalEngine.UI.Infrastructure.Filters;
    using EvalEngine.UI.Filters;

    //// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    //// visit http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// The MvcApplication class
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Registers global filters
        /// </summary>
        /// <param name="filters">
        /// GlobalFilterCollection filters, the collection of global filters
        /// </param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CheckUserActionFilterAttribute());
        }

        /// <summary>
        /// Registers a collection of routes
        /// </summary>
        /// <param name="routes">
        /// RouteCollection routes, a collection of routes
        /// </param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }); //// Parameter 
        }

        /// <summary>
        /// Handles registering filters, registering routes, controller building and model binding
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders[typeof(IPrincipal)] = new IPrincipalModelBinder();

            MvcHandler.DisableMvcResponseHeader = true;


            DependencyResolver.SetResolver(new NinjectDependencyResolver());

            FilterProviders.Providers.Clear();
            FilterProviders.Providers.Add(new DIFilterProvider());


            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
        }
    }
}