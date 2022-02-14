using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;
using SmartEdu.Data.Repositories;

namespace SmartEdu.Bll.Services
{
    public interface ISharedService {
        List<ADM_LOV> GetLovList(int collegeId, List<string> lovTypeList);
        List<ADM_COLLEGE> GetCollegeList();
        List<ADM_DEPARTMENTS> GetDepartmentList(int collegeId);
    }
    public class SharedService : ISharedService
    {
        private readonly IAdmLOVRepository aLOVRepository;
        private readonly ICollegeRepository collegeRepository;
        private readonly IDepartmentsRepository departmentsRepository;
        public SharedService(IAdmLOVRepository aLOVRepository, ICollegeRepository collegeRepository, IDepartmentsRepository departmentsRepository)
        {
            this.aLOVRepository = aLOVRepository;
            this.collegeRepository = collegeRepository;
            this.departmentsRepository = departmentsRepository;
        }
        public List<ADM_LOV> GetLovList(int collegeId, List<string> lovTypeList)
        {
            try {
                return aLOVRepository.GetMany(exp => lovTypeList.Contains(exp.AL_Type) && exp.AL_IsActive).ToList();
            }
            finally { }
        }
        public List<ADM_COLLEGE> GetCollegeList() {
            try {
                return collegeRepository.GetMany(exp => exp.ACE_IsActive == true).ToList();
            }
            finally { }
        }
        public List<ADM_DEPARTMENTS> GetDepartmentList(int collegeId) {
            try
            {
                return departmentsRepository.GetMany(exp => exp.ADP_CollegeId == collegeId && exp.ADP_IsActive == true).ToList();
            }
            finally { }
        }
    }
}