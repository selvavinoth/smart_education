using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEdu.Bll.Services;
using SmartEdu.ViewModel;
using SmartEdu.Data.Models;
using SmartEdu.SessionHolder;
using SmartEdu.Helper;

namespace SmartEdu.Controllers
{
    public class OrganizationController : Controller
    {
        //
        // GET: /Organization/
        private int CollegeID { 
            get{ return SessionPersistor.CollegeId;}
            set { SessionPersistor.CollegeId = value; }
        }
        private int DepartmetID
        {
            get { return SessionPersistor.DPID; }
            set { SessionPersistor.DPID = value; }
        }
        private int SelectedCollegeID
        {
            get { return SessionPersistor.SelectedCollegeID; }
            set { SessionPersistor.SelectedCollegeID = value; }
        }
        private int SelectedDepartmetID
        {
            get { return SessionPersistor.SelectedDPID; }
            set { SessionPersistor.SelectedDPID = value; }
        }
        private readonly IOrganizationService organizationService;
        private readonly ISharedService sharedService;
        public OrganizationController(IOrganizationService organizationService, ISharedService sharedService)
        {
            this.organizationService = organizationService;
            this.sharedService = sharedService;
        }

        private IEnumerable<SelectListItem> GetQuestionList()
        {
            List<ADM_LOV> LovList = null;
            try
            {
                LovList = sharedService.GetLovList(0, new List<string>() {  Convert.ToString(DataKeeper.LOVTypeKeeper.AuthenticationQuestion) });
                if (LovList == null) { return new List<SelectListItem>(); }
                return (from f in LovList
                        select new SelectListItem { Text = f.AL_Description, Value = f.AL_Id.ToString() }).ToList();
            }
            finally { LovList = null; }
        }
        private IEnumerable<SelectListItem> GetRoleList()
        {
            List<SADM_ROLE> roleList = null;
            try
            {
                roleList = organizationService.GetRoleList();
                if (roleList == null) { return new List<SelectListItem>(); }
                return (from f in roleList
                        select new SelectListItem { Text = f.SR_Description, Value = f.SR_Id.ToString() }).ToList();
            }
            finally { roleList = null; }
        }

        private OrganizationViewModel viewModel = null;
        public ActionResult Index()
        {
            try
            {
                viewModel = new OrganizationViewModel();
                viewModel.OCPViewModel = new OrganizationContactPersonViewModel();
                SetUrl(true);
                return View(viewModel);
            }
            catch (Exception exe) { return View(viewModel); }
            finally { viewModel = null; }
        }
        public ActionResult Department()
        {
            try
            {
                viewModel = new OrganizationViewModel();
                viewModel.OCPViewModel = new OrganizationContactPersonViewModel();
                SetUrl(false);
                return View("Index",viewModel);
            }
            catch (Exception exe) { return View("Index",viewModel); }
            finally { viewModel = null; }
        }
        private void SetUrl(bool isCollege) {
            try {
                viewModel.IUVModel = new ImageUploderViewModel();
                if (isCollege) {
                    viewModel.Add_EdtitUrl = "~/Organization/CollegeIUAction";
                    viewModel.OCPViewModel.AddressUrl = "~/Organization/CollegeCPIUAction";
                    viewModel.DeleteUrl = "~/Organization/DeleteCollege";
                    viewModel.GridLoadUrl = "~/Organization/LoadCollegeGrid";
                    viewModel.RedirectUrl = "~/Organization/Details?id=";
                    viewModel.CancelUrl = "~/Organization/Index";
                    viewModel.OCPViewModel.CPGridLoadUrl = "~/Organization/LoadCollegeCPGridData";
                    ViewBag.Title = ResourceKeeper.GetResource("APP_ORG_College_Details");
                    viewModel.IUVModel.FormId = "orgForm";
                    viewModel.IUVModel.FuctionName = "UploadCollegeLogo_Image()";
                    viewModel.IUVModel.TargetName = "UploadCollegeLogo";
                    viewModel.IUVModel.CallBackFunction = "UploadCollegeLogo_Complete()";
                    viewModel.IUVModel.ImgDiv = "UpCollegeImgDiv";
                }
                else {
                    ViewBag.NEED_COLLEGE_SELECTION = "TRUE"; 
                    viewModel.Add_EdtitUrl = "~/Organization/DepartmentIUAction";
                    viewModel.OCPViewModel.AddressUrl = "~/Organization/DepartmentCPIUAction";
                    viewModel.DeleteUrl = "~/Organization/DeleteDepartment";
                    viewModel.GridLoadUrl = "~/Organization/LoadDepartmentGrid";
                    viewModel.RedirectUrl = "~/Organization/DepartmentDetails?id=";
                    viewModel.CancelUrl = "~/Organization/Department";
                    viewModel.OCPViewModel.CPGridLoadUrl = "~/Organization/LoadDepartmentCPGridData";
                    ViewBag.Title = ResourceKeeper.GetResource("APP_ORG_Department_Details");
                    viewModel.IUVModel.FormId = "dpLogo";
                    viewModel.IUVModel.FuctionName = "UploadDpLogo_Image()";
                    viewModel.IUVModel.TargetName = "UploadDpLogo";
                    viewModel.IUVModel.CallBackFunction = "UploadDpLogo_Complete()";
                    viewModel.IUVModel.ImgDiv = "UpDpImgDiv";
                }
            }
            finally { }
        }

        public ActionResult Details(int ? id)
        {
            try
            {
                viewModel = new OrganizationViewModel();
                viewModel.UserAuthentication = new AuthenticationViewModel();
                viewModel.UserAuthentication.NeedToShowRole = true;
                viewModel.UserAuthentication.RoleList = GetRoleList();
                viewModel.UserAuthentication.QuestionList = GetQuestionList();
                if (id.HasValue) { FillOrganizationViewModel(id ?? 0, true); }
                viewModel.OCPViewModel = new OrganizationContactPersonViewModel();
                SetUrl(true);
                return View(viewModel);
            }
            catch (Exception exe) { return View(viewModel); }
            finally { viewModel = null; }
        }
        public ActionResult DepartmentDetails(int ? id)
        {
            try
            {
                viewModel = new OrganizationViewModel();
                viewModel.UserAuthentication = new AuthenticationViewModel();
                viewModel.UserAuthentication.NeedToShowRole = true;
                viewModel.UserAuthentication.RoleList = GetRoleList();
                viewModel.UserAuthentication.QuestionList = GetQuestionList();
                if (id.HasValue) { FillOrganizationViewModel(id ?? 0, true); }
                viewModel.OCPViewModel = new OrganizationContactPersonViewModel();
                SetUrl(false);
                return View("Details",viewModel);
            }
            catch (Exception exe) { return View(viewModel); }
            finally { viewModel = null; }
        }

        public ActionResult LoadCollegeGrid() {
            try {
                return Json(new { GridData=organizationService.GetCollegeGridData()},JsonRequestBehavior.AllowGet);
            }
            finally { }
        }
        public ActionResult LoadCollegeCPGridData()
        {
            try
            {
                return Json(new { GridData = GetCollegeCPGridData() }, JsonRequestBehavior.AllowGet);
            }
            finally { }
        }
        private object GetCollegeCPGridData()
        {
            List<ADM_COLLEGE_CONTACT_PERSON> cpList;
            try
            {
                cpList = organizationService.GetCollegeContactPersonList();
                if (cpList.Count == 0) { return new { total_count = 0, rows = new { }}; }
                return new
                {
                    total_count = cpList.Count,
                    rows = (from c in cpList
                            select new
                            {
                                id = c.ACCP_Id,
                                data = new string[] {c.ACCP_Id.ToString(), c.ACCP_ContactPerson, c.ACCP_Qualification, c.ACCP_PhoneNumber, c.ACCP_EmailId, c.ACCP_FaxNo, c.ACCP_Country, (c.ACCP_State??""), (c.ACCP_City??""), (c.ACCP_Pincode??""),"","",(c.ACCP_ProfileImage??"") }
                            }).ToList()
                };
            }
            finally { cpList = null; }
        }

        public ActionResult LoadDepartmentGrid()
        {
            try
            {
                if (SelectedCollegeID == 0) { return Json(new { GridData = new { total_count = 0, rows = new { }} }, JsonRequestBehavior.AllowGet); }
                return Json(new { GridData = organizationService.GetDepartmentGridData(SelectedCollegeID) }, JsonRequestBehavior.AllowGet);
            }
            finally { }
        }
        public ActionResult LoadDepartmentCPGridData()
        {
            try
            {
                return Json(new { GridData = GetDepartmentCPGridData(SelectedCollegeID, SelectedDepartmetID) }, JsonRequestBehavior.AllowGet);
            }
            finally { }
        }
        private object GetDepartmentCPGridData(int collegeId, int dpId)
        {
            List<ADM_DEPARTMENT_CONTACT_PERSON> cpList;
            try
            {
                cpList = organizationService.GetDepartmentContactPersonList(collegeId);
                if (cpList.Count == 0) { return new { total_count = 0, rows = new { }}; }
                return new
                {
                    total_count = 0,
                    rows = (from c in cpList
                            select new
                            {
                                id = c.ADCP_Id,
                                data = new string[] {c.ADCP_Id.ToString(), c.ADCP_ContactPerson, c.ADCP_Qualification, c.ADCP_PhoneNumber, c.ADCP_EmailId, c.ADCP_FaxNo, c.ADCP_Country, c.ADCP_State, c.ADCP_City, c.ADCP_Pincode ,"","",c.ADCP_ProfileImage}
                            }).ToList()
                };
            }
            finally { cpList = null; }
        }

        private void FillOrganizationViewModel(int id,bool isCollege) {
            try {
                if (isCollege) { FillCollegeViewModel(id); }
                else { FillDepViewModel(id); }
            }
            finally { }
        }
        private void FillCollegeViewModel(int orgId)
        {
            ADM_COLLEGE model = new ADM_COLLEGE();
            try
            {
                model = organizationService.GetCollege(orgId);
                if (model == null) { return; }
                SelectedCollegeID = orgId;
                viewModel.ORG_Code=model.ACE_Code;
                viewModel.ORG_DepartmentLogo=model.ACE_CollegeLogo;
                viewModel.ORG_Description=model.ACE_Description;
                viewModel.ORG_EmailId=model.ACE_EmailId;
                viewModel.ORG_FaxNo=model.ACE_FaxNo;
                viewModel.ORG_History=model.ACE_History;
                viewModel.ORG_MobileNo=model.ACE_MobileNo;
                viewModel.ORG_PhoneNo=model.ACE_PhoneNo;
                viewModel.ORG_ShortDescription=model.ACE_ShortDescription;
                viewModel.ORG_Website=model.ACE_Website;
                FillUserViewModel(orgId, 0);
            }
            finally { model = null; }
        }
        private void FillDepViewModel(int depId)
        {
            ADM_DEPARTMENTS model = new ADM_DEPARTMENTS();
            try
            {
                model = organizationService.GetDepartment(depId);
                if (model == null) { return; }
                SelectedCollegeID = model.ADP_CollegeId;
                SelectedDepartmetID = depId;
                viewModel.ORG_Code=model.ADP_Code;
                viewModel.ORG_DepartmentLogo=model.ADP_DepartmentLogo;
                viewModel.ORG_Description=model.ADP_Description;
                viewModel.ORG_EmailId=model.ADP_EmailId;
                viewModel.ORG_FaxNo=model.ADP_FaxNo;
                viewModel.ORG_History=model.ADP_History;
                viewModel.ORG_MobileNo=model.ADP_MobileNo;
                viewModel.ORG_PhoneNo=model.ADP_PhoneNo;
                viewModel.ORG_ShortDescription=model.ADP_ShortDescription;
                viewModel.ORG_Website=model.ADP_Website;
                FillUserViewModel(model.ADP_CollegeId, depId);
            }
            finally { model = null; }
        }
        private void FillUserViewModel(int orgId,int userId) {
            ADM_USER_DETAILS model = new ADM_USER_DETAILS();
            try {
                model = organizationService.GetUserDetails(orgId, userId, DateTime.Now.Date);
                if (model == null) { return; }
                viewModel.UserAuthentication.AUT_Answer = model.AUD_Answer;
                viewModel.UserAuthentication.AUT_Login_Id = model.AUD_Login_Id;
                viewModel.UserAuthentication.AUT_Password = model.AUD_Password;
                viewModel.UserAuthentication.AUT_Question = model.AUD_Question;
                viewModel.UserAuthentication.AUT_RoleId = model.AUD_RoleId;
            }
            finally { }
        }

        public ActionResult CollegeIUAction(OrganizationViewModel viewModel)
        {
            bool flag = false;
            try
            {
                if (viewModel == null) { return Json(new { Status = false, Msg = ResourceKeeper.GetResource("CMN_NoDetailsFound") }, JsonRequestBehavior.AllowGet); }
                if (viewModel.ORG_Id == 0)
                {
                    organizationService.InsertCollege(FillCollegeModel(viewModel), FillCollegeContactPerson(viewModel.OCPViewModelList), FillUserDetails(viewModel), 0);
                    return Json(new { Status = true, Msg = ResourceKeeper.GetResource("CMN_Added_Successfully") }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    flag = organizationService.UpdateCollege(FillCollegeModel(viewModel), FillCollegeContactPerson(viewModel.OCPViewModelList), 0, true);
                    return Json(new { Status = flag, Msg = (flag ? ResourceKeeper.GetResource("CMN_Updated_Successfully") : ResourceKeeper.GetResource("CMN_Invalid_Access")) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exe) { return Json(false); }
        }
        public ActionResult DepartmentIUAction(OrganizationViewModel viewModel)
        {
            bool flag = false;
            try {
                if (viewModel == null) { return Json(new { Status = false, Msg = ResourceKeeper.GetResource("CMN_NoDetailsFound") }, JsonRequestBehavior.AllowGet); }
                if (viewModel.ORG_Id == 0) {
                    organizationService.InsertDepartment(FillDepModel(viewModel), FillDepContactPerson(viewModel.OCPViewModelList), FillUserDetails(viewModel), 0);
                    return Json(new { Status = true, Msg = ResourceKeeper.GetResource("CMN_Added_Successfully") }, JsonRequestBehavior.AllowGet);
                }
                else {
                    flag = organizationService.UpdateDepartment(FillDepModel(viewModel), FillDepContactPerson(viewModel.OCPViewModelList), 0,true);
                    return Json(new { Status = flag, Msg = (flag ? ResourceKeeper.GetResource("CMN_Updated_Successfully") : ResourceKeeper.GetResource("CMN_Invalid_Access")) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exe) { return Json(false); }
        }

        private ADM_COLLEGE FillCollegeModel(OrganizationViewModel viewModel)
        {
            ADM_COLLEGE model = new ADM_COLLEGE();
            try {
                model.ACE_Code = viewModel.ORG_Code;
                model.ACE_CollegeLogo = viewModel.ORG_DepartmentLogo;
                model.ACE_Description = viewModel.ORG_Description;
                model.ACE_EmailId = viewModel.ORG_EmailId;
                model.ACE_CreatedDate = DateTime.Now.Date;
                model.ACE_FaxNo = viewModel.ORG_FaxNo;
                model.ACE_History = viewModel.ORG_History;
                model.ACE_MobileNo = viewModel.ORG_MobileNo;
                model.ACE_PhoneNo = viewModel.ORG_PhoneNo;
                model.ACE_ShortDescription = viewModel.ORG_ShortDescription;
                model.ACE_StartDate = DateTime.Now.Date;
                model.ACE_Website = viewModel.ORG_Website;
                return model;
            }
            finally { model = null; }
        }
        private ADM_DEPARTMENTS FillDepModel(OrganizationViewModel viewModel)
        {
            ADM_DEPARTMENTS model = new ADM_DEPARTMENTS();
            try
            {
                model.ADP_CollegeId = SelectedCollegeID;
                model.ADP_Code = viewModel.ORG_Code;
                model.ADP_DepartmentLogo = viewModel.ORG_DepartmentLogo;
                model.ADP_Description = viewModel.ORG_Description;
                model.ADP_EmailId = viewModel.ORG_EmailId;
                model.ADP_Created_By = 0;
                model.ADP_CreatedDate = DateTime.Now.Date;
                model.ADP_FaxNo = viewModel.ORG_FaxNo;
                model.ADP_History = viewModel.ORG_History;
                model.ADP_MobileNo = viewModel.ORG_MobileNo;
                model.ADP_PhoneNo = viewModel.ORG_PhoneNo;
                model.ADP_ShortDescription = viewModel.ORG_ShortDescription;
                model.ADP_StartDate = DateTime.Now.Date;
                model.ADP_Website = viewModel.ORG_Website;
                return model;
            }
            finally { model = null; }
        }

        private ADM_USER_DETAILS FillUserDetails(OrganizationViewModel viewModel)
        {
            ADM_USER_DETAILS model = new ADM_USER_DETAILS();
            AuthenticationViewModel vModel = new AuthenticationViewModel();
            try {
                vModel = viewModel.UserAuthentication;
                if (vModel == null) { return new ADM_USER_DETAILS(); }
                model.AUD_Answer = vModel.AUT_Answer;
                model.AUD_Login_Id = vModel.AUT_Login_Id;
                model.AUD_Password = vModel.AUT_Password;
                model.AUD_Question = vModel.AUT_Question;
                model.AUD_RoleId=vModel.AUT_RoleId;
                model.AUD_User_Code=viewModel.ORG_Code;
                model.AUD_RoleId = vModel.AUT_RoleId;
                return model;
            }
            finally { model = null; vModel = null; }
        }

        private List<ADM_COLLEGE_CONTACT_PERSON> FillCollegeContactPerson(List<OrganizationContactPersonViewModel> viewModel) {
            ADM_COLLEGE_CONTACT_PERSON model = null;
            List<ADM_COLLEGE_CONTACT_PERSON> modelList = new List<ADM_COLLEGE_CONTACT_PERSON>();
            try { 
                foreach(OrganizationContactPersonViewModel m in viewModel){
                    model = new ADM_COLLEGE_CONTACT_PERSON();
                    model.ACCP_City = m.ORGCP_City;
                    model.ACCP_ContactPerson = m.ORGCP_ContactPerson;
                    model.ACCP_Country = m.ORGCP_Country;
                    model.ACCP_Created_By = 0;
                    model.ACCP_Created_Date = DateTime.Now.Date;
                    model.ACCP_EmailId = m.ORGCP_EmailId;
                    model.ACCP_FaxNo = m.ORGCP_FaxNo;
                    model.ACCP_PhoneNumber = m.ORGCP_PhoneNumber;
                    model.ACCP_Pincode = m.ORGCP_Pincode;
                    model.ACCP_ProfileImage = m.ORGCP_ProfileImage;
                    model.ACCP_Qualification = m.ORGCP_Qualification;
                    model.ACCP_StartDate = DateTime.Now.Date;
                    modelList.Add(model);
                }
                return modelList;
            }
            finally { modelList = null; model = null; }
        }
        private List<ADM_DEPARTMENT_CONTACT_PERSON> FillDepContactPerson(List<OrganizationContactPersonViewModel> viewModel)
        {
            ADM_DEPARTMENT_CONTACT_PERSON model = null;
            List<ADM_DEPARTMENT_CONTACT_PERSON> modelList = new List<ADM_DEPARTMENT_CONTACT_PERSON>();
            try
            {
                foreach (OrganizationContactPersonViewModel m in viewModel)
                {
                    model = new ADM_DEPARTMENT_CONTACT_PERSON();
                    model.ADCP_College_Id = SelectedCollegeID;
                    model.ADCP_Department_Id = SelectedDepartmetID;
                    model.ADCP_City = m.ORGCP_City;
                    model.ADCP_ContactPerson = m.ORGCP_ContactPerson;
                    model.ADCP_Country = m.ORGCP_Country;
                    model.ADCP_Created_By = 0;
                    model.ADCP_Created_Date = DateTime.Now.Date;
                    model.ADCP_EmailId = m.ORGCP_EmailId;
                    model.ADCP_FaxNo = m.ORGCP_FaxNo;
                    model.ADCP_PhoneNumber = m.ORGCP_PhoneNumber;
                    model.ADCP_Pincode = m.ORGCP_Pincode;
                    model.ADCP_ProfileImage = m.ORGCP_ProfileImage;
                    model.ADCP_Qualification = m.ORGCP_Qualification;
                    model.ADCP_StartDate = DateTime.Now.Date;
                    modelList.Add(model);
                }
                return modelList;
            }
            finally { modelList = null; model = null; }
        }

        [HttpPost]
        public ActionResult DeleteCollge(int id) {
            try
            {
                if (id > 0) { if (organizationService.DeleteCollege(id, 0)) { return Json(new { Msg = ResourceKeeper.GetResource("CMN_Deleted_Successfully"), data = organizationService.GetCollegeGridData() }, JsonRequestBehavior.AllowGet); } }
                return Json(new { Msg = ResourceKeeper.GetResource("CMN_Invalid_Access"), data = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
        }
        [HttpPost]
        public ActionResult DeleteDepartment(int id)
        {
            try
            {
                if (id > 0) { if (organizationService.DeleteDepartment(SelectedCollegeID, id, 0)) { return Json(new { Msg = ResourceKeeper.GetResource("CMN_Deleted_Successfully"), data = organizationService.GetDepartmentGridData(CollegeID) }, JsonRequestBehavior.AllowGet); } }
                return Json(new { Msg = ResourceKeeper.GetResource("CMN_Invalid_Access"), data = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
        }
    }
}
