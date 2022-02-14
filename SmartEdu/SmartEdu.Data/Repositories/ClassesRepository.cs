using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Models;

namespace SmartEdu.Data.Repositories
{
    public interface IClassesRepository : IRepository<ADM_CLASSES>
    {
    }
    public class ClassesRepository : RepositoryBase<ADM_CLASSES>, IClassesRepository
    {
        public ClassesRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }
}