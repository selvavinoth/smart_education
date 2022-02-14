using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Models;

namespace SmartEdu.Data.Repositories
{
    public interface IStudentAddressRepository : IRepository<ADM_STUDENT_ADDRESS>
    {
    }
    public class StudentAddressRepository : RepositoryBase<ADM_STUDENT_ADDRESS>, IStudentAddressRepository
    {
        public StudentAddressRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }
}