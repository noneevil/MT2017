using System;
using System.Web;
using System.Web.Caching;

namespace CommonUtils
{
    /// <summary>
    /// 天气预报信息获取处理类
    /// </summary>
    public class WeatherHelper
    {
        /// <summary>
        /// 取得指定城市的天气信息
        /// </summary>
        /// <param name="code">城市代码</param>
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
        /// 取得贵阳的天气信息
        /// </summary>
        /// <returns></returns>
        public static String GetWeather()
        {
            return GetWeather("227");
        }

        /// <summary>
        /// 取得贵阳天气信息
        /// </summary>
        /// <param name="code">城市代码</param>
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

                //读取城市
                int i = key.IndexOf("class=\"wht1 lk37\">");
                key = key.Substring(i, key.Length - i);
                int j = key.IndexOf("</td>");
                w.City = key.Substring(0, j).Replace("class=\"wht1 lk37\">", "");

                //读取图片
                i = key.IndexOf("<img");
                key = key.Substring(i, key.Length - i);
                j = key.IndexOf("</td>");
                w.WeaherImage = key.Substring(0, j);

                //读取天气信息
                i = key.IndexOf("<div class=\"txbd\">");
                key = key.Substring(i, key.Length - i);
                i = key.IndexOf("</div>");
                w.WeaherInfor = key.Substring(0, i).Replace("<div class=\"txbd\">", "");
                key = key.Substring(i, key.Length - i);

                //读取气温
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

        //<option selected value="125">北京</option>
        //<option value="292">广州</option>
        //<option value="252">上海</option>
        //<option value="127">天津</option>
        //<option value="212">重庆</option>
        //<option value="115">沈阳</option>
        //<option value="244">南京</option>
        //<option value="211">武汉</option>
        //<option value="166">成都</option>
        //<option value="186">西安</option>
        //<option value="82">石家庄</option>
        //<option value="84">太原</option>
        //<option value="189">郑州</option>
        //<option value="103">长春</option>
        //<option value="17">哈尔滨</option>
        //<option value="69">呼和浩特</option>
        //<option value="140">济南</option>
        //<option value="248">合肥</option>
        //<option value="255">杭州</option>
        //<option value="276">福州</option>
        //<option value="287">厦门</option>
        //<option value="218">长沙</option>
        //<option value="296">深圳</option>
        //<option value="295">南宁</option>
        //<option value="232">桂林</option>
        //<option value="264">南昌</option>
        //<option value="227">贵阳</option>
        //<option value="1">香港</option>
        //<option value="2">澳门</option>
        //<option value="179">昆明</option>
        //<option value="280">台北</option>
        //<option value="150">拉萨</option>
        //<option value="303">海口</option>
        //<option value="57">兰州</option>
        //<option value="78">银川</option>
        //<option value="56">西宁</option>
        //<option value="28">乌鲁木齐</option>

    }
    /// <summary>
    /// 天气类
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