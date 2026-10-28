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

            var producto = productoDao.Find(productoId);
            if (producto == null)
            {
                throw new Exception("El producto especificado no existe.");
            }

            var autor = usuarioDao.Find(autorId);
            if (autor == null)
            {
                throw new Exception("El usuario autor no existe.");
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

        public ResultadoValoraciones ObtenerValoracionesVendedor(long vendedorId, int startIndex, int count)
        {
            var valoraciones = valoracionDao.FindByVendedorId(vendedorId, startIndex, count);
            int total = valoracionDao.GetTotalCountByVendedorId(vendedorId);

            return new ResultadoValoraciones(valoraciones, total);
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
