// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnalysisController.cs" company="RTI, Inc.">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// <summary>
//   The analysis controller. Generates an analysis request, displays results.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EvalEngine.Domain.Abstract;
using EvalEngine.Domain.Entities;
using EvalEngine.Infrastructure.Abstract;
using EvalEngine.UI.Models;
using HttpUtils;
using LinqToExcel;
using Resources;
using Rotativa;

namespace EvalEngine.UI.Controllers
{
    /// <summary>
    /// The analysis controller.
    /// </summary>
    [HandleError]
    public class AnalysisController : Controller
    {
        /// <summary>
        /// The analysis repository.
        /// </summary>
        private readonly IAnalysesRepository _analysisRepository;

        /// <summary>
        /// The state assignment repository.
        /// </summary>
        private readonly IStateAssignmentRepository _stateAssignmentRepository;

        /// <summary>
        /// The state repository.
        /// </summary>
        private readonly IStateRepository _stateRepository;

        /// <summary>
        /// The job message repository.
        /// </summary>
        private readonly IJobMessageRepository _jobMessageRepository;

        /// <summary>
        /// The job results repository.
        /// </summary>
        private readonly IJobResultsRepository _jobResultsRepository;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalysisController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="stateAssignmentRepository">The state assignment repository.</param>
        /// <param name="stateRepository">The state repository.</param>
        /// <param name="analysisRepository">The analysis repository.</param>
        /// <param name="jobMessageRepository">The job message repository.</param>
        /// <param name="jobResultsRepository">The job results repository.</param>
        public AnalysisController(ILogger logger, IStateAssignmentRepository stateAssignmentRepository, IStateRepository stateRepository, IAnalysesRepository analysisRepository, IJobMessageRepository jobMessageRepository, IJobResultsRepository jobResultsRepository)
        {
            this.logger = logger;
            _stateAssignmentRepository = stateAssignmentRepository;
            _stateRepository = stateRepository;
            _analysisRepository = analysisRepository;
            _jobMessageRepository = jobMessageRepository;
            _jobResultsRepository = jobResultsRepository;
        }

        /// <summary>
        /// Gets the list of states for a particular user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>List{CheckboxItem}.</returns>
        private List<CheckboxItem> StateList(IPrincipal user)
        {
            List<CheckboxItem> states = new List<CheckboxItem>();
            if (user.IsInRole("Project Administrator") || user.IsInRole("Project User") || user.IsInRole("Site Administrator"))
            {
                states = _stateRepository.GetStates().Select(
                        i => new CheckboxItem { Label = i.FullName, Value = i.StateAbbrev, Checked = false }).ToList();
                this.TempData["ShowStates"] = true;
            }
            else if (user.IsInRole("Multiple State User"))
            {
                states = _stateAssignmentRepository.GetStatesByUserName(user.Identity.Name).Select(
                        i => new CheckboxItem { Label = i.FullName, Value = i.StateAbbrev, Checked = false }).ToList();
                this.TempData["ShowStates"] = true;
            }
            else
            {
                this.TempData["ShowStates"] = false;
            }
            return states;
        }

        /// <summary>
        /// Displays Step1 of Analysis
        /// </summary>
        /// <param name="user">The user who will own the Analysis.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>Step1 view</returns>
        [Authorize]
        public ActionResult Step1(IPrincipal user, int id = 0)
        {
            Step1Model m = new Step1Model();

            m.RadioStates = StateList(user);
            string info = "";


            if (user.IsInRole("State Administrator") || user.IsInRole("State User"))
            {
                m.State = _stateAssignmentRepository.GetStateAbbrevsByUserName(user.Identity.Name).FirstOrDefault();
                this.TempData["StateIDText"] = _stateRepository.GetStateIdText(m.State);
                info = GetStateHelpfulText(m.State);
            }
            else
            {
                this.TempData["StateIDText"] = "the state student ID";
                foreach (CheckboxItem s in m.RadioStates)
                {
                    info += GetStateHelpfulText(s.Value);
                }
            }

            m.HelpfulInfo = info;
            m.HasStateID = true;

            if (id != 0)
            {
                if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) || _jobResultsRepository.GetJobByJobGUID(_analysisRepository.GetById(id).JobGUID) != null)
                {
                    return this.RedirectToAction("PermissionDenied", "Error");
                }
                Analysis userAnalysis = _analysisRepository.GetById(id);
                m.HasStateID = true;
                m.State = userAnalysis.State;
            }

            return this.View(m);
        }

        /// <summary>
        /// Processes Step1 form.
        /// </summary>
        /// <param name="submitForm">The submit button text.</param>
        /// <param name="model">The Step1 model to process.</param>
        /// <param name="user">The current user.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>Redirects to Step1B or redisplays form.</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Step1(string submitForm, Step1Model model, IPrincipal user, int id = 0)
        {
            if (submitForm.Equals("Back"))
            {
                return this.RedirectToAction("Index", "Home", string.Empty);
            }
            if (model.HasStateID == false)
            {
                this.TempData["message"] = "State student IDs are currently required to use the Evaluation Engine. Please check with district or school officials about obtaining the necessary IDs for students in your intervention.";
                model.RadioStates = StateList(user);
                string info = "";
                if (user.IsInRole("State Administrator") || user.IsInRole("State User"))
                {
                    model.State = _stateAssignmentRepository.GetStateAbbrevsByUserName(user.Identity.Name).FirstOrDefault();
                    this.TempData["StateIDText"] = _stateRepository.GetStateIdText(model.State);
                    info = GetStateHelpfulText(model.State);
                }
                else
                {
                    this.TempData["StateIDText"] = "the state student ID";
                    foreach (CheckboxItem s in model.RadioStates)
                    {
                        info += GetStateHelpfulText(s.Value);
                    }
                    model.HelpfulInfo = info;
                }


                model.HelpfulInfo = info;
                return this.View(model);
            }

            if (!this.ModelState.IsValid)
            {
                model.RadioStates = StateList(user);
                if (user.IsInRole("State Administrator") || user.IsInRole("State User"))
                {
                    model.State = _stateAssignmentRepository.GetStateAbbrevsByUserName(user.Identity.Name).FirstOrDefault();
                    this.TempData["StateIDText"] = _stateRepository.GetStateIdText(model.State);
                    model.HelpfulInfo = GetStateHelpfulText(model.State);
                }
                else
                {
                    this.TempData["StateIDText"] = "the state student ID";
                    string info = "";
                    foreach (CheckboxItem s in model.RadioStates)
                    {
                        info += GetStateHelpfulText(s.Value);
                    }
                    model.HelpfulInfo = info;
                }

                return this.View("Step1", model);
            }

            Analysis userAnalysis = new Analysis();
            int insertedId = 0;

            if (id != 0)
            {
                if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) || _jobResultsRepository.GetJobByJobGUID(_analysisRepository.GetById(id).JobGUID) != null)
                {
                    return this.RedirectToAction("PermissionDenied", "Error");
                }
                userAnalysis = _analysisRepository.GetById(id);
                insertedId = id;
                model.HasStateID = true;
            }
            else
            {
                var currentuser = Membership.GetUser(user.Identity.Name);
                string userEmail = currentuser.Email;
                insertedId = _analysisRepository.CreateAnalysis(user.Identity.Name, userEmail);
                userAnalysis = _analysisRepository.GetById(insertedId);
            }

            if (user.IsInRole("State Administrator") || user.IsInRole("State User"))
            {
                userAnalysis.State = _stateAssignmentRepository.GetStateAbbrevsByUserName(user.Identity.Name).FirstOrDefault();
                model.HelpfulInfo = GetStateHelpfulText(model.State);
            }
            else
            {
                userAnalysis.State = model.State;
            }

            userAnalysis.UseIdentifier = "STUDENTID_STATE";

            _analysisRepository.Update(userAnalysis);

            return this.RedirectToAction("Step1b", "Analysis", new { id = insertedId });
        }

        public string GetStateHelpfulText(string val)
        {
            string info = "";
            info = _stateRepository.GetStateHelpfulText(val);
            return info;
        }

        /// <summary>
        /// Displays Step1B.
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Step1B view</returns>
        [Authorize]
        public ActionResult Step1b(IPrincipal user, int id = 0)
        {
            if (id != 0)
            {
                if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) || _jobResultsRepository.GetJobByJobGUID(_analysisRepository.GetById(id).JobGUID) != null)
                {
                    return this.RedirectToAction("PermissionDenied", "Error");
                }
                Analysis userAnalysis = _analysisRepository.GetById(id);
                Step1BModel model = new Step1BModel
                {
                    ParticipantFile = userAnalysis.IdentifierFile,
                    ParticipantText = userAnalysis.IdentifierList
                };
                return this.View(model);
            }
            else
            {
                return this.View();
            }

        }

        /// <summary>
        /// Process Step1B form
        /// </summary>
        /// <param name="submitForm">The submit button text.</param>
        /// <param name="model">The Step1B model to process</param>
        /// <param name="file">The uploaded file.</param>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Redirects to Step2 or redisplays form.</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Step1b(string submitForm, Step1BModel model, HttpPostedFileBase file, IPrincipal user, int id = 0)
        {
            Analysis userAnalysis = null;
            var path = string.Empty;

            if (id != 0)
            {
                if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) || _jobResultsRepository.GetJobByJobGUID(_analysisRepository.GetById(id).JobGUID) != null)
                {
                    return this.RedirectToAction("PermissionDenied", "Error");
                }

                userAnalysis = _analysisRepository.GetById(id);
            }

            if (submitForm.Equals("Back"))
            {
                return (id != 0) ? this.RedirectToAction("Step1", "Analysis", new { id = id }) : this.RedirectToAction("Step1", "Analysis", new { id = 0 });
            }

            if (!this.ModelState.IsValid)
            {
                TempData["message"] = UserFeedbackMessages.StudentIdUploadError;
                return this.View("Step1b");
            }

            if (file != null && file.ContentLength > 0 && userAnalysis != null)
            {
                userAnalysis.IdentifierFile = SaveUploadFile(file, userAnalysis);
                _analysisRepository.Update(userAnalysis);
                string[] jobStudyIds = this.ProcessTreatmentGroupFile(userAnalysis);
                if (jobStudyIds == null)
                {
                    return this.RedirectToAction("Step1b", "Analysis", new { id = id });
                }
                return this.RedirectToAction("Step2", "Analysis", new { id = id });
            }
            else if (model.ParticipantText != null)
            {
                userAnalysis.IdentifierList = model.ParticipantText;
                _analysisRepository.Update(userAnalysis);
                string[] jobStudyIds = this.ProcessTreatmentGroupText(userAnalysis);
                if (jobStudyIds == null)
                {
                    return this.RedirectToAction("Step1b", "Analysis", new { id = id });
                }
                return this.RedirectToAction("Step2", "Analysis", new { id = id });
            }

            return this.RedirectToAction("Step1b", "Analysis", new { id = id });
        }

        public string SaveUploadFile(HttpPostedFileBase file, Analysis userAnalysis)
        {
            string uploadFolder = ConfigurationManager.AppSettings["AnalysisUploadFolder"];
            string path = String.Empty;
            var fileName = Path.GetFileName(file.FileName);
            string ipath = uploadFolder + userAnalysis.JobGUID;
            string mappedPath = Server.MapPath(ipath);
            path = Path.Combine(mappedPath, fileName);
            new FileInfo(path).Directory.Create();
            file.SaveAs(path);
            return path;
        }

        /// <summary>
        /// Displays Step2 form
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Step2 view</returns>
        [Authorize]
        public ActionResult Step2(IPrincipal user, int id = 0)
        {
            if (id != 0)
            {
                if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) || _jobResultsRepository.GetJobByJobGUID(_analysisRepository.GetById(id).JobGUID) != null)
                {
                    return this.RedirectToAction("PermissionDenied", "Error");
                }
                Analysis userAnalysis = _analysisRepository.GetById(id);
                Step2Model model = new Step2Model
                {
                    StudyDescription = userAnalysis.StudyDescription,
                    StudyName = userAnalysis.StudyName
                };
                return this.View(model);
            }
            else
            {
                return this.View();
            }
        }

        /// <summary>
        /// Processes Step2 form
        /// </summary>
        /// <param name="submitForm">The submit button text.</param>
        /// <param name="model">The Step2 model to process</param>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Redirects to Step2B or redisplays form</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Step2(string submitForm, Step2Model model, IPrincipal user, int id = 0)
        {
            if (submitForm.Equals("Back"))
            {
                return this.RedirectToAction("Step1b", "Analysis", new { id = id });
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("Step2");
            }


            if (id != 0)
            {
                if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) || _jobResultsRepository.GetJobByJobGUID(_analysisRepository.GetById(id).JobGUID) != null)
                {
                    return this.RedirectToAction("PermissionDenied", "Error");
                }
                
                Analysis userAnalysis = _analysisRepository.GetById(id);
                userAnalysis.StudyName = model.StudyName;
                userAnalysis.StudyDescription = model.StudyDescription;
                _analysisRepository.Update(userAnalysis);
            }

            return this.RedirectToAction("Step2b", "Analysis", new { id = id });
        }

        /// <summary>
        /// The Step2C form
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Step2C form</returns>
        [Authorize]
        public ActionResult Step2c(IPrincipal user, int id = 0)
        {
            if (id != 0)
            {
                if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) || _jobResultsRepository.GetJobByJobGUID(_analysisRepository.GetById(id).JobGUID) != null)
                {
                    return this.RedirectToAction("PermissionDenied", "Error");
                }
                Analysis userAnalysis = _analysisRepository.GetById(id);

                Step2CModel model = new Step2CModel
                {
                    DistrictMatch = userAnalysis.DistrictMatch
                };

                return this.View(model);
            }
            else
            {
                return this.View();
            }
        }

        /// <summary>
        /// Processes Step2C form
        /// </summary>
        /// <param name="submitForm">The submit button text</param>
        /// <param name="model">The StepC model</param>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Redirects to Step3 or redisplays form.</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Step2c(string submitForm, Step2CModel model, IPrincipal user, int id = 0)
        {
            if (submitForm.Equals("Back"))
            {
                return this.RedirectToAction("Step2b", "Analysis", new { id });
            }

            Analysis userAnalysis = _analysisRepository.GetById(id);
            if (!ModelState.IsValid)
            {
                return View("Step2c", model);
            }

            if (id != 0)
            {
                if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) || _jobResultsRepository.GetJobByJobGUID(_analysisRepository.GetById(id).JobGUID) != null)
                {
                    return this.RedirectToAction("PermissionDenied", "Error");
                }

                userAnalysis = _analysisRepository.GetById(id);

                userAnalysis.DistrictMatch = model.DistrictMatch;
                _analysisRepository.Update(userAnalysis);
                
               
            }

            return RedirectToAction("Step2b", "Analysis", new { id });
        }

        private void RunAnalysis(Analysis userAnalysis)
        {

            if (_jobResultsRepository.GetJobByJobGUID(userAnalysis.JobGUID) == null)
            {
                if (this.ValidateTreatmentGroupFile(userAnalysis))
                {
                    string[] jobStudyIds = ProcessTreatmentGroupFile(userAnalysis);
                    _jobMessageRepository.CreateJobAndIds(userAnalysis, jobStudyIds);
                }
                else if (userAnalysis.IdentifierList != null)
                {
                    string[] jobStudyIds = ProcessTreatmentGroupText(userAnalysis);
                    _jobMessageRepository.CreateJobAndIds(userAnalysis, jobStudyIds);
                }
            }
            if (_jobResultsRepository.GetJobByJobGUID(userAnalysis.JobGUID) != null && _jobResultsRepository.GetJobByJobGUID(userAnalysis.JobGUID).StatusId == 1)
            {
                if (ConfigurationManager.AppSettings["RServerURL"] != "")
                {
                    logger.Info("Sending Request to R Server: " + userAnalysis.JobGUID);
                    Thread.Sleep(10000);
                    GenerateRServerRequest(userAnalysis.JobGUID.ToString(), "1");
                }
                if (_jobResultsRepository.GetJobResultByJobGUID(userAnalysis.JobGUID) == null)
                {
                    if (ConfigurationManager.AppSettings["RServerURL"] == "")
                    {
                        logger.Info("Fake Job Created: " + userAnalysis.JobGUID);
                        _jobResultsRepository.InsertFakeResults(userAnalysis.JobGUID, userAnalysis.State);
                        _jobResultsRepository.SubmitChanges();
                    }
                }
            }
            if (ConfigurationManager.AppSettings["RServerURL"] == "")
            {
                userAnalysis.StatusId = 3;
                _analysisRepository.Update(userAnalysis);
                logger.Info("Fake Analysis Results Created: " + userAnalysis.JobGUID);
            }
        }

        /// <summary>
        /// Transforms string list of items to checkboxes.
        /// </summary>
        /// <param name="slist">The slist.</param>
        /// <returns>List{CheckboxItem}.</returns>
        private List<CheckboxItem> ListToCheckboxes(string slist)
        {
            if (!string.IsNullOrEmpty(slist))
            {
                string[] alist = slist.Split(',');
                return alist.Select(a => new CheckboxItem
                {
                    Checked = true, Label = a, Value = a
                }).ToList();
            }
            else
            {

                return new List<CheckboxItem>();
            }
        }

        /// <summary>
        /// Transforms string list of items to checkboxes.
        /// </summary>
        /// <param name="slist">The slist.</param>
        /// <returns>List{CheckboxItem}.</returns>
        private List<CheckboxItem> GetGradesCheckboxes(string slist)
        {
            List<CheckboxItem> cblist = new List<CheckboxItem>();
            for (int i = 0; i <= 12; i++)
            {
                string val = i.ToString();
                if (i == 0)
                {
                    val = "K";
                }
                cblist.Add(new CheckboxItem
                {
                    Checked = false,
                    Label = val,
                    Value = val
                });                
            }
            if (!string.IsNullOrEmpty(slist))
            {
                string[] alist = slist.Split(',');
                foreach (string a in alist)
                {
                    var val = Int32.Parse(a);
                    if (a == "K")
                    {
                        val = 0;
                    }
                    cblist[val].Checked = true;
                }
            }
            return cblist;
        }


        /// <summary>
        /// The Step2B form
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Step2B form</returns>
        [Authorize]
        public ActionResult Step2b(IPrincipal user, int id = 0)
        {
            if (id != 0)
            {
                if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) || _jobResultsRepository.GetJobByJobGUID(_analysisRepository.GetById(id).JobGUID) != null)
                {
                    return this.RedirectToAction("PermissionDenied", "Error");
                }
                Analysis userAnalysis = _analysisRepository.GetById(id);
                var yearlist = _analysisRepository.GetYearsOfInterest(userAnalysis.State);
                var subgroups = _analysisRepository.GetUserOptionsBySection(_stateRepository.GetStateId(userAnalysis.State), "subgroup");
                var outcomes = _analysisRepository.GetUserOptionsBySection(_stateRepository.GetStateId(userAnalysis.State), "outcome");
                var outcomelist = outcomes.Select(
                        i => new UserOutcomeOption { Id = i.Id, Label = i.Label, Value = i.Value, isHeader = i.IsHeader, isLabel = i.IsLabel, Rank = i.Rank, Section = i.Section, Checked = false, parentId = i.ParentId }).ToList();
                var subgrouplist = subgroups.Select(
                        i => new CheckboxItem { Label = i.Label, Value = i.Value, Checked = (userAnalysis.SubgroupAnalyses.IndexOf(i.Value) >= 0) }).ToList();
                var yearslist = yearlist.Select(
                        i => new CheckboxItem { Label = i.Label, Value = i.Value, Checked = _analysisRepository.DoesAnalysisContainYear(i.Value, userAnalysis.Id) }).ToList();

                Step2BModel model = new Step2BModel
                {
                    AnalysisDescription = userAnalysis.AnalysisDescription,
                    AnalysisName = userAnalysis.AnalysisName,
                    GradeLevels = GetGradesCheckboxes(userAnalysis.InterventionGradeLevels),
                    GradeLevel = userAnalysis.InterventionGradeLevels,
                    //InterventionAreas = ListToCheckboxes(userAnalysis.InterventionAreas),
                    InterventionStartDate = userAnalysis.InterventionStartDate,
                    InterventionEndDate = userAnalysis.InterventionEndDate,
                    OutcomeMeasures = ListToCheckboxes(userAnalysis.OutcomeAreas),
                    SubgroupAnalyses = subgrouplist,
                    YearsOfInterest = yearslist,
                    OutcomeItems = outcomelist
                };

                return this.View(model);
            }
            else
            {
                return this.View();
            }
        }

        /// <summary>
        /// Processes Step2B form
        /// </summary>
        /// <param name="submitForm">The submit button text</param>
        /// <param name="model">The Step2B model</param>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Redirects to Step2c or redisplays form.</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Step2b(string submitForm, Step2BModel model, IPrincipal user, int id = 0)
        {
            if (submitForm.Equals("Back"))
            {
                return this.RedirectToAction("Step2", "Analysis", new { id = id });
            }

            Analysis userAnalysis = _analysisRepository.GetById(id);

            var defaultStartDate = _analysisRepository.GetDefaultStartDate(userAnalysis.State);

            if (model.InterventionStartDate.CompareTo(defaultStartDate) < 0)
            {
                ModelState.AddModelError("InterventionStartDate", String.Format("Start date must be after {0}.", defaultStartDate.ToShortDateString()));
            }

            var numErrorDates = 0;
            var july2011 = new DateTime(2011, 7, 1);
            var july2012 = new DateTime(2012, 7, 1);
            if (model.InterventionStartDate.CompareTo(july2011) > 0)
            {
                numErrorDates = model.YearsOfInterest.Count(x => x.Checked && x.Value.Equals("2010-2011"));
            }

            if (model.InterventionStartDate.CompareTo(july2012) > 0)
            {
                numErrorDates += model.YearsOfInterest.Count(x => x.Checked && x.Value.Equals("2011-2012"));
            }

            if (model.YearsOfInterest.Count(x => x.Checked) == 0)
            {
                ModelState.AddModelError("YearsOfInterest", string.Empty);
            }

            if (numErrorDates > 0)
            {
                ModelState.AddModelError("YearsOfInterest[0].Checked", string.Empty);
            }

            if (!this.ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                userAnalysis = _analysisRepository.GetById(id);
                var yearlist = _analysisRepository.GetYearsOfInterest(userAnalysis.State);
                var grades = string.Join(",", model.GradeLevels.FindAll(x => x.Checked.Equals(true)).Select(x => x.Value.ToString()).ToArray());
                List<string> gradeStringList = grades.Split(',').ToList();
                var subgroups = _analysisRepository.GetUserOptionsBySection(_stateRepository.GetStateId(userAnalysis.State), "subgroup");
                var outcomes = _analysisRepository.GetUserOptionsBySection(_stateRepository.GetStateId(userAnalysis.State), "outcome");
                var outcomelist = outcomes.Select(
                        i => new UserOutcomeOption { Id = i.Id, Label = i.Label, Value = i.Value, isHeader = i.IsHeader, isLabel = i.IsLabel, Rank = i.Rank, Section = i.Section, Checked = false, parentId = i.ParentId }).ToList();
                var subgrouplist = subgroups.Select(
                        i => new CheckboxItem { Label = i.Label, Value = i.Value, Checked = (userAnalysis.SubgroupAnalyses.IndexOf(i.Value) >= 0) }).ToList();
                var yearslist = yearlist.Select(
                        i => new CheckboxItem { Label = i.Label, Value = i.Value, Checked = _analysisRepository.DoesAnalysisContainYear(i.Value, userAnalysis.Id) }).ToList();
                model.OutcomeItems = outcomelist;
                model.YearsOfInterest = yearslist;
                model.SubgroupAnalyses = subgrouplist;
                model.GradeLevels = GetGradesCheckboxes(userAnalysis.InterventionGradeLevels);
                model.InterventionStartDate = userAnalysis.InterventionStartDate;
                return this.View("Step2b", model);
            }

            if (id != 0)
            {
                if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) || _jobResultsRepository.GetJobByJobGUID(_analysisRepository.GetById(id).JobGUID) != null)
                {
                    return this.RedirectToAction("PermissionDenied", "Error");
                }

                userAnalysis = _analysisRepository.GetById(id);
                userAnalysis.AnalysisName = model.AnalysisName;
                userAnalysis.AnalysisDescription = model.AnalysisDescription;
                //userAnalysis.InterventionAreas = string.Join(",", model.InterventionAreas.Where(x => x.Checked.Equals(true)).Select(x => x.Value.ToString()).ToArray());
                userAnalysis.InterventionGradeLevels = string.Join(",", model.GradeLevels.FindAll(x => x.Checked.Equals(true)).Select(x => x.Value.ToString()).ToArray());
                //userAnalysis.InterventionGradeLevels = model.GradeLevel;
                userAnalysis.OutcomeAreas = string.Join(",", model.OutcomeMeasures.FindAll(x => x.Checked.Equals(true)).Select(x => x.Value.ToString()).ToArray());
                userAnalysis.SubgroupAnalyses = string.Join(",", model.SubgroupAnalyses.FindAll(x => x.Checked.Equals(true)).Select(x => x.Value.ToString()).ToArray());
                userAnalysis.InterventionStartDate = model.InterventionStartDate;
                //userAnalysis.InterventionEndDate = model.InterventionEndDate;
                userAnalysis.YearsOfInterest = string.Join(",", model.YearsOfInterest.FindAll(x => x.Checked.Equals(true)).Select(x => x.Value.ToString()).ToArray());
                userAnalysis.DistrictMatch = 0;
                _analysisRepository.Update(userAnalysis);

                    if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) || _jobResultsRepository.GetJobByJobGUID(_analysisRepository.GetById(id).JobGUID) != null)
                    {
                        return this.RedirectToAction("PermissionDenied", "Error");
                    }

                if (submitForm.Equals("Advanced Options"))
                {
                    return this.RedirectToAction("Step2c", "Analysis", new {id = id});
                }
                else
                {

                   RunAnalysis(userAnalysis);
                }
            }

            return this.RedirectToAction("Step3", "Analysis", new { id });
        }

        /// <summary>
        /// Displays Step3
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Redirects to Step4</returns>
        [Authorize]
        public ActionResult Step3(IPrincipal user, int id = 0)
        {
            if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) && !user.IsInRole("Site Administrator"))
            {
                return RedirectToAction("PermissionDenied", "Error");
            }
            return View();
        }

        /// <summary>
        /// Displays Step3
        /// </summary>
        /// <param name="submitForm">The submit form.</param>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Redirects to Step4</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Step3(string submitForm, IPrincipal user, int id = 0)
        {
            Analysis userAnalysis = null;

            if (submitForm.Equals("Back"))
            {
                return RedirectToAction("Step2c", "Analysis", new { id = id });
            }

            if (id != 0 && submitForm.Equals("View Results"))
            {
                if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) && !user.IsInRole("Site Administrator"))
                {
                    return RedirectToAction("PermissionDenied", "Error");
                }

                return RedirectToAction("Step4", "Analysis", new { id = id });
            }
            return RedirectToAction("PermissionDenied", "Error");
        }

        /// <summary>
        /// Displays Step4, the finished report.
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Step4 view</returns>
        public ActionResult Step4PDF(IPrincipal user, int id = 0)
        {
            Analysis analysis = new Analysis();
            Job job = new Job();
            if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) && !user.IsInRole("Site Administrator"))
            {
                RedirectToAction("PermissionDenied", "Error");
            }
            else
            {
                analysis = _analysisRepository.GetById(id);
                job = _jobResultsRepository.GetJobByJobGUID(analysis.JobGUID);
            }

            if (job.Id != 0 && (_jobResultsRepository.GetJobStatusNameById(job.StatusId) == "DONE"))
            {
                Step4Model model = PrepareReportView(job, analysis);
                return View(model); //                
            }

            if (job.Id != 0 && (_jobResultsRepository.GetJobStatusNameById(job.StatusId) != "DONE"))
            {
                return RedirectToAction("ReportNotReady", "Error");
            }

            return View();
        }

        /// <summary>
        /// Displays Step4, the finished report.
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Step4 view</returns>
        public ActionResult Step4PDFCover(IPrincipal user, int id = 0)
        {
            Analysis analysis = new Analysis();
            Job job = new Job();
            if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) && !user.IsInRole("Site Administrator"))
            {
                this.RedirectToAction("PermissionDenied", "Error");
            }
            else
            {
                analysis = _analysisRepository.GetById(id);
                job = _jobResultsRepository.GetJobByJobGUID(analysis.JobGUID);
            }

            if (job.Id != 0 && (_jobResultsRepository.GetJobStatusNameById(job.StatusId) == "DONE"))
            {
                Step4Model model = PrepareReportView(job, analysis);
                return View(model); //                
            }

            if (job.Id != 0 && (_jobResultsRepository.GetJobStatusNameById(job.StatusId) != "DONE"))
            {
                return RedirectToAction("ReportNotReady", "Error");
            }

            return View();
        }

        /// <summary>
        /// Displays Step4, the finished report.
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Step4 view</returns>
        public ActionResult Step4PDFTOC(IPrincipal user, int id = 0)
        {
            Analysis analysis = new Analysis();
            Job job = new Job();
            if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) && !user.IsInRole("Site Administrator"))
            {
                RedirectToAction("PermissionDenied", "Error");
            }
            else
            {
                analysis = _analysisRepository.GetById(id);
                job = _jobResultsRepository.GetJobByJobGUID(analysis.JobGUID);
            }

            if (job.Id != 0 && (_jobResultsRepository.GetJobStatusNameById(job.StatusId) == "DONE"))
            {
                Step4Model model = PrepareReportView(job, analysis);
                return View(model); //                
            }

            if (job.Id != 0 && (_jobResultsRepository.GetJobStatusNameById(job.StatusId) != "DONE"))
            {
                return RedirectToAction("ReportNotReady", "Analysis");
            }

            return View();
        }

        /// <summary>
        /// The report not ready page.
        /// </summary>
        /// <returns>InternalServerError view</returns>
        public ActionResult ReportNotReady()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return this.View("Error/ReportNotReady");
        }

        /// <summary>
        /// Handles converting Step4PDF view, as well as Step4PDFCover and Step4PDFTOC into a PDF, using Rotativa.
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis Id</param>
        /// <returns></returns>
        public ActionResult Step4Rota(IPrincipal user, int id = 0)
        {
            /*UserProfile profile = UserProfile.GetUserProfile(user.Identity.Name);
            var thisUser = Membership.GetUser(user.Identity.Name);
            Analysis analysis = new Analysis();
            Job job = new Job();
if (!this.analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) && !user.IsInRole("Site Administrator"))
            {
                return this.RedirectToAction("PermissionDenied", "Error");
            }
            else
            {
                analysis = this.analysisRepository.GetById(id);
                job = this.jobResultsRepository.GetJobByJobGUID(analysis.JobGUID);
            }*/


            //string customSwitches = "--print-media-type --footer-line --footer-right [page] cover {0}";
            string customSwitches = "--print-media-type --footer-html {0} cover {1} ";
            string filename = "EE Report" + id;
            string coverURL = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("Step4PDFCover", "Analysis", new { id = id });
            string footerURL = Request.Url.Scheme + "://" + Request.Url.Authority + "/Content/inc/files/Step4PDFFooter.html";
            string TOCURL = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("Step4PDFTOC", "Analysis", new { id = id });
            customSwitches = String.Format(customSwitches, footerURL, coverURL, TOCURL);


            return new ActionAsPdf("Step4PDF", new { id }) { FileName = filename + ".pdf", CustomSwitches = customSwitches };
        }

        /// <summary>
        /// Displays Step4, teh overall results section of the finished report.
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Step4 view</returns>
        [Authorize]
        public ActionResult Step4(IPrincipal user, int id = 0)
        {
            Analysis analysis = new Analysis();
            Job job = new Job();
            if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) && !user.IsInRole("Site Administrator"))
            {
                return this.RedirectToAction("PermissionDenied", "Error");
            }
            else
            {
                analysis = _analysisRepository.GetById(id);
                job = _jobResultsRepository.GetJobByJobGUID(analysis.JobGUID);
            }

            var jobStatusName = _jobResultsRepository.GetJobStatusNameById(job.StatusId);
            if (job.Id != 0 && jobStatusName.Equals("DONE"))
            {
                Step4Model model = PrepareReportView(job, analysis);
                ViewBag.ID = id;
                return this.View(model);
            }
            else if (job.Id != 0 && jobStatusName.Equals("ERROR"))
            {
                return this.RedirectToAction("ErrorGeneratingReport", "Analysis", new { id = id });
            }
            else if (job.Id != 0)
            {
                return this.RedirectToAction("ReportNotReady", "Error");
            }

            return this.View();
        }

        /// <summary>
        /// Displays Step4, teh overall results section of the finished report.
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Step4 view</returns>
        [Authorize]
        public ActionResult ErrorGeneratingReport(IPrincipal user, int id = 0)
        {
            Analysis analysis = new Analysis();
            Job job = new Job();
            if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) && !user.IsInRole("Site Administrator"))
            {
                return this.RedirectToAction("PermissionDenied", "Error");
            }
            else
            {
                analysis = _analysisRepository.GetById(id);
                job = _jobResultsRepository.GetJobByJobGUID(analysis.JobGUID);
            }

            var jobStatusName = _jobResultsRepository.GetJobStatusNameById(job.StatusId);
            if (job.Id != 0 && jobStatusName.Equals("ERROR"))
            {
                ErrorModel model = new ErrorModel
                {
                    Error = job.ErrorMessages
                };
                ViewBag.ID = id;
                return this.View(model);
            }

            return this.View();
        }

        /// <summary>
        /// Displays Step4, teh overall results section of the finished report.
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Step4 view</returns>
        [Authorize]
        public ActionResult SupplementalInfo(IPrincipal user, int id = 0)
        {
            Analysis analysis = new Analysis();
            Job job = new Job();
            if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) && !user.IsInRole("Site Administrator"))
            {
                return this.RedirectToAction("PermissionDenied", "Error");
            }
            else
            {
                analysis = _analysisRepository.GetById(id);
                job = _jobResultsRepository.GetJobByJobGUID(analysis.JobGUID);
            }

            var jobStatusName = _jobResultsRepository.GetJobStatusNameById(job.StatusId);
            if (job.Id != 0 && jobStatusName.Equals("DONE"))
            {
                Step4Model model = PrepareReportView(job, analysis);
                ViewBag.ID = id;
                return this.View(model);
            }
            else if (job.Id != 0 && jobStatusName.Equals("ERROR"))
            {
                return this.RedirectToAction("ErrorGeneratingReport", new { id = id });
            }
            else if (job.Id != 0)
            {
                return this.RedirectToAction("ReportNotReady", "Error");
            }

            return this.View();
        }

        /// <summary>
        /// Processes the Step4 form, adding the analyst notes for the overall results section for the finished report.
        /// </summary>
        /// <param name="submitForm">The submit form.</param>
        /// <param name="user">The current user</param>
        /// <param name="model">The model.</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Step4 view</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Step4(string submitForm, IPrincipal user, Step4Model model, int id = 0)
        {
            if (submitForm.Equals("Back"))
            {
                return this.RedirectToAction("Step3", "Analysis", new { id = id });
            }
            else
            {
                Analysis analysis = new Analysis();
                Job job = new Job();
                if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) && !user.IsInRole("Site Administrator"))
                {
                    return this.RedirectToAction("PermissionDenied", "Error");
                }
                else
                {
                    analysis = _analysisRepository.GetById(id);
                    job = _jobResultsRepository.GetJobByJobGUID(analysis.JobGUID);
                }
                foreach (ReportChart chart in model.Report.ChartCollection)
                {
                    _jobResultsRepository.UpdateChartNoteByJobGUID(analysis.JobGUID, chart.Rank, chart.Note, chart.Type);
                    _jobResultsRepository.SubmitChanges();
                }
            }

            return this.RedirectToAction("Step4", "Analysis", new { id = id });
        }

        /// <summary>
        /// Displays Step4a, the subgroup analysis tables.
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Step4 view</returns>
        [Authorize]
        public ActionResult Step4a(IPrincipal user, int id = 0)
        {
            Analysis analysis = new Analysis();
            Job job = new Job();
            if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) && !user.IsInRole("Site Administrator"))
            {
                return this.RedirectToAction("PermissionDenied", "Error");
            }
            else
            {
                analysis = _analysisRepository.GetById(id);
                job = _jobResultsRepository.GetJobByJobGUID(analysis.JobGUID);
            }

            var jobStatusName = _jobResultsRepository.GetJobStatusNameById(job.StatusId);
            if (job.Id != 0 && jobStatusName.Equals("DONE"))
            {
                Step4Model model = PrepareReportView(job, analysis);
                ViewBag.ID = id;
                return View(model);
            }
            else if (job.Id != 0 && jobStatusName.Equals("ERROR"))
            {
                return this.RedirectToAction("ErrorGeneratingReport", new { id = id });
            }
            else if (job.Id != 0)
            {
                return this.RedirectToAction("ReportNotReady", "Error");
            }

            return this.View();
        }

        /// <summary>
        /// Processes the Step4a form, taking in analyst notes for the subgroup analysis tables.
        /// </summary>
        /// <param name="submitForm">The submit form.</param>
        /// <param name="user">The current user</param>
        /// <param name="model">The model.</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Step4 view</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Step4a(string submitForm, IPrincipal user, Step4Model model, int id = 0)
        {
            if (submitForm.Equals("Back"))
            {
                return this.RedirectToAction("Step3", "Analysis", new { id = id });
            }
            else
            {
                Analysis analysis = new Analysis();
                Job job = new Job();
                if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) && !user.IsInRole("Site Administrator"))
                {
                    return this.RedirectToAction("PermissionDenied", "Error");
                }
                else
                {
                    analysis = _analysisRepository.GetById(id);
                    job = _jobResultsRepository.GetJobByJobGUID(analysis.JobGUID);
                }
                foreach (ReportTable table in model.Report.TableCollection)
                {
                    _jobResultsRepository.UpdateTableNoteByJobGUID(analysis.JobGUID, table.Rank, table.Note, table.Type);
                    _jobResultsRepository.SubmitChanges();
                }
            }

            return this.RedirectToAction("Step4a", "Analysis", new { id = id });
        }

        /// <summary>
        /// Displays Step4b, the data and statistical methods section.
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Step4 view</returns>
        [Authorize]
        public ActionResult Step4b(IPrincipal user, int id = 0)
        {
            Analysis analysis = new Analysis();
            Job job = new Job();
            if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) && !user.IsInRole("Site Administrator"))
            {
                return this.RedirectToAction("PermissionDenied", "Error");
            }
            else
            {
                analysis = _analysisRepository.GetById(id);
                job = _jobResultsRepository.GetJobByJobGUID(analysis.JobGUID);
            }

            var jobStatusName = _jobResultsRepository.GetJobStatusNameById(job.StatusId);
            if (job.Id != 0 && jobStatusName.Equals("DONE"))
            {
                Step4Model model = PrepareReportView(job, analysis);
                ViewBag.ID = id;
                return this.View(model);
            }
            else if (job.Id != 0 && jobStatusName.Equals("ERROR"))
            {
                return this.RedirectToAction("ErrorGeneratingReport", new { id = id });
            }
            else if (job.Id != 0)
            {
                return this.RedirectToAction("ReportNotReady", "Error");
            }

            return this.View();
        }

        /// <summary>
        /// Displays Step4c, the user defined descriptions of the finished report.
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="id">The Analysis id</param>
        /// <returns>Step4 view</returns>
        [Authorize]
        public ActionResult Step4c(IPrincipal user, int id = 0)
        {
            Analysis analysis = new Analysis();
            Job job = new Job();
            if (!_analysisRepository.IsOwnerOfAnalysis(user.Identity.Name, id) && !user.IsInRole("Site Administrator"))
            {
                return this.RedirectToAction("PermissionDenied", "Error");
            }
            else
            {
                analysis = _analysisRepository.GetById(id);
                job = _jobResultsRepository.GetJobByJobGUID(analysis.JobGUID);
            }

            var jobStatusName = _jobResultsRepository.GetJobStatusNameById(job.StatusId);
            if (job.Id != 0 && jobStatusName.Equals("DONE"))
            {
                Step4Model model = PrepareReportView(job, analysis);
                ViewBag.ID = id;
                return this.View(model);
            }
            else if (job.Id != 0 && jobStatusName.Equals("ERROR"))
            {
                return this.RedirectToAction("ErrorGeneratingReport", new { id = id });
            }
            else if (job.Id != 0)
            {
                return this.RedirectToAction("ReportNotReady", "Error");
            }

            return this.View();
        }

        /// <summary>
        /// Grabs SVG from database, returns it as a file to the view.
        /// </summary>
        /// <param name="jobGUID">the jobGUID</param>
        /// <param name="rank">rank order of outcomes</param>
        /// <param name="type">ss for scale score or pp for percent proficient</param>
        [Authorize]
        public ActionResult ChartImage(Guid jobGUID, int rank, string type)
        {
            var chart = "";
            if (type == "balance")
            {
                chart = _jobResultsRepository.GetBalanceChartByJobGUID(jobGUID, false);
            }
            else if (type == "balance-2")
            {
                chart = _jobResultsRepository.GetBalanceChartByJobGUID(jobGUID, true);
            }
            else
            {
                chart = _jobResultsRepository.GetChartByJobGUIDAndRank(jobGUID, rank, type);
            }

            if (chart != null)
            {
                var byteArray = Encoding.ASCII.GetBytes(chart);

                return File(byteArray, "image/svg+xml");

            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Displays library page.
        /// </summary>
        /// <param name="user">The current user</param>
        /// <returns>Library view</returns>
        [Authorize]
        public ViewResult Library(IPrincipal user)
        {
            Guid username = new Guid(user.Identity.Name);
            List<Analysis> analyses = _analysisRepository.GetAnalysesByUserId(username);
            List<Job> jobs = _jobResultsRepository.GetJobsByAnalyses(analyses);
            List<LibraryItem> model = (from job in jobs
                let an = _analysisRepository.GetAnalysisByJobGUID(job.JobGUID)
                select new LibraryItem
                {
                    JobGUID = job.JobGUID.ToString(), State = job.State, Status = _jobResultsRepository.GetJobStatusNameById(job.StatusId), Link = an.Id.ToString(), Name = an.AnalysisName, Params = "Analysis Name: " + an.AnalysisName + "<br/>" + "Study Name: " + an.StudyName + "<br/>" + "Intervention Start: " + an.InterventionStartDate + "<br/>" + "Intervention End: " + an.InterventionEndDate + "<br/>" + "Outcome Areas: " + an.OutcomeAreas + "<br/>" + "Subgroups: " + an.SubgroupAnalyses + "<br/>" + "Grade Levels: " + an.InterventionGradeLevels
                }).ToList();

            return this.View(model);
        }

        /// <summary>
        /// Validates uploaded treatment group file.
        /// </summary>
        /// <param name="analysis">The Analysis for which to validate.</param>
        /// <returns>true if file is valid (always returns true for now)</returns>
        public bool ValidateTreatmentGroupFile(Analysis analysis)
        {
            return (!String.IsNullOrEmpty(analysis.IdentifierFile));
        }

        /// <summary>
        /// Processes treatment group file
        /// </summary>
        /// <param name="userAnalysis">The Analysis to process treatment group file for/</param>
        /// <returns>string array of job ids.</returns>
        public string[] ProcessTreatmentGroupFile(Analysis userAnalysis)
        {
            string ext = Path.GetExtension(userAnalysis.IdentifierFile);
            string[] jobStudyIds = null;
            if (!String.IsNullOrEmpty(userAnalysis.IdentifierFile) && (Path.GetExtension(userAnalysis.IdentifierFile).Contains("txt") || Path.GetExtension(userAnalysis.IdentifierFile).Contains("csv")))
            {
                string[] jobStudyIdsLines = null;
                List<string> jobStudyIdsList = new List<string>();
                jobStudyIdsLines = System.IO.File.ReadAllLines(userAnalysis.IdentifierFile);
                foreach (string line in jobStudyIdsLines)
                {
                    string[] tlist = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string id in tlist)
                    {
                        jobStudyIdsList.Add(id.Trim());
                    }
                }
                jobStudyIds = jobStudyIdsList.ToArray();
            }
            else if (!String.IsNullOrEmpty(userAnalysis.IdentifierFile) && Path.GetExtension(userAnalysis.IdentifierFile).Contains("xlsx"))
            {
                var excel = new ExcelQueryFactory(userAnalysis.IdentifierFile);
                var worksheetName = excel.GetWorksheetNames().First();
                string name = Path.GetFileNameWithoutExtension(excel.FileName);
                if (Path.GetFileNameWithoutExtension(excel.FileName) == "EE ID Submission Template")
                {
                    var jobIds = (from c in excel.WorksheetNoHeader()
                                  select c[0]).Skip(3).ToArray();
                    jobStudyIds = Array.ConvertAll(jobIds, item => item.ToString().Trim());
                }
                else if (excel.GetColumnNames(worksheetName).First() != userAnalysis.UseIdentifier)
                {
                    var jobIds = (from c in excel.WorksheetNoHeader()
                                  select c[0]).ToArray();
                    jobStudyIds = Array.ConvertAll(jobIds, item => item.ToString().Trim());
                }
                else
                {
                    var jobIds = (from c in excel.WorksheetNoHeader()
                                  select c[0]).Skip(1).ToArray();
                    jobStudyIds = Array.ConvertAll(jobIds, item => item.ToString().Trim());
                }
            }

            return jobStudyIds;
        }

        /// <summary>
        /// Processes the treatment group text.
        /// </summary>
        /// <param name="userAnalysis">The user analysis.</param>
        /// <returns>Array of strings, each element is a student ID.</returns>
        public string[] ProcessTreatmentGroupText(Analysis userAnalysis)
        {
            string idlist = userAnalysis.IdentifierList;
            string[] rows = idlist.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var jobStudyIds = (from row in rows from id in row.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries) select id.Trim()).ToArray();
            return jobStudyIds;

        }

        /// <summary>
        /// Generates the R server request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="api">The API version number.</param>
        public void GenerateRServerRequest(string id, string api)
        {
            string endPoint = ConfigurationManager.AppSettings["RServerURL"];
            var client = new RestClient(endPoint);
            //client.Method = HttpVerb.POST;
            var json = client.MakeRequest("?" + ConfigurationManager.AppSettings["RServerParamName"] + "=" + id + "&" + ConfigurationManager.AppSettings["RServerParamName2"] + "=" + api);
        }

        /// <summary>
        /// Gets the name of the outcome title by.
        /// </summary>
        /// <param name="stateAbbv">The state abbreviation.</param>
        /// <param name="outcomeName">Name of the outcome.</param>
        /// <param name="isChart">True if this title is for a chart.</param>
        /// <param name="type">The type of the chart, scale score, percent proficient or rate.</param>
        /// <returns>System.String.</returns>
        public string GetOutcomeTitleByName(string stateAbbv, string outcomeName, bool isChart, string type)
        {
            string title = "";

            title += _analysisRepository.GetOutcomeTitleByStateAndValue(_stateRepository.GetStateId(stateAbbv), outcomeName);

           /* if (isChart)
            {
                if (type == "pp")
                {
                    title += " percent proficient or advanced";
                }
                else if (type == "ss")
                {
                    title += ", average scale score";
                }
                else if (type == "rate")
                {
                    title += " rate";
                }                  
            } */

            return title;
        }

        /// <summary>
        /// Gets the figure title.
        /// </summary>
        /// <param name="outcome">The outcome.</param>
        /// <param name="hasPP">if set to <c>true</c> [has pp].</param>
        /// <param name="currentfignum">The currentfignum.</param>
        /// <param name="type">The type.</param>
        /// <param name="stateAbbv">The state abbv.</param>
        /// <returns>System.String.</returns>
        public string GetFigureTitle(JobResults_Outcomes outcome, bool hasPP, int currentfignum, string type, string stateAbbv)
        {
            string title = "Figure ";

            title += currentfignum.ToString();
            if (hasPP)
            {
                if (type == "ss")
                {
                    title += "A";
                    title += ": Average scale scores in ";
                }
                if (type == "pp")
                {
                    title += "B";
                    title += ": Proficiency rates in ";
                }
            }
            else
            {
                title += ": " + _analysisRepository.GetFigureTitleByStateAndValue(_stateRepository.GetStateId(stateAbbv), outcome.OutcomeName) + " ";
            }

            title += _analysisRepository.GetSubjectByStateAndValue(_stateRepository.GetStateId(stateAbbv), outcome.OutcomeName);

            //title += outcome.OutcomeYear;

            return title;
        }

        /// <summary>
        /// Gets the data source.
        /// </summary>
        /// <param name="stateAbbv">The state abbv.</param>
        /// <param name="testName">Name of the test.</param>
        /// <param name="date">The date.</param>
        /// <param name="dataNote">The note from the statistical backend about the data.</param>
        /// <returns>System.String.</returns>
        public string GetDataSource(string stateAbbv, string testName, string date, string dataNote)
        {
            string datasource = "";
            if (String.IsNullOrEmpty(dataNote))
            {
                datasource += "<p><span class=\"sm-font\">Source: " +
                              _analysisRepository.GetStateDataSourceByStateAndValue(
                                  _stateRepository.GetStateId(stateAbbv));
                datasource +=
                    _analysisRepository.GetOutcomeDataSourceByStateAndValue(
                        _stateRepository.GetStateId(stateAbbv), testName) + "</span></p>";
            }
            else
            {
                datasource += "<p><span class=\"sm-font\">Data: " + dataNote + "</span></p>";
                datasource += "<p><span class=\"sm-font\">Source: " +
                              _analysisRepository.GetStateDataSourceByStateAndValue(
                                  _stateRepository.GetStateId(stateAbbv));
                datasource +=
                    _analysisRepository.GetOutcomeDataSourceByStateAndValue(
                        _stateRepository.GetStateId(stateAbbv), testName) + "</span></p>";
            }
            //datasource += "<br />Created by Evaluation Engine 1.0, " + date + "</span></p>";
            return datasource;
        }

        /// <summary>
        /// Gets the outcome note.
        /// </summary>
        /// <param name="outcome">The outcome.</param>
        /// <param name="hasPP">if set to <c>true</c> [has pp].</param>
        /// <param name="type">The type.</param>
        /// <returns>System.String.</returns>
        public string GetOutcomeNote(JobResults_Outcomes outcome, bool hasPP, string type, int fignum)
        {
            string note;

            if (hasPP && type == "ss")
            {
                note = "Please see note below Figure " + Convert.ToString(fignum) + "B.";
            }
            else if (hasPP && type == "pp")
            {
                note = "Notes: " + outcome.OutcomeNote;
            }
            else if (!hasPP && type == "ss")
            {
                note = "Notes: " + outcome.OutcomeNote;
            }
            else
            {
                note = "";
            }

            return note;
        }

        /// <summary>
        /// Determines whether the specified outcome has a percent proficient table/charts.
        /// </summary>
        /// <param name="outcome">The outcome.</param>
        /// <returns><c>true</c> if [has percent proficient] [the specified outcome]; otherwise, <c>false</c>.</returns>
        public bool hasPercentProficient(JobResults_Outcomes outcome)
        {
            return (!String.IsNullOrEmpty(outcome.ChartValues_PercentProficient_Control) && outcome.ChartValues_PercentProficient_Control != "NA");
        }

        /// <summary>
        /// Prepares the report view.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <param name="analysis">The analysis.</param>
        /// <returns>Step4Model.</returns>
        public Step4Model PrepareReportView(Job job, Analysis analysis)
        {
            int stateId = _stateRepository.GetStateId(analysis.State);
            ReportGenerator rg = new ReportGenerator(_analysisRepository);
            JobResults results = _jobResultsRepository.GetJobResultByJobGUID(analysis.JobGUID);
            IOrderedEnumerable<JobResults_Outcomes> outcomes = _jobResultsRepository.GetOutcomesByJobGUID(analysis.JobGUID).OrderBy(x => x.OutcomeYear).ThenBy(x => x.Rank );
            //outcomes.OrderBy(x => x.OutcomeName).ThenBy(x => x.OutcomeYear);
            outcomes = outcomes.OrderBy(x => x.Rank);
            Step4Model model = new Step4Model();
            model.Report = new ReportModel();
            model.Report.Title = results.Title;
            model.Report.IsMultiGrade = analysis.InterventionGradeLevels.Contains((","));
            model.Report.GradeList = analysis.InterventionGradeLevels.Contains((",")) ? ReplaceLastOccurrence(analysis.InterventionGradeLevels,","," and ").Replace(",",", ") : analysis.InterventionGradeLevels;
            model.Report.Header = results.IntroText;
            model.Report.AnalysisName = analysis.AnalysisName;
            model.Report.StudyName = analysis.StudyName;
            model.Report.BalanceChart = results.Image;
            model.Report.AnalysisDescription = analysis.AnalysisDescription;
            model.Report.StudyDescription = analysis.StudyDescription;
            model.Report.ControlCount = results.ControlCount.ToString();
            model.Report.TreatmentCount = results.TreatmentCount.ToString();
            model.Report.TreatmentExcludedCount = results.TreatmentExcludedCount.ToString();
            model.Report.DataText = _stateRepository.GetStateDataText(analysis.State);
            model.Report.BalanceMainPval = results.BalanceMainPval;
            model.Report.BalanceInclusivePval = results.BalanceInclusivePval;
            model.Report.WithinDistrictMatchesPct = results.WithinDistrictMatchesPct;
            model.Report.GeneratedOn = analysis.CreatedOn;
            model.Report.Subgroups = analysis.SubgroupAnalyses;
            model.Report.JobGUID = analysis.JobGUID;
            model.Report.SupplementalInformation = results.SupplementalInformation;
            model.Report.ChartCollection = new List<ReportChart>();
            model.Report.TableCollection = new List<ReportTable>();
            model.Report.LibraryEntry =  new LibraryItem
                {
                    JobGUID = job.JobGUID.ToString(),
                    State = job.State,
                    Status = _jobResultsRepository.GetJobStatusNameById(job.StatusId),
                    Link = analysis.Id.ToString(),
                    Name = analysis.AnalysisName,
                    Params = "Analysis Name: " + analysis.AnalysisName + "<br/>" +
                                "Study Name: " + analysis.StudyName + "<br/>" +
                                "Intervention Start: " + analysis.InterventionStartDate + "<br/>" +
                                "Intervention End: " + analysis.InterventionEndDate + "<br/>" +
                                "Outcome Areas: " + analysis.OutcomeAreas + "<br/>" +
                                "Subgroups: " + analysis.SubgroupAnalyses + "<br/>" +
                                "Grade Levels: " + analysis.InterventionGradeLevels
                };

            int fignum = 1;

            foreach (var outcome in outcomes)
            {
                if (!String.IsNullOrEmpty(outcome.OrdinalAverages))
                {
                    ReportChart stackedchart = new ReportChart
                    {
                        OutcomeTitle = outcome.Title,
                        OutcomeNote = GetOutcomeNote(outcome, hasPercentProficient(outcome), "ss", fignum),
                        Title = GetOutcomeTitleByName(analysis.State, outcome.OutcomeName, true, "ss") + ", " + outcome.OutcomeYear,
                        Header = GetFigureTitle(outcome, hasPercentProficient(outcome), fignum, "ss", analysis.State),
                        OutcomeName = outcome.OutcomeName,
                        Note = outcome.DataNote_ScaleScore,
                        HTML = "",
                        Footer = GetDataSource(analysis.State, outcome.OutcomeName, analysis.CreatedOn.Date.ToShortDateString(), outcome.DataNote),
                        Chart = outcome.Chart_ScaleScore,
                        Rank = outcome.Rank,
                        Type = "stacked",
                        OutcomeYear = outcome.OutcomeYear,
                        ChartData_Intervention = "",
                        ChartData_Control = "",
                        ChartData_Control_SD = "",
                        ChartData_Intervention_SD = "",
                        ChartData_PerGrade = rg.GetStackedChartValues(outcome.OrdinalAverages, _stateRepository.GetStateId(analysis.State)),
                        //ChartData_Intervention = this.analysisRepository.IsOutcomePercentByStateAndValue(this.stateRepository.GetStateId(analysis.State), outcome.OutcomeName) ? formatPercent(outcome.ChartValues_ScaleScore_Intervention, 0) : String.Format("{0:0}", Math.Ceiling(Convert.ToDouble(outcome.ChartValues_ScaleScore_Intervention))),
                        //ChartData_Control = this.analysisRepository.IsOutcomePercentByStateAndValue(this.stateRepository.GetStateId(analysis.State), outcome.OutcomeName) ? formatPercent(outcome.ChartValues_ScaleScore_Control, 0) : String.Format("{0:0}", Math.Ceiling(Convert.ToDouble(outcome.ChartValues_ScaleScore_Control))),
                        Format = _analysisRepository.IsOutcomePercentByStateAndValue(_stateRepository.GetStateId(analysis.State), outcome.OutcomeName) ? "percent" : "string",
                        ChartData_EffectSize = rg.FormatStringAsNumber(outcome.ChartValues_Main_Difference.ToString()),
                        ChartData_Proficiency_Control = "",
                        ChartData_Proficiency_Intervention = "",
                        GradeList = analysis.InterventionGradeLevels,
                        ErrorMessage = outcome.ErrorMessages
                    };
                    model.Report.ChartCollection.Add(stackedchart);
                }
                else if (String.IsNullOrEmpty(outcome.PerGrade_Averages_YAML))
                {
                    if (!String.IsNullOrEmpty(outcome.Chart_ScaleScore) && outcome.ChartValues_ScaleScore_Control != "NA" && outcome.ChartValues_ScaleScore_Intervention != "NA")
                    {
                        string type = (!hasPercentProficient(outcome)) ? "rate" : "ss";
                        ReportChart chart = new ReportChart
                        {
                            OutcomeTitle = outcome.Title,
                            OutcomeNote = GetOutcomeNote(outcome, hasPercentProficient(outcome), type, fignum),
                            Title = GetOutcomeTitleByName(analysis.State, outcome.OutcomeName, true, type) + ", " + outcome.OutcomeYear,
                            Header = GetFigureTitle(outcome, hasPercentProficient(outcome), fignum, type, analysis.State),
                            OutcomeName = outcome.OutcomeName,
                            Note = outcome.DataNote_ScaleScore,
                            HTML = "",
                            Footer = GetDataSource(analysis.State, outcome.OutcomeName, analysis.CreatedOn.Date.ToShortDateString(), outcome.DataNote),
                            Chart = outcome.Chart_ScaleScore,
                            Rank = outcome.Rank,
                            Type = type,
                            OutcomeYear = outcome.OutcomeYear,
                            ChartData_Intervention = _analysisRepository.IsOutcomePercentByStateAndValue(_stateRepository.GetStateId(analysis.State), outcome.OutcomeName) ? formatPercent(outcome.ChartValues_ScaleScore_Intervention, 0) : String.Format("{0:0}", Math.Ceiling(Convert.ToDouble(outcome.ChartValues_ScaleScore_Intervention))),
                            ChartData_Control = _analysisRepository.IsOutcomePercentByStateAndValue(_stateRepository.GetStateId(analysis.State), outcome.OutcomeName) ? formatPercent(outcome.ChartValues_ScaleScore_Control, 0) : String.Format("{0:0}", Math.Ceiling(Convert.ToDouble(outcome.ChartValues_ScaleScore_Control))),
                            Format = _analysisRepository.IsOutcomePercentByStateAndValue(_stateRepository.GetStateId(analysis.State), outcome.OutcomeName) ? "percent" : "string",
                            GradeList = analysis.InterventionGradeLevels,
                            ErrorMessage = outcome.ErrorMessages
                        };
                        model.Report.ChartCollection.Add(chart);
                    }

                    if (!String.IsNullOrEmpty(outcome.Chart_PercentProficient) && outcome.ChartValues_PercentProficient_Control != "NA" && outcome.ChartValues_PercentProficient_Intervention != "NA")
                    {
                        ReportChart ppChart = new ReportChart
                        {
                            OutcomeTitle = outcome.Title,
                            OutcomeNote = GetOutcomeNote(outcome, hasPercentProficient(outcome), "pp", fignum),
                            Title = GetOutcomeTitleByName(analysis.State, outcome.OutcomeName, true, "pp") + ", " + outcome.OutcomeYear,
                            Header = GetFigureTitle(outcome, hasPercentProficient(outcome), fignum, "pp", analysis.State),
                            OutcomeName = outcome.OutcomeName,
                            Note = outcome.DataNote_PercentProficient,
                            HTML = "",
                            Footer = GetDataSource(analysis.State, outcome.OutcomeName, analysis.CreatedOn.Date.ToShortDateString(), outcome.DataNote),
                            Chart = outcome.Chart_PercentProficient,
                            Rank = outcome.Rank,
                            Type = "pp",
                            OutcomeYear = outcome.OutcomeYear,
                            ChartData_Intervention = formatPercent(outcome.ChartValues_PercentProficient_Intervention, 0),
                            ChartData_Control = formatPercent(outcome.ChartValues_PercentProficient_Control, 0),
                            Format = _analysisRepository.IsOutcomePercentByStateAndValue(_stateRepository.GetStateId(analysis.State), outcome.OutcomeName) ? "percent" : "string",
                            GradeList = analysis.InterventionGradeLevels,
                            ErrorMessage = outcome.ErrorMessages
                        };
                        model.Report.ChartCollection.Add(ppChart);
                    }
                }

                else 
                {
                        ReportChart chart = new ReportChart
                        {
                            OutcomeTitle = outcome.Title,
                            OutcomeNote = GetOutcomeNote(outcome, hasPercentProficient(outcome), "ss", fignum),
                            Title = GetOutcomeTitleByName(analysis.State, outcome.OutcomeName, true, "ss") + ", " + outcome.OutcomeYear,
                            Header = GetFigureTitle(outcome, hasPercentProficient(outcome), fignum, "ss", analysis.State),
                            OutcomeName = outcome.OutcomeName,
                            Note = outcome.DataNote_ScaleScore,
                            HTML = "",
                            Footer = GetDataSource(analysis.State, outcome.OutcomeName, analysis.CreatedOn.Date.ToShortDateString(), outcome.DataNote),
                            Chart = outcome.Chart_ScaleScore,
                            Rank = outcome.Rank,
                            Type = "ss",
                            OutcomeYear = outcome.OutcomeYear,
                            ChartData_Intervention = rg.GetChartValues(outcome.PerGrade_Averages_YAML, stateId, "ss", false),
                            ChartData_Control = rg.GetChartValues(outcome.PerGrade_Averages_YAML, stateId, "ss", true),
                            ChartData_Control_SD = rg.GetChartValues(outcome.PerGrade_Averages_YAML, stateId, "sd", true),
                            ChartData_Intervention_SD = rg.GetChartValues(outcome.PerGrade_Averages_YAML, stateId, "sd", false),
                            ChartData_PerGrade = rg.GetChartDataJSON(outcome.OutcomeName, rg.GetChartValues(outcome.PerGrade_Averages_YAML, stateId, "ss", true), rg.GetChartValues(outcome.PerGrade_Averages_YAML, stateId, "ss", false), rg.GetChartGrades(outcome.PerGrade_Averages_YAML, stateId), _analysisRepository.IsOutcomePercentByStateAndValue(_stateRepository.GetStateId(analysis.State), outcome.OutcomeName), rg.GetChartValues(outcome.PerGrade_Averages_YAML, stateId, "sd", true), rg.GetChartValues(outcome.PerGrade_Averages_YAML, stateId, "sd", false)),
                            //ChartData_Intervention = this.analysisRepository.IsOutcomePercentByStateAndValue(this.stateRepository.GetStateId(analysis.State), outcome.OutcomeName) ? formatPercent(outcome.ChartValues_ScaleScore_Intervention, 0) : String.Format("{0:0}", Math.Ceiling(Convert.ToDouble(outcome.ChartValues_ScaleScore_Intervention))),
                            //ChartData_Control = this.analysisRepository.IsOutcomePercentByStateAndValue(this.stateRepository.GetStateId(analysis.State), outcome.OutcomeName) ? formatPercent(outcome.ChartValues_ScaleScore_Control, 0) : String.Format("{0:0}", Math.Ceiling(Convert.ToDouble(outcome.ChartValues_ScaleScore_Control))),
                            Format = _analysisRepository.IsOutcomePercentByStateAndValue(_stateRepository.GetStateId(analysis.State), outcome.OutcomeName) ? "percent" : "string",
                            ChartData_EffectSize = rg.FormatStringAsNumber(outcome.ChartValues_Main_Difference.ToString()),
                            ChartData_Proficiency_Control = "",
                            ChartData_Proficiency_Intervention = "",
                            GradeList = analysis.InterventionGradeLevels,
                            ErrorMessage = outcome.ErrorMessages

                        };
                        model.Report.ChartCollection.Add(chart);


                    if (outcome.ChartValues_PercentProficient_Control != null && outcome.ChartValues_PercentProficient_Intervention != null && outcome.ChartValues_PercentProficient_Control != "NA" && outcome.ChartValues_PercentProficient_Intervention != "NA")
                    {
                        ReportChart ppChart = new ReportChart
                        {
                            OutcomeTitle = outcome.Title,
                            OutcomeNote = GetOutcomeNote(outcome, hasPercentProficient(outcome), "pp", fignum),
                            Title = GetOutcomeTitleByName(analysis.State, outcome.OutcomeName, true, "pp") + ", " + outcome.OutcomeYear,
                            Header = GetFigureTitle(outcome, hasPercentProficient(outcome), fignum, "pp", analysis.State),
                            OutcomeName = outcome.OutcomeName,
                            Note = outcome.DataNote_PercentProficient,
                            HTML = "",
                            Footer = GetDataSource(analysis.State, outcome.OutcomeName, analysis.CreatedOn.Date.ToShortDateString(), outcome.DataNote),
                            Chart = outcome.Chart_PercentProficient,
                            Rank = outcome.Rank,
                            Type = "pp",
                            OutcomeYear = outcome.OutcomeYear,
                            ChartData_PerGrade = rg.GetChartDataJSON(outcome.OutcomeName, rg.GetChartValues(outcome.PerGrade_Averages_YAML, stateId, "pp", true), rg.GetChartValues(outcome.PerGrade_Averages_YAML, stateId, "pp", false), rg.GetChartGrades(outcome.PerGrade_Averages_YAML, stateId), true, rg.GetChartValues(outcome.PerGrade_Averages_YAML, stateId, "sd", true), rg.GetChartValues(outcome.PerGrade_Averages_YAML, stateId, "sd", false)),
                            ChartData_Intervention = rg.GetChartValues(outcome.PerGrade_Averages_YAML, stateId, "pp", false),
                            ChartData_Control = rg.GetChartValues(outcome.PerGrade_Averages_YAML, stateId, "pp", true),
                            Format = _analysisRepository.IsOutcomePercentByStateAndValue(_stateRepository.GetStateId(analysis.State), outcome.OutcomeName) ? "percent" : "string",
                            ChartData_EffectSize = rg.FormatStringAsNumber(outcome.ChartValues_Proficiency_Difference.ToString()),
                            ChartData_Proficiency_Control = formatPercent(outcome.ChartValues_PercentProficient_Control,0),
                            ChartData_Proficiency_Intervention = formatPercent(outcome.ChartValues_PercentProficient_Intervention,0),
                            GradeList = analysis.InterventionGradeLevels,
                            ErrorMessage = outcome.ErrorMessages
                        };
                        model.Report.ChartCollection.Add(ppChart);
                    }                    
                }

                if (!String.IsNullOrEmpty(outcome.OutcomeYAML))
                {
                    ReportTable outcometable = new ReportTable
                    {
                        Title = GetOutcomeTitleByName(analysis.State, outcome.OutcomeName, false, "pp") + ", " + outcome.OutcomeYear,
                        Header = outcome.OutcomeHeader,
                        OutcomeName = outcome.OutcomeName,
                        Note = GetSubgroupNote(outcome.OutcomeHeader),
                        SubgroupNote = outcome.SubgroupNote,
                        HTML = rg.GetTableHTML(outcome.OutcomeYAML, stateId),
                        Footer = GetDataSource(analysis.State, outcome.OutcomeName, analysis.CreatedOn.Date.ToShortDateString(), outcome.DataNote),
                        Table = outcome.TableNote_PercentProficient,
                        Rank = outcome.Rank,
                        Type = "pp",
                        OutcomeYear = outcome.OutcomeYear,
                        Subtitle = GetSubtitle(outcome.OutcomeNote, outcome.OutcomeHeader)
                    };
                    model.Report.TableCollection.Add(outcometable);
                }
                model.Report.JobGUID = analysis.JobGUID;
                fignum++;
            }
            return model;
        }

        /// <summary>
        /// Returns a string based on the value of outcomeHeader.
        /// </summary>
        /// <param name="outcomeHeader">The control string passeed by the R server to differentiate between difference and presence tests.</param>
        /// <returns></returns>
        private static string GetSubgroupNote(string outcomeHeader)
        {
            string note = "";
            if (!String.IsNullOrEmpty(outcomeHeader))
            {
                if (outcomeHeader.Trim() == "W.PRESENCE.TESTS")
                    note =
                        "This table presents one-sided tests of statistical significance. Smaller p-values correspond to stronger indications that the program benefited the specified subgroup. The p-values have been corrected for multiple comparisons.";
                else if (outcomeHeader.Trim() == "W.DIFFERENCE.TESTS")
                    note =
                        "This table gives presents two-sided tests of statistical significance. Smaller p-values correspond to stronger indications that the program's average effect within the specified subgroup differed from the overall average effect. The p-values have been corrected for multiple comparisons.";
                else
                note = outcomeHeader;
            }

        return note;
        }

        /// <summary>
        /// Returns a string based on the value of outcomeHeader.
        /// </summary>
        /// <param name="outcomeNote">The note of statistical significance for subgroup outcomes.</param>
        /// <param name="outcomeHeader">The control string passeed by the R server to differentiate between difference and presence tests.</param>
        /// <returns></returns>
        private static string GetSubtitle(string outcomeNote, string outcomeHeader)
        {
            string note = "";
            if (!String.IsNullOrEmpty(outcomeHeader))
            {
                if (outcomeHeader.Trim() == "W.PRESENCE.TESTS")
                    note =
                        "Effect estimates by subgroup, with tests for presence of effect";
                else if (outcomeHeader.Trim() == "W.DIFFERENCE.TESTS")
                    note =
                        "Effect estimates by subgroup, with tests for differences from overall effect";
                else
                    note = outcomeHeader;
            }
            else if (!String.IsNullOrEmpty(outcomeNote))
            {
                if (outcomeNote.Contains("not statistically significant"))
                    note ="Effect estimates by subgroup, with tests for presence of effects";
                else
                    if (outcomeNote.Contains("p<0.005"))
                    note =
                        "Effect estimates by subgroup, with tests for differences from overall effect";
                else
                    note = 
                        "";
            }

            return note;
        }

        /// <summary>
        /// Replaces the last occurence of string Find with string Replace in string Source.
        /// </summary>
        /// <param name="source">The string to search in.</param>
        /// <param name="find">The string to replace.</param>
        /// <param name="replace">The string replacement.</param>
        /// <returns>String with last occurence of Find replaced with Replace.</returns>
        private static string ReplaceLastOccurrence(string source, string find, string replace)
        {
            int place = source.LastIndexOf(find);
            string result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

        /// <summary>
        /// Formats a string as a percentage (minus the percentage sign, as the values are used for charting)
        /// </summary>
        /// <param name="value">The strign to format.</param>
        /// <param name="precision">The nubmer of decimal places to ROUND to (not output)</param>
        /// <returns>A string formatted as a percent.</returns>
        string formatPercent(string value, int precision)
        {
            double dval = Convert.ToDouble(value) * 100;
            dval = Math.Round(dval, precision);
            string ret = String.Format("{0:0}", dval);
            return ret;
        }

        /// <summary>
        /// Removes the phrase "Data source:" from a note passed from the R server.
        /// </summary>
        /// <param name="rNote">The note fromt the R server.</param>
        /// <returns>A note with the text "Data source:" removed.</returns>
        string TrimDataSource(string rNote)
        {
            string note = "";
            if (rNote.Contains("Data source:"))
            {
                note = rNote.Remove(rNote.IndexOf("Data source:"));
            }

            return note;
        }
    }
}
