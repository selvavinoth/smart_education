using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Repositories;
using SmartEdu.Data.Models;

namespace SmartEdu.Bll.Services
{
    public interface IStaffService {
        object GetGridData(int collegeId,string type);
        object GetAddressGridData(int collegeId, long staffId, string addressType);
        bool IsUserAvailable(string userName,long staffId);
        ADM_USER_DETAILS GetUserDetails(long staffId, int collegeId);
        ADM_STAFF_DETAILS GetStaffDetails(long staffId, int collegeId);
        void InsertStaff(ADM_STAFF_DETAILS model, ADM_USER_DETAILS userAuthModel, List<ADM_STAFF_ADDRESS> addressList, long modifiedBy);
        bool UpdateStaff(ADM_STAFF_DETAILS model, ADM_USER_DETAILS userAuthModel, List<ADM_STAFF_ADDRESS> addressList, long modifiedBy);
        bool DeleteStaff(int collegId, long staffId, long modifiedBy);
    }
    public class StaffService : IStaffService
    {
        private readonly IStaffDetailsRepository staffDetailsRepository;
        private readonly IStaffAddressRepository staffAddressRepository;
        private readonly IUserDetailsRepository userDetailsRepository;
        private readonly IAdmLOVRepository aLOVRepository;
        private readonly IUnitOfWork unitOfWork;
        public StaffService(IStaffDetailsRepository staffDetailsRepository, IStaffAddressRepository staffAddressRepository, IUserDetailsRepository userDetailsRepository, IUnitOfWork unitOfWork, IAdmLOVRepository aLOVRepository)
        {
            this.staffDetailsRepository = staffDetailsRepository;
            this.staffAddressRepository = staffAddressRepository;
            this.userDetailsRepository = userDetailsRepository;
            this.aLOVRepository = aLOVRepository;
            this.unitOfWork = unitOfWork;
        }
        private DateTime currentDate = DateTime.Now.Date;
        public ADM_USER_DETAILS GetUserDetails(long staffId, int collegeId) {
            try {
                return userDetailsRepository.Get(exp => exp.AUD_College_Id == collegeId && exp.AUD_User_Id == staffId && exp.AUD_Start_Date <= currentDate && (exp.AUD_End_Date ?? currentDate) >= currentDate);
            }
            finally { }
        }
        public ADM_STAFF_DETAILS GetStaffDetails(long staffId, int collegeId)
        {
            try
            {
                return staffDetailsRepository.Get(exp => exp.ASF_CollegeId == collegeId && exp.ASF_Id == staffId && exp.ASF_StartDate <= currentDate && (exp.ASF_EndDate ?? currentDate) >= currentDate);
            }
            finally { }
        }

        public object GetGridData(int collegeId,string departmentType) {
            List<ADM_STAFF_DETAILS> staffList = null;
            List<ADM_LOV> lovList = null;
            try {
                staffList = staffDetailsRepository.GetMany(exp => exp.ASF_CollegeId == collegeId && exp.ASF_StartDate <= currentDate && (exp.ASF_EndDate ?? currentDate) >= currentDate).ToList();
                lovList = aLOVRepository.GetMany(exp => exp.AL_Type == departmentType && exp.AL_IsActive).ToList();
                return new { 
                    total_count=staffList.Count,
                    rows=(from s in staffList
                          join l in lovList on s.ASF_Department equals l.AL_Id
                          select new {
                            id=s.ASF_Id,
                            data=new string[]{ (s.ASF_FirstName??"")+ " "+(s.ASF_MiddleName??"")+" "+(s.ASF_LastName??""),l.AL_Description,(s.ASF_Qualification??""),(s.ASF_PhoneNo??""),(s.ASF_Email_Id??""),(s.ASF_FatherName??""),(s.ASF_StartDate.ToString("dd/MM/yyyy")),(s.ASF_EndDate.HasValue?s.ASF_EndDate.Value.ToString("dd/MM/yyyy"):"")}
                          }).ToList()
                };
            }
            finally { staffList = null; lovList = null; }
        }
        public object GetAddressGridData(int collegeId, long staffId,string addressType) {
            List<ADM_LOV> lovList = null;
            List<ADM_STAFF_ADDRESS> staffAddressList = null;
            try {
                staffAddressList = staffAddressRepository.GetMany(exp => exp.ASFA_College_Id == collegeId && exp.ASFA_Staff_Id == staffId && exp.ASFA_IsActive).ToList();
                lovList = aLOVRepository.GetMany(exp => exp.AL_Type == addressType && exp.AL_IsActive).ToList();
                if (staffAddressList == null || lovList == null) { return new { total_count = 0, rows = new { }}; }
                return new { 
                    total_count=staffAddressList.Count,
                    rows=(from s in staffAddressList
                          join l in lovList on s.ASFA_AddressType equals l.AL_Id
                          select new {
                            id=s.ASFA_Id,
                            data=new string[]{s.ASFA_Id.ToString(),l.AL_Description,l.AL_Id.ToString(),(s.ASFA_Address1??""),(s.ASFA_Address2??""),(s.ASFA_Street??""),(s.ASFA_City??""),(s.ASFA_State??""),(s.ASFA_Country??""),(s.ASFA_Pincode??""),"false"}
                          }).ToList()
                };
            }
            finally { lovList = null; staffAddressList = null; }
        }

        public bool IsUserAvailable(string userName,long staffId)
        {
            ADM_USER_DETAILS model = null;
            try
            {
                if(staffId==0)
                    model = userDetailsRepository.Get(exp => exp.AUD_Login_Id == userName && exp.AUD_Start_Date <= currentDate && (exp.AUD_End_Date ?? currentDate) >= currentDate);
                else
                    model = userDetailsRepository.Get(exp => exp.AUD_Login_Id == userName && exp.AUD_User_Id!=staffId && exp.AUD_Start_Date <= currentDate && (exp.AUD_End_Date ?? currentDate) >= currentDate);
                return (model == null);
            }
            finally { model = null; }
        }
        public void InsertStaff(ADM_STAFF_DETAILS model, ADM_USER_DETAILS userAuthModel, List<ADM_STAFF_ADDRESS> addressList, long modifiedBy)
        {
            try
            {
                model.ASF_Created_By = modifiedBy;
                model.ASF_CreatedDate = DateTime.Now;
                model.ASF_Status = "I";
                model.ASF_IsActive = true;
                model.ASF_StartDate = DateTime.Now.Date;
                model.ASF_Anniversary_Date = DateTime.Now.Date;
                staffDetailsRepository.Add(model);
                unitOfWork.Commit();
                InsertUserAuthentication(userAuthModel, model, modifiedBy);
                InsertStaffAddress(addressList, model, modifiedBy);
                unitOfWork.Commit();
            }
            finally { }
        }
        private void InsertUserAuthentication(ADM_USER_DETAILS model, ADM_STAFF_DETAILS staff, long modifiedBy)
        {
            try
            {
                model.AUD_User_Id = staff.ASF_Id;
                model.AUD_Start_Date = staff.ASF_StartDate;
                model.AUD_Status = "I";
                model.AUD_Created_By = modifiedBy;
                model.AUD_Created_Date = DateTime.Now;
                model.AUD_IsActive = true;
                userDetailsRepository.Add(model);
            }
            finally { }
        }
        private void InsertStaffAddress(List<ADM_STAFF_ADDRESS> addressList, ADM_STAFF_DETAILS staff, long modifiedBy)
        {
            try
            {
                if (addressList.Count == 0) { return; }
                foreach (ADM_STAFF_ADDRESS model in addressList)
                {
                    model.ASFA_Staff_Id = staff.ASF_Id;
                    model.ASFA_Created_By = modifiedBy;
                    model.ASFA_Created_Date = DateTime.Now;
                    model.ASFA_IsActive = true;
                    staffAddressRepository.Add(model);
                    if (model.ASFA_IsPrimary) { unitOfWork.Commit(); staff.ASF_PrimaryAddress_Id = model.ASFA_Id; }
                }
            }
            finally { }
        }
       



        

        public bool UpdateStaff(ADM_STAFF_DETAILS model, ADM_USER_DETAILS userAuthModel, List<ADM_STAFF_ADDRESS> addressList, long modifiedBy) {
            ADM_STAFF_DETAILS temp = null;
            try {
                temp = staffDetailsRepository.Get(exp => exp.ASF_CollegeId == model.ASF_CollegeId && exp.ASF_Id == model.ASF_Id && exp.ASF_IsActive);
                if (temp == null) { return false; }
                UpdateStaffDetails(model, temp, modifiedBy);
                UpdateUserAuthentication(userAuthModel, modifiedBy);
                StaffAddressIUAction(addressList, model, modifiedBy);
                unitOfWork.Commit();
                return true;
            }
            finally { temp = null; }
        }
        private void UpdateStaffDetails(ADM_STAFF_DETAILS model, ADM_STAFF_DETAILS temp,long modifiedBy)
        {
            try {
                temp.ASF_FirstName = model.ASF_FirstName;
                temp.ASF_Anniversary_Date = model.ASF_Anniversary_Date;
                temp.ASF_Department = model.ASF_Department;
                temp.ASF_DOB = model.ASF_DOB;
                temp.ASF_DOJ = model.ASF_DOJ;
                temp.ASF_Email_Id = model.ASF_Email_Id;
                temp.ASF_FatherName = model.ASF_FatherName;
                temp.ASF_FatherPhoneNo = model.ASF_FatherPhoneNo;
                temp.ASF_FaxNo = model.ASF_FaxNo;
                temp.ASF_Gender = model.ASF_Gender;
                temp.ASF_IsFA = model.ASF_IsFA;
                temp.ASF_LastName = model.ASF_LastName;
                temp.ASF_MiddleName = model.ASF_MiddleName;
                temp.ASF_MobileNo = model.ASF_MobileNo;
                temp.ASF_PhoneNo = model.ASF_PhoneNo;
                temp.ASF_Qualification = model.ASF_Qualification;
                temp.ASF_Title = model.ASF_Title;

                temp.ASF_Modified_By = modifiedBy;
                temp.ASF_ModifiedDate = DateTime.Now;
                temp.ASF_Status = "U";
                staffDetailsRepository.Update(temp);
            }
            finally { }
        }
        private void UpdateUserAuthentication(ADM_USER_DETAILS model, long modifiedBy)
        {
            ADM_USER_DETAILS temp = null;
            try
            {
                temp = userDetailsRepository.Get(exp => exp.AUD_Id == model.AUD_Id && exp.AUD_Start_Date <= currentDate && (exp.AUD_End_Date ?? currentDate) >= currentDate);
                if (temp == null) { return; }
                temp.AUD_Password = model.AUD_Password;
                temp.AUD_Question = model.AUD_Question;
                temp.AUD_Answer = model.AUD_Answer;
                temp.AUD_Status = "U";
                temp.AUD_Modified_By = modifiedBy;
                temp.AUD_Created_Date = DateTime.Now;
                userDetailsRepository.Update(temp);
            }
            finally { }
        }
        private void StaffAddressIUAction(List<ADM_STAFF_ADDRESS> addressList, ADM_STAFF_DETAILS staff, long modifiedBy)
        {
            List<ADM_STAFF_ADDRESS> modelAddressList = null;
            List<ADM_STAFF_ADDRESS> updatingAddressList = null;
            List<long> exIdList = new List<long>();
            List<long> newIdList = new List<long>();
            try
            {
                modelAddressList = staffAddressRepository.GetMany(exp => exp.ASFA_College_Id == staff.ASF_CollegeId && exp.ASFA_Staff_Id == staff.ASF_Id && exp.ASFA_IsActive).ToList();
                InsertStaffAddress(addressList.FindAll(exp => exp.ASFA_Id == 0), staff, modifiedBy);
                exIdList = modelAddressList.Select(exp => exp.ASFA_Id).ToList();
                updatingAddressList = addressList.FindAll(exp => exIdList.Contains(exp.ASFA_Id));
                UpdateStaffAddress(addressList, updatingAddressList, modifiedBy);
                newIdList = updatingAddressList.Select(exp => exp.ASFA_Id).ToList();
                DeleteStaffAddress(modelAddressList.FindAll(exp => !newIdList.Contains(exp.ASFA_Id)), modifiedBy);
                unitOfWork.Commit();
            }
            finally { modelAddressList = null; updatingAddressList = null; exIdList = null; newIdList = null; }
        }
        private void UpdateStaffAddress(List<ADM_STAFF_ADDRESS> tempList, List<ADM_STAFF_ADDRESS> modelList, long modifiedBy)
        {
            ADM_STAFF_ADDRESS temp = null;
            try
            {
                foreach (ADM_STAFF_ADDRESS model in modelList)
                {
                    temp = tempList.Find(exp => exp.ASFA_Id == temp.ASFA_Id);
                    if (model != null)
                    {
                        model.ASFA_Address1 = temp.ASFA_Address1;
                        model.ASFA_Address2 = temp.ASFA_Address2;
                        model.ASFA_AddressType = temp.ASFA_AddressType;
                        model.ASFA_City = temp.ASFA_City;
                        model.ASFA_Country = temp.ASFA_Country;
                        model.ASFA_Pincode = temp.ASFA_Pincode;
                        model.ASFA_State = temp.ASFA_State;
                        model.ASFA_Street = temp.ASFA_Street;
                        model.ASFA_IsPrimary = temp.ASFA_IsPrimary;
                        model.ASFA_Status = "U";
                        model.ASFA_Modified_By = modifiedBy;
                        model.ASFA_Modified_Date = DateTime.Now;
                        staffAddressRepository.Update(model);
                    }
                }
            }
            finally { temp = null; }
        }

        public bool DeleteStaff(int collegId,long staffId,long modifiedBy)
        {
            ADM_STAFF_DETAILS temp = null;
            try
            {
                temp = staffDetailsRepository.Get(exp => exp.ASF_CollegeId == collegId && exp.ASF_Id == staffId && exp.ASF_IsActive);
                if (temp == null) { return false; }
                temp.ASF_Modified_By = modifiedBy;
                temp.ASF_ModifiedDate = DateTime.Now;
                temp.ASF_Status = "U";
                temp.ASF_IsActive = true;
                temp.ASF_EndDate = DateTime.Now.Date;
                staffDetailsRepository.Update(temp);
                DeleteUserAuthentication(collegId, staffId, modifiedBy);
                DeleteAddressDetails(collegId, staffId, modifiedBy);
                unitOfWork.Commit();
                return true;
            }
            finally { temp = null; }
        }
        private void DeleteUserAuthentication(int collegId,long staffId, long modifiedBy)
        {
            ADM_USER_DETAILS temp = null;
            try
            {
                temp = userDetailsRepository.Get(exp => exp.AUD_College_Id==collegId && exp.AUD_User_Id==staffId && exp.AUD_Start_Date <= currentDate && (exp.AUD_End_Date ?? currentDate) >= currentDate);
                if (temp == null) { return; }
                temp.AUD_Status = "U";
                temp.AUD_IsActive = false;
                temp.AUD_Modified_By = modifiedBy;
                temp.AUD_Modified_Date = DateTime.Now;
                temp.AUD_End_Date = DateTime.Now.Date;
                userDetailsRepository.Update(temp);
            }
            finally { }
        }
        private void DeleteAddressDetails(int collegId, long staffId, long modifiedBy)
        {
            List<ADM_STAFF_ADDRESS> modelAddressList = null;
            try {
                modelAddressList = staffAddressRepository.GetMany(exp => exp.ASFA_College_Id == collegId && exp.ASFA_Staff_Id == staffId && exp.ASFA_IsActive).ToList();
                DeleteStaffAddress(modelAddressList, modifiedBy);
                unitOfWork.Commit();
            }
            finally { modelAddressList = null; }
        }
        private void DeleteStaffAddress(List<ADM_STAFF_ADDRESS> addressList, long modifiedBy)
        {
            try
            {
                foreach (ADM_STAFF_ADDRESS temp in addressList)
                {
                    temp.ASFA_Status = "U";
                    temp.ASFA_IsActive = false;
                    temp.ASFA_Modified_By = modifiedBy;
                    temp.ASFA_Modified_Date = DateTime.Now;
                    staffAddressRepository.Update(temp);
                }
            }
            finally { }
        }
    }
}