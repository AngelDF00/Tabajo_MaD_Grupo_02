using System;
using System.Data.Entity;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.PracticaMAD.Model.Entities;
using Es.Udc.DotNet.PracticaMAD.Model.UsuarioDao;

namespace Es.Udc.DotNet.PracticaMAD.Model.UsuarioService
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioDao usuarioDao;
        private readonly DbContext contexto;

        public UsuarioService(IUsuarioDao usuarioDao, DbContext contexto)
        {
            this.usuarioDao = usuarioDao;
            this.contexto = contexto;
        }

        public long RegistrarUsuario(
            string login, string contrasena, DatosPerfilUsuario datos)
        {
            if (datos == null)
                throw new ArgumentNullException(nameof(datos));

            ValidarRequerido(login, 30, nameof(login));
            ValidarRequerido(contrasena, int.MaxValue, nameof(contrasena));
            ValidarRequerido(datos.Nombre, 50, nameof(datos.Nombre));
            ValidarRequerido(datos.Apellidos, 100, nameof(datos.Apellidos));
            ValidarRequerido(datos.Correo, 254, nameof(datos.Correo));

            if (datos.Idioma != null && datos.Idioma.Length > 10)
                throw new ArgumentException("Idioma demasiado largo.", nameof(datos));

            if (datos.Pais != null && datos.Pais.Length > 100)
                throw new ArgumentException("País demasiado largo.", nameof(datos));

            string loginNormalizado = login.Trim().ToLowerInvariant();

            if (usuarioDao.FindByLogin(loginNormalizado) != null)
                throw new DuplicateInstanceException(loginNormalizado, "Usuario");

            var usuario = new Usuario
            {
                Login = loginNormalizado,
                HashContrasena = ContrasenaHash.Crear(contrasena),
                Nombre = datos.Nombre.Trim(),
                Apellidos = datos.Apellidos.Trim(),
                Correo = datos.Correo.Trim(),
                Idioma = datos.Idioma,
                Pais = datos.Pais
            };

            usuarioDao.Create(usuario);
            contexto.SaveChanges();

            return usuario.UsuarioId;
        }

        private static void ValidarRequerido(
            string valor, int longitudMaxima, string nombreParametro)
        {
            if (string.IsNullOrWhiteSpace(valor) || valor.Length > longitudMaxima)
                throw new ArgumentException(
                    "El campo es obligatorio o supera la longitud permitida.",
                    nombreParametro);
        }

        public long? Autenticar(string login, string contrasena)
        {
            throw new NotImplementedException();
        }

        public PerfilUsuario ObtenerPerfil(long usuarioId)
        {
            throw new NotImplementedException();
        }

        public void ActualizarPerfil(long usuarioId, DatosPerfilUsuario datos)
        {
            throw new NotImplementedException();
        }

        public void CambiarContrasena(
            long usuarioId, string contrasenaActual, string contrasenaNueva)
        {
            throw new NotImplementedException();
        }
    }
}