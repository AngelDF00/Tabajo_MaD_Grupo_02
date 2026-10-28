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
        long ValorarVendedor(long autorId, long productoId, int voto, string comentario);

        ResultadoValoraciones ObtenerValoracionesVendedor(long vendedorId, int startIndex, int count);

        int ObtenerTotalValoracionesVendedor(long vendedorId);

        double? ObtenerMediaValoracionesVendedor(long vendedorId);
    }
}