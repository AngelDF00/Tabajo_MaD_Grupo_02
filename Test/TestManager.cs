using Ninject;
using System.Configuration;
using System.Data.Entity;
using Es.Udc.DotNet.PracticaMAD.Model.Persistence;

namespace Es.Udc.DotNet.PracticaMAD.Test
{
    public class TestManager
    {
        /// <summary>
        /// Configures and populates the Ninject kernel
        /// </summary>
        /// <returns>The NInject kernel</returns>
        public static IKernel ConfigureNInjectKernel()
        {
            NinjectSettings settings = new NinjectSettings() { LoadExtensions = true };

            IKernel kernel = new StandardKernel(settings);

            #region PracticaMAD Bindings

            /* 
             * kernel.Bind<IUserProfileDao>().To<UserProfileDaoEntityFramework>();
             * 
             * kernel.Bind<IUserService>().To<UserService>();
             */

            #endregion

            string connectionString =
                ConfigurationManager.ConnectionStrings["PracticaMADTest"].ConnectionString;

            kernel.Bind<DbContext>().
                To<MarketplaceDbContext>().
                InSingletonScope().
                WithConstructorArgument("nameOrConnectionString", connectionString);

            return kernel;
        }


        public static void ClearNInjectKernel(IKernel kernel)
        {
            kernel.Dispose();
        }
    }
}
