using Ninject;
using Ninject.Modules;
using Staffinfo.DAL.Context;
using Staffinfo.DAL.Models;
using Staffinfo.DAL.Repositories;
using Staffinfo.DAL.Repositories.Interfaces;

namespace Staffinfo.DAL.Infrastructure
{
    /// <summary>
    /// A module of the dependency injection
    /// </summary>
    public class StaffInjectModule: NinjectModule
    {
        public override void Load()
        {
            AddRepositoryBindings();
        }

        private void AddRepositoryBindings()
        {
            this.Bind<StaffContext>().ToSelf().InSingletonScope();
            //Unit repository
            this.Bind<IUnitRepository>().To<StaffUnitRepository>();

            this.Bind<IRepository<Address>>().To<AddressRepository>();
            this.Bind<IRepository<Employee>>().To<EmployeeRepository>();
            this.Bind<IRepository<Location>>().To<LocationRepository>();
            this.Bind<IRepository<MesAchievement>>().To<MesAchievementRepository>();
            this.Bind<IRepository<MilitaryService>>().To<MilitaryServiceRepository>();
            this.Bind<IRepository<Passport>>().To<PassportRepository>();
            this.Bind<IRepository<Post>>().To<PostRepository>();
            this.Bind<IRepository<Rank>>().To<RankRepository>();
            this.Bind<IRepository<Service>>().To<ServiceRepository>();
            this.Bind<IRepository<WorkTerm>>().To<WorkTermRepository>();
        }
    }
}