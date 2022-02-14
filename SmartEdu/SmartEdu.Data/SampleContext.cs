using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SmartEdu.Data.Models;

namespace SmartEdu.Data
{
    public class SampleContext : DbContext
    {
        // SADM Related DataSet
        public DbSet<SADM_ROLE> role { get; set; }
        public DbSet<SADM_TASK> task { get; set; }
        public DbSet<SADM_MENU> menu { get; set; }
        public DbSet<SADM_ROLE_TASK_MAPPING> roleTaskMapping { get; set; }

        public DbSet<ADM_LOV> admLov { get; set; }
        public DbSet<SADM_HOBBIESLIST> hobby { get; set; }
        public DbSet<ADM_POSITION_LEVEL> posLevel { get; set; }
        public DbSet<ADM_POSITIONS> position { get; set; }
        public DbSet<ADM_POSITION_STAFF_MAPPING> positionStaffMapping { get; set; }
        public DbSet<ADM_USERS> user { get; set; }
        public DbSet<ADM_USER_AUTHENTICATION> userAuthentication { get; set; }
        public DbSet<ADM_USER_ADDRESS> userAddress { get; set; }
        
        public DbSet<ADM_PAPER> paper { get; set; }
        public DbSet<ADM_CLASSES> classes { get; set; }
        public DbSet<ADM_BATCH> batch { get; set; }
        public DbSet<ADM_SEMESTER> semester { get; set; }
        public DbSet<ADM_SEMESTER_PAPER_MAPPING> semPaperMapping { get; set; }

        public DbSet<ADM_STAFF_DETAILS> staffDetails { get; set; }
        public DbSet<ADM_STAFF_ADDRESS> staffAddress { get; set; }
        public DbSet<ADM_STUDENT_DETAILS> studentDetails { get; set; }
        public DbSet<ADM_STUDENT_ADDRESS> studentAddress { get; set; }
        public DbSet<ADM_PARENT_DETAILS> parentDetails { get; set; }

        public DbSet<ADM_USER_DETAILS> userDetails { get; set; }
        public DbSet<ADM_DEPARTMENTS> departments { get; set; }
        public DbSet<ADM_COLLEGE> college { get; set; }
        public DbSet<ADM_DEPARTMENT_CONTACT_PERSON> departmetContactPerson { get; set; }
        public DbSet<ADM_COLLEGE_CONTACT_PERSON> collegeContactPerson { get; set; }


        #region Config Models
        public DbSet<CONFIG_FIELDLEVEL_VALIDATION> ConfigFielLevelValidation { get; set; }
        #endregion
        public virtual void Commit()
        {
            base.SaveChanges();
        }
    }
}