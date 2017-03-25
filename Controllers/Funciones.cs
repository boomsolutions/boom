using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Globalization;

namespace MvcApplication4.Controllers
{
    public static class Funciones
    {
        public static string Encrypt(string clearText)
        {

            if (clearText == null)
            {
                clearText = "";
            }
            else
            {
                string EncryptionKey = "MLApro69";
                byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            return clearText;

        }

        public static string Decrypt(string cipherText)
        {
            if (cipherText == null)
            {
                cipherText = "";
            }
            else
            {
                string EncryptionKey = "MLApro69";
                //cipherText = cipherText.Replace("@", "%");
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = null;
                cipherBytes = Convert.FromBase64String(cipherText);
                
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = System.Text.Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                
            }
            return cipherText;
        }

        public static string GetFraction(decimal num)
        {

            //ver que separador de comas usar
            string sep = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            return num.ToString().Substring(num.ToString().IndexOf(sep) + 1, 2);
        }

        
    }
}