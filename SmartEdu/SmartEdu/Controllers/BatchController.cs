using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEdu.ViewModel;
using SmartEdu.Bll.Services;
using SmartEdu.Data.Models;
using SmartEdu.Helper;

namespace SmartEdu.Controllers
{
    public class BatchController : Controller
    {
        //
        // GET: /Batch/
        private int CollegeId = 1;
        private long UserId = 0;
        private readonly IBatchService batchService;
        public BatchController(IBatchService batchService) {
            this.batchService = batchService;
        }
        public ActionResult Index()
        {
            BatchViewModel viewModel = new BatchViewModel();
            try
            {
                InitViewModel(viewModel);
                return View(viewModel);
            }
            catch (Exception exe) { return View(viewModel); }
            finally { viewModel = null; }
        }

        private void InitViewModel(BatchViewModel viewModel)
        {
            List<ADM_LOV> lovList = new List<ADM_LOV>();
            string type = "";
            try
            {
                lovList = batchService.GetLovList(CollegeId, new List<string>() { Convert.ToString(DataKeeper.LOVTypeKeeper.Graduation), Convert.ToString(DataKeeper.LOVTypeKeeper.Department), Convert.ToString(DataKeeper.LOVTypeKeeper.Qualification) }).ToList();
                type = DataKeeper.LOVTypeKeeper.Department.ToString();
                viewModel.DepartmentList = GetLovListByType(lovList.FindAll(exp => exp.AL_Type == type));
                type = DataKeeper.LOVTypeKeeper.Graduation.ToString();
                viewModel.GraduationList = GetLovListByType(lovList.FindAll(exp => exp.AL_Type == type));
                type = DataKeeper.LOVTypeKeeper.Year.ToString();
                type = DataKeeper.LOVTypeKeeper.Qualification.ToString();
                viewModel.DegreeList = GetLovListByType(lovList.FindAll(exp => exp.AL_Type == type));
                viewModel.YearList = GetYearList();
            }
            finally { lovList = null; type = string.Empty; }
        }
        private IEnumerable<SelectListItem> GetYearList() {
            List<int> yearList = new List<int>();
            try {
                for (int i = -4; i <= 5; i++) {
                    yearList.Add(DateTime.Now.Year + i);
                }
                return (from y in yearList
                        select new SelectListItem { Text=y.ToString(),Value=y.ToString()}).ToList();
            }
            finally { }
        }
        private IEnumerable<SelectListItem> GetLovListByType(List<ADM_LOV> lovList)
        {

            try
            {
                if (lovList == null) { lovList = new List<ADM_LOV>(); }
                return (new List<SelectListItem>() { new SelectListItem { Text = "- Select -", Value = "0" } }).Union((from l in lovList
                                                                                                                       select new SelectListItem { Text = l.AL_Code, Value = l.AL_Id.ToString() }).OrderBy(exp => exp.Text)).ToList();
            }
            finally { }
        }

        [HttpPost]
        public ActionResult GetBatchGridData()
        {
            try
            {
                return Json(batchService.GetBatchGridData(CollegeId, new List<string>() { Convert.ToString(DataKeeper.LOVTypeKeeper.Graduation), Convert.ToString(DataKeeper.LOVTypeKeeper.Department), Convert.ToString(DataKeeper.LOVTypeKeeper.Qualification) }, DateTime.Now.Date.AddDays(1)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { }
        }

        [HttpPost]
        public ActionResult BatchIUAction(BatchViewModel viewModel)
        {
            string msg = "";
            ADM_BATCH model = new ADM_BATCH();
            bool flag = false;
            try
            {
                if (viewModel == null) { return Json(false); }
                msg = ValidateData(viewModel);
                FillViewModel(viewModel, model);
                if (msg != "") { return Json(new { Msg = msg, Status = false, GridData = false }, JsonRequestBehavior.AllowGet); }
                if (!batchService.CheckForDuplicate(model, DateTime.Now.Date)) { return Json(new { Msg = "Code alreay excist.", Status = false, GridData = false }, JsonRequestBehavior.AllowGet); }
                if (model.AB_Id == 0) { batchService.InsertBatch(model, UserId); msg = "Added Successfully."; flag = true; }
                else { flag = batchService.UpdateBatch(model, UserId, DateTime.Now.Date); msg = (flag ? "Updated Successfully." : "Invalid Update."); }
                return Json(new { Msg = msg, Status = flag, GridData = (flag ? batchService.GetBatchGridData(CollegeId, new List<string>() { Convert.ToString(DataKeeper.LOVTypeKeeper.Graduation), Convert.ToString(DataKeeper.LOVTypeKeeper.Department), Convert.ToString(DataKeeper.LOVTypeKeeper.Qualification) }, DateTime.Now.Date.AddDays(1)) : false) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { model = null; msg = string.Empty; }
        }

        private string ValidateData(BatchViewModel viewModel)
        {
            try
            {
                if (viewModel.AB_Department <= 0) { return "Please select department"; }
                if (viewModel.AB_Degree <= 0) { return "Please select degree"; }
                if (viewModel.AB_Graduate <= 0) { return "Please select graduation"; }
                if (viewModel.AB_Year == null || viewModel.AB_Year <= 0) { return "Please select year"; }
                return "";
            }
            finally { }
        }
        private void FillViewModel(BatchViewModel viewModel, ADM_BATCH model)
        {
            try
            {
                model.AB_Graduate = viewModel.AB_Graduate;
                model.AB_CollegeId = CollegeId;
                model.AB_Degree = viewModel.AB_Degree;
                model.AB_Id = viewModel.AB_Id;
                model.AB_Department = viewModel.AB_Department;
                model.AB_Year = viewModel.AB_Year;
                model.AB_Batch = viewModel.AB_Year+"-"+(viewModel.AB_Year + 4);
            }
            finally { }
        }

        [HttpPost]
        public ActionResult DeleteBatch(int batchId)
        {
            bool flag = false;
            try
            {
                if (batchId <= 0) { return Json(false); }
                flag = batchService.DeleteBatch(CollegeId, batchId, UserId, DateTime.Now.Date);
                return Json(new { Msg = (flag ? "Deleted Successfully." : "Invalid access."), Status = flag, GridData = (flag ? batchService.GetBatchGridData(CollegeId, new List<string>() { Convert.ToString(DataKeeper.LOVTypeKeeper.Graduation), Convert.ToString(DataKeeper.LOVTypeKeeper.Department), Convert.ToString(DataKeeper.LOVTypeKeeper.Qualification) }, DateTime.Now.Date.AddDays(1)) : false) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { }
        }
    }
}
