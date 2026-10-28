using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ProductoService
{
    public interface IProductoService
    {
        List<Producto> FindProductosByKeyword (String keyword, int page, int count);

        Producto FindById(long productoId);
    }
}
