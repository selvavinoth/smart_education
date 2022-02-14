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
    public class TaskController : Controller
    {
        //
        // GET: /Task/

        private readonly ITaskService taskService;
        public TaskController(ITaskService taskService) { this.taskService = taskService; }

        public ActionResult Index()
        {
            try { return View(); }
            catch (Exception exe) { return View(); }
        }

        [HttpPost]
        public ActionResult GetGridData()
        {
            try { return Json(taskService.GetTaskGridData(), JsonRequestBehavior.AllowGet); }
            catch (Exception exe) { return Json(false, JsonRequestBehavior.AllowGet); }
        }
        [HttpPost]
        public ActionResult TaskIUAction(TaskViewModel viewModel)
        {
            SADM_TASK model = new SADM_TASK();
            try
            {
                if (viewModel == null) { return Json(false); }
                if (viewModel.ST_Task_Name == null || viewModel.ST_Task_Name.Trim() == "") { return Json(new { Msg = ResourceKeeper.GetResource("CMN_PleaseEnter") + " " + ResourceKeeper.GetResource("APP_TaskName"), Status = false }, JsonRequestBehavior.AllowGet); }
                if (viewModel.ST_URL == null || viewModel.ST_URL.Trim() == "") { return Json(new { Msg = ResourceKeeper.GetResource("CMN_PleaseEnter") + " " + ResourceKeeper.GetResource("APP_Url"), Status = false }, JsonRequestBehavior.AllowGet); }
                if (!taskService.CheckForDublication(viewModel.ST_Id, viewModel.ST_Task_Name)) { return Json(new { Msg = ResourceKeeper.GetResource("APP_TaskName") + " " + ResourceKeeper.GetResource("CMN_Already_Exist"), Status = false }, JsonRequestBehavior.AllowGet); }
                model.ST_Task_Name = viewModel.ST_Task_Name;
                model.ST_URL = viewModel.ST_URL;
                if (viewModel.ST_Id != 0) {
                    model.ST_Id = viewModel.ST_Id;
                    if (taskService.Update(model)) { return Json(new { Msg = ResourceKeeper.GetResource("CMN_Updated_Successfully"), data = taskService.GetTaskGridData(), isUpdate = true, Status = true }, JsonRequestBehavior.AllowGet); }
                    else { return Json(new { Msg = ResourceKeeper.GetResource("CMN_Invalid_Update"), data = false, isUpdate = true, Status = false }, JsonRequestBehavior.AllowGet); }
                }
                else { taskService.Insert(model); return Json(new { Msg = ResourceKeeper.GetResource("CMN_Added_Successfully"), data = taskService.GetTaskGridData(), isUpdate = false, Status = true }, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception exe) { return Json(false); }
            finally { model = null; }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                if (id > 0) { if (taskService.Delete(id)) { return Json(new { Msg = ResourceKeeper.GetResource("CMN_Deleted_Successfully"), data = taskService.GetTaskGridData() }, JsonRequestBehavior.AllowGet); } }
                return Json(new { Msg = ResourceKeeper.GetResource("CMN_Invalid_Access"), data = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) { return Json(false,JsonRequestBehavior.AllowGet); }
        }

    }
}
