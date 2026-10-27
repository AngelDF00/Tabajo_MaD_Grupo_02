using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;


namespace Es.Udc.DotNet.PracticaMAD.Model.DAO
{
    public interface IValoracionDao : IGenericDao<Valoracion, long>
    {
        List<Valoracion> FindByVendedorId(long vendedorId, int startIndex, int count);
        int GetTotalCountByVendedorId(long vendedorId);
        double? GetAverageByVendedorId(long vendedorId);
    }
}
