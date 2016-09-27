// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// <summary>
//   The home controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using EvalEngine.Domain.Abstract;
using EvalEngine.Domain.Entities;
using EvalEngine.UI.Models;

namespace EvalEngine.UI.Controllers
{
    #region

    

    #endregion

    /// <summary>
    /// The home controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The job results repository.
        /// </summary>
        private readonly IJobResultsRepository _jobResultsRepository;

        /// <summary>
        /// The job results repository.
        /// </summary>
        private readonly IAnalysesRepository _analysisRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="jobResultsRepository">The job results repository.</param>
        /// <param name="analysisRepository">The analysis repository.</param>
        public HomeController(IJobResultsRepository jobResultsRepository, IAnalysesRepository analysisRepository)
        {
            _jobResultsRepository = jobResultsRepository;
            _analysisRepository = analysisRepository;
        }

        /// <summary>
        /// Index view
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The Index view</returns>
        [Authorize]
        public ActionResult Index(IndexModel model)
        {
            ViewBag.Message = "Welcome to Evaluation Engine.";
            List<Guid> myGUIDs = _analysisRepository.GetGUIDsByUserName(HttpContext.Profile.UserName);
            List<Guid> jobGUIDs = _jobResultsRepository.GetJobGUIDsByJobGUIDsAndStatus(myGUIDs, 3);
            List<Analysis> myRecentAnalyses = _analysisRepository.GetTopDoneAnalysesByJobGUIDs(jobGUIDs, 3);
            model.Reports = new List<ReportDisplay>();
            foreach (Analysis a in myRecentAnalyses)
            {
                ReportDisplay r = new ReportDisplay
                {
                    Id = a.Id.ToString(),
                    Name = a.AnalysisName
                };
                model.Reports.Add(r);

            }
            return View(model);
        }

        /// <summary>
        /// About view
        /// </summary>
        /// <returns>
        /// The About view
        /// </returns>
        [Authorize]        
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Help view
        /// </summary>
        /// <returns>
        /// The Help view
        /// </returns>
        [Authorize]
        public ActionResult Help()
        {
            var list = new SelectList(new[]
                                          {
                                              new {ID="Functionality",Name="Functionality"},
                                              new{ID="Data",Name="Data"},
                                              new{ID="General question",Name="General question"},
                                              new{ID="General comment",Name="General comment"},
                                              new{ID="Other",Name="Other"}
                                          },
                                      "ID", "Name", 1);
            ViewData["list"] = list;
            return View();
        }

        /// <summary>
        /// Help action
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="user">The user.</param>
        /// <returns>The Help view</returns>
        [Authorize]
        [HttpPost]
        public ActionResult Help(HelpModel model, IPrincipal user)
        {
            if (ModelState.IsValid)
            {
                MembershipUser muser = Membership.GetUser(user.Identity.Name);

                // send email
                if (muser != null)
                {
                    var message = new MailMessage
                    {
                        Subject = model.Category + ": " + model.Subject,
                        Body = model.Description + "\r\n\r\n" + muser.Email
                    };
                    message.To.Add(ConfigurationManager.AppSettings["InfoEmailAccount"]);
                    message.From = new MailAddress(muser.Email);
                    SendEmail(message);
                }

                return RedirectToAction("Help");
            }

            return View(model);
        }

        /// <summary>
        /// Documentation view
        /// </summary>
        /// <returns>
        /// The Documentation view
        /// </returns>
        [Authorize]
        public ActionResult Documentation()
        {
            return View();
        }

        /// <summary>
        /// Send emails to users.
        /// </summary>
        /// <param name="message">The messsage to be sent.</param>
        [NonAction]
        public void SendEmail(MailMessage message)
        {
            var section = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
            var client = new SmtpClient {EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"])};
            client.Send(message);
        }
    }
}
