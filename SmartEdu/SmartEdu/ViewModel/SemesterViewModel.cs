using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;
using System.Web.Mvc;

namespace SmartEdu.ViewModel
{
    public class SemesterViewModel
    {
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

        public int AS_Graduate { get; set; }
        public int AS_Degree { get; set; }

        public IEnumerable<SelectListItem> GraduationList { get; set; }
        public IEnumerable<SelectListItem> DegreeList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }

        public SemesterViewModel() { }
        public SemesterViewModel(ADM_SEMESTER model) {
            this.AS_Id = model.AS_Id;
            this.AS_CollegeId = model.AS_CollegeId;
            this.AS_BatchId = model.AS_BatchId;
            this.AS_Code = model.AS_Code;
            this.AS_Description = model.AS_Description;
            this.AS_CreatedBy = model.AS_CreatedBy;
            this.AS_CreatedDate = model.AS_CreatedDate;
            this.AS_ModifiedBy = model.AS_ModifiedBy;
            this.AS_ModifiedDate = model.AS_ModifiedDate;
            this.AS_StartDate = model.AS_StartDate;
            this.AS_EndDate = model.AS_EndDate;
            this.AS_IsActive = model.AS_IsActive;
            this.AS_Status = model.AS_Status;
        }
    }
}