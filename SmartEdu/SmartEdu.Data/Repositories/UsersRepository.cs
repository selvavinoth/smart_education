using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;
using SmartEdu.Data.infrastructure;

namespace SmartEdu.Data.Repositories
{
    public interface IUsersRepository : IRepository<ADM_USERS> { }
    public class UsersRepository : RepositoryBase<ADM_USERS>, IUsersRepository
    {
        public UsersRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }
}