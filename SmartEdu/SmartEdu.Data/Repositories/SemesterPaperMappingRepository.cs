using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Models;

namespace SmartEdu.Data.Repositories
{
    public interface ISemesterPaperMappingRepository : IRepository<ADM_SEMESTER_PAPER_MAPPING>
    {
    }
    public class SemesterPaperMappingRepository : RepositoryBase<ADM_SEMESTER_PAPER_MAPPING>, ISemesterPaperMappingRepository
    {
        public SemesterPaperMappingRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }
}