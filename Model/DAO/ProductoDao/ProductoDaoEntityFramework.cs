using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Es.Udc.DotNet.PracticaMAD.Model.DAO
{
    public class ProductoDaoEntityFramework: GenericDaoEntityFramework<Producto, long>, IProductoDao
    {
        public  List<Producto> FindByKeyword(String keyword, int startIndex, int count)
        {
            DbSet<Producto> productos = Context.Set<Producto>();

            var result =
                    (from p in productos
                     where p.Nombre.Contains(keyword)
                     orderby p.FechaAlta descending
                     select p).Skip(startIndex).Take(count).ToList();

            return result;
        }
    }
}
