using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using System.Collections.Generic;

namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ComentarioService
{
    public interface IComentarioService
    {
        long CreateComentario(long userId, long productoId, string texto, List<string> nombresEtiquetas);

        /// <exception cref="System.UnauthorizedAccessException">Se produce cuando el usuario no es el autor del comentario.</exception>
        void DeleteComentario(long userId, long comentarioId);

        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Se produce cuando no se encuentra el comentario.</exception>
        /// <exception cref="Exceptions.ComentarioEnRevisionException">Se produce cuando el comentario está en revisión.</exception>
        /// <exception cref="System.UnauthorizedAccessException">Se produce cuando el usuario no es el autor del comentario.</exception>
        void UpdateComentario(long userId, string nuevoTexto, long comentarioId);

        Block<Comentario> FindComentariosByProducto(long productoId, int page, int count);
    }
}
