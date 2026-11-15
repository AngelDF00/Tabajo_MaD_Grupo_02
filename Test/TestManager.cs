using Ninject;
using System.Data.Entity;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioDao;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioService;
using Es.Udc.DotNet.PracticaMAD.Model.DAO;
using Es.Udc.DotNet.PracticaMAD.Model.DAO.EtiquetaDao; 
using Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ValoracionService;
using Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ReporteService;
using Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ProductoService;
using Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ComentarioService;
using Es.Udc.DotNet.PracticaMAD.Model.SERVICES.EtiquetaService; 

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

            kernel.Bind<DbContext>().
                To<MarketplaceEntities>().
                InSingletonScope();

            kernel.Bind<IUsuarioDao>().
                To<UsuarioDaoEntityFramework>();

            kernel.Bind<IUsuarioService>().
                To<UsuarioService>();

            kernel.Bind<IValoracionDao>().
                To<ValoracionDaoEntityFramework>();

            kernel.Bind<IValoracionService>().
                To<ValoracionService>();

            kernel.Bind<IReporteDao>().
                To<ReporteDaoEntityFramework>();

            kernel.Bind<IReporteService>().
                To<ReporteService>();

            kernel.Bind<IComentarioDao>().
                To<ComentarioDaoEntityFramework>();

            kernel.Bind<IComentarioService>().
                To<ComentarioService>();

            kernel.Bind<IProductoDao>().
                To<ProductoDaoEntityFramework>();

            kernel.Bind<IProductoService>().
                To<ProductoService>();

            kernel.Bind<IEtiquetaDao>().
                To<EtiquetaDaoEntityFramework>();

            kernel.Bind<IEtiquetaService>().
                To<EtiquetaService>();

            #endregion

            return kernel;
        }


        public static void ClearNInjectKernel(IKernel kernel)
        {
            kernel.Dispose();
        }
    }
}