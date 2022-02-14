using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Repositories;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Models;

namespace SmartEdu.Bll.Services
{
    public interface IMenuService {
        IEnumerable<SADM_ROLE> GetRoleList(int collegeId);
        IEnumerable<SADM_TASK> GetTaskList(int roleId);
        IEnumerable<SADM_MENU> GetMenuList(int collegeId, int roleId);
        object GetMenuGridData(int collageId, int positionLevelId);
        bool CheckForDuplication(int menuId, int taskId, int roleId, int collegeId, string menuName);
        void Insert(SADM_MENU model);
        bool Update(SADM_MENU model);
        bool Delete(int id);
    }
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository menuRepository;
        private readonly ITaskRepository taskRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IRoleTaskMappingRepository roleTaskMappingRepository;
        private readonly IUnitOfWork unitOfWork;
        public MenuService(IMenuRepository menuRepository, ITaskRepository taskRepository, IRoleRepository roleRepository,IRoleTaskMappingRepository roleTaskMappingRepository, IUnitOfWork unitOfWork)
        {
            this.menuRepository = menuRepository;
            this.taskRepository = taskRepository;
            this.roleRepository = roleRepository;
            this.roleTaskMappingRepository = roleTaskMappingRepository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<SADM_ROLE> GetRoleList(int collegeId)
        {
            try { return roleRepository.GetMany(exp => exp.SR_IsActive == true).ToList(); }
            finally { }
        }
        public IEnumerable<SADM_TASK> GetTaskList(int roleId)
        {
            List<int> taskList;
            try {
                taskList = roleTaskMappingRepository.GetMany(exp => exp.SRTM_Role_Id == roleId && exp.SRTM_IsActive == true).Select(exp=>exp.SRTM_Task_Id).ToList();
                return taskRepository.GetMany(exp =>taskList.Contains(exp.ST_Id) && exp.ST_IsActive == true).ToList(); 
            }
            finally { taskList = null; }
        }
        public IEnumerable<SADM_MENU> GetMenuList(int collegeId, int roleId)
        {
            try { return menuRepository.GetMany(exp => exp.SM_College_Id == collegeId && exp.SM_Role_Id == roleId && exp.SM_IsActive == true); }
            finally { }
        }

        public object GetMenuGridData(int collegeId,int roleId)
        {
            IEnumerable<SADM_TASK> taskList;
            IEnumerable<SADM_MENU> menuList;
            IEnumerable<SADM_ROLE> roleList;
            try {
                taskList = taskRepository.GetMany(exp => exp.ST_IsActive == true).ToList();
                menuList = menuRepository.GetMany(exp => exp.SM_College_Id == collegeId && exp.SM_Role_Id == roleId && exp.SM_IsActive == true).ToList();
                roleList = GetRoleList(collegeId);
                return new {
                    total_count = menuList.ToList(),
                    rows=(from m in menuList 
                           join t in taskList on m.SM_Task_Id equals t.ST_Id into tp
                           from t in tp.DefaultIfEmpty()
                          join p in roleList on m.SM_Role_Id equals p.SR_Id
                           join mm in menuList on m.SM_Parent_Id equals mm.SM_Id into menuParentList
                           from mp in menuParentList.DefaultIfEmpty()
                           select new 
                           {
                               id=m.SM_Id,
                               data=new string[]{ m.SM_Menu_Name,(t!=null?t.ST_Task_Name:""),(t!=null? t.ST_URL:""),(mp!=null?mp.SM_Menu_Name:""),p.SR_Id.ToString(),(t!=null?t.ST_Id.ToString():"0"),m.SM_Parent_Id.ToString(),(m.SM_Class??""),(m.SM_IsComponent?"true":"false") }
                           }).ToList()
                };
            }
            finally { taskList = null; menuList = null; roleList = null; }
        }
        public bool CheckForDuplication(int menuId, int taskId, int roleId, int collegeId, string menuName)
        {
            SADM_MENU model=null;
            try {
                if (menuId == 0) { model = menuRepository.Get(exp => exp.SM_Menu_Name == menuName && exp.SM_Task_Id == taskId && exp.SM_Role_Id == roleId && exp.SM_College_Id == collegeId && exp.SM_IsActive == true); }
                else { model = menuRepository.Get(exp => exp.SM_Menu_Name == menuName && exp.SM_Task_Id == taskId && exp.SM_Role_Id == roleId && exp.SM_College_Id == collegeId && exp.SM_IsActive == true && exp.SM_Id != menuId); }
                return (model == null);
            }
            finally { model = null; }
        }
        public void Insert(SADM_MENU model)
        {
            try
            {
                model.SM_IsActive = true;
                model.SM_Status = "I";
                menuRepository.Add(model);
                unitOfWork.Commit();
            }
            finally { }
        }
        public bool Update(SADM_MENU model)
        {
            SADM_MENU temp = null;
            try
            {
                temp = menuRepository.Get(exp => exp.SM_Id == model.SM_Id && exp.SM_IsActive == true);
                if (temp != null)
                {
                    temp.SM_IsComponent = model.SM_IsComponent;
                    temp.SM_College_Id = model.SM_College_Id;
                    temp.SM_Menu_Name = model.SM_Menu_Name;
                    temp.SM_Parent_Id = model.SM_Parent_Id;
                    temp.SM_Role_Id = model.SM_Role_Id;
                    temp.SM_Status = "U";
                    temp.SM_Task_Id = model.SM_Task_Id;
                    temp.SM_Class = model.SM_Class;
                    menuRepository.Update(temp);
                    unitOfWork.Commit();
                    return true;
                }
                return false;
            }
            finally { temp = null; }
        }
        public bool Delete(int id)
        {
            SADM_MENU temp = null;
            try
            {
                temp = menuRepository.Get(exp => exp.SM_Id == id && exp.SM_IsActive == true);
                if (temp != null)
                {
                    temp.SM_IsActive = false;
                    temp.SM_Status = "U";
                    menuRepository.Update(temp);
                    unitOfWork.Commit();
                    return true;
                }
                return false;
            }
            finally { temp = null; }
        }
    }
}