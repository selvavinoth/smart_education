using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEdu.ViewModel;
using SmartEdu.Data.Models;
using SmartEdu.Helper;
using SmartEdu.Bll.Services;

namespace SmartEdu.Controllers
{
    public class SemesterController : Controller
    {
        //
        // GET: /Semester/
        private int CollegeId = 1;
        private long UserId = 0;

        private readonly ISemesterService semesterService;
        public SemesterController(ISemesterService semesterService) {
            this.semesterService = semesterService;
        }
        public ActionResult Index()
        {
            SemesterViewModel viewModel = new SemesterViewModel();
            try
            {
                InitViewModel(viewModel);
                return View(viewModel);
            }
            catch (Exception exe) { return View(viewModel); }
            finally { }
        }

        private void InitViewModel(SemesterViewModel viewModel)
        {
            List<ADM_LOV> lovList = new List<ADM_LOV>();
            string type = "";
            try
            {
                lovList = semesterService.GetLovList(CollegeId, Convert.ToString(DataKeeper.LOVTypeKeeper.Graduation),DateTime.Now.Date).ToList();
                type = DataKeeper.LOVTypeKeeper.Graduation.ToString();
                viewModel.GraduationList = GetLovListByType(lovList.FindAll(exp => exp.AL_Type == type));
            }
            finally { lovList = null; type = string.Empty; }
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
        public ActionResult GetSemesterGridData()
        {
            try
            {
                return Json(semesterService.GetSemesterGridData(CollegeId, new List<string>() { Convert.ToString(DataKeeper.LOVTypeKeeper.Graduation), Convert.ToString(DataKeeper.LOVTypeKeeper.Department), Convert.ToString(DataKeeper.LOVTypeKeeper.Qualification) },DateTime.Now.Date, DateTime.Now.Date.AddDays(1)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { }
        }
        [HttpPost]
        public ActionResult GetDegreeList(int graduationId)
        {
            List<ADM_LOV> lovList = new List<ADM_LOV>();
            try
            {
                 lovList = semesterService.GetLovList(CollegeId, Convert.ToString(DataKeeper.LOVTypeKeeper.Qualification),DateTime.Now.Date).ToList();
                 return Json(GetLovListByType(lovList), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { }
        }
        [HttpPost]
        public ActionResult GetBatchList(int graduationId,int degreeId)
        {
            IEnumerable<ADM_LOV> lovList;
            IEnumerable<ADM_BATCH> batchList;
            object data = new object();
            try
            {
                lovList = semesterService.GetLovList(CollegeId, Convert.ToString(DataKeeper.LOVTypeKeeper.Department), DateTime.Now.Date).ToList();
                batchList = semesterService.GetBatchList(CollegeId, graduationId, degreeId, DateTime.Now.Date);
                data = (new List<SelectListItem>() { new SelectListItem { Text = "- Select -", Value = "0" } }).Union((
                    from b in batchList
                    join l in lovList on b.AB_Department equals l.AL_Id
                    select new SelectListItem { Text = b.AB_Batch+" [ "+l.AL_Code+" ] ", Value = b.AB_Id.ToString() }).OrderBy(exp => exp.Text)).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { lovList = null; batchList = null; }
        }

        [HttpPost]
        public ActionResult SemesterIUAction(SemesterViewModel viewModel)
        {
            string msg = "";
            ADM_SEMESTER model = new ADM_SEMESTER();
            bool flag = false;
            try
            {
                if (viewModel == null) { return Json(false); }
                msg = ValidateData(viewModel);
                if (msg != "") { return Json(new { Msg = msg, Status = false, GridData = false }, JsonRequestBehavior.AllowGet); }
                FillViewModel(viewModel, model);
                if (!semesterService.CheckForDuplicate(model, DateTime.Now.Date)) { return Json(new { Msg = "Data alreay excist.", Status = false, GridData = false }, JsonRequestBehavior.AllowGet); }
                if (model.AS_Id == 0) { semesterService.InsertSemester(model, UserId); msg = "Added Successfully."; flag = true; }
                else { flag = semesterService.UpdateSemester(model, UserId, DateTime.Now.Date); msg = (flag ? "Updated Successfully." : "Invalid Update."); }
                return Json(new { Msg = msg, Status = flag, GridData = (flag ? semesterService.GetSemesterGridData(CollegeId, new List<string>() { Convert.ToString(DataKeeper.LOVTypeKeeper.Graduation), Convert.ToString(DataKeeper.LOVTypeKeeper.Department), Convert.ToString(DataKeeper.LOVTypeKeeper.Qualification) }, DateTime.Now.Date,DateTime.Now.Date.AddDays(1)) : false) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { model = null; msg = string.Empty; }
        }

        private string ValidateData(SemesterViewModel viewModel)
        {
            try
            {
                if (viewModel.AS_Code == null || viewModel.AS_Code.Trim()=="") { return "Please enter code."; }
                if (viewModel.AS_Description == null || viewModel.AS_Description.Trim() == "") { return "Please enter description"; }
                if (viewModel.AS_BatchId <= 0) { return "Please select batch"; }
                return "";
            }
            finally { }
        }
        private void FillViewModel(SemesterViewModel viewModel, ADM_SEMESTER model)
        {
            try
            {
                model.AS_BatchId = viewModel.AS_BatchId;
                model.AS_CollegeId = CollegeId;
                model.AS_Code = viewModel.AS_Code;
                model.AS_Id = viewModel.AS_Id;
                model.AS_Description = viewModel.AS_Description;
            }
            finally { }
        }
        [HttpPost]
        public ActionResult DeleteSemester(int semesterId)
        {
            bool flag = false;
            try
            {
                flag = semesterService.DeleteSemester(CollegeId, semesterId, UserId, DateTime.Now.Date);
                return Json(new { Msg = (flag ? "Deleted Successfully." : "Invalid access."), Status = flag, GridData = (flag ? semesterService.GetSemesterGridData(CollegeId, new List<string>() { Convert.ToString(DataKeeper.LOVTypeKeeper.Graduation), Convert.ToString(DataKeeper.LOVTypeKeeper.Department), Convert.ToString(DataKeeper.LOVTypeKeeper.Qualification) }, DateTime.Now.Date,DateTime.Now.Date.AddDays(1)) : false) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { }
        }
    }
}
