using System;
using System.Data.Entity;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Es.Udc.DotNet.PracticaMAD.Model.SERVICES;
using Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ValoracionService;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace Es.Udc.DotNet.PracticaMAD.Test
{
    [TestClass]
    public class ValoracionServiceTest
    {
        private static long CrearProductoParaVendedor(DbContext contexto, long vendedorId, string nombreProducto)
        {
            Categoria categoria = new Categoria
            {
                Nombre = "CategoriaTest_" + Guid.NewGuid().ToString("N").Substring(0, 6)
            };
            contexto.Set<Categoria>().Add(categoria);
            contexto.SaveChanges();

            Producto producto = new Producto
            {
                Nombre = nombreProducto,
                VendedorId = vendedorId,
                CategoriaId = categoria.CategoriaId,
                FechaAlta = DateTime.UtcNow
            };
            contexto.Set<Producto>().Add(producto);
            contexto.SaveChanges();

            return producto.ProductoId;
        }

        private static long CrearUsuarioTest(IUsuarioService usuarioService, string prefijo)
        {
            string sufijo = Guid.NewGuid().ToString("N").Substring(0, 8);
            string login = (prefijo.Length > 8 ? prefijo.Substring(0, 8) : prefijo) + "_" + sufijo.Substring(0, 5);

            DatosPerfilUsuario datosUsuario = new DatosPerfilUsuario
            {
                Nombre = "Test",
                Apellidos = "User",
                Correo = prefijo + "_" + sufijo + "@example.invalid"
            };

            return usuarioService.RegistrarUsuario(login, "Clave123!", datosUsuario);
        }

        [TestMethod]
        public void ValorarVendedorYObtenerEstadisticas()
        {
            IKernel kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                DbContext contexto = kernel.Get<DbContext>();
                IValoracionService valoracionService = kernel.Get<IValoracionService>();
                IUsuarioService usuarioService = kernel.Get<IUsuarioService>();

                using (DbContextTransaction transaccion = contexto.Database.BeginTransaction())
                {
                    long vendedorId = CrearUsuarioTest(usuarioService, "vend");
                    long compradorId = CrearUsuarioTest(usuarioService, "comp");
                    long productoId = CrearProductoParaVendedor(contexto, vendedorId, "Producto Test");

                    long valoracionId = valoracionService.ValorarVendedor(compradorId, productoId, 5, "Excelente vendedor");
                    Assert.IsTrue(valoracionId > 0);

                    bool duplicadoRechazado = false;
                    try
                    {
                        valoracionService.ValorarVendedor(compradorId, productoId, 4, "Segundo intento");
                    }
                    catch (InvalidOperationException)
                    {
                        duplicadoRechazado = true;
                    }

                    Assert.IsTrue(duplicadoRechazado);

                    int total = valoracionService.ObtenerTotalValoracionesVendedor(vendedorId);
                    double? media = valoracionService.ObtenerMediaValoracionesVendedor(vendedorId);
                    Block<Valoracion> resultadoPaginado = valoracionService.ObtenerValoracionesVendedor(vendedorId, 1, 10);

                    Assert.AreEqual(1, total);
                    Assert.IsNotNull(media);
                    Assert.AreEqual(5.0, media.Value);
                    Assert.AreEqual(1, resultadoPaginado.items.Count);
                    Assert.IsFalse(resultadoPaginado.existMoreItems);

                    transaccion.Rollback();
                }
            }
            finally
            {
                TestManager.ClearNInjectKernel(kernel);
            }
        }

        [TestMethod]
        public void VendedorSinValoracionesDevuelveCeroYMediaNula()
        {
            IKernel kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                DbContext contexto = kernel.Get<DbContext>();
                IValoracionService valoracionService = kernel.Get<IValoracionService>();
                IUsuarioService usuarioService = kernel.Get<IUsuarioService>();

                using (DbContextTransaction transaccion = contexto.Database.BeginTransaction())
                {
                    long vendedorId = CrearUsuarioTest(usuarioService, "vend");
                    CrearProductoParaVendedor(contexto, vendedorId, "Producto Sin Votos");

                    int total = valoracionService.ObtenerTotalValoracionesVendedor(vendedorId);
                    double? media = valoracionService.ObtenerMediaValoracionesVendedor(vendedorId);
                    Block<Valoracion> pagina = valoracionService.ObtenerValoracionesVendedor(vendedorId, 1, 10);

                    Assert.AreEqual(0, total);
                    Assert.IsNull(media);
                    Assert.AreEqual(0, pagina.items.Count);
                    Assert.IsFalse(pagina.existMoreItems);

                    transaccion.Rollback();
                }
            }
            finally
            {
                TestManager.ClearNInjectKernel(kernel);
            }
        }

        [TestMethod]
        public void PaginacionValoracionesConYSinPaginaSiguiente()
        {
            IKernel kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                DbContext contexto = kernel.Get<DbContext>();
                IValoracionService valoracionService = kernel.Get<IValoracionService>();
                IUsuarioService usuarioService = kernel.Get<IUsuarioService>();

                using (DbContextTransaction transaccion = contexto.Database.BeginTransaction())
                {
                    long vendedorId = CrearUsuarioTest(usuarioService, "vend");
                    long comprador1Id = CrearUsuarioTest(usuarioService, "comp1");
                    long comprador2Id = CrearUsuarioTest(usuarioService, "comp2");

                    long producto1Id = CrearProductoParaVendedor(contexto, vendedorId, "Producto 1");
                    long producto2Id = CrearProductoParaVendedor(contexto, vendedorId, "Producto 2");

                    valoracionService.ValorarVendedor(comprador1Id, producto1Id, 5, "Muy bien");
                    valoracionService.ValorarVendedor(comprador2Id, producto2Id, 3, "Correcto");

                    Block<Valoracion> pagina1 = valoracionService.ObtenerValoracionesVendedor(vendedorId, 1, 1);
                    Assert.AreEqual(1, pagina1.items.Count);
                    Assert.IsTrue(pagina1.existMoreItems);

                    Block<Valoracion> pagina2 = valoracionService.ObtenerValoracionesVendedor(vendedorId, 2, 1);
                    Assert.AreEqual(1, pagina2.items.Count);
                    Assert.IsFalse(pagina2.existMoreItems);

                    int total = valoracionService.ObtenerTotalValoracionesVendedor(vendedorId);
                    double? media = valoracionService.ObtenerMediaValoracionesVendedor(vendedorId);
                    Assert.AreEqual(2, total);
                    Assert.IsNotNull(media);
                    Assert.AreEqual(4.0, media.Value);

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