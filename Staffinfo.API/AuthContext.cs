using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Staffinfo.API
{
    public class AuthContext: IdentityDbContext<IdentityUser>
    {
        static AuthContext()
        {
            System.Data.Entity.Database.SetInitializer(new AuthContextSeedinitializer());
        }

        public AuthContext() : base("AuthContext")
        {
            
        }

        public static AuthContext Create() => new AuthContext();
    }

    public class AuthContextSeedinitializer : DropCreateDatabaseAlways<AuthContext>
    {
        protected override void Seed(AuthContext context)
        {
            if (!context.Roles.Any(r => r.Name == "admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole("admin");

                manager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "reporter"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole("reporter");

                manager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "editor"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole("editor");

                manager.Create(role);
            }
            if (!context.Users.Any(r => r.UserName == "admin"))
            {
                var store = new UserStore<IdentityUser>(context);
                var manager = new UserManager<IdentityUser>(store);
                var user = new IdentityUser
                {
                    UserName = "admin",
                    PasswordHash = new PasswordHasher().HashPassword("123456")
                };

                manager.Create(user);
                manager.AddToRole(user.Id, "admin");
            }
        }
    }
}