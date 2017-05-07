using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CommonUtils
{
    /// <summary>
    /// Http请求辅助处理类
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 向指定地址发送POST请求
        /// </summary>
        /// <param name="postUrl">指定的网页地址</param>
        /// <param name="postData">POST的数据（格式为：p1=v1&p1=v2）</param>
        /// <param name="chars_set">可采用如UTF-8,GB2312,GBK等</param>
        /// <param name="postCookie">设置与此请求关联的 cookie</param>
        /// <returns>页面返回内容</returns>
        public static String Post(String postUrl, NameValueCollection postData, String chars_set)
        {
            //数据编码
            String postdata = ToNameValueString(postData);
            Encoding encoding = Encoding.GetEncoding(chars_set);
            Byte[] data = encoding.GetBytes(postdata);

            //请求目标网页
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.Method = "POST";
            request.AllowAutoRedirect = false;
            request.ContentLength = data.Length;
            request.Referer = "http://www.google.com";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0+(compatible;+Googlebot/2.1;++http://www.google.com/bot.html)";

            //模拟一个UserAgent
            Stream newStream = request.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            //获取网页响应结果
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, encoding, true))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
        /// <summary>
        /// 向地址发送GET请求
        /// </summary>
        /// <param name="getUrl">地址(格式:http://host/page?p1=v1&p2=v2</param>
        /// <param name="chars_set">可采用如UTF-8,GB2312,GBK等</param>
        /// <returns>页面返回内容</returns>
        public static String Get(String getUrl, String chars_set)
        {
            CookieContainer cookie = new CookieContainer();
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(getUrl);
            myRequest.AllowAutoRedirect = true;
            myRequest.CookieContainer = cookie;
            using (HttpWebResponse myresponse = (HttpWebResponse)myRequest.GetResponse())
            {
                myresponse.Cookies = cookie.GetCookies(myRequest.RequestUri);
                using (Stream mystream = myresponse.GetResponseStream())
                {
                    using (StreamReader myreader = new StreamReader(mystream, Encoding.GetEncoding(chars_set), true))
                    {
                        return myreader.ReadToEnd();
                    }
                }
            }
        }
        /// <summary>
        /// 转发客户端请求数据
        /// </summary>
        /// <param name="Url">转发地址</param>
        /// <param name="Request">请求的页的HttpRequest对象</param>
        /// <returns>转发地址处理后返回的字符</returns>
        public static String UploadData(String Url, HttpRequest Request)
        {
            Byte[] data = new Byte[Request.InputStream.Length];
            Request.InputStream.Read(data, 0, data.Length);

            System.Net.WebClient web = new System.Net.WebClient();
            web.Headers.Add("Referer", Request.Url.AbsoluteUri);
            web.Headers.Add("Accept", Request.Headers["Accept"]);
            web.Headers.Add("User-Agent", Request.Headers["User-Agent"]);
            web.Headers.Add("Content-Type", Request.Headers["Content-Type"]);
            web.Headers.Add("Accept-Encoding", Request.Headers["Accept-Encoding"]);

            Byte[] byt = web.UploadData(Url, data);
            String result = Encoding.UTF8.GetString(byt);
            web.Dispose();
            return result;
        }
        /// <summary>
        /// 生成NameValueCollection字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns>返回QueryString字符串</returns>
        public static String ToNameValueString(NameValueCollection data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Count; i++)
            {
                if (i != 0) sb.Append("&");
                sb.Append(data.GetKey(i)).Append("=").Append(data[i]);
            }
            return sb.ToString();
        }

        public static HttpWebRequest CreateHttpWebRequest(String url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            //有些页面不设置用户代理信息则会抓取不到内容
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1";
            return httpWebRequest;
        }

        public static String GetResponseText(HttpWebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            HttpWebResponse httpWebResponse = null;
            String result;
            try
            {
                httpWebResponse = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    result = ex.Message;
                    return result;
                }
                httpWebResponse = (HttpWebResponse)ex.Response;
            }
            using (httpWebResponse)
            {
                Stream responseStream = httpWebResponse.GetResponseStream();
                using (StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding(httpWebResponse.CharacterSet)))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            return result;
        }

        public static String HttpGet(String url, CookieContainer cookieContainer)
        {
            HttpWebRequest httpWebRequest = HttpHelper.CreateHttpWebRequest(url);
            if (cookieContainer != null)
            {
                httpWebRequest.CookieContainer = cookieContainer;
            }
            return HttpHelper.GetResponseText(httpWebRequest);
        }

        public static String HttpPost(String url, String postData, CookieContainer cookieContainer)
        {
            HttpWebRequest httpWebRequest = HttpHelper.CreateHttpWebRequest(url);
            if (cookieContainer != null)
            {
                httpWebRequest.CookieContainer = cookieContainer;
            }
            HttpHelper.WritePostData(httpWebRequest, postData, Encoding.UTF8);

            return HttpHelper.GetResponseText(httpWebRequest);
        }

        public static void WritePostData(HttpWebRequest request, String postData, Encoding encoding)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if (!String.IsNullOrEmpty(postData))
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }
                if (request.Method != "POST")
                {
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded; charset=" + encoding.WebName;
                }
                using (BinaryWriter binaryWriter = new BinaryWriter(request.GetRequestStream()))
                {
                    binaryWriter.Write(encoding.GetBytes(postData));
                }
            }
        }


        /// <summary>
        /// 解析URL(可以正确识别UTF-8和GB2312编码)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static String DecodeURL(String url)
        {
            //正则1：^(?:[\x00-\x7f]|[\xe0-\xef][\x80-\xbf]{2})+$
            //正则2：^(?:[\x00-\x7f]|[\xfc-\xff][\x80-\xbf]{5}|[\xf8-\xfb][\x80-\xbf]{4}|[\xf0-\xf7][\x80-\xbf]{3}|[\xe0-\xef][\x80-\xbf]{2}|[\xc0-\xdf][\x80-\xbf])+$
            //如果不考虑哪些什么拉丁文啊，希腊文啊。。。乱七八糟的外文，用短的正则，即正则1
            //如果考虑哪些什么拉丁文啊，希腊文啊。。。乱七八糟的外文，用长的正则，即正则2
            //本方法使用的正则1
            if (Regex.IsMatch(HttpUtility.UrlDecode(url, Encoding.GetEncoding("iso-8859-1")), @"^(?:[\x00-\x7f]|[\xe0-\xef][\x80-\xbf]{2})+$"))
            {
                return HttpUtility.UrlDecode(url, Encoding.GetEncoding("UTF-8"));
            }
            else
            {
                return HttpUtility.UrlDecode(url, Encoding.GetEncoding("GB2312"));
            }
        }
        /// <summary>
        /// 解析URL，返回查询字符串集合(已经正确识别UTF-8和GB2312编码)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static NameValueCollection ParseQuery(String url)
        {
            NameValueCollection query = new NameValueCollection();
            if (!String.IsNullOrEmpty(url))
            {
                if (url.Contains("?"))
                {
                    query = HttpUtility.ParseQueryString(DecodeURL(url.Substring(url.IndexOf("?"))));
                }
                else
                {
                    query = HttpUtility.ParseQueryString(DecodeURL(url));
                }
            }
            return query;
        }
        /// <summary>
        /// 解析URL返回域名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static String PraseDomain(String url)
        {
            if (String.IsNullOrEmpty(url)) return String.Empty;

            try
            {
                Uri uri = new Uri(url);
                return uri.Host;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}