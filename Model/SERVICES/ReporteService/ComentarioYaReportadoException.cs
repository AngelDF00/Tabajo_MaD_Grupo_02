using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ReporteService
{
    public class ComentarioYaReportadoException : Exception
    {
        public long ComentarioId { get; private set; }
        public long AutorId { get; private set; }

        public ComentarioYaReportadoException(long comentarioId, long autorId)
            : base($"El usuario con ID {autorId} ya ha reportado el comentario con ID {comentarioId}.")
        {
            ComentarioId = comentarioId;
            AutorId = autorId;
        }
    }
}