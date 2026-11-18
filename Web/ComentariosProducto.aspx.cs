using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.PracticaMAD.Model.DAO;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Es.Udc.DotNet.PracticaMAD.Model.SERVICES;
using Es.Udc.DotNet.PracticaMAD.Model.SERVICES.ComentarioService;

namespace Es.Udc.DotNet.PracticaMAD.Web
{
    public partial class ComentariosProducto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long productoId;

            if (!long.TryParse(Request.QueryString["productoId"], out productoId)
                || productoId <= 0)
            {
                lblEstado.Text = "Indica un identificador de producto válido.";
                return;
            }

            using (MarketplaceEntities contexto = new MarketplaceEntities())
            {
                ComentarioDaoEntityFramework comentarioDao =
                    new ComentarioDaoEntityFramework { Context = contexto };

                IComentarioService servicio =
                    new ComentarioService(comentarioDao);

                Block<Comentario> resultado =
                    servicio.FindComentariosByProducto(productoId, 1, 5);

                gvComentarios.DataSource = resultado.items
                    .Select(c => new { c.Texto, c.FechaCreacion })
                    .ToList();

                gvComentarios.DataBind();

                if (resultado.items.Count == 0)
                {
                    lblEstado.Text = "Este producto no tiene comentarios.";
                }
            }
        }
    }
}