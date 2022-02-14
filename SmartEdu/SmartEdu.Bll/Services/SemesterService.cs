using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Repositories;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Models;

namespace SmartEdu.Bll.Services
{
    public interface ISemesterService
    {
        IEnumerable<ADM_LOV> GetLovList(int collegeId, string type,DateTime cDate);
        IEnumerable<ADM_BATCH> GetBatchList(int collegeId, int graduateId, int degreeId, DateTime currentDate);
        object GetSemesterGridData(int collegeId, List<string> typeList, DateTime cDate,DateTime nDate);
        bool CheckForDuplicate(ADM_SEMESTER model, DateTime cDate);
        void InsertSemester(ADM_SEMESTER model, long modifiedBy);
        bool UpdateSemester(ADM_SEMESTER model, long modifiedBy, DateTime cDate);
        bool DeleteSemester(int collegeId, int semesterId, long modifiedBy, DateTime cDate);
    }
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository semesterRepository;
        private readonly IAdmLOVRepository aLOVRepository;
        private readonly IBatchRepository batchRepository;
        private readonly IUnitOfWork unitOfWork;
        public SemesterService(ISemesterRepository semesterRepository, IAdmLOVRepository aLOVRepository, IBatchRepository batchRepository, IUnitOfWork unitOfWork)
        {
            this.semesterRepository = semesterRepository;
            this.aLOVRepository = aLOVRepository;
            this.batchRepository = batchRepository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ADM_LOV> GetLovList(int collegeId, string type, DateTime cDate)
        {
            try
            {
                return aLOVRepository.GetMany(exp => exp.AL_Type == type && exp.AL_StartDate <= cDate && (exp.AL_EndDate ?? cDate) >= cDate).ToList();
            }
            finally { }
        }
        private IEnumerable<ADM_LOV> GetLovList(int collegeId, List<string> typeList, DateTime cDate)
        {
            try
            {
                return aLOVRepository.GetMany(exp => typeList.Contains(exp.AL_Type) && exp.AL_StartDate <= cDate && (exp.AL_EndDate ?? cDate) >= cDate).ToList();
            }
            finally { }
        }
        public IEnumerable<ADM_BATCH> GetBatchList(int collegeId, int graduateId, int degreeId,DateTime currentDate)
        {
            try
            {
                return batchRepository.GetMany(exp => exp.AB_CollegeId == collegeId && exp.AB_Graduate == graduateId && exp.AB_Degree == degreeId && exp.AB_StartDate <= currentDate && (exp.AB_EndDate ?? currentDate) >= currentDate);
            }
            finally { }
        }
        public object GetSemesterGridData(int collegeId, List<string> typeList, DateTime cDate,DateTime nDate)
        {
            IEnumerable<ADM_SEMESTER> semesterList;
            IEnumerable<ADM_BATCH> batchList;
            IEnumerable<ADM_LOV> lovList;
            try
            {
                semesterList = semesterRepository.GetMany(exp => exp.AS_CollegeId == collegeId && exp.AS_StartDate <= cDate && (exp.AS_EndDate ?? nDate) >= nDate);
                if (semesterList == null || semesterList.Count() == 0) { return new { total_count = 0, rows = new { } }; }
                batchList = batchRepository.GetMany(exp => exp.AB_CollegeId == collegeId && exp.AB_StartDate <= cDate && (exp.AB_EndDate ?? cDate) >= cDate);
                if (batchList == null || batchList.Count() == 0) { return new { total_count = 0, rows = new { } }; }
                lovList = GetLovList(collegeId, typeList, cDate);
                if (lovList == null || lovList.Count() == 0) { return new { total_count = 0, rows = new { } }; }

                return (new
                {
                    total_count = 0,
                    rows = (from s in semesterList
                            join b in batchList on s.AS_BatchId equals b.AB_Id
                            join l in lovList on b.AB_Graduate equals l.AL_Id
                            join ld in lovList on b.AB_Degree equals ld.AL_Id
                            join ldep in lovList on b.AB_Department equals ldep.AL_Id
                            select new
                            {
                                id = s.AS_Id,
                                data = new string[] { l.AL_Id.ToString(), ld.AL_Id.ToString(), b.AB_Id.ToString(), l.AL_Code, ld.AL_Code, ldep.AL_Code, b.AB_Batch, s.AS_Code, s.AS_Description, s.AS_StartDate.ToString("dd/MM/yyyy"), (s.AS_EndDate.HasValue ? s.AS_EndDate.Value.ToString("dd/MM/yyyy") : "") }
                            }).ToList()
                });

            }
            finally { semesterList = null; batchList = null; lovList = null; }
        }
        public bool CheckForDuplicate(ADM_SEMESTER model, DateTime cDate)
        {
            ADM_SEMESTER temp = null;
            try
            {
                if (model.AS_Id == 0) { temp = semesterRepository.Get(exp => exp.AS_CollegeId == model.AS_CollegeId && exp.AS_BatchId == model.AS_BatchId && exp.AS_Code == model.AS_Code && exp.AS_StartDate <= cDate && (exp.AS_EndDate ?? cDate) >= cDate); }
                else { temp = semesterRepository.Get(exp => exp.AS_CollegeId == model.AS_CollegeId && exp.AS_BatchId == model.AS_BatchId && exp.AS_Code == model.AS_Code && exp.AS_StartDate <= cDate && (exp.AS_EndDate ?? cDate) >= cDate && exp.AS_Id != model.AS_Id); }
                return (temp == null);
            }
            finally { temp = null; }
        }
        public void InsertSemester(ADM_SEMESTER model, long modifiedBy)
        {
            try
            {
                model.AS_CreatedBy = modifiedBy;
                model.AS_CreatedDate = DateTime.Now;
                model.AS_IsActive = true;
                model.AS_Status = "I";
                model.AS_StartDate = DateTime.Now.Date;
                semesterRepository.Add(model);
                unitOfWork.Commit();
            }
            finally
            {
            }
        }
        public bool UpdateSemester(ADM_SEMESTER model, long modifiedBy, DateTime cDate)
        {
            ADM_SEMESTER temp = null;
            try
            {
                temp = semesterRepository.Get(exp => exp.AS_CollegeId == model.AS_CollegeId && exp.AS_Id == model.AS_Id && (exp.AS_EndDate ?? cDate) >= cDate);
                if (temp == null) { return false; }
                temp.AS_BatchId = model.AS_BatchId;
                temp.AS_Code = model.AS_Code;
                temp.AS_Description = model.AS_Description;
                temp.AS_Status = "U";
                temp.AS_ModifiedBy = modifiedBy;
                temp.AS_ModifiedDate = DateTime.Now;
                semesterRepository.Update(temp);
                unitOfWork.Commit();
                return true;
            }
            finally
            {
                temp = null;
            }
        }
        public bool DeleteSemester(int collegeId, int batchId, long modifiedBy, DateTime cDate)
        {
            ADM_SEMESTER temp = null;
            try
            {
                temp = semesterRepository.Get(exp => exp.AS_CollegeId == collegeId && exp.AS_Id == batchId && (exp.AS_EndDate ?? cDate) >= cDate);
                if (temp == null) { return false; }
                temp.AS_Status = "U";
                temp.AS_IsActive = false;
                temp.AS_ModifiedBy = modifiedBy;
                temp.AS_ModifiedDate = DateTime.Now;
                temp.AS_EndDate = cDate;
                semesterRepository.Update(temp);
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