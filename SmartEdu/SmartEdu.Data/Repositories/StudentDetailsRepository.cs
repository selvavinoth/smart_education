using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Models;

namespace SmartEdu.Data.Repositories
{
    public interface IStudentDetailsRepository : IRepository<ADM_STUDENT_DETAILS>{}
    public class StudentDetailsRepository : RepositoryBase<ADM_STUDENT_DETAILS>, IStudentDetailsRepository
    {
        public StudentDetailsRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }
}