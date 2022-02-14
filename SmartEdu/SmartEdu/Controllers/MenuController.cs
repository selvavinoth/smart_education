using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Bll.Services;
using SmartEdu.Data.Models;
using SmartEdu.ViewModel;
using SmartEdu.Helper;

namespace SmartEdu.Controllers
{
    public class MenuController : BaseController
    {
        //
        // GET: /Menu/
        private int collegeId = 1;
        private int DPId = 1;

        private readonly IMenuService menuService;
        public MenuController(IMenuService menuService) { this.menuService = menuService; }
        public ActionResult Index()
        {
            MenuViewModel viewModel = new MenuViewModel();
            try
            {
                viewModel.RoleList = GetRoleList();
                viewModel.MenuList = (new List<SelectListItem>() { new SelectListItem() { Text = ResourceKeeper.GetResource("CMN_Select"), Value = "0" } }).ToList();
                return View(viewModel);
            }
            catch (Exception exe) { return View(new MenuViewModel()); }
            finally { viewModel = null; }
        }
        private IEnumerable<SelectListItem> GetMenuList(int roleId)
        {
            IEnumerable<SADM_MENU> menuList;
            try {
                menuList = menuService.GetMenuList(collegeId, roleId);
                return (new List<SelectListItem>() { new SelectListItem() { Text = ResourceKeeper.GetResource("CMN_Root"), Value = "0" } }).Union(
                        from m in menuList select new SelectListItem { Text = m.SM_Menu_Name, Value = m.SM_Id.ToString() }).ToList();
            }
            finally { menuList = null; }
        }
        private IEnumerable<SelectListItem> GetTaskList(int roleId)
        {
            IEnumerable<SADM_TASK> taskList;
            try
            {
                taskList = menuService.GetTaskList(roleId);
                return (new List<SelectListItem>() { new SelectListItem()
 { Text = ResourceKeeper.GetResource("CMN_Select"), Value = "0" } }).Union(
                        from m in taskList select new SelectListItem { Text = m.ST_Task_Name, Value = m.ST_Id.ToString() }).ToList();
            }
            finally { taskList = null; }
        }
        private IEnumerable<SelectListItem> GetRoleList()
        {
            IEnumerable<SADM_ROLE> roleList;
            try
            {
                roleList = menuService.GetRoleList(collegeId);
                return (new List<SelectListItem>() { new SelectListItem() { Text = ResourceKeeper.GetResource("CMN_Select"), Value = "0" } }).Union(
                        from m in roleList select new SelectListItem { Text = m.SR_Code, Value = m.SR_Id.ToString() }).ToList();
            }
            finally { roleList = null; }
        }

        [HttpPost]
        public ActionResult GetTask(int roleId)
        {
            try { return Json(new { taskList = GetTaskList(roleId) }, JsonRequestBehavior.AllowGet); }
            catch (Exception exe) { return Json(false, JsonRequestBehavior.AllowGet); }
        }
        [HttpPost]
        public ActionResult GetGridData(int roleId)
        {
            try { return Json(new { Data = menuService.GetMenuGridData(collegeId, roleId), menuList = GetMenuList(roleId) }, JsonRequestBehavior.AllowGet); }
            catch (Exception exe) { return Json(false,JsonRequestBehavior.AllowGet); }
        }
        [HttpPost]
        public ActionResult MenuIUAction(MenuViewModel viewModel)
        {
            SADM_MENU model = new SADM_MENU();
            try
            {
                if (viewModel == null) { return Json(false,JsonRequestBehavior.AllowGet); }
                if (viewModel.SM_Menu_Name == null || viewModel.SM_Menu_Name.Trim() == "") { return Json(new { Msg = ResourceKeeper.GetResource("CMN_PleaseEnter") + " " + ResourceKeeper.GetResource("APP_MenuName"), Status = false }, JsonRequestBehavior.AllowGet); }
                if (!menuService.CheckForDuplication(viewModel.SM_Id, viewModel.SM_Task_Id, viewModel.SM_Role_Id, collegeId, viewModel.SM_Menu_Name)) { return Json(new { Msg = ResourceKeeper.GetResource("APP_MenuName") + " " + ResourceKeeper.GetResource("CMN_Already_Exist"), Status = false }, JsonRequestBehavior.AllowGet); }
                model.SM_Menu_Name = viewModel.SM_Menu_Name;
                model.SM_College_Id = collegeId;
                model.SM_Role_Id = viewModel.SM_Role_Id;
                model.SM_Task_Id = viewModel.SM_Task_Id;
                model.SM_Parent_Id = viewModel.SM_Parent_Id;
                model.SM_IsComponent = viewModel.SM_IsComponent;
                model.SM_Class = viewModel.SM_Class;
                if (viewModel.SM_Id != 0)
                {
                    model.SM_Id = viewModel.SM_Id;
                    if (menuService.Update(model)) { return Json(new { Msg = ResourceKeeper.GetResource("CMN_Updated_Successfully"), data = menuService.GetMenuGridData(collegeId, viewModel.SM_Role_Id), isUpdate = true, Status = true }, JsonRequestBehavior.AllowGet); }
                    else { return Json(new { Msg = ResourceKeeper.GetResource("CMN_Invalid_Access"), data = false, isUpdate = true, Status = false }, JsonRequestBehavior.AllowGet); }
                }
                else { menuService.Insert(model); return Json(new { Msg = ResourceKeeper.GetResource("CMN_Added_Successfully"), data = menuService.GetMenuGridData(collegeId, viewModel.SM_Role_Id), isUpdate = false, Status = true }, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception exe) { return Json(false,JsonRequestBehavior.AllowGet); }
            finally { model = null; }
        }
        [HttpPost]
        public ActionResult Delete(int id,int roleId)
        {
            try
            {
                if (id > 0) { if (menuService.Delete(id)) { return Json(new { Msg = ResourceKeeper.GetResource("CMN_Deleted_Successfully"), data = menuService.GetMenuGridData(collegeId, roleId) }, JsonRequestBehavior.AllowGet); } }
                return Json(new { Msg = ResourceKeeper.GetResource("CMN_Invalid_Access"), data = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false,JsonRequestBehavior.AllowGet); }
        }
    }
}
