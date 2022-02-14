using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartEdu.Data.infrastructure
{
    public interface IDataBaseFactory
    {
       SampleContext Get();
    }
    public class DataBaseFactory :Disposable,IDataBaseFactory
    {
        private SampleContext dataContext;
        public SampleContext Get()
        {
            return dataContext ?? (dataContext = new SampleContext());
        }
        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}