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
    public class ClassesController : Controller
    {
        //
        // GET: /Classes/
        private int CollegeId = 1;
        private long UserId = 0;
        private readonly IClassesService classesService;
        public ClassesController(IClassesService classesService) {
            this.classesService = classesService;
        }

        public ActionResult Index()
        {
            ClassesViewModel viewModel = new ClassesViewModel();
            try
            {
                InitViewModel(viewModel);
                return View(viewModel);
            }
            catch (Exception exe) { return View(viewModel); }
            finally { viewModel = null; }
        }
        private void InitViewModel(ClassesViewModel viewModel)
        {
            List<ADM_LOV> lovList = new List<ADM_LOV>();
            string type = "";
            try
            {
                lovList = classesService.GetLovList(CollegeId, new List<string>() { Convert.ToString(DataKeeper.LOVTypeKeeper.Graduation), Convert.ToString(DataKeeper.LOVTypeKeeper.Year), Convert.ToString(DataKeeper.LOVTypeKeeper.Department), Convert.ToString(DataKeeper.LOVTypeKeeper.Section), Convert.ToString(DataKeeper.LOVTypeKeeper.Qualification) }).ToList();
                type = DataKeeper.LOVTypeKeeper.Department.ToString();
                viewModel.DepartmentList = GetLovListByType(lovList.FindAll(exp => exp.AL_Type == type));
                type = DataKeeper.LOVTypeKeeper.Graduation.ToString();
                viewModel.GraduationList = GetLovListByType(lovList.FindAll(exp => exp.AL_Type == type));
                type = DataKeeper.LOVTypeKeeper.Year.ToString();
                viewModel.YearList = GetLovListByType(lovList.FindAll(exp => exp.AL_Type == type));
                type = DataKeeper.LOVTypeKeeper.Section.ToString();
                viewModel.SectionList = GetLovListByType(lovList.FindAll(exp => exp.AL_Type == type));
                type = DataKeeper.LOVTypeKeeper.Qualification.ToString();
                viewModel.DegreeList = GetLovListByType(lovList.FindAll(exp => exp.AL_Type == type));
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
        public ActionResult GetClassGridData()
        {
            try
            {
                return Json(classesService.GetClassGridData(CollegeId, new List<string>() { Convert.ToString(DataKeeper.LOVTypeKeeper.Graduation), Convert.ToString(DataKeeper.LOVTypeKeeper.Year), Convert.ToString(DataKeeper.LOVTypeKeeper.Department), Convert.ToString(DataKeeper.LOVTypeKeeper.Section), Convert.ToString(DataKeeper.LOVTypeKeeper.Qualification) }, DateTime.Now.Date.AddDays(1)), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { }
        }

        [HttpPost]
        public ActionResult ClassesIUAction(ClassesViewModel viewModel)
        {
            string msg = "";
            ADM_CLASSES model = new ADM_CLASSES();
            bool flag = false;
            try
            {
                if (viewModel == null) { return Json(false); }
                msg = ValidateData(viewModel);
                if (msg != "") { return Json(new { Msg = msg, Status = false, GridData = false }, JsonRequestBehavior.AllowGet); }
                FillViewModel(viewModel, model);
                if (!classesService.CheckForDuplicate(model, DateTime.Now.Date)) { return Json(new { Msg = "Code alreay excist.", Status = false, GridData = false }, JsonRequestBehavior.AllowGet); }
                if (model.AC_Id == 0) { classesService.InsertClasses(model, UserId); msg = "Added Successfully."; flag = true; }
                else { flag = classesService.UpdateClasses(model, UserId,DateTime.Now.Date); msg = (flag ? "Updated Successfully." : "Invalid Update."); }
                return Json(new { Msg = msg, Status = flag, GridData = (flag ? classesService.GetClassGridData(CollegeId, new List<string>() { Convert.ToString(DataKeeper.LOVTypeKeeper.Graduation), Convert.ToString(DataKeeper.LOVTypeKeeper.Year), Convert.ToString(DataKeeper.LOVTypeKeeper.Department), Convert.ToString(DataKeeper.LOVTypeKeeper.Section), Convert.ToString(DataKeeper.LOVTypeKeeper.Qualification) }, DateTime.Now.Date.AddDays(1)) : false) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { model = null; msg = string.Empty; }
        }

        private string ValidateData(ClassesViewModel viewModel)
        {
            try
            {
                if (viewModel.AC_Department <= 0) { return "Please select department"; }
                if (viewModel.AC_Degree<=0) { return "Please select degree"; }
                if (viewModel.AC_Graduate <= 0) { return "Please select graduation"; }
                if (viewModel.AC_Year <= 0) { return "Please select year"; }
                if (viewModel.AC_Section <= 0) { return "Please select Section"; }
                return "";
            }
            finally { }
        }
        private void FillViewModel(ClassesViewModel viewModel, ADM_CLASSES model)
        {
            try
            {
                model.AC_Graduate = viewModel.AC_Graduate;
                model.AC_CollegeId = CollegeId;
                model.AC_Degree = viewModel.AC_Degree;
                model.AC_Id = viewModel.AC_Id;
                model.AC_Department = viewModel.AC_Department;
                model.AC_Year = viewModel.AC_Year;
                model.AC_Section = viewModel.AC_Section;
            }
            finally { }
        }
        [HttpPost]
        public ActionResult DeleteClasses(int ClassesId)
        {
            bool flag = false;
            try
            {
                flag = classesService.DeleteClasses(CollegeId, ClassesId, UserId, DateTime.Now.Date);
                return Json(new { Msg = (flag ? "Deleted Successfully." : "Invalid access."), Status = flag, GridData = (flag ? classesService.GetClassGridData(CollegeId, new List<string>() { Convert.ToString(DataKeeper.LOVTypeKeeper.Graduation), Convert.ToString(DataKeeper.LOVTypeKeeper.Year), Convert.ToString(DataKeeper.LOVTypeKeeper.Department), Convert.ToString(DataKeeper.LOVTypeKeeper.Section), Convert.ToString(DataKeeper.LOVTypeKeeper.Qualification) }, DateTime.Now.Date.AddDays(1)) : false) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false); }
            finally { }
        }
    }
}
