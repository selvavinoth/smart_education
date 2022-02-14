using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEdu.Data.Models;
using SmartEdu.Helper;
using SmartEdu.CustomValidation;

namespace SmartEdu.ViewModel
{
    // Login ViewModel
    public class LoginViewModel {
        public long PositionId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public IEnumerable<SelectListItem> PositionList { get; set; }
    }

    #region Master ViewModels ( Menu Related )
    public class TaskViewModel
    {
        public int ST_Id { get; set; }
        [CustomValidationAttribute]
        public string ST_Task_Name { get; set; }
        [CustomValidationAttribute]
        public string ST_URL { get; set; }
        public bool ST_IsActive { get; set; }
        [CustomValidationAttribute]
        public string ReleaseMemory { get; set; }

        public TaskViewModel() { }
        public TaskViewModel(SADM_TASK model)
        {
            this.ST_Id = model.ST_Id;
            this.ST_Task_Name = model.ST_Task_Name;
            this.ST_URL = model.ST_URL;
            this.ST_IsActive = model.ST_IsActive;
        }
    }
    public class RoleViewModel
    {
        public int SR_Id { get; set; }
        [CustomValidationAttribute]
        public string SR_Code { get; set; }
        [CustomValidationAttribute]
        public string SR_Description { get; set; }
        public bool SR_IsActive { get; set; }
        [CustomValidationAttribute]
        public string ReleaseMemory { get; set; }

        public RoleViewModel() { }
        public RoleViewModel(SADM_ROLE model)
        {
            this.SR_Id = model.SR_Id;
            this.SR_Code = model.SR_Code;
            this.SR_Description = model.SR_Description;
            this.SR_IsActive = model.SR_IsActive;
        }
    }
    public class MenuViewModel
    {
        public int SM_Id { get; set; }
        public int SM_College_Id { get; set; }
        public int SM_DP_Id { get; set; }
        public int SM_Parent_Id { get; set; }
        [CustomValidationAttribute]
        public string SM_Menu_Name { get; set; }
        public int SM_Role_Id { get; set; }
        public int SM_Task_Id { get; set; }
        public bool SM_IsComponent { get; set; }
        public bool SM_IsActive { get; set; }
        [CustomValidationAttribute]
        public bool ReleaseMemory { get; set; }
        public string SM_Class { get; set; }

        public IEnumerable<SelectListItem> MenuList { get; set; }
        public IEnumerable<SelectListItem> TaskList { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }


        public MenuViewModel() { }
        public MenuViewModel(SADM_MENU model)
        {
            this.SM_Id = model.SM_Id;
            this.SM_College_Id = model.SM_College_Id;
            this.SM_Parent_Id = model.SM_Parent_Id;
            this.SM_Menu_Name = model.SM_Menu_Name;
            this.SM_Role_Id = model.SM_Role_Id;
            this.SM_Task_Id = model.SM_Task_Id;
            this.SM_IsComponent = model.SM_IsComponent;
            this.SM_IsActive = model.SM_IsActive;
            this.SM_Class = model.SM_Class;
        }
    }
    public class RoleTaskMappingViewModel
    {
        public int SRTM_Id { get; set; }
        public int SRTM_Role_Id { get; set; }
        public int SRTM_Task_Id { get; set; }
        public bool? STRM_IsActive { get; set; }

        public IEnumerable<SelectListItem> RoleList { get; set; }
        public List<int> TaskIdList { get; set; }
        public RoleTaskMappingViewModel() { }
        public RoleTaskMappingViewModel(SADM_ROLE_TASK_MAPPING model)
        {
            this.SRTM_Id = model.SRTM_Id;
            this.SRTM_Role_Id = model.SRTM_Role_Id;
            this.SRTM_Task_Id = model.SRTM_Task_Id;
            this.STRM_IsActive = model.SRTM_IsActive;
        }
    }
    #endregion


    #region ADM Related VieModels
    public class AdmLovViewModel
    {
        public int AL_Id { get; set; }
        [CustomValidationAttribute]
        public string AL_Code { get; set; }
        [CustomValidationAttribute]
        public string AL_Type { get; set; }
        public int AL_CollegeId { get;set;}
        [CustomValidationAttribute]
        public string AL_Description { get; set; }
        public bool AL_IsActive { get; set; }
        public DateTime AL_StartDate { get; set; }
        public DateTime? AL_EndDate { get; set; }
        public int? AL_Parent_Id { get; set; }
        public bool Flag { get; set; }

        public List<SelectListItem> LovTypList { get; set; }

        public AdmLovViewModel() { }
        public AdmLovViewModel(ADM_LOV model)
        {
            this.AL_Id = model.AL_Id;
            this.AL_CollegeId=model.AL_CollegeId;
            this.AL_Code = model.AL_Code;
            this.AL_Type = model.AL_Type;
            this.AL_Description = model.AL_Description;
            this.AL_IsActive = model.AL_IsActive;
            this.AL_StartDate = model.AL_StartDate;
            this.AL_EndDate = model.AL_EndDate;
            this.AL_Parent_Id = AL_Parent_Id;
        }
    }
    public class ImageUploderViewModel {
        public string FormId { get; set; }
        public string TargetName { get; set; }
        public string FuctionName { get; set; }
        public string CallBackFunction { get; set; }
        public string ReturnFunction { get; set; }
        public string ImgDiv { get; set; }
    }
    public class PositionLevelViewModel
    {
        public int APL_ID { get; set; }
        public int APL_College_Id { get; set; }
        public int APL_Parent_Id { get; set; }
        public string APL_ShortName { get; set; }
        public string APL_Description { get; set; }
        public bool APL_IsActive { get; set; }
        public bool APL_IsBaseLevel { get; set; }
        public int? APL_BaseLevel_Id { get; set; }
        public int APL_Role_Id { get; set; }
        [CustomValidationAttribute]
        public bool ReleaseMemory { get; set; }

        public IEnumerable<SelectListItem> RoleList { get; set; }

        public PositionLevelViewModel() { }
        public PositionLevelViewModel(ADM_POSITION_LEVEL model)
        {
            this.APL_ID = model.APL_ID;
            this.APL_College_Id = model.APL_College_Id;
            this.APL_ShortName = model.APL_ShortName;
            this.APL_Description = model.APL_Description;
            this.APL_Parent_Id = model.APL_Parent_Id;
            this.APL_IsActive = model.APL_IsActive;
            this.APL_IsBaseLevel = model.APL_IsBaseLevel??false;
            this.APL_BaseLevel_Id = model.APL_BaseLevel_Id;
            this.APL_Role_Id = model.APL_Role_Id;
        }
    }
    public class PositionsViewModel
    {
        public long AP_ID { get; set; }
        public int AP_College_Id { get; set; }
        public int AP_Department_Id { get; set; }
        public long AP_Parent_Id { get; set; }
        public int AP_PositionLevel_Id { get; set; }
        public string AP_ShortName { get; set; }
        public string AP_Description { get; set; }
        public DateTime AP_StartDate { get; set; }
        public DateTime? AP_EndDate { get; set; }

        public string AP_PositionLevelString { get; set; }
        public bool NeedToShowCollege { get; set; }

        public IEnumerable<SelectListItem> PositionLevelList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public PositionsViewModel() { }
        public PositionsViewModel(ADM_POSITIONS model)
        {
            this.AP_ID = model.AP_ID;
            this.AP_College_Id = model.AP_College_Id;
            this.AP_Department_Id = model.AP_Department_Id;
            this.AP_PositionLevel_Id = model.AP_PositionLevel_Id;
            this.AP_ShortName = model.AP_ShortName;
            this.AP_Description = model.AP_Description;
            this.AP_Parent_Id = model.AP_Parent_Id;
            this.AP_StartDate = model.AP_StartDate;
            this.AP_EndDate = model.AP_EndDate;
        }

    }
    public class PositionStaffMappingViewModel
    {
        public long APSM_ID { get; set; }
        public int APSM_College_Id { get; set; }
        public int APSM_Department_Id { get; set; }
        public long APSM_Position_Id { get; set; }
        public long APSM_Staff_Id { get; set; }
        public int APSM_PositionLevel_Id { get; set; }
        public DateTime APSM_StartDate { get; set; }
        public DateTime? APSM_EndDate { get; set; }
        public bool? APSM_IsActive { get; set; }
        public PositionStaffMappingViewModel(ADM_POSITION_STAFF_MAPPING model)
        {
            this.APSM_ID = model.APSM_ID;
            this.APSM_College_Id = model.APSM_College_Id;
            this.APSM_Department_Id = model.APSM_Department_Id;
            this.APSM_Position_Id = model.APSM_Position_Id;
            this.APSM_PositionLevel_Id = model.APSM_PositionLevel_Id;
            this.APSM_Staff_Id = model.APSM_Staff_Id;
            this.APSM_StartDate = model.APSM_StartDate;
            this.APSM_EndDate = model.APSM_EndDate;
            this.APSM_IsActive = model.APSM_IsActive;
        }
    }
    #endregion

    public class StaffViewModel
    {
        public UsersViewModel UserViewModel { get; set; }
        public AuthenticationViewModel UserAuthentication { get; set; }
        public AddressViewModel AddressViewModel { get; set; }

        public List<AddressViewModel> AddressList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> GenderList { get; set; }

        public StaffViewModel() { }
    }
    public class StudentViewModel
    {
        public UsersViewModel UserViewModel { get; set; }
        public AuthenticationViewModel UserAuthentication { get; set; }
        public AddressViewModel AddressViewModel { get; set; }
        public ParentViewModel ParentViewModel { get; set; }

        public List<AddressViewModel> AddressList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> GenderList { get; set; }

        public StudentViewModel() { }
    }
    public class ParentViewModel
    {
        public long APD_Id { get; set; }
        public int APD_CollegeId { get; set; }
        public string APD_Title { get; set; }
        public string APD_FirstName { get; set; }
        public string APD_MiddleName { get; set; }
        public string APD_LastName { get; set; }
        public string APD_Gender { get; set; }
        public DateTime? APD_DOB { get; set; }
        public string APD_User_Type { get; set; }
        public DateTime? APD_Anniversary_Date { get; set; }
        public DateTime APD_StartDate { get; set; }
        public DateTime? APD_EndDate { get; set; }
        public int APD_Occupation { get; set; }
        public string APD_Email_Id { get; set; }
        public string APD_PhoneNo { get; set; }
        public string APD_MobileNo { get; set; }
        public string APD_FaxNo { get; set; }

        public IEnumerable<DataKeeper.UserDetails> UserDetailList { get; set; }
        public IEnumerable<DataKeeper.UserDetails> ParentDetailList { get; set; }
        public AuthenticationViewModel UserAuthentication { get; set; }
        public AddressViewModel AddressViewModel { get; set; }
        

        public List<AddressViewModel> AddressList { get; set; }
        public IEnumerable<SelectListItem> GenderList { get; set; }
        public IEnumerable<SelectListItem> OccupationList { get; set; }

        public ParentViewModel() { }
        public ParentViewModel(ADM_PARENT_DETAILS model)
        {
            this.APD_Id = model.APD_Id;
            this.APD_CollegeId = model.APD_CollegeId;
            this.APD_Title = model.APD_Title;
            this.APD_FirstName = model.APD_FirstName;
            this.APD_MiddleName = model.APD_MiddleName;
            this.APD_LastName = model.APD_LastName;
            this.APD_Gender = model.APD_Gender;
            this.APD_DOB = model.APD_DOB;
            this.APD_Anniversary_Date = model.APD_Anniversary_Date;
            this.APD_StartDate = model.APD_StartDate;
            this.APD_EndDate = model.APD_EndDate;
            this.APD_Occupation = model.APD_Occupation;
            this.APD_Email_Id = model.APD_Email_Id;
            this.APD_PhoneNo = model.APD_PhoneNo;
            this.APD_MobileNo = model.APD_MobileNo;
            this.APD_FaxNo = model.APD_FaxNo;
        }
    }

    public class OrganizationViewModel
    {
        public long ORG_Id { get; set; }
        public string ORG_Code { get; set; }
        public string ORG_ShortDescription { get; set; }
        public string ORG_Description { get; set; }
        public string ORG_History { get; set; }
        public string ORG_DepartmentLogo { get; set; }
        public string ORG_PhoneNo { get; set; }
        public string ORG_MobileNo { get; set; }
        public string ORG_EmailId { get; set; }
        public string ORG_Website { get; set; }
        public string ORG_FaxNo { get; set; }
        public DateTime ORG_StartDate { get; set; }
        public DateTime? ORG_EndDate { get; set; }

        public string Add_EdtitUrl { get; set; }
        public string DeleteUrl { get; set; }
        public string GridLoadUrl { get; set; }
        public string RedirectUrl { get; set; }
        public string CancelUrl { get; set; }
        public string LogoBinaryCode { get; set; }

        public List<OrganizationContactPersonViewModel> OCPViewModelList { get; set; }
        public OrganizationContactPersonViewModel OCPViewModel { get; set; }
        public AuthenticationViewModel UserAuthentication { get; set; }
        public ImageUploderViewModel IUVModel { get; set; }
    }
    public class OrganizationContactPersonViewModel
    {
        public long ORGCP_Id { get; set; }
        public string ORGCP_ContactPerson { get; set; }
        public string ORGCP_Qualification { get; set; }
        public string ORGCP_ProfileImage { get; set; }
        public string ORGCP_PhoneNumber { get; set; }
        public string ORGCP_EmailId { get; set; }
        public string ORGCP_FaxNo { get; set; }
        public string ORGCP_City { get; set; }
        public string ORGCP_State { get; set; }
        public string ORGCP_Country { get; set; }
        public string ORGCP_Pincode { get; set; }
        public DateTime ORGCP_StartDate { get; set; }
        public DateTime? ORGCP_EndDate { get; set; }
        public string CPGridLoadUrl { get; set; }
        public string AddressUrl { get; set; }
        public string CPBinaryCode { get; set; }
       
    }


#region Config ViewModel
    public class ConfigFieldLevelValidation
    {
        public int CFV_ID { get; set; }
        [CustomValidationAttribute]
        public string CFV_ViewModelName { get; set; }
        public string CFV_ValidationString { get; set; }
        public Dictionary<string, Dictionary<string, string>> KeyList { get; set; }
        public bool IsMandatory { get; set; }
        public bool Allow_WhiteSpace { get; set; }
        public bool Need_EmailValidation { get; set; }
        public bool Allow_SpecialCharacter { get; set; }
        public bool IsInteger { get; set; }
        public bool IsAmount { get; set; }
        public string DateFormat { get; set; }
        public int ? MinimumLength { get; set; }
        public int ? MaximumLength { get; set; }
        public bool IsAlphaNumeric { get; set; }
        public string DependentField { get; set; }
        public string Range { get; set; }
        [CustomValidationAttribute]
        public string DisplayName { get; set; }
        [CustomValidationAttribute]
        public string ReleaseMemory { get; set; }


        public string DataType { get; set; }
        public string SelectedFieldName { get; set; }
        public IEnumerable<SelectListItem> ViewModelNameList { get; set; }
        public IEnumerable<SelectListItem> DataTypeList { get; set; }
        public IEnumerable<SelectListItem> FilteredFieldList { get; set; }
        public Dictionary<string, Dictionary<string, object>> ValidationList { get; set; }
    }
#endregion
}