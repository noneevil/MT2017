using System;
using System.Web;
using System.Web.Caching;

namespace CommonUtils
{
    /// <summary>
    /// ����Ԥ����Ϣ��ȡ������
    /// </summary>
    public class WeatherHelper
    {
        /// <summary>
        /// ȡ��ָ�����е�������Ϣ
        /// </summary>
        /// <param name="code">���д���</param>
        /// <returns></returns>
        public static String GetWeather(String code)
        {
            WeaherObject w = GetWeatherInfor(code);
            if (w != null)
            {
                return String.Format("<a href='http://weather.news.qq.com/preend.htm?dc" + code
                    + ".htm' target='_blank'><span style='color:#333;'>{1} {0} {2}</span></a>", w.Temperature, w.City, w.WeaherInfor);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// ȡ�ù�����������Ϣ
        /// </summary>
        /// <returns></returns>
        public static String GetWeather()
        {
            return GetWeather("227");
        }

        /// <summary>
        /// ȡ�ù���������Ϣ
        /// </summary>
        /// <param name="code">���д���</param>
        /// <returns></returns>
        public static WeaherObject GetWeatherInfor(String code)
        {
            WeaherObject w = null;
            try
            {
                w = (WeaherObject)HttpContext.Current.Cache["_WeaherObject"];
            }
            catch { }
            if (w != null) return w;
            String url = "http://weather.news.qq.com/inc/07_ss" + code + ".htm";
            System.Net.WebClient client = new System.Net.WebClient();
            try
            {
                String key = client.DownloadString(url);
                w = new WeaherObject();

                //��ȡ����
                int i = key.IndexOf("class=\"wht1 lk37\">");
                key = key.Substring(i, key.Length - i);
                int j = key.IndexOf("</td>");
                w.City = key.Substring(0, j).Replace("class=\"wht1 lk37\">", "");

                //��ȡͼƬ
                i = key.IndexOf("<img");
                key = key.Substring(i, key.Length - i);
                j = key.IndexOf("</td>");
                w.WeaherImage = key.Substring(0, j);

                //��ȡ������Ϣ
                i = key.IndexOf("<div class=\"txbd\">");
                key = key.Substring(i, key.Length - i);
                i = key.IndexOf("</div>");
                w.WeaherInfor = key.Substring(0, i).Replace("<div class=\"txbd\">", "");
                key = key.Substring(i, key.Length - i);

                //��ȡ����
                w.Temperature = key.Substring(6, key.IndexOf("</td") - 6);
                HttpContext.Current.Cache.Add("_WeaherObject", w, null, DateTime.Now.AddHours(1),
                    Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                return w;
            }
            catch // (Exception err)
            {
                return null;
            }
        }

        //<option selected value="125">����</option>
        //<option value="292">����</option>
        //<option value="252">�Ϻ�</option>
        //<option value="127">���</option>
        //<option value="212">����</option>
        //<option value="115">����</option>
        //<option value="244">�Ͼ�</option>
        //<option value="211">�人</option>
        //<option value="166">�ɶ�</option>
        //<option value="186">����</option>
        //<option value="82">ʯ��ׯ</option>
        //<option value="84">̫ԭ</option>
        //<option value="189">֣��</option>
        //<option value="103">����</option>
        //<option value="17">������</option>
        //<option value="69">���ͺ���</option>
        //<option value="140">����</option>
        //<option value="248">�Ϸ�</option>
        //<option value="255">����</option>
        //<option value="276">����</option>
        //<option value="287">����</option>
        //<option value="218">��ɳ</option>
        //<option value="296">����</option>
        //<option value="295">����</option>
        //<option value="232">����</option>
        //<option value="264">�ϲ�</option>
        //<option value="227">����</option>
        //<option value="1">���</option>
        //<option value="2">����</option>
        //<option value="179">����</option>
        //<option value="280">̨��</option>
        //<option value="150">����</option>
        //<option value="303">����</option>
        //<option value="57">����</option>
        //<option value="78">����</option>
        //<option value="56">����</option>
        //<option value="28">��³ľ��</option>

    }
    /// <summary>
    /// ������
    /// </summary>
    public class WeaherObject
    {
        private String _Temperature = "";
        private String _WeaherInfor = "";
        private String _City = "";
        private String _WeaherImage = "";

        public String Temperature
        {
            get { return _Temperature; }
            set { _Temperature = value; }
        }

        public String WeaherInfor
        {
            get { return _WeaherInfor; }
            set { _WeaherInfor = value; }
        }

        public String City
        {
            get { return _City; }
            set { _City = value; }
        }

        public String WeaherImage
        {
            get { return _WeaherImage; }
            set { _WeaherImage = value; }
        }
    }
}