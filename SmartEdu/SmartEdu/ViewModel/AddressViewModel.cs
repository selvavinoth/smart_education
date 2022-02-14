using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartEdu.ViewModel
{
    public class AddressViewModel
    {
        public long AD_Id { get; set; }
        public int AD_College_Id { get; set; }
        public long AD_User_Id { get; set; }
        public int AD_AddressType { get; set; }
        public string AD_Address1 { get; set; }
        public string AD_Address2 { get; set; }
        public string AD_Street { get; set; }
        public string AD_City { get; set; }
        public string AD_State { get; set; }
        public string AD_Country { get; set; }
        public string AD_Pincode { get; set; }
        public bool AD_IsPrimary { get; set; }
        public long AD_Created_By { get; set; }
        public DateTime AD_Created_Date { get; set; }
        public long? AD_Modified_By { get; set; }
        public DateTime? AD_Modified_Date { get; set; }
        public string AD_Status { get; set; }
        public bool AD_IsActive { get; set; }

        public string AD_GridLoadUrl { get; set; }

        public IEnumerable<SelectListItem> AddressTypeList { get; set; }

    }
}