
using System.Security.Cryptography;

namespace PlantCatalogue.Controllers
{
    public class Services
    {
        //Jelszavak kezeléséért felelős metódus
        public string Encription(string password)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = new byte[] { 24, 85, 236, 168, 68, 66, 17, 60, 1, 67, 106, 129, 239, 131, 175, 181, 161, 191, 160, 95, 253, 231, 237, 137, 131, 176, 189, 172, 166, 210, 43, 82 };
                aesAlg.IV = new byte[] { 251, 55, 226, 56, 41, 220, 232, 9, 51, 114, 106, 151, 185, 119, 186, 209 };
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(password);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }
        //Jelszó egyezést ellenőrizzük vele
        public string Decription(string password)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = new byte[] { 24, 85, 236, 168, 68, 66, 17, 60, 1, 67, 106, 129, 239, 131, 175, 181, 161, 191, 160, 95, 253, 231, 237, 137, 131, 176, 189, 172, 166, 210, 43, 82 };
                aesAlg.IV = new byte[] { 251, 55, 226, 56, 41, 220, 232, 9, 51, 114, 106, 151, 185, 119, 186, 209 };
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(password)))
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
        //Az E-mail linkekben szereplő oldalak fájlkezelése
        public string FromFile( string path)
        {
            using (StreamReader reader = new StreamReader( path))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
