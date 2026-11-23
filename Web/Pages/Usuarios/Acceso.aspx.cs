using System;
using System.Globalization;
using System.Web.Security;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioDao;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioService;

namespace Es.Udc.DotNet.PracticaMAD.Web.Pages.Usuarios
{
    public partial class Acceso : System.Web.UI.Page
    {
        protected void CheckBox1_CheckedChanged(object sender, System.EventArgs e)
        {

        }

        protected void txtLogin_TextChanged(object sender, System.EventArgs e)
        {

        }

        protected void btnAcceder_Click(object sender, System.EventArgs e)
        {
            lblError.Text = string.Empty;

            string login = txtLogin.Text.Trim();
            string contrasena = txtContrasena.Text;

            if (string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(contrasena))
            {
                lblError.Text = "Introduce el usuario y la contraseña.";
                return;
            }

            long? usuarioId;

            try
            {
                using (MarketplaceEntities contexto = new MarketplaceEntities())
                {
                    UsuarioDaoEntityFramework usuarioDao =
                        new UsuarioDaoEntityFramework { Context = contexto };

                    IUsuarioService servicio =
                        new UsuarioService(usuarioDao, contexto);

                    usuarioId = servicio.Autenticar(login, contrasena);
                }
            }
            catch (ArgumentException)
            {
                lblError.Text = "El usuario o la contraseña no son válidos.";
                return;
            }

            if (!usuarioId.HasValue)
            {
                lblError.Text = "El usuario o la contraseña no son válidos.";
                return;
            }

            FormsAuthentication.RedirectFromLoginPage(
                usuarioId.Value.ToString(CultureInfo.InvariantCulture),
                Recuerdame.Checked);
        }

    }
}
