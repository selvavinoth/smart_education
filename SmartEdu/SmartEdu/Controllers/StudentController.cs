using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEdu.SessionHolder;
using SmartEdu.Bll.Services;
using SmartEdu.Helper;
using SmartEdu.ViewModel;
using SmartEdu.Data.Models;

namespace SmartEdu.Controllers
{
    public class StudentController : Controller
    {
        //
        // GET: /Student/
        private int CollegeId { get { return SessionPersistor.CollegeId; } }
        private long UserId { get { return SessionPersistor.CollegeId; } }
        private long SelectedStudentId
        {
            get { return Convert.ToInt64(SessionPersistor.getTempValue(DataKeeper.SelectedStudentId)); }
            set { SessionPersistor.setTempValue(DataKeeper.SelectedStudentId, value); }
        }
        private string RenderingField = "AU_Title,AU_FirstName,AU_MiddleName,AU_LastName,AU_Gender,AU_Department,AU_DOB,AU_StartDate,AU_Email_Id,AU_PhoneNo,AU_MobileNo,AU_FaxNo,AU_Id";
        private string ParentRenderingField = "AU_Title,AU_FirstName,AU_MiddleName,AU_LastName,AU_Gender,AU_Occupation,AU_DOB,AU_Anniversary_Date,AU_StartDate,AU_Email_Id,AU_PhoneNo,AU_MobileNo,AU_FaxNo,AU_Id";
        private readonly IStudentService studentService;
        private readonly ISharedService sharedService;
        public StudentController(IStudentService studentService, ISharedService sharedService) { this.studentService = studentService; this.sharedService = sharedService; }
        private StudentViewModel studentViewModel = null;

        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception exe) { return View(); }
            finally { }
        }
        public ActionResult Details(long? id)
        {
            studentViewModel = new StudentViewModel();
            try
            {
                studentViewModel.UserViewModel = new UsersViewModel();
                studentViewModel.ParentViewModel = new ParentViewModel();
                studentViewModel.UserViewModel.UserDetailList = GetUserDetails();
                studentViewModel.ParentViewModel.ParentDetailList = GetParentDetails();
                studentViewModel.AddressViewModel = new AddressViewModel();
                studentViewModel.AddressViewModel.AD_GridLoadUrl = Url.Content("~/Student/GetStudentAddressData");
                studentViewModel.UserAuthentication = new AuthenticationViewModel();
                FillDetailsViewModel();
                studentViewModel.ParentViewModel.GenderList = studentViewModel.UserViewModel.GenderList;
                if ((id ?? 0) > 0)
                {
                    SelectedStudentId = id.Value;
                    FillAuthenticationDetails(id ?? 0, studentViewModel.UserAuthentication);
                    FillStudentDetails(id ?? 0, studentViewModel.UserViewModel);
                    FillParentDetails(id ?? 0, studentViewModel.ParentViewModel);
                }
                return View(studentViewModel);
            }
            catch (Exception exe) { return View(studentViewModel); }
            finally { studentViewModel = null; }
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
                data.Add(DataKeeper.UserDetails.AU_Anniversary_Date);
                data.Add(DataKeeper.UserDetails.AU_StartDate);
                data.Add(DataKeeper.UserDetails.AU_Email_Id);
                data.Add(DataKeeper.UserDetails.AU_PhoneNo);
                data.Add(DataKeeper.UserDetails.AU_MobileNo);
                data.Add(DataKeeper.UserDetails.AU_FaxNo);
                data.Add(DataKeeper.UserDetails.AU_Id);
                return data;
            }
            finally { data = null; }
        }
        private List<DataKeeper.UserDetails> GetParentDetails()
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
                data.Add(DataKeeper.UserDetails.AU_DOB);
                data.Add(DataKeeper.UserDetails.AU_Anniversary_Date);
                data.Add(DataKeeper.UserDetails.AU_StartDate);
                data.Add(DataKeeper.UserDetails.AU_Email_Id);
                data.Add(DataKeeper.UserDetails.AU_PhoneNo);
                data.Add(DataKeeper.UserDetails.AU_MobileNo);
                data.Add(DataKeeper.UserDetails.AU_FaxNo);
                data.Add(DataKeeper.UserDetails.AU_Id);
                return data;
            }
            finally { data = null; }
        }
        private void FillDetailsViewModel()
        {
            List<ADM_LOV> LovList = null;
            string type = "";
            try
            {
                LovList = sharedService.GetLovList(CollegeId, new List<string>() { Convert.ToString(DataKeeper.LOVTypeKeeper.AddressType), Convert.ToString(DataKeeper.LOVTypeKeeper.Gender), Convert.ToString(DataKeeper.LOVTypeKeeper.Qualification), Convert.ToString(DataKeeper.LOVTypeKeeper.AuthenticationQuestion), Convert.ToString(DataKeeper.LOVTypeKeeper.Department) });
                type = Convert.ToString(DataKeeper.LOVTypeKeeper.AddressType);
                studentViewModel.AddressViewModel.AddressTypeList = GetLovList(LovList.FindAll(exp => exp.AL_Type == type));
                type = Convert.ToString(DataKeeper.LOVTypeKeeper.AuthenticationQuestion);
                studentViewModel.UserAuthentication.QuestionList = GetLovList(LovList.FindAll(exp => exp.AL_Type == type));
                type = Convert.ToString(DataKeeper.LOVTypeKeeper.Department);
                studentViewModel.UserViewModel.DepartmentList = GetLovList(LovList.FindAll(exp => exp.AL_Type == type));
                type = Convert.ToString(DataKeeper.LOVTypeKeeper.Gender);
                studentViewModel.UserViewModel.GenderList = GetLovList(LovList.FindAll(exp => exp.AL_Type == type));
            }
            finally { LovList = null; }
        }
        private void FillAuthenticationDetails(long studentId, AuthenticationViewModel vModel)
        {
            ADM_USER_DETAILS userDetail = null;
            try
            {
                userDetail = studentService.GetUserDetails(studentId, CollegeId);
                if (userDetail == null) { return; }
                vModel.AUT_Login_Id = userDetail.AUD_Login_Id;
                vModel.AUT_Password = userDetail.AUD_Password;
                vModel.AUT_Question = userDetail.AUD_Question;
                vModel.AUT_Answer = userDetail.AUD_Answer;
            }
            finally { userDetail = null; }
        }
        private void FillStudentDetails(long studentId, UsersViewModel viewModel)
        {
            ADM_STUDENT_DETAILS model = null;
            List<DataKeeper.UserDetails> userDetailList = null;
            try
            {
                userDetailList = GetUserDetails();
                model = studentService.GetStudentDetails(studentId, CollegeId);
                if (model == null) { return; }
                foreach (DataKeeper.UserDetails field in userDetailList)
                {
                    switch (field)
                    {
                        case DataKeeper.UserDetails.AU_Title:
                            viewModel.AU_Title = model.ASD_Title;
                            break;
                        case DataKeeper.UserDetails.AU_FirstName:
                            viewModel.AU_FirstName = model.ASD_FirstName;
                            break;
                        case DataKeeper.UserDetails.AU_MiddleName:
                            viewModel.AU_MiddleName = model.ASD_MiddleName;
                            break;
                        case DataKeeper.UserDetails.AU_LastName:
                            viewModel.AU_LastName = model.ASD_LastName;
                            break;
                        case DataKeeper.UserDetails.AU_Gender:
                            viewModel.AU_Gender = model.ASD_Gender;
                            break;
                        case DataKeeper.UserDetails.AU_Department:
                            viewModel.AU_Department = model.ASD_Department;
                            break;
                        case DataKeeper.UserDetails.AU_DOB:
                            viewModel.AU_DOB = model.ASD_DOB;
                            break;
                        case DataKeeper.UserDetails.AU_DOJ:
                            viewModel.AU_DOJ = model.ASD_DOJ;
                            break;
                        case DataKeeper.UserDetails.AU_StartDate:
                            viewModel.AU_StartDate = model.ASD_StartDate;
                            break;
                        case DataKeeper.UserDetails.AU_PhoneNo:
                            viewModel.AU_PhoneNo = model.ASD_PhoneNo;
                            break;
                        case DataKeeper.UserDetails.AU_Email_Id:
                            viewModel.AU_Email_Id = model.ASD_Email_Id;
                            break;
                        case DataKeeper.UserDetails.AU_MobileNo:
                            viewModel.AU_MobileNo = model.ASD_MobileNo;
                            break;
                        case DataKeeper.UserDetails.AU_FatherName:
                            viewModel.AU_FirstName = model.ASD_FirstName;
                            break;
                        case DataKeeper.UserDetails.AU_FaxNo:
                            viewModel.AU_FaxNo = model.ASD_FaxNo;
                            break;
                        case DataKeeper.UserDetails.AU_Id:
                            viewModel.AU_Id = model.ASD_Id;
                            break;
                        default:
                            break;
                    }
                }
            }
            finally { model = null; userDetailList = null; }
        }
        private void FillParentDetails(long studentId, ParentViewModel viewModel)
        {
            ADM_PARENT_DETAILS model = null;
            List<DataKeeper.UserDetails> userDetailList = null;
            try
            {
                userDetailList = GetParentDetails();
                model = studentService.GetParentDetails(studentId, CollegeId);
                if (model == null) { return; }
                foreach (DataKeeper.UserDetails field in userDetailList)
                {
                    switch (field)
                    {
                        case DataKeeper.UserDetails.AU_Title:
                            viewModel.APD_Title = model.APD_Title;
                            break;
                        case DataKeeper.UserDetails.AU_FirstName:
                            viewModel.APD_FirstName = model.APD_FirstName;
                            break;
                        case DataKeeper.UserDetails.AU_MiddleName:
                            viewModel.APD_MiddleName = model.APD_MiddleName;
                            break;
                        case DataKeeper.UserDetails.AU_LastName:
                            viewModel.APD_LastName = model.APD_LastName;
                            break;
                        case DataKeeper.UserDetails.AU_Gender:
                            viewModel.APD_Gender = model.APD_Gender;
                            break;
                        case DataKeeper.UserDetails.AU_Anniversary_Date:
                            viewModel.APD_Anniversary_Date = model.APD_Anniversary_Date;
                            break;
                        case DataKeeper.UserDetails.AU_DOB:
                            viewModel.APD_DOB = model.APD_DOB;
                            break;
                        case DataKeeper.UserDetails.AU_Occupation:
                            viewModel.APD_Occupation = model.APD_Occupation;
                            break;
                        case DataKeeper.UserDetails.AU_StartDate:
                            viewModel.APD_StartDate = model.APD_StartDate;
                            break;
                        case DataKeeper.UserDetails.AU_PhoneNo:
                            viewModel.APD_PhoneNo = model.APD_PhoneNo;
                            break;
                        case DataKeeper.UserDetails.AU_Email_Id:
                            viewModel.APD_Email_Id = model.APD_Email_Id;
                            break;
                        case DataKeeper.UserDetails.AU_MobileNo:
                            viewModel.APD_MobileNo = model.APD_MobileNo;
                            break;
                        case DataKeeper.UserDetails.AU_FaxNo:
                            viewModel.APD_FaxNo = model.APD_FaxNo;
                            break;
                        case DataKeeper.UserDetails.AU_Id:
                            viewModel.APD_Id = model.APD_Id;
                            break;
                        default:
                            break;
                    }
                }
            }
            finally { model = null; userDetailList = null; }
        }
        private bool NeedToShowField(string fieldName)
        {
            try
            {
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
                        select new SelectListItem { Text = f.AL_Description, Value = f.AL_Id.ToString() }).ToList();
            }
            finally { }
        }

        #region Grid Loading
        public ActionResult GetStudentData()
        {
            try
            {
                return Json(new { GridData = studentService.GetGridData(CollegeId, Convert.ToString(DataKeeper.LOVTypeKeeper.Department)) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
        }
        public ActionResult GetStudentAddressData()
        {
            try
            {
                if (SelectedStudentId <= 0) { return Json(new { total_count = 0, rows = new { } }, JsonRequestBehavior.AllowGet); }
                return Json(studentService.GetAddressGridData(CollegeId, SelectedStudentId, Convert.ToString(DataKeeper.LOVTypeKeeper.AddressType)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false,JsonRequestBehavior.AllowGet); }
        }
        #endregion

        #region StudentIU Action
        [HttpPost]
        public ActionResult StudentIUAction(StudentViewModel viewModel)
        {
            string msg = "";
            bool flag = false;
            ADM_STUDENT_DETAILS model = new ADM_STUDENT_DETAILS();
            try
            {
                if (viewModel == null) { return Json(new { Status = false, Msg = "No Data Found." }); }
                msg = ValidateStudentAuthentication(viewModel.UserAuthentication);
                if (msg != "") { return Json(new { Status = false, Msg = msg }); }
                FillStudentModel(viewModel.UserViewModel, model);
                if (model.ASD_Id == 0)
                {
                    studentService.InsertStudent(model, FillParentModel(viewModel.ParentViewModel), FillUserModel(viewModel.UserAuthentication), GetUserAddressList(viewModel.AddressList), UserId);
                    msg = "Added Successfully.";
                }
                else
                {
                    flag = studentService.UpdateStudent(model, FillParentModel(viewModel.ParentViewModel), FillUserModel(viewModel.UserAuthentication), GetUserAddressList(viewModel.AddressList), UserId);
                    msg = "Updated Successfully.";
                }
                return Json(new { Status = true, Msg = msg });
            }
            catch (Exception exe) { return Json(false); }
            finally { msg = string.Empty; model = null; }
        }

        private string ValidateStudentAuthentication(AuthenticationViewModel auViewModel)
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
        private void FillStudentModel(UsersViewModel viewModel, ADM_STUDENT_DETAILS model)
        {
            try
            {
                model.ASD_Id = SelectedStudentId;
                model.ASD_Title = viewModel.AU_Title;
                model.ASD_CollegeId = CollegeId;
                model.ASD_Department = viewModel.AU_Department;
                model.ASD_DOB = viewModel.AU_DOB;
                model.ASD_DOJ = viewModel.AU_DOJ;
                model.ASD_DOJ = DateTime.Now.Date;
                model.ASD_Email_Id = viewModel.AU_Email_Id;
                model.ASD_EndDate = viewModel.AU_EndDate;
                model.ASD_FaxNo = viewModel.AU_FaxNo;
                model.ASD_FirstName = viewModel.AU_FirstName;
                model.ASD_Gender = viewModel.AU_Gender;
                model.ASD_LastName = viewModel.AU_LastName;
                model.ASD_MiddleName = viewModel.AU_MiddleName;
                model.ASD_MobileNo = viewModel.AU_MobileNo;
                model.ASD_PhoneNo = viewModel.AU_PhoneNo;
                model.ASD_StartDate = viewModel.AU_StartDate;
            }

            finally { }
        }
        private ADM_PARENT_DETAILS FillParentModel(ParentViewModel viewModel)
        {
            ADM_PARENT_DETAILS model = new ADM_PARENT_DETAILS();
            try
            {
                model.APD_Id = viewModel.APD_Id;
                model.APD_Title = viewModel.APD_Title;
                model.APD_CollegeId = CollegeId;
                model.APD_DOB = DateTime.Now.Date;
                model.APD_Email_Id = viewModel.APD_Email_Id;
                model.APD_FaxNo = viewModel.APD_FaxNo;
                model.APD_FirstName = viewModel.APD_FirstName;
                model.APD_Gender = viewModel.APD_Gender;
                model.APD_LastName = viewModel.APD_LastName;
                model.APD_MiddleName = viewModel.APD_MiddleName;
                model.APD_MobileNo = viewModel.APD_MobileNo;
                model.APD_PhoneNo = viewModel.APD_PhoneNo;
                model.APD_StartDate = DateTime.Now.Date;
                return model;
            }

            finally { model = null; }
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
        private List<ADM_STUDENT_ADDRESS> GetUserAddressList(List<AddressViewModel> addressList)
        {
            List<ADM_STUDENT_ADDRESS> addList = new List<ADM_STUDENT_ADDRESS>();
            ADM_STUDENT_ADDRESS model = null;
            try
            {
                foreach (AddressViewModel viewModel in addressList)
                {
                    model = new ADM_STUDENT_ADDRESS();
                    model.ASA_Address1 = viewModel.AD_Address1;
                    model.ASA_Address2 = viewModel.AD_Address2;
                    model.ASA_AddressType = viewModel.AD_AddressType;
                    model.ASA_City = viewModel.AD_City;
                    model.ASA_College_Id = CollegeId;
                    model.ASA_Country = viewModel.AD_Country;
                    model.ASA_Id = viewModel.AD_Id;
                    model.ASA_IsPrimary = viewModel.AD_IsPrimary;
                    model.ASA_Pincode = viewModel.AD_Pincode;
                    model.ASA_State = viewModel.AD_State;
                    model.ASA_Street = viewModel.AD_Street;
                    addList.Add(model);
                }
                return addList;
            }
            finally { model = null; addList = null; }
        }
        #endregion

        #region Delete
        [HttpPost]
        public ActionResult DeleteStudent(long studentId)
        {
            bool flag = false;
            try
            {
                if (studentId <= 0) { return Json(false); }
                flag = studentService.DeleteStudent(CollegeId, studentId, UserId);
                return Json(new { GridData = (flag ? studentService.GetGridData(CollegeId, Convert.ToString(DataKeeper.LOVTypeKeeper.Department)) : false), Msg = (flag ? "Deleted Successfully" : "Invalid Access.") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
        }
        #endregion
    }
}
