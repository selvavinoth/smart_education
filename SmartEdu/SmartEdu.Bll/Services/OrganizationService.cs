using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Repositories;
using SmartEdu.Data.Models;

namespace SmartEdu.Bll.Services
{
    public interface IOrganizationService {
        // College
        object GetCollegeGridData();
        ADM_COLLEGE GetCollege(int collegeId);
        ADM_COLLEGE_CONTACT_PERSON GetCollgeContactPerson(int collegeId);
        List<ADM_COLLEGE_CONTACT_PERSON> GetCollegeContactPersonList();
        void InsertCollege(ADM_COLLEGE model, List<ADM_COLLEGE_CONTACT_PERSON> contactPerson,ADM_USER_DETAILS userDetails, long createdBy);
        bool UpdateCollege(ADM_COLLEGE model, List<ADM_COLLEGE_CONTACT_PERSON> contactPerson, long modifiedBy, bool isNewContactPerson);
        bool DeleteCollege(int collegeId, long modifiedBy);

        // Department
        object GetDepartmentGridData(int collegeId);
        ADM_DEPARTMENTS GetDepartment(int departmentId);
        ADM_DEPARTMENT_CONTACT_PERSON GetDepartmentContactPerson(int collegeId, int departmentId);
        List<ADM_DEPARTMENT_CONTACT_PERSON> GetDepartmentContactPersonList(int collegeId);

        void InsertDepartment(ADM_DEPARTMENTS model, List<ADM_DEPARTMENT_CONTACT_PERSON> contactPerson, ADM_USER_DETAILS userDetails, long createdBy);
        bool UpdateDepartment(ADM_DEPARTMENTS model, List<ADM_DEPARTMENT_CONTACT_PERSON> contactPersonModel, long modifiedBy, bool isNewContactPerson);
        bool DeleteDepartment(int collegeId, int departmentId, long modifiedBy);

        ADM_USER_DETAILS GetUserDetails(int orgId, int userId, DateTime curentDate);
        List<SADM_ROLE> GetRoleList();
    }
    public class OrganizationService : IOrganizationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDepartmentsRepository departmentsRepository;
        private readonly IDepartmentContactPersonRepository departmentContactPersonRepository;
        private readonly ICollegeRepository collegeRepository;
        private readonly ICollegeContactPersonRepository collegeContactPersonRepository;
        private readonly IUserDetailsRepository userDetailsRepository;
        private readonly IRoleRepository roleRepository;
        public OrganizationService(ICollegeRepository collegeRepository, ICollegeContactPersonRepository collegeContactPersonRepository, IDepartmentsRepository departmentsRepository, IDepartmentContactPersonRepository departmentContactPersonRepository, IUserDetailsRepository userDetailsRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            this.collegeRepository = collegeRepository;
            this.collegeContactPersonRepository = collegeContactPersonRepository;
            this.departmentsRepository = departmentsRepository;
            this.departmentContactPersonRepository = departmentContactPersonRepository;
            this.userDetailsRepository = userDetailsRepository;
            this.roleRepository = roleRepository;
            this.unitOfWork = unitOfWork;
        }

        private DateTime currentDate = DateTime.Now.Date;

        // College
        #region Methods related to college
        public object GetCollegeGridData()
        {
            List<ADM_COLLEGE> CollegeList;
            List<ADM_COLLEGE_CONTACT_PERSON> CcpList;
            try
            {
                CollegeList = GetCollegeList();
                CcpList = GetCollegeContactPersonList();
                if (CollegeList.Count == 0 || CcpList.Count == 0) { return new { total_count = 0, rows = new { } }; }
                return new
                {
                    total_count = CollegeList.Count,
                    rows = (from d in CollegeList
                            join dc in CcpList on d.ACE_Id equals dc.ACCP_College_Id
                            select new
                            {
                                id = d.ACE_Id,
                                data = new string[] { d.ACE_Code, d.ACE_ShortDescription, d.ACE_Description, dc.ACCP_ContactPerson, d.ACE_Website, d.ACE_MobileNo, d.ACE_StartDate.ToString("dd/MM/yyyy"), (d.ACE_EndDate.HasValue ? d.ACE_EndDate.Value.ToString("dd/MM/yyyy") : "") }
                            }).ToList()
                };

            }
            finally { CollegeList = null; CcpList = null; }
        }
        public ADM_COLLEGE GetCollege(int collegeId) {
            try { return collegeRepository.Get(exp => exp.ACE_Id == collegeId && exp.ACE_IsActive); }
            finally { }
        }
        public ADM_COLLEGE_CONTACT_PERSON GetCollgeContactPerson(int collegeId) {
            try {
                return collegeContactPersonRepository.Get(exp => exp.ACCP_College_Id == collegeId && exp.ACCP_StartDate <= currentDate && exp.ACCP_EndDate >= currentDate);
            }
            finally { }
        }

        private List<ADM_COLLEGE> GetCollegeList()
        {
            try
            {
                return collegeRepository.GetMany(exp => exp.ACE_IsActive).ToList();
            }
            finally { }
        }
        public List<ADM_COLLEGE_CONTACT_PERSON> GetCollegeContactPersonList()
        {
            try { return collegeContactPersonRepository.GetMany(exp => exp.ACCP_StartDate <= currentDate && (exp.ACCP_EndDate??currentDate) >= currentDate).ToList(); }
            finally { }
        }

        #region College IU Action
        public void InsertCollege(ADM_COLLEGE model, List<ADM_COLLEGE_CONTACT_PERSON> contactPerson, ADM_USER_DETAILS userDetails, long createdBy)
        {
            try
            {
                model.ACE_Created_By = createdBy;
                model.ACE_CreatedDate = currentDate;
                model.ACE_IsActive = true;
                model.ACE_Status = "I";
                collegeRepository.Add(model);
                unitOfWork.Commit();
                InsertCollegeContactPerson(model.ACE_Id, contactPerson, createdBy, currentDate);
                InsertUserAuthentication(userDetails,(int) model.ACE_Id, 0, model.ACE_StartDate, null, createdBy);
                unitOfWork.Commit();
            }
            finally { }
        }
        private void InsertCollegeContactPerson(long collgeId, List<ADM_COLLEGE_CONTACT_PERSON> modelList, long createdBy, DateTime createdDate)
        {
            ADM_COLLEGE_CONTACT_PERSON contactPerson = null;
            try
            {
                foreach (ADM_COLLEGE_CONTACT_PERSON model in modelList)
                {
                    contactPerson = new ADM_COLLEGE_CONTACT_PERSON();
                    contactPerson.ACCP_City = model.ACCP_City;
                    contactPerson.ACCP_ContactPerson = model.ACCP_ContactPerson;
                    contactPerson.ACCP_Country = model.ACCP_Country;
                    contactPerson.ACCP_EmailId = model.ACCP_EmailId;
                    contactPerson.ACCP_FaxNo = model.ACCP_FaxNo;
                    contactPerson.ACCP_PhoneNumber = model.ACCP_PhoneNumber;
                    contactPerson.ACCP_Pincode = model.ACCP_Pincode;
                    contactPerson.ACCP_ProfileImage = model.ACCP_ProfileImage;
                    contactPerson.ACCP_Qualification = model.ACCP_Qualification;
                    contactPerson.ACCP_Created_By = createdBy;
                    contactPerson.ACCP_Created_Date = createdDate;
                    contactPerson.ACCP_IsActive = true;
                    contactPerson.ACCP_Status = "I";
                    contactPerson.ACCP_College_Id = (int)collgeId;
                    contactPerson.ACCP_StartDate = DateTime.Now.Date;
                    collegeContactPersonRepository.Add(contactPerson);
                }
            }
            finally { contactPerson = null; }
        }

        public bool UpdateCollege(ADM_COLLEGE model, List<ADM_COLLEGE_CONTACT_PERSON> contactPerson, long modifiedBy, bool isNewContactPerson)
        {
            ADM_COLLEGE temp = null;
            try
            {
                temp = GetCollege((int)model.ACE_Id);
                if (model == null) { return false; }
                temp.ACE_Code = model.ACE_Code;
                temp.ACE_CollegeLogo = model.ACE_CollegeLogo;
                temp.ACE_Description = model.ACE_Description;
                temp.ACE_EmailId = model.ACE_EmailId;
                temp.ACE_FaxNo = model.ACE_FaxNo;
                temp.ACE_History = model.ACE_History;
                temp.ACE_MobileNo = model.ACE_MobileNo;
                temp.ACE_PhoneNo = model.ACE_PhoneNo;
                temp.ACE_ShortDescription = model.ACE_ShortDescription;
                temp.ACE_Website = model.ACE_Website;
                temp.ACE_Modified_By = modifiedBy;
                temp.ACE_ModifiedDate = currentDate;
                temp.ACE_Status = "U";
                collegeRepository.Update(temp);
                CollegeContactPersonIUAction((int)temp.ACE_Id, contactPerson, modifiedBy, isNewContactPerson);
                unitOfWork.Commit();
                return true;
            }
            finally { temp = null; }
        }
        private void CollegeContactPersonIUAction(int collegeId, List<ADM_COLLEGE_CONTACT_PERSON> modelList, long modifiedBy, bool isNewCp)
        {
            ADM_COLLEGE_CONTACT_PERSON temp = null;
            try
            {
                foreach (ADM_COLLEGE_CONTACT_PERSON model in modelList)
                {
                    temp = collegeContactPersonRepository.Get(exp => exp.ACCP_Id == model.ACCP_Id && exp.ACCP_College_Id == collegeId && exp.ACCP_IsActive);
                    if (temp == null) { return; }
                    if (isNewCp && temp.ACCP_StartDate < currentDate)
                    {
                        CollegeContactPersonUpdate(model, modifiedBy, currentDate);
                        InsertCollegeContactPerson(model.ACCP_College_Id, new List<ADM_COLLEGE_CONTACT_PERSON>(){model}, modifiedBy, currentDate.AddDays(1));
                    }
                    else { CollegeContactPersonUpdate(model, modifiedBy, null); }
                }
            }
            finally { temp = null; }
        }
        private void CollegeContactPersonUpdate(ADM_COLLEGE_CONTACT_PERSON model, long modifiedBy, DateTime? endDate)
        {
            ADM_COLLEGE_CONTACT_PERSON contactPerson = new ADM_COLLEGE_CONTACT_PERSON();
            try
            {
                contactPerson.ACCP_ContactPerson = model.ACCP_ContactPerson;
                contactPerson.ACCP_College_Id = model.ACCP_College_Id;
                contactPerson.ACCP_City = model.ACCP_City;
                contactPerson.ACCP_Country = model.ACCP_Country;
                contactPerson.ACCP_EmailId = model.ACCP_EmailId;
                contactPerson.ACCP_FaxNo = model.ACCP_FaxNo;
                contactPerson.ACCP_PhoneNumber = model.ACCP_PhoneNumber;
                contactPerson.ACCP_Pincode = model.ACCP_Pincode;
                contactPerson.ACCP_ProfileImage = model.ACCP_ProfileImage;
                contactPerson.ACCP_Qualification = model.ACCP_Qualification;
                contactPerson.ACCP_Modified_By = modifiedBy;
                contactPerson.ACCP_EndDate = endDate;
                contactPerson.ACCP_Status = "U";
                collegeContactPersonRepository.Update(contactPerson);
            }
            finally { contactPerson = null; }
        }

        public bool DeleteCollege(int collegeId, long modifiedBy)
        {
            ADM_COLLEGE model = null;
            try
            {
                model = GetCollege(collegeId);
                if (model == null) { return false; }
                model.ACE_Modified_By = modifiedBy;
                model.ACE_ModifiedDate = currentDate;
                model.ACE_EndDate = currentDate;
                model.ACE_IsActive = false;
                model.ACE_Status = "U";
                collegeRepository.Update(model);
                unitOfWork.Commit();
                DeleteCollegeContactPerson(collegeId, modifiedBy);
                return true;
            }
            finally { model = null; }
        }
        private void DeleteCollegeContactPerson(int collegeId,long modifiedBy)
        {
            List<ADM_COLLEGE_CONTACT_PERSON> CcList = null;
            try
            {
                CcList = collegeContactPersonRepository.GetMany(exp => exp.ACCP_College_Id == collegeId && exp.ACCP_IsActive).ToList();
                if (CcList.Count == 0) { return; }
                foreach (ADM_COLLEGE_CONTACT_PERSON model in CcList)
                {
                    model.ACCP_Modified_By = modifiedBy;
                    model.ACCP_Modified_Date = currentDate;
                    model.ACCP_EndDate = currentDate;
                    model.ACCP_IsActive = false;
                    model.ACCP_Status = "U";
                    collegeContactPersonRepository.Update(model);
                }
            }
            finally { CcList = null; }
        }

        
        #endregion

        #endregion

        // Department
        #region Methods related to department
        public object GetDepartmentGridData(int collegeId)
        {
            List<ADM_DEPARTMENTS> DpList;
            List<ADM_DEPARTMENT_CONTACT_PERSON> DpcList;
            List<ADM_COLLEGE> CollegeList;
            try
            {
                DpList = GetDepartmentList(collegeId);
                DpcList = GetDepartmentContactPersonList(collegeId);
                CollegeList = GetCollegeList();
                if (DpcList.Count == 0 || DpcList.Count == 0 || CollegeList.Count == 0) { return new { total_count = 0, rows = new { } }; }
                return new
                {
                    total_count = DpList.Count,
                    rows = (from d in DpList
                            join dc in DpcList on d.ADP_Id equals dc.ADCP_Department_Id
                            join c in CollegeList on d.ADP_CollegeId equals c.ACE_Id
                            select new
                            {
                                id = d.ADP_Id,
                                data = new string[] { d.ADP_Code, d.ADP_ShortDescription, d.ADP_Description, dc.ADCP_ContactPerson, d.ADP_Website, d.ADP_MobileNo, d.ADP_StartDate.ToString("dd/MM/yyyy"), (d.ADP_EndDate.HasValue ? d.ADP_EndDate.Value.ToString("dd/MM/yyyy") : "") }
                            }).ToList()
                };

            }
            finally { DpList = null; DpcList = null; CollegeList = null; }
        }
        public ADM_DEPARTMENTS GetDepartment(int departmentId)
        {
            try
            {
                return departmentsRepository.Get(exp => exp.ADP_Id == departmentId && exp.ADP_IsActive);
            }
            finally { }
        }
        private ADM_DEPARTMENTS GetDepartment(int collegeId,int departmentId) {
            try {
                return departmentsRepository.Get(exp => exp.ADP_Id == departmentId && exp.ADP_CollegeId == collegeId && exp.ADP_IsActive);
            }
            finally { }
        }
        public ADM_DEPARTMENT_CONTACT_PERSON GetDepartmentContactPerson(int collegeId, int departmentId) {
            try {
                return departmentContactPersonRepository.Get(exp => exp.ADCP_College_Id == collegeId && exp.ADCP_Department_Id == departmentId && exp.ADCP_StartDate <= currentDate && exp.ADCP_EndDate >= currentDate);
            }
            finally { }
        }

        private List<ADM_DEPARTMENTS> GetDepartmentList(int collegeId)
        {
            try
            {
                return departmentsRepository.GetMany(exp =>exp.ADP_CollegeId == collegeId && exp.ADP_IsActive).ToList();
            }
            finally { }
        }
        public List<ADM_DEPARTMENT_CONTACT_PERSON> GetDepartmentContactPersonList(int collegeId)
        {
            try
            {
                return departmentContactPersonRepository.GetMany(exp => exp.ADCP_College_Id == collegeId && exp.ADCP_StartDate <= currentDate && (exp.ADCP_EndDate??currentDate) >= currentDate).ToList();
            }
            finally { }
        }

        #region Department IUD Action
        public void InsertDepartment(ADM_DEPARTMENTS model, List<ADM_DEPARTMENT_CONTACT_PERSON> contactPerson, ADM_USER_DETAILS userDetails, long createdBy)
        {
            try {
                model.ADP_Created_By = createdBy;
                model.ADP_CreatedDate = currentDate;
                model.ADP_IsActive = true;
                model.ADP_Status = "I";
                departmentsRepository.Add(model);
                unitOfWork.Commit();
                InsertDepartmentContactPerson(model.ADP_Id, contactPerson, createdBy,currentDate);
                InsertUserAuthentication(userDetails, model.ADP_CollegeId, model.ADP_Id, model.ADP_StartDate, null, createdBy);
                unitOfWork.Commit();
            }
            finally { }
        }
        private void InsertDepartmentContactPerson(long dpId, List<ADM_DEPARTMENT_CONTACT_PERSON> modelList, long createdBy, DateTime createdDate)
        {
            ADM_DEPARTMENT_CONTACT_PERSON contactPerson = null;
            try {
                foreach (ADM_DEPARTMENT_CONTACT_PERSON model in modelList)
                {
                    contactPerson = new ADM_DEPARTMENT_CONTACT_PERSON();
                    contactPerson.ADCP_ContactPerson = model.ADCP_ContactPerson;
                    contactPerson.ADCP_College_Id = model.ADCP_College_Id;
                    contactPerson.ADCP_City = model.ADCP_City;
                    contactPerson.ADCP_Country = model.ADCP_Country;
                    contactPerson.ADCP_EmailId = model.ADCP_EmailId;
                    contactPerson.ADCP_FaxNo = model.ADCP_FaxNo;
                    contactPerson.ADCP_PhoneNumber = model.ADCP_PhoneNumber;
                    contactPerson.ADCP_Pincode = model.ADCP_Pincode;
                    contactPerson.ADCP_ProfileImage = model.ADCP_ProfileImage;
                    contactPerson.ADCP_Qualification = model.ADCP_Qualification;
                    contactPerson.ADCP_Created_By = createdBy;
                    contactPerson.ADCP_Created_Date = createdDate;
                    contactPerson.ADCP_IsActive = true;
                    contactPerson.ADCP_Status = "I";
                    contactPerson.ADCP_Department_Id = (int)dpId;
                    contactPerson.ADCP_StartDate = createdDate;
                    departmentContactPersonRepository.Add(contactPerson);
                }
            }
            finally { contactPerson = null; }
        }

        public bool UpdateDepartment(ADM_DEPARTMENTS model, List<ADM_DEPARTMENT_CONTACT_PERSON> contactPersonModel, long modifiedBy, bool isNewContactPerson)
        {
            ADM_DEPARTMENTS temp = null;
            try {
                temp = GetDepartment(model.ADP_CollegeId, (int)model.ADP_Id);
                if (model == null) { return false; }
                temp.ADP_Code = model.ADP_Code;
                temp.ADP_DepartmentLogo = model.ADP_DepartmentLogo;
                temp.ADP_Description = model.ADP_Description;
                temp.ADP_EmailId = model.ADP_EmailId;
                temp.ADP_FaxNo = model.ADP_FaxNo;
                temp.ADP_History = model.ADP_History;
                temp.ADP_MobileNo = model.ADP_MobileNo;
                temp.ADP_PhoneNo = model.ADP_PhoneNo;
                temp.ADP_ShortDescription = model.ADP_ShortDescription;
                temp.ADP_Website = model.ADP_Website;
                temp.ADP_Modified_By = modifiedBy;
                temp.ADP_ModifiedDate = currentDate;
                temp.ADP_Status = "U";
                departmentsRepository.Update(temp);
                DepartmentContactPersonIUAction((int)temp.ADP_Id, contactPersonModel, modifiedBy, isNewContactPerson);
                unitOfWork.Commit();
                return true;
            }
            finally { temp = null; }
        }
        private void DepartmentContactPersonIUAction(int dpId, List<ADM_DEPARTMENT_CONTACT_PERSON> modelList, long modifiedBy, bool isNewCp)
        {
            ADM_DEPARTMENT_CONTACT_PERSON temp = null;
            try {
                foreach (ADM_DEPARTMENT_CONTACT_PERSON model in modelList)
                {
                    temp = departmentContactPersonRepository.Get(exp => exp.ADCP_Department_Id == dpId && exp.ADCP_Id == model.ADCP_Id && exp.ADCP_IsActive);
                    if (temp == null) { return; }
                    if (isNewCp && temp.ADCP_StartDate < currentDate)
                    {
                        DepartmentContactPersonUpdate(model, modifiedBy, currentDate);
                        InsertDepartmentContactPerson(model.ADCP_Department_Id, new List<ADM_DEPARTMENT_CONTACT_PERSON>(){model}, modifiedBy, currentDate.AddDays(1));
                    }
                    else { DepartmentContactPersonUpdate(model, modifiedBy, null); }
                }
            }
            finally { temp = null; }
        }
        private void DepartmentContactPersonUpdate(ADM_DEPARTMENT_CONTACT_PERSON model, long modifiedBy,DateTime ? endDate) {
            ADM_DEPARTMENT_CONTACT_PERSON contactPerson = new ADM_DEPARTMENT_CONTACT_PERSON();
            try
            {
                contactPerson.ADCP_ContactPerson = model.ADCP_ContactPerson;
                contactPerson.ADCP_College_Id = model.ADCP_College_Id;
                contactPerson.ADCP_City = model.ADCP_City;
                contactPerson.ADCP_Country = model.ADCP_Country;
                contactPerson.ADCP_EmailId = model.ADCP_EmailId;
                contactPerson.ADCP_FaxNo = model.ADCP_FaxNo;
                contactPerson.ADCP_PhoneNumber = model.ADCP_PhoneNumber;
                contactPerson.ADCP_Pincode = model.ADCP_Pincode;
                contactPerson.ADCP_ProfileImage = model.ADCP_ProfileImage;
                contactPerson.ADCP_Qualification = model.ADCP_Qualification;
                contactPerson.ADCP_Modified_By = modifiedBy;
                contactPerson.ADCP_EndDate = endDate;
                contactPerson.ADCP_Status = "U";
                departmentContactPersonRepository.Update(contactPerson);
            }
            finally { contactPerson = null; }
        }

        public bool DeleteDepartment( int collegeId,int departmentId, long modifiedBy) {
            ADM_DEPARTMENTS model = null;
            try {
                model = GetDepartment(collegeId, departmentId);
                if (model == null) { return false; }
                model.ADP_Modified_By = modifiedBy;
                model.ADP_ModifiedDate = currentDate;
                model.ADP_EndDate = currentDate;
                model.ADP_IsActive = false;
                model.ADP_Status = "U";
                departmentsRepository.Update(model);
                unitOfWork.Commit();
                DeleteDepartmentContactPerson(collegeId, departmentId, modifiedBy);
                return true;
            }
            finally { model = null; }
        }
        private void DeleteDepartmentContactPerson(int collegeId,int departmentId, long modifiedBy) {
            List<ADM_DEPARTMENT_CONTACT_PERSON> dpcList = null;
            try {
                dpcList = departmentContactPersonRepository.GetMany(exp=>exp.ADCP_College_Id==collegeId && exp.ADCP_Department_Id==departmentId && exp.ADCP_IsActive).ToList();
                if (dpcList.Count == 0) { return; }
                foreach (ADM_DEPARTMENT_CONTACT_PERSON model in dpcList) {
                    model.ADCP_Modified_By = modifiedBy;
                    model.ADCP_Modified_Date = currentDate;
                    model.ADCP_EndDate = currentDate;
                    model.ADCP_IsActive = false;
                    model.ADCP_Status = "U";
                    departmentContactPersonRepository.Update(model);
                }
            }
            finally { dpcList = null; }
        }
        #endregion

        #region Insertinto User
        private void InsertUserAuthentication(ADM_USER_DETAILS model,int orgId,long userId,DateTime startDate,DateTime? endDate, long modifiedBy)
        {
            try
            {
                model.AUD_College_Id = orgId;
                model.AUD_User_Id = userId;
                model.AUD_Start_Date = startDate.Date;
                model.AUD_End_Date = endDate;
                model.AUD_User_Type = "A";
                model.AUD_Status = "I";
                model.AUD_Created_By = modifiedBy;
                model.AUD_Created_Date = DateTime.Now.Date;
                model.AUD_IsActive = true;
                userDetailsRepository.Add(model);
            }
            finally { }
        }
        private void UpdateUserAuthentication(ADM_USER_DETAILS model, DateTime startDate, DateTime? endDate, long modifiedBy)
        {
            try
            {
                model.AUD_Start_Date = startDate;
                model.AUD_End_Date = endDate;
                model.AUD_User_Type = "A";
                model.AUD_Status = "I";
                model.AUD_Created_By = modifiedBy;
                model.AUD_Created_Date = DateTime.Now;
                model.AUD_IsActive = true;
                userDetailsRepository.Add(model);
            }
            finally { }
        }
        public ADM_USER_DETAILS GetUserDetails(int orgId, int userId, DateTime curentDate) {
            try {
                return userDetailsRepository.Get(exp => exp.AUD_College_Id == orgId && exp.AUD_User_Id == userId && exp.AUD_User_Type == "A" && exp.AUD_Start_Date <= curentDate && (exp.AUD_End_Date ?? curentDate) >= curentDate && exp.AUD_IsActive == true);
            }
            finally { }
        }
        public List<SADM_ROLE> GetRoleList() {
            try {
                return roleRepository.GetMany(exp => exp.SR_IsActive).ToList();
            }
            finally { }
        }
        #endregion
        #endregion
    }
}