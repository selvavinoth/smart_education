using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using SmartEdu.Data.infrastructure;
using SmartEdu.Data.Repositories;
using SmartEdu.Bll.Services;
namespace SmartRepV4.Web
{
    public static class AppStarter
    {
        public static void Run()
        {
            SetAutofacContainer();
        }

        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerHttpRequest();
            builder.RegisterType<DataBaseFactory>().As<IDataBaseFactory>().InstancePerHttpRequest();
            builder.RegisterAssemblyTypes(typeof(IStudentDetailsRepository).Assembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces().InstancePerHttpRequest();

            builder.RegisterType<StudentService>().As<IStudentService>().InstancePerHttpRequest();
            builder.RegisterAssemblyTypes(typeof(StudentService).Assembly)
            .Where(t => t.Name.EndsWith("Service"))
            .AsImplementedInterfaces().InstancePerHttpRequest();

            builder.RegisterFilterProvider();
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

    }
}
