using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartEdu.Helper
{
    public static class DataKeeper
    {
        public enum LOVTypeKeeper
        {
            Department,
            Year,
            Section,
            Qualification,
            Semester,
            Graduation,
            AddressType,
            Gender,
            AuthenticationQuestion,
            Exam,
            Batch
        };

        public enum UserDetails
        {
            AU_Id,
            AU_CollegeId,
            AU_Title,
            AU_FirstName,
            AU_MiddleName,
            AU_LastName,
            AU_Gender,
            AU_DOB,
            AU_DOJ,
            AU_Anniversary_Date,
            AU_User_Type,
            AU_StartDate,
            AU_EndDate,
            AU_Qualification,
            AU_Department,
            AU_IsFA,
            AU_FatherName,
            AU_Email_Id,
            AU_PhoneNo,
            AU_MobileNo,
            AU_FaxNo,
            AU_FatherPhoneNo,
            AU_Occupation
        };
        public static string DepartMent_Type = "DEPARTMENT";
        public static string Graduation_Type = "GRADUATION";
        public static string Semester_Type = "SEMESTER";
        public static string Year_Type = "YEAR";


        // Session Keeper Values
        public static string SelectedUserId = "SELECTEDUSERID";
        public static string SelectedStaffId = "SELECTEDSTAFFID";
        public static string SelectedStudentId = "SELECTEDSTUDENTID";
    }
}