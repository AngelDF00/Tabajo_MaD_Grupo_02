using Ninject;
using System.Data.Entity;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioDao;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioService;

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

            kernel.Bind<IUsuarioDao>().
                To<UsuarioDaoEntityFramework>();

            kernel.Bind<IUsuarioService>().
                To<UsuarioService>();

            return kernel;
        }


        public static void ClearNInjectKernel(IKernel kernel)
        {
            kernel.Dispose();
        }
    }
}
