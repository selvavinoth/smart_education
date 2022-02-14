using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEdu.ViewModel;
using SmartEdu.Data.Models;
using SmartEdu.Bll.Services;

namespace SmartEdu.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        private readonly ILoginService loginService;
        public LoginController(ILoginService loginService) {
            this.loginService = loginService;
        }
        public ActionResult Index()
        {
            LoginViewModel v = new LoginViewModel();
            try { return View(v); }
            catch (Exception e) { return View(v); }
            finally { v = null; }
        }
        public ActionResult Login(LoginViewModel viewmodel)
        {
            string msg = "";
            try {
                msg = ValidateLogin(viewmodel);
                if (msg != "") { return Json(new { Msg = msg, Status = false }, JsonRequestBehavior.AllowGet); }

                return Json(new { Msg = msg, Status = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e) { return Json(new { Msg = "", Status = false }, JsonRequestBehavior.AllowGet); }
            finally { msg = null; }
        }
        private string ValidateLogin(LoginViewModel viewModel) {
            try {
                if (viewModel == null) { return "Fill the form first."; }
                if (viewModel.UserName == null || viewModel.UserName.Trim() == "") { return "Please enter username"; }
                if (viewModel.Password == null || viewModel.Password.Trim() == "") { return "Please enter password."; }
                return "";
            }
            finally { }
        }
        private string AuthenticateUser(LoginViewModel viewModel){
            ADM_USER_DETAILS users = null;
            try {
                users = loginService.GerUserDetails(viewModel.UserName,viewModel.Password);
                if (users == null) { return "userame or password is incorrect."; }
                else {
                    if (users.AUD_LoginStatus == "B") { return "Account is blocked. Please contact admin."; }
                    users.AUD_LoginAttempt = (users.AUD_LoginAttempt??0) + 1;
                    users.AUD_Modified_By = 0;
                    if (users.AUD_LoginAttempt >= 5) { users.AUD_LoginStatus = "B";  loginService.UpdateUserDetail(users); return "Account is blocked. Please contact admin."; }
                    if (users.AUD_Password != viewModel.Password) { return "Enter correct password."; }
                    else { users.AUD_LoginAttempt = 0; users.AUD_LoginStatus = ""; loginService.UpdateUserDetail(users); }
                }
                return "";
            }
            finally { }
        }

    }
}
