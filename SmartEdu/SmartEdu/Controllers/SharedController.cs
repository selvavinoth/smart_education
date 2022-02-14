using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEdu.Data.Models;
using SmartEdu.Bll.Services;
using SmartEdu.Helper;
using SmartEdu.SessionHolder;
using System.IO;

namespace SmartEdu.Controllers
{
    public class SharedController : Controller
    {
        //
        // GET: /Shared/
        private int CollegeID
        {
            get { return SessionPersistor.CollegeId; }
            set { SessionPersistor.CollegeId = value; }
        }
        private int DepartmetID
        {
            get { return SessionPersistor.DPID; }
            set { SessionPersistor.DPID = value; }
        }
        private int SelectedCollegeID
        {
            get { return SessionPersistor.SelectedCollegeID; }
            set { SessionPersistor.SelectedCollegeID = value; }
        }
        private int SelectedDepartmetID
        {
            get { return SessionPersistor.SelectedDPID; }
            set { SessionPersistor.SelectedDPID = value; }
        }

        private readonly ISharedService sharedService;
        public SharedController(ISharedService sharedService) { this.sharedService = sharedService; }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetCollge() {
            try {
                return Json(new { collegeList = GetCollegeList() }, JsonRequestBehavior.AllowGet);
            }
            finally { }
        }
        [HttpPost]
        public ActionResult GetDepartment(int collegeId)
        {
            try
            {
                return Json(new { dpList = GetDepartmentList(collegeId)}, JsonRequestBehavior.AllowGet);
            }
            finally { }
        }
        [HttpPost]
        public ActionResult SetSelectedCollegeId(int id) {
            try {
                SelectedCollegeID = id;
                SelectedDepartmetID = 0;
                return Json(false);
            }
            finally { }
        }
        [HttpPost]
        public ActionResult SetSelectedDPId(int id)
        {
            try
            {
                SelectedDepartmetID = id;
                return Json(false);
            }
            finally { }
        }

        #region List Filling
        private IEnumerable<SelectListItem> GetCollegeList() {
            List<ADM_COLLEGE> collegeList;
            try {
                collegeList = sharedService.GetCollegeList();
                return (from m in collegeList select new SelectListItem { Text = m.ACE_ShortDescription, Value = m.ACE_Id.ToString() }).ToList();
            }
            finally { collegeList = null; }
        }
        private IEnumerable<SelectListItem> GetDepartmentList(int collegeId)
        {
            List<ADM_DEPARTMENTS> dpList;
            try
            {
                dpList = sharedService.GetDepartmentList(collegeId);
                return (from m in dpList select new SelectListItem { Text = m.ADP_ShortDescription, Value = m.ADP_Id.ToString() }).ToList();
            }
            finally { dpList = null; }
        }
        #endregion

        #region Image Upload
        [HttpPost]
        public AjaxFileUploadResponse UploadImage(HttpPostedFileBase imageFile)
        {
            if (imageFile == null || imageFile.ContentLength == 0)
            {
                return new AjaxFileUploadResponse
                {
                    Data = new
                    {
                        IsValid = false,
                        Message = "No file was uploaded.",
                        ImagePath = string.Empty
                    }
                };
            }

            var fileName = String.Format("{0}.jpg", Guid.NewGuid().ToString());
            var imagePath = Path.Combine(Server.MapPath(Url.Content("~/Uploads")), fileName);

            imageFile.SaveAs(imagePath);

            return new AjaxFileUploadResponse
            {
                Data = new
                {
                    IsValid = true,
                    Message = string.Empty,
                    ImagePath = Url.Content(String.Format("~/Uploads/{0}", fileName))
                }
            };
        }
        #endregion
    }
}
