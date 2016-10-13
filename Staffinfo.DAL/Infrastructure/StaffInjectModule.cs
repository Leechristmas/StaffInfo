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

            this.Bind<IRepository<Address>>().To<Repository<Address>>();
            this.Bind<IRepository<Employee>>().To<Repository<Employee>>();
            this.Bind<IRepository<Location>>().To<Repository<Location>>();
            this.Bind<IRepository<MesAchievement>>().To<Repository<MesAchievement>>();
            this.Bind<IRepository<MilitaryService>>().To<Repository<MilitaryService>>();
            this.Bind<IRepository<Passport>>().To<Repository<Passport>>();
            this.Bind<IRepository<Post>>().To<Repository<Post>>();
            this.Bind<IRepository<Rank>>().To<Repository<Rank>>();
            this.Bind<IRepository<Service>>().To<Repository<Service>>();
            this.Bind<IRepository<WorkTerm>>().To<Repository<WorkTerm>>();
            
        }
    }
}