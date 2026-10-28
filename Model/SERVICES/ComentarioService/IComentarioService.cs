using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ComentarioService
{
    public interface IComentarioService
    {
        void CreateComentario(Comentario comentario);

        void DeleteComentario(long userId, long comentarioId);

        void UpdateComentario(long userId, Comentario comentario);

        List<Comentario> FindComentariosByProducto(long productoId, int page, int count);

    }
}
