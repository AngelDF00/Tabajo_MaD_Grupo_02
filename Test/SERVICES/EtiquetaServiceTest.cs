using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ComentarioService;
using Es.Udc.DotNet.PracticaMAD.Model.SERVICES.EtiquetaService;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace Es.Udc.DotNet.PracticaMAD.Test.SERVICES
{
    [TestClass]
    public class EtiquetaServiceTest
    {
        [TestMethod]
        public void CrearComentarioConEtiquetasReutilizaYCreaNuevasYGeneraNube()
        {
            IKernel kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                DbContext contexto = kernel.Get<DbContext>();
                IComentarioService comentarioService = kernel.Get<IComentarioService>();
                IEtiquetaService etiquetaService = kernel.Get<IEtiquetaService>();
                IUsuarioService usuarioService = kernel.Get<IUsuarioService>();

                using (DbContextTransaction transaccion = contexto.Database.BeginTransaction())
                {
                    Producto producto = CrearProducto(contexto, usuarioService);
                    long compradorId = CrearUsuario(usuarioService, "comprador_etiqueta");

                    List<string> etiquetasPrimerComentario = new List<string> { "ganga", "OFERTA" };
                    long comentarioId1 = comentarioService.CreateComentarioConEtiquetas(
                        compradorId, producto.ProductoId, "Muy buen producto y barato", etiquetasPrimerComentario);

                    List<Etiqueta> etiquetasAsociadas = etiquetaService.ObtenerEtiquetasPorComentario(comentarioId1);
                    Assert.AreEqual(2, etiquetasAsociadas.Count);

                    List<string> etiquetasSegundoComentario = new List<string> { "GANGA", "pesimo" };
                    long comentarioId2 = comentarioService.CreateComentarioConEtiquetas(
                        compradorId, producto.ProductoId, "No me ha gustado tanto", etiquetasSegundoComentario);

                    IDictionary<string, int> nubeEtiquetas = etiquetaService.ObtenerNubeEtiquetas();

                    Assert.IsNotNull(nubeEtiquetas);
                    Assert.IsTrue(nubeEtiquetas.ContainsKey("ganga"));
                    Assert.AreEqual(2, nubeEtiquetas["ganga"]);

                    Assert.IsTrue(nubeEtiquetas.ContainsKey("oferta"));
                    Assert.AreEqual(1, nubeEtiquetas["oferta"]);

                    Assert.IsTrue(nubeEtiquetas.ContainsKey("pesimo"));
                    Assert.AreEqual(1, nubeEtiquetas["pesimo"]);

                    transaccion.Rollback();
                }
            }
            finally
            {
                TestManager.ClearNInjectKernel(kernel);
            }
        }

        private static Producto CrearProducto(DbContext contexto, IUsuarioService usuarioService)
        {
            long vendedorId = CrearUsuario(usuarioService, "vendedor");

            Categoria categoria = new Categoria
            {
                Nombre = "Categoria_" + Guid.NewGuid().ToString("N").Substring(0, 8)
            };

            contexto.Set<Categoria>().Add(categoria);
            contexto.SaveChanges();

            Producto producto = new Producto
            {
                Nombre = "Producto_" + Guid.NewGuid().ToString("N").Substring(0, 8),
                FechaAlta = DateTime.UtcNow,
                VendedorId = vendedorId,
                CategoriaId = categoria.CategoriaId
            };

            contexto.Set<Producto>().Add(producto);
            contexto.SaveChanges();

            return producto;
        }

        private static long CrearUsuario(IUsuarioService usuarioService, string prefijo)
        {
            DatosPerfilUsuario datosUsuario = new DatosPerfilUsuario
            {
                Nombre = "Usuario",
                Apellidos = "Prueba",
                Correo = prefijo + "_" + Guid.NewGuid().ToString("N").Substring(0, 8) + "@example.invalid"
            };

            string loginCorto = (prefijo.Length > 4 ? prefijo.Substring(0, 4) : prefijo) + "_" + Guid.NewGuid().ToString("N").Substring(0, 6);

            return usuarioService.RegistrarUsuario(
                loginCorto,
                "Clave123!",
                datosUsuario);
        }
    }
}