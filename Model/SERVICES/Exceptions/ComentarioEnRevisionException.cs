using System;
namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.Exceptions
{
    public class ComentarioEnRevisionException : Exception
    {
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
        public long comentarioId { get; set; }

        public ComentarioEnRevisionException(long comentarioId, string message)
            : base(message)
        {
            this.comentarioId = comentarioId;
=======
>>>>>>> Stashed changes
        public long ComentarioId { get; private set; }

        public ComentarioEnRevisionException(long comentarioId)
            : base($"El comentario con ID {comentarioId} está en revisión y no se puede modificar.")
        {
            ComentarioId = comentarioId;
<<<<<<< Updated upstream
=======
>>>>>>> 21133f7ce0fb8243a6ecf76965eb989306767e41
>>>>>>> Stashed changes
        }
    }
}
