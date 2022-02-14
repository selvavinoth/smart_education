using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;
using SmartEdu.Data.infrastructure;

namespace SmartEdu.Data.Repositories
{
    public interface IUserAddressRepository : IRepository<ADM_USER_ADDRESS> { }

    public class UserAddressRepository : RepositoryBase<ADM_USER_ADDRESS>, IUserAddressRepository
    {
        public UserAddressRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }
}