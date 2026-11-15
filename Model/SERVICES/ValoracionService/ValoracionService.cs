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
                throw new Exception("El producto especificado no existe.");
            }

            Usuario autor = usuarioDao.Find(autorId);
            if (autor == null)
            {
                throw new Exception("El usuario autor no existe.");
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

        public ResultadoValoraciones ObtenerValoracionesVendedor(long vendedorId, int page, int count)
        {
            int totalValoraciones = valoracionDao.GetTotalCountByVendedorId(vendedorId);
            double? mediaValoraciones = valoracionDao.GetAverageByVendedorId(vendedorId);

            int startIndex = (page - 1) * count;
            List<Valoracion> items = valoracionDao.FindByVendedorId(vendedorId, startIndex, count + 1);

            bool existMoreItems;
            if (existMoreItems = (items.Count > count))
            {
                items.RemoveAt(count);
            }

            Block<Valoracion> block = new Block<Valoracion>(existMoreItems, items);

            return new ResultadoValoraciones(items, totalValoraciones, mediaValoraciones, block);
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
