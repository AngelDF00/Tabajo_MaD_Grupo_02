using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using System.Collections.Generic;

namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ComentarioService
{
    public interface IComentarioService
    {
        void CreateComentario(Comentario comentario);

        void DeleteComentario(long userId, long comentarioId);

        void UpdateComentario(long userId, string nuevoTexto, long comentarioId);

        List<Comentario> FindComentariosByProducto(long productoId, int page, int count);
    }
}
