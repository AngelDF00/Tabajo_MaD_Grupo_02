using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMAD.Model.DAO
{
    public interface IComentarioDao : IGenericDao<Comentario, long>
    {
        List<Comentario> FindByProductoIdOrderByFechaCreacionDesc(long productoId, int startIndex, int count);
    }
}
