using System;
using System.Text;
using System.Web;

namespace CommonUtils.Tenpay
{
    /// <summary>
    /// TenpayUtil 的摘要说明。
    /// </summary>
    public class TenpayUtil
    {
        public TenpayUtil()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /** 对字符串进行URL编码 */
        public static String UrlEncode(String instr, String charset)
        {
            //return instr;
            if (instr == null || instr.Trim() == "")
                return "";
            else
            {
                String res;

                try
                {
                    res = HttpUtility.UrlEncode(instr, Encoding.GetEncoding(charset));

                }
                catch (Exception)
                {
                    res = HttpUtility.UrlEncode(instr, Encoding.GetEncoding("GB2312"));
                }


                return res;
            }
        }


        /** 对字符串进行URL解码 */
        public static String UrlDecode(String instr, String charset)
        {
            if (instr == null || instr.Trim() == "")
                return "";
            else
            {
                String res;

                try
                {
                    res = HttpUtility.UrlDecode(instr, Encoding.GetEncoding(charset));

                }
                catch (Exception)
                {
                    res = HttpUtility.UrlDecode(instr, Encoding.GetEncoding("GB2312"));
                }


                return res;

            }
        }

        /** 取时间戳生成随即数,替换交易单号中的后10位流水号 */
        public static UInt32 UnixStamp()
        {
            TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToUInt32(ts.TotalSeconds);
        }


        /** 取随机数 */
        public static String BuildRandomStr(int length)
        {
            Random rand = new Random();

            int num = rand.Next();

            String str = num.ToString();

            if (str.Length > length)
            {
                str = str.Substring(0, length);
            }
            else if (str.Length < length)
            {
                int n = length - str.Length;
                while (n > 0)
                {
                    str.Insert(0, "0");
                    n--;
                }
            }

            return str;
        }
    }
}
