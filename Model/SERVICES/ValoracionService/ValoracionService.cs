using Es.Udc.DotNet.PracticaMAD.Model.DAO;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ValoracionService
{
    public class ValoracionService : IValoracionService
    {
        public IValoracionDao ValoracionDao { get; set; }
        public IProductoDao ProductoDao { get; set; }
        public IUsuarioDao UsuarioDao { get; set; }

        public long ValorarVendedor(long autorId, long productoId, int voto, string comentario)
        {
            if (voto < 1 || voto > 5)
            {
                throw new ArgumentException("El voto debe estar comprendido entre 1 y 5.");
            }

            var producto = ProductoDao.Find(productoId);
            if (producto == null)
            {
                throw new Exception("El producto especificado no existe.");
            }

            var autor = UsuarioDao.Find(autorId);
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

            ValoracionDao.Create(valoracion);
            return valoracion.ValoracionId;
        }

        public ResultadoValoraciones ObtenerValoracionesVendedor(long vendedorId, int startIndex, int count)
        {
            var valoraciones = ValoracionDao.FindByVendedorId(vendedorId, startIndex, count);
            int total = ValoracionDao.GetTotalCountByVendedorId(vendedorId);

            return new ResultadoValoraciones(valoraciones, total);
        }

        public int ObtenerTotalValoracionesVendedor(long vendedorId)
        {
            return ValoracionDao.GetTotalCountByVendedorId(vendedorId);
        }

        public double? ObtenerMediaValoracionesVendedor(long vendedorId)
        {
            return ValoracionDao.GetAverageByVendedorId(vendedorId);
        }
    }
}