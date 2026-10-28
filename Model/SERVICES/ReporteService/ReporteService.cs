using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Es.Udc.DotNet.PracticaMAD.Model.DAO;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;

namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ReporteService
{
    public class ReporteService : IReporteService
    {
        public IReporteDao ReporteDao { get; set; }
        public IComentarioDao ComentarioDao { get; set; }

        public long ReportarComentario(long autorId, long comentarioId, string motivo, int umbralRevision)
        {
            if (ReporteDao.Exists(comentarioId, autorId))
            {
                throw new ComentarioYaReportadoException(comentarioId, autorId);
            }

            Reporte reporte = new Reporte()
            {
                ComentarioId = comentarioId,
                AutorId = autorId,
                Motivo = motivo,
                FechaCreacion = DateTime.UtcNow
            };

            ReporteDao.Create(reporte);

            int totalReportes = ReporteDao.CountByComentarioId(comentarioId);
            if (totalReportes >= umbralRevision)
            {
                var comentario = ComentarioDao.Find(comentarioId);
                if (comentario != null && !comentario.EnRevision)
                {
                    comentario.EnRevision = true;
                    ComentarioDao.Update(comentario);
                }
            }

            return reporte.ReporteId;
        }
    }
}