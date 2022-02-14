using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;
using SmartEdu.Data.Repositories;
using SmartEdu.Data.infrastructure;

namespace SmartEdu.Bll.Services
{
    public interface IStudentService
    {
        object GetGridData(int collegeId, string departmentType);
        object GetAddressGridData(int collegeId, long studentId, string addressType);
        bool IsUserAvailable(string userName, long studentId);
        ADM_USER_DETAILS GetUserDetails(long studentId, int collegeId);
        ADM_STUDENT_DETAILS GetStudentDetails(long studentId, int collegeId);
        ADM_PARENT_DETAILS GetParentDetails(long studentId, int collegeId);

        void InsertStudent(ADM_STUDENT_DETAILS model,ADM_PARENT_DETAILS parent, ADM_USER_DETAILS userAuthModel, List<ADM_STUDENT_ADDRESS> addressList, long modifiedBy);
        bool UpdateStudent(ADM_STUDENT_DETAILS model, ADM_PARENT_DETAILS parent, ADM_USER_DETAILS userAuthModel, List<ADM_STUDENT_ADDRESS> addressList, long modifiedBy);
        bool DeleteStudent(int collegId, long studentId, long modifiedBy);
    }
    public class StudentService : IStudentService
    {
        private readonly IUserDetailsRepository userDetailsRepository;
        private readonly IStudentDetailsRepository studentDetailsRepository;
        private readonly IStudentAddressRepository studentAddressRepository;
        private readonly IParentDetailsRepository parentDetailsRepository;
        private readonly IAdmLOVRepository aLOVRepository;
        private readonly IUnitOfWork unitOfWork;
        public StudentService(IUserDetailsRepository userDetailsRepository, IStudentDetailsRepository studentDetailsRepository, IStudentAddressRepository studentAddressRepository,IParentDetailsRepository parentDetailsRepository,IAdmLOVRepository aLOVRepository, IUnitOfWork unitOfWork)
        {
            this.userDetailsRepository = userDetailsRepository;
            this.studentDetailsRepository = studentDetailsRepository;
            this.studentAddressRepository = studentAddressRepository;
            this.parentDetailsRepository = parentDetailsRepository;
            this.aLOVRepository = aLOVRepository;
            this.unitOfWork = unitOfWork;
        }

        private DateTime currentDate = DateTime.Now.Date;
        public ADM_USER_DETAILS GetUserDetails(long studentId, int collegeId)
        {
            try
            {
                return userDetailsRepository.Get(exp => exp.AUD_College_Id == collegeId && exp.AUD_User_Id == studentId && exp.AUD_Start_Date <= currentDate && (exp.AUD_End_Date ?? currentDate) >= currentDate);
            }
            finally { }
        }
        public ADM_STUDENT_DETAILS GetStudentDetails(long studentId, int collegeId)
        {
            try
            {
                return studentDetailsRepository.Get(exp => exp.ASD_CollegeId == collegeId && exp.ASD_Id == studentId && exp.ASD_StartDate <= currentDate && (exp.ASD_EndDate ?? currentDate) >= currentDate);
            }
            finally { }
        }
        public ADM_PARENT_DETAILS GetParentDetails(long studentId, int collegeId)
        {
            try
            {
                return parentDetailsRepository.Get(exp => exp.APD_CollegeId == collegeId && exp.APD_StudentId == studentId && exp.APD_StartDate <= currentDate && (exp.APD_EndDate ?? currentDate) >= currentDate);
            }
            finally { }
        }
        
        public bool IsUserAvailable(string userName, long studentId)
        {
            ADM_USER_DETAILS model = null;
            try
            {
                if (studentId == 0)
                    model = userDetailsRepository.Get(exp => exp.AUD_Login_Id == userName && exp.AUD_Start_Date <= currentDate && (exp.AUD_End_Date ?? currentDate) >= currentDate);
                else
                    model = userDetailsRepository.Get(exp => exp.AUD_Login_Id == userName && exp.AUD_User_Id != studentId && exp.AUD_Start_Date <= currentDate && (exp.AUD_End_Date ?? currentDate) >= currentDate);
                return (model == null);
            }
            finally { model = null; }
        }

        #region GridLoading
        public object GetGridData(int collegeId, string departmentType)
        {
            List<ADM_STUDENT_DETAILS> studentList = null;
            List<ADM_PARENT_DETAILS> parentDetails = null;
            List<ADM_LOV> lovList = null;
            try
            {
                studentList = studentDetailsRepository.GetMany(exp => exp.ASD_CollegeId == collegeId && exp.ASD_StartDate <= currentDate && (exp.ASD_EndDate ?? currentDate) >= currentDate).ToList();
                parentDetails = parentDetailsRepository.GetMany(exp => exp.APD_CollegeId == collegeId && exp.APD_StartDate <= currentDate && (exp.APD_EndDate ?? currentDate) >= currentDate).ToList();
                lovList = aLOVRepository.GetMany(exp => exp.AL_Type == departmentType && exp.AL_IsActive).ToList();
                return new
                {
                    total_count = studentList.Count,
                    rows = (from s in studentList
                            join p in parentDetails on s.ASD_Id equals p.APD_StudentId
                            join l in lovList on s.ASD_Department equals l.AL_Id
                            select new
                            {
                                id = s.ASD_Id,
                                data = new string[] { (s.ASD_FirstName ?? "") + " " + (s.ASD_MiddleName ?? "") + " " + (s.ASD_LastName ?? ""), l.AL_Description, (s.ASD_PhoneNo ?? ""), (s.ASD_Email_Id ?? ""), ((p.APD_FirstName ?? "")+" "+(p.APD_MiddleName??"")+" "+(p.APD_LastName??"")), (s.ASD_StartDate.ToString("dd/MM/yyyy")), (s.ASD_EndDate.HasValue ? s.ASD_EndDate.Value.ToString("dd/MM/yyyy") : "") }
                            }).ToList()
                };
            }
            finally { studentList = null; parentDetails = null; lovList = null; }
        }
        public object GetAddressGridData(int collegeId, long staffId, string addressType)
        {
            List<ADM_LOV> lovList = null;
            List<ADM_STUDENT_ADDRESS> studentAddressList = null;
            try
            {
                studentAddressList = studentAddressRepository.GetMany(exp => exp.ASA_College_Id == collegeId && exp.ASA_Student_Id == staffId && exp.ASA_IsActive).ToList();
                lovList = aLOVRepository.GetMany(exp => exp.AL_Type == addressType && exp.AL_IsActive).ToList();
                if (studentAddressList == null || lovList == null) { return new { total_count = 0, rows = new { }}; }
                return new
                {
                    total_count = studentAddressList.Count,
                    rows = (from s in studentAddressList
                            join l in lovList on s.ASA_AddressType equals l.AL_Id
                            select new
                            {
                                id = s.ASA_Id,
                                data = new string[] { s.ASA_Id.ToString(), l.AL_Description, l.AL_Id.ToString(), (s.ASA_Address1 ?? ""), (s.ASA_Address2 ?? ""), (s.ASA_Street ?? ""), (s.ASA_City ?? ""), (s.ASA_State ?? ""), (s.ASA_Country ?? ""), (s.ASA_Pincode ?? ""), "false" }
                            }).ToList()
                };
            }
            finally { lovList = null; studentAddressList = null; }
        }
        #endregion

        #region Insert
        public void InsertStudent(ADM_STUDENT_DETAILS model, ADM_PARENT_DETAILS parentModel, ADM_USER_DETAILS userAuthModel, List<ADM_STUDENT_ADDRESS> addressList, long modifiedBy)
        {
            try
            {
                model.ASD_Created_By = modifiedBy;
                model.ASD_CreatedDate = DateTime.Now;
                model.ASD_Status = "I";
                model.ASD_IsActive = true;
                model.ASD_StartDate = DateTime.Now.Date;
                studentDetailsRepository.Add(model);
                unitOfWork.Commit();
                InsertUserAuthentication(userAuthModel, model, modifiedBy);
                InsertParentDetails(model.ASD_Id, parentModel, modifiedBy);
                unitOfWork.Commit();
                InsertParentAuthentication(userAuthModel, parentModel, modifiedBy);
                InsertStudentAddress(addressList, model, modifiedBy);
                unitOfWork.Commit();
            }
            finally { }
        }
        private void InsertUserAuthentication(ADM_USER_DETAILS model, ADM_STUDENT_DETAILS student, long modifiedBy)
        {
            try
            {
                model.AUD_User_Id = student.ASD_Id;
                model.AUD_Start_Date = student.ASD_StartDate;
                model.AUD_Status = "I";
                model.AUD_User_Type = "S";
                model.AUD_Created_By = modifiedBy;
                model.AUD_Created_Date = DateTime.Now;
                model.AUD_IsActive = true;
                userDetailsRepository.Add(model);
            }
            finally { }
        }
        private void InsertParentAuthentication(ADM_USER_DETAILS temp, ADM_PARENT_DETAILS parent, long modifiedBy)
        {
            ADM_USER_DETAILS model = new ADM_USER_DETAILS();
            try
            {
                model.AUD_User_Id = parent.APD_Id;
                model.AUD_Login_Id = temp.AUD_Login_Id + parent.APD_Id;
                model.AUD_Password = temp.AUD_Password;
                model.AUD_Start_Date = parent.APD_StartDate;
                model.AUD_Question = temp.AUD_Question;
                model.AUD_Answer = temp.AUD_Answer;
                model.AUD_Status = "I";
                model.AUD_User_Type = "P";
                model.AUD_Created_By = modifiedBy;
                model.AUD_Created_Date = DateTime.Now;
                model.AUD_IsActive = true;
                userDetailsRepository.Add(model);
            }
            finally { }
        }
        private void InsertParentDetails(long studentId, ADM_PARENT_DETAILS parent, long modifiedBy)
        {
            try
            {
                parent.APD_StudentId = studentId;
                parent.APD_StartDate = DateTime.Now.Date;
                parent.APD_Status = "I";
                parent.APD_Created_By = modifiedBy;
                parent.APD_CreatedDate = DateTime.Now;
                parent.APD_IsActive = true;
                parentDetailsRepository.Add(parent);
            }
            finally { }
        }
        private void InsertStudentAddress(List<ADM_STUDENT_ADDRESS> addressList, ADM_STUDENT_DETAILS student, long modifiedBy)
        {
            try
            {
                if (addressList.Count == 0) { return; }
                foreach (ADM_STUDENT_ADDRESS model in addressList)
                {
                    model.ASA_Student_Id = student.ASD_Id;
                    model.ASA_Created_By = modifiedBy;
                    model.ASA_Created_Date = DateTime.Now;
                    model.ASA_IsActive = true;
                    studentAddressRepository.Add(model);
                    if (model.ASA_IsPrimary) { unitOfWork.Commit(); student.ASD_PrimaryAddress_Id = model.ASA_Id; }
                }
            }
            finally { }
        }
        #endregion

        #region Update
        public bool UpdateStudent(ADM_STUDENT_DETAILS model,ADM_PARENT_DETAILS parentModel, ADM_USER_DETAILS userAuthModel, List<ADM_STUDENT_ADDRESS> addressList, long modifiedBy)
        {
            ADM_STUDENT_DETAILS temp = null;
            try
            {
                temp = studentDetailsRepository.Get(exp => exp.ASD_CollegeId == model.ASD_CollegeId && exp.ASD_Id == model.ASD_Id && exp.ASD_IsActive);
                if (temp == null) { return false; }
                UpdateStudentDetails(model, temp, modifiedBy);
                UpdateParentDetails(parentModel,modifiedBy);
                UpdateUserAuthentication(userAuthModel, modifiedBy);
                StaffAddressIUAction(addressList, model, modifiedBy);
                unitOfWork.Commit();
                return true;
            }
            finally { temp = null; }
        }
        private void UpdateStudentDetails(ADM_STUDENT_DETAILS model, ADM_STUDENT_DETAILS temp, long modifiedBy)
        {
            try
            {
                temp.ASD_FirstName = model.ASD_FirstName;
                temp.ASD_Department = model.ASD_Department;
                temp.ASD_DOB = model.ASD_DOB;
                temp.ASD_DOJ = model.ASD_DOJ;
                temp.ASD_Email_Id = model.ASD_Email_Id;
                temp.ASD_FaxNo = model.ASD_FaxNo;
                temp.ASD_Gender = model.ASD_Gender;
                temp.ASD_LastName = model.ASD_LastName;
                temp.ASD_MiddleName = model.ASD_MiddleName;
                temp.ASD_MobileNo = model.ASD_MobileNo;
                temp.ASD_PhoneNo = model.ASD_PhoneNo;
                temp.ASD_Title = model.ASD_Title;
                temp.ASD_Modified_By = modifiedBy;
                temp.ASD_ModifiedDate = DateTime.Now;
                temp.ASD_Status = "U";
                studentDetailsRepository.Update(temp);
            }
            finally { }
        }
        private void UpdateParentDetails(ADM_PARENT_DETAILS model, long modifiedBy)
        {
            ADM_PARENT_DETAILS temp = null;
            try
            {
                temp = parentDetailsRepository.Get(exp => exp.APD_CollegeId == model.APD_CollegeId && exp.APD_Id == model.APD_Id && exp.APD_IsActive);
                if (temp == null) { return; }
                temp.APD_FirstName = model.APD_FirstName;
                temp.APD_DOB = model.APD_DOB;
                temp.APD_Email_Id = model.APD_Email_Id;
                temp.APD_FaxNo = model.APD_FaxNo;
                temp.APD_Gender = model.APD_Gender;
                temp.APD_LastName = model.APD_LastName;
                temp.APD_MiddleName = model.APD_MiddleName;
                temp.APD_MobileNo = model.APD_MobileNo;
                temp.APD_PhoneNo = model.APD_PhoneNo;
                temp.APD_Title = model.APD_Title;
                temp.APD_Modified_By = modifiedBy;
                temp.APD_ModifiedDate = DateTime.Now;
                temp.APD_Status = "U";
                parentDetailsRepository.Update(temp);
            }
            finally { temp = null; }
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
        private void StaffAddressIUAction(List<ADM_STUDENT_ADDRESS> addressList, ADM_STUDENT_DETAILS student, long modifiedBy)
        {
            List<ADM_STUDENT_ADDRESS> modelAddressList = null;
            List<ADM_STUDENT_ADDRESS> updatingAddressList = null;
            List<long> exIdList = new List<long>();
            List<long> newIdList = new List<long>();
            try
            {
                modelAddressList = studentAddressRepository.GetMany(exp => exp.ASA_College_Id == student.ASD_CollegeId && exp.ASA_Student_Id == student.ASD_Id && exp.ASA_IsActive).ToList();
                InsertStudentAddress(addressList.FindAll(exp => exp.ASA_Id == 0), student, modifiedBy);
                exIdList = modelAddressList.Select(exp => exp.ASA_Id).ToList();
                updatingAddressList = addressList.FindAll(exp => exIdList.Contains(exp.ASA_Id));
                UpdateStudentAddress(addressList, updatingAddressList, modifiedBy);
                newIdList = updatingAddressList.Select(exp => exp.ASA_Id).ToList();
                DeleteStudentAddress(modelAddressList.FindAll(exp => !newIdList.Contains(exp.ASA_Id)), modifiedBy);
                unitOfWork.Commit();
            }
            finally { modelAddressList = null; updatingAddressList = null; exIdList = null; newIdList = null; }
        }
        private void UpdateStudentAddress(List<ADM_STUDENT_ADDRESS> tempList, List<ADM_STUDENT_ADDRESS> modelList, long modifiedBy)
        {
            ADM_STUDENT_ADDRESS temp = null;
            try
            {
                foreach (ADM_STUDENT_ADDRESS model in modelList)
                {
                    temp = tempList.Find(exp => exp.ASA_Id == temp.ASA_Id);
                    if (model != null)
                    {
                        model.ASA_Address1 = temp.ASA_Address1;
                        model.ASA_Address2 = temp.ASA_Address2;
                        model.ASA_AddressType = temp.ASA_AddressType;
                        model.ASA_City = temp.ASA_City;
                        model.ASA_Country = temp.ASA_Country;
                        model.ASA_Pincode = temp.ASA_Pincode;
                        model.ASA_State = temp.ASA_State;
                        model.ASA_Street = temp.ASA_Street;
                        model.ASA_IsPrimary = temp.ASA_IsPrimary;
                        model.ASA_Status = "U";
                        model.ASA_Modified_By = modifiedBy;
                        model.ASA_Modified_Date = DateTime.Now;
                        studentAddressRepository.Update(model);
                    }
                }
            }
            finally { temp = null; }
        }
        #endregion

        #region Delete
        public bool DeleteStudent(int collegId, long studentId, long modifiedBy)
        {
            ADM_STUDENT_DETAILS temp = null;
            try
            {
                temp = studentDetailsRepository.Get(exp => exp.ASD_CollegeId == collegId && exp.ASD_Id == studentId && exp.ASD_IsActive);
                if (temp == null) { return false; }
                temp.ASD_Modified_By = modifiedBy;
                temp.ASD_ModifiedDate = DateTime.Now;
                temp.ASD_Status = "U";
                temp.ASD_IsActive = true;
                temp.ASD_EndDate = DateTime.Now.Date;
                studentDetailsRepository.Update(temp);
                DeleteParent(collegId, studentId, modifiedBy);
                DeleteUserAuthentication(collegId, studentId, modifiedBy);
                DeleteAddressDetails(collegId, studentId, modifiedBy);
                unitOfWork.Commit();
                return true;
            }
            finally { temp = null; }
        }
        private void DeleteParent(int collegId, long staffId, long modifiedBy)
        {
            ADM_PARENT_DETAILS temp = null;
            try
            {
                temp = parentDetailsRepository.Get(exp => exp.APD_CollegeId == collegId && exp.APD_Id == staffId && exp.APD_IsActive);
                if (temp == null) { return ; }
                temp.APD_Modified_By = modifiedBy;
                temp.APD_ModifiedDate = DateTime.Now;
                temp.APD_Status = "U";
                temp.APD_IsActive = true;
                temp.APD_EndDate = DateTime.Now.Date;
                parentDetailsRepository.Update(temp);
            }
            finally { temp = null; }
        }
        private void DeleteUserAuthentication(int collegId, long studentId, long modifiedBy)
        {
            ADM_USER_DETAILS temp = null;
            try
            {
                temp = userDetailsRepository.Get(exp => exp.AUD_College_Id == collegId && exp.AUD_User_Id == studentId && exp.AUD_Start_Date <= currentDate && (exp.AUD_End_Date ?? currentDate) >= currentDate);
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
            List<ADM_STUDENT_ADDRESS> modelAddressList = null;
            try
            {
                modelAddressList = studentAddressRepository.GetMany(exp => exp.ASA_College_Id == collegId && exp.ASA_Student_Id == staffId && exp.ASA_IsActive).ToList();
                DeleteStudentAddress(modelAddressList, modifiedBy);
                unitOfWork.Commit();
            }
            finally { modelAddressList = null; }
        }
        private void DeleteStudentAddress(List<ADM_STUDENT_ADDRESS> addressList, long modifiedBy)
        {
            try
            {
                foreach (ADM_STUDENT_ADDRESS temp in addressList)
                {
                    temp.ASA_Status = "U";
                    temp.ASA_IsActive = false;
                    temp.ASA_Modified_By = modifiedBy;
                    temp.ASA_Modified_Date = DateTime.Now;
                    studentAddressRepository.Update(temp);
                }
            }
            finally { }
        }
        #endregion
    }
}