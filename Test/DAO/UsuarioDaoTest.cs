using System;
using System.Data.Entity;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioDao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace Es.Udc.DotNet.PracticaMAD.Test
{
    [TestClass]
    public class UsuarioDaoTest
    {
        [TestMethod]
        public void FindByLoginEncuentraUsuarioYDevuelveNullSiNoExiste()
        {
            var kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                var context = kernel.Get<DbContext>();
                var dao = kernel.Get<IUsuarioDao>();
                var login = "test_" + Guid.NewGuid().ToString("N").Substring(0, 20);

                using (var transaction = context.Database.BeginTransaction())
                {
                    var usuario = new Usuario
                    {
                        Login = login,
                        HashContrasena = "hash-de-prueba",
                        Nombre = "Prueba",
                        Apellidos = "DAO",
                        Correo = "dao@example.invalid"
                    };

                    context.Set<Usuario>().Add(usuario);
                    context.SaveChanges();

                    context.Entry(usuario).State = EntityState.Detached;

                    var encontrado = dao.FindByLogin(login);
                    Assert.IsNotNull(encontrado);
                    Assert.AreEqual(login, encontrado.Login);

                    Assert.IsNull(dao.FindByLogin(login + "_no"));

                    transaction.Rollback();
                }
            }
            finally
            {
                TestManager.ClearNInjectKernel(kernel);
            }
        }
    }
}