using Ninject;

namespace Staffinfo.DAL.Infrastructure
{
    /// <summary>
    /// Dependency injection configuration (ninject)
    /// </summary>
    public class DIConfig
    {
        static DIConfig()
        {
            Register();
        }

        public static IKernel Kernel;

        public static void Register()
        {
            Kernel = new StandardKernel(new StaffInjectModule());
        }
    }
}