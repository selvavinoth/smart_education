using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SmartEdu.Data.Models
{
    public class SADM_TASK
    {
        [Key]
        public int ST_Id { get; set; }
        public string ST_Task_Name { get; set; }
        public string ST_URL { get; set; }
        public bool ST_IsActive { get; set; }
        public string ST_Status { get; set; }
    }
    public class SADM_ROLE
    {
        [Key]
        public int SR_Id { get; set; }
        public string SR_Code { get; set; }
        public string SR_Description { get; set; }
        public bool SR_IsActive { get; set; }
        public string SR_Status { get; set; }
    }
    public class SADM_MENU
    {
        [Key]
        public int SM_Id { get; set; }
        public int SM_College_Id { get; set; }
        public int SM_Parent_Id { get; set; }
        public string SM_Menu_Name { get;set;}
        public int SM_Role_Id { get; set; }
        public int SM_Task_Id { get; set; }
        public bool SM_IsComponent { get; set; }
        public bool SM_IsActive { get; set; }
        public string SM_Status { get; set; }
        public string SM_Class { get; set; }
    }
    public class SADM_ROLE_TASK_MAPPING {
        [Key]
        public int SRTM_Id { get; set; }
        public int SRTM_Role_Id { get; set; }
        public int SRTM_Task_Id { get; set; }
        public bool ? SRTM_IsActive { get; set; }
        public string SRTM_Status { get; set; }
    }

    public class SADM_HOBBIESLIST
    {
        [Key]
        public int SHL_ID { get; set; }
        public string SHL_Code { get; set; }
        public string SHL_Description { get; set; }
        public bool SHL_IsActive { get; set; }
    }
}