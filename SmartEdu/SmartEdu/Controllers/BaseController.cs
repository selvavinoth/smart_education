using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEdu.SessionHolder;
using System.Threading;
using System.Globalization;
using SmartEdu.Helper;

namespace SmartEdu.Controllers
{
    /// <summary>
    /// It is the base class for each controller in this application. Some function are overridden to redefine the process flow
    /// </summary>
    public class BaseController : Controller
    {

        /// <summary>
        ///  Called automatically when authorized [Authorize] action method is called in controller class.
        ///  filtercontext wrapped the User identity. It redirects the execution flow to login controller (set
        ///  in web config : authentication mode="Forms"
        ///  if authentication fails.   
        /// </summary>
        /// <param name="filterContext">contains the principal object which wraps user identity and role</param>
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!string.IsNullOrEmpty(SessionPersistor.UserName))
            {
                filterContext.HttpContext.User =
                    new CustomPrincipal(
                        new CustomIdentity(
                            SessionPersistor.UserFullName));
            }
            //else if (filterContext.HttpContext.Request.IsAjaxRequest()) // Session Timeout check
            //{
            //    filterContext.Result = new JsonResult
            //    {
            //        Data = new { SessionTimeoutMessage = "SessionTimeout" },
            //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //    };
            //}

            base.OnAuthorization(filterContext);
        }
        /// <summary>
        /// Called automatically after the controller action executed
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Is it View ?
            ViewResultBase view = filterContext.Result as ViewResultBase;
            if (view == null) // if not exit
                return;

            string cultureName = Thread.CurrentThread.CurrentCulture.Name; // e.g. "en-US" // filterContext.HttpContext.Request.UserLanguages[0]; // needs validation return "en-us" as default            

            // Is it default culture? exit
            if (cultureName == CultureHelper.GetDefaultCulture())
                return;



            filterContext.Controller.ViewBag._culture = "." + cultureName;

            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// called automatically before the controller 's action called.
        /// </summary>
        protected override void ExecuteCore()
        {
            string cultureName = null;
            // Attempt to read the culture cookie from Request
            // obtain the culture string from mime header/cookie based on which the language labeling will be changed in the browser.
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages[0]; // obtain it from HTTP header AcceptLanguages

            // Validate culture name
            cultureName = CultureHelper.GetValidCulture(cultureName); // This is safe

            // Modify current thread's culture            
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);//"es-CL");//
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);//"es-CL");//
            base.ExecuteCore();
        }

    }
}