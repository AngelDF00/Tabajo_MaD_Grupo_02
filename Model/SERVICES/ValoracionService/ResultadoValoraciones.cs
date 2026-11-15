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
        public double? MediaValoraciones { get; private set; }
        public Block<Valoracion> ValoracionesBlock { get; set; }

        public ResultadoValoraciones(List<Valoracion> valoraciones, int totalValoraciones, double? mediaValoraciones, Block<Valoracion> valoracionesBlock)
        {
            Valoraciones = valoraciones;
            TotalValoraciones = totalValoraciones;
            MediaValoraciones = mediaValoraciones;
            ValoracionesBlock = valoracionesBlock;
        }
    }
}
