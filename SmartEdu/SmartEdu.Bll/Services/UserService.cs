using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Models;
using SmartEdu.Data.Repositories;

namespace SmartEdu.Bll.Services
{
    public interface IUserService {
       // object GetUserGridData(int collegeId);
        bool IsUserAvailable(int collegeId, string userName,DateTime cDate);
        void InsertUser(ADM_USERS model, ADM_USER_AUTHENTICATION userAuthModel, List<ADM_USER_ADDRESS> addressList, long modifiedBy);
        bool UpdateUser(ADM_USERS model, ADM_USER_AUTHENTICATION userAuthModel, List<ADM_USER_ADDRESS> addressList, long modifiedBy);
     //   bool DeleteUser(int collegeId,long userId, long modifiedBy);
    }
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUsersRepository usersRepository;
        private readonly IUserAuthenticationRepository userAuthenticationRepository;
        private readonly IUserAddressRepository userAddressRepository;
        private readonly IAdmLOVRepository aLOVRepository;
        public UserService(IUsersRepository usersRepository,IUserAuthenticationRepository userAuthenticationRepository,IUserAddressRepository userAddressRepository,IAdmLOVRepository aLOVRepository,IUnitOfWork unitOfWork) {
            this.usersRepository = usersRepository;
            this.userAuthenticationRepository = userAuthenticationRepository;
            this.userAddressRepository = userAddressRepository;
            this.aLOVRepository = aLOVRepository;
            this.unitOfWork = unitOfWork;
        }

        public bool IsUserAvailable(int collegeId, string userName,DateTime cDate) {
            ADM_USER_AUTHENTICATION model = null;
            try {
                model = userAuthenticationRepository.Get(exp => exp.AUA_College_Id == collegeId && exp.AUA_Login_Id == userName);
                return (model == null);
            }
            finally { model = null; }
        }
        public void InsertUser(ADM_USERS model, ADM_USER_AUTHENTICATION userAuthModel, List<ADM_USER_ADDRESS> addressList, long modifiedBy)
        {
            try {
                model.AU_Created_By = modifiedBy;
                model.AU_CreatedDate = DateTime.Now;
                model.AU_Status = "I";
                model.AU_User_Type = "U";
                model.AU_IsActive = true;
                usersRepository.Add(model);
                unitOfWork.Commit();
                InsertUserAuthentication(userAuthModel, model, modifiedBy);
                InsertUserAddress(addressList, model, modifiedBy);
                unitOfWork.Commit();
            }
            finally { }
        }
        private void InsertUserAuthentication(ADM_USER_AUTHENTICATION model,ADM_USERS user, long modifiedBy)
        {
            try {
                model.AUA_User_Id = user.AU_Id;
                model.AUA_Start_Date = user.AU_StartDate;
                model.AUA_Status = "I";
                model.AUA_Created_By = modifiedBy;
                model.AUA_Created_Date = DateTime.Now;
                model.AUA_IsActive = true;
            }
            finally { }
        }
        private void InsertUserAddress(List<ADM_USER_ADDRESS> addressList, ADM_USERS user, long modifiedBy) {
            try {
                if (addressList.Count == 0) { return; }
                foreach (ADM_USER_ADDRESS model in addressList) {
                    model.AUAD_Created_By = modifiedBy;
                    model.AUAD_Created_Date = DateTime.Now;
                    model.AUAD_IsActive = true;
                    userAddressRepository.Add(model); 
                    if (model.AUAD_IsPrimary) { 
                        unitOfWork.Commit();
                        user.AU_PrimaryAddress_Id = model.AUAD_Id;
                    }
                }
            }
            finally { }
        }

        public bool UpdateUser(ADM_USERS model, ADM_USER_AUTHENTICATION userAuthModel, List<ADM_USER_ADDRESS> addressList, long modifiedBy)
        {
            ADM_USERS user = null;
            DateTime cDate=DateTime.Now.Date;
            try {
                user = usersRepository.Get(exp => exp.AU_CollegeId == model.AU_CollegeId && exp.AU_Id==model.AU_Id && (exp.AU_EndDate ?? cDate) >= cDate);
                if (user == null) { return false; }
                UpdateUserDetail(user, model, modifiedBy);
                if (!UpdateUserAuthentication(userAuthModel, model.AU_Id, model.AU_EndDate, modifiedBy, cDate)) { return false; }
                return true;
            }
            finally { user = null; }
        }

        private void UpdateUserDetail(ADM_USERS model, ADM_USERS temp,long modifiedBy) {
            try {
                model.AU_Title = temp.AU_Title;
                model.AU_Anniversary_Date = temp.AU_Anniversary_Date;
                model.AU_Department = temp.AU_Department;
                model.AU_DOB = temp.AU_DOB;
                model.AU_DOJ = temp.AU_DOJ;
                model.AU_Email_Id = temp.AU_Email_Id;
                model.AU_EndDate = temp.AU_EndDate;
                model.AU_FatherName = temp.AU_FatherName;
                model.AU_FatherPhoneNo = temp.AU_FatherPhoneNo;
                model.AU_FaxNo = temp.AU_FaxNo;
                model.AU_FirstName = temp.AU_FirstName;
                model.AU_Gender = temp.AU_Gender;
                model.AU_IsFA = temp.AU_IsFA;
                model.AU_LastName = temp.AU_LastName;
                model.AU_MiddleName = temp.AU_MiddleName;
                model.AU_MobileNo = temp.AU_MobileNo;
                model.AU_PhoneNo = temp.AU_PhoneNo;
                model.AU_Qualification = temp.AU_Qualification;
                model.AU_StartDate = temp.AU_StartDate;
                model.AU_EndDate = temp.AU_EndDate;
                model.AU_Status = "U";
                model.AU_Modified_By = modifiedBy;
                model.AU_ModifiedDate = DateTime.Now;
                usersRepository.Update(model);
            }
            finally { }
        }
        private bool UpdateUserAuthentication(ADM_USER_AUTHENTICATION model,long userId,DateTime ? eDate,long modifiedBy,DateTime cDate) {
            ADM_USER_AUTHENTICATION temp = null;
            try {
                temp = userAuthenticationRepository.Get(exp => exp.AUA_College_Id == model.AUA_College_Id && exp.AUA_User_Id==userId && (exp.AUA_EndDate_Date ?? cDate) >= cDate);
                if (temp == null) { return false; }
                temp.AUA_Login_Id = model.AUA_Login_Id;
                temp.AUA_Password = model.AUA_Password;
                temp.AUA_Question = model.AUA_Question;
                temp.AUA_Answer = model.AUA_Answer;
                temp.AUA_Modified_By = modifiedBy;
                temp.AUA_Modified_Date = DateTime.Now;
                temp.AUA_EndDate_Date = eDate;
                temp.AUA_Status = "U";
                userAuthenticationRepository.Update(temp);
                return true;
            }
            finally { temp = null; }
        }
        private bool UpdateUserAddress(List<ADM_USER_ADDRESS> addressList,ADM_USERS user,long modifiedBy) {
            List<ADM_USER_ADDRESS> exAddressList = null;
            List<long> exIdList = new List<long>();
            List<long> newIdList = new List<long>();
            try {
                exAddressList = userAddressRepository.GetMany(exp => exp.AUAD_College_Id == user.AU_CollegeId && exp.AUAD_User_Id == user.AU_Id && exp.AUAD_IsActive).ToList();
                if (exAddressList == null) { exAddressList = new List<ADM_USER_ADDRESS>(); }
                exIdList = exAddressList.Select(exp => exp.AUAD_Id).ToList();
                newIdList = addressList.Select(exp => exp.AUAD_Id).ToList();
                UpdateUserAddressDetails(exAddressList.FindAll(exp=>newIdList.Contains(exp.AUAD_Id)), addressList.FindAll(exp => exIdList.Contains(exp.AUAD_Id)), modifiedBy);
                DeleteUserAddressDetails(exAddressList.FindAll(exp => !newIdList.Contains(exp.AUAD_Id)), modifiedBy);
                InsertUserAddress(exAddressList.FindAll(exp => !exIdList.Contains(exp.AUAD_Id)), user, modifiedBy);
                unitOfWork.Commit();
                return true;
            }
            finally { exAddressList = null; exIdList = null; newIdList = null; }
        }
        private void UpdateUserAddressDetails(IEnumerable<ADM_USER_ADDRESS> modelList,List<ADM_USER_ADDRESS> tempList,long modifiedBy) {
            ADM_USER_ADDRESS temp = null;
            try {
                if (modelList == null) { return; }
                foreach (ADM_USER_ADDRESS model in modelList) {
                    temp = tempList.Find(exp => exp.AUAD_Id == model.AUAD_Id);
                    if (temp == null) { continue; }
                    model.AUAD_AddressType = temp.AUAD_AddressType;
                    model.AUAD_Address1 = temp.AUAD_Address1;
                    model.AUAD_Address2 = temp.AUAD_Address2;
                    model.AUAD_City = temp.AUAD_City;
                    model.AUAD_Country = temp.AUAD_Country;
                    model.AUAD_IsPrimary = temp.AUAD_IsPrimary;
                    model.AUAD_Pincode=temp.AUAD_Pincode;
                    model.AUAD_State = temp.AUAD_State;
                    model.AUAD_Street = temp.AUAD_Street;
                    model.AUAD_Status = "U";
                    model.AUAD_Modified_By = modifiedBy;
                    model.AUAD_Modified_Date = DateTime.Now;
                    userAddressRepository.Update(model);
                }
            }
            finally { temp = null; }
        }
        private void DeleteUserAddressDetails(IEnumerable<ADM_USER_ADDRESS> modelList, long modifiedBy)
        {
            try
            {
                if (modelList == null) { return; }
                foreach (ADM_USER_ADDRESS model in modelList)
                {
                    model.AUAD_Status = "U";
                    model.AUAD_Modified_By = modifiedBy;
                    model.AUAD_Modified_Date = DateTime.Now;
                    model.AUAD_IsActive = false;
                    userAddressRepository.Update(model);
                }
            }
            finally {}
        }
    }
}