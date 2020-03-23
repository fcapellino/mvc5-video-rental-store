using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.Identity;
using VideoRentalStore.DataAccess;
using VideoRentalStore.DataAccess.Common;
using VideoRentalStore.DataAccess.DataAccessObjects;
using VideoRentalStore.Domain.Entities;

namespace VideoRentalStore.Web
{
    public static class AutofacConfig
    {
        public static void ConfigureDependencyInjection()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterFilterProvider();

            builder.RegisterType<ApplicationDbContext>()
                .InstancePerDependency();

            builder.RegisterType<ClientsDataAccessObject>()
                .As<IDataAccess<Client>>()
                .InstancePerRequest();

            builder.RegisterType<MovieGenresDataAccessObject>()
                .As<IDataAccess<MovieGenre>>()
                .InstancePerRequest();

            builder.RegisterType<MoviesDataAccessObject>()
                .As<IDataAccess<Movie>>()
                .InstancePerRequest();

            builder.RegisterType<RentalsDataAccessObject>()
                .As<IDataAccess<Rental>>()
                .InstancePerRequest();

            builder.RegisterType<EmailService>()
                .As<IIdentityMessageService>()
                .InstancePerRequest();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}
