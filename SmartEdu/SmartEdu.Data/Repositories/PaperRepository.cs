using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Models;

namespace SmartEdu.Data.Repositories
{
    public interface IPaperRepository : IRepository<ADM_PAPER> { 
    }
    public class PaperRepository : RepositoryBase<ADM_PAPER>, IPaperRepository
    {
        public PaperRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }
}