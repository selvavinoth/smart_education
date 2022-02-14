using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data;
using System.Linq.Expressions;

namespace SmartEdu.Data.infrastructure
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        T Get(Expression<Func<T, bool>> where);
    }
    public abstract class RepositoryBase<T> where T : class
    {
        private SampleContext dataContext;
        private readonly DbSet<T> dataset;
        private readonly IDataBaseFactory DataBaseFactory;
        protected SampleContext DataContext
        {
            get { return dataContext ?? (dataContext = DataBaseFactory.Get()); }
        }
        public RepositoryBase(IDataBaseFactory databaseFactory)
        {
            DataBaseFactory = databaseFactory;
            dataset = DataContext.Set<T>();
        }
        public virtual void Add(T entity)
        {
            dataset.Add(entity);
        }
        public virtual void Update(T entity)
        {
            dataset.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete(T entity)
        {
            dataset.Remove(entity);
        }
        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return dataset.Where(where).FirstOrDefault<T>();
        }
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dataset.Where(where).ToList();
        }
    }
}