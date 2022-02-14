using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Repositories;
using SmartEdu.Data.Models;
using SmartEdu.Data.infrastructure;

namespace SmartEdu.Bll.Services
{
    public interface IRoleService {
        object GetRoleGridData();
        bool CheckForDublication(int id, string code);
        void Insert(SADM_ROLE model);
        bool Update(SADM_ROLE model);
        bool Delete(int id);
    }
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository roleRepository;
        private readonly IUnitOfWork unitOfWork;
        public RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            this.roleRepository = roleRepository;
            this.unitOfWork = unitOfWork;
        }
        public object GetRoleGridData()
        {
            IEnumerable<SADM_ROLE> roleList;
            try
            {
                roleList = roleRepository.GetMany(exp => exp.SR_IsActive == true).ToList();
                if (roleList == null) { return null; }
                return new
                {
                    total_count = roleList.Count(),
                    rows = (from r in roleList
                            select new
                            {
                                id = r.SR_Id,
                                data = new string[] { r.SR_Code, r.SR_Description }
                            }).ToList()

                };
            }
            finally { roleList = null; }
        }
        public bool CheckForDublication(int id,string code)
        {
            SADM_ROLE model = null;
            try
            {
                if(id==0)
                    model = roleRepository.Get(exp => exp.SR_Code == code && exp.SR_IsActive == true);
                else
                     model = roleRepository.Get(exp => exp.SR_Code == code && exp.SR_Id!=id && exp.SR_IsActive == true);
                return (model == null);
            }
            finally { model = null; }
        }
        public void Insert(SADM_ROLE model)
        {
            try
            {
                model.SR_IsActive = true;
                model.SR_Status = "I";
                roleRepository.Add(model);
                unitOfWork.Commit();
            }
            finally { }
        }
        public bool Update(SADM_ROLE model)
        {
            SADM_ROLE temp = new SADM_ROLE();
            try
            {
                temp = roleRepository.Get(exp => exp.SR_Id == model.SR_Id && exp.SR_IsActive == true);
                if (temp == null) { return false; }
                temp.SR_Code = model.SR_Code;
                temp.SR_Description = model.SR_Description;
                temp.SR_Status = "U";
                roleRepository.Update(temp);
                unitOfWork.Commit();
                return true;
            }
            finally { temp = null; }
        }
        public bool Delete(int id)
        {
            SADM_ROLE temp = new SADM_ROLE();
            try
            {
                temp = roleRepository.Get(exp => exp.SR_Id == id && exp.SR_IsActive == true);
                if (temp == null) { return false; }
                temp.SR_IsActive = false;
                temp.SR_Status = "U";
                roleRepository.Update(temp);
                unitOfWork.Commit();
                return true;
            }
            finally { temp = null; }
        }
    }
}