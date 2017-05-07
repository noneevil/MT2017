using System;
using System.Security.Cryptography;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 得到随机安全码（哈希加密）。
    /// </summary>
    public class HashEncode
    {
        /// <summary>
        /// 得到随机哈希加密字符串
        /// </summary>
        /// <returns></returns>
        public static String GetSecurity()
        {
            String Security = HashEncoding(GetRandomValue());
            return Security;
        }
        /// <summary>
        /// 得到一个随机数值
        /// </summary>
        /// <returns></returns>
        public static String GetRandomValue()
        {
            Random Seed = new Random();
            String RandomVaule = Seed.Next(1, int.MaxValue).ToString();
            return RandomVaule;
        }
        /// <summary>
        /// 哈希加密一个字符串
        /// </summary>
        /// <param name="Security"></param>
        /// <returns></returns>
        public static String HashEncoding(String Security)
        {
            Byte[] Value;
            UnicodeEncoding Code = new UnicodeEncoding();
            Byte[] Message = Code.GetBytes(Security);
            SHA512Managed Arithmetic = new SHA512Managed();
            Value = Arithmetic.ComputeHash(Message);
            Security = "";
            foreach (Byte o in Value)
            {
                Security += (int)o + "O";
            }
            return Security;
        }
    }
}
