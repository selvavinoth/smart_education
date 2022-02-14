using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Repositories;
using SmartEdu.Data.Models;

namespace SmartEdu.Bll.Services
{
    public interface ISemesterPaperMappingService {
        IEnumerable<ADM_SEMESTER> GetSemesterList(int collegeId,DateTime cDate);
        IEnumerable<ADM_PAPER> GetPaperList(int collegeId,DateTime cDate);
        IEnumerable<ADM_BATCH> GetBatchList(int collegeId, string batchString,int graduation, DateTime cDate);
        IEnumerable<string> GetBatchList(int collegeId, DateTime cDate);
        IEnumerable<ADM_LOV> GetLovList(int collegeId, List<string> typeList);
        object GetMappingGridData(int collegeId, DateTime cDate, DateTime nDate, List<string> typeList);
        void Insert(ADM_SEMESTER_PAPER_MAPPING model, long modifiedBy);
        bool Update(ADM_SEMESTER_PAPER_MAPPING model, long modifiedBy, DateTime cDate);
        bool Delete(int collegeId, int mappingId, long modifiedBy, DateTime cDate);
        bool CheckForDuplicate(ADM_SEMESTER_PAPER_MAPPING model, DateTime cDate);
    }

    public class SemesterPaperMappingService : ISemesterPaperMappingService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISemesterPaperMappingRepository semesterPaperMappingRepository;
        private readonly ISemesterRepository semesterRepository;
        private readonly IPaperRepository paperRepository;
        private readonly IBatchRepository batchRepository;
        private readonly IAdmLOVRepository aLOVRepository;
        public SemesterPaperMappingService(IUnitOfWork unitOfWork, ISemesterPaperMappingRepository semesterPaperMappingRepository, ISemesterRepository semesterRepository, IPaperRepository paperRepository, IBatchRepository batchRepository, IAdmLOVRepository aLOVRepository)
        {
            this.unitOfWork = unitOfWork;
            this.semesterPaperMappingRepository = semesterPaperMappingRepository;
            this.semesterRepository = semesterRepository;
            this.paperRepository = paperRepository;
            this.batchRepository = batchRepository;
            this.aLOVRepository = aLOVRepository;
        }

        public IEnumerable<ADM_PAPER> GetPaperList(int collegeId, DateTime cDate)
        {
            try {
                return paperRepository.GetMany(exp => exp.AP_College_Id == collegeId && (exp.AP_EndDate??cDate)>=cDate).ToList();
            }
            finally { }
        }
        public IEnumerable<ADM_SEMESTER> GetSemesterList(int collegeId, DateTime cDate)
        {
            try
            {
                return semesterRepository.GetMany(exp => exp.AS_CollegeId == collegeId && (exp.AS_EndDate??cDate)>=cDate).ToList();
            }
            finally { }
        }
        public IEnumerable<string> GetBatchList(int collegeId, DateTime cDate)
        {
            try
            {
                return batchRepository.GetMany(exp => exp.AB_CollegeId == collegeId && (exp.AB_EndDate ?? cDate) >= cDate).Select(exp=>exp.AB_Batch).Distinct().ToList();
            }
            finally { }
        }
        public IEnumerable<ADM_BATCH> GetBatchList(int collegeId,string batchString,int graduation, DateTime cDate)
        {
            try
            {
                return batchRepository.GetMany(exp => exp.AB_CollegeId == collegeId && exp.AB_Batch == batchString && exp.AB_Graduate==graduation && (exp.AB_EndDate ?? cDate) >= cDate).ToList();
            }
            finally { }
        }
        public IEnumerable<ADM_LOV> GetLovList(int collegeId, List<string> typeList)
        {
            try
            {
                return aLOVRepository.GetMany(exp => typeList.Contains(exp.AL_Type) && exp.AL_IsActive).ToList();
            }
            finally { }
        }
        public object GetMappingGridData(int collegeId,DateTime cDate,DateTime nDate,List<string> typeList) {
            IEnumerable<ADM_SEMESTER_PAPER_MAPPING> mappingList;
            IEnumerable<ADM_SEMESTER> semesterList;
            IEnumerable<ADM_BATCH> batchList;
            IEnumerable<ADM_PAPER> paperList;
            IEnumerable<ADM_LOV> lovList;
            try {
                mappingList = semesterPaperMappingRepository.GetMany(exp => exp.ASPM_CollegeId == collegeId && (exp.ASPM_EndDate ?? nDate) >= nDate).ToList();
                if (mappingList == null || mappingList.Count() == 0) { return new { total_count = 0, rows = new { }}; }
                semesterList = semesterRepository.GetMany(exp => exp.AS_CollegeId == collegeId && (exp.AS_EndDate ?? cDate) >= cDate).ToList();
                if (semesterList == null || semesterList.Count() == 0) { return new { total_count = 0, rows = new { }}; }
                paperList = paperRepository.GetMany(exp => exp.AP_College_Id == collegeId && (exp.AP_EndDate ?? cDate) >= cDate).ToList();
                if (paperList == null || paperList.Count() == 0) { return new { total_count = 0, rows = new { }}; }
                batchList = batchRepository.GetMany(exp => exp.AB_CollegeId == collegeId && (exp.AB_EndDate ?? cDate) >= cDate).ToList();
                if (batchList == null || batchList.Count() == 0) { return new { total_count = 0, rows = new { }}; }
                lovList = GetLovList(collegeId, typeList);
                if (lovList == null || lovList.Count() == 0) { return new { total_count = 0, rows = new { }}; }
                return new {
                    total_count = mappingList.Count(),
                    rows=(from m in mappingList
                          join s in semesterList on m.ASPM_AS_Id equals s.AS_Id
                          join p in paperList on m.ASPM_AP_Id equals p.AP_Id
                          join b in batchList on s.AS_BatchId equals b.AB_Id
                          join dl in lovList on b.AB_Department equals dl.AL_Id
                          join dg in lovList on b.AB_Degree equals dg.AL_Id
                          select new {
                            id=m.ASPM_Id,
                            data = new string[] { m.ASPM_AS_Id.ToString(), m.ASPM_AP_Id.ToString(), b.AB_Batch, s.AS_Description,p.AP_Description,(p.AP_IsPractical?"NO":"YES"), dg.AL_Code + " - [ " + dl.AL_Code + " ]", m.ASPM_StartDate.ToString("dd/MM/yyyy"), (m.ASPM_EndDate.HasValue ? m.ASPM_EndDate.Value.ToString("dd/MM/yyyy") : ""),b.AB_Graduate.ToString() }
                          }).ToList()
                };
            }
            finally { mappingList = null; semesterList = null; paperList = null; lovList = null; batchList = null; }
        }

        public bool CheckForDuplicate(ADM_SEMESTER_PAPER_MAPPING model, DateTime cDate)
        {
            ADM_SEMESTER_PAPER_MAPPING temp = null;
            try
            {
                if (model.ASPM_Id == 0) { temp = semesterPaperMappingRepository.Get(exp => exp.ASPM_CollegeId == model.ASPM_CollegeId && exp.ASPM_AP_Id== model.ASPM_AP_Id && exp.ASPM_AS_Id==model.ASPM_AS_Id && (exp.ASPM_EndDate ?? cDate) >= cDate); }
                else { temp = semesterPaperMappingRepository.Get(exp => exp.ASPM_CollegeId == model.ASPM_CollegeId && exp.ASPM_AP_Id == model.ASPM_AP_Id && exp.ASPM_AS_Id == model.ASPM_AS_Id && (exp.ASPM_EndDate ?? cDate) >= cDate && exp.ASPM_Id != model.ASPM_Id); }
                return (temp == null);
            }
            finally { temp = null; }
        }
        public void Insert(ADM_SEMESTER_PAPER_MAPPING model, long modifiedBy)
        {
            try
            {
                model.ASPM_CreatedBy = modifiedBy;
                model.ASPM_CreatedDate = DateTime.Now;
                model.ASPM_IsActive = true;
                model.ASPM_Status = "I";
                model.ASPM_StartDate = DateTime.Now.Date;
                semesterPaperMappingRepository.Add(model);
                unitOfWork.Commit();
            }
            finally
            {
            }
        }
        public bool Update(ADM_SEMESTER_PAPER_MAPPING model, long modifiedBy, DateTime cDate)
        {
            ADM_SEMESTER_PAPER_MAPPING temp = null;
            try
            {
                temp = semesterPaperMappingRepository.Get(exp => exp.ASPM_CollegeId == model.ASPM_CollegeId && exp.ASPM_Id == model.ASPM_Id && exp.ASPM_IsActive == true);
                if (temp == null) { return false; }
                temp.ASPM_AS_Id = model.ASPM_AS_Id;
                temp.ASPM_AP_Id = model.ASPM_AP_Id;
                temp.ASPM_Status = "U";
                temp.ASPM_ModifiedBy = modifiedBy;
                temp.ASPM_ModifiedDate = DateTime.Now;
                semesterPaperMappingRepository.Update(temp);
                unitOfWork.Commit();
                return true;
            }
            finally
            {
                temp = null;
            }
        }
        public bool Delete(int collegeId, int mappingId, long modifiedBy, DateTime cDate)
        {
            ADM_SEMESTER_PAPER_MAPPING temp = null;
            try
            {
                temp = semesterPaperMappingRepository.Get(exp => exp.ASPM_CollegeId == collegeId && exp.ASPM_Id == mappingId && exp.ASPM_IsActive==true);
                if (temp == null) { return false; }
                temp.ASPM_Status = "U";
                temp.ASPM_IsActive = false;
                temp.ASPM_ModifiedBy = modifiedBy;
                temp.ASPM_ModifiedDate = DateTime.Now;
                temp.ASPM_EndDate = cDate;
                semesterPaperMappingRepository.Update(temp);
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