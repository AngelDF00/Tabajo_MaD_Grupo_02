using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Es.Udc.DotNet.PracticaMAD.Model.Entities;

namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ValoracionService
{
    public interface IValoracionService
    {
        /// <exception cref="ArgumentException">El voto no está entre 1 y 5.</exception>
        /// <exception cref="KeyNotFoundException">El producto o el usuario autor no existen.</exception>
        /// <exception cref="InvalidOperationException">El usuario ya ha valorado el producto.</exception>
        long ValorarVendedor(long autorId, long productoId, int voto, string comentario);

        /// <exception cref="ArgumentOutOfRangeException">La página o el tamaño de página son menores que 1.</exception>
        Block<Valoracion> ObtenerValoracionesVendedor(long vendedorId, int page, int count);

        int ObtenerTotalValoracionesVendedor(long vendedorId);

        double? ObtenerMediaValoracionesVendedor(long vendedorId);
    }
}