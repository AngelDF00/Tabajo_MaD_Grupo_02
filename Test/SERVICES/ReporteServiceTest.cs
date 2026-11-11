using Es.Udc.DotNet.PracticaMAD.Model.DAO;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ReporteService;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMAD.Test
{
    [TestClass]
    public class ReporteServiceTest
    {
        [TestMethod]
        public void ReportarComentarioYAlcanzarUmbralRevision()
        {
            IKernel kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                DbContext contexto = kernel.Get<DbContext>();
                IReporteService reporteService = kernel.Get<IReporteService>();
                IUsuarioService usuarioService = kernel.Get<IUsuarioService>();
                IComentarioDao comentarioDao = kernel.Get<IComentarioDao>();

                string sufijo = Guid.NewGuid().ToString("N").Substring(0, 8);
                DatosPerfilUsuario datos = new DatosPerfilUsuario { Nombre = "T", Apellidos = "T", Correo = "t@example.invalid" };

                using (DbContextTransaction transaccion = contexto.Database.BeginTransaction())
                {
                    long autorVendedorId = usuarioService.RegistrarUsuario("vend_" + sufijo, "Clave123!", datos);
                    long autorComentarioId = usuarioService.RegistrarUsuario("comen_" + sufijo, "Clave123!", datos);
                    long autorReporte1Id = usuarioService.RegistrarUsuario("rep1_" + sufijo, "Clave123!", datos);
                    long autorReporte2Id = usuarioService.RegistrarUsuario("rep2_" + sufijo, "Clave123!", datos);

                    Categoria categoria = new Categoria { Nombre = "Cat_" + sufijo };
                    contexto.Set<Categoria>().Add(categoria);
                    contexto.SaveChanges();

                    Producto producto = new Producto { Nombre = "Prod_" + sufijo, VendedorId = autorVendedorId, CategoriaId = categoria.CategoriaId };
                    contexto.Set<Producto>().Add(producto);
                    contexto.SaveChanges();

                    Comentario comentario = new Comentario { ProductoId = producto.ProductoId, AutorId = autorComentarioId, Texto = "Comentario de prueba", EnRevision = false };
                    contexto.Set<Comentario>().Add(comentario);
                    contexto.SaveChanges();

                    int umbralRevision = 2;

                    long reporte1Id = reporteService.ReportarComentario(autorReporte1Id, comentario.ComentarioId, "Ofensivo", umbralRevision);
                    Assert.IsTrue(reporte1Id > 0);

                    Comentario comentarioActualizado = comentarioDao.Find(comentario.ComentarioId);
                    Assert.IsFalse(comentarioActualizado.EnRevision);

                    long reporte2Id = reporteService.ReportarComentario(autorReporte2Id, comentario.ComentarioId, "Spam", umbralRevision);
                    Assert.IsTrue(reporte2Id > 0);

                    comentarioActualizado = comentarioDao.Find(comentario.ComentarioId);
                    Assert.IsTrue(comentarioActualizado.EnRevision);

                    transaccion.Rollback();
                }
            }
            finally
            {
                TestManager.ClearNInjectKernel(kernel);
            }
        }

        [TestMethod]
        public void ReportarComentarioDuplicadoLanzaExcepcion()
        {
            IKernel kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                DbContext contexto = kernel.Get<DbContext>();
                IReporteService reporteService = kernel.Get<IReporteService>();
                IUsuarioService usuarioService = kernel.Get<IUsuarioService>();

                string sufijo = Guid.NewGuid().ToString("N").Substring(0, 8);
                DatosPerfilUsuario datos = new DatosPerfilUsuario { Nombre = "T", Apellidos = "T", Correo = "t@example.invalid" };

                using (DbContextTransaction transaccion = contexto.Database.BeginTransaction())
                {
                    long vendedorId = usuarioService.RegistrarUsuario("vend_dup_" + sufijo, "Clave123!", datos);
                    long autorComentarioId = usuarioService.RegistrarUsuario("com_dup_" + sufijo, "Clave123!", datos);
                    long reportadorId = usuarioService.RegistrarUsuario("rep_dup_" + sufijo, "Clave123!", datos);

                    Categoria categoria = new Categoria { Nombre = "Cat_dup_" + sufijo };
                    contexto.Set<Categoria>().Add(categoria);
                    contexto.SaveChanges();

                    Producto producto = new Producto { Nombre = "Prod_dup_" + sufijo, VendedorId = vendedorId, CategoriaId = categoria.CategoriaId };
                    contexto.Set<Producto>().Add(producto);
                    contexto.SaveChanges();

                    Comentario comentario = new Comentario { ProductoId = producto.ProductoId, AutorId = autorComentarioId, Texto = "Texto", EnRevision = false };
                    contexto.Set<Comentario>().Add(comentario);
                    contexto.SaveChanges();

                    reporteService.ReportarComentario(reportadorId, comentario.ComentarioId, "Motivo 1", 3);

                    bool duplicadoDetectado = false;

                    try
                    {
                        reporteService.ReportarComentario(reportadorId, comentario.ComentarioId, "Motivo 2", 3);
                    }
                    catch (ComentarioYaReportadoException)
                    {
                        duplicadoDetectado = true;
                    }

                    Assert.IsTrue(duplicadoDetectado);
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