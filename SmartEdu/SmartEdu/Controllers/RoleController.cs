using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEdu.Bll.Services;
using SmartEdu.ViewModel;
using SmartEdu.Data.Models;
using SmartEdu.Helper;

namespace SmartEdu.Controllers
{
    public class RoleController : Controller
    {
        //
        // GET: /Role/
        private readonly IRoleService roleService;
        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }
        public ActionResult Index()
        {
            return View(new RoleViewModel());
        }

        [HttpPost]
        public ActionResult GetGridData()
        {
            try { return Json(roleService.GetRoleGridData(), JsonRequestBehavior.AllowGet); }
            catch (Exception exe) { return Json(false); }
        }
        [HttpPost]
        public ActionResult RoleIUAction(RoleViewModel viewModel)
        {
            SADM_ROLE model = new SADM_ROLE();
            try
            {
                if (viewModel == null) { return Json(false); }
                if (viewModel.SR_Code == null || viewModel.SR_Code.Trim() == "") { return Json(new { Msg = ResourceKeeper.GetResource("CMN_PleaseEnter") + " " + ResourceKeeper.GetResource("APP_Code"), Status = false }, JsonRequestBehavior.AllowGet); }
                if (viewModel.SR_Description == null || viewModel.SR_Description.Trim() == "") { return Json(new { Msg = ResourceKeeper.GetResource("CMN_PleaseEnter") + " " + ResourceKeeper.GetResource("APP_Description"), Status = false }, JsonRequestBehavior.AllowGet); }
                if (!roleService.CheckForDublication(viewModel.SR_Id, viewModel.SR_Code)) { return Json(new { Msg = ResourceKeeper.GetResource("APP_Code") + " " + ResourceKeeper.GetResource("CMN_Already_Exist"), Status = false }, JsonRequestBehavior.AllowGet); }
                model.SR_Code = viewModel.SR_Code;
                model.SR_Description = viewModel.SR_Description;
                if (viewModel.SR_Id != 0)
                {
                    model.SR_Id = viewModel.SR_Id;
                    if (roleService.Update(model)) { return Json(new { Msg = ResourceKeeper.GetResource("CMN_Updated_Successfully"), data = roleService.GetRoleGridData(), isUpdate = true, Status = true }, JsonRequestBehavior.AllowGet); }
                    else { return Json(new { Msg = ResourceKeeper.GetResource("CMN_Invalid_Update"), data = false, isUpdate = true, Status = false }, JsonRequestBehavior.AllowGet); }
                }
                else { roleService.Insert(model); return Json(new { Msg = ResourceKeeper.GetResource("CMN_Added_Successfully"), data = roleService.GetRoleGridData(), isUpdate = false, Status = true }, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception exe) { return Json(false); }
            finally { model = null; }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                if (id > 0) { if (roleService.Delete(id)) { return Json(new { Msg = ResourceKeeper.GetResource("CMN_Deleted_Successfully"), data = roleService.GetRoleGridData() }, JsonRequestBehavior.AllowGet); } }
                return Json(new { Msg = ResourceKeeper.GetResource("CMN_Invalid_Access"), data = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
        }
    }
}
