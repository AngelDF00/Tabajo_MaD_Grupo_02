using System;
using System.Data.Entity;
using System.Linq;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ValoracionService;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace Es.Udc.DotNet.PracticaMAD.Test
{
    [TestClass]
    public class ValoracionServiceTest
    {
        [TestMethod]
        public void ValorarVendedorYObtenerEstadisticas()
        {
            IKernel kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                DbContext contexto = kernel.Get<DbContext>();
                IValoracionService valoracionService = kernel.Get<IValoracionService>();
                IUsuarioService usuarioService = kernel.Get<IUsuarioService>();

                string loginVendedor = "vendedor_" + Guid.NewGuid().ToString("N").Substring(0, 8);
                string loginComprador = "comprador_" + Guid.NewGuid().ToString("N").Substring(0, 8);

                DatosPerfilUsuario datosUsuario = new DatosPerfilUsuario
                {
                    Nombre = "Test",
                    Apellidos = "User",
                    Correo = "test@example.invalid"
                };

                using (DbContextTransaction transaccion = contexto.Database.BeginTransaction())
                {
                    long vendedorId = usuarioService.RegistrarUsuario(loginVendedor, "Clave123!", datosUsuario);
                    long compradorId = usuarioService.RegistrarUsuario(loginComprador, "Clave123!", datosUsuario);

                    Categoria categoria = new Categoria { Nombre = "CategoriaTest_" + Guid.NewGuid().ToString("N").Substring(0, 4) };
                    contexto.Set<Categoria>().Add(categoria);
                    contexto.SaveChanges();

                    Producto producto = new Producto
                    {
                        Nombre = "Producto Test",
                        VendedorId = vendedorId,
                        CategoriaId = categoria.CategoriaId,
                        FechaAlta = DateTime.UtcNow
                    };
                    contexto.Set<Producto>().Add(producto);
                    contexto.SaveChanges();

                    long valoracionId = valoracionService.ValorarVendedor(compradorId, producto.ProductoId, 5, "Excelente vendedor");

                    bool duplicadoRechazado = false;

                    try
                    {
                        valoracionService.ValorarVendedor(
                            compradorId, producto.ProductoId, 4, "Segundo intento");
                    }
                    catch (InvalidOperationException)
                    {
                        duplicadoRechazado = true;
                    }

                    Assert.IsTrue(duplicadoRechazado);
                    Assert.AreEqual(
                        1, valoracionService.ObtenerTotalValoracionesVendedor(vendedorId));

                    Assert.IsTrue(valoracionId > 0);

                    double? media = valoracionService.ObtenerMediaValoracionesVendedor(vendedorId);
                    int total = valoracionService.ObtenerTotalValoracionesVendedor(vendedorId);
                    ResultadoValoraciones resultadoPaginado = valoracionService.ObtenerValoracionesVendedor(vendedorId, 0, 10);

                    Assert.IsNotNull(media);
                    Assert.AreEqual(5.0, media.Value);
                    Assert.AreEqual(1, total);
                    Assert.AreEqual(1, resultadoPaginado.TotalValoraciones);
                    Assert.AreEqual(1, resultadoPaginado.Valoraciones.Count);

                    transaccion.Rollback();
                }
            }
            finally
            {
                TestManager.ClearNInjectKernel(kernel);
            }
        }

        [TestMethod]
        public void ValorarVotoFueraDeRangoLanzaExcepcion()
        {
            IKernel kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                DbContext contexto = kernel.Get<DbContext>();
                IValoracionService valoracionService = kernel.Get<IValoracionService>();

                using (DbContextTransaction transaccion = contexto.Database.BeginTransaction())
                {
                    bool argumentoInvalidoDetectado = false;

                    try
                    {
                        valoracionService.ValorarVendedor(1L, 1L, 6, "Voto erróneo");
                    }
                    catch (ArgumentException)
                    {
                        argumentoInvalidoDetectado = true;
                    }

                    Assert.IsTrue(argumentoInvalidoDetectado);
                    transaccion.Rollback();
                }
            }
            finally
            {
                TestManager.ClearNInjectKernel(kernel);
            }
        }
    }
}
