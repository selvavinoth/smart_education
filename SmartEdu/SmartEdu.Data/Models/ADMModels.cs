using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SmartEdu.Data.Models
{
    public class ADM_LOV
    {
        [Key]
        public int AL_Id { get; set; }
        public int AL_CollegeId { get; set; }
        public string AL_Code { get; set; }
        public string AL_Type { get; set; }
        public string AL_Description { get; set; }
        public bool AL_IsActive { get; set; }
        public DateTime AL_StartDate { get; set; }
        public DateTime? AL_EndDate { get; set; }
        public int? AL_Parent_Id { get; set; }
    }
    public class ADM_POSITION_LEVEL
    {
        [Key]
        public int APL_ID {get;set;}
        public int APL_College_Id { get; set; }
        public int  APL_Parent_Id {get;set;}
        public string APL_ShortName { get;set;}
        public string APL_Description { get;set;}
        public bool APL_IsActive { get;set;}
        public bool? APL_IsBaseLevel { get; set; }
        public int? APL_BaseLevel_Id { get; set; }
        public int APL_Role_Id { get; set; }
    }
    public class ADM_POSITIONS
    {
        [Key]
        public long AP_ID{get;set;}
        public int AP_College_Id{get;set;}
        public int AP_Department_Id { get; set; }
        public long AP_Parent_Id{get;set;}
        public int AP_PositionLevel_Id { get; set; }
        public string AP_ShortName{get;set;}
        public string AP_Description{get;set;}
        public long AP_CreatedBy{get;set;}
        public DateTime AP_CreatedDate { get;set;}
        public long AP_ModifiedBy{get;set;}
        public DateTime ? AP_ModifiedDate { get;set;}
        public DateTime AP_StartDate { get;set;}
        public DateTime ? AP_EndDate { get;set;}
        public bool ? AP_IsActive { get;set;}
    }
    public class ADM_POSITION_STAFF_MAPPING
    {
        [Key]
        public long APSM_ID { get; set; }
        public int APSM_College_Id { get; set; }
        public int APSM_Department_Id { get; set; }
        public long APSM_Position_Id { get; set; }
        public long APSM_Staff_Id { get; set; }
        public int APSM_PositionLevel_Id { get; set; }
        public long APSM_CreatedBy { get; set; }
        public DateTime APSM_CreatedDate { get; set; }
        public long ? APSM_ModifiedBy { get; set; }
        public DateTime? APSM_ModifiedDate { get; set; }
        public DateTime APSM_StartDate { get; set; }
        public DateTime? APSM_EndDate { get; set; }
        public bool? APSM_IsActive { get; set; }
    }
    public class ADM_PAPER
    { 
        [Key]
        public long AP_Id{get;set;}
        public int AP_College_Id { get; set; }
        public int AP_Department_Id { get; set; }
        public string AP_Code { get;set;}
        public string AP_ShortName { get;set;}
        public string AP_Description { get;set;}
        public long AP_CreatedBy { get; set; }
        public long ? AP_ModifiedBy { get; set; }
        public DateTime AP_CreatedDate { get; set; }
        public DateTime? AP_ModifiedDate { get; set; }
        public DateTime AP_StartDate { get; set; }
        public DateTime ? AP_EndDate { get; set; }
        public bool AP_IsPractical { get; set; }
        public bool AP_IsActive { get; set; }
    }
    public class ADM_CLASSES
    {
        [Key]
        public int AC_Id { get; set; }
        public int AC_CollegeId { get; set; }
        public int AC_Graduate { get; set; }
        public int AC_Degree { get; set; }
        public int AC_Department { get; set; }
        public int AC_Year { get; set; }
        public int AC_Section { get; set; }
        public long AC_CreatedBy { get; set; }
        public long? AC_ModifiedBy { get; set; }
        public DateTime AC_CreatedDate { get; set; }
        public DateTime? AC_ModifiedDate { get; set; }
        public DateTime AC_StartDate { get; set; }
        public DateTime? AC_EndDate { get; set; }
        public string AC_Status { get; set; }
        public bool AC_IsActive { get; set; }
    }
    public class ADM_BATCH { 
        [Key]
        public int AB_Id { get;set;}
        public int AB_CollegeId { get;set;}
        public int AB_Department { get;set;}
        public int AB_Graduate { get;set;}
        public int AB_Degree { get;set;}
        public int? AB_Year { get; set; }
        public string AB_Batch { get;set;}
        public long AB_CreatedBy { get;set;}
        public long AB_ModifiedBy { get;set;}
        public DateTime  AB_CreatedDate { get;set;}
        public DateTime ? AB_ModifiedDate { get;set;}
        public DateTime  AB_StartDate { get;set;}
        public DateTime ? AB_EndDate { get;set;}
        public string AB_Status { get;set;}
        public bool ? AB_IsActive { get;set;}
    }
    public class ADM_SEMESTER
    {
        [Key]
        public int AS_Id { get; set; }
        public int AS_CollegeId { get; set; }
        public int AS_BatchId { get; set; }
        public string AS_Code { get; set; }
        public string AS_Description { get; set; }
        public long AS_CreatedBy { get; set; }
        public long AS_ModifiedBy { get; set; }
        public DateTime AS_CreatedDate { get; set; }
        public DateTime? AS_ModifiedDate { get; set; }
        public DateTime AS_StartDate { get; set; }
        public DateTime? AS_EndDate { get; set; }
        public string AS_Status { get; set; }
        public bool? AS_IsActive { get; set; }
    }
    public class ADM_SEMESTER_PAPER_MAPPING
    {
         [Key]
        public long ASPM_Id { get; set; }
        public int ASPM_CollegeId { get; set; }
        public int ASPM_AS_Id { get; set; }
        public int ASPM_AP_Id { get; set; }
        public long ASPM_CreatedBy { get; set; }
        public long ASPM_ModifiedBy { get; set; }
        public DateTime ASPM_CreatedDate { get; set; }
        public DateTime? ASPM_ModifiedDate { get; set; }
        public DateTime ASPM_StartDate { get; set; }
        public DateTime? ASPM_EndDate { get; set; }
        public string ASPM_Status { get; set; }
        public bool? ASPM_IsActive { get; set; }
    }


    public class ADM_USERS
    { 
        [Key]
        public long AU_Id { get;set;}
        public int AU_CollegeId { get; set; }
        public string AU_Title { get; set; }
        public string AU_FirstName { get; set; }
        public string AU_MiddleName { get; set; }
        public string AU_LastName { get; set; }
        public string AU_Gender { get; set; }
        public DateTime ? AU_DOB { get; set; }
        public DateTime AU_DOJ { get; set; }
        public string AU_User_Type { get;set;}
        public DateTime ? AU_Anniversary_Date { get; set; }
        public DateTime AU_StartDate { get; set; }
        public DateTime ? AU_EndDate { get; set; }
        public string AU_Qualification { get;set;}
        public int AU_Department { get; set; }
        public bool AU_IsFA { get;set;}
        public string AU_FatherName { get;set;}
        public string AU_Email_Id { get;set;}
        public string AU_PhoneNo { get;set;}
        public string AU_MobileNo { get;set;}
        public string AU_FaxNo { get;set;}
        public string AU_FatherPhoneNo { get;set;}
        public long?AU_PrimaryAddress_Id { get; set; }
        public int? AU_PrimarySpecializationId { get; set; }
        public int ? AU_PrimaryHobby_Id { get; set; }
        public int? AU_Login_Attumpt { get; set; }
        public string AU_Log_Status { get; set; }
        public long AU_Created_By { get; set; }
        public DateTime AU_CreatedDate { get; set; }
        public long ? AU_Modified_By { get; set; }
        public DateTime ? AU_ModifiedDate { get; set; }
        public string AU_Status { get; set; }
        public bool  AU_IsActive { get;set;}
    }
    public class ADM_USER_AUTHENTICATION {
        [Key]
        public long AUA_Id { get ; set;}
        public int AUA_College_Id { get;set;}
        public long AUA_User_Id { get;set;}
        public string AUA_Login_Id { get;set;}
        public string AUA_Password { get;set;}
        public string AUA_Question { get;set;}
        public string AUA_Answer { get;set;}
        public DateTime AUA_Start_Date { get;set;}
        public DateTime ? AUA_EndDate_Date { get;set;}
        public long AUA_Created_By { get;set;}
        public DateTime AUA_Created_Date { get;set;}
        public long ? AUA_Modified_By { get;set;}
        public DateTime ? AUA_Modified_Date { get;set;}
        public string AUA_Status { get;set;}
        public bool AUA_IsActive { get;set;}
    }

    public class ADM_USER_DETAILS{
         [Key]
        public long AUD_Id { get ; set;}
        public int AUD_College_Id { get;set;}
        public long AUD_User_Id { get;set;}
        public string AUD_User_Type { get; set; }
        public string AUD_User_Code { get; set; }
        public string AUD_Login_Id { get;set;}
        public string AUD_Password { get;set;}
        public int AUD_Question { get;set;}
        public string AUD_Answer { get;set;}
        public DateTime AUD_Start_Date { get;set;}
        public DateTime ? AUD_End_Date { get;set;}
        public long AUD_Created_By { get;set;}
        public DateTime AUD_Created_Date { get;set;}
        public long ? AUD_Modified_By { get;set;}
        public DateTime ? AUD_Modified_Date { get;set;}
        public string AUD_Status { get;set;}
        public bool AUD_IsActive { get;set;}
        public int? AUD_RoleId { get; set; }
        public int? AUD_LoginAttempt { get; set; }
        public string AUD_LoginStatus { get; set; }

    }
    public class ADM_USER_ADDRESS { 
        [Key]
        public long AUAD_Id {get;set;}
        public int AUAD_College_Id { get ;set ;}
        public long AUAD_User_Id { get;set;}
        public int AUAD_AddressType { get ;set;}
        public string AUAD_Address1 { get;set;}
        public string AUAD_Address2 { get;set;}
        public string AUAD_Street { get;set;}
        public string AUAD_City { get;set;}
        public string AUAD_State { get;set;}
        public string AUAD_Country { get;set;}
        public string AUAD_Pincode { get;set;}
        public bool AUAD_IsPrimary { get;set;}
        public long AUAD_Created_By { get;set;}
        public DateTime AUAD_Created_Date { get;set;}
        public long ? AUAD_Modified_By { get;set;}
        public DateTime ? AUAD_Modified_Date { get;set;}
        public string AUAD_Status { get;set;}
        public bool AUAD_IsActive { get;set;}
    }

    public class ADM_STAFF_DETAILS
    { 
        [Key]
        public long ASF_Id { get; set; }
        public int ASF_CollegeId { get; set; }
        public int ASF_Department { get; set; }
        public string ASF_Title { get; set; }
        public string ASF_FirstName { get; set; }
        public string ASF_MiddleName { get; set; }
        public string ASF_LastName { get; set; }
        public string ASF_Gender { get; set; }
        public DateTime? ASF_DOB { get; set; }
        public DateTime ASF_DOJ { get; set; }
        public DateTime? ASF_Anniversary_Date { get; set; }
        public DateTime ASF_StartDate { get; set; }
        public DateTime? ASF_EndDate { get; set; }
        public string ASF_Qualification { get; set; }
        public bool ASF_IsFA { get; set; }
        public string ASF_Email_Id { get; set; }
        public string ASF_PhoneNo { get; set; }
        public string ASF_MobileNo { get; set; }
        public string ASF_FaxNo { get; set; }
        public string ASF_FatherName { get; set; }
        public string ASF_FatherPhoneNo { get; set; }
        public long? ASF_PrimaryAddress_Id { get; set; }
        public int? ASF_PrimarySpecializationId { get; set; }
        public int? ASF_PrimaryHobby_Id { get; set; }
        public long ASF_Created_By { get; set; }
        public DateTime ASF_CreatedDate { get; set; }
        public long? ASF_Modified_By { get; set; }
        public DateTime? ASF_ModifiedDate { get; set; }
        public string ASF_Status { get; set; }
        public bool ASF_IsActive { get; set; }
    }
    public class ADM_STAFF_ADDRESS { 
        [Key]
        public long ASFA_Id {get;set;}
        public int ASFA_College_Id { get ;set ;}
        public long ASFA_Staff_Id { get;set;}
        public int ASFA_AddressType { get ;set;}
        public string ASFA_Address1 { get;set;}
        public string ASFA_Address2 { get;set;}
        public string ASFA_Street { get;set;}
        public string ASFA_City { get;set;}
        public string ASFA_State { get;set;}
        public string ASFA_Country { get;set;}
        public string ASFA_Pincode { get;set;}
        public bool ASFA_IsPrimary { get;set;}
        public long ASFA_Created_By { get;set;}
        public DateTime ASFA_Created_Date { get;set;}
        public long ? ASFA_Modified_By { get;set;}
        public DateTime ? ASFA_Modified_Date { get;set;}
        public string ASFA_Status { get;set;}
        public bool ASFA_IsActive { get;set;}
    }
    public class ADM_STUDENT_DETAILS
    {
        [Key]
        public long ASD_Id { get; set; }
        public int ASD_CollegeId { get; set; }
        public int ASD_Department { get; set; }
        public string ASD_Title { get; set; }
        public string ASD_FirstName { get; set; }
        public string ASD_MiddleName { get; set; }
        public string ASD_LastName { get; set; }
        public string ASD_Gender { get; set; }
        public DateTime? ASD_DOB { get; set; }
        public DateTime ASD_DOJ { get; set; }
        public int ASD_Batch_Id { get; set; }
        public DateTime ASD_StartDate { get; set; }
        public DateTime? ASD_EndDate { get; set; }
        public string ASD_Email_Id { get; set; }
        public string ASD_PhoneNo { get; set; }
        public string ASD_MobileNo { get; set; }
        public string ASD_FaxNo { get; set; }
        public long? ASD_PrimaryAddress_Id { get; set; }
        public int? ASD_PrimarySpecializationId { get; set; }
        public int? ASD_PrimaryHobby_Id { get; set; }
        public long ASD_Created_By { get; set; }
        public DateTime ASD_CreatedDate { get; set; }
        public long? ASD_Modified_By { get; set; }
        public DateTime? ASD_ModifiedDate { get; set; }
        public string ASD_Status { get; set; }
        public bool ASD_IsActive { get; set; }
    }
    public class ADM_STUDENT_ADDRESS {
        [Key]
        public long ASA_Id { get; set; }
        public int ASA_College_Id { get; set; }
        public long ASA_Student_Id { get; set; }
        public long ASA_Parent_Id { get; set; }
        public int ASA_AddressType { get; set; }
        public string ASA_Address1 { get; set; }
        public string ASA_Address2 { get; set; }
        public string ASA_Street { get; set; }
        public string ASA_City { get; set; }
        public string ASA_State { get; set; }
        public string ASA_Country { get; set; }
        public string ASA_Pincode { get; set; }
        public bool ASA_IsPrimary { get; set; }
        public long ASA_Created_By { get; set; }
        public DateTime ASA_Created_Date { get; set; }
        public long? ASA_Modified_By { get; set; }
        public DateTime? ASA_Modified_Date { get; set; }
        public string ASA_Status { get; set; }
        public bool ASA_IsActive { get; set; }
    }
    public class ADM_PARENT_DETAILS
    {
        [Key]
        public long APD_Id { get; set; }
        public int APD_CollegeId { get; set; }
        public string APD_Title { get; set; }
        public string APD_FirstName { get; set; }
        public string APD_MiddleName { get; set; }
        public string APD_LastName { get; set; }
        public string APD_Gender { get; set; }
        public long APD_StudentId { get; set; }
        public DateTime? APD_DOB { get; set; }
        public DateTime? APD_Anniversary_Date { get; set; }
        public DateTime APD_StartDate { get; set; }
        public DateTime? APD_EndDate { get; set; }
        public int APD_Occupation { get; set; }
        public string APD_Email_Id { get; set; }
        public string APD_PhoneNo { get; set; }
        public string APD_MobileNo { get; set; }
        public string APD_FaxNo { get; set; }
        public long? APD_PrimaryAddress_Id { get; set; }
        public long APD_Created_By { get; set; }
        public DateTime APD_CreatedDate { get; set; }
        public long? APD_Modified_By { get; set; }
        public DateTime? APD_ModifiedDate { get; set; }
        public string APD_Status { get; set; }
        public bool APD_IsActive { get; set; }
    }

    public class ADM_DEPARTMENTS {
        [Key]
        public long ADP_Id { get; set; }
        public int ADP_CollegeId { get; set; }
        public string ADP_Code { get; set; }
        public string ADP_ShortDescription { get; set; }
        public string ADP_Description { get; set; }
        public string ADP_History { get; set; }
        public string ADP_DepartmentLogo { get; set; }
        public string ADP_PhoneNo { get; set; }
        public string ADP_MobileNo { get; set; }
        public string ADP_EmailId { get; set; }
        public string ADP_Website { get; set; }
        public string ADP_FaxNo { get; set; }
        public DateTime ADP_StartDate { get; set; }
        public DateTime? ADP_EndDate { get; set; }
        public long ADP_Created_By { get; set; }
        public DateTime ADP_CreatedDate { get; set; }
        public long? ADP_Modified_By { get; set; }
        public DateTime? ADP_ModifiedDate { get; set; }
        public string ADP_Status { get; set; }
        public bool ADP_IsActive { get; set; }
    }
    public class ADM_DEPARTMENT_CONTACT_PERSON
    {
        [Key]
        public long ADCP_Id { get; set; }
        public int ADCP_College_Id { get; set; }
        public int ADCP_Department_Id { get; set; }
        public string ADCP_ContactPerson { get; set; }
        public string ADCP_Qualification { get; set; }
        public string ADCP_ProfileImage { get; set; }
        public string ADCP_PhoneNumber { get; set; }
        public string ADCP_EmailId { get; set; }
        public string ADCP_FaxNo { get; set; }
        public string ADCP_City { get; set; }
        public string ADCP_State { get; set; }
        public string ADCP_Country { get; set; }
        public string ADCP_Pincode { get; set; }
        public DateTime ADCP_StartDate { get; set; }
        public DateTime? ADCP_EndDate { get; set; }
        public long ADCP_Created_By { get; set; }
        public DateTime ADCP_Created_Date { get; set; }
        public long? ADCP_Modified_By { get; set; }
        public DateTime? ADCP_Modified_Date { get; set; }
        public string ADCP_Status { get; set; }
        public bool ADCP_IsActive { get; set; }
    }
    public class ADM_COLLEGE
    {
        [Key]
        public long ACE_Id { get; set; }
        public string ACE_Code { get; set; }
        public string ACE_ShortDescription { get; set; }
        public string ACE_Description { get; set; }
        public string ACE_History { get; set; }
        public string ACE_CollegeLogo { get; set; }
        public string ACE_PhoneNo { get; set; }
        public string ACE_MobileNo { get; set; }
        public string ACE_EmailId { get; set; }
        public string ACE_Website { get; set; }
        public string ACE_FaxNo { get; set; }
        public DateTime ACE_StartDate { get; set; }
        public DateTime? ACE_EndDate { get; set; }
        public long ACE_Created_By { get; set; }
        public DateTime ACE_CreatedDate { get; set; }
        public long? ACE_Modified_By { get; set; }
        public DateTime? ACE_ModifiedDate { get; set; }
        public string ACE_Status { get; set; }
        public bool ACE_IsActive { get; set; }
    }
    public class ADM_COLLEGE_CONTACT_PERSON
    {
        [Key]
        public long ACCP_Id { get; set; }
        public int ACCP_College_Id { get; set; }
        public string ACCP_ContactPerson { get; set; }
        public string ACCP_Qualification { get; set; }
        public string ACCP_ProfileImage { get; set; }
        public string ACCP_PhoneNumber { get; set; }
        public string ACCP_EmailId { get; set; }
        public string ACCP_FaxNo { get; set; }
        public string ACCP_City { get; set; }
        public string ACCP_State { get; set; }
        public string ACCP_Country { get; set; }
        public string ACCP_Pincode { get; set; }
        public DateTime ACCP_StartDate { get; set; }
        public DateTime? ACCP_EndDate { get; set; }
        public long ACCP_Created_By { get; set; }
        public DateTime ACCP_Created_Date { get; set; }
        public long? ACCP_Modified_By { get; set; }
        public DateTime? ACCP_Modified_Date { get; set; }
        public string ACCP_Status { get; set; }
        public bool ACCP_IsActive { get; set; }
    }



    #region Configuration Models
    public class CONFIG_FIELDLEVEL_VALIDATION
    {
        [Key]
        public int CFV_ID { get; set; }
        public int CFV_College_Id { get; set; }
        public int CFV_DP_Id { get; set; }
        public string CFV_ViewModelName { get; set; }
        public string CFV_ValidationString { get; set; }
        public long CFV_CreatedBy { get; set; }
        public DateTime CFV_CreatedDate { get; set; }
        public long CFV_ModifiedBy { get; set; }
        public DateTime? CFV_ModifiedDate { get; set; }
        public bool CFV_IsActive { get; set; }
        public string CFV_Status { get; set; }
    }
    #endregion
}