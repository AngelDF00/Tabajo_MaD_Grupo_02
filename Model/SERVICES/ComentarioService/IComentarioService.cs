using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using System.Collections.Generic;

namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ComentarioService
{
    public interface IComentarioService
    {
        long CreateComentario(long userId, long productoId, string texto, List<string> nombresEtiquetas);

        void DeleteComentario(long userId, long comentarioId);

        void UpdateComentario(long userId, string nuevoTexto, long comentarioId);

        Block<Comentario> FindComentariosByProducto(long productoId, int page, int count);
    }
}
