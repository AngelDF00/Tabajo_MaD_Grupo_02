using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Es.Udc.DotNet.PracticaMAD.Model.DAO.EtiquetaDao;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;

namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.EtiquetaService
{
    public class EtiquetaService : IEtiquetaService
    {
        private readonly IEtiquetaDao etiquetaDao;

        public EtiquetaService(IEtiquetaDao etiquetaDao)
        {
            this.etiquetaDao = etiquetaDao;
        }

        public IDictionary<string, int> ObtenerNubeEtiquetas()
        {
            IDictionary<string, int> nubeEtiquetas = etiquetaDao.GetNubeEtiquetas();

            return nubeEtiquetas;
        }

        public List<Etiqueta> ObtenerEtiquetasPorComentario(long comentarioId)
        {
            List<Etiqueta> etiquetas = etiquetaDao.FindByComentarioId(comentarioId);

            return etiquetas;
        }
    }
}