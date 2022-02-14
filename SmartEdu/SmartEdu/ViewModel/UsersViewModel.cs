using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;
using System.Web.Mvc;
using SmartEdu.Helper;

namespace SmartEdu.ViewModel
{
    public class UsersViewModel
    {
        public long AU_Id { get; set; }
        public int AU_CollegeId { get; set; }
        public string AU_Title { get; set; }
        public string AU_FirstName { get; set; }
        public string AU_MiddleName { get; set; }
        public string AU_LastName { get; set; }
        public string AU_Gender { get; set; }
        public DateTime? AU_DOB { get; set; }
        public DateTime AU_DOJ { get; set; }
        public string AU_User_Type { get; set; }
        public DateTime? AU_Anniversary_Date { get; set; }
        public DateTime AU_StartDate { get; set; }
        public DateTime? AU_EndDate { get; set; }
        public string AU_Qualification { get; set; }
        public int AU_Department { get; set; }
        public bool AU_IsFA { get; set; }
        public string AU_FatherName { get; set; }
        public string AU_Email_Id { get; set; }
        public string AU_PhoneNo { get; set; }
        public string AU_MobileNo { get; set; }
        public string AU_FaxNo { get; set; }
        public string AU_FatherPhoneNo { get; set; }
        public long? AU_PrimaryAddress_Id { get; set; }
        public int? AU_PrimarySpecializationId { get; set; }
        public int? AU_PrimaryHobby_Id { get; set; }
        public int? AU_Login_Attumpt { get; set; }
        public string AU_Log_Status { get; set; }

        public ImageUploderViewModel ImageUploaderViewModel { get; set; }
        public IEnumerable<DataKeeper.UserDetails> UserDetailList { get; set; }
        public AuthenticationViewModel UserAuthentication { get; set; }
        public AddressViewModel AddressViewModel { get; set; }

        public List<AddressViewModel> AddressList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> GenderList { get; set; }
        
        
        public UsersViewModel() { }
        public UsersViewModel(ADM_USER_DETAILS model) { 
            this.AU_Id=model.AUD_Id;
            this.AU_CollegeId = model.AUD_College_Id;
            this.AU_User_Type=model.AUD_User_Type;
            this.AU_StartDate=model.AUD_Start_Date;
            this.AU_EndDate=model.AUD_End_Date;
            this.AU_Login_Attumpt = model.AUD_LoginAttempt;
            this.AU_Log_Status = model.AUD_LoginStatus;
        }
    }
}