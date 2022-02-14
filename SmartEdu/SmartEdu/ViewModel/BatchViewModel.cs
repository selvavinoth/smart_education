using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;
using System.Web.Mvc;

namespace SmartEdu.ViewModel
{
    public class BatchViewModel
    {
        public int AB_Id { get; set; }
        public int AB_CollegeId { get; set; }
        public int AB_Department { get; set; }
        public int AB_Graduate { get; set; }
        public int AB_Degree { get; set; }
        public string AB_Batch { get; set; }
        public long AB_CreatedBy { get; set; }
        public long AB_ModifiedBy { get; set; }
        public DateTime AB_CreatedDate { get; set; }
        public DateTime? AB_ModifiedDate { get; set; }
        public DateTime AB_StartDate { get; set; }
        public DateTime? AB_EndDate { get; set; }
        public string AB_Status { get; set; }
        public bool? AB_IsActive { get; set; }
        public int AB_Year { get; set; }

        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> GraduationList { get; set; }
        public IEnumerable<SelectListItem> DegreeList { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }

        public BatchViewModel() { }
        public BatchViewModel(ADM_BATCH model) {
            this.AB_Id = model.AB_Id;
            this.AB_CollegeId = model.AB_CollegeId;
            this.AB_Graduate = model.AB_Graduate;
            this.AB_Department = model.AB_Department;
            this.AB_Degree = model.AB_Degree;
            this.AB_Batch = model.AB_Batch;
            this.AB_CreatedBy = model.AB_CreatedBy;
            this.AB_CreatedDate = model.AB_CreatedDate;
            this.AB_ModifiedBy = model.AB_ModifiedBy;
            this.AB_ModifiedDate = model.AB_ModifiedDate;
            this.AB_StartDate = model.AB_StartDate;
            this.AB_EndDate = model.AB_EndDate;
            this.AB_Status = model.AB_Status;
            this.AB_IsActive = model.AB_IsActive;
        }
    }
}