using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEdu.ViewModel;
using SmartEdu.Helper;
using SmartEdu.Data.Models;
using SmartEdu.Bll.Services;
using SmartEdu.SessionHolder;

namespace SmartEdu.Controllers
{
    public class StaffController : Controller
    {
        //
        // GET: /Staff/
        private int TenantId {
            get {
                return SessionPersistor.CollegeId;
            }
        }
        private long SelectedStaffId
        {
            get
            {
                return Convert.ToInt64(SessionPersistor.getTempValue(DataKeeper.SelectedStaffId));
            }
            set { SessionPersistor.setTempValue(DataKeeper.SelectedStaffId, value); }
        }
        private int CollegeId=1;
        private long UserId = 0;
        private string RenderingField = "AU_Title,AU_FirstName,AU_MiddleName,AU_LastName,AU_Gender,AU_Department,AU_DOB,AU_DOJ,AU_Anniversary_Date,AU_StartDate,AU_Qualification,AU_IsFA,AU_Email_Id,AU_PhoneNo,AU_MobileNo,AU_FatherName,AU_FatherPhoneNo,AU_FaxNo,AU_Id";
        private readonly  IStaffService staffService;
        private readonly ISharedService sharedService;
        public StaffController(IStaffService staffService, ISharedService sharedService) { this.staffService = staffService; this.sharedService = sharedService; }
        private StaffViewModel staffViewModel = null;

        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception exe) { return View(); }
            finally { }
        }
        public ActionResult Details(long ?id)
        {
            staffViewModel = new StaffViewModel();
            try
            {
                staffViewModel.UserViewModel = new UsersViewModel();
                staffViewModel.UserViewModel.UserDetailList = GetUserDetails();
                staffViewModel.AddressViewModel = new AddressViewModel();
                staffViewModel.AddressViewModel.AD_GridLoadUrl = Url.Content("~/Staff/GetStaffAddressData");
                staffViewModel.UserAuthentication = new AuthenticationViewModel();
                FillDetailsViewModel();
                if ((id ?? 0) > 0) {
                    SelectedStaffId = id.Value;
                    FillAuthenticationDetails(id ?? 0, staffViewModel.UserAuthentication);
                    FillStudentDetails(id ?? 0, staffViewModel.UserViewModel);
                }
                return View(staffViewModel);
            }
            catch (Exception exe) { return View(staffViewModel); }
            finally { staffViewModel = null; }
        }
        private List<DataKeeper.UserDetails> GetUserDetails()
        {
            List<DataKeeper.UserDetails> data = new List<DataKeeper.UserDetails>();
            try
            {
               // if (RenderingField == null || RenderingField.Trim() == "") { return new List<DataKeeper.UserDetails>(); }
                data.Add(DataKeeper.UserDetails.AU_Title);
                data.Add(DataKeeper.UserDetails.AU_FirstName);
                data.Add(DataKeeper.UserDetails.AU_MiddleName);
                data.Add(DataKeeper.UserDetails.AU_LastName);
                data.Add(DataKeeper.UserDetails.AU_Gender);
                data.Add(DataKeeper.UserDetails.AU_Department);
                data.Add(DataKeeper.UserDetails.AU_DOB);
                data.Add(DataKeeper.UserDetails.AU_DOJ);
                data.Add(DataKeeper.UserDetails.AU_Anniversary_Date);
                data.Add(DataKeeper.UserDetails.AU_StartDate);
                data.Add(DataKeeper.UserDetails.AU_Qualification);
                data.Add(DataKeeper.UserDetails.AU_IsFA);
                data.Add(DataKeeper.UserDetails.AU_Email_Id);
                data.Add(DataKeeper.UserDetails.AU_PhoneNo);
                data.Add(DataKeeper.UserDetails.AU_MobileNo);
                data.Add(DataKeeper.UserDetails.AU_FatherName);
                data.Add(DataKeeper.UserDetails.AU_FatherPhoneNo);
                data.Add(DataKeeper.UserDetails.AU_FaxNo);
                data.Add(DataKeeper.UserDetails.AU_Id);
                return data;
            }
            finally { data = null; }
        }

        public ActionResult IsUserAvailable(string loginId) {
            bool flag = false;
            try {
                if (loginId == null || loginId.Trim() == "") { return Json(new { Msg="Please enter the user name." },JsonRequestBehavior.AllowGet); }
                flag = staffService.IsUserAvailable(loginId,SelectedStaffId);
                return Json(new { Msg=(flag?"User name available.": (SelectedStaffId==0?" User name already exist.":"Please Enter different user name."))}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
        }

        public ActionResult GetStaffData() {
            try {
                return Json(new { GridData=staffService.GetGridData(CollegeId,Convert.ToString(DataKeeper.LOVTypeKeeper.Department))}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
        }
        public ActionResult GetStaffAddressData()
        {
            try
            {
                if (SelectedStaffId <= 0) { return Json( new { total_count = 0, rows = new { } }, JsonRequestBehavior.AllowGet); }
                return Json(staffService.GetAddressGridData(CollegeId,SelectedStaffId, Convert.ToString(DataKeeper.LOVTypeKeeper.AddressType)) , JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
        }

        private void FillDetailsViewModel()
        {
            List<ADM_LOV> LovList = null;
            string type="";
            try {
                LovList = sharedService.GetLovList(CollegeId, new List<string>() { Convert.ToString(DataKeeper.LOVTypeKeeper.AddressType),Convert.ToString(DataKeeper.LOVTypeKeeper.Gender),Convert.ToString(DataKeeper.LOVTypeKeeper.Qualification),Convert.ToString(DataKeeper.LOVTypeKeeper.AuthenticationQuestion),Convert.ToString(DataKeeper.LOVTypeKeeper.Department)});
                type=Convert.ToString(DataKeeper.LOVTypeKeeper.AddressType);
                staffViewModel.AddressViewModel.AddressTypeList = GetLovList(LovList.FindAll(exp => exp.AL_Type == type));
                type = Convert.ToString(DataKeeper.LOVTypeKeeper.AuthenticationQuestion);
                staffViewModel.UserAuthentication.QuestionList = GetLovList(LovList.FindAll(exp => exp.AL_Type == type));
                type = Convert.ToString(DataKeeper.LOVTypeKeeper.Department);
                staffViewModel.UserViewModel.DepartmentList = GetLovList(LovList.FindAll(exp => exp.AL_Type == type));
                type = Convert.ToString(DataKeeper.LOVTypeKeeper.Gender);
                staffViewModel.UserViewModel.GenderList = GetLovList(LovList.FindAll(exp => exp.AL_Type == type));
            }
            finally { LovList = null; }
        }
        private void FillAuthenticationDetails(long staffId, AuthenticationViewModel vModel) {
            ADM_USER_DETAILS userDetail = null;
            try {
                userDetail = staffService.GetUserDetails(staffId, CollegeId);
                if (userDetail == null) { return; }
                vModel.AUT_Login_Id = userDetail.AUD_Login_Id;
                vModel.AUT_Password = userDetail.AUD_Password;
                vModel.AUT_Question = userDetail.AUD_Question;
                vModel.AUT_Answer = userDetail.AUD_Answer;
            }
            finally { userDetail = null;}
        }
        private void FillStudentDetails(long staffId,UsersViewModel viewModel)
        {
            ADM_STAFF_DETAILS model = null;
            List<DataKeeper.UserDetails> userDetailList = null;
            try {
                userDetailList = GetUserDetails();
                model = staffService.GetStaffDetails(staffId, CollegeId);
                if (model == null) { return; }
                foreach (DataKeeper.UserDetails field in userDetailList) {
                    switch (field) { 
                        case DataKeeper.UserDetails.AU_Title:
                            viewModel.AU_Title = model.ASF_Title;
                            break;
                        case DataKeeper.UserDetails.AU_FirstName:
                            viewModel.AU_FirstName = model.ASF_FirstName;
                            break;
                        case DataKeeper.UserDetails.AU_MiddleName:
                            viewModel.AU_MiddleName = model.ASF_MiddleName;
                            break;
                        case DataKeeper.UserDetails.AU_LastName:
                            viewModel.AU_LastName = model.ASF_LastName;
                            break;
                        case DataKeeper.UserDetails.AU_Gender:
                            viewModel.AU_Gender = model.ASF_Gender;
                            break;
                        case DataKeeper.UserDetails.AU_Department:
                            viewModel.AU_Department = model.ASF_Department;
                            break;
                        case DataKeeper.UserDetails.AU_DOB:
                            viewModel.AU_DOB = model.ASF_DOB;
                            break;
                        case DataKeeper.UserDetails.AU_DOJ:
                            viewModel.AU_DOJ = model.ASF_DOJ;
                            break;
                        case DataKeeper.UserDetails.AU_Anniversary_Date:
                            viewModel.AU_Anniversary_Date = model.ASF_Anniversary_Date;
                            break;
                        case DataKeeper.UserDetails.AU_StartDate:
                            viewModel.AU_StartDate = model.ASF_StartDate;
                            break;
                        case DataKeeper.UserDetails.AU_Qualification:
                            viewModel.AU_Qualification = model.ASF_Qualification;
                            break;
                        case DataKeeper.UserDetails.AU_IsFA:
                            viewModel.AU_IsFA = model.ASF_IsFA;
                            break;
                        case DataKeeper.UserDetails.AU_PhoneNo:
                            viewModel.AU_PhoneNo = model.ASF_PhoneNo;
                            break;
                        case DataKeeper.UserDetails.AU_Email_Id:
                            viewModel.AU_Email_Id = model.ASF_Email_Id;
                            break;
                        case DataKeeper.UserDetails.AU_MobileNo:
                            viewModel.AU_MobileNo = model.ASF_MobileNo;
                            break;
                        case DataKeeper.UserDetails.AU_FatherName:
                            viewModel.AU_FirstName = model.ASF_FirstName;
                            break;
                        case DataKeeper.UserDetails.AU_FatherPhoneNo:
                            viewModel.AU_FatherPhoneNo = model.ASF_FatherPhoneNo;
                            break;
                        case DataKeeper.UserDetails.AU_FaxNo:
                            viewModel.AU_FaxNo = model.ASF_FaxNo;
                            break;
                        case DataKeeper.UserDetails.AU_Id:
                            viewModel.AU_Id = model.ASF_Id;
                            break;
                        default:
                            break;
                    }
                }
            }
            finally { model = null; userDetailList = null; }
        }


        private bool NeedToShowField(string fieldName) {
            try {
                if (RenderingField == null || RenderingField.Trim() == "") { return false; }
                return RenderingField.Contains(fieldName);
            }
            finally { }
        }
        private IEnumerable<SelectListItem> GetLovList(List<ADM_LOV> LovList)
        {
            try
            {
                if (LovList == null) { return new List<SelectListItem>(); }
                return (from f in LovList
                        select new SelectListItem { Text=f.AL_Description,Value=f.AL_Id.ToString() }).ToList();
            }
            finally { }
        }
        [HttpPost]
        public ActionResult StaffIUAction(StaffViewModel viewModel)
        {
            string msg = "";
            bool flag = false;
            ADM_STAFF_DETAILS model = new ADM_STAFF_DETAILS();
            try
            {
                if (viewModel == null) { return Json(new { Status = false, Msg = "No Data Found." }); }
                msg = ValidateStaffAuthentication(viewModel.UserAuthentication);
                if (msg != "") { return Json(new { Status = false, Msg = msg }); }
                FillStaffModel(viewModel.UserViewModel, model);
                if (model.ASF_Id == 0)
                {
                    staffService.InsertStaff(model, FillUserModel(viewModel.UserAuthentication), GetUserAddressList(viewModel.AddressList), UserId);
                    msg = "Added Successfully.";
                }
                else {
                    flag=staffService.UpdateStaff(model, FillUserModel(viewModel.UserAuthentication), GetUserAddressList(viewModel.AddressList), UserId);
                    msg = "Updated Successfully.";
                }
                return Json(new { Status = true, Msg = msg });
            }
            catch (Exception exe) { return Json(false); }
            finally { msg = string.Empty; model = null; }
        }

        private void FillStaffModel(UsersViewModel viewModel, ADM_STAFF_DETAILS model)
        {
            try
            {
                model.ASF_Id = SelectedStaffId;
                model.ASF_Title = viewModel.AU_Title;
                model.ASF_Anniversary_Date = viewModel.AU_Anniversary_Date;
                model.ASF_CollegeId = CollegeId;
                model.ASF_Department = viewModel.AU_Department;
                model.ASF_DOB = viewModel.AU_DOB;
                model.ASF_DOJ = viewModel.AU_DOJ;
                model.ASF_Email_Id = viewModel.AU_Email_Id;
                model.ASF_EndDate = viewModel.AU_EndDate;
                model.ASF_FatherName = viewModel.AU_FatherName;
                model.ASF_FatherPhoneNo = viewModel.AU_FatherPhoneNo;
                model.ASF_FaxNo = viewModel.AU_FaxNo;
                model.ASF_FirstName = viewModel.AU_FirstName;
                model.ASF_Gender = viewModel.AU_Gender;
                model.ASF_IsFA = viewModel.AU_IsFA;
                model.ASF_LastName = viewModel.AU_LastName;
                model.ASF_MiddleName = viewModel.AU_MiddleName;
                model.ASF_MobileNo = viewModel.AU_MobileNo;
                model.ASF_PhoneNo = viewModel.AU_PhoneNo;
                model.ASF_Qualification = viewModel.AU_Qualification;
                model.ASF_StartDate = viewModel.AU_StartDate;
            }

            finally { }
        }
        private ADM_USER_DETAILS FillUserModel(AuthenticationViewModel userAuthModel)
        {
            ADM_USER_DETAILS model = new ADM_USER_DETAILS();
            try
            {
                if (userAuthModel == null) { return model; }
                model.AUD_Answer = userAuthModel.AUT_Answer;
                model.AUD_College_Id = CollegeId;
                model.AUD_Id = userAuthModel.AUT_Id;
                model.AUD_Login_Id = userAuthModel.AUT_Login_Id;
                model.AUD_Password = userAuthModel.AUT_Password;
                model.AUD_Question = userAuthModel.AUT_Question;
                model.AUD_User_Type = "U";
                model.AUD_User_Code = "STAFF";
                return model;
            }
            finally { model = null; }
        }
        private List<ADM_STAFF_ADDRESS> GetUserAddressList(List<AddressViewModel> addressList)
        {
            List<ADM_STAFF_ADDRESS> addList = new List<ADM_STAFF_ADDRESS>();
            ADM_STAFF_ADDRESS model = null;
            try
            {
                foreach (AddressViewModel viewModel in addressList)
                {
                    model = new ADM_STAFF_ADDRESS();
                    model.ASFA_Address1 = viewModel.AD_Address1;
                    model.ASFA_Address2 = viewModel.AD_Address2;
                    model.ASFA_AddressType = viewModel.AD_AddressType;
                    model.ASFA_City= viewModel.AD_City;
                    model.ASFA_College_Id = CollegeId;
                    model.ASFA_Country = viewModel.AD_Country;
                    model.ASFA_Id = viewModel.AD_Id;
                    model.ASFA_IsPrimary = viewModel.AD_IsPrimary;
                    model.ASFA_Pincode = viewModel.AD_Pincode;
                    model.ASFA_State = viewModel.AD_State;
                    model.ASFA_Street = viewModel.AD_Street;
                    addList.Add(model);
                }
                return addList;
            }
            finally { model = null; addList = null; }
        }
        private string ValidateStaffAuthentication(AuthenticationViewModel auViewModel)
        {
            try
            {
                if (auViewModel == null) { return " Please Fill Authentication Tab."; }
                if (auViewModel.AUT_Login_Id == null || auViewModel.AUT_Login_Id.Trim() == "") { return " Please enter user name."; }
                if (auViewModel.AUT_Password == null || auViewModel.AUT_Password.Trim() == "") { return " Please enter password."; }
                if (auViewModel.AUT_Confirm_Password == null || auViewModel.AUT_Confirm_Password.Trim() == "") { return " Please enter confirm password."; }
                if (auViewModel.AUT_Password != auViewModel.AUT_Confirm_Password) { return " Password and Confirm password must be same."; }
                return "";
            }
            finally { auViewModel = null; }
        }

        [HttpPost]
        public ActionResult DeleteStaff(long staffId)
        {
            bool flag = false;
            try
            {
                if (staffId <= 0) { return Json(false); }
                flag = staffService.DeleteStaff(CollegeId, staffId, UserId);
                return Json(new { GridData = (flag ? staffService.GetGridData(CollegeId, Convert.ToString(DataKeeper.LOVTypeKeeper.Department)) : false), Msg = (flag ? "Deleted Successfully" : "Invalid Access.") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
        }
    }
}
