// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NinjectDependencyResolver.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// <summary>
//   TODO: Update summary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EvalEngine.UI.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.Mvc;
    using Ninject;
    using Ninject.Syntax;
    using EvalEngine.Domain.Abstract;
    using EvalEngine.Domain.Concrete;
    using EvalEngine.UI.Infrastructure.Abstract;


    /// <summary>
    /// A class in charge of resolving dependencies.
    /// </summary>
    public class NinjectDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// The kernel
        /// </summary>
        private IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectDependencyResolver"/> class.
        /// </summary>
        public NinjectDependencyResolver()
        {
            this.kernel = new StandardKernel();
            this.AddBindings();
        }

        /// <summary>
        /// Gets the kernel.
        /// </summary>
        /// <value>
        /// The kernel.
        /// </value>
        public IKernel Kernel
        {
            get { return this.kernel; }
        }

        /// <summary>
        /// Resolves singly registered services that support arbitrary object creation.
        /// </summary>
        /// <param name="serviceType">The type of the requested service or object.</param>
        /// <returns>
        /// The requested service or object.
        /// </returns>
        public object GetService(Type serviceType)
        {
            return this.kernel.TryGet(serviceType);
        }

        /// <summary>
        /// Resolves multiply registered services.
        /// </summary>
        /// <param name="serviceType">The type of the requested services.</param>
        /// <returns>
        /// The requested services.
        /// </returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.kernel.GetAll(serviceType);
        }

        /// <summary>
        /// Binds this instance.
        /// </summary>
        /// <typeparam name="T">The type of object to bind.</typeparam>
        /// <returns>The target of a binding.</returns>
        public IBindingToSyntax<T> Bind<T>()
        {
            return this.kernel.Bind<T>();
        }

        /// <summary>
        /// Adds the bindings.
        /// </summary>
        private void AddBindings()
        {
            var EEConnectionString = ConfigurationManager.ConnectionStrings["EvalEngineConnectionString"].ToString();
            var EEMessagesConnectionString = ConfigurationManager.ConnectionStrings["EEMessengerConnectionString"].ToString();

            this.Bind<IStateAssignmentRepository>().To<SqlStateAssignmentRepository>().WithConstructorArgument("connectionString", EEConnectionString);
            this.Bind<IAnalysesRepository>().To<SqlAnalysesRepository>().WithConstructorArgument("connectionString", EEConnectionString);
            this.Bind<IPasswordHistoryRepository>().To<SqlPasswordHistoryRepository>().WithConstructorArgument("connectionString", EEConnectionString);
            this.Bind<IStateAssignmentRepository>().To<SqlStateAssignmentRepository>().WithConstructorArgument("connectionString", EEConnectionString);
            this.Bind<IStateRepository>().To<SqlStateRepository>().WithConstructorArgument("connectionString", EEConnectionString);
            this.Bind<IUserAccountInfoRepository>().To<SqlUserAccountInfoRepository>().WithConstructorArgument("connectionString", EEConnectionString);

            this.Bind<IJobMessageRepository>().To<SqlJobMessageRepository>().WithConstructorArgument("connectionString", EEMessagesConnectionString);
            this.Bind<IJobResultsRepository>().To<SqlJobResultsRepository>().WithConstructorArgument("connectionString", EEMessagesConnectionString);
        }
    }
}