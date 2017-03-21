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

    public class AuthContextSeedinitializer : DropCreateDatabaseIfModelChanges<AuthContext>
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
            if (!context.Roles.Any(r => r.Name == "reader"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole("reader");

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

                user.Claims.Add(new IdentityUserClaim { ClaimType = "lastname", ClaimValue = "Шевчук" });
                user.Claims.Add(new IdentityUserClaim { ClaimType = "firstname", ClaimValue = "Дмитрий" });
                user.Claims.Add(new IdentityUserClaim { ClaimType = "middlename", ClaimValue = "Павлович" });

                manager.AddToRole(user.Id, "admin");
            }
            if (!context.Users.Any(r => r.UserName == "test_editor"))
            {
                var store = new UserStore<IdentityUser>(context);
                var manager = new UserManager<IdentityUser>(store);
                var user = new IdentityUser
                {
                    UserName = "test_editor",
                    PasswordHash = new PasswordHasher().HashPassword("123456")
                };

                manager.Create(user);

                user.Claims.Add(new IdentityUserClaim { ClaimType = "employeeId", ClaimValue = "1" });

                manager.AddToRole(user.Id, "editor");
            }
            if (!context.Users.Any(r => r.UserName == "test_reader"))
            {
                var store = new UserStore<IdentityUser>(context);
                var manager = new UserManager<IdentityUser>(store);
                var user = new IdentityUser
                {
                    UserName = "test_reader",
                    PasswordHash = new PasswordHasher().HashPassword("123456")
                };

                manager.Create(user);

                user.Claims.Add(new IdentityUserClaim { ClaimType = "employeeId", ClaimValue = "2" });

                manager.AddToRole(user.Id, "reader");
            }
        }
    }
}