using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Repositories;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Models;

namespace SmartEdu.Bll.Services
{
    public interface IFieldLevelValidationService {
        CONFIG_FIELDLEVEL_VALIDATION GetFieldLevelConfig(int collegeId,int departmentId,string viewModelName);
        void InsertFieldLevelConfig(CONFIG_FIELDLEVEL_VALIDATION model, long createdBy);
        bool UpdateFieldLevelConfig(CONFIG_FIELDLEVEL_VALIDATION model, long modifiedBy);
        bool DeleteFieldLevelConfig(int cfv_Id, long modifiedBy);
    }

    public class FieldLevelValidationService : IFieldLevelValidationService
    {
        private readonly IFieldLevelValidationConfigRepository fieldLevelValidationConfigRepository;
        private readonly IUnitOfWork unitOfWork;
        public FieldLevelValidationService(IFieldLevelValidationConfigRepository fieldLevelValidationConfigRepository, IUnitOfWork unitOfWork)
        {
            this.fieldLevelValidationConfigRepository = fieldLevelValidationConfigRepository;
            this.unitOfWork = unitOfWork;
        }

        public CONFIG_FIELDLEVEL_VALIDATION GetFieldLevelConfig(int collegeId,int departmentId,string viewModelName) {
            try {
                return fieldLevelValidationConfigRepository.Get(exp => exp.CFV_College_Id==collegeId && exp.CFV_DP_Id==departmentId && exp.CFV_ViewModelName == viewModelName && exp.CFV_IsActive);
            }
            finally { }
        }
        public void InsertFieldLevelConfig(CONFIG_FIELDLEVEL_VALIDATION model, long createdBy) {
            try {
                model.CFV_CreatedBy = createdBy;
                model.CFV_CreatedDate = DateTime.Now.Date;
                model.CFV_Status = "I";
                model.CFV_IsActive = true;
                fieldLevelValidationConfigRepository.Add(model);
                unitOfWork.Commit();
            }
            finally { }
        }
        public bool UpdateFieldLevelConfig(CONFIG_FIELDLEVEL_VALIDATION model, long modifiedBy)
        {
            CONFIG_FIELDLEVEL_VALIDATION temp = null;
            try
            {
                temp = fieldLevelValidationConfigRepository.Get(exp => exp.CFV_College_Id == model.CFV_College_Id && exp.CFV_DP_Id == model.CFV_DP_Id && exp.CFV_ID == model.CFV_ID && exp.CFV_IsActive);
                if (temp == null) { return false; }
                temp.CFV_ValidationString = model.CFV_ValidationString;
                temp.CFV_ModifiedBy = modifiedBy;
                temp.CFV_ModifiedDate = DateTime.Now.Date;
                temp.CFV_Status = "U";
                fieldLevelValidationConfigRepository.Update(temp);
                unitOfWork.Commit();
                return true;
            }
            finally { temp = null; }
        }
        public bool DeleteFieldLevelConfig(int cfv_Id, long modifiedBy) {
            CONFIG_FIELDLEVEL_VALIDATION temp = null;
            try
            {
                temp = fieldLevelValidationConfigRepository.Get(exp => exp.CFV_ID == cfv_Id && exp.CFV_IsActive);
                if (temp == null) { return false; }
                temp.CFV_ModifiedBy = modifiedBy;
                temp.CFV_ModifiedDate = DateTime.Now.Date;
                temp.CFV_Status = "U";
                temp.CFV_IsActive = false;
                fieldLevelValidationConfigRepository.Update(temp);
                unitOfWork.Commit();
                return true;
            }
            finally { temp = null; }
        }
    }
}