using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Models;

namespace SmartEdu.Data.Repositories
{

    #region SADM Master Repository ( Menu Related )
    public interface ITaskRepository : IRepository<SADM_TASK> { }
    public class TaskRepository : RepositoryBase<SADM_TASK>, ITaskRepository {
        public TaskRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IRoleRepository : IRepository<SADM_ROLE> { }
    public class RoleRepository : RepositoryBase<SADM_ROLE>, IRoleRepository {
        public RoleRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IMenuRepository : IRepository<SADM_MENU> { }
    public class MenuRepository : RepositoryBase<SADM_MENU>, IMenuRepository {
        public MenuRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IRoleTaskMappingRepository : IRepository<SADM_ROLE_TASK_MAPPING> { }
    public class RoleTaskMappingRepository : RepositoryBase<SADM_ROLE_TASK_MAPPING>, IRoleTaskMappingRepository
    {
        public RoleTaskMappingRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }
    #endregion

    #region ADM Related Repository ADM_LOV

    public interface IAdmLOVRepository : IRepository<ADM_LOV> { }
    public class AdmLOVRepository : RepositoryBase<ADM_LOV>, IAdmLOVRepository
    {
        public AdmLOVRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }
    #endregion

    public interface IStaffDetailsRepository : IRepository<ADM_STAFF_DETAILS>{}
    public class StaffDetailsRepository : RepositoryBase<ADM_STAFF_DETAILS>, IStaffDetailsRepository
    {
        public StaffDetailsRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IStaffAddressRepository : IRepository<ADM_STAFF_ADDRESS> { }
    public class StaffAddressRepository : RepositoryBase<ADM_STAFF_ADDRESS>, IStaffAddressRepository
    {
        public StaffAddressRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IUserDetailsRepository : IRepository<ADM_USER_DETAILS> { }
    public class UserDetailsRepository : RepositoryBase<ADM_USER_DETAILS>, IUserDetailsRepository
    {
        public UserDetailsRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ICollegeContactPersonRepository : IRepository<ADM_COLLEGE_CONTACT_PERSON>{}
    public class CollegeContactPersonRepository : RepositoryBase<ADM_COLLEGE_CONTACT_PERSON>, ICollegeContactPersonRepository{
        public CollegeContactPersonRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IDepartmentContactPersonRepository : IRepository<ADM_DEPARTMENT_CONTACT_PERSON> { }
    public class DepartmentContactPersonRepository : RepositoryBase<ADM_DEPARTMENT_CONTACT_PERSON>, IDepartmentContactPersonRepository{
        public DepartmentContactPersonRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ICollegeRepository : IRepository<ADM_COLLEGE> { }
    public class CollegeRepository : RepositoryBase<ADM_COLLEGE>, ICollegeRepository
    {
        public CollegeRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IDepartmentsRepository : IRepository<ADM_DEPARTMENTS>{}
    public class DepartmentsRepository : RepositoryBase<ADM_DEPARTMENTS>, IDepartmentsRepository{
        public DepartmentsRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IPositionLevelRepository : IRepository<ADM_POSITION_LEVEL> { }
    public class PositionLevelRepository : RepositoryBase<ADM_POSITION_LEVEL>, IPositionLevelRepository{ public PositionLevelRepository(IDataBaseFactory dataBaseFactory) : base(dataBaseFactory) { } }

    public interface IPositionsRepository : IRepository<ADM_POSITIONS> { }
    public class PositionsRepository : RepositoryBase<ADM_POSITIONS>, IPositionsRepository { public PositionsRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { } }

    public interface IPositionStaffMappingRepository : IRepository<ADM_POSITION_STAFF_MAPPING> { }
    public class PositionStaffMappingRepository : RepositoryBase<ADM_POSITION_STAFF_MAPPING>, IPositionStaffMappingRepository { public PositionStaffMappingRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { } }

    #region Config Repository
    public interface IFieldLevelValidationConfigRepository : IRepository<CONFIG_FIELDLEVEL_VALIDATION> { }
    public class FieldLevelValidationConfigRepository : RepositoryBase<CONFIG_FIELDLEVEL_VALIDATION>, IFieldLevelValidationConfigRepository
    {
        public FieldLevelValidationConfigRepository(IDataBaseFactory databaseFactory) : base(databaseFactory) { }
    }
    #endregion
}