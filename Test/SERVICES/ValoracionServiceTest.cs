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
            var kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                var contexto = kernel.Get<DbContext>();
                var valoracionService = kernel.Get<IValoracionService>();
                var usuarioService = kernel.Get<IUsuarioService>();

                var loginVendedor = "vendedor_" + Guid.NewGuid().ToString("N").Substring(0, 8);
                var loginComprador = "comprador_" + Guid.NewGuid().ToString("N").Substring(0, 8);

                var datosUsuario = new DatosPerfilUsuario
                {
                    Nombre = "Test",
                    Apellidos = "User",
                    Correo = "test@example.invalid"
                };

                using (var transaccion = contexto.Database.BeginTransaction())
                {
                    long vendedorId = usuarioService.RegistrarUsuario(loginVendedor, "Clave123!", datosUsuario);
                    long compradorId = usuarioService.RegistrarUsuario(loginComprador, "Clave123!", datosUsuario);

                    var categoria = new Categoria { Nombre = "CategoriaTest_" + Guid.NewGuid().ToString("N").Substring(0, 4) };
                    contexto.Set<Categoria>().Add(categoria);
                    contexto.SaveChanges();

                    var producto = new Producto
                    {
                        Nombre = "Producto Test",
                        VendedorId = vendedorId,
                        CategoriaId = categoria.CategoriaId,
                        FechaAlta = DateTime.UtcNow
                    };
                    contexto.Set<Producto>().Add(producto);
                    contexto.SaveChanges();

                    long valoracionId = valoracionService.ValorarVendedor(compradorId, producto.ProductoId, 5, "Excelente vendedor");

                    Assert.IsTrue(valoracionId > 0);

                    double? media = valoracionService.ObtenerMediaValoracionesVendedor(vendedorId);
                    int total = valoracionService.ObtenerTotalValoracionesVendedor(vendedorId);
                    var resultadoPaginado = valoracionService.ObtenerValoracionesVendedor(vendedorId, 0, 10);

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
            var kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                var contexto = kernel.Get<DbContext>();
                var valoracionService = kernel.Get<IValoracionService>();

                using (var transaccion = contexto.Database.BeginTransaction())
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
