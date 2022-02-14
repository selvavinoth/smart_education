using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Models;

namespace SmartEdu.Data.Repositories
{
    public interface IParentDetailsRepository : IRepository<ADM_PARENT_DETAILS>
    {
    }
    public class ParentDetailsRepository : RepositoryBase<ADM_PARENT_DETAILS>, IParentDetailsRepository
    {
        public ParentDetailsRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }
}