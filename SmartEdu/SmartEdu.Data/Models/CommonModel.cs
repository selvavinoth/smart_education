using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartEdu.Data.Models
{
    public class CommonMenuModel
    {
        public int SM_MenuId { get; set; }
        public string SM_Menu_Name { get; set; }
        public string SM_Task_Name { get; set; }
        public string SM_URL { get; set; }
        public int SM_Parent_Id { get; set; }
        public bool SM_IsComponent { get; set; }
        public string SM_Class { get; set; }
    }
}