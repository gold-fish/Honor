using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Web.Utils
{
    public class SecurityHelper
    {
        #region MD5加密

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="pwd">加密前的密码</param>
        /// <returns>返回加密后的密码</returns>
        public static string MD5Encrypt(string pwd)
        {
            StringBuilder sbPwd = new StringBuilder();

            MD5 md5 = new MD5CryptoServiceProvider();

            //用md5算法对密码进行加密，然后存放在字节数组中
            byte[] byteArr = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(pwd));
            int len = byteArr.Length;

            //将加密后的字节数组转化为32位的字符串
            for (int i = 0; i < len; i++)
            {
                sbPwd.Append(byteArr[i].ToString("X").PadLeft(2, '0'));
            }

            return sbPwd.ToString();
        }

        #endregion

        #region DES加密

        public static string Encrypt(string pToEncrypt)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //把字符串放到byte数组中  
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);

            //建立加密对象的密钥和偏移量  
            //原文使用ASCIIEncoding.ASCII方法的GetBytes方法  
            //使得输入密码必须输入英文文本  
            des.Key = ASCIIEncoding.ASCII.GetBytes("goldfish");
            des.IV = ASCIIEncoding.ASCII.GetBytes("goldfish");
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }

            return ret.ToString();
        }

        #endregion

        #region DES解密

        public static string Decrypt(string pToDecrypt)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];

            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            //建立加密对象的密钥和偏移量，此值重要，不能修改  
            des.Key = ASCIIEncoding.ASCII.GetBytes("goldfish");
            des.IV = ASCIIEncoding.ASCII.GetBytes("goldfish");
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

        #endregion
    }
}