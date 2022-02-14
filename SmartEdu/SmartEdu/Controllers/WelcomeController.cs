using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEdu.Data.Models;
using SmartEdu.Bll.Services;
using System.Text;

namespace SmartEdu.Controllers
{
    public class WelcomeController : Controller
    {
        //
        // GET: /Welcome/

        private int collegeId = 1;
        private int positionLevelId = 3;

        private readonly IWelcomeService welcomeService;
        public WelcomeController(IWelcomeService welcomeService) {
            this.welcomeService = welcomeService;
        }
        public ActionResult Index()
        {
            try
            {
              // ViewBag.MenuData = GetMenuString();
                return View();
            }
            catch (Exception exe) { return View(); }
        }

        public ActionResult LandingPage() {
             ViewBag.MenuData = Bootstrap();
            return View();
        }

        private string GetMenuString()
        {
            List<CommonMenuModel> menuList = new List<CommonMenuModel>();
            List<CommonMenuModel> parentList = new List<CommonMenuModel>();
            StringBuilder sb = new StringBuilder();
            List<CommonMenuModel> subList = null;
            bool needChild = false;
            try
            {
                menuList = welcomeService.GetMenuList(collegeId, positionLevelId).ToList();
                if (menuList.Count > 0)
                {
                    parentList = menuList.FindAll(exp => exp.SM_Parent_Id == 0);
                    sb.Append("<div id='menudiv'>");
                    sb.Append("<ul class='udm' id='udm' >");
                    foreach (CommonMenuModel m in parentList)
                    {

                        subList = menuList.FindAll(exp => exp.SM_Parent_Id == m.SM_MenuId).ToList();
                        if (subList != null && subList.Count != 0) { needChild = true; }
                        sb.Append("<li ").Append(m.SM_URL).Append(needChild ? " class='MainMenu' " : "MainMenu").Append(" ><a href='#'>").Append(m.SM_Menu_Name).Append("</a>");
                        if (needChild)
                        {
                            sb.Append("<ul class='temp-menu' >");
                            sb.Append(GetSubMenu(menuList, subList));
                            sb.Append("</ul>");
                            needChild = false;
                        }
                        sb.Append("</li>");
                    }
                    sb.Append("</ul>");
                    sb.Append("</div>");
                }
                return sb.ToString();
            }
            finally { menuList = null; parentList = null; sb = null; subList = null; }
        }

        [HttpPost]
        public ActionResult GetMenuData()
        {
            List<CommonMenuModel> menuList = new List<CommonMenuModel>();
            List<CommonMenuModel> parentList = new List<CommonMenuModel>();
            StringBuilder sb = new StringBuilder();
            List<CommonMenuModel> subList = null;
            bool needChild = false;
            try
            {
                menuList = welcomeService.GetMenuList(collegeId, positionLevelId).ToList();
                if (menuList.Count > 0)
                {
                    parentList = menuList.FindAll(exp => exp.SM_Parent_Id == 0);
                    sb.Append("<div id='menudiv'>");
                    sb.Append("<ul class='udm' id='udm' >");
                    foreach (CommonMenuModel m in parentList)
                    {

                        subList = menuList.FindAll(exp => exp.SM_Parent_Id == m.SM_MenuId).ToList();
                        if (subList != null && subList.Count != 0) { needChild = true; }
                        sb.Append("<li ").Append(m.SM_URL).Append(needChild ? " class='MainMenu' " : "MainMenu").Append(" ><a href='#'>").Append(m.SM_Menu_Name).Append("</a>");
                        if (needChild)
                        {
                            sb.Append("<ul class='temp-menu' >");
                            sb.Append(GetSubMenu(menuList, subList));
                            sb.Append("</ul>");
                            needChild = false;
                        }
                        sb.Append("</li>");
                    }
                    sb.Append("</ul>");
                    sb.Append("</div>");
                }
                return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
            }
            finally { menuList = null; parentList = null; sb = null; subList = null; }
        }

        [HttpPost]
        public ActionResult BootStrapMenuData()
        {
            List<CommonMenuModel> menuList = new List<CommonMenuModel>();
            List<CommonMenuModel> parentList = new List<CommonMenuModel>();
            StringBuilder sb = new StringBuilder();
            List<CommonMenuModel> subList = null;
            bool needChild = false;
            try
            {
                menuList = welcomeService.GetMenuList(collegeId, positionLevelId).ToList();
                if (menuList.Count > 0)
                {
                    parentList = menuList.FindAll(exp => exp.SM_Parent_Id == 0);
                    sb.Append("<div id='sidebar' class='nav-collapse'>");
                    sb.Append("<ul class='sidebar-menu' id='nav-accordion'>");
                    foreach (CommonMenuModel m in parentList)
                    {

                        subList = menuList.FindAll(exp => exp.SM_Parent_Id == m.SM_MenuId).ToList();
                        if (subList != null && subList.Count != 0) { needChild = true; }
                        sb.Append("<li ").Append(m.SM_URL).Append(needChild ? " class='sub-menu' " : "").Append(" ><a href='javascript:;'>");
                        sb.Append("<i  ");
                        sb.Append((m.SM_Class ?? "").Trim() != "" ? "class='" + m.SM_Class + "'" : "");
                        sb.Append(" ></i><span>").Append(m.SM_Menu_Name).Append("</span></a>");
                        if (needChild)
                        {
                            sb.Append("<ul class='sub' >");
                            sb.Append(GetSubMenu(menuList, subList));
                            sb.Append("</ul>");
                            needChild = false;
                        }
                        sb.Append("</li>");
                    }
                    sb.Append("</ul>");
                    sb.Append("</div>");
                }
                return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
            }
            finally { menuList = null; parentList = null; sb = null; subList = null; }
        }
        private string GetSubMenu(List<CommonMenuModel> menuList, List<CommonMenuModel> childList)
        {
            StringBuilder sb = new StringBuilder();
            List<CommonMenuModel> subList = null;
            bool needChild = false;
            try {
                if(menuList.Count==0){return "";}
                foreach (CommonMenuModel m in childList)
                {
                    subList = menuList.FindAll(exp => exp.SM_Parent_Id == m.SM_MenuId).ToList();
                    if (subList != null && subList.Count != 0){ needChild = true; }
                    sb.Append("<li ").Append(m.SM_URL).Append(needChild ? " class='sub-menu' " : "").Append(" ><a href=\"javascript:fnLoadFrame('").Append(m.SM_URL).Append("')\">").Append(m.SM_Menu_Name).Append("</a>");
                    if(needChild){
                        sb.Append("<ul class='sub'>");
                        sb.Append(GetSubMenu(menuList, subList));
                        sb.Append("</ul>");
                        needChild = false;
                    }
                    sb.Append("</li>");
                }
                return sb.ToString();
            }
            finally { sb = null; subList = null; }
        }

        private string Bootstrap() { 
            List<CommonMenuModel> menuList = new List<CommonMenuModel>();
            List<CommonMenuModel> parentList = new List<CommonMenuModel>();
            StringBuilder sb = new StringBuilder();
            List<CommonMenuModel> subList = null;
            bool needChild = false;
            try
            {
                menuList = welcomeService.GetMenuList(collegeId, positionLevelId).ToList();
                if (menuList.Count > 0)
                {
                    parentList = menuList.FindAll(exp => exp.SM_Parent_Id == 0);
                    sb.Append("<div id='sidebar' class='nav-collapse'>");
                    sb.Append("<ul class='sidebar-menu' id='nav-accordion'>");
                    foreach (CommonMenuModel m in parentList)
                    {

                        subList = menuList.FindAll(exp => exp.SM_Parent_Id == m.SM_MenuId).ToList();
                        if (subList != null && subList.Count != 0) { needChild = true; }
                        sb.Append("<li ").Append(m.SM_URL).Append(needChild ? " class='sub-menu' " : "").Append(" ><a href='javascript:;'><i ");
                        sb.Append((m.SM_Class ?? "").Trim() != "" ? "class='"+m.SM_Class+"'" : "");
                        sb.Append(" ></i><span>").Append(m.SM_Menu_Name).Append("</span></a>");
                        if (needChild)
                        {
                            sb.Append("<ul class='sub' >");
                            sb.Append(GetSubMenu(menuList, subList));
                            sb.Append("</ul>");
                            needChild = false;
                        }
                        sb.Append("</li>");
                    }
                    sb.Append("</ul>");
                    sb.Append("</div>");
                }
                return sb.ToString();
            }
            finally { }
        }
    }
}
