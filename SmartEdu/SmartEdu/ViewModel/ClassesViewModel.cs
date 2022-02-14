using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;
using System.Web.Mvc;

namespace SmartEdu.ViewModel
{
    public class ClassesViewModel
    {
        public int AC_Id { get; set; }
        public int AC_CollegeId { get; set; }
        public int AC_Department { get; set; }
        public int AC_Graduate { get; set; }
        public int AC_Degree { get; set; }
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

        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> GraduationList { get; set; }
        public IEnumerable<SelectListItem> DegreeList { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
        public IEnumerable<SelectListItem> SectionList { get; set; }
       
        
        public ClassesViewModel() { }

        public ClassesViewModel(ADM_CLASSES model) {
            this.AC_Id = model.AC_Id;
            this.AC_CollegeId = model.AC_CollegeId;
            this.AC_Graduate = model.AC_Graduate;
            this.AC_Degree = model.AC_Degree;
            this.AC_Department = model.AC_Department;
            this.AC_Year = model.AC_Year;
            this.AC_Section = model.AC_Section;
            this.AC_CreatedBy = model.AC_CreatedBy;
            this.AC_CreatedDate = model.AC_CreatedDate;
            this.AC_ModifiedBy = model.AC_ModifiedBy;
            this.AC_ModifiedDate = model.AC_ModifiedDate;
            this.AC_StartDate = model.AC_StartDate;
            this.AC_EndDate = model.AC_EndDate;
            this.AC_Status = model.AC_Status;
            this.AC_IsActive = model.AC_IsActive;
        
        }
    }
}