using Microsoft.AspNet.Identity.EntityFramework;

namespace Staffinfo.API
{
    public class AuthContext: IdentityDbContext<IdentityUser>
    {
        public AuthContext(): base("AuthContext")
        { }
    }
}