using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Es.Udc.DotNet.PracticaMAD.Model.Entities;

namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ValoracionService
{
    public class ResultadoValoraciones
    {
        public List<Valoracion> Valoraciones { get; private set; }
        public int TotalValoraciones { get; private set; }

        public ResultadoValoraciones(List<Valoracion> valoraciones, int totalValoraciones)
        {
            Valoraciones = valoraciones;
            TotalValoraciones = totalValoraciones;
        }
    }
}