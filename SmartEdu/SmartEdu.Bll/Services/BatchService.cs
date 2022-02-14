using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Repositories;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Models;

namespace SmartEdu.Bll.Services
{
    public interface IBatchService
    {
        IEnumerable<ADM_LOV> GetLovList(int collegeId, List<string> typeList);
        object GetBatchGridData(int collegeId, List<string> typeList, DateTime cDate);
        bool CheckForDuplicate(ADM_BATCH model, DateTime cDate);
        void InsertBatch(ADM_BATCH model, long modifiedBy);
        bool UpdateBatch(ADM_BATCH model, long modifiedBy, DateTime cDate);
        bool DeleteBatch(int collegeId, int classId, long modifiedBy, DateTime cDate);
    }
    public class BatchService : IBatchService
    {
        private readonly IBatchRepository batchRepository;
        private readonly IAdmLOVRepository aLOVRepository;
        private readonly IUnitOfWork unitOfWork;
        public BatchService(IBatchRepository batchRepository,IAdmLOVRepository aLOVRepository, IUnitOfWork unitOfWork)
        {
            this.batchRepository = batchRepository;
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
        public object GetBatchGridData(int collegeId, List<string> typeList, DateTime cDate)
        {
            IEnumerable<ADM_BATCH> batchList;
            IEnumerable<ADM_LOV> lovList;
            try
            {
                batchList = batchRepository.GetMany(exp => exp.AB_CollegeId == collegeId && (exp.AB_EndDate ?? cDate) >= cDate).ToList();
                if (batchList == null || batchList.Count() == 0) { return new { total_count = 0, rows = new { }}; }
                lovList = GetLovList(collegeId, typeList);
                if (lovList == null || lovList.Count() == 0) { return new { total_count = 0, rows = new { }}; }
                return (new
                {
                    total_cout = batchList.Count(),
                    rows = (from c in batchList
                            join d in lovList on c.AB_Graduate equals d.AL_Id
                            join dg in lovList on c.AB_Degree equals dg.AL_Id
                            join dp in lovList on c.AB_Department equals dp.AL_Id
                            select new
                            {
                                id = c.AB_Id,
                                data = new string[] { d.AL_Id.ToString(), dg.AL_Id.ToString(), dp.AL_Id.ToString(), d.AL_Code, dg.AL_Code, dp.AL_Code,c.AB_Batch, (c.AB_StartDate.ToString("dd/MM/yyyy")), (c.AB_EndDate.HasValue ? c.AB_EndDate.Value.ToString("dd/MM/yyyy") : ""),c.AB_Year.Value.ToString() }
                            }).ToList()
                });
            }
            finally { batchList = null; lovList = null; }
        }
        public bool CheckForDuplicate(ADM_BATCH model, DateTime cDate)
        {
            ADM_BATCH temp = null;
            try
            {
                if (model.AB_Id == 0) { temp = batchRepository.Get(exp => exp.AB_CollegeId == model.AB_CollegeId && exp.AB_Graduate == model.AB_Graduate && exp.AB_Degree == model.AB_Degree && exp.AB_Department == model.AB_Department && exp.AB_Year == model.AB_Year && exp.AB_Batch == model.AB_Batch && (exp.AB_EndDate ?? cDate) >= cDate); }
                else { temp = batchRepository.Get(exp => exp.AB_CollegeId == model.AB_CollegeId && exp.AB_Graduate == model.AB_Graduate && exp.AB_Degree == model.AB_Degree && exp.AB_Department == model.AB_Department && exp.AB_Year == model.AB_Year && exp.AB_Batch == model.AB_Batch && (exp.AB_EndDate ?? cDate) >= cDate && exp.AB_Id != model.AB_Id); }
                return (temp == null);
            }
            finally { temp = null; }
        }
        public void InsertBatch(ADM_BATCH model, long modifiedBy)
        {
            try
            {
                model.AB_CreatedBy = modifiedBy;
                model.AB_CreatedDate = DateTime.Now;
                model.AB_IsActive = true;
                model.AB_Status = "I";
                model.AB_StartDate = DateTime.Now.Date;
                batchRepository.Add(model);
                unitOfWork.Commit();
            }
            finally
            {
            }
        }
        public bool UpdateBatch(ADM_BATCH model, long modifiedBy, DateTime cDate)
        {
            ADM_BATCH temp = null;
            try
            {
                temp = batchRepository.Get(exp => exp.AB_CollegeId == model.AB_CollegeId && exp.AB_Id == model.AB_Id && (exp.AB_EndDate ?? cDate) >= cDate);
                if (temp == null) { return false; }
                temp.AB_Graduate = model.AB_Graduate;
                temp.AB_Year = model.AB_Year;
                temp.AB_Batch = model.AB_Batch;
                temp.AB_Degree = model.AB_Degree;
                temp.AB_Department = model.AB_Department;
                temp.AB_Status = "U";
                temp.AB_ModifiedBy = modifiedBy;
                temp.AB_ModifiedDate = DateTime.Now;
                batchRepository.Update(temp);
                unitOfWork.Commit();
                return true;
            }
            finally
            {
                temp = null;
            }
        }
        public bool DeleteBatch(int collegeId, int batchId, long modifiedBy, DateTime cDate)
        {
            ADM_BATCH temp = null;
            try
            {
                temp = batchRepository.Get(exp => exp.AB_CollegeId == collegeId && exp.AB_Id == batchId && (exp.AB_EndDate ?? cDate) >= cDate);
                if (temp == null) { return false; }
                temp.AB_Status = "U";
                temp.AB_IsActive = false;
                temp.AB_ModifiedBy = modifiedBy;
                temp.AB_ModifiedDate = DateTime.Now;
                temp.AB_EndDate = cDate;
                batchRepository.Update(temp);
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