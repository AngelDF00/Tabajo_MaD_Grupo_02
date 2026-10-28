using Es.Udc.DotNet.PracticaMAD.Model.DAO;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using System;
using System.Collections.Generic;

namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ComentarioService
{
    public class ComentarioService : IComentarioService
    {
        private IComentarioDao comentarioDao {  get; set; }

        public void CreateComentario (Comentario comentario)
        {
            comentarioDao.Create(comentario);
        }

        public void DeleteComentario (long userId, long comentarioId)
        {
            Comentario comentario = comentarioDao.Find(comentarioId);

            if (comentario.AutorId != userId)
            {
                throw new UnauthorizedAccessException("ComentarioService.DeleteComentario.PermissionException");
            }

            comentarioDao.Remove(comentarioId);
        }

        public void UpdateComentario (long userId, Comentario comentario)
        {
            //Accedemos a base de datos para saber el autorId del comentario en BD es el del usuario  por seguridad
            Comentario comentarioInDb = comentarioDao.Find(comentario.ComentarioId)
            if (comentarioInDb.AutorId != userId)
            {
                throw new UnauthorizedAccessException("ComentarioService.UpdateComentario.PermissionException");
            }

            //Por seguridad nos aseguramos de que el AutorId siga siendo el mismo
            comentario.AutorId = comentarioInDb.AutorId;
            
            comentarioDao.Update(comentario);
        }

        public List<Comentario> FindComentariosByProducto (long productoId, int page, int count)
        {
            int startIndex = (page - 1) * count;
            return comentarioDao.FindByProductoIdOrderByFechaCreacionDesc(productoId, startIndex, count);
        }

    }
}
