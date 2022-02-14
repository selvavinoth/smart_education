using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;

namespace SmartEdu.ViewModel
{
    public class UserAuthenticationViewModel
    {
        public long AUA_Id { get; set; }
        public int AUA_College_Id { get; set; }
        public long AUA_User_Id { get; set; }
        public string AUA_Login_Id { get; set; }
        public string AUA_Password { get; set; }
        public string AUA_Question { get; set; }
        public string AUA_Answer { get; set; }
        public DateTime AUA_Start_Date { get; set; }
        public DateTime? AUA_EndDate_Date { get; set; }
        public long AUA_Created_By { get; set; }
        public DateTime AUA_Created_Date { get; set; }
        public long? AUA_Modified_By { get; set; }
        public DateTime? AUA_Modified_Date { get; set; }
        public string AUA_Status { get; set; }
        public bool AUA_IsActive { get; set; }

        public UserAuthenticationViewModel() { }
        public UserAuthenticationViewModel(ADM_USER_AUTHENTICATION model) {
            this.AUA_Id=model.AUA_Id;
            this.AUA_College_Id=model.AUA_College_Id;
            this.AUA_User_Id=model.AUA_User_Id;
            this.AUA_Login_Id=model.AUA_Login_Id;
            this.AUA_Password=model.AUA_Password;
            this.AUA_Question=model.AUA_Question;
            this.AUA_Answer=model.AUA_Answer;
            this.AUA_Start_Date=model.AUA_Start_Date;
            this.AUA_EndDate_Date=model.AUA_EndDate_Date;
            this.AUA_Created_By=model.AUA_Created_By;
            this.AUA_Created_Date=model.AUA_Created_Date;
            this.AUA_Modified_By=model.AUA_Modified_By;
            this.AUA_Modified_Date=model.AUA_Modified_Date;
            this.AUA_Status=model.AUA_Status;
            this.AUA_IsActive=model.AUA_IsActive;
        }
    }
}