using System;
using System.Data.Entity;
using System.Linq;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace Es.Udc.DotNet.PracticaMAD.Test
{
    [TestClass]
    public class UsuarioServiceTest
    {
        [TestMethod]
        public void RegistrarUsuarioGuardaHashYRechazaDuplicado()
        {
            var kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                var contexto = kernel.Get<DbContext>();
                var servicio = kernel.Get<IUsuarioService>();
                var login = "registro_" +
                    Guid.NewGuid().ToString("N").Substring(0, 12);
                const string contrasena = "Clave123!";

                var datos = new DatosPerfilUsuario
                {
                    Nombre = "Prueba",
                    Apellidos = "Registro",
                    Correo = "registro@example.invalid",
                    Idioma = "es",
                    Pais = "España"
                };

                using (var transaccion = contexto.Database.BeginTransaction())
                {
                    long usuarioId = servicio.RegistrarUsuario(
                        login, contrasena, datos);

                    var guardado = contexto.Set<Usuario>()
                        .AsNoTracking()
                        .SingleOrDefault(u => u.UsuarioId == usuarioId);

                    Assert.IsNotNull(guardado);
                    Assert.IsTrue(usuarioId > 0);
                    Assert.AreEqual(login, guardado.Login);
                    Assert.AreEqual(datos.Nombre, guardado.Nombre);
                    Assert.AreEqual(datos.Correo, guardado.Correo);
                    Assert.AreNotEqual(contrasena, guardado.HashContrasena);
                    StringAssert.StartsWith(
                        guardado.HashContrasena, "PBKDF2-SHA256$100000$");

                    bool duplicadoDetectado = false;

                    try
                    {
                        servicio.RegistrarUsuario(
                            login.ToUpperInvariant(), contrasena, datos);
                    }
                    catch (DuplicateInstanceException)
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

        [TestMethod]
        public void AutenticarDevuelveIdSoloConCredencialesCorrectas()
        {
            var kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                var contexto = kernel.Get<DbContext>();
                var servicio = kernel.Get<IUsuarioService>();
                var login = "auth_" +
                    Guid.NewGuid().ToString("N").Substring(0, 12);
                const string contrasena = "Clave123!";

                var datos = new DatosPerfilUsuario
                {
                    Nombre = "Prueba",
                    Apellidos = "Autenticación",
                    Correo = "auth@example.invalid"
                };

                using (var transaccion = contexto.Database.BeginTransaction())
                {
                    long usuarioId = servicio.RegistrarUsuario(
                        login, contrasena, datos);

                    Assert.AreEqual(
                        (long?)usuarioId,
                        servicio.Autenticar(login.ToUpperInvariant(), contrasena));

                    Assert.IsNull(
                        servicio.Autenticar(login, "ClaveIncorrecta!"));

                    Assert.IsNull(
                        servicio.Autenticar(login + "_ausente", contrasena));

                    transaccion.Rollback();
                }
            }
            finally
            {
                TestManager.ClearNInjectKernel(kernel);
            }
        }

        [TestMethod]
        public void ObtenerPerfilDevuelveDatosYRechazaIdInexistente()
        {
            var kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                var contexto = kernel.Get<DbContext>();
                var servicio = kernel.Get<IUsuarioService>();
                var login = "perfil_" +
                    Guid.NewGuid().ToString("N").Substring(0, 12);

                var datos = new DatosPerfilUsuario
                {
                    Nombre = "Prueba",
                    Apellidos = "Perfil",
                    Correo = "perfil@example.invalid",
                    Idioma = "es",
                    Pais = "España"
                };

                using (var transaccion = contexto.Database.BeginTransaction())
                {
                    long usuarioId = servicio.RegistrarUsuario(
                        login, "Clave123!", datos);

                    PerfilUsuario perfil = servicio.ObtenerPerfil(usuarioId);

                    Assert.AreEqual(usuarioId, perfil.UsuarioId);
                    Assert.AreEqual(login, perfil.Login);
                    Assert.AreEqual(datos.Nombre, perfil.Nombre);
                    Assert.AreEqual(datos.Apellidos, perfil.Apellidos);
                    Assert.AreEqual(datos.Correo, perfil.Correo);
                    Assert.AreEqual(datos.Idioma, perfil.Idioma);
                    Assert.AreEqual(datos.Pais, perfil.Pais);

                    bool noEncontrado = false;

                    try
                    {
                        servicio.ObtenerPerfil(long.MaxValue);
                    }
                    catch (InstanceNotFoundException)
                    {
                        noEncontrado = true;
                    }

                    Assert.IsTrue(noEncontrado);
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