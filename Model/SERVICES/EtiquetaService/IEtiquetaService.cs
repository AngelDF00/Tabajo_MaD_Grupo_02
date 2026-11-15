using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;

namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.EtiquetaService
{
    public interface IEtiquetaService
    {
        IDictionary<string, int> ObtenerNubeEtiquetas();
        List<Etiqueta> ObtenerEtiquetasPorComentario(long comentarioId);
    }
}