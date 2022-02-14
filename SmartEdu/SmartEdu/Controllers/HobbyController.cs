using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEdu.Bll.Services;
using SmartEdu.ViewModel;
using SmartEdu.Data.Models;

namespace SmartEdu.Controllers
{
    public class HobbyController : Controller
    {
        //
        // GET: /Hobby/
        private readonly IHobbyService hobbyService;
        public HobbyController(IHobbyService hobbyService) {
            this.hobbyService = hobbyService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetGridData() {
            try
            {
                return Json(hobbyService.GetHobbyGridData(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe) {
                return Json(false);
            }
            finally { }
        }
        [HttpPost]
        public ActionResult HobbyIUAction(HobbyViewModel viewModel) {
            SADM_HOBBIESLIST model = new SADM_HOBBIESLIST();
            try {
                if (viewModel == null) { return Json(false); }
                if (viewModel.SHL_Code == null || viewModel.SHL_Code.Trim() == "") { return Json(new { Msg = "Please Enter the code", Status = false }, JsonRequestBehavior.AllowGet); }
                if (viewModel.SHL_Description == null || viewModel.SHL_Description.Trim() == "") { return Json(new { Msg = "Please Enter the description", Status = false }, JsonRequestBehavior.AllowGet); }
                if (!hobbyService.CheckForDublication(viewModel.SHL_ID,viewModel.SHL_Code)) { return Json(new { Msg = "Code already excist.", Status = false }, JsonRequestBehavior.AllowGet); }
                model.SHL_Code = viewModel.SHL_Code;
                model.SHL_Description = viewModel.SHL_Description;
                if (viewModel.SHL_ID != 0)
                {
                    viewModel.SHL_ID = viewModel.SHL_ID;
                    if (hobbyService.Update(model)) { return Json(new { Msg = "UpDated Successfully....", data = hobbyService.GetHobbyGridData(), isUpdate = true, Status = true }, JsonRequestBehavior.AllowGet); }
                    else { return Json(new { Msg = "Invalid Updation", data = false, isUpdate = true, Status = false }, JsonRequestBehavior.AllowGet); }
                }
                else { hobbyService.Insert(model); return Json(new { Msg = "Inserted Successfully....", data = hobbyService.GetHobbyGridData(), isUpdate = false, Status = true }, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception exe) { return Json(false); }
            finally { viewModel = null; model = null; }
        }

        [HttpPost]
        public ActionResult Delete(int id) {
            try
            {
                if (id > 0)
                {
                    if (hobbyService.Delete(id)) { return Json(new { Msg = "Deleted Successfully....", data = hobbyService.GetHobbyGridData() }, JsonRequestBehavior.AllowGet); }
                }
                return Json(new { Msg = "Invalid Deletion", data = false }, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception exe) {
                return Json(false);
            }
            finally { }
        }
    }
}
