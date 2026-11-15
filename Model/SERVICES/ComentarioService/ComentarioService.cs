using Es.Udc.DotNet.PracticaMAD.Model.DAO;
using Es.Udc.DotNet.PracticaMAD.Model.DAO.EtiquetaDao;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Es.Udc.DotNet.PracticaMAD.Model.SERVICES.Exceptions;
using System;
using System.Collections.Generic;

namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ComentarioService
{
    public class ComentarioService : IComentarioService
    {
        private readonly IComentarioDao comentarioDao;
        private readonly IEtiquetaDao etiquetaDao;

        public ComentarioService(IComentarioDao comentarioDao)
        {
            this.comentarioDao = comentarioDao;
        }

        public ComentarioService(IComentarioDao comentarioDao, IEtiquetaDao etiquetaDao)
        {
            this.comentarioDao = comentarioDao;
            this.etiquetaDao = etiquetaDao;
        }

        public void CreateComentario (Comentario comentario)
        {
            comentarioDao.Create(comentario);
        }

        public long CreateComentarioConEtiquetas(long userId, long productoId, string texto, List<string> nombresEtiquetas)
        {
            Comentario comentario = new Comentario()
            {
                ProductoId = productoId,
                AutorId = userId,
                Texto = texto,
                FechaCreacion = DateTime.UtcNow,
                EnRevision = false
            };

            comentarioDao.Create(comentario);

            if (nombresEtiquetas != null && nombresEtiquetas.Count > 0)
            {
                foreach (string nombre in nombresEtiquetas)
                {
                    string nombreLimpio = nombre.Trim().ToLower();
                    if (string.IsNullOrEmpty(nombreLimpio))
                    {
                        continue;
                    }

                    Etiqueta etiqueta = etiquetaDao.FindByName(nombreLimpio);

                    if (etiqueta == null)
                    {
                        etiqueta = new Etiqueta() { Nombre = nombreLimpio };
                        etiquetaDao.Create(etiqueta);
                    }

                    comentario.Etiqueta.Add(etiqueta);
                }
                comentarioDao.Update(comentario);
            }
            return comentario.ComentarioId;
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

        public void UpdateComentario (long userId, string nuevoTexto, long comentarioId)
        {


            //Accedemos a base de datos para saber el autorId del comentario en BD es el del usuario  por seguridad y demas operaciones
            Comentario comentario = comentarioDao.Find(comentarioId);

            if (comentario == null) //verificar si existe
            {
                throw new KeyNotFoundException("ComentarioService.UpdateComentario.KeyNotFoundException");
            }

            if (comentario.EnRevision)
            {
                throw new ComentarioEnRevisionException(comentario.ComentarioId, "ComentarioService.UpdateComentario.ComentarioEnRevisionException");
            }

            if (comentario.AutorId != userId)
            {
                throw new UnauthorizedAccessException("ComentarioService.UpdateComentario.PermissionException");
            }

            
            comentario.Texto = nuevoTexto;

            comentarioDao.Update(comentario);
        }

        public Block<Comentario> FindComentariosByProducto (long productoId, int page, int count)
        {
            int startIndex = (page - 1) * count;
            List<Comentario> items = comentarioDao.FindByProductoIdOrderByFechaCreacionDesc(productoId, startIndex, count + 1);
            Boolean existMoreItems;
            if (existMoreItems = (items.Count > count))
            {
                items.RemoveAt(count);
            }

            return new Block<Comentario>(existMoreItems, items);

        }

    }
}
