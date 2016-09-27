// -----------------------------------------------------------------------
// <copyright file="DIFilterProvider.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.UI.Infrastructure.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Resolves the dependencies that occur in the filters.
    /// </summary>
    public class DIFilterProvider : FilterAttributeFilterProvider
    {
        /// <summary>
        /// Returns filters with all of their dependencies resolved.
        /// </summary>
        /// <param name="controllerContext">The ControllerContext for the filters.</param>
        /// <param name="actionDescriptor">An ActionDescriptor object.</param>
        /// <returns>An IEnumerable of filters.</returns>
        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            IEnumerable<Filter> filters = base.GetFilters(controllerContext, actionDescriptor);

            NinjectDependencyResolver dependencyResolver = DependencyResolver.Current as NinjectDependencyResolver;

            if (dependencyResolver == null)
            {
                dependencyResolver = new NinjectDependencyResolver();
            }

            foreach (Filter filter in filters)
            {
                dependencyResolver.Kernel.Inject(filter.Instance);
            }

            return filters;
        }
    }
}