using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;
using System.Web.Mvc;

namespace SmartEdu.ViewModel
{
    public class SemesterPaperMappingViewModel
    {
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

        public string ASPM_Batch { get; set; }
        public int ASPM_Graduation { get; set; }
        public IEnumerable<SelectListItem> BatchList { get; set; }
        public IEnumerable<SelectListItem> PaperList { get; set; }
        public IEnumerable<SelectListItem> SemesterList { get; set; }
        public IEnumerable<SelectListItem> GraduationList { get; set; }

        public SemesterPaperMappingViewModel() { }
        public SemesterPaperMappingViewModel(ADM_SEMESTER_PAPER_MAPPING model) {
            this.ASPM_Id = model.ASPM_Id;
            this.ASPM_CollegeId = model.ASPM_CollegeId;
            this.ASPM_AP_Id = model.ASPM_AP_Id;
            this.ASPM_AS_Id = model.ASPM_AS_Id;
            this.ASPM_CreatedBy = model.ASPM_CreatedBy;
            this.ASPM_CreatedDate = model.ASPM_CreatedDate;
            this.ASPM_ModifiedBy = model.ASPM_ModifiedBy;
            this.ASPM_ModifiedDate = model.ASPM_ModifiedDate;
            this.ASPM_StartDate = model.ASPM_StartDate;
            this.ASPM_EndDate = model.ASPM_EndDate;
            this.ASPM_Status = model.ASPM_Status;
            this.ASPM_IsActive = model.ASPM_IsActive;
        }
    }
}