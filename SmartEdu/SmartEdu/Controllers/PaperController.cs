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
    public class PaperController : Controller
    {
        //
        // GET: /Paper/
        private int CollegeId = 1;
        private long UserId = 0;
        private IPaperService paperService;
        public PaperController(IPaperService paperService) {
            this.paperService = paperService;
        }
        public ActionResult Index(){
            PaperViewModel viewModel = new PaperViewModel();
            try
            {
                //InitViewModel(viewModel);
                return View(viewModel);
            }
            catch (Exception exe) { return View(viewModel); }
            finally { viewModel = null; }
        }

        [HttpPost]
        public ActionResult GetPaperGridData() {
            try
            {
                return Json(paperService.GetGridData(CollegeId, DateTime.Now.Date.AddDays(1)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { }
        }

        [HttpPost]
        public ActionResult PaperIUAction(PaperViewModel viewModel)
        {
            string msg = "";
            ADM_PAPER model=new ADM_PAPER();
            bool flag = false;
            try {
                if (viewModel == null) { return Json(false); }
                msg = ValidateData(viewModel);
                FillViewModel(viewModel, model);
                if (!paperService.CheckDuplicate(model, DateTime.Now.Date)) { return Json(new {Msg="Code alreay excist.", Status=false,GridData=false }, JsonRequestBehavior.AllowGet); }
                if (model.AP_Id == 0) {paperService.InsertPaper(model, UserId);msg="Added Successfully.";flag=true;}
                else {flag = paperService.UpdatePaper(model, UserId);msg = (flag ? "Updated Successfully." : "Invalid Update.");}
                return Json(new { Msg = msg, Status = flag, GridData = (flag ? paperService.GetGridData(CollegeId, DateTime.Now.Date.AddDays(1)) : false) }, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception exe) { return Json(false); }
            finally { model = null; msg = string.Empty; }
        }

        private string ValidateData(PaperViewModel viewModel)
        {
            try {
                //if (viewModel.AP_Department == null || viewModel.AP_Department.Trim() == "") { return "Please select department"; }
                //if (viewModel.AP_Degree == null || viewModel.AP_Degree.Trim() == "") { return "Please select degree"; }
                //if (viewModel.AP_Graduation == null || viewModel.AP_Graduation.Trim() == "") { return "Please select graduation"; }
                //if (viewModel.AP_Year == null || viewModel.AP_Year.Trim() == "") { return "Please select year"; }
                //if (viewModel.AP_Semester == null || viewModel.AP_Semester.Trim() == "") { return "Please select semester"; }
                if (viewModel.AP_Code == null || viewModel.AP_Code.Trim() == "") { return "Please enter code"; }
                if (viewModel.AP_ShortName == null || viewModel.AP_ShortName.Trim() == "") { return "Please enter shortname"; }
                if (viewModel.AP_Description == null || viewModel.AP_Description.Trim() == "") { return "Please enter description"; }
                return "";
            }
            finally { }
        }
        private void FillViewModel(PaperViewModel viewModel,ADM_PAPER model)
        {
            try
            {
                model.AP_Code = viewModel.AP_Code;
                model.AP_College_Id = CollegeId;
                model.AP_Description = viewModel.AP_Description;
                model.AP_Id = viewModel.AP_Id;
                model.AP_IsPractical = viewModel.AP_IsPractical;
                model.AP_ShortName = viewModel.AP_ShortName;
            }
            finally { }
        }

        [HttpPost]
        public ActionResult DeletePaper(int paperId) {
            bool flag = false;
            try {
                flag = paperService.DeletePaper(paperId, CollegeId, DateTime.Now.Date, UserId);
                return Json(new { Msg = (flag?"Deleted Successfully.": "Invalid access."), Status = flag, GridData = (flag ? paperService.GetGridData(CollegeId, DateTime.Now.Date.AddDays(1)) : false) }, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception exe) { return Json(false); }
            finally { }
        }
    }
}
