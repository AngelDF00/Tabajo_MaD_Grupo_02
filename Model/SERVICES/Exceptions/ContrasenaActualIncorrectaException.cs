using System;

namespace Es.Udc.DotNet.PracticaMAD.Model.UsuarioService
{
    /// <summary>
    /// Excepción que se lanza cuando la contraseña actual introducida por el usuario no es correcta durante una operación de cambio de contraseña.
    /// </summary>
    public class ContrasenaActualIncorrectaException : Exception
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ContrasenaActualIncorrectaException"/>.
        /// </summary>
        public ContrasenaActualIncorrectaException()
            : base("La contraseña actual es incorrecta.")
        {
        }
    }
}