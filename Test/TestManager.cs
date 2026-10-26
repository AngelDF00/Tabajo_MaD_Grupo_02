using Ninject;
using System.Data.Entity;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;

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

            kernel.Bind<DbContext>().
                To<MarketplaceEntities>().
                InSingletonScope();

            return kernel;
        }


        public static void ClearNInjectKernel(IKernel kernel)
        {
            kernel.Dispose();
        }
    }
}
