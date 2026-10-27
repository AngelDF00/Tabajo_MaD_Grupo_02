using System;

namespace Es.Udc.DotNet.PracticaMAD.Model.UsuarioService
{
    public class ContrasenaActualIncorrectaException : Exception
    {
        public ContrasenaActualIncorrectaException()
            : base("La contraseña actual es incorrecta.")
        {
        }
    }
}