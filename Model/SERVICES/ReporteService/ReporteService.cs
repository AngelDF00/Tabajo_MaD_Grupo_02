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
        private readonly IReporteDao reporteDao;
        private readonly IComentarioDao comentarioDao;

        // Inyección por constructor para que Ninject le pase las instancias automáticamente
        public ReporteService(IReporteDao reporteDao, IComentarioDao comentarioDao)
        {
            this.reporteDao = reporteDao;
            this.comentarioDao = comentarioDao;
        }

        public long ReportarComentario(long autorId, long comentarioId, string motivo, int umbralRevision)
        {
            if (reporteDao.Exists(comentarioId, autorId))
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

            reporteDao.Create(reporte);

            int totalReportes = reporteDao.CountByComentarioId(comentarioId);
            if (totalReportes >= umbralRevision)
            {
                var comentario = comentarioDao.Find(comentarioId);
                if (comentario != null && !comentario.EnRevision)
                {
                    comentario.EnRevision = true;
                    comentarioDao.Update(comentario);
                }
            }

            return reporte.ReporteId;
        }
    }
}