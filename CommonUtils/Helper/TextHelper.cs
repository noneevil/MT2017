using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CommonUtils
{
    /// <summary>
    /// 用于字符及文字转换的类
    /// </summary>
    public abstract class TextHelper
    {
        /// <summary>
        /// URL解码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String UrlDecode(String text)
        {
            return HttpUtility.UrlDecode(text);
        }
        /// <summary>
        /// URL解码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encoding">编码格式</param>
        /// <returns></returns>
        public static String UrlDecode(String text, String encoding)
        {
            return HttpUtility.UrlDecode(text, Encoding.GetEncoding(encoding));
        }
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String UrlEncode(String text)
        {
            return HttpUtility.UrlEncode(text);
        }
        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encoding">编码格式</param>
        /// <returns></returns>
        public static String UrlEncode(String text, String encoding)
        {
            return HttpUtility.UrlEncode(text, Encoding.GetEncoding(encoding));
        }
        /// <summary>
        /// HTML解码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String HtmlDecode(String text)
        {
            return HttpUtility.HtmlDecode(text);
        }
        /// <summary>
        /// HTML编码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String HtmlEncode(String text)
        {
            return HttpUtility.HtmlEncode(text);
        }
        /// <summary>
        /// 取得自定义长度的字符串
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public String CutChar(String text, int length)
        {
            if (text.Length > length)
            {
                return text.Substring(0, length);
            }
            else
            {
                return text;
            }
        }
        /// <summary>
        /// 取得自定义长度的字符串  中文123456789           
        /// </summary>
        /// <param name="text"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        public static String GetLimitChar(String text, int lenght)
        {
            String result = String.Empty;
            if (text.Length > lenght)
            {
                //int tempnum = 0;
                int tempnum1 = 0;
                int tempnum2 = 0;
                Byte[] bytes = ASCIIEncoding.ASCII.GetBytes(text);
                for (int i = 0; i < text.Length; i++)
                {
                    if ((int)bytes[i] != 63) tempnum1++; else tempnum2++;
                    if (tempnum2 * 2 + tempnum1 >= lenght * 2) break;

                }
                result = text.Substring(0, tempnum2 + tempnum1);
            }
            else
            {
                result = text;
            }
            return result;
        }
        /// <summary>
        /// 生成随机密码
        /// </summary>
        /// <param name="length"></param>
        /// <param name="Seed"></param>
        /// <returns></returns>
        public static String RandomText(int length)
        {
            //因1与l不容易分清楚，所以去除 
            String text = "2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,j,k,m,n,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,W,X,Y,Z,@,#,$,%,&,+,=,?";
            String[] symbol = text.Split(',');
            String result = String.Empty;
            Random random = new Random();

            //生成随机字符串 
            for (int i = 0; i < length; i++)
            {
                result += symbol[random.Next(symbol.Length)];
            }

            return result;
        }
        /// <summary>
        /// 产生随机二进制数据流
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Byte[] RandomByte(long length)
        {
            Byte[] buffer = new Byte[length];
            using (RNGCryptoServiceProvider pr = new RNGCryptoServiceProvider())
            {
                pr.GetNonZeroBytes(buffer);
            }
            return buffer;
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        public static String[] SplitString(String text, String separator)
        {
            if (text.IndexOf(separator) < 0)
            {
                String[] tmp = { text };
                return tmp;
            }
            return Regex.Split(text, separator.Replace(".", @"\."), RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }
        /// <summary>
        /// 高效字符串拼接实现(比直接使用String.Format()方法高效)。
        /// </summary>
        /// <param name="format">复合格式字符串。</param>
        /// <param name="args"> 一个对象数组，其中包含零个或多个要设置格式的对象。</param>
        /// <returns></returns>
        public static String Format(String format, params Object[] args)
        {
            if (format == null || args == null) throw new ArgumentNullException((format == null) ? "format" : "args");
            int capacity = format.Length + args.Where(a => a != null).Select(a => a.ToString()).Sum(p => p.Length);
            StringBuilder sb = new StringBuilder(capacity);
            sb.AppendFormat(format, args);
            return sb.ToString();
        }
        /// <summary>
        /// 返回字符串真实长度 1个汉字长度为2
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int GetStringLength(String text)
        {
            return Encoding.Default.GetBytes(text).Length;
        }
        /// <summary>
        /// 替换sql语句中的有问题符号
        /// </summary>
        public static String CheckSQL(String text)
        {
            String result = String.Empty;
            if (!String.IsNullOrEmpty(text))
            {
                result = text.Replace("'", "''");
            }
            return result;
        }
        /// <summary>
        /// 返回清除JS的HTML代码
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static String ClearJavaScript(String html)
        {
            if (String.IsNullOrEmpty(html)) return String.Empty;
            String _result = RegexHelper.RemoveScripts(html);
            return _result;
        }
        /// <summary>
        /// 返回清除HTML格式的文字内容
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static String ClearHTML(String html)
        {
            //删除脚本    
            String result = Regex.Replace(html, @"<script[^>]*>([\s\S](?!<script))*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML    
            result = Regex.Replace(result, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"-->", "", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"<!--.*", "", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            result.Replace("<", "");
            result.Replace(">", "");
            result.Replace("\r\n", "");
            return result;
        }
        /// <summary>
        /// 返回字符串子串(...形式)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static String GetSubString(Object text, int length)
        {
            String result = Convert.ToString(text);
            if (String.IsNullOrEmpty(result)) return String.Empty;
            if (result.Length > length && length > 0)
            {
                return result.Substring(0, length) + "…";
            }
            else
            {
                return result;
            }
        }
        /// <summary>
        /// 返回字符串子串
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static String GetSubStringEx(Object text, int length)
        {
            String result = Convert.ToString(text);
            if (String.IsNullOrEmpty(result)) return String.Empty;
            if (result.Length > length && length > 0)
            {
                return result.Substring(0, length);
            }
            else
            {
                return result;
            }
        }
        /// <summary>
        /// 把对应的文本转换为HTML
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String TextToHtml(String text)
        {
            String result = text.Replace(" ", "&nbsp;");
            result = result.Replace("<", "&lt;");
            result = result.Replace(">", "&gt;");
            result = text.Replace("\r\n", "<br />");
            result = result.Replace("\n", "<br /");
            return result;
        }
        /// <summary> 
        /// 在指定的字符串列表text中检索符合拼音索引字符串 
        /// </summary> 
        /// <param name="text">汉字字符串</param> 
        /// <returns>相对应的汉语拼音首字母串</returns> 
        public static String GetSpellCode(String text)
        {
            String result = String.Empty;
            for (int i = 0; i < text.Length; i++)
            {
                result += GetCharSpellCode(text.Substring(i, 1));
            }
            return result;
        }
        /// <summary> 
        /// 得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回大写字母 
        /// </summary> 
        /// <param name="text">单个汉字</param> 
        /// <returns>单个大写字母</returns> 
        public static String GetCharSpellCode(String text)
        {
            long cnchar;
            Byte[] ZW = Encoding.Default.GetBytes(text);

            //如果是字母，则直接返回 
            if (ZW.Length == 1)
            {
                return text.ToUpper();
            }
            else
            {
                // get the array of Byte from the single Char 
                int i1 = (short)(ZW[0]);
                int i2 = (short)(ZW[1]);
                cnchar = i1 * 256 + i2;
            }
            // iCnChar match the constant 
            if ((cnchar >= 45217) && (cnchar <= 45252)) return "A";
            else if ((cnchar >= 45253) && (cnchar <= 45760)) return "B";
            else if ((cnchar >= 45761) && (cnchar <= 46317)) return "C";
            else if ((cnchar >= 46318) && (cnchar <= 46825)) return "D";
            else if ((cnchar >= 46826) && (cnchar <= 47009)) return "E";
            else if ((cnchar >= 47010) && (cnchar <= 47296)) return "F";
            else if ((cnchar >= 47297) && (cnchar <= 47613)) return "G";
            else if ((cnchar >= 47614) && (cnchar <= 48118)) return "H";
            else if ((cnchar >= 48119) && (cnchar <= 49061)) return "J";
            else if ((cnchar >= 49062) && (cnchar <= 49323)) return "K";
            else if ((cnchar >= 49324) && (cnchar <= 49895)) return "L";
            else if ((cnchar >= 49896) && (cnchar <= 50370)) return "M";
            else if ((cnchar >= 50371) && (cnchar <= 50613)) return "N";
            else if ((cnchar >= 50614) && (cnchar <= 50621)) return "O";
            else if ((cnchar >= 50622) && (cnchar <= 50905)) return "P";
            else if ((cnchar >= 50906) && (cnchar <= 51386)) return "Q";
            else if ((cnchar >= 51387) && (cnchar <= 51445)) return "R";
            else if ((cnchar >= 51446) && (cnchar <= 52217)) return "S";
            else if ((cnchar >= 52218) && (cnchar <= 52697)) return "T";
            else if ((cnchar >= 52698) && (cnchar <= 52979)) return "W";
            else if ((cnchar >= 52980) && (cnchar <= 53640)) return "X";
            else if ((cnchar >= 53689) && (cnchar <= 54480)) return "Y";
            else if ((cnchar >= 54481) && (cnchar <= 55289)) return "Z";
            else return ("?");
        }

        ///// <summary>
        ///// 取得当前URL
        ///// </summary>
        ///// <returns></returns>
        //public static String GetPath()
        //{
        //    String strPath = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] +
        //        HttpContext.Current.Request.ServerVariables["PATH_INFO"] + "?" +
        //        HttpContext.Current.Request.ServerVariables["QUERY_STRING"];
        //    if (strPath.IndexOf("?") > 0)
        //    {
        //        strPath = strPath.Substring(0, strPath.IndexOf("?"));
        //    }
        //    return strPath;
        //}
        ///// <summary>
        ///// 取得当前服务器的网址
        ///// </summary>
        ///// <returns></returns>
        //public static String GetWebSiteUrl()
        //{
        //    return "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
        //}
        ///// <summary>
        ///// 登录验证码
        ///// </summary>
        //public static String ValidateCode
        //{
        //    get
        //    {
        //        return Convert.ToString(HttpContext.Current.Session["WEB_CheckCode"]);
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["WEB_CheckCode"] = value.ToLower();
        //    }
        //}
    }
}
