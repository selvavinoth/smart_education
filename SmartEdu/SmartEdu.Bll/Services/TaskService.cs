using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Repositories;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Models;

namespace SmartEdu.Bll.Services
{
    public interface ITaskService {
        object GetTaskGridData();
        bool CheckForDublication(int id, string taskName);
        void Insert(SADM_TASK model);
        bool Update(SADM_TASK model);
        bool Delete(int id);
    }
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository taskRepository;
        private readonly IUnitOfWork unitOfWork;
        public TaskService(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
        {
            this.taskRepository = taskRepository;
            this.unitOfWork = unitOfWork;
        }

        public object GetTaskGridData()
        {
            IEnumerable<SADM_TASK> taskList;
            try
            {
                taskList = taskRepository.GetMany(exp => exp.ST_IsActive == true).ToList();
                if (taskList == null) { return null; }
                return new {
                    total_count = taskList.Count(),
                    rows = (from h in taskList
                            select new {
                                id = h.ST_Id,
                                data = new string[] { h.ST_Task_Name, h.ST_URL }
                            }).ToList()
                };
            }
            finally { taskList = null; }
        }
        public bool CheckForDublication(int id,string taskName)
        {
            SADM_TASK model = null;
            try
            {
                if (id == 0) { model = taskRepository.Get(exp => exp.ST_Task_Name == taskName && exp.ST_IsActive == true); }
                else { model = taskRepository.Get(exp => exp.ST_Task_Name == taskName && exp.ST_Id != id && exp.ST_IsActive == true); }
                return (model == null);
            }
            finally { model = null; }
        }

        public void Insert(SADM_TASK model)
        {
            try
            {
                model.ST_IsActive = true;
                model.ST_Status = "I";
                taskRepository.Add(model);
                unitOfWork.Commit();
            }
            finally { }
        }
        public bool Update(SADM_TASK model)
        {
            SADM_TASK temp = new SADM_TASK();
            try
            {
                temp = taskRepository.Get(exp => exp.ST_Id == model.ST_Id && exp.ST_IsActive == true);
                if (temp != null)
                {
                    temp.ST_Task_Name = model.ST_Task_Name;
                    temp.ST_URL = model.ST_URL;
                    temp.ST_Status = "U";
                    taskRepository.Update(temp);
                    unitOfWork.Commit();
                    return true;
                }
                return false;
            }
            finally { temp = null; }
        }
        public bool Delete(int id)
        {
            SADM_TASK temp = new SADM_TASK();
            try
            {
                temp = taskRepository.Get(exp => exp.ST_Id == id && exp.ST_IsActive == true);
                if (temp != null)
                {
                    temp.ST_IsActive = false;
                    temp.ST_Status = "U";
                    taskRepository.Update(temp);
                    unitOfWork.Commit();
                    return true;
                }
                return false;
            }
            finally { temp = null; }
        }
    }
}