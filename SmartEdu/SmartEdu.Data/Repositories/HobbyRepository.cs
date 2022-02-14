using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.Models;
using SmartEdu.Data.infrastructure;

namespace SmartEdu.Data.Repositories
{
    public interface IHobbyRepository : IRepository<SADM_HOBBIESLIST>
    {
    }
    public class HobbyRepository : RepositoryBase<SADM_HOBBIESLIST>, IHobbyRepository
    {
        public HobbyRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }

    }
}