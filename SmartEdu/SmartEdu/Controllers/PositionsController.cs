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
    public class PositionsController : Controller
    {
        //
        // GET: /Positions/
        private int collegeId = 1;
        private int DepartmentId = 1;
        private long createdBy = 0;
        private readonly IPositionsService positionsService;
        public PositionsController(IPositionsService positionsService) {
            this.positionsService = positionsService;
        }
        public ActionResult Index()
        {
            PositionsViewModel v = new PositionsViewModel();
            try {
                v.PositionLevelList = GetPosLevelList(collegeId, 0);
                v.DepartmentList = GetDepartmentList(collegeId,0);
                return View(v);
            }
            catch (Exception e) { return View(v); }
            finally { v = null; }
            
        }
        private IEnumerable<SelectListItem> GetPosLevelList(int collegeId, int parentId) {
            List<ADM_POSITION_LEVEL> posLevelList = null;
            try {
                posLevelList = positionsService.GetPositionLevelList(collegeId, parentId);
                return (from p in posLevelList
                        select new SelectListItem { 
                            Text= p.APL_Description,
                            Value=p.APL_ID.ToString()
                        }).ToList();
            }
            finally { posLevelList=null;}
        }
        private IEnumerable<SelectListItem> GetDepartmentList(int collegeId, int departmentId)
        {
            List<ADM_DEPARTMENTS> departmentList = null;
            try
            {
                departmentList = positionsService.GetDepartmentList(collegeId);
                if (departmentId != 0) { 
                    departmentList = departmentList.FindAll(exp => exp.ADP_Id == departmentId);
                    return (from d in departmentList
                            select new SelectListItem
                            {
                                Text = d.ADP_ShortDescription,
                                Value = d.ADP_Id.ToString()
                            }).ToList();
                }
                return (new List<SelectListItem>(){new SelectListItem(){ Value="0",Text="-All-"}}).Union((from d in departmentList
                        select new SelectListItem
                        {
                            Text = d.ADP_ShortDescription,
                            Value = d.ADP_Id.ToString()
                        })).ToList();
            }
            finally { departmentList = null; }
        }

        [HttpPost]
        public ActionResult GetInitGridData(long parentId)
        {
            try
            {
                if (parentId >= 0) { return Json(new {PosLevlString=positionsService.GetPositionLevelString(collegeId) , Data = positionsService.GetGridData(collegeId, parentId), Msg = "", Status = true }, JsonRequestBehavior.AllowGet); }
                return Json(new { Status = false, Msg = "Invalid Access." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(new { Status = false, Msg = "Some Problem in Application." }, JsonRequestBehavior.AllowGet); }
            finally { }
        }
        [HttpPost]
        public ActionResult GetGridData(long parentId) {
            try {
                if (parentId >= 0) { return Json(new { Data = positionsService.GetGridData(collegeId, parentId), Msg = "", Status = true, DropDownData = FillDropDownList(parentId) }, JsonRequestBehavior.AllowGet); }
                return Json(new { Status = false, Msg = "Invalid Access." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(new { Status = false, Msg = "Some Problem in Application." }, JsonRequestBehavior.AllowGet); }
            finally { }
        }
        private object FillDropDownList(long posId) {
            ADM_POSITIONS model = null;
            try {
                if (posId == 0) { return new { DpList=GetDepartmentList(collegeId,0),PSLList=GetPosLevelList(collegeId,0)}; }
                else {
                    model = positionsService.GetPositionModelById(posId, collegeId);
                    if (model != null) { return new { DpList = GetDepartmentList(collegeId, model.AP_Department_Id), PSLList = GetPosLevelList(collegeId, model.AP_PositionLevel_Id) }; }
                    else { return new { }; }
                }
            }
            finally { model = null; }
        }
        [HttpPost]
        public ActionResult GetGridDataByLevelId(int posLevelId)
        {
            try
            {
                if (posLevelId > 0) { return Json(new { Data = positionsService.GetGridDataByLevel(collegeId, posLevelId), Msg = "", Status = true }, JsonRequestBehavior.AllowGet); }
                return Json(new { Status = false, Msg = "Invalid Access." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(new { Status = false, Msg = "Some Problem in Application." }, JsonRequestBehavior.AllowGet); }
            finally { }
        }
        [HttpPost]
        public ActionResult PositionIUAction(PositionsViewModel viewModel) {
            ADM_POSITIONS model = new ADM_POSITIONS();
            string msg = "";
            try {
                if (viewModel == null) { return Json(new { Status = false, Msg = "Invalid Access." }, JsonRequestBehavior.AllowGet); }
                msg=ValidateInputData(viewModel);
                if (msg != "") { return Json(new { Status = false, Msg = msg }, JsonRequestBehavior.AllowGet); }
                FillPositionModel(model, viewModel);
                if (!positionsService.CheckForDuplicate(viewModel.AP_ShortName, viewModel.AP_Description, collegeId)) { return Json(new { Status = false, Msg = "Short Name already exist." }, JsonRequestBehavior.AllowGet); }
                if (viewModel.AP_ID == 0)
                {
                    positionsService.Insert(model);
                    return Json(new { Status = true, Msg = "Inserted Successfully." , Data=positionsService.GetGridData(collegeId,viewModel.AP_Parent_Id)}, JsonRequestBehavior.AllowGet);
                }
                else {
                    if (positionsService.Update(model)) { return Json(new { Status = true, Msg = "Update Successfully", Data = positionsService.GetGridData(collegeId, viewModel.AP_Parent_Id), isUpdate=true }, JsonRequestBehavior.AllowGet); }
                    return Json(new { Status = true, Msg = "Invalid Updation.", Data = false, isUpdate=true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception exe) { return Json(new { Status = false, Msg = "Some Problem in Application." }, JsonRequestBehavior.AllowGet); }
            finally { model = null; msg = string.Empty; }
        }
        private string ValidateInputData(PositionsViewModel viewModel)
        {
            try
            {
                if (viewModel.AP_ShortName == null || viewModel.AP_ShortName.Trim() == "") { return "Please enter Short Name"; }
                if (viewModel.AP_Description == null || viewModel.AP_Description.Trim() == "") { return "Please enter Description"; }
                if (viewModel.AP_Parent_Id < 0 || viewModel.AP_PositionLevel_Id <= 0) { return "Dont Misuse."; }
                return "";
            }
            finally { }
        }
        private void FillPositionModel(ADM_POSITIONS model,PositionsViewModel viewModel)
        {
            try {
                model.AP_College_Id = collegeId;
                model.AP_Department_Id = viewModel.AP_Department_Id;
                model.AP_CreatedBy = createdBy;
                model.AP_CreatedDate = DateTime.Now;
                model.AP_Description = viewModel.AP_Description;
                model.AP_Parent_Id = viewModel.AP_Parent_Id;
                model.AP_PositionLevel_Id = viewModel.AP_PositionLevel_Id;
                model.AP_ShortName = viewModel.AP_ShortName;
                model.AP_StartDate = DateTime.Now.Date;
                model.AP_IsActive = true;
            }
            finally { }
        }

        [HttpPost]
        public ActionResult Delete(int id,DateTime deActivateDate)
        {
            ADM_POSITIONS model = new ADM_POSITIONS();
            try
            {
                if (id > 0)
                {
                    model=positionsService.GetPositionModelById(id, collegeId);
                    if ( model!= null)
                    {
                        if (positionsService.Delete(id, createdBy, deActivateDate)) { return Json(new { Status=true,Msg = "Deleted Successfully....", Data = positionsService.GetGridData(collegeId,model.AP_Parent_Id) }, JsonRequestBehavior.AllowGet); }
                    }
                }
                return Json(new {Status=false, Msg = "Invalid Deletion", Data = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(new { Status = false, Msg = "Some Problem in Application." }, JsonRequestBehavior.AllowGet); }
            finally { }
        }
    }
}
