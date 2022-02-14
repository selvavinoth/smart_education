using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;
using SmartEdu.Data.Repositories;

namespace SmartEdu.Bll.Services
{
    public interface IWelcomeService
    {
        IEnumerable<CommonMenuModel> GetMenuList(int collegeId, int positionLevelId);
    }
    public class WelcomeService : IWelcomeService
    {
        private readonly IMenuRepository menuRepository;
        private readonly ITaskRepository taskRepository;
        public WelcomeService(IMenuRepository menuRepository, ITaskRepository taskRepository)
        {
            this.menuRepository = menuRepository;
            this.taskRepository = taskRepository;
        }
        public IEnumerable<CommonMenuModel> GetMenuList(int collegeId, int roleId)
        {
            IEnumerable<SADM_MENU> menuList;
            IEnumerable<SADM_TASK> taskList;
            try {
                menuList = menuRepository.GetMany(exp => exp.SM_College_Id == collegeId && exp.SM_Role_Id == roleId && exp.SM_IsActive == true);
                taskList = taskRepository.GetMany(exp => exp.ST_IsActive == true);
                return (from m in menuList
                        join t in taskList on m.SM_Task_Id equals t.ST_Id into task
                        from t in task.DefaultIfEmpty()
                        select new CommonMenuModel
                        {
                            SM_MenuId=m.SM_Id,
                            SM_Menu_Name=m.SM_Menu_Name,
                            SM_Task_Name=(t!=null?t.ST_Task_Name:""),
                            SM_IsComponent=m.SM_IsComponent,
                            SM_Parent_Id=m.SM_Parent_Id,
                            SM_URL=(t!=null?t.ST_URL:""),
                            SM_Class=(m.SM_Class??"")
                        }).ToList();

            }
            finally { }
        }
    }
}