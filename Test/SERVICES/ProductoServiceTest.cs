using System;
using System.Data.Entity;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Es.Udc.DotNet.PracticaMAD.Model.SERVICES;
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
            IKernel kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                DbContext contexto = kernel.Get<DbContext>();
                IProductoService productoService = kernel.Get<IProductoService>();
                IUsuarioService usuarioService = kernel.Get<IUsuarioService>();

                using (DbContextTransaction transaccion = contexto.Database.BeginTransaction())
                {
                    DatosPerfilUsuario datosUsuario = new DatosPerfilUsuario
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

                    Categoria categoria = new Categoria
                    {
                        Nombre = "Categoría_" +
                            Guid.NewGuid().ToString("N").Substring(0, 8)
                    };

                    contexto.Set<Categoria>().Add(categoria);
                    contexto.SaveChanges();

                    string palabraClave = "Producto_" +
                        Guid.NewGuid().ToString("N").Substring(0, 8);

                    Producto producto = new Producto
                    {
                        Nombre = palabraClave + "_Prueba",
                        FechaAlta = DateTime.UtcNow,
                        VendedorId = vendedorId,
                        CategoriaId = categoria.CategoriaId
                    };

                    contexto.Set<Producto>().Add(producto);
                    contexto.SaveChanges();

                    Block<Producto> productosEncontrados =
                        productoService.FindProductosByKeyword(
                            palabraClave,
                            1,
                            10);

                    Assert.IsNotNull(productosEncontrados);
                    Assert.AreEqual(1, productosEncontrados.items.Count);
                    Assert.AreEqual(
                        producto.ProductoId,
                        productosEncontrados.items[0].ProductoId);
                    Assert.AreEqual(
                        producto.Nombre,
                        productosEncontrados.items[0].Nombre);

                    Assert.IsFalse(productosEncontrados.existMoreItems);

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
            IKernel kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                DbContext contexto = kernel.Get<DbContext>();
                IProductoService productoService = kernel.Get<IProductoService>();
                IUsuarioService usuarioService = kernel.Get<IUsuarioService>();

                using (DbContextTransaction transaccion = contexto.Database.BeginTransaction())
                {
                    DatosPerfilUsuario datosUsuario = new DatosPerfilUsuario
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

                    Categoria categoria = new Categoria
                    {
                        Nombre = "CategoríaBúsqueda_" +
                            Guid.NewGuid().ToString("N").Substring(0, 8)
                    };

                    contexto.Set<Categoria>().Add(categoria);
                    contexto.SaveChanges();

                    Producto producto = new Producto
                    {
                        Nombre = "Producto de prueba",
                        FechaAlta = DateTime.UtcNow,
                        VendedorId = vendedorId,
                        CategoriaId = categoria.CategoriaId
                    };

                    contexto.Set<Producto>().Add(producto);
                    contexto.SaveChanges();

                    Producto productoEncontrado =
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
