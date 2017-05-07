using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace CommonUtils
{
    public class AliPay
    {
        /// <summary>
        /// 与ASP兼容的MD5加密算法
        /// </summary>
        public static String GetMD5(String s, String InputCharset)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] t = md5.ComputeHash(Encoding.GetEncoding(InputCharset).GetBytes(s));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 冒泡排序法
        /// 按照字母序列从a到z的顺序排列
        /// </summary>
        public static String[] BubbleSort(String[] r)
        {
            int i, j; //交换标志 
            String temp;

            Boolean exchange;

            for (i = 0; i < r.Length; i++) //最多做R.Length-1趟排序 
            {
                exchange = false; //本趟排序开始前，交换标志应为假

                for (j = r.Length - 2; j >= i; j--)
                {//交换条件
                    if (System.String.CompareOrdinal(r[j + 1], r[j]) < 0)
                    {
                        temp = r[j + 1];
                        r[j + 1] = r[j];
                        r[j] = temp;

                        exchange = true; //发生了交换，故将交换标志置为真 
                    }
                }

                if (!exchange) //本趟排序未发生交换，提前终止算法 
                {
                    break;
                }
            }
            return r;
        }
        /// <summary>
        /// 生成URL链接或加密结果
        /// </summary>
        /// <param name="para">参数加密数组</param>
        /// <param name="_input_charset">编码格式</param>
        /// <param name="sign_type">加密类型</param>
        /// <param name="key">安全校验码</param>
        /// <returns>字符串URL或加密结果</returns>
        public static String CreatUrl(
            //String gateway,//GET方式传递参数时请去掉注释
            String[] para, String _input_charset, String sign_type, String key)
        {
            int i;
            //进行排序；
            String[] Sortedstr = BubbleSort(para);
            //构造待md5摘要字符串 ；
            StringBuilder prestr = new StringBuilder();
            for (i = 0; i < Sortedstr.Length; i++)
            {
                if (i == Sortedstr.Length - 1)
                {
                    prestr.Append(Sortedstr[i]);
                }
                else
                {
                    prestr.Append(Sortedstr[i] + "&");
                }
            }
            prestr.Append(key);
            //生成Md5摘要；
            String sign = GetMD5(prestr.ToString(), _input_charset);
            //以下是POST方式传递参数
            return sign;
            //以下是GET方式传递参数
            //构造支付Url；
            //            Char[] delimiterChars = { '='};
            //            StringBuilder parameter = new StringBuilder();
            //            parameter.Append(gateway);
            //            for (i = 0; i < Sortedstr.Length; i++)
            //            {//UTF-8格式的编码转换
            //                parameter.Append(Sortedstr[i].Split(delimiterChars)[0] + "=" + HttpUtility.UrlEncode(Sortedstr[i].Split(delimiterChars)[1]) + "&");
            //            }
            //
            //            parameter.Append("sign=" + sign + "&sign_type=" + sign_type);
            //
            //            //返回支付Url；
            //            return parameter.ToString();
        }
        //获取远程服务器ATN结果，验证是否是支付宝服务器发来的请求
        public static String Get_Http(String a_strUrl, int timeout)
        {
            String strResult;
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(a_strUrl);
                myReq.Timeout = timeout;
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, Encoding.Default);
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }
                strResult = strBuilder.ToString();
            }
            catch (Exception exp)
            {

                strResult = "错误：" + exp.Message;
            }
            return strResult;
        }

    }
}
