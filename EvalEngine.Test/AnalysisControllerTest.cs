// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalysisControllerTest.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// <summary>
//   The analysis controller test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace EvalEngine.Test
{
    using System;
    using System.Security.Principal;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Xml;
    using EvalEngine.Domain.Abstract;
    using EvalEngine.Domain.Entities;
    using EvalEngine.Infrastructure.Abstract;
    using EvalEngine.UI.Controllers;
    using Moq;
    using NUnit.Framework;
    using System.Data.SqlTypes;
    using EvalEngine.UI.Models;
    using System.Web;
    using System.IO;
    using System.Text;
    using System.Resources;
    using System.Reflection;
    using System.Configuration;
    using System.Web.Routing;

    /// <summary>
    /// The analysis controller test.
    /// </summary>
    [TestFixture]
    class AnalysisControllerTest
    {
        /// <summary>
        /// The mock state.
        /// </summary>
        private Mock<IStateRepository> mockState = new Mock<IStateRepository>();

        /// <summary>
        /// The mock state assignment.
        /// </summary>
        private Mock<IStateAssignmentRepository> mockStateAssignment = new Mock<IStateAssignmentRepository>();

        /// <summary>
        /// Mock http context base
        /// </summary>
        private Mock<HttpContextBase> mockContext;

        private Mock<ILogger> mockLogger = new Mock<ILogger>();

        private Mock<IAnalysesRepository> mockAnalysis = new Mock<IAnalysesRepository>();

        private Mock<IJobMessageRepository> mockMessages = new Mock<IJobMessageRepository>();

        private Mock<IJobResultsRepository> mockResults = new Mock<IJobResultsRepository>();

        private Mock<IPrincipal> mockUser = new Mock<IPrincipal>();

        //private Mock<ControllerContext> controllerContext = new Mock<ControllerContext>();

        [SetUp]
        public void SetUp()
        {
           string testingFolder = @"C:\GEE\EvalEngine\EvalEngine.Test\TextFixtures\";

           Analysis testAnalysis = new Analysis
            {
                UseIdentifier = "state",
                IdentifierList = null,
                IdentifierFile = null,
                StudyName = null,
                StudyDescription = null,
                AnalysisName = null,
                AnalysisDescription = null,
                InterventionStartDate = (DateTime)SqlDateTime.MinValue,
                InterventionEndDate = (DateTime)SqlDateTime.MinValue,
                InterventionGradeLevels = null,
                InterventionAreas = null,
                OutcomeAreas = null,
                SubgroupAnalyses = null,
                UserName = "asmith@mprinc.com",
                GeneratedOn = (DateTime)SqlDateTime.MinValue,
                StatusId = 0,
                CreatedOn = DateTime.Now,
                JobGUID = Guid.NewGuid(),
                State = null
            };

           Mock<IStateAssignmentRepository> mockStateAssignment = new Mock<IStateAssignmentRepository>();
           Mock<IStateRepository> mockState = new Mock<IStateRepository>();
           Mock<IAnalysesRepository> mockAnalysis = new Mock<IAnalysesRepository>();
           Mock<IJobMessageRepository> mockMessages = new Mock<IJobMessageRepository>();
           Mock<IJobResultsRepository> mockResults = new Mock<IJobResultsRepository>();
           Mock<ILogger> mockLogger = new Mock<ILogger>();

           this.mockAnalysis.Setup(a => a.GetById(1)).Returns(testAnalysis);
           this.mockAnalysis.Setup(a => a.IsOwnerOfAnalysis("asmith@mprinc.com", 1)).Returns(true);
           this.mockAnalysis.Setup(a => a.IsOwnerOfAnalysis("asmith@mprinc.com", 2)).Returns(false);

           this.mockStateAssignment.Setup(a => a.GetStateNamesByUserName("asmith@mprinc.com")).Returns(new System.Collections.Generic.List<string>() { "New Mexico" });

           this.mockUser.Setup(p => p.IsInRole("State Administrator")).Returns(true);
           this.mockUser.SetupGet(x => x.Identity.Name).Returns("asmith@mprinc.com");

           /*Mock<ControllerContext> controllerContext = new Mock<ControllerContext>();
           this.controllerContext.SetupGet(x => x.HttpContext.User).Returns(mockUser.Object);*/

           Mock<HttpServerUtilityBase> mockServer = new Mock<HttpServerUtilityBase>();
           mockServer.Setup(s => s.MapPath(It.IsAny<string>())).Returns(testingFolder);
            var mockResponse = new Mock<HttpResponseBase>(MockBehavior.Strict);
            var mockRequest = new Mock<HttpRequestBase>(MockBehavior.Strict);

            mockContext = new Mock<HttpContextBase>();
            mockContext.SetupGet(x => x.Request).Returns(mockRequest.Object);
            mockContext.SetupGet(x => x.Response).Returns(mockResponse.Object);
            mockContext.SetupGet(x => x.Server).Returns(mockServer.Object);
            mockContext.SetupGet(x => x.User).Returns(mockUser.Object);


        }

        AnalysisController GetAnalysisController()
        {
            AnalysisController ac = new AnalysisController(mockLogger.Object, mockStateAssignment.Object, mockState.Object, mockAnalysis.Object, mockMessages.Object, mockResults.Object);
            ac.ControllerContext = new ControllerContext(mockContext.Object, new RouteData(), ac);
            return ac;
        }

        /// <summary>
        /// The step1 method returns a view
        /// </summary>
        [Test]
        [Ignore("Ignore a test")]
        public void Step1_Returns_View()
        {
            // Arrange
            AnalysisController analysis = GetAnalysisController();
            // Act
            ActionResult view = analysis.Step1(analysis.ControllerContext.HttpContext.User);

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), view);
            
        }

        /// <summary>
        /// The step1 method returns a view
        /// </summary>
        [Test]
        [Ignore("Ignore a test")]
        public void Step1_Submit_Returns_View()
        {
            AnalysisController analysis = GetAnalysisController();

            var st1 = new State { FullName = "New Mexico", Id = 1, StateAbbrev = "NM", StateId = 1 };
            var list = new System.Collections.Generic.List<State>() { st1 };

            Step1Model model = new Step1Model {
                HasSchoolID = false,
                HasDistrictID = false,
                HasStateID =  true,
                RadioStates = list.ConvertAll(
                        i => new CheckboxItem { Label = "New Mexico", Value = "NM", Checked = false }),
                State = "New Mexico"
            };

            // Act
            var action = (RedirectToRouteResult)analysis.Step1("Submit", model, analysis.ControllerContext.HttpContext.User, 1);
            // Assert
            Assert.IsInstanceOf(typeof(ActionResult), action);
            //Assert.AreEqual(action.RouteValues["action"], "Step1b");
            //Assert.AreEqual(action.RouteValues["controller"], "Analysis");

            var result = (RedirectToRouteResult)analysis.Step1("Back", model, analysis.ControllerContext.HttpContext.User, 1);
            // Assert
            //Assert.AreEqual(result.RouteValues["action"], "Index");
            //Assert.AreEqual(result.RouteValues["controller"], "Home");
        }

        /// <summary>
        /// The step1 method returns a view
        /// </summary>
        [Test]
        public void Step1B_Returns_View()
        {
            // Arrange
            AnalysisController analysis = GetAnalysisController();

            // Act
            ActionResult view = analysis.Step1b(analysis.ControllerContext.HttpContext.User, 1);

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), view);

        }

        /// <summary>
        /// The step1B method returns a view
        /// </summary>
        [Test]
        [Ignore("Ignore a test")]
        public void Step1B_Submit_Returns_View()
        {
            // Arrange
            AnalysisController analysis = GetAnalysisController();
            
            var st1 = new State { FullName = "New Mexico", Id = 1, StateAbbrev = "NM", StateId = 1 };
            var list = new System.Collections.Generic.List<State>() { st1 };

            var uploadFile = GetMockUploadFile("UploadTestFile");
            
            Step1BModel model = new Step1BModel
            {
                ParticipantFile = "C:\fakedir\fake\fakefile.csv",
                ParticipantText = null

            };

            // Act
            var action = (RedirectToRouteResult)analysis.Step1b("Submit", model, uploadFile, analysis.ControllerContext.HttpContext.User, 1);
            // Assert
            Assert.IsInstanceOf(typeof(ActionResult), action);
            Assert.AreEqual(action.RouteValues["action"], "Step2");
            Assert.AreEqual(action.RouteValues["controller"], "Analysis");

            action = (RedirectToRouteResult)analysis.Step1b("Submit", model, uploadFile, analysis.ControllerContext.HttpContext.User, 2);
            // Assert
            Assert.IsInstanceOf(typeof(ActionResult), action);
            Assert.AreEqual(action.RouteValues["action"], "PermissionDenied");
            Assert.AreEqual(action.RouteValues["controller"], "Error");

            var result = (RedirectToRouteResult)analysis.Step1b("Back", model, uploadFile, analysis.ControllerContext.HttpContext.User, 1);
            // Assert
            Assert.AreEqual(result.RouteValues["action"], "Step1");
            Assert.AreEqual(result.RouteValues["controller"], "Analysis");
            Assert.AreEqual(result.RouteValues["id"], 1);

            model.ParticipantFile = null;
            result = (RedirectToRouteResult)analysis.Step1b("Submit", model, null, analysis.ControllerContext.HttpContext.User, 1);
            // Assert
            Assert.AreEqual(result.RouteValues["action"], "Step1b");
            Assert.AreEqual(result.RouteValues["controller"], "Analysis");
            Assert.AreEqual(result.RouteValues["id"], 1);
        }

        /// <summary>
        /// The step2 method returns a view
        /// </summary>
        [Test]
        public void Step2_Get_Returns_View()
        {
            // Arrange
            AnalysisController analysis = GetAnalysisController();

            // Act
            ActionResult view = analysis.Step2(analysis.ControllerContext.HttpContext.User, 1);

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), view);
        }

        /// <summary>
        /// The step2 method returns a view
        /// </summary>
        [Test]
        public void Step2_Submit_Returns_View()
        {
            // Arrange
            AnalysisController analysis = GetAnalysisController();

            Step2Model model = new Step2Model
            {
                StudyName = "Name",
                StudyDescription = "Desc"
            };

            // Act
            RedirectToRouteResult action = (RedirectToRouteResult)analysis.Step2("Submit", model, analysis.ControllerContext.HttpContext.User, 1);

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult), action);
            Assert.AreEqual(action.RouteValues["action"], "Step2b");
            Assert.AreEqual(action.RouteValues["controller"], "Analysis");

            action = (RedirectToRouteResult)analysis.Step2("Submit", model, analysis.ControllerContext.HttpContext.User, 2);

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult), action);
            Assert.AreEqual(action.RouteValues["action"], "PermissionDenied");
            Assert.AreEqual(action.RouteValues["controller"], "Error");

            action = (RedirectToRouteResult)analysis.Step2("Back", model, analysis.ControllerContext.HttpContext.User, 2);

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult), action);
            Assert.AreEqual(action.RouteValues["action"], "Step1b");
            Assert.AreEqual(action.RouteValues["controller"], "Analysis");
        }

        /// <summary>
        /// The step2 method returns a view
        /// </summary>
        [Test]
        public void Step2b_Get_Returns_View()
        {
            // Arrange
            AnalysisController analysis = GetAnalysisController();

            // Act
            ActionResult view = analysis.Step2b(analysis.ControllerContext.HttpContext.User, 1);

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), view);
        }

        /// <summary>
        /// The step2 method returns a view
        /// </summary>
        [Test]
        [Ignore("Ignore a test")]
        public void Step2b_Submit_Returns_View()
        {
            // Arrange
            AnalysisController analysis = GetAnalysisController();

            Step2BModel model = new Step2BModel
            {
                AnalysisDescription = "Desc",
                AnalysisName = "Name",
                GradeLevels = new System.Collections.Generic.List<CheckboxItem>() { new CheckboxItem { Label = "k", Value = "k", Checked = false }, new CheckboxItem { Label = "1", Value = "1", Checked = false }, new CheckboxItem { Label = "2", Value = "2", Checked = false }, new CheckboxItem { Label = "3", Value = "3", Checked = false }, new CheckboxItem { Label = "4", Value = "4", Checked = false }, new CheckboxItem { Label = "5", Value = "5", Checked = false }, new CheckboxItem { Label = "6", Value = "6", Checked = false }, new CheckboxItem { Label = "7", Value = "7", Checked = false }, new CheckboxItem { Label = "8", Value = "8", Checked = false }, new CheckboxItem { Label = "9", Value = "9", Checked = true }, new CheckboxItem { Label = "10", Value = "10", Checked = true }, new CheckboxItem { Label = "11", Value = "11", Checked = true }, new CheckboxItem { Label = "12", Value = "12", Checked = true } },
                //InterventionAreas = new System.Collections.Generic.List<CheckboxItem> { new CheckboxItem { Label = "english", Value = "english", Checked = true }, new CheckboxItem { Label = "math", Value = "math", Checked = false }, new CheckboxItem { Label = "grad", Value = "grad", Checked = false }, new CheckboxItem { Label = "hs", Value = "hs", Checked = false }, new CheckboxItem { Label = "college", Value = "college", Checked = false }, new CheckboxItem { Label = "other", Value = "other", Checked = false } },
                InterventionStartDate = (DateTime)SqlDateTime.MinValue,
                InterventionEndDate = (DateTime)SqlDateTime.MinValue,
                OutcomeMeasures = new System.Collections.Generic.List<CheckboxItem> { new CheckboxItem { Label = "english", Value = "english", Checked = true }, new CheckboxItem { Label = "math", Value = "math", Checked = false }, new CheckboxItem { Label = "grad", Value = "grad", Checked = false }, new CheckboxItem { Label = "hs", Value = "hs", Checked = false }, new CheckboxItem { Label = "college", Value = "college", Checked = false }, new CheckboxItem { Label = "other", Value = "other", Checked = false } },
                SubgroupAnalyses = new System.Collections.Generic.List<CheckboxItem> { new CheckboxItem { Label = "gender", Value = "gender", Checked = true}, new CheckboxItem { Label = "race", Value = "race", Checked = false }, new CheckboxItem { Label = "ELL", Value = "ELL", Checked = false }, new CheckboxItem { Label = "ED", Value = "ED", Checked = false }, new CheckboxItem { Label = "disabilities", Value = "disabilities", Checked = false } }
            };

            // Act
            RedirectToRouteResult action = (RedirectToRouteResult)analysis.Step2b("Submit", model, analysis.ControllerContext.HttpContext.User, 1);

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult), action);
            Assert.AreEqual(action.RouteValues["action"], "Step3");
            Assert.AreEqual(action.RouteValues["controller"], "Analysis");

            action = (RedirectToRouteResult)analysis.Step2b("Submit", model, analysis.ControllerContext.HttpContext.User, 2);

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult), action);
            Assert.AreEqual(action.RouteValues["action"], "PermissionDenied");
            Assert.AreEqual(action.RouteValues["controller"], "Error");

            action = (RedirectToRouteResult)analysis.Step2b("Back", model, analysis.ControllerContext.HttpContext.User, 2);

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult), action);
            Assert.AreEqual(action.RouteValues["action"], "Step2");
            Assert.AreEqual(action.RouteValues["controller"], "Analysis");
        }

        /// <summary>
        /// The step2 method returns a view
        /// </summary>
        [Test]
        public void Step3_Get_Returns_View()
        {
            // Arrange
            AnalysisController analysis = GetAnalysisController();

            // Act
            ActionResult view = analysis.Step3(analysis.ControllerContext.HttpContext.User, 1);

            // Assert
            Assert.IsInstanceOf(typeof(ViewResult), view);
        }

        /// <summary>
        /// The step2 method returns a view
        /// </summary>
        [Test]
        [Ignore("Ignore a test")]
        public void Step3_Submit_Returns_View()
        {
            // Arrange
            AnalysisController analysis = GetAnalysisController();

            // Act
            RedirectToRouteResult action = (RedirectToRouteResult)analysis.Step3("View Results", analysis.ControllerContext.HttpContext.User, 1);

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult), action);
            Assert.AreEqual(action.RouteValues["action"], "Step4");
            Assert.AreEqual(action.RouteValues["controller"], "Analysis");

            action = (RedirectToRouteResult)analysis.Step3("View Results", analysis.ControllerContext.HttpContext.User, 2);

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult), action);
            Assert.AreEqual(action.RouteValues["action"], "PermissionDenied");
            Assert.AreEqual(action.RouteValues["controller"], "Error");

            action = (RedirectToRouteResult)analysis.Step3("Back", analysis.ControllerContext.HttpContext.User, 2);

            // Assert
            Assert.IsInstanceOf(typeof(ActionResult), action);
            Assert.AreEqual(action.RouteValues["action"], "Step2b");
            Assert.AreEqual(action.RouteValues["controller"], "Analysis");
        }



        /// <summary>
        /// The ListToCheckboxes method works as intended.
        /// </summary>
        [Test]
        public void ValidateTreatmentGroupFile_returns_true()
        {
            // Arrange
            AnalysisController analysis = GetAnalysisController();

            var userAnalysis = new Analysis();

            // Act
            
            // Assert
            Assert.IsFalse(analysis.ValidateTreatmentGroupFile(userAnalysis));

            userAnalysis.IdentifierFile = "C:\fakedir\fakefile.xlsx";

            Assert.IsTrue(analysis.ValidateTreatmentGroupFile(userAnalysis));
        }

        public static HttpPostedFileBase GetMockUploadFile(string fileName)
        {
            Mock<HttpPostedFileBase> mockFile = new Mock<HttpPostedFileBase>();
            UTF8Encoding enc = new UTF8Encoding();
            mockFile.Setup(f => f.ContentLength).Returns(EvalEngine.Test.UploadTestFiles.test_data_upload.Length * 8);
            mockFile.Setup(f => f.FileName).Returns(fileName);
            mockFile.Setup(f => f.SaveAs(It.IsAny<string>()));
            mockFile.Setup(f => f.ContentType).Returns("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            mockFile.Setup(f => f.InputStream).Returns(new MemoryStream(EvalEngine.Test.UploadTestFiles.test_data_upload));
            return mockFile.Object;
        }
    }
}
