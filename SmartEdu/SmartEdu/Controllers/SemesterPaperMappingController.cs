using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEdu.Bll.Services;
using SmartEdu.Helper;
using SmartEdu.ViewModel;
using SmartEdu.Data.Models;

namespace SmartEdu.Controllers
{
    public class SemesterPaperMappingController : Controller
    {
        //
        // GET: /SemesterPaperMappig/
        private int CollegeId = 1;
        private long UserId = 0;

        private readonly ISemesterPaperMappingService semesterPaperMappingService;
        public SemesterPaperMappingController(ISemesterPaperMappingService semesterPaperMappingService)
        {
            this.semesterPaperMappingService = semesterPaperMappingService;
        }

        public ActionResult Index()
        {
            SemesterPaperMappingViewModel viewModel = new SemesterPaperMappingViewModel();
            try
            {
                viewModel.BatchList = GetBatchList();
                viewModel.PaperList = GetPaperList();
                viewModel.GraduationList = GetGraduationList();
                return View(viewModel);
            }
            catch (Exception exe) { return View(viewModel); }
            finally { viewModel = null; }
        }

        private IEnumerable<SelectListItem> GetBatchList()
        {
            IEnumerable<string> batchList;
            try
            {
                batchList = semesterPaperMappingService.GetBatchList(CollegeId, DateTime.Now.Date);
                if (batchList == null || batchList.Count() == 0) { return null; }
                return (from b in batchList
                        select new SelectListItem { Text = b, Value = b }).OrderBy(exp => exp.Text).ToList();
            }
            finally { batchList = null; }
        }
        private IEnumerable<SelectListItem> GetPaperList()
        {
            IEnumerable<ADM_PAPER> paperList;
            try
            {
                paperList = semesterPaperMappingService.GetPaperList(CollegeId, DateTime.Now.Date);
                if (paperList == null || paperList.Count() == 0) { return null; }
                return (from p in paperList
                        select new SelectListItem { Text = p.AP_Description+" - "+(p.AP_IsPractical?"[P]":"[T]"), Value = p.AP_Id.ToString() }).OrderBy(exp => exp.Text).ToList();
            }
            finally { paperList = null; }
        }
        private IEnumerable<SelectListItem> GetGraduationList()
        {
            IEnumerable<ADM_LOV> lovList;
            try
            {
                lovList = semesterPaperMappingService.GetLovList(CollegeId, new List<string>() { Convert.ToString(DataKeeper.LOVTypeKeeper.Graduation) });
                if (lovList == null || lovList.Count() == 0) { return null; }
                return (from l in lovList
                        select new SelectListItem { Text = l.AL_Code, Value = l.AL_Id.ToString() }).OrderBy(exp => exp.Text).ToList();
            }
            finally { lovList = null; }
        }
        private IEnumerable<SelectListItem> GetSemesterList(string batchString, int graduation)
        {
            IEnumerable<ADM_SEMESTER> semesterList;
            IEnumerable<ADM_BATCH> batchList;
            IEnumerable<ADM_LOV> lovList;
            try
            {
                semesterList = semesterPaperMappingService.GetSemesterList(CollegeId, DateTime.Now.Date);
                if (semesterList == null || semesterList.Count() == 0) { return (new List<SelectListItem>() { new SelectListItem { Text = "- Select -", Value = "0" } }).ToList(); }
                batchList = semesterPaperMappingService.GetBatchList(CollegeId, batchString,graduation, DateTime.Now.Date);
                if (batchList == null || batchList.Count() == 0) { return (new List<SelectListItem>() { new SelectListItem { Text = "- Select -", Value = "0" } }).ToList(); }
                lovList = semesterPaperMappingService.GetLovList(CollegeId, new List<string>() { Convert.ToString(DataKeeper.LOVTypeKeeper.Department) });
                if (lovList == null || lovList.Count() == 0) { return (new List<SelectListItem>() { new SelectListItem { Text = "- Select -", Value = "0" } }).ToList(); }
                return (new List<SelectListItem>() { new SelectListItem { Text = "- Select -", Value = "0" } }).Union(
                    (from s in semesterList
                     join b in batchList on s.AS_BatchId equals b.AB_Id
                     join d in lovList on b.AB_Department equals d.AL_Id
                     select new SelectListItem { Text = d.AL_Code + " [ " + s.AS_Description + " ]", Value = s.AS_Id.ToString() }).OrderBy(exp => exp.Text)).ToList();
            }
            finally { semesterList = null; batchList = null; lovList = null; }
        }
        [HttpPost]
        public ActionResult GetGridData()
        {
            try
            {
                return Json(semesterPaperMappingService.GetMappingGridData(CollegeId, DateTime.Now.Date, DateTime.Now.Date.AddDays(1), new List<string>() { Convert.ToString(DataKeeper.LOVTypeKeeper.Department), Convert.ToString(DataKeeper.LOVTypeKeeper.Qualification) }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { }
        }

        [HttpPost]
        public ActionResult GetSemesterData(string batchString,int graduation)
        {
            try
            {
                return Json(GetSemesterList(batchString, graduation), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { }
        }
        [HttpPost]
        public ActionResult SemseterPaperMappingIUAction(SemesterPaperMappingViewModel viewModel)
        {
            string msg = "";
            ADM_SEMESTER_PAPER_MAPPING model = new ADM_SEMESTER_PAPER_MAPPING();
            bool flag = false;
            try
            {
                if (viewModel == null) { return Json(false); }
                msg = ValidateData(viewModel);
                if (msg != "") { return Json(new { Msg = msg, Status = false, GridData = false }, JsonRequestBehavior.AllowGet); }
                FillViewModel(viewModel, model);
                if (!semesterPaperMappingService.CheckForDuplicate(model, DateTime.Now.Date)) { return Json(new { Msg = "Mapping Combination alreay excist.", Status = false, GridData = false }, JsonRequestBehavior.AllowGet); }
                if (model.ASPM_Id == 0) { semesterPaperMappingService.Insert(model, UserId); msg = "Added Successfully."; flag = true; }
                else { flag = semesterPaperMappingService.Update(model, UserId, DateTime.Now.Date); msg = (flag ? "Updated Successfully." : "Invalid Update."); }
                return Json(new { Msg = msg, Status = flag, GridData = (flag ? semesterPaperMappingService.GetMappingGridData(CollegeId, DateTime.Now.Date, DateTime.Now.Date.AddDays(1), new List<string>() { Convert.ToString(DataKeeper.LOVTypeKeeper.Department), Convert.ToString(DataKeeper.LOVTypeKeeper.Qualification) }) : false) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { model = null; msg = string.Empty; }
        }

        private string ValidateData(SemesterPaperMappingViewModel viewModel)
        {
            try
            {
                if (viewModel.ASPM_AP_Id <= 0) { return "Please select Paper"; }
                if (viewModel.ASPM_AS_Id <= 0) { return "Please select Semester"; }
                return "";
            }
            finally { }
        }
        private void FillViewModel(SemesterPaperMappingViewModel viewModel, ADM_SEMESTER_PAPER_MAPPING model)
        {
            try
            {
                model.ASPM_CollegeId = CollegeId;
                model.ASPM_AP_Id = viewModel.ASPM_AP_Id;
                model.ASPM_AS_Id = viewModel.ASPM_AS_Id;
                model.ASPM_Id = viewModel.ASPM_Id;
            }
            finally { }
        }

        [HttpPost]
        public ActionResult Delete(int mappingIdId)
        {
            bool flag = false;
            try
            {
                if (mappingIdId <= 0) { return Json(false); }
                flag = semesterPaperMappingService.Delete(CollegeId, mappingIdId, UserId, DateTime.Now.Date);
                return Json(new { Msg = (flag ? "Deleted Successfully." : "Invalid access."), Status = flag, GridData = (flag ? semesterPaperMappingService.GetMappingGridData(CollegeId, DateTime.Now.Date, DateTime.Now.Date.AddDays(1), new List<string>() { Convert.ToString(DataKeeper.LOVTypeKeeper.Department), Convert.ToString(DataKeeper.LOVTypeKeeper.Qualification) }) : false) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { }
        }
    }
}
