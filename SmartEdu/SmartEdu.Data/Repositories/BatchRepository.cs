using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Models;

namespace SmartEdu.Data.Repositories
{
    public interface IBatchRepository : IRepository<ADM_BATCH>
    {
    }
    public class BatchRepository : RepositoryBase<ADM_BATCH>, IBatchRepository
    {
        public BatchRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }
}