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
        public int TotalValoraciones { get; private set; }
        public double? MediaValoraciones { get; private set; }

        public ResultadoValoraciones(int totalValoraciones, double? mediaValoraciones)
        {
            TotalValoraciones = totalValoraciones;
            MediaValoraciones = mediaValoraciones;
        }
    }
}