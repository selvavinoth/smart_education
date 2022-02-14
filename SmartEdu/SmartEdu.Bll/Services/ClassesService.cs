using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;
using SmartEdu.Data.Repositories;
using SmartEdu.Data.infrastructure;

namespace SmartEdu.Bll.Services
{
    public interface IClassesService
    {
        IEnumerable<ADM_LOV> GetLovList(int collegeId, List<string> typeList);
        object GetClassGridData(int collegeId, List<string> typeList,DateTime cDate);
        bool CheckForDuplicate(ADM_CLASSES model, DateTime cDate);
        void InsertClasses(ADM_CLASSES model, long modifiedBy);
        bool UpdateClasses(ADM_CLASSES model, long modifiedBy,DateTime cDate);
        bool DeleteClasses(int collegeId, int classId, long modifiedBy, DateTime cDate);
    }
    public class ClassesService : IClassesService
    {
        private readonly IClassesRepository classesRepository;
        private readonly IAdmLOVRepository aLOVRepository;
        private readonly IUnitOfWork unitOfWork;
        public ClassesService(IClassesRepository classesRepository, IAdmLOVRepository aLOVRepository, IUnitOfWork unitOfWork)
        {
            this.classesRepository = classesRepository;
            this.aLOVRepository = aLOVRepository;
            this.unitOfWork = unitOfWork;
        }


        public IEnumerable<ADM_LOV> GetLovList(int collegeId, List<string> typeList)
        {
            try
            {
                return aLOVRepository.GetMany(exp => typeList.Contains(exp.AL_Type) && exp.AL_IsActive).ToList();
            }
            finally { }
        }
        public object GetClassGridData(int collegeId, List<string> typeList, DateTime cDate)
        {
            IEnumerable<ADM_CLASSES> classList;
            IEnumerable<ADM_LOV> lovList;
            try {
                classList = classesRepository.GetMany(exp => exp.AC_CollegeId == collegeId && (exp.AC_EndDate??cDate)>=cDate).ToList();
                if (classList == null || classList.Count() == 0) { return new { total_count = 0, rows = new { }}; }
                lovList = GetLovList(collegeId, typeList);
                if (lovList == null || lovList.Count() == 0) { return new { total_count = 0, rows = new { }}; }

                return (new {
                    total_cout=classList.Count(),
                    rows=(from c in classList
                          join d in lovList on c.AC_Graduate equals d.AL_Id
                          join dg in lovList on c.AC_Degree equals dg.AL_Id
                          join dp in lovList on c.AC_Department equals dp.AL_Id
                          join y in lovList on c.AC_Year equals y.AL_Id
                          join s in lovList on c.AC_Section equals s.AL_Id
                          select new {
                            id=c.AC_Id,
                            data = new string[] { d.AL_Id.ToString(), dg.AL_Id.ToString(), dp.AL_Id.ToString(), y.AL_Id.ToString(), s.AL_Id.ToString(), d.AL_Code, dg.AL_Code, dp.AL_Code, y.AL_Code, s.AL_Code, (c.AC_StartDate.ToString("dd/MM/yyyy")), (c.AC_EndDate.HasValue ? c.AC_EndDate.Value.ToString("dd/MM/yyyy") : "") }
                          }).ToList()
                });
            }
            finally { classList = null; lovList = null; }
        }
        public bool CheckForDuplicate(ADM_CLASSES model,DateTime cDate) {
            ADM_CLASSES temp = null;
            try {
                if (model.AC_Id == 0){temp = classesRepository.Get(exp => exp.AC_CollegeId == model.AC_CollegeId && exp.AC_Graduate == model.AC_Graduate && exp.AC_Degree == model.AC_Degree && exp.AC_Department == model.AC_Department && exp.AC_Year == model.AC_Year && exp.AC_Section == model.AC_Section && (exp.AC_EndDate ?? cDate) >= cDate);}
                else { temp = classesRepository.Get(exp => exp.AC_CollegeId == model.AC_CollegeId && exp.AC_Graduate == model.AC_Graduate && exp.AC_Degree == model.AC_Degree && exp.AC_Department == model.AC_Department && exp.AC_Year == model.AC_Year && exp.AC_Section == model.AC_Section && (exp.AC_EndDate ?? cDate) >= cDate && exp.AC_Id!=model.AC_Id); }
                return (temp == null);
            }
            finally { temp = null; }
        }
        public void InsertClasses(ADM_CLASSES model, long modifiedBy) {
            try {
                model.AC_CreatedBy = modifiedBy;
                model.AC_CreatedDate = DateTime.Now;
                model.AC_IsActive = true;
                model.AC_Status = "I";
                model.AC_StartDate = DateTime.Now.Date;
                classesRepository.Add(model);
                unitOfWork.Commit();
            }
            finally {
            }
        }
        public bool UpdateClasses(ADM_CLASSES model, long modifiedBy,DateTime cDate) {
            ADM_CLASSES temp = null;
            try
            {
                temp = classesRepository.Get(exp => exp.AC_CollegeId == model.AC_CollegeId && exp.AC_Id==model.AC_Id && (exp.AC_EndDate ?? cDate) >= cDate);
                if (temp == null) { return false; }
                temp.AC_Graduate = model.AC_Graduate;
                temp.AC_Degree = model.AC_Degree;
                temp.AC_Department = model.AC_Department;
                temp.AC_Year = model.AC_Year;
                temp.AC_Section = model.AC_Section;
                temp.AC_Status = "U";
                temp.AC_ModifiedBy = modifiedBy;
                temp.AC_ModifiedDate = DateTime.Now;
                classesRepository.Update(temp);
                unitOfWork.Commit();
                return true;
            }
            finally
            {
                temp = null;
            }
        }
        public bool DeleteClasses(int collegeId,int classId, long modifiedBy,DateTime cDate) {
            ADM_CLASSES temp = null;
            try
            {
                temp = classesRepository.Get(exp => exp.AC_CollegeId == collegeId && exp.AC_Id == classId && (exp.AC_EndDate ?? cDate) >= cDate);
                if (temp == null) { return false; }
                temp.AC_Status = "U";
                temp.AC_IsActive = false;
                temp.AC_ModifiedBy = modifiedBy;
                temp.AC_ModifiedDate = DateTime.Now;
                temp.AC_EndDate = cDate;
                classesRepository.Update(temp);
                unitOfWork.Commit();
                return true;
            }
            finally
            {
                temp = null;
            }
        }

    }


}