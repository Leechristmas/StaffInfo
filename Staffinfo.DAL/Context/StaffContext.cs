using System.Data.Entity;
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

        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Post> Posts { get; set; } 
        public virtual DbSet<Employee> Employees { get; set; }
        //public virtual DbSet<EmployeeMin> EmployeesMin { get; set; }
        public virtual DbSet<Location> Locations { get; set; } 
        public virtual DbSet<MesAchievement> MesAchievements { get; set; } 
        public virtual DbSet<MilitaryService> MilitaryServices { get; set; } 
        public virtual DbSet<Passport> Passports { get; set; } 
        public virtual DbSet<Rank> Ranks { get; set; } 
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<WorkTerm> WorkTerms { get; set; } 

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
                .Add(new WorkTermMap());
            //.Add(new EmployeeMinMap());
        }
    }
}