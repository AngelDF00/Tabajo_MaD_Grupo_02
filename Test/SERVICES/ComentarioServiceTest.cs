using System;
using System.Data.Entity;
using System.Linq;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ComentarioService;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace Es.Udc.DotNet.PracticaMAD.Test.SERVICES
{
    [TestClass]
    public class ComentarioServiceTest
    {
        [TestMethod]
        public void CreateComentarioGuardaElComentario()
        {
            var kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                var contexto = kernel.Get<DbContext>();
                var comentarioService = kernel.Get<IComentarioService>();
                var usuarioService = kernel.Get<IUsuarioService>();

                using (var transaccion = contexto.Database.BeginTransaction())
                {
                    Producto producto = CrearProducto(
                        contexto,
                        usuarioService);

                    var comentario = new Comentario
                    {
                        ProductoId = producto.ProductoId,
                        AutorId = producto.VendedorId,
                        Texto = "Comentario de prueba",
                        FechaCreacion = DateTime.UtcNow,
                        EnRevision = false
                    };

                    comentarioService.CreateComentario(comentario);

                    var comentarioGuardado = contexto.Set<Comentario>()
                        .SingleOrDefault(c =>
                            c.ComentarioId == comentario.ComentarioId);

                    Assert.IsNotNull(comentarioGuardado);
                    Assert.AreEqual(
                        comentario.ProductoId,
                        comentarioGuardado.ProductoId);
                    Assert.AreEqual(
                        comentario.AutorId,
                        comentarioGuardado.AutorId);
                    Assert.AreEqual(
                        comentario.Texto,
                        comentarioGuardado.Texto);

                    transaccion.Rollback();
                }
            }
            finally
            {
                TestManager.ClearNInjectKernel(kernel);
            }
        }

        [TestMethod]
        public void DeleteComentarioEliminaElComentarioDelAutor()
        {
            var kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                var contexto = kernel.Get<DbContext>();
                var comentarioService = kernel.Get<IComentarioService>();
                var usuarioService = kernel.Get<IUsuarioService>();

                using (var transaccion = contexto.Database.BeginTransaction())
                {
                    Producto producto = CrearProducto(
                        contexto,
                        usuarioService);

                    var comentario = new Comentario
                    {
                        ProductoId = producto.ProductoId,
                        AutorId = producto.VendedorId,
                        Texto = "Comentario para eliminar",
                        FechaCreacion = DateTime.UtcNow,
                        EnRevision = false
                    };

                    contexto.Set<Comentario>().Add(comentario);
                    contexto.SaveChanges();

                    comentarioService.DeleteComentario(
                        producto.VendedorId,
                        comentario.ComentarioId);

                    var comentarioEliminado = contexto.Set<Comentario>()
                        .SingleOrDefault(c =>
                            c.ComentarioId == comentario.ComentarioId);

                    Assert.IsNull(comentarioEliminado);

                    transaccion.Rollback();
                }
            }
            finally
            {
                TestManager.ClearNInjectKernel(kernel);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void DeleteComentarioRechazaUnUsuarioQueNoEsElAutor()
        {
            var kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                var contexto = kernel.Get<DbContext>();
                var comentarioService = kernel.Get<IComentarioService>();
                var usuarioService = kernel.Get<IUsuarioService>();

                using (var transaccion = contexto.Database.BeginTransaction())
                {
                    Producto producto = CrearProducto(
                        contexto,
                        usuarioService);

                    long otroUsuarioId = CrearUsuario(
                        usuarioService,
                        "otro");

                    var comentario = new Comentario
                    {
                        ProductoId = producto.ProductoId,
                        AutorId = producto.VendedorId,
                        Texto = "Comentario protegido",
                        FechaCreacion = DateTime.UtcNow,
                        EnRevision = false
                    };

                    contexto.Set<Comentario>().Add(comentario);
                    contexto.SaveChanges();

                    comentarioService.DeleteComentario(
                            otroUsuarioId,
                            comentario.ComentarioId);

                    Assert.IsNotNull(
                        contexto.Set<Comentario>()
                            .SingleOrDefault(c =>
                                c.ComentarioId == comentario.ComentarioId));

                    transaccion.Rollback();
                }
            }
            finally
            {
                TestManager.ClearNInjectKernel(kernel);
            }
        }

        [TestMethod]
        public void UpdateComentarioActualizaElTextoYConservaElAutor()
        {
            var kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                var contexto = kernel.Get<DbContext>();
                var comentarioService = kernel.Get<IComentarioService>();
                var usuarioService = kernel.Get<IUsuarioService>();

                using (var transaccion = contexto.Database.BeginTransaction())
                {
                    Producto producto = CrearProducto(
                        contexto,
                        usuarioService);

                    var comentario = new Comentario
                    {
                        ProductoId = producto.ProductoId,
                        AutorId = producto.VendedorId,
                        Texto = "Texto original",
                        FechaCreacion = DateTime.UtcNow,
                        EnRevision = false
                    };

                    contexto.Set<Comentario>().Add(comentario);
                    contexto.SaveChanges();

                    comentarioService.UpdateComentario(
                        producto.VendedorId,
                        "Texto actualizado",
                        comentario.ComentarioId);

                    var comentarioActualizado = contexto.Set<Comentario>()
                        .Single(c =>
                            c.ComentarioId == comentario.ComentarioId);

                    Assert.AreEqual(
                        "Texto actualizado",
                        comentarioActualizado.Texto);

                    Assert.AreEqual(
                        producto.VendedorId,
                        comentarioActualizado.AutorId);

                    transaccion.Rollback();
                }
            }
            finally
            {
                TestManager.ClearNInjectKernel(kernel);
            }
        }

        [TestMethod]
        public void FindComentariosByProductoDevuelveComentariosPaginados()
        {
            var kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                var contexto = kernel.Get<DbContext>();
                var comentarioService = kernel.Get<IComentarioService>();
                var usuarioService = kernel.Get<IUsuarioService>();

                using (var transaccion = contexto.Database.BeginTransaction())
                {
                    Producto producto = CrearProducto(
                        contexto,
                        usuarioService);

                    DateTime fechaAntigua = DateTime.UtcNow.AddMinutes(-2);
                    DateTime fechaReciente = DateTime.UtcNow;

                    var comentarioAntiguo = new Comentario
                    {
                        ProductoId = producto.ProductoId,
                        AutorId = producto.VendedorId,
                        Texto = "Comentario antiguo",
                        FechaCreacion = fechaAntigua,
                        EnRevision = false
                    };

                    var comentarioReciente = new Comentario
                    {
                        ProductoId = producto.ProductoId,
                        AutorId = producto.VendedorId,
                        Texto = "Comentario reciente",
                        FechaCreacion = fechaReciente,
                        EnRevision = false
                    };

                    contexto.Set<Comentario>().Add(comentarioAntiguo);
                    contexto.Set<Comentario>().Add(comentarioReciente);
                    contexto.SaveChanges();

                    var primeraPagina =
                        comentarioService.FindComentariosByProducto(
                            producto.ProductoId,
                            1,
                            1);

                    var segundaPagina =
                        comentarioService.FindComentariosByProducto(
                            producto.ProductoId,
                            2,
                            1);

                    Assert.AreEqual(1, primeraPagina.Count);
                    Assert.AreEqual(1, segundaPagina.Count);

                    Assert.AreEqual(
                        comentarioReciente.ComentarioId,
                        primeraPagina[0].ComentarioId);

                    Assert.AreEqual(
                        comentarioAntiguo.ComentarioId,
                        segundaPagina[0].ComentarioId);

                    transaccion.Rollback();
                }
            }
            finally
            {
                TestManager.ClearNInjectKernel(kernel);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void UpdateComentarioRechazaUnUsuarioQueNoEsElAutor()
        {
            var kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                var contexto = kernel.Get<DbContext>();
                var comentarioService = kernel.Get<IComentarioService>();
                var usuarioService = kernel.Get<IUsuarioService>();

                using (var transaccion = contexto.Database.BeginTransaction())
                {
                    Producto producto = CrearProducto(
                        contexto,
                        usuarioService);

                    long otroUsuarioId = CrearUsuario(
                        usuarioService,
                        "otro_usuario");

                    var comentario = new Comentario
                    {
                        ProductoId = producto.ProductoId,
                        AutorId = producto.VendedorId,
                        Texto = "Texto original",
                        FechaCreacion = DateTime.UtcNow,
                        EnRevision = false
                    };

                    contexto.Set<Comentario>().Add(comentario);
                    contexto.SaveChanges();

                    comentarioService.UpdateComentario(
                        otroUsuarioId,
                        "Texto modificado por otro usuario",
                        comentario.ComentarioId);

                    transaccion.Rollback();
                }
            }
            finally
            {
                TestManager.ClearNInjectKernel(kernel);
            }
        }


        private static Producto CrearProducto(
            DbContext contexto,
            IUsuarioService usuarioService)
        {
            long vendedorId = CrearUsuario(
                usuarioService,
                "producto");

            var categoria = new Categoria
            {
                Nombre = "Categoría_" +
                    Guid.NewGuid().ToString("N").Substring(0, 8)
            };

            contexto.Set<Categoria>().Add(categoria);
            contexto.SaveChanges();

            var producto = new Producto
            {
                Nombre = "Producto_" +
                    Guid.NewGuid().ToString("N").Substring(0, 8),
                FechaAlta = DateTime.UtcNow,
                VendedorId = vendedorId,
                CategoriaId = categoria.CategoriaId
            };

            contexto.Set<Producto>().Add(producto);
            contexto.SaveChanges();

            return producto;
        }

        private static long CrearUsuario(
            IUsuarioService usuarioService,
            string prefijo)
        {
            var datosUsuario = new DatosPerfilUsuario
            {
                Nombre = "Usuario",
                Apellidos = "Prueba",
                Correo = prefijo + "_" +
                    Guid.NewGuid().ToString("N") +
                    "@example.invalid"
            };

            return usuarioService.RegistrarUsuario(
                prefijo + "_" +
                Guid.NewGuid().ToString("N").Substring(0, 12),
                "Clave123!",
                datosUsuario);
        }

    }
}
