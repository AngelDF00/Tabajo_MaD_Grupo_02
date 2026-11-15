using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;

namespace Es.Udc.DotNet.PracticaMAD.Model.DAO.EtiquetaDao
{
    public class EtiquetaDaoEntityFramework : GenericDaoEntityFramework<Etiqueta, long>, IEtiquetaDao
    {
        public Etiqueta FindByName(string nombre)
        {
            string nombreNormalizado = nombre.Trim().ToLower();

            return Context.Set<Etiqueta>()
                .SingleOrDefault(e => e.Nombre.ToLower() == nombreNormalizado);
        }

        public IDictionary<string, int> GetNubeEtiquetas()
        {
            var query = Context.Set<Etiqueta>()
                .Select(e => new
                {
                    NombreEtiqueta = e.Nombre,
                    Contador = e.Comentario.Count()
                })
                .OrderByDescending(x => x.Contador)
                .ToDictionary(x => x.NombreEtiqueta, x => x.Contador);

            return query;
        }

        public List<Etiqueta> FindByComentarioId(long comentarioId)
        {
            Comentario comentario = Context.Set<Comentario>()
                .Include("Etiqueta")
                .SingleOrDefault(c => c.ComentarioId == comentarioId);

            if (comentario == null || comentario.Etiqueta == null)
            {
                return new List<Etiqueta>();
            }

            return comentario.Etiqueta.ToList();
        }
    }
}
