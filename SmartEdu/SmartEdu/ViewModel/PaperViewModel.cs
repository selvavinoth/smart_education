using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;
using System.Web.Mvc;

namespace SmartEdu.ViewModel
{
    public class PaperViewModel
    {
        public long AP_Id { get; set; }
        public int AP_College_Id { get; set; }
        public int AP_Department_Id { get; set; }
        public string AP_Code { get; set; }
        public string AP_ShortName { get; set; }
        public string AP_Description { get; set; }
        public long AP_CreatedBy { get; set; }
        public long? AP_ModifiedBy { get; set; }
        public DateTime AP_CreatedDate { get; set; }
        public DateTime? AP_ModifiedDate { get; set; }
        public DateTime AP_StartDate { get; set; }
        public DateTime? AP_EndDate { get; set; }
        public bool AP_IsPractical { get; set; }
        public bool? AP_IsActive { get; set; }

        public PaperViewModel() { }
        public PaperViewModel(ADM_PAPER model) {
            this.AP_Code = model.AP_Code;
            this.AP_College_Id = model.AP_College_Id;
            this.AP_Department_Id = model.AP_Department_Id;
            this.AP_CreatedBy = model.AP_CreatedBy;
            this.AP_CreatedDate = model.AP_CreatedDate;
            this.AP_Description = model.AP_Description;
            this.AP_EndDate = model.AP_EndDate;
            this.AP_Id = model.AP_Id;
            this.AP_IsActive = model.AP_IsActive;
            this.AP_IsPractical = model.AP_IsPractical;
            this.AP_ModifiedBy = model.AP_ModifiedBy;
            this.AP_ModifiedDate = model.AP_ModifiedDate;
            this.AP_ShortName = model.AP_ShortName;
            this.AP_StartDate = model.AP_StartDate;
            
        }
    }
}