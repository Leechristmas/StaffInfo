using System.Data.Entity;
using Staffinfo.DAL.Mapping;
using Staffinfo.DAL.Models;

namespace Staffinfo.DAL.Context
{
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

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Post> Posts { get; set; } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations
                .Add(new AddressMap())
                .Add(new PostMap())
                .Add(new ServiceMap());
        }
    }
}