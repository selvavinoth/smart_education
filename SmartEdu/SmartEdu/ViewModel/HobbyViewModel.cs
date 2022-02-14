using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;
namespace SmartEdu.ViewModel
{
    public class HobbyViewModel
    {
        public int SHL_ID { get; set; }
        public string SHL_Code { get; set; }
        public string SHL_Description { get; set; }
        public bool SHL_IsActive { get; set; }

        public HobbyViewModel() { }
        public HobbyViewModel(SADM_HOBBIESLIST model) {
            this.SHL_Code = model.SHL_Code;
            this.SHL_Description = model.SHL_Description;
            this.SHL_ID = model.SHL_ID;
            this.SHL_IsActive = model.SHL_IsActive;
        }
    }
}