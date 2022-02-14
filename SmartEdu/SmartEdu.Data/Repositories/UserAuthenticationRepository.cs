using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;
using SmartEdu.Data.infrastructure;

namespace SmartEdu.Data.Repositories
{

    public interface IUserAuthenticationRepository : IRepository<ADM_USER_AUTHENTICATION> { }
    public class UserAuthenticationRepository : RepositoryBase<ADM_USER_AUTHENTICATION>, IUserAuthenticationRepository
    {
        public UserAuthenticationRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }
}