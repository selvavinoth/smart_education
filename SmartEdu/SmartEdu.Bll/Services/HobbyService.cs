using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Repositories;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Models;

namespace SmartEdu.Bll.Services
{
    public interface IHobbyService
    {
        object GetHobbyGridData();
        bool CheckForDublication(int id,string code);
        void Insert(SADM_HOBBIESLIST model);
        bool Update(SADM_HOBBIESLIST model);
        bool Delete(int id);
    }
    public class HobbyService : IHobbyService
    {
        private readonly IHobbyRepository hobbyRepository;
        private readonly IUnitOfWork unitOfWork;
        public HobbyService(IHobbyRepository hobbyRepository, IUnitOfWork unitOfWork)
        {
            this.hobbyRepository = hobbyRepository;
            this.unitOfWork = unitOfWork;
        }
        public object GetHobbyGridData(){
            IEnumerable<SADM_HOBBIESLIST> hobbyList;
            try
            {
                hobbyList = hobbyRepository.GetMany(exp => exp.SHL_IsActive == true).ToList();
                if (hobbyList == null) { return null; }
                return new { 
                    total_count=hobbyList.Count(),
                    rows=(from h in hobbyList
                          select new{
                            id=h.SHL_ID,
                            data=new string[]{h.SHL_Code,h.SHL_Description}
                          }).ToList()
                
                };
            }
            finally { }
        
        }
        public bool CheckForDublication(int id, string code)
        {
            SADM_HOBBIESLIST model = null;
            try {
                if(id==0)
                    model = hobbyRepository.Get(exp => exp.SHL_Code == code && exp.SHL_IsActive == true);
                else
                    model = hobbyRepository.Get(exp => exp.SHL_Code == code && exp.SHL_ID!=id && exp.SHL_IsActive == true);
                return (model == null);
            }
            finally {
                model = null;
            }
        }
        public void Insert(SADM_HOBBIESLIST model){
            try {
                model.SHL_IsActive = true;
                hobbyRepository.Add(model);
                unitOfWork.Commit();
            }
            finally { }
        }
        public bool Update(SADM_HOBBIESLIST model){
            SADM_HOBBIESLIST temp = new SADM_HOBBIESLIST();
            try
            {
                temp = hobbyRepository.Get(exp => exp.SHL_ID == model.SHL_ID && exp.SHL_IsActive == true);
                if (temp != null) {
                    temp.SHL_Code = model.SHL_Code;
                    temp.SHL_Description = model.SHL_Description;
                    hobbyRepository.Update(temp);
                    unitOfWork.Commit();
                    return true;
                }
                return false;
            }
            finally { temp = null; }
        }
        public bool Delete(int id) {
            SADM_HOBBIESLIST temp = new SADM_HOBBIESLIST();
            try
            {
                temp = hobbyRepository.Get(exp => exp.SHL_ID == id && exp.SHL_IsActive == true);
                if (temp != null)
                {
                    temp.SHL_IsActive = false;
                    hobbyRepository.Update(temp);
                    unitOfWork.Commit();
                    return true;
                }
                return false;
            }
            finally { temp = null; }
        }
    }
}