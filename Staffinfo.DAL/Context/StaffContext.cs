using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Staffinfo.DAL.Mapping;
using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Context
{
    /// <summary>
    /// The context provides the data about employees
    /// </summary>
    public class StaffContext: DbContext
    {
        static StaffContext()
        {
            Database.SetInitializer<StaffContext>(null);
        }
        //TODO: remove the dependency of connection name
        public StaffContext(): base("Name=StaffContext")
        {
            //Database.SetInitializer<StaffContext>(null);
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<MesAchievement> MesAchievements { get; set; }
        public virtual DbSet<MilitaryService> MilitaryServices { get; set; }
        public virtual DbSet<Passport> Passports { get; set; }
        public virtual DbSet<Rank> Ranks { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<WorkTerm> WorkTerms { get; set; }
        public virtual DbSet<Dismissed> Dismissed { get; set; }
        public virtual DbSet<DisciplineItem> DisciplineItems { get; set; }
        public virtual DbSet<OutFromOffice> OutFromOffices { get; set; }
        public virtual DbSet<EducationItem> EducationItems { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<Sertification> Sertifications { get; set; }
        public virtual DbSet<Relative> Relatives { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations
                .Add(new AddressMap())
                .Add(new EmployeeMap())
                .Add(new LocationMap())
                .Add(new MesAchievementMap())
                .Add(new MilitaryServiceMap())
                .Add(new PassportMap())
                .Add(new PostMap())
                .Add(new RankMap())
                .Add(new ServiceMap())
                .Add(new WorkTermMap())
                .Add(new DismissedMap())
                .Add(new DisciplineItemMap())
                .Add(new OutFromOfficeMap())
                .Add(new EducationLevelMap())
                .Add(new EducationItemMap())
                .Add(new ContractMap())
                .Add(new RelativeMap())
                .Add(new SertificationMap());
        }

        public virtual ObjectResult<int> GetExpirience(int employeeId)
        {
            var param = new ObjectParameter("EmployeeId", employeeId);

            return ((IObjectContextAdapter) this).ObjectContext.ExecuteFunction<int>("fn_GetMESExpirienceByEmployeeID",
                param);
        } 
    }
}