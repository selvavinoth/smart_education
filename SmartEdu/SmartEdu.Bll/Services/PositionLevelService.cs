using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Repositories;
using SmartEdu.Data.Models;
using SmartEdu.Data.infrastructure;

namespace SmartEdu.Bll.Services
{
    public interface IPositionLevelService 
    {
        object GetGridDataByLevel(int collegId,int parentId);
        bool CheckForDuplication(int collegeId,string shortName, string description);
        void Insert(ADM_POSITION_LEVEL model);
        bool Update(ADM_POSITION_LEVEL model);
        bool Delete(ADM_POSITION_LEVEL model);
        ADM_POSITION_LEVEL GetPositionLevelModel(int Id);
        List<SADM_ROLE> GetRoleList();
    }
    public class PositionLevelService : IPositionLevelService
    {
        private readonly IPositionLevelRepository positionLevelRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUnitOfWork unitOfWork;
        public PositionLevelService(IPositionLevelRepository positionLevelRepository, IRoleRepository roleRepository,IUnitOfWork unitOfWork)
        {
            this.positionLevelRepository = positionLevelRepository;
            this.roleRepository = roleRepository;
            this.unitOfWork = unitOfWork;
        
        }
        public List<SADM_ROLE> GetRoleList() {
            try {
                return roleRepository.GetMany(exp => exp.SR_IsActive).ToList();
            }
            finally { }
        }
        public object GetGridDataByLevel(int collegeId,int parentId)
        {
            IEnumerable<ADM_POSITION_LEVEL> posLevelList;
            List<SADM_ROLE> roleList = null;
            try
            {
                posLevelList = positionLevelRepository.GetMany(exp => exp.APL_College_Id == collegeId && exp.APL_Parent_Id == parentId && exp.APL_IsActive == true).ToList();
                if (posLevelList == null) { return null; }
                roleList = GetRoleList();
                return new { 
                    total_count=posLevelList.Count(),
                    rows=(from p in posLevelList
                          join r in roleList on p.APL_Role_Id equals r.SR_Id
                          select new {
                            id=p.APL_ID,
                            data=new string[]{p.APL_Parent_Id.ToString(),p.APL_ShortName,p.APL_Description,((p.APL_IsBaseLevel??false)?"True":"False"),(p.APL_BaseLevel_Id??0).ToString(),r.SR_Description,r.SR_Id.ToString()}
                          }).ToList()
                };
            }
            finally { posLevelList = null; roleList = null; }
        }
        public bool CheckForDuplication(int collegeId,string shortName, string description)
        {
            ADM_POSITION_LEVEL temp = new ADM_POSITION_LEVEL();
            try
            {
                temp = positionLevelRepository.Get(exp => exp.APL_College_Id==collegeId && exp.APL_ShortName.ToUpper() == shortName && exp.APL_Description == description && exp.APL_IsActive == true);
                return (temp == null);
            }
            finally { temp = null; }
        }
        public void Insert(ADM_POSITION_LEVEL model)
        {
            try {
                model.APL_IsActive = true;
                SetBaseLevel(model.APL_Parent_Id, model);
                positionLevelRepository.Add(model);
                unitOfWork.Commit();
            }
            finally { }
        }
        public bool Update(ADM_POSITION_LEVEL model)
        {
            ADM_POSITION_LEVEL temp = new ADM_POSITION_LEVEL();
            try {
                temp = positionLevelRepository.Get(exp => exp.APL_ID == model.APL_ID && exp.APL_IsActive == true);
                if (temp != null)
                {
                    temp.APL_ShortName = model.APL_ShortName;
                    temp.APL_Description = model.APL_Description;
                 //   temp.APL_BaseLevel_Id = model.APL_BaseLevel_Id;
                  //  temp.APL_IsBaseLevel = model.APL_IsBaseLevel;
                    temp.APL_Role_Id = model.APL_Role_Id;
                    positionLevelRepository.Update(temp);
                    unitOfWork.Commit();
                    return true;
                }
                return false;
            }
            finally { temp = null; }
        }
        public bool Delete(ADM_POSITION_LEVEL temp)
        {
            try
            {
                if (temp != null)
                {
                    temp.APL_IsActive = false;
                    positionLevelRepository.Update(temp);
                    unitOfWork.Commit();
                    return true;
                }
                return false;
            }
            finally { temp = null; }
        }
        public ADM_POSITION_LEVEL GetPositionLevelModel(int id)
        {
            try {
                return positionLevelRepository.Get(exp => exp.APL_ID == id && exp.APL_IsActive == true);
            }
            finally { }
        }

        private void SetBaseLevel(int parentId,ADM_POSITION_LEVEL model) {
            List<ADM_POSITION_LEVEL> levelList = null;
            ADM_POSITION_LEVEL temp = null;
            try {
                levelList = positionLevelRepository.GetMany(exp => exp.APL_Parent_Id == parentId && exp.APL_College_Id == model.APL_College_Id && exp.APL_IsActive == true).ToList();
                if (levelList.Count == 0) { model.APL_IsBaseLevel = true; }
                else {
                    temp = levelList.Find(exp => exp.APL_IsBaseLevel == true);
                    model.APL_BaseLevel_Id = (temp == null ? 0 : temp.APL_ID);
                }
            }
            finally { levelList = null; temp = null; }
        }
    }
}