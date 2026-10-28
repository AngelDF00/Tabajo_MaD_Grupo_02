using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ReporteService
{
    public interface IReporteService
    {
        long ReportarComentario(long autorId, long comentarioId, string motivo, int umbralRevision);
    }
}