using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.WebApi;
using log4net;
using SACS.BusinessLayer.BusinessLogic.Application;
using SACS.BusinessLayer.BusinessLogic.Email;
using SACS.BusinessLayer.BusinessLogic.Loader;
using SACS.BusinessLayer.BusinessLogic.Loader.Interfaces;
using SACS.BusinessLayer.Factories;
using SACS.BusinessLayer.Factories.Interfaces;
using SACS.Common.Factories;
using SACS.Common.Factories.Interfaces;
using SACS.DataAccessLayer.DataAccess;
using SACS.DataAccessLayer.DataAccess.Interfaces;
using SACS.DataAccessLayer.Factories;
using SACS.DataAccessLayer.Factories.Interfaces;
using SACS.WindowsService.Components;

namespace SACS.WindowsService.AppStart
{
    internal class DependencyConfig
    {
        /// <summary>
        /// Register the components with the DI container
        /// </summary>
        public static IContainer RegisterComponents()
        {
            // TODO: will make sense to split this up into different methods
            var builder = new ContainerBuilder();

            // important to register the Web API controllers, otherwise DI will not work.
            builder.RegisterApiControllers(typeof(Program).Assembly);

            EmailProvider emailer = new SmtpEmailProvider(LogManager.GetLogger(typeof(EmailProvider)));
            builder.RegisterInstance(emailer);

            DaoFactory factory = new DaoFactory();
            factory.RegisterDao<IServiceAppDao, ServiceAppDao>();
            factory.RegisterDao<ISystemDao, SystemDao>();
            builder.RegisterInstance(factory).As<IDaoFactory>();

            //// Factories
            builder.RegisterType<ProcessWrapperFactory>()
                .As<IProcessWrapperFactory>();

            builder.RegisterType<ServiceAppProcessWrapperFactory>()
                .As<IServiceAppProcessFactory>();
            //// ---------

            //// Single Instance Components
            builder.RegisterType<ServiceAppSchedulingService>()
                .As<IServiceAppSchedulingService>()
                .SingleInstance();

            builder.RegisterType<AppManager>()
                .As<IAppManager>()
                .SingleInstance();

            builder.RegisterType<SystemMonitor>()
                .SingleInstance();

            builder.RegisterType<WebAPIComponent>()
                .SingleInstance();
            //// ---------

            //// The object graph base
            builder.RegisterType<ServiceContainer>()
                .InstancePerLifetimeScope();
            //// ======

            return builder.Build();
        }
    }
}
