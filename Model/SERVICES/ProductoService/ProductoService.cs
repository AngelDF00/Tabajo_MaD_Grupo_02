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
        private readonly IProductoDao productoDao;

        public ProductoService(IProductoDao productoDao)
        {
            this.productoDao = productoDao;
        }
        public Producto FindById (long productoId)
        {
            return productoDao.Find(productoId);
        }

        public List<Producto> FindProductosByKeyword (String keyword, int page, int count)
        {
            int startIndex = (page - 1) * count;
            return productoDao.FindByKeyword(keyword, startIndex, count);
        }

    }
}
