using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ApiOAuthEmpleados.Helpers
{
    public class HelperCifrado
    {
        private static string _secretKey;

        public HelperCifrado(IConfiguration configuration)
        {
            // Opcional: depender de la sección de tu appsettings. 
            // Usa la misma donde tienes la clave del token, por ejemplo "ApiOAuthToken:SecretKey" o "ClaveToken:SecretKey"
            _secretKey = configuration.GetValue<string>("ApiOAuthToken:SecretKey");
        }

        // Método para encriptar el texto (en tu caso, el JSON del UserData)
        public static string Encrypt(string texto)
        {
            if (string.IsNullOrEmpty(_secretKey))
                throw new InvalidOperationException("La clave secreta no se ha inicializado en HelperCifrado.");

            byte[] keyBytes = SHA256.HashData(Encoding.UTF8.GetBytes(_secretKey));
            
            // Creamos un vector de inicialización de 16 bytes (por simplicidad, a 0)
            byte[] iv = new byte[16];

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(texto);
                        }
                        
                        // Convertimos los bytes cifrados en una cadena Base64
                        byte[] encryptedBytes = msEncrypt.ToArray();
                        return Convert.ToBase64String(encryptedBytes);
                    }
                }
            }
        }

        // Método para desencriptar el texto
        public static string Decrypt(string textoCifrado)
        {
            if (string.IsNullOrEmpty(_secretKey))
                throw new InvalidOperationException("La clave secreta no se ha inicializado en HelperCifrado.");

            byte[] keyBytes = SHA256.HashData(Encoding.UTF8.GetBytes(_secretKey));
            byte[] iv = new byte[16];
            byte[] cipherBytes = Convert.FromBase64String(textoCifrado);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
