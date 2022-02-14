using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;
using SmartEdu.Data.Repositories;
using SmartEdu.Data.infrastructure;
using System.Text;

namespace SmartEdu.Bll.Services
{
    public interface IPositionsService
    {
        List<ADM_POSITION_LEVEL> GetPositionLevelList(int collegeId, int parentId);
        List<ADM_DEPARTMENTS> GetDepartmentList(int collegeId);
        string[] GetPositionLevelString(int collegeId);
        object GetGridData(int collegeId, long parentId);
        object GetGridDataByLevel(int collegeId, int posLevelId);
        bool CheckForDuplicate(string shortName, string description, int collegeId);
        void Insert(ADM_POSITIONS model);
        bool Update(ADM_POSITIONS model);
        bool Delete(long id, long modifiedBy, DateTime deActivateDate);
        ADM_POSITIONS GetPositionModelById(long id, int collegeId);
    }
    public class PositionsService : IPositionsService
    {
        private readonly IPositionsRepository positionsRepository;
        private readonly IPositionLevelRepository positionLevelRepository;
        private readonly IPositionStaffMappingRepository positionStaffMappingRepository;
        private readonly IDepartmentsRepository departmentsRepository; 
        private readonly IUnitOfWork unitOfWork;
        public PositionsService(IPositionsRepository positionsRepository, IPositionLevelRepository positionLevelRepository, IPositionStaffMappingRepository positionStaffMappingRepository, IDepartmentsRepository departmentsRepository,IUnitOfWork unitOfWork)
        {
            this.positionsRepository = positionsRepository;
            this.positionLevelRepository = positionLevelRepository;
            this.positionStaffMappingRepository = positionStaffMappingRepository;
            this.departmentsRepository = departmentsRepository;
            this.unitOfWork = unitOfWork;
        }
        public List<ADM_DEPARTMENTS> GetDepartmentList(int collegeId)
        {
            try
            {
                return departmentsRepository.GetMany(exp => exp.ADP_CollegeId == collegeId && exp.ADP_IsActive).ToList();
            }
            finally { }
        }
        public List<ADM_POSITION_LEVEL> GetPositionLevelList(int collegeId, int parentId)
        {
            try {
                return positionLevelRepository.GetMany(exp => exp.APL_College_Id == collegeId && exp.APL_Parent_Id == parentId && exp.APL_IsActive).ToList();
            }
            finally { }
        }
        public ADM_POSITIONS GetPositionModelById(long id, int collegeId)
        {
            try {
                return positionsRepository.Get(exp => exp.AP_College_Id == collegeId && exp.AP_ID == id && exp.AP_IsActive == true);
            }
            finally { }
        }
        public string[] GetPositionLevelString(int collegeId) {
            IEnumerable<ADM_POSITION_LEVEL> posLevelList;
            StringBuilder sb = new StringBuilder();
            string[] data=new string[2];
            int i = 0;
            try {
                posLevelList = positionLevelRepository.GetMany(exp => exp.APL_College_Id == collegeId && exp.APL_IsActive == true).ToList();
                if (posLevelList == null) { return null; }
                foreach (ADM_POSITION_LEVEL model in posLevelList) {
                    if (i != 0){sb.Append("<span> - </span>");}
                    if (model.APL_Parent_Id == 0) { data[0] = model.APL_ID.ToString(); }
                    sb.Append("<span onclick='fnLoadGridByLevel(this)' levelId='").Append(model.APL_ID).Append("' class='").Append(i == 0 ? "heighlitter" : "").Append("'>").Append(model.APL_ShortName).Append("</span>");
                    i++;
                }
                data[1] = sb.ToString();
                return data;
            }
            finally { sb = null; posLevelList = null; }
        }
        public object GetGridData(int collegeId, long parentId)
        {
            IEnumerable<ADM_POSITIONS> posList;
            try {
                posList = positionsRepository.GetMany(exp => exp.AP_College_Id == collegeId && exp.AP_Parent_Id == parentId && exp.AP_IsActive == true).ToList();
                if (posList != null) {
                    return new
                    {
                        total_count = posList.Count(),
                        rows = (from p in posList
                                select new
                                {
                                    id = p.AP_ID,
                                    data = new string[]{ p.AP_PositionLevel_Id.ToString(),p.AP_ShortName,p.AP_Description,p.AP_Department_Id.ToString(),p.AP_PositionLevel_Id.ToString()}
                                }).ToList()
                    }; 
                }
                return new { total_count = 0, rows = new { } };
            }
            finally { posList = null; }
        }
        public object GetGridDataByLevel(int collegeId, int posLevelId)
        {
            IEnumerable<ADM_POSITIONS> posList;
            try
            {
                posList = positionsRepository.GetMany(exp => exp.AP_College_Id == collegeId && exp.AP_PositionLevel_Id == posLevelId && exp.AP_IsActive == true).ToList();
                if (posList != null)
                {
                    return new
                    {
                        total_count = posList.Count(),
                        rows = (from p in posList
                                select new
                                {
                                    id = p.AP_ID,
                                    data = new string[] { p.AP_PositionLevel_Id.ToString(), p.AP_ShortName, p.AP_Description }
                                }).ToList()
                    };
                }
                return new { total_count = 0, rows = new { } };
            }
            finally { posList = null; }
        }
        public bool CheckForDuplicate(string shortName, string description, int collegeId)
        {
            ADM_POSITIONS model = null;
            try {
                model = positionsRepository.Get(exp => exp.AP_College_Id == collegeId && exp.AP_ShortName == shortName && exp.AP_Description == description && exp.AP_IsActive == true);
                return (model == null);
            }
            finally { model = null; }
        }
        public void Insert(ADM_POSITIONS model)
        {
            try {
                model.AP_IsActive = true;
                positionsRepository.Add(model);
                unitOfWork.Commit();
                InsertPositionUserMapping(model);
            }
            finally { }
        }
        private void InsertPositionUserMapping(ADM_POSITIONS model)
        {
            ADM_POSITION_STAFF_MAPPING temp = new ADM_POSITION_STAFF_MAPPING();
            try {
                temp.APSM_College_Id = model.AP_College_Id;
                temp.APSM_Department_Id = model.AP_Department_Id;
                temp.APSM_CreatedBy = model.AP_CreatedBy;
                temp.APSM_CreatedDate = DateTime.Now;
                temp.APSM_IsActive = true;
                temp.APSM_Position_Id = model.AP_ID;
                temp.APSM_PositionLevel_Id = model.AP_PositionLevel_Id;
                temp.APSM_StartDate = model.AP_StartDate;
                temp.APSM_Staff_Id = 0;
                positionStaffMappingRepository.Add(temp);
                unitOfWork.Commit();
            }
            finally { temp = null; }
        }
        public bool Update(ADM_POSITIONS model) {
            ADM_POSITIONS temp = null;
            try {
                temp = positionsRepository.Get(exp => exp.AP_ID == model.AP_ID && exp.AP_IsActive == true);
                if (temp == null) {return false;}
                temp.AP_Description = model.AP_Description;
                temp.AP_EndDate = model.AP_EndDate;
                temp.AP_ModifiedBy = model.AP_ModifiedBy;
                temp.AP_ModifiedDate = DateTime.Now;
                temp.AP_ShortName = model.AP_ShortName;
                positionsRepository.Update(temp);
                unitOfWork.Commit();
                return true;
            }
            finally { temp = null; }
        }
        public bool Delete(long id, long modifiedBy,DateTime deActivateDate) {
            ADM_POSITIONS temp = null;
            try
            {
                temp = positionsRepository.Get(exp => exp.AP_ID == id && exp.AP_IsActive == true);
                if (temp == null) { return false; }
                temp.AP_ModifiedBy = modifiedBy;
                temp.AP_ModifiedDate = DateTime.Now;
                temp.AP_EndDate = deActivateDate;
                temp.AP_IsActive = false;
                positionsRepository.Update(temp);
                DeletePositionStaffMapping(id, deActivateDate, modifiedBy);
                unitOfWork.Commit();
                return true;
            }
            finally { temp = null; }
        }

        private void DeletePositionStaffMapping(long positionId, DateTime deActivatedDate, long modifiedBy)
        {
            List<ADM_POSITION_STAFF_MAPPING> posStaffList = null;
            try {
                posStaffList = positionStaffMappingRepository.GetMany(exp => (exp.APSM_EndDate ?? deActivatedDate) >= deActivatedDate && exp.APSM_IsActive==true).ToList();
                foreach (ADM_POSITION_STAFF_MAPPING model in posStaffList) {
                    if (model.APSM_StartDate <= deActivatedDate) {
                        model.APSM_EndDate = deActivatedDate;
                    }
                    else {
                        model.APSM_IsActive = false;
                        model.APSM_EndDate = deActivatedDate;
                    }
                    model.APSM_ModifiedBy = modifiedBy;
                    model.APSM_ModifiedDate = DateTime.Now;
                    positionStaffMappingRepository.Update(model);
                }
            }
            finally { posStaffList = null; }
        }
    }
}