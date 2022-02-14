using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Repositories;
using SmartEdu.Data.Models;

namespace SmartEdu.Bll.Services
{
    public class ConfigService
    {
        private readonly IFieldLevelValidationConfigRepository fieldLevelValidationConfigRepository;
        public ConfigService(IFieldLevelValidationConfigRepository fieldLevelValidationConfigRepository) {
            this.fieldLevelValidationConfigRepository = fieldLevelValidationConfigRepository;
        }
        public CONFIG_FIELDLEVEL_VALIDATION GetFieldLevelConfig(string viewModelName) {
            try {
                return fieldLevelValidationConfigRepository.Get(exp => exp.CFV_ViewModelName == viewModelName && exp.CFV_IsActive);
            }
            finally { }
        }
    }
}