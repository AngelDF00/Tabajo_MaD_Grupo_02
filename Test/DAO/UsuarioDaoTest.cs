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
            IKernel kernel = TestManager.ConfigureNInjectKernel();

            try
            {
                DbContext context = kernel.Get<DbContext>();
                IUsuarioDao dao = kernel.Get<IUsuarioDao>();
                string login = "test_" + Guid.NewGuid().ToString("N").Substring(0, 20);

                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    Usuario usuario = new Usuario
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

                    Usuario encontrado = dao.FindByLogin(login);
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