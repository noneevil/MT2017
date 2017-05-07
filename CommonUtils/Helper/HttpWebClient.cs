using System;
using System.Net;

namespace CommonUtils
{
    /// <summary>
    /// WebClient扩展,支持会话状态
    /// </summary>
    public class HttpWebClient : System.Net.WebClient
    {
        public HttpWebClient()
        {
            this.Cookies = new CookieContainer();
        }

        public HttpWebClient(CookieContainer cookie)
        {
            this.Headers.Add("Content-Type", "application/x-www-form-urlencoded;");
            this.Headers.Add("Referer", "http://www.google.com");
            this.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)");
            this.Headers.Add("x-requested-with", "XMLHttpRequest");
            this.Cookies = cookie;
        }

        protected CookieContainer Cookies { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            this.Headers["Referer"] = address.AbsoluteUri;
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest) //判断是不是HttpWebRequest.只有HttpWebRequest才有此属性 
            {
                HttpWebRequest httpRequest = request as HttpWebRequest;
                httpRequest.CookieContainer = Cookies;
            }
            return request;
        }
    }
}