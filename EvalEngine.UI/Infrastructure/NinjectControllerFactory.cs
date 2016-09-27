//-----------------------------------------------------------------------
// <copyright file="NinjectControllerFactory.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// The controller factory for dependancy injection.
// </copyright>
//-----------------------------------------------------------------------

namespace EvalEngine.Infrastructure
{
    using System;
    using System.Configuration;
    using System.Web.Mvc;
    using System.Web.Routing;
    using EvalEngine.Domain.Abstract;
    using EvalEngine.Domain.Concrete;
    using EvalEngine.Infrastructure.Abstract;
    using EvalEngine.Infrastructure.Concrete;
    using Ninject;

    /// <summary>
    /// NinjectControllerFactory - Creates binding for controllers in the application
    /// </summary>
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// A factory that will create controllers.
        /// </summary>
        private IKernel ninjectKernel;

        /// <summary>
        /// Initializes a new instance of the NinjectControllerFactory class.
        /// </summary>
        public NinjectControllerFactory()
        {
            this.ninjectKernel = new StandardKernel();
            this.AddBindings();
        }

        /// <summary>
        /// Gets an instance of the requested controller.
        /// </summary>
        /// <param name="requestContext">The RequestContext for the controller.</param>
        /// <param name="controllerType">The Type of controller we are requesting.</param>
        /// <returns>An object that implments the IController interface.</returns>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)this.ninjectKernel.Get(controllerType);
        }

        /// <summary>
        /// Use Ninject to generate bindings.
        /// </summary>
        private void AddBindings()
        {
            // binding for the logger
            this.ninjectKernel.Bind<ILogger>().To<NLogLogger>().WithConstructorArgument("currentClassName", x => x.Request.ParentContext.Request.Service.FullName);

            // put additional bindings here
            this.ninjectKernel.Bind<IStateAssignmentRepository>().To<SqlStateAssignmentRepository>().WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["EvalEngineConnectionString"].ToString());
            this.ninjectKernel.Bind<IStateRepository>().To<SqlStateRepository>().WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["EvalEngineConnectionString"].ToString());
            this.ninjectKernel.Bind<IUserAccountInfoRepository>().To<SqlUserAccountInfoRepository>().WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["EvalEngineConnectionString"].ToString());
            this.ninjectKernel.Bind<IPasswordHistoryRepository>().To<SqlPasswordHistoryRepository>().WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["EvalEngineConnectionString"].ToString()).WithConstructorArgument("numberOfGenerations", Convert.ToInt32(ConfigurationManager.AppSettings["PasswordNumberOfGenerations"].ToString()));
            this.ninjectKernel.Bind<IAnalysesRepository>().To<SqlAnalysesRepository>().WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["EvalEngineConnectionString"].ToString());
            this.ninjectKernel.Bind<IJobMessageRepository>().To<SqlJobMessageRepository>().WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["EEMessengerConnectionString"].ToString());
            this.ninjectKernel.Bind<IJobResultsRepository>().To<SqlJobResultsRepository>().WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["EEMessengerConnectionString"].ToString());
        }
    }
}