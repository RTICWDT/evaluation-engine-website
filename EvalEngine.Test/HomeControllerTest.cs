// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeControllerTest.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// <summary>
//   The home controller test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace EvalEngine.Test
{
    #region

    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using EvalEngine.Domain.Abstract;
    using EvalEngine.UI.Controllers;
    using EvalEngine.UI.Models;
    using Moq;
    using NUnit.Framework;

    #endregion

    /// <summary>
    /// The home controller test.
    /// </summary>
    [TestFixture]
    public class HomeControllerTest
    {
        #region Public Methods and Operators

        /// <summary>
        /// The about.
        /// </summary>
        [Test]
        public void About()
        {
            // Arrange
           /* HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);*/
        }

        /// <summary>
        /// The index.
        /// </summary>
        [Test]
        public void Index()
        {
            // Arrange
           /* HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            ViewDataDictionary viewData = result.ViewData;
            Assert.AreEqual("Welcome to Evaluation Engine.", viewData["Message"]);*/
        }

        #endregion
    }
}