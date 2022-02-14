using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Models;

namespace SmartEdu.Data.Repositories
{
        public interface ISemesterRepository : IRepository<ADM_SEMESTER>
        {
        }
        public class SemesterRepository : RepositoryBase<ADM_SEMESTER>, ISemesterRepository
        {
            public SemesterRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
        }
    }