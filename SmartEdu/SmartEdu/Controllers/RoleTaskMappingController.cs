using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEdu.ViewModel;
using SmartEdu.Bll.Services;
using SmartEdu.Helper;
using SmartEdu.Data.Models;

namespace SmartEdu.Controllers
{
    public class RoleTaskMappingController : Controller
    {
        //
        // GET: /RoleTaskMapping/
        private readonly IRoleTaskMappingService roleTaskMappingService;
        public RoleTaskMappingController(IRoleTaskMappingService roleTaskMappingService) { this.roleTaskMappingService = roleTaskMappingService; }
        public ActionResult Index()
        {
            RoleTaskMappingViewModel viewModel = new RoleTaskMappingViewModel();
            try {
                viewModel.RoleList = GetRoleList();
                return View(viewModel);
            }
            catch (Exception exe) { return View(new RoleTaskMappingViewModel()); }
            finally { viewModel = null; }
        }
        private IEnumerable<SelectListItem> GetRoleList()
        {
            IEnumerable<SADM_ROLE> roleList;
            try
            {
                roleList = roleTaskMappingService.GetRoleList();
                return (new List<SelectListItem>() { new SelectListItem() { Text = ResourceKeeper.GetResource("CMN_Select"), Value = "0" } }).Union(
                        from m in roleList select new SelectListItem { Text = m.SR_Code, Value = m.SR_Id.ToString() }).ToList();
            }
            finally { roleList = null; }
        }
        [HttpPost]
        public ActionResult GetGridData(int? roleId)
        {
            try { return Json(roleTaskMappingService.GetRoleTaskMappingGridData(roleId.Value), JsonRequestBehavior.AllowGet); }
            catch (Exception exe) { return Json(false); }
        }
        [HttpPost]
        public ActionResult RoleTaskMappingIUAction(RoleTaskMappingViewModel viewModel)
        {
            try
            {
                if (viewModel == null) { return Json(false); }
                if (viewModel.SRTM_Role_Id <= 0) { return Json(new { Msg = ResourceKeeper.GetResource("CMN_PleaseSelect") + " " + ResourceKeeper.GetResource("APP_Role"), Status = false }, JsonRequestBehavior.AllowGet); }
                if (viewModel.TaskIdList.Count == 0) { return Json(new { Msg = ResourceKeeper.GetResource("CMN_PleaseSelect") + " " + ResourceKeeper.GetResource("APP_Task"), Status = false }, JsonRequestBehavior.AllowGet); }
                roleTaskMappingService.InsertUpdateRoleTask(viewModel.TaskIdList, viewModel.SRTM_Role_Id);
                return Json(new { Msg = ResourceKeeper.GetResource("CMN_Added_Successfully"), data = roleTaskMappingService.GetRoleTaskMappingGridData(viewModel.SRTM_Role_Id), isUpdate = false, Status = true }, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception exe) { return Json(false); }
        }
        [HttpPost]
        public ActionResult Delete(int id,int roleId)
        {
            try
            {
                if (id > 0) { if (roleTaskMappingService.Delete(id)) { return Json(new { Msg = ResourceKeeper.GetResource("CMN_Deleted_Successfully"), data = roleTaskMappingService.GetRoleTaskMappingGridData(roleId) }, JsonRequestBehavior.AllowGet); } }
                return Json(new { Msg = ResourceKeeper.GetResource("CMN_Invalid_Access"), data = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
        }
    }
}
