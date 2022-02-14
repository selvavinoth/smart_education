using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEdu.Data.Models;
using SmartEdu.ViewModel;
using SmartEdu.Bll.Services;

namespace SmartEdu.Controllers
{
    public class PositionLevelController : Controller
    {
        //
        // GET: /PositionLevel/
        private int collegeId = 1;
        private readonly IPositionLevelService positionLevelService;
        public PositionLevelController(IPositionLevelService positionLevelService) {
            this.positionLevelService = positionLevelService;
        }

        public ActionResult Index()
        {
            PositionLevelViewModel v = new PositionLevelViewModel();
            try {
                v.RoleList = GetRoleList();
                return View(v);
            }
            catch (Exception ex) { return View(v); }
            finally { v = null; }
            
        }
        [HttpPost]
        public ActionResult GetGridData(int parentId) {
            try {
                return Json(positionLevelService.GetGridDataByLevel(collegeId, parentId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { }
        }
        private IEnumerable<SelectListItem> GetRoleList() {
            List<SADM_ROLE> roleList = null;
            try {
                roleList = positionLevelService.GetRoleList();
                return (new List<SelectListItem>(){ new SelectListItem(){Text="Select",Value="0"}}).Union((from r in roleList
                        select new SelectListItem { 
                            Text=r.SR_Description,
                            Value=r.SR_Id.ToString()
                        })).ToList();
            }
            finally { roleList = null; }
        }
        [HttpPost]
        public ActionResult PositionLevelIUAction(PositionLevelViewModel viewModel)
        {
            ADM_POSITION_LEVEL model = new ADM_POSITION_LEVEL();
            try {
                if (viewModel == null) { return Json(false); }
                if (viewModel.APL_ShortName == null || viewModel.APL_ShortName.Trim() == "") { return Json(new { Msg = "Please enter the Short Name", Status = false }, JsonRequestBehavior.AllowGet); }
                if (viewModel.APL_Description == null || viewModel.APL_Description.Trim() == "") { return Json(new { Msg = "Please enter the description", Status = false }, JsonRequestBehavior.AllowGet); }
                if (!positionLevelService.CheckForDuplication(collegeId, viewModel.APL_ShortName, viewModel.APL_Description)) { return Json(new { Msg = "Short Name already exist.", Status = false }, JsonRequestBehavior.AllowGet); }
                model.APL_ShortName = viewModel.APL_ShortName;
                model.APL_Description = viewModel.APL_Description;
                model.APL_Parent_Id = viewModel.APL_Parent_Id;
                model.APL_College_Id = collegeId;
                model.APL_IsBaseLevel = viewModel.APL_IsBaseLevel;
                model.APL_BaseLevel_Id = viewModel.APL_BaseLevel_Id;
                model.APL_Role_Id = viewModel.APL_Role_Id;
                if (viewModel.APL_ID != 0)
                {
                    model.APL_ID = viewModel.APL_ID;
                    if (positionLevelService.Update(model)) { return Json(new { Msg = "UpDated Successfully....", data = positionLevelService.GetGridDataByLevel(collegeId, viewModel.APL_Parent_Id), isUpdate = true, Status = true }, JsonRequestBehavior.AllowGet); }
                    else { return Json(new { Msg = "Invalid Updation", data = false, isUpdate = true, Status = false }, JsonRequestBehavior.AllowGet); }
                }
                else { positionLevelService.Insert(model); return Json(new { Msg = "Inserted Successfully....", data = positionLevelService.GetGridDataByLevel(collegeId, viewModel.APL_Parent_Id), isUpdate = false, Status = true }, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception exe) { return Json(false); }
            finally { model = null; }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            ADM_POSITION_LEVEL model = new ADM_POSITION_LEVEL();
            try
            {
                if (id > 0)
                {
                    model = positionLevelService.GetPositionLevelModel(id);
                    if (model != null)
                    {
                        if (positionLevelService.Delete(model)) { return Json(new { Msg = "Deleted Successfully....", data = positionLevelService.GetGridDataByLevel(collegeId, model.APL_Parent_Id) }, JsonRequestBehavior.AllowGet); }
                    }
                }
                return Json(new { Msg = "Invalid Deletion", data = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { model = null; }
        }
    }
}
