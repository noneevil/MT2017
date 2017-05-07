using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Web.Security;

namespace CommonUtils
{
    /// <summary>
    /// 对字符串进行MD5或ASH1方式加密(用于加密密码)
    /// 对字符串进行加密和解密
    /// </summary>
    public abstract class EncryptHelper
    {
        #region Base64加密解密

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String Base64Encrypt(String text)
        {
            Byte[] bytes = Encoding.UTF8.GetBytes(text);
            String result = Convert.ToBase64String(bytes);
            return result;
        }
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String Base64Decrypt(String text)
        {
            Byte[] bytes = Convert.FromBase64String(text);
            String result = Encoding.UTF8.GetString(bytes);
            return result;
        }

        #endregion

        /// <summary>
        /// MD5加密32位大写
        /// </summary>
        /// <returns></returns>
        public static string MD5Upper32(String text)
        {
            String result = FormsAuthentication.HashPasswordForStoringInConfigFile(text, FormsAuthPasswordFormat.MD5.ToString());
            return result.ToUpper();
        }
        /// <summary>
        /// MD5加密32位小写
        /// </summary>
        /// <returns></returns>
        public static string MD5Lower32(String text)
        {
            String result = FormsAuthentication.HashPasswordForStoringInConfigFile(text, FormsAuthPasswordFormat.MD5.ToString());
            return result.ToLower();
        }
        /// <summary>
        /// MD5加密16位大写
        /// </summary>
        /// <returns></returns>
        public static string MD5Upper16(String text)
        {
            String result = FormsAuthentication.HashPasswordForStoringInConfigFile(text, FormsAuthPasswordFormat.MD5.ToString());
            return result.ToUpper().Substring(8, 16);
        }
        /// <summary>
        /// MD5加密16位小写
        /// </summary>
        /// <returns></returns>
        public static string MD5Lower16(String text)
        {
            String result = FormsAuthentication.HashPasswordForStoringInConfigFile(text, FormsAuthPasswordFormat.MD5.ToString());
            return result.ToLower().Substring(8, 16);
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String SHA1(String text)
        {
            String result = FormsAuthentication.HashPasswordForStoringInConfigFile(text, FormsAuthPasswordFormat.SHA1.ToString());
            return result;
        }

        #region 获取由SHA1加密的字符串

        /// <summary>
        /// 获取由SHA1加密的字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String EncryptToSHA1(String text)
        {
            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            Byte[] bytes = Encoding.UTF8.GetBytes(text);
            Byte[] result = sha.ComputeHash(bytes);
            sha.Clear();
            sha.Dispose();
            return Convert.ToBase64String(result);
        }

        #endregion

        #region 获取由MD5加密的字符串

        /// <summary>
        ///  获取由MD5加密的字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String EncryptToMD5(String text)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            Byte[] bytes = Encoding.UTF8.GetBytes(text);
            Byte[] result = md5.ComputeHash(bytes, 0, bytes.Length);
            md5.Clear();
            md5.Dispose();
            return Convert.ToBase64String(result);
        }

        #endregion

        /// <summary>
        /// 字符串加密算法
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String EncryptString(String text)
        {
            Char[] Base64Code = new Char[] { 'a', 'b', 'c', 'd', 'e'
                , 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o'
                , 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y'
                , 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I'
                , 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S'
                , 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2'
                , '3', '4', '5', '6', '7', '8', '9', '+', '/', '=' };
            Byte empty = (Byte)0;
            ArrayList byteMessage = new ArrayList(Encoding.Default.GetBytes(text));
            StringBuilder sb;
            int messageLen = byteMessage.Count;
            int page = messageLen / 3;
            int use = 0;
            if ((use = messageLen % 3) > 0)
            {
                for (int i = 0; i < 3 - use; i++)
                    byteMessage.Add(empty);
                page++;
            }
            sb = new StringBuilder(page * 4);
            for (int i = 0; i < page; i++)
            {
                Byte[] instr = new Byte[3];
                instr[0] = (Byte)byteMessage[i * 3];
                instr[1] = (Byte)byteMessage[i * 3 + 1];
                instr[2] = (Byte)byteMessage[i * 3 + 2];
                int[] outstr = new int[4];
                outstr[0] = instr[0] >> 2;

                outstr[1] = ((instr[0] & 0x03) << 4) ^ (instr[1] >> 4);
                if (!instr[1].Equals(empty))
                    outstr[2] = ((instr[1] & 0x0f) << 2) ^ (instr[2] >> 6);
                else
                    outstr[2] = 64;
                if (!instr[2].Equals(empty))
                    outstr[3] = (instr[2] & 0x3f);
                else
                    outstr[3] = 64;
                sb.Append(Base64Code[outstr[0]]);
                sb.Append(Base64Code[outstr[1]]);
                sb.Append(Base64Code[outstr[2]]);
                sb.Append(Base64Code[outstr[3]]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 字符串解密算法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String DecryptString(String text)
        {
            if ((text.Length % 4) != 0)
            {
                throw new ArgumentException("不是正确的BASE64编码，请检查。", "str");
            }
            if (!Regex.IsMatch(text, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
            {
                throw new ArgumentException("包含不正确的BASE64编码，请检查。", "str");
            }
            String Base64Code = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+/=";
            int page = text.Length / 4;
            ArrayList outMessage = new ArrayList(page * 3);
            Char[] message = text.ToCharArray();
            for (int i = 0; i < page; i++)
            {
                Byte[] instr = new Byte[4];
                instr[0] = (Byte)Base64Code.IndexOf(message[i * 4]);
                instr[1] = (Byte)Base64Code.IndexOf(message[i * 4 + 1]);
                instr[2] = (Byte)Base64Code.IndexOf(message[i * 4 + 2]);
                instr[3] = (Byte)Base64Code.IndexOf(message[i * 4 + 3]);
                Byte[] outstr = new Byte[3];
                outstr[0] = (Byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));
                if (instr[2] != 64)
                {
                    outstr[1] = (Byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
                }
                else
                {
                    outstr[2] = 0;
                }
                if (instr[3] != 64)
                {
                    outstr[2] = (Byte)((instr[2] << 6) ^ instr[3]);
                }
                else
                {
                    outstr[2] = 0;
                }
                outMessage.Add(outstr[0]);
                if (outstr[1] != 0)
                    outMessage.Add(outstr[1]);
                if (outstr[2] != 0)
                    outMessage.Add(outstr[2]);
            }
            Byte[] outbyte = (Byte[])outMessage.ToArray(Type.GetType("System.Byte"));
            return Encoding.Default.GetString(outbyte);
        }

        #region DES加密

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="sInputString">输入字符</param>
        /// <param name="sKey">Key(8位长度字符串)</param>
        /// <param name="iv">偏移向量(8位长度字符串)</param>
        /// <returns>加密结果</returns>
        public static String DesEncrypt(String str, String key, String iv)
        {
            Byte[] data = Encoding.UTF8.GetBytes(str);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = Encoding.UTF8.GetBytes(key);
            DES.IV = Encoding.UTF8.GetBytes(iv);
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            Byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            return Convert.ToBase64String(result);
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="sInputString">输入字符</param>
        /// <param name="sKey">Key(8位长度字符串)</param>
        /// <param name="iv">偏移向量(8位长度字符串)</param>
        /// <returns>解密结果</returns>
        public static String DesDecrypt(String str, String key, String iv)
        {
            Byte[] data = Convert.FromBase64String(str);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = Encoding.UTF8.GetBytes(key);
            DES.IV = Encoding.UTF8.GetBytes(iv);
            ICryptoTransform desencrypt = DES.CreateDecryptor();
            Byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            return Encoding.UTF8.GetString(result);
        }

        #endregion

        #region 3Des加密

        /// <summary>
        /// 3Des加密
        /// </summary>
        /// <param name="str">要进行加密的字符串(内部对字符串采用utf8编码)</param>
        /// <param name="key">加密key(24位字符串)</param>
        /// <param name="iv">偏移向量(8位字符串)</param>
        /// <returns>base64编码的字符串</returns>
        public static String TripleDesEncrypt(String str, String key, String iv)
        {
            iv = MD5Upper32(iv).Substring(0, 8);
            key = MD5Upper32(key).Substring(0, 24);
            TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
            //设置偏移向量
            tdsp.IV = Encoding.UTF8.GetBytes(iv);
            //设置加密密匙
            tdsp.Key = Encoding.UTF8.GetBytes(key);
            //设置加密算法运算模式为ECB(保持和java兼容)
            tdsp.Mode = CipherMode.CBC;
            //设置加密算法的填充模式为PKCS7(保持和java兼容)
            tdsp.Padding = PaddingMode.PKCS7;
            //对输入字符串采用utf8编码获取字节
            Byte[] data = Encoding.UTF8.GetBytes(str);
            //加密后采用base64编码生成加密串
            ICryptoTransform ct = tdsp.CreateEncryptor();
            String result = Convert.ToBase64String(ct.TransformFinalBlock(data, 0, data.Length));
            return result;
        }

        /// <summary>
        /// 3Des解密
        /// </summary>
        /// <param name="str">要进行解密base64字符串</param>
        /// <param name="key">解密key(24位字符串)</param>
        /// <param name="iv">偏移向量(8位字符串)</param>
        /// <returns>原始字符串(内部对字符串采用utf8进行解码)</returns>
        public static String TripleDesDecrypt(String str, String key, String iv)
        {
            iv = MD5Upper32(iv).Substring(0, 8);
            key = MD5Upper32(key).Substring(0, 24);
            TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
            tdsp.IV = Encoding.UTF8.GetBytes(iv);
            //设置偏移向量
            tdsp.IV = Encoding.UTF8.GetBytes(iv);
            //设置加密密匙
            tdsp.Key = System.Text.Encoding.UTF8.GetBytes(key);
            //设置加密算法运算模式为ECB(保持和java兼容)
            tdsp.Mode = CipherMode.CBC;
            //设置加密算法的填充模式为PKCS7(保持和java兼容)
            tdsp.Padding = PaddingMode.PKCS7;

            //加密串用base64编码,需要采用base64方式解析为字节
            Byte[] data = Convert.FromBase64String(str);
            ICryptoTransform ct = tdsp.CreateDecryptor();
            //用utf8编码还原原始字符串
            String result = Encoding.UTF8.GetString(ct.TransformFinalBlock(data, 0, data.Length));
            return result;
        }

        #endregion
    }
}