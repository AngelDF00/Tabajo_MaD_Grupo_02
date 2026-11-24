using System;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioDao;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioService;

namespace Es.Udc.DotNet.PracticaMAD.Web.Pages.Usuarios
{
    public partial class Registro : System.Web.UI.Page
    {
        protected void btnRegistrarse_Click(object sender, EventArgs e)
        {
            lblErrorReg.Text = string.Empty;

            string nombre = txtNombreReg.Text.Trim();
            string apellidos = txtApellidoReg.Text.Trim();
            string correo = txtCorreoReg.Text.Trim();
            string login = txtUserReg.Text.Trim();
            string contrasena = txtContrasenaReg1.Text;
            string confirmacion = txtContrasenaReg2.Text;

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(apellidos) ||
                string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(contrasena))
            {
                lblErrorReg.Text = "Completa todos los campos obligatorios.";
                return;
            }

            if (contrasena != confirmacion)
            {
                lblErrorReg.Text = "Las contraseñas no coinciden.";
                return;
            }

            DatosPerfilUsuario datos = new DatosPerfilUsuario
            {
                Nombre = nombre,
                Apellidos = apellidos,
                Correo = correo
            };

            try
            {
                using (MarketplaceEntities contexto = new MarketplaceEntities())
                {
                    UsuarioDaoEntityFramework usuarioDao =
                        new UsuarioDaoEntityFramework { Context = contexto };

                    IUsuarioService servicio =
                        new UsuarioService(usuarioDao, contexto);

                    servicio.RegistrarUsuario(login, contrasena, datos);
                }
            }
            catch (DuplicateInstanceException)
            {
                lblErrorReg.Text = "Ese nombre de usuario ya existe.";
                return;
            }
            catch (ArgumentException)
            {
                lblErrorReg.Text = "Revisa los datos y su longitud.";
                return;
            }

            Session["RegistroCorrecto"] = true;
            Response.Redirect(ResolveUrl("~/Pages/Usuarios/Acceso.aspx"));
        }

    }
}
