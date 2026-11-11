using System;
namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.Exceptions
{
    public class ComentarioEnRevisionException : Exception
    {
        public long ComentarioId { get; private set; }

        public ComentarioEnRevisionException(long comentarioId)
            : this(comentarioId, $"El comentario con ID {comentarioId} está en revisión y no se puede modificar.")
        {
        }

        public ComentarioEnRevisionException(long comentarioId, string message)
            : base(message)
        {
            ComentarioId = comentarioId;
        }
    }
}
