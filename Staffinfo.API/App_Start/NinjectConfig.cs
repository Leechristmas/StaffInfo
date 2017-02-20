using System;
using System.Reflection;
using Ninject;
using Ninject.Web.Common;
using NLog;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories;
using Staffinfo.DAL.Repositories.Interfaces;

public static class NinjectConfig
{
    public static Lazy<IKernel> CreateKernel = new Lazy<IKernel>(() =>
    {
        var kernel = new StandardKernel();
        kernel.Load(Assembly.GetExecutingAssembly());

        RegisterServices(kernel);

        return kernel;
    });

    private static void RegisterServices(KernelBase kernel)
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
        kernel.Bind<IRepository<Dismissed>>().To<Repository<Dismissed>>().InRequestScope();
        kernel.Bind<IRepository<DisciplineItem>>().To<Repository<DisciplineItem>>().InRequestScope();
        kernel.Bind<IRepository<OutFromOffice>>().To<Repository<OutFromOffice>>().InRequestScope();
        kernel.Bind<IRepository<Sertification>>().To<Repository<Sertification>>().InRequestScope();
        kernel.Bind<IRepository<EducationItem>>().To<Repository<EducationItem>>().InRequestScope();
        kernel.Bind<IRepository<Contract>>().To<Repository<Contract>>().InRequestScope();
        kernel.Bind<IRepository<Relative>>().To<Repository<Relative>>().InRequestScope();
        kernel.Bind<ILogger>().ToMethod(lm => LogManager.GetLogger("ControllerLogger"));
    }
}