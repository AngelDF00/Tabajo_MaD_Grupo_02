using System;
using Es.Udc.DotNet.ModelUtil.Exceptions;

namespace Es.Udc.DotNet.PracticaMAD.Model.UsuarioService
{
    public interface IUsuarioService
    {
        /// <exception cref="ArgumentException">Datos de registro inválidos.</exception>
        /// <exception cref="DuplicateInstanceException">El login ya existe.</exception>
        long RegistrarUsuario(string login, string contrasena,
            DatosPerfilUsuario datos);

        /// <summary>Devuelve null si el login o la contraseña no coinciden.</summary>
        /// <exception cref="ArgumentException">Datos de acceso inválidos.</exception>
        long? Autenticar(string login, string contrasena);

        /// <exception cref="InstanceNotFoundException">El usuario no existe.</exception>
        PerfilUsuario ObtenerPerfil(long usuarioId);

        /// <exception cref="InstanceNotFoundException">El usuario no existe.</exception>
        /// <exception cref="ArgumentException">Datos del perfil inválidos.</exception>
        void ActualizarPerfil(long usuarioId, DatosPerfilUsuario datos);

        /// <exception cref="InstanceNotFoundException">El usuario no existe.</exception>
        /// <exception cref="ArgumentException">Contraseña nueva inválida.</exception>
        /// <exception cref="ContrasenaActualIncorrectaException">
        /// La contraseña actual no coincide.
        /// </exception>
        void CambiarContrasena(long usuarioId, string contrasenaActual,
            string contrasenaNueva);
    }
}