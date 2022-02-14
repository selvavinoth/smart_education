using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEdu.ViewModel;
using SmartEdu.Data.Models;
using SmartEdu.Bll.Services;
using SmartEdu.Helper;

namespace SmartEdu.Controllers
{
    public class ALOVController : Controller
    {
        //
        // GET: /ALOV/
        private readonly IADM_LOVService admLovService;
        public ALOVController(IADM_LOVService admLovService) { this.admLovService = admLovService; }
        public ActionResult Index()
        {
            AdmLovViewModel viewModel = new AdmLovViewModel();
            try { viewModel.LovTypList = GetLovTypeList().ToList(); return View(viewModel); }
            catch (Exception exe) { return View(viewModel); }
            finally { viewModel = null; }
        }
        public ActionResult Detail(string LovType)
        {
            AdmLovViewModel viewModel = new AdmLovViewModel();
            try {
                if (LovType == null) { return RedirectToAction("Index"); }
                viewModel.Flag = true;
                viewModel.AL_Type= LovType; return View("Index",viewModel); 
            }
            catch (Exception exe) { return RedirectToAction("Index"); }
            finally { viewModel = null; }
        }
        private List<SelectListItem> GetLovTypeList()
        {
            List<string> lovList = new List<string>();
            try
            {
                lovList = Enum.GetNames(typeof(DataKeeper.LOVTypeKeeper)).ToList();
                return (new List<SelectListItem>() { new SelectListItem { Text = ResourceKeeper.GetResource("CMN_Select"), Value = "" } }).Union((from l in lovList
                                                                                                        select new SelectListItem {Text=l,Value=l}).OrderBy(exp=>exp.Text)).ToList();                         
            }
            finally{lovList = null;}
        }
        [HttpPost]
        public ActionResult GetGridDataByType(string type)
        {
            try { return Json(admLovService.GetGridData(type), JsonRequestBehavior.AllowGet); }
            catch (Exception exe) { return Json(false, JsonRequestBehavior.AllowGet); }
        }
        [HttpPost]
        public ActionResult GetGridData()
        {
            try { return Json(admLovService.GetGridData(null), JsonRequestBehavior.AllowGet); }
            catch (Exception exe) { return Json(false, JsonRequestBehavior.AllowGet); }
        }
        [HttpPost]
        public ActionResult EditLov(int id)
        {
            ADM_LOV model = new ADM_LOV();
            try {
                if (id > 0) {
                    model = admLovService.GetLOVModelById(id);
                    if (model != null) { return Json(new { model = new AdmLovViewModel(model), Status = true }, JsonRequestBehavior.AllowGet); }
                }
                return Json(new { model = false, Status = false, Msg = ResourceKeeper.GetResource("CMN_Invalid_Access") }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(new { model = false, Status = false, Msg = ResourceKeeper.GetResource("CMN_Invalid_Access") }, JsonRequestBehavior.AllowGet); }
            finally { model = null; }
        }
        [HttpPost]
        public ActionResult ALOVIUAction(AdmLovViewModel viewModel)
        {
            ADM_LOV model = new ADM_LOV();
            try
            {
                if (viewModel == null) { return Json(false, JsonRequestBehavior.AllowGet); }
                model.AL_Code = viewModel.AL_Code;
                model.AL_Type = viewModel.AL_Type;
                model.AL_Description = viewModel.AL_Description;
                model.AL_StartDate = DateTime.Now.Date;
                model.AL_EndDate = DateTime.Now.AddDays(10).Date;
                model.AL_IsActive = true;
                if (viewModel.AL_Code == null || viewModel.AL_Code.Trim() == "") { return Json(new { Msg = ResourceKeeper.GetResource("CMN_PleaseEnter") + " " + ResourceKeeper.GetResource("APP_Code"), data = false }, JsonRequestBehavior.AllowGet); }
                if (viewModel.AL_Type == null || viewModel.AL_Type.Trim() == "") { return Json(new { Msg = ResourceKeeper.GetResource("CMN_PleaseSelect") + " " + ResourceKeeper.GetResource("APP_Type"), data = false }, JsonRequestBehavior.AllowGet); }
                if (!admLovService.CheckForDuplication(viewModel.AL_Type, viewModel.AL_Code)) { return Json(new { Msg = ResourceKeeper.GetResource("CMN_Combination_Of") + " " + ResourceKeeper.GetResource("APP_Code") + " , " + ResourceKeeper.GetResource("APP_Type") + " " + ResourceKeeper.GetResource("CMN_Already_Exist"), data = false }, JsonRequestBehavior.AllowGet); }
                if (viewModel.AL_Id != 0)
                {
                    model.AL_Id = viewModel.AL_Id;
                    if (admLovService.UpdateLOV(model)) { return Json(new { Msg = ResourceKeeper.GetResource("CMN_Updated_Successfully"), data = admLovService.GetGridData((viewModel.Flag?viewModel.AL_Type:null)), isUpdate = true, Status = true }, JsonRequestBehavior.AllowGet); }
                    else { return Json(new { Msg = ResourceKeeper.GetResource("CMN_Invalid_Access"), data = false, isUpdate = true, Status = false }, JsonRequestBehavior.AllowGet); }
                }
                else { admLovService.InsertLOV(model); return Json(new { Msg = ResourceKeeper.GetResource("CMN_Added_Successfully"), data = admLovService.GetGridData((viewModel.Flag ? viewModel.AL_Type : null)), isUpdate = false, Status = true }, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception e) { return Json(false, JsonRequestBehavior.AllowGet); }
            finally { model = null; }
            
        }
        [HttpPost]
        public ActionResult Delete(int id,bool flag,string type)
        {
            try
            {
                if (id > 0) { if (admLovService.DeleteLOV(id)) { return Json(new { Msg = ResourceKeeper.GetResource("CMN_Deleted_Successfully"), data = admLovService.GetGridData((flag?type:null)) }, JsonRequestBehavior.AllowGet); } }
                return Json(new { Msg = ResourceKeeper.GetResource("CMN_Invalid_Access"), data = false }, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception exe) { return Json(new { Msg = ResourceKeeper.GetResource("CMN_Invalid_Access"), data = false }, JsonRequestBehavior.AllowGet); }
        }
    }
}
