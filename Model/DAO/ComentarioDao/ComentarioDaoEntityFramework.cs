using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMAD.Model.DAO
{
    public class ComentarioDaoEntityFramework: GenericDaoEntityFramework<Comentario, long>, IComentarioDao
    {
        public List<Comentario> FindByProductoIdOrderByFechaCreacionDesc(long productoId, int startIndex, int count)
        {
            DbSet<Comentario> comentarios = Context.Set<Comentario>();

            var result = (from c in comentarios
                          where c.ProductoId == productoId
                          orderby c.FechaCreacion descending
                          select c).Skip(startIndex).Take(count).ToList();

            return result;
        }
    }
}
