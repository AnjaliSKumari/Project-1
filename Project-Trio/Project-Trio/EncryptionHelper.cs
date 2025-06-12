using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Project_Trio
{

    public static class EncryptionHelper
    {
        private static readonly string EncryptionKey = "12345678901234567890123456789012"; // Must be 32 chars

        public static string Encrypt(string plainText)
        {
            byte[] clearBytes = Encoding.UTF8.GetBytes(plainText);
            using (Aes encryptor = Aes.Create())
            {
                encryptor.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                encryptor.IV = new byte[16]; // Use a fixed or random IV in real scenarios

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
    }
}
