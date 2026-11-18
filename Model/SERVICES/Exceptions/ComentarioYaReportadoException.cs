using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ReporteService
{
    /// <summary>
    /// Excepción que se lanza cuando un usuario intenta reportar un comentario que ya había reportado previamente.
    /// </summary>
    public class ComentarioYaReportadoException : Exception
    {
        /// <summary>
        /// Obtiene el identificador del comentario.
        /// </summary>
        public long ComentarioId { get; private set; }

        /// <summary>
        /// Obtiene el identificador del autor del reporte.
        /// </summary>
        public long AutorId { get; private set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ComentarioYaReportadoException"/>.
        /// </summary>
        /// <param name="comentarioId">El identificador del comentario.</param>
        /// <param name="autorId">El identificador del autor.</param>
        public ComentarioYaReportadoException(long comentarioId, long autorId)
            : base($"El usuario con ID {autorId} ya ha reportado el comentario con ID {comentarioId}.")
        {
            ComentarioId = comentarioId;
            AutorId = autorId;
        }
    }
}