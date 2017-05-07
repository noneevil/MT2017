using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;
using System.Xml;
using System.IO;

namespace WebSite.Interface.Sina
{
    public class SinaApi
    {
        private string basicApiUrl = "http://api.t.sina.com.cn/";

        /// <summary>
        /// 连接新浪微博 
        /// </summary>
        public void ConnectSina()
        {
            HttpGet httpRequest = HttpRequestFactory.CreateHttpRequest(Method.GET) as HttpGet;
            httpRequest.GetRequestToken();
            string url = httpRequest.GetAuthorizationUrl();
            HttpContext.Current.Session["sina_token"] = httpRequest.Token;
            HttpContext.Current.Session["sina_token_secret"] = httpRequest.TokenSecret;
            HttpContext.Current.Response.Redirect(url + "&oauth_callback=" + HttpContext.Current.Server.UrlEncode(SinaConfig.CallBackURI) + "");
        }
        /// <summary>
        /// 退出微博 
        /// </summary>
        public void DisConnectSina()
        {
            HttpContext.Current.Session["sina_token"] = null;
            HttpContext.Current.Session["sina_token_secret"] = null;
            HttpContext.Current.Session.Abandon();
        }
        /// <summary>
        /// 获取token 
        /// </summary>
        /// <param name="oauth_verifier"></param>
        public void getAccessToken(string oauth_verifier)
        {
            if (!string.IsNullOrEmpty(oauth_verifier))
            {
                HttpGet httpRequest = HttpRequestFactory.CreateHttpRequest(Method.GET) as HttpGet;
                httpRequest.Token = HttpContext.Current.Session["sina_token"].ToString();
                httpRequest.TokenSecret = HttpContext.Current.Session["sina_token_secret"].ToString();
                httpRequest.Verifier = oauth_verifier;
                httpRequest.GetAccessToken();
                HttpContext.Current.Session["sina_token"] = httpRequest.Token;
                HttpContext.Current.Session["sina_token_secret"] = httpRequest.TokenSecret;
            }
        }
        /// <summary>
        /// Oauth调用新浪微博接口，返回xml数据格式的字符串  
        /// </summary>
        /// <param name="methodname"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string GetSinaResponse(string methodname, Method method, string postData)
        {
            if (HttpContext.Current.Session["sina_token"] != null && HttpContext.Current.Session["sina_token_secret"] != null)
            {
                return this.GetSinaResponse(methodname, method, postData, HttpContext.Current.Session["sina_token"].ToString(), HttpContext.Current.Session["sina_token_secret"].ToString());
            }
            return string.Empty;
        }
        /// <summary>
        /// Oauth调用新浪微博接口，返回xml数据格式的字符串   
        /// </summary>
        /// <param name="methodname"></param>
        /// <param name="method"></param>
        /// <param name="postData"></param>
        /// <param name="oauth_token"></param>
        /// <param name="oauth_token_secret"></param>
        /// <returns></returns>
        private string GetSinaResponse(string methodname, Method method, string postData, string oauth_token, string oauth_token_secret)
        {
            if (!string.IsNullOrEmpty(oauth_token) && !string.IsNullOrEmpty(oauth_token_secret))
            {
                var httpRequest = HttpRequestFactory.CreateHttpRequest(method);
                httpRequest.Token = oauth_token;
                httpRequest.TokenSecret = oauth_token_secret;
                httpRequest.UserRemoteIP = HttpContext.Current.Request.UserHostAddress;
                string url = basicApiUrl + methodname;
                return httpRequest.Request(url, postData);
            }
            return string.Empty;
        }
        /// <summary>
        /// 发布微博 
        /// </summary>
        /// <param name="msg"></param>
        public void PublicWeibo(string msg)
        {
            this.GetSinaResponse("statuses/update.xml?", Method.POST, "status=" + HttpUtility.UrlEncode(msg) + "");
        }
        /// <summary>
        /// 发布带图标的微博 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="picpath"></param>
        public void PublicWeiboWithPicture(string msg, string picpath)
        {
            HttpPost httpRequest = HttpRequestFactory.CreateHttpRequest(Method.POST) as HttpPost;
            httpRequest.Token = HttpContext.Current.Session["sina_token"].ToString();
            httpRequest.TokenSecret = HttpContext.Current.Session["sina_token_secret"].ToString();
            httpRequest.UserRemoteIP = HttpContext.Current.Request.UserHostAddress;
            string url = basicApiUrl + "statuses/upload.xml?";
            httpRequest.RequestWithPicture(url, "status=" + HttpUtility.UrlEncode(msg), picpath);
        }
        public string GetUserResponse()
        {
            return this.GetSinaResponse("account/verify_credentials.xml", Method.GET, string.Empty);
        }
        /// <summary>
        /// 验证用户是否登陆了新浪微博，返回用户实体 
        /// </summary>
        /// <returns></returns>
        public SinaUser verify_credentials()
        {
            return this.GetSinaUserModel(GetUserResponse());
        }

        /// <summary>
        /// 根据存储的oauth_token和oauth_token_secret验证用户是否登陆了新浪微博，返回用户实体  
        /// </summary>
        /// <param name="oauth_token"></param>
        /// <param name="oauth_token_secret"></param>
        /// <returns></returns>
        public SinaUser verify_credentials(string oauth_token, string oauth_token_secret)
        {
            string responseBody = this.GetSinaResponse("account/verify_credentials.xml", Method.GET, string.Empty, oauth_token, oauth_token_secret);
            return this.GetSinaUserModel(responseBody);
        }

        /// <summary>
        /// 获取登陆用户实体 
        /// </summary>
        /// <param name="xmlResponseBody"></param>
        /// <returns></returns>
        public SinaUser GetSinaUserModel(string xmlResponseBody)
        {
            if (!string.IsNullOrEmpty(xmlResponseBody) && xmlResponseBody.IndexOf("screen_name") > -1)
            {
                SinaUser user = new SinaUser();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlResponseBody);
                user.id = Convert.ToInt32(xmlDoc.GetElementsByTagName("id")[0].InnerText);
                user.screen_name = xmlDoc.GetElementsByTagName("screen_name")[0].InnerText;
                user.name = xmlDoc.GetElementsByTagName("name")[0].InnerText;
                user.province = xmlDoc.GetElementsByTagName("province")[0].InnerText;
                user.city = xmlDoc.GetElementsByTagName("city")[0].InnerText;
                user.location = xmlDoc.GetElementsByTagName("location")[0].InnerText;
                user.description = xmlDoc.GetElementsByTagName("description")[0].InnerText;
                user.url = xmlDoc.GetElementsByTagName("url")[0].InnerText;
                user.profile_image_url = xmlDoc.GetElementsByTagName("profile_image_url")[0].InnerText;
                user.domain = xmlDoc.GetElementsByTagName("domain")[0].InnerText;
                user.gender = xmlDoc.GetElementsByTagName("gender")[0].InnerText;
                user.statuses_count = Convert.ToInt32(xmlDoc.GetElementsByTagName("statuses_count")[0].InnerText);
                user.friends_count = Convert.ToInt32(xmlDoc.GetElementsByTagName("friends_count")[0].InnerText);
                user.followers_count = Convert.ToInt32(xmlDoc.GetElementsByTagName("followers_count")[0].InnerText);
                return user;
            }
            return null;
        }
    }
}
