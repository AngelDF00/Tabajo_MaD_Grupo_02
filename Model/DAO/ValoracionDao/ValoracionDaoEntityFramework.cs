using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;


namespace Es.Udc.DotNet.PracticaMAD.Model.DAO
{
    public class ValoracionDaoEntityFramework : GenericDaoEntityFramework<Valoracion, long>, IValoracionDao
    {
        public List<Valoracion> FindByVendedorId(long vendedorId, int startIndex, int count)
        {
            return Context.Set<Valoracion>()
                .Where(v => v.Producto.VendedorId == vendedorId)
                .OrderByDescending(v => v.FechaCreacion)
                .ThenByDescending(v => v.ValoracionId)
                .Skip(startIndex)
                .Take(count)
                .ToList();
        }

        public int GetTotalCountByVendedorId(long vendedorId)
        {
            return Context.Set<Valoracion>()
                .Count(v => v.Producto.VendedorId == vendedorId);
        }

        public double? GetAverageByVendedorId(long vendedorId)
        {
            var valoraciones = Context.Set<Valoracion>()
                .Where(v => v.Producto.VendedorId == vendedorId);

            if (!valoraciones.Any())
            {
                return null;
            }

            return valoraciones.Average(v => v.Voto);
        }
    }
}
