using System;

namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.Exceptions
{
    /// <summary>
    /// Excepción que se lanza cuando se intenta realizar una acción sobre un comentario que se encuentra actualmente en revisión.
    /// </summary>
    public class ComentarioEnRevisionException : Exception
    {
        /// <summary>
        /// Obtiene el identificador del comentario que está en revisión.
        /// </summary>
        public long ComentarioId { get; private set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ComentarioEnRevisionException"/>.
        /// </summary>
        /// <param name="comentarioId">El identificador del comentario.</param>
        /// <param name="message">El mensaje que describe el error.</param>
        /// <exception cref="ArgumentNullException">Se produce si el mensaje proporcionado es nulo.</exception>
        public ComentarioEnRevisionException(long comentarioId, string message)
            : base(message)
        {
            this.ComentarioId = comentarioId;
        }
    }
}