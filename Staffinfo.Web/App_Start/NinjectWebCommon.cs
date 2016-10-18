using System.Web.Http;
using Ninject.Web.WebApi;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories;
using Staffinfo.DAL.Repositories.Interfaces;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Staffinfo.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Staffinfo.Web.App_Start.NinjectWebCommon), "Stop")]

namespace Staffinfo.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }


        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);

            // Install our Ninject-based IDependencyResolver into the Web API config
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<StaffContext>().ToSelf().InRequestScope();

            //Unit repository
            kernel.Bind<IUnitRepository>().To<StaffUnitRepository>().InRequestScope();

            kernel.Bind<IRepository<Address>>().To<Repository<Address>>().InRequestScope();
            kernel.Bind<IRepository<Employee>>().To<Repository<Employee>>().InRequestScope();
            kernel.Bind<IRepository<Location>>().To<Repository<Location>>().InRequestScope();
            kernel.Bind<IRepository<MesAchievement>>().To<Repository<MesAchievement>>().InRequestScope();
            kernel.Bind<IRepository<MilitaryService>>().To<Repository<MilitaryService>>().InRequestScope();
            kernel.Bind<IRepository<Passport>>().To<Repository<Passport>>().InRequestScope();
            kernel.Bind<IRepository<Post>>().To<Repository<Post>>().InRequestScope();
            kernel.Bind<IRepository<Rank>>().To<Repository<Rank>>().InRequestScope();
            kernel.Bind<IRepository<Service>>().To<Repository<Service>>().InRequestScope();
            kernel.Bind<IRepository<WorkTerm>>().To<Repository<WorkTerm>>().InRequestScope();
        }
    }
}
