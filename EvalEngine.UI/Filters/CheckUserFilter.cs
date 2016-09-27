using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EvalEngine.UI.Models;

namespace EvalEngine.UI.Filters
{
    public class CheckUserActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["fullName"] == null && HttpContext.Current.User.Identity.IsAuthenticated == true)
            {
                var user = Membership.GetUser(HttpContext.Current.User.Identity.Name);
                UserProfile profile = UserProfile.GetUserProfile(user.UserName);
                HttpContext.Current.Session["fullName"] = profile.FirstName + " " + profile.LastName;
            }

        }
    }
}
