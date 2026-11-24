using Es.Udc.DotNet.PracticaMAD.Model.DAO;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioDao;
using System;
using System.Collections.Generic;

namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ValoracionService
{
    public class ValoracionService : IValoracionService
    {
        private readonly IValoracionDao valoracionDao;
        private readonly IProductoDao productoDao;
        private readonly IUsuarioDao usuarioDao;

        // Inyección por constructor (Ninject la detecta automáticamente)
        public ValoracionService(IValoracionDao valoracionDao, IProductoDao productoDao, IUsuarioDao usuarioDao)
        {
            this.valoracionDao = valoracionDao;
            this.productoDao = productoDao;
            this.usuarioDao = usuarioDao;
        }

        public long ValorarVendedor(long autorId, long productoId, int voto, string comentario)
        {
            if (voto < 1 || voto > 5)
            {
                throw new ArgumentException("El voto debe estar comprendido entre 1 y 5.");
            }

            Producto producto = productoDao.Find(productoId);
            if (producto == null)
            {
                throw new KeyNotFoundException("El producto especificado no existe.");
            }

            Usuario autor = usuarioDao.Find(autorId);
            if (autor == null)
            {
                throw new KeyNotFoundException("El usuario autor no existe.");
            }

            if (valoracionDao.ExistsByProductoAndAutor(productoId, autorId))
            {
                throw new InvalidOperationException(
                    "Este usuario ya ha valorado este producto.");
            }

            Valoracion valoracion = new Valoracion()
            {
                ProductoId = productoId,
                AutorId = autorId,
                Voto = voto,
                Comentario = comentario,
                FechaCreacion = DateTime.UtcNow
            };

            valoracionDao.Create(valoracion);
            return valoracion.ValoracionId;
        }

        public Block<Valoracion> ObtenerValoracionesVendedor(long vendedorId, int page, int count)
        {
            if (page < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(page), "La página debe ser mayor o igual que 1.");
            }

            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "El tamaño de página debe ser mayor o igual que 1.");
            }

            int startIndex = (page - 1) * count;
            List<Valoracion> items = valoracionDao.FindByVendedorId(vendedorId, startIndex, count + 1);

            bool existMoreItems = items.Count > count;
            if (existMoreItems)
            {
                items.RemoveAt(count);
            }

            return new Block<Valoracion>(existMoreItems, items);
        }

        public int ObtenerTotalValoracionesVendedor(long vendedorId)
        {
            return valoracionDao.GetTotalCountByVendedorId(vendedorId);
        }

        public double? ObtenerMediaValoracionesVendedor(long vendedorId)
        {
            return valoracionDao.GetAverageByVendedorId(vendedorId);
        }
    }
}
