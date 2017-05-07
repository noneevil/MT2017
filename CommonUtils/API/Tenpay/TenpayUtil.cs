using System;
using System.Text;
using System.Web;

namespace CommonUtils.Tenpay
{
    /// <summary>
    /// TenpayUtil ��ժҪ˵����
    /// </summary>
    public class TenpayUtil
    {
        public TenpayUtil()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }
        /** ���ַ�������URL���� */
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


        /** ���ַ�������URL���� */
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

        /** ȡʱ��������漴��,�滻���׵����еĺ�10λ��ˮ�� */
        public static UInt32 UnixStamp()
        {
            TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToUInt32(ts.TotalSeconds);
        }


        /** ȡ����� */
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
