using System.Linq;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Es.Udc.DotNet.PracticaMAD.Test
{
    [TestClass]
    public class EntityModelMappingTest
    {
        [TestMethod]
        public void GeneratedModelReadsTestDatabase()
        {
            using (var context = new MarketplaceEntities())
            {
                Assert.IsTrue(context.Database.Exists());

                // Una consulta por entidad comprueba el mapeo sin modificar los datos.
                context.Usuario.Take(1).ToList();
                context.Categoria.Take(1).ToList();
                context.Producto.Take(1).ToList();
                context.Comentario.Take(1).ToList();
                context.Reporte.Take(1).ToList();
                context.Valoracion.Take(1).ToList();
            }
        }
    }
}
