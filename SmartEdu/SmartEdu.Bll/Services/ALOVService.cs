using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Repositories;

namespace SmartEdu.Bll.Services
{
    public interface IADM_LOVService
    {
        void InsertLOV(ADM_LOV model);
        bool UpdateLOV(ADM_LOV model);
        bool CheckForDuplication(string type,string code);
        bool DeleteLOV(int id);
        object GetGridData(string type);
        ADM_LOV GetLOVModelById(int lovId);
    }

    public class ADM_LOVService : IADM_LOVService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAdmLOVRepository aLOVRepository;
        public ADM_LOVService(IUnitOfWork unitOfWork, IAdmLOVRepository aLOVRepository)
        {
            this.unitOfWork = unitOfWork;
            this.aLOVRepository = aLOVRepository;
        }
        public bool CheckForDuplication(string type, string code)
        {
            ADM_LOV model = null;
            try {
                model = aLOVRepository.Get(exp=> exp.AL_Code==code && exp.AL_Type==type && exp.AL_IsActive==true);
                return (model == null);
            }
            finally { }
        }
        public ADM_LOV GetLOVModelById(int lovId)
        {
            try { return aLOVRepository.Get(exp => exp.AL_Id == lovId && exp.AL_IsActive == true); }
            finally { }
        }
        public object GetGridData(string type)
        {
            IEnumerable<ADM_LOV> LovList;
            try
            {
                if (type == null || type.Trim() == "") { LovList = aLOVRepository.GetMany(exp => exp.AL_IsActive == true); }
                else { LovList = aLOVRepository.GetMany(exp => exp.AL_Type==type && exp.AL_IsActive == true); }
                if (LovList == null) { return new { total_Count = 0, rows = new { } }; }
                return new
                {
                    total_Count = LovList.Count(),
                    rows = (from d in LovList
                            select new
                            {
                                id = d.AL_Id,
                                data = new string[] { d.AL_Id.ToString(), d.AL_Type, d.AL_Code, d.AL_Description }
                            }).ToList()
                };
            }
            finally { LovList = null; }
        }
        public void InsertLOV(ADM_LOV model)
        {
            try
            {
                aLOVRepository.Add(model);
                unitOfWork.Commit();
            }
            finally { }
        }
        public bool UpdateLOV(ADM_LOV model)
        {
            ADM_LOV data = null;
            try
            {
                data = aLOVRepository.Get(exp => exp.AL_Id == model.AL_Id && exp.AL_IsActive == true);
                if (data == null) { return false; }
                data.AL_Code = model.AL_Code;
                data.AL_Type = model.AL_Type;
                data.AL_Description = model.AL_Description;
                aLOVRepository.Update(data);
                unitOfWork.Commit();
                return true;
            }
            finally { data = null; }
        }
        public bool DeleteLOV(int id)
        {
            ADM_LOV data = null;
            try
            {
                data = aLOVRepository.Get(exp => exp.AL_Id == id && exp.AL_IsActive == true);
                if (data == null) { return false; }
                data.AL_IsActive = false;
                data.AL_EndDate = DateTime.Now.Date;
                aLOVRepository.Update(data);
                unitOfWork.Commit();
                return true;
            }
            finally { data = null; }
        }
    }
}