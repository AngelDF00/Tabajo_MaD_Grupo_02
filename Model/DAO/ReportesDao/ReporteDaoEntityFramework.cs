using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;


namespace Es.Udc.DotNet.PracticaMAD.Model.DAO
{
    public class ReporteDaoEntityFramework : GenericDaoEntityFramework<Reporte, long>, IReporteDao
    {
        public bool Exists(long comentarioId, long autorId)
        {
            return Context.Set<Reporte>()
                .Any(r => r.ComentarioId == comentarioId && r.AutorId == autorId);
        }

        public int CountByComentarioId(long comentarioId)
        {
            return Context.Set<Reporte>()
                .Count(r => r.ComentarioId == comentarioId);
        }
    }
}