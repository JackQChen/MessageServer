using System;
using System.Security.Cryptography;
using System.Text;

namespace AccessService
{
    public class Encrypt
    {

        public static string AESEncrypt(string encryptStr, string key)
        {
            byte[] keyArray = new byte[16];
            byte[] array = Encoding.UTF8.GetBytes(key);
            Buffer.BlockCopy(array, 0, keyArray, 0, array.Length);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(encryptStr);
            var rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string AESDecrypt(string decryptStr, string key)
        {
            byte[] keyArray = new byte[16];
            byte[] array = Encoding.UTF8.GetBytes(key);
            Buffer.BlockCopy(array, 0, keyArray, 0, array.Length);
            byte[] toEncryptArray = Convert.FromBase64String(decryptStr);
            var rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }

    }
}
