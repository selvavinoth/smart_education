using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartEdu.ViewModel
{
    public class AuthenticationViewModel
    {
        public long AUT_Id { get; set; }
        public int AUT_College_Id { get; set; }
        public long AUT_User_Id { get; set; }
        public string AUT_Login_Id { get; set; }
        public string AUT_Password { get; set; }
        public string AUT_Confirm_Password { get; set; }
        public int AUT_Question { get; set; }
        public string AUT_Answer { get; set; }
        public DateTime AUT_Start_Date { get; set; }
        public DateTime? AUT_EndDate_Date { get; set; }
        public long AUT_Created_By { get; set; }
        public DateTime AUT_Created_Date { get; set; }
        public long? AUT_Modified_By { get; set; }
        public DateTime? AUT_Modified_Date { get; set; }
        public string AUT_Status { get; set; }
        public bool AUT_IsActive { get; set; }
        public int? AUT_RoleId { get; set; }
        public bool NeedToShowRole { get; set; }


        public IEnumerable<SelectListItem> RoleList { get; set; }
        public IEnumerable<SelectListItem> QuestionList { get; set; }

    }
}