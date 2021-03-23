using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;
using NLog;

namespace SupplierPlatform.Helper
{
    public class AesCrypto
    {
        /// <summary>
        /// 小額電商使用AES
        /// </summary>
        AesCryptoServiceProvider aes { get; set; }
        public AesCrypto(string Key, string IV)
        {
            aes = new AesCryptoServiceProvider()
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                //BlockSize = 128,
                //KeySize = 256,
                //FeedbackSize = 128,
                Key = new SHA256Managed().ComputeHash(Encoding.ASCII.GetBytes(Key)),
                IV = Encoding.ASCII.GetBytes(IV)
            };
        }

        /// <summary>
        /// 字串加密
        /// </summary>
        /// <param name="Source">加密前字串</param>
        /// <returns>加密後字串</returns>
        public string Encryptor(string Source)
        {
            byte[] dataByteArray = Encoding.UTF8.GetBytes(Source);
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(dataByteArray, 0, dataByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        /// <summary>
        /// 字串解密
        /// </summary>
        /// <param name="Source">解密前字串</param>
        /// <returns>解密後字串</returns>
        public string Decryptor(string Source)
        {
            byte[] dataByteArray = Convert.FromBase64String(Source);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
    }

    public static class AES
    {
        private static NLog.Logger logger = LogManager.GetLogger("AESHelper");

        /// <summary>
        /// EncryptAES256.
        /// </summary>
        /// <param name="encryptData">encryptData</param>
        /// <returns>string</returns>
        public static string EncryptAES256(string encryptData)
        {
            try
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(encryptData);

                string key = ConfigurationManager.AppSettings["DATA_AESKEY"];

                var aes = new RijndaelManaged();
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.Zeros;
                ICryptoTransform transform = aes.CreateEncryptor();

                return Convert.ToBase64String(transform.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length));
            }
            catch (Exception ex)
            {
                logger.Error($"requestMethod=EncryptAES256 Err={ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// decryptAES256.
        /// </summary>
        /// <param name="decryptData">decryptData</param>
        /// <returns>string</returns>
        public static string DecryptAES256(string decryptData)
        {
            try
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.Key = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["DATA_AESKEY"]);
                    byte[] cipherText = Convert.FromBase64String(decryptData);

                    aes.Mode = CipherMode.ECB;
                    aes.Padding = PaddingMode.Zeros;

                    // Create a decryptor to perform the stream transform.
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    string result = "";
                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                                return result.TrimEnd('\0');
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"requestMethod=DecryptAES256 Err={ex.Message}");
                return string.Empty;
            }
        }
    }
}