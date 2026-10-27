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
        public static bool Verificar(string contrasena, string valorGuardado)
        {
            if (string.IsNullOrEmpty(contrasena) ||
                string.IsNullOrEmpty(valorGuardado))
                return false;

            string[] partes = valorGuardado.Split('$');

            if (partes.Length != 4 ||
                partes[0] != "PBKDF2-SHA256" ||
                partes[1] != "100000")
                return false;

            byte[] sal;
            byte[] hashGuardado;

            try
            {
                sal = Convert.FromBase64String(partes[2]);
                hashGuardado = Convert.FromBase64String(partes[3]);
            }
            catch (FormatException)
            {
                return false;
            }

            if (sal.Length != 16 || hashGuardado.Length != 32)
                return false;

            using (var derivador = new Rfc2898DeriveBytes(
                contrasena, sal, 100000, HashAlgorithmName.SHA256))
            {
                byte[] hashCalculado = derivador.GetBytes(32);
                int diferencia = 0;

                for (int i = 0; i < hashGuardado.Length; i++)
                    diferencia |= hashGuardado[i] ^ hashCalculado[i];

                return diferencia == 0;
            }
        }
    }
}