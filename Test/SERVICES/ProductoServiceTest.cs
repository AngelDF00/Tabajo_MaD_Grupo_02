using System;
using System.Data.Entity;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ProductoService;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace Es.Udc.DotNet.PracticaMAD.Test.SERVICES
{
    [TestClass]
    public class ProductoServiceTest
    {
        [TestMethod]
        public void FindProductosByKeywordTest()
        {
            var kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                var contexto = kernel.Get<DbContext>();
                var productoService = kernel.Get<IProductoService>();
                var usuarioService = kernel.Get<IUsuarioService>();

                using (var transaccion = contexto.Database.BeginTransaction())
                {
                    var datosUsuario = new DatosPerfilUsuario
                    {
                        Nombre = "Vendedor",
                        Apellidos = "Producto",
                        Correo = "producto_" +
                            Guid.NewGuid().ToString("N") +
                            "@example.invalid"
                    };

                    long vendedorId = usuarioService.RegistrarUsuario(
                        "vendedor_" +
                        Guid.NewGuid().ToString("N").Substring(0, 12),
                        "Clave123!",
                        datosUsuario);

                    var categoria = new Categoria
                    {
                        Nombre = "Categoría_" +
                            Guid.NewGuid().ToString("N").Substring(0, 8)
                    };

                    contexto.Set<Categoria>().Add(categoria);
                    contexto.SaveChanges();

                    string palabraClave = "Producto_" +
                        Guid.NewGuid().ToString("N").Substring(0, 8);

                    var producto = new Producto
                    {
                        Nombre = palabraClave + "_Prueba",
                        FechaAlta = DateTime.UtcNow,
                        VendedorId = vendedorId,
                        CategoriaId = categoria.CategoriaId
                    };

                    contexto.Set<Producto>().Add(producto);
                    contexto.SaveChanges();

                    var productosEncontrados =
                        productoService.FindProductosByKeyword(
                            palabraClave,
                            1,
                            10);

                    Assert.IsNotNull(productosEncontrados);
                    Assert.AreEqual(1, productosEncontrados.Count);
                    Assert.AreEqual(
                        producto.ProductoId,
                        productosEncontrados[0].ProductoId);
                    Assert.AreEqual(
                        producto.Nombre,
                        productosEncontrados[0].Nombre);

                    transaccion.Rollback();
                }
            }
            finally
            {
                TestManager.ClearNInjectKernel(kernel);
            }
        }

        [TestMethod]
        public void FindByIdTest()
        {
            var kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                var contexto = kernel.Get<DbContext>();
                var productoService = kernel.Get<IProductoService>();
                var usuarioService = kernel.Get<IUsuarioService>();

                using (var transaccion = contexto.Database.BeginTransaction())
                {
                    var datosUsuario = new DatosPerfilUsuario
                    {
                        Nombre = "Vendedor",
                        Apellidos = "Búsqueda",
                        Correo = "busqueda_" +
                            Guid.NewGuid().ToString("N") +
                            "@example.invalid"
                    };

                    long vendedorId = usuarioService.RegistrarUsuario(
                        "busqueda_" +
                        Guid.NewGuid().ToString("N").Substring(0, 12),
                        "Clave123!",
                        datosUsuario);

                    var categoria = new Categoria
                    {
                        Nombre = "CategoríaBúsqueda_" +
                            Guid.NewGuid().ToString("N").Substring(0, 8)
                    };

                    contexto.Set<Categoria>().Add(categoria);
                    contexto.SaveChanges();

                    var producto = new Producto
                    {
                        Nombre = "Producto de prueba",
                        FechaAlta = DateTime.UtcNow,
                        VendedorId = vendedorId,
                        CategoriaId = categoria.CategoriaId
                    };

                    contexto.Set<Producto>().Add(producto);
                    contexto.SaveChanges();

                    var productoEncontrado =
                        productoService.FindById(producto.ProductoId);

                    Assert.IsNotNull(productoEncontrado);
                    Assert.AreEqual(
                        producto.ProductoId,
                        productoEncontrado.ProductoId);
                    Assert.AreEqual(
                        producto.Nombre,
                        productoEncontrado.Nombre);
                    Assert.AreEqual(
                        producto.VendedorId,
                        productoEncontrado.VendedorId);
                    Assert.AreEqual(
                        producto.CategoriaId,
                        productoEncontrado.CategoriaId);

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
