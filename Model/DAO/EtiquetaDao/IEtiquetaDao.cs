using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMAD.Model.DAO.EtiquetaDao
{
    public interface IEtiquetaDao : IGenericDao<Etiqueta, long>
    {
        Etiqueta FindByName(string nombre);
        IDictionary<string, int> GetNubeEtiquetas();
        List<Etiqueta> FindByComentarioId(long comentarioId);
    }
}
