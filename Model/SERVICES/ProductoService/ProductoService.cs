using Es.Udc.DotNet.PracticaMAD.Model.DAO;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ProductoService
{
    public class ProductoService : IProductoService
    {
        private IProductoDao ProductoDao { get; set; }


        public Producto FindById (long productoId)
        {
            return ProductoDao.Find(productoId);
        }

        public List<Producto> FindProductosByKeyword (String keyword, int page, int count)
        {
            int startIndex = (page - 1) * count;
            return ProductoDao.FindByKeyword(keyword, startIndex, count);
        }

    }
}
