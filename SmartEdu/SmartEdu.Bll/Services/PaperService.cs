using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Repositories;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Models;

namespace SmartEdu.Bll.Services
{
    public interface IPaperService {
        object GetGridData(int collegeId, DateTime nDate);
        bool CheckDuplicate(ADM_PAPER model, DateTime cDate);
        void InsertPaper(ADM_PAPER model, long modifiedBy);
        bool UpdatePaper(ADM_PAPER model, long modifiedBy);
        bool DeletePaper(int paperId, int collegeId, DateTime cDate, long modifiedBy);
    }
    public class PaperService : IPaperService 
    {
        private readonly IPaperRepository paperRepository;
        private readonly IUnitOfWork unitOfWork;
        public PaperService(IPaperRepository paperRepository, IUnitOfWork unitOfWork)
        {
            this.paperRepository = paperRepository;
            this.unitOfWork = unitOfWork;
        }

        public object GetGridData(int collegeId,DateTime nDate) {
            IEnumerable<ADM_PAPER> paperList;
            try {
                paperList = paperRepository.GetMany(exp => exp.AP_College_Id == collegeId && (exp.AP_EndDate ?? nDate) >= nDate).ToList();
                if (paperList == null || paperList.Count() == 0) { return new { total_count = 0, rows = new { } }; }
                return (new { 
                    total_count=paperList.Count(),
                    rows=(from p in paperList
                          select new {
                            id=p.AP_Id,
                            data=new string[]{ p.AP_Code,p.AP_ShortName,p.AP_Description,(p.AP_IsPractical?"Yes":"No"),p.AP_StartDate.ToString("dd/MM/yyyy"),(p.AP_EndDate.HasValue?p.AP_EndDate.Value.ToString("dd/MM/yyyy"):"") }
                          }).ToList()
                });
            }
            finally { paperList = null; }
        }

        public bool CheckDuplicate(ADM_PAPER model,DateTime cDate) {
            ADM_PAPER temp = null;
            try {
                if (model.AP_Id == 0) { 
                    temp=paperRepository.Get(exp=>exp.AP_College_Id==model.AP_College_Id && exp.AP_Code==model.AP_Code && (exp.AP_EndDate??cDate)>=cDate);
                }
                else {
                    temp = paperRepository.Get(exp => exp.AP_College_Id == model.AP_College_Id && exp.AP_Code == model.AP_Code && (exp.AP_EndDate ?? cDate) >= cDate && exp.AP_Id!=model.AP_Id);
                }
                return (temp == null);
            }
            finally { temp = null; }
        }

        public void InsertPaper(ADM_PAPER model, long modifiedBy)
        {
            try {
                model.AP_IsActive = true;
                model.AP_StartDate = DateTime.Now.Date;
                model.AP_CreatedBy = modifiedBy;
                model.AP_CreatedDate = DateTime.Now;
                paperRepository.Add(model);
                unitOfWork.Commit();
            }
            finally { }
        }
        public bool UpdatePaper(ADM_PAPER model, long modifiedBy) {
            ADM_PAPER temp = null;
            DateTime cDate=DateTime.Now.Date;
            try { 
                temp=paperRepository.Get(exp=>exp.AP_College_Id==model.AP_College_Id && exp.AP_Id==model.AP_Id && (exp.AP_EndDate??cDate)>=cDate);
                if (temp == null) { return false; }
                temp.AP_Code = model.AP_Code;
                temp.AP_Description = model.AP_Description;
                temp.AP_EndDate = model.AP_EndDate;
                temp.AP_IsPractical = model.AP_IsPractical;
                temp.AP_ShortName = model.AP_ShortName;
                temp.AP_ModifiedBy = modifiedBy;
                temp.AP_ModifiedDate = DateTime.Now;
                paperRepository.Update(temp);
                unitOfWork.Commit();
                return true;
            }
            finally { temp = null; }
        }
        public bool DeletePaper(int paperId, int collegeId, DateTime cDate,long modifiedBy) {
            ADM_PAPER temp = null;
            try {
                temp = paperRepository.Get(exp => exp.AP_College_Id == collegeId && exp.AP_Id == paperId && (exp.AP_EndDate ?? cDate) >= cDate);
                if (temp == null) { return false; }
                temp.AP_ModifiedBy = modifiedBy;
                temp.AP_ModifiedDate = DateTime.Now;
                temp.AP_IsActive = false;
                temp.AP_EndDate = cDate;
                paperRepository.Update(temp);
                unitOfWork.Commit();
                return true;
            }
            finally { temp = null; }
        }
    }
}