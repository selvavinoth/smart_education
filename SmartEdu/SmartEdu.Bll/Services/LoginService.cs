using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Repositories;
using SmartEdu.Data.Models;
using SmartEdu.Data.infrastructure;

namespace SmartEdu.Bll.Services
{
    public interface ILoginService {
        ADM_USER_DETAILS GerUserDetails(string loginId, string password);
        void UpdateUserDetail(ADM_USER_DETAILS model);
    }
    public class LoginService : ILoginService
    {
        private readonly IUserDetailsRepository userDetailsRepository;
        private readonly IUnitOfWork unitOfWork;
        private DateTime currentDate = DateTime.Now.Date;
        public LoginService(IUserDetailsRepository userDetailsRepository, IUnitOfWork unitOfWork)
        {
            this.userDetailsRepository = userDetailsRepository;
            this.unitOfWork = unitOfWork;
        }

        public ADM_USER_DETAILS GerUserDetails(string loginId, string password) {
            try {
                return userDetailsRepository.Get(exp => exp.AUD_Login_Id == loginId && exp.AUD_Start_Date <= currentDate && (exp.AUD_End_Date ?? currentDate) >= currentDate);
            }
            finally { }
        }
        public void UpdateUserDetail(ADM_USER_DETAILS model) {
            try {
                model.AUD_Modified_Date = DateTime.Now;
                model.AUD_Status = "U";
                userDetailsRepository.Update(model);
                unitOfWork.Commit();
            }
            finally { }
        }
    }
}