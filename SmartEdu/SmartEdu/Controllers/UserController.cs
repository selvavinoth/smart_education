using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEdu.Bll.Services;
using SmartEdu.ViewModel;
using SmartEdu.Data.Models;
using SmartEdu.Helper;

namespace SmartEdu.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        private int CollegeId=1;
        private long UserId=0;
        private readonly IUserService userService;
        public UserController(IUserService userService)
        { 
            this.userService= userService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details()
        {
            UsersViewModel viewModel = new UsersViewModel();
            try
            {
                viewModel.AddressViewModel = new AddressViewModel();
                viewModel.UserAuthentication = new AuthenticationViewModel();
                return View(viewModel);
            }
            catch (Exception exe) { return View(viewModel); }
            finally { viewModel = null; }
        }
        public ActionResult CommonUsers()
        {
            UsersViewModel viewModel = new UsersViewModel();
            try
            {
                viewModel.AddressViewModel = new AddressViewModel();
                viewModel.UserAuthentication = new AuthenticationViewModel();
                viewModel.UserDetailList = GetUserDetails();
                return View(viewModel);
            }
            catch (Exception exe) { return View(viewModel); }
            finally { viewModel = null; }
        }
        private List<DataKeeper.UserDetails> GetUserDetails() {
            List<DataKeeper.UserDetails> data = new List<DataKeeper.UserDetails>();
            try {
                data.Add(DataKeeper.UserDetails.AU_FirstName);
                data.Add(DataKeeper.UserDetails.AU_MiddleName);
                data.Add(DataKeeper.UserDetails.AU_LastName);
                data.Add(DataKeeper.UserDetails.AU_Email_Id);
                data.Add(DataKeeper.UserDetails.AU_MobileNo);
                data.Add(DataKeeper.UserDetails.AU_PhoneNo);
                data.Add(DataKeeper.UserDetails.AU_Gender);
                data.Add(DataKeeper.UserDetails.AU_DOB);
                data.Add(DataKeeper.UserDetails.AU_DOJ);
                data.Add(DataKeeper.UserDetails.AU_FatherName);
                return data;
            }
            finally { }
        }
        [HttpPost]
        public ActionResult UserIUAction(UsersViewModel viewModel) {
            try {
                if (viewModel == null) { return Json(new { Status=false,Msg="No Data Found."}); }

                return Json(false);
            }
            catch (Exception exe) { return Json(false); }
            finally { }
        }

        private void FillUserModel(UsersViewModel viewModel,ADM_USERS model) {
            try {
                model.AU_Title = viewModel.AU_Title;
                model.AU_Anniversary_Date = viewModel.AU_Anniversary_Date;
                model.AU_CollegeId = CollegeId;
                model.AU_Department = viewModel.AU_Department;
                model.AU_DOB = viewModel.AU_DOB;
                model.AU_DOJ = viewModel.AU_DOJ;
                model.AU_Email_Id = viewModel.AU_Email_Id;
                model.AU_EndDate = viewModel.AU_EndDate;
                model.AU_FatherName = viewModel.AU_FatherName;
                model.AU_FatherPhoneNo = viewModel.AU_FatherPhoneNo;
                model.AU_FaxNo = viewModel.AU_FaxNo;
                model.AU_FirstName = viewModel.AU_FirstName;
                model.AU_Gender = viewModel.AU_Gender;
                model.AU_Id = viewModel.AU_Id;
                model.AU_IsFA = viewModel.AU_IsFA;
                model.AU_LastName = viewModel.AU_LastName;
                model.AU_MiddleName = viewModel.AU_MiddleName;
                model.AU_MobileNo = viewModel.AU_MobileNo;
                model.AU_PhoneNo = viewModel.AU_PhoneNo;
                model.AU_Qualification = viewModel.AU_Qualification;
                model.AU_StartDate = viewModel.AU_StartDate;
                model.AU_User_Type = "U";
            }

            finally { }
        }
        private ADM_USER_AUTHENTICATION FillUserModel(UsersViewModel viewModel)
        {
            ADM_USER_AUTHENTICATION model = new ADM_USER_AUTHENTICATION();
            try
            {
                AuthenticationViewModel userAuthModel=viewModel.UserAuthentication;
                model.AUA_Answer = userAuthModel.AUT_Answer;
                model.AUA_College_Id = CollegeId;
                model.AUA_Id = userAuthModel.AUT_Id;
                model.AUA_Login_Id = userAuthModel.AUT_Login_Id;
                model.AUA_Password = userAuthModel.AUT_Password;
                //model.AUA_Question = userAuthModel.AUT_Question;
                return model;
            }
            finally { model = null; }
        }
        private List<ADM_USER_ADDRESS> GetUserAddressList(List<AddressViewModel> addressList) {
            List<ADM_USER_ADDRESS> addList = new List<ADM_USER_ADDRESS>();
            ADM_USER_ADDRESS model = null;
            try {
                foreach (AddressViewModel viewModel in addressList) {
                    model = new ADM_USER_ADDRESS();
                    model.AUAD_Address1 = viewModel.AD_Address1;
                    model.AUAD_Address2 = viewModel.AD_Address2;
                    model.AUAD_AddressType = viewModel.AD_AddressType;
                    model.AUAD_City = viewModel.AD_City;
                    model.AUAD_College_Id = viewModel.AD_College_Id;
                    model.AUAD_Country = viewModel.AD_Country;
                    model.AUAD_Id = viewModel.AD_Id;
                    model.AUAD_IsPrimary = viewModel.AD_IsPrimary;
                    model.AUAD_Pincode = viewModel.AD_Pincode;
                    model.AUAD_State = viewModel.AD_State;
                    model.AUAD_Street = viewModel.AD_Street;
                    addList.Add(model);
                }
                return addList;
            }
            finally { model = null; addList = null; }
        }
        private string ValidateUser(UsersViewModel viewModel) {
            AuthenticationViewModel auViewModel = viewModel.UserAuthentication;
            try {
                if (auViewModel == null) { return " Please Fill Authentication Tab."; }
                if (auViewModel.AUT_Login_Id == null || auViewModel.AUT_Login_Id.Trim()=="") { return " Please enter user name."; }
                if (auViewModel.AUT_Password == null || auViewModel.AUT_Password.Trim() == "") { return " Please enter password."; }
                if (auViewModel.AUT_Confirm_Password == null || auViewModel.AUT_Confirm_Password.Trim() == "") { return " Please enter confirm password."; }
                if (auViewModel.AUT_Password != auViewModel.AUT_Confirm_Password) { return " Password and Confirm password must be same."; }
                return "";
            }
            finally { auViewModel = null; }
        }
        [HttpPost]
        public ActionResult DeleteUser(long userId) {
            try {
                return Json(false);
            }
            catch (Exception exe) { return Json(false); }
            finally { }
        }
    }
}
