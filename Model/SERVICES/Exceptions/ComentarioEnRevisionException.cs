using System;
namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.Exceptions
{
    public class ComentarioEnRevisionException : Exception
    {
        public long ComentarioId { get; private set; }

        public ComentarioEnRevisionException(long comentarioId, string message)
            : base(message)
        {
            this.ComentarioId = comentarioId;
        }
    }
}
