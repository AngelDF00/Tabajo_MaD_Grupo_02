using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;


namespace Es.Udc.DotNet.PracticaMAD.Model.DAO
{
    public interface IReporteDao : IGenericDao<Reporte, long>
    {
        bool Exists(long comentarioId, long autorId);
        int CountByComentarioId(long comentarioId);
    }
}