using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;
using SmartEdu.Data.Repositories;
using SmartEdu.Data.infrastructure;

namespace SmartEdu.Bll.Services
{
    public interface IRoleTaskMappingService
    {
        object GetRoleTaskMappingGridData(int roleId);
        List<SADM_ROLE> GetRoleList();
        void InsertUpdateRoleTask(List<int> taskIdList,int role);
        bool Delete(int id);
    }
    public class RoleTaskMappingService : IRoleTaskMappingService
    {
        private readonly ITaskRepository taskRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IRoleTaskMappingRepository roleTaskMappingRepository;
        private readonly IUnitOfWork unitOfWork;
        public RoleTaskMappingService(IRoleTaskMappingRepository roleTaskMappingRepository,ITaskRepository taskRepository,IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            this.roleTaskMappingRepository = roleTaskMappingRepository;
            this.taskRepository = taskRepository;
            this.roleRepository = roleRepository;
            this.unitOfWork = unitOfWork;
        }
        public List<SADM_ROLE> GetRoleList() {
            try {
                return roleRepository.GetMany(exp => exp.SR_IsActive == true).ToList();
            }
            finally { }
        }
        private List<SADM_TASK> GetTaskList() {
            try { return taskRepository.GetMany(exp => exp.ST_IsActive == true).ToList(); }
            finally { }
        }
        private List<SADM_ROLE_TASK_MAPPING> mappingList;
        private List<SADM_ROLE> roleList;
        private List<SADM_TASK> taskList;
        public object GetRoleTaskMappingGridData(int roleId)
        {
            try
            {
                taskList = GetTaskList();
                mappingList = roleTaskMappingRepository.GetMany(exp =>exp.SRTM_Role_Id==roleId && exp.SRTM_IsActive==true).ToList();
                if (mappingList.Count == 0) { return new { RTMGridData = new { total_count = 0, rows = new { }}, TaskGridData = GetTaskGridData() }; }
                roleList = GetRoleList();
                return new { RTMGridData = GetRTMData(), TaskGridData = GetTaskGridData() };
            }
            finally { mappingList = null; taskList = null; roleList = null; }
        }
        private object GetRTMData()
        {
            try {
                return new
                {
                    total_count = mappingList.Count(),
                    rows = (from rtm in mappingList
                            join t in taskList on rtm.SRTM_Task_Id equals t.ST_Id
                            join r in roleList on rtm.SRTM_Role_Id equals r.SR_Id
                            select new
                            {
                                id = rtm.SRTM_Id,
                                data = new string[] { r.SR_Code, t.ST_Task_Name, rtm.SRTM_Role_Id.ToString(), rtm.SRTM_Task_Id.ToString() }
                            }).ToList()

                };
            }
            finally { }
        }
        private object GetTaskGridData()
        {
            try
            {
                return new
                {
                    total_count = taskList.Count(),
                    rows = (from t in taskList
                            join m in mappingList on t.ST_Id equals m.SRTM_Task_Id into sdata
                            from s in sdata.DefaultIfEmpty()
                            select new
                            {
                                id = t.ST_Id,
                                data = new string[] { t.ST_Task_Name, t.ST_URL, (s==null?"false":"true") }
                            }).ToList()
                };
            }
            finally {}
        }
        public void InsertUpdateRoleTask(List<int> taskIdList, int roleId) {
            List<SADM_ROLE_TASK_MAPPING> mappingList;
            List<SADM_ROLE_TASK_MAPPING> deleteList;
            SADM_ROLE_TASK_MAPPING model = null;
            try {
                mappingList = roleTaskMappingRepository.GetMany(exp => exp.SRTM_Role_Id==roleId && exp.SRTM_IsActive == true).ToList();
                deleteList = mappingList.FindAll(exp => !taskIdList.Contains(exp.SRTM_Task_Id));
                foreach (SADM_ROLE_TASK_MAPPING m in deleteList) { DeleteMapping(m); }
                foreach (int m in taskIdList) {
                    model = mappingList.Find(exp => exp.SRTM_Task_Id == m);
                    if (model == null) { Insert(m, roleId); }
                }
                unitOfWork.Commit();
            }
            finally { model = null; mappingList = null; deleteList = null; }
        }
        private void Insert(int taskId, int roleId)
        {
            SADM_ROLE_TASK_MAPPING model = new SADM_ROLE_TASK_MAPPING();
            try
            {
                model.SRTM_Task_Id = taskId;
                model.SRTM_Role_Id = roleId;
                model.SRTM_IsActive = true;
                model.SRTM_Status = "I";
                roleTaskMappingRepository.Add(model);
            }
            finally { model = null; }
        }
        private void DeleteMapping(SADM_ROLE_TASK_MAPPING temp)
        {
            try
            {
                temp.SRTM_IsActive = false;
                temp.SRTM_Status = "U";
                roleTaskMappingRepository.Update(temp);
            }
            finally { }
        }
        public bool Delete(int id)
        {
            SADM_ROLE_TASK_MAPPING temp = new SADM_ROLE_TASK_MAPPING();
            try
            {
                temp = roleTaskMappingRepository.Get(exp => exp.SRTM_Id == id && exp.SRTM_IsActive == true);
                if (temp == null) { return false; }
                DeleteMapping(temp);
                unitOfWork.Commit();
                return true;
            }
            finally { temp = null; }
        }
    }
}