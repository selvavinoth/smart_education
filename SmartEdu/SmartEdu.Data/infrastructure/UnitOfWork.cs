using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartEdu.Data.infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDataBaseFactory databaseFactory;
        private SampleContext dataContext;

        public UnitOfWork(IDataBaseFactory databaseFactory)
        {
            this.databaseFactory = databaseFactory;
        }
        protected SampleContext DataContext
        {
            get { return dataContext ?? (dataContext = databaseFactory.Get()); }
        }
        
        public void Commit()
        {
            DataContext.Commit();
        }
    }
}