using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;

namespace SmartEdu.ViewModel
{
    public class UserAddressViewModel
    {
        public long AUAD_Id { get; set; }
        public int AUAD_College_Id { get; set; }
        public long AUAD_User_Id { get; set; }
        public int AUAD_AddressType { get; set; }
        public string AUAD_Address1 { get; set; }
        public string AUAD_Address2 { get; set; }
        public string AUAD_Street { get; set; }
        public string AUAD_City { get; set; }
        public string AUAD_State { get; set; }
        public string AUAD_Country { get; set; }
        public string AUAD_Pincode { get; set; }
        public bool AUAD_IsPrimary { get; set; }
        public long AUAD_Created_By { get; set; }
        public DateTime AUAD_Created_Date { get; set; }
        public long? AUAD_Modified_By { get; set; }
        public DateTime? AUAD_Modified_Date { get; set; }
        public string AUAD_Status { get; set; }
        public bool AUAD_IsActive { get; set; }

        public UserAddressViewModel() { }
        public UserAddressViewModel(ADM_USER_ADDRESS model) { 
            this.AUAD_Id=model.AUAD_Id;
            this.AUAD_College_Id=model.AUAD_College_Id;
            this.AUAD_User_Id=model.AUAD_User_Id;
            this.AUAD_AddressType=model.AUAD_AddressType;
            this.AUAD_Address1=model.AUAD_Address1;
            this.AUAD_Address2=model.AUAD_Address2;
            this.AUAD_Street=model.AUAD_Street;
            this.AUAD_City=model.AUAD_City;
            this.AUAD_State=model.AUAD_State;
            this.AUAD_Country=model.AUAD_Country;
            this.AUAD_Pincode=model.AUAD_Pincode;
            this.AUAD_IsPrimary=model.AUAD_IsPrimary;
            this.AUAD_Created_By=model.AUAD_Created_By;
            this.AUAD_Created_Date=model.AUAD_Created_Date;
            this.AUAD_Modified_By=model.AUAD_Modified_By;
            this.AUAD_Modified_Date=model.AUAD_Modified_Date;
            this.AUAD_Status=model.AUAD_Status;
            this.AUAD_IsActive=model.AUAD_IsActive;
        }
    }
}