using System;
using System.Security.Cryptography;

namespace Es.Udc.DotNet.PracticaMAD.Model.UsuarioService
{
    internal static class ContrasenaHash
    {
        public static string Crear(string contrasena)
        {
            byte[] sal = new byte[16];

            using (var generador = RandomNumberGenerator.Create())
            {
                generador.GetBytes(sal);
            }

            using (var derivador = new Rfc2898DeriveBytes(
                contrasena, sal, 100000, HashAlgorithmName.SHA256))
            {
                byte[] hash = derivador.GetBytes(32);

                return "PBKDF2-SHA256$100000$"
                    + Convert.ToBase64String(sal) + "$"
                    + Convert.ToBase64String(hash);
            }
        }
    }
}