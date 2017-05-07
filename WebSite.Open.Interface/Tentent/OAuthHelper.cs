using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Web;
using CommonUtils;

namespace WebSite.Interface.Tencent
{
    public static class OAuthHelper
    {
        #region 地址常量
        /// <summary>
        /// 请求临时token url
        /// </summary>
        internal const string QZONEOAUTH_REQUEST_TOKEN = "http://openapi.qzone.qq.com/oauth/qzoneoauth_request_token";

        /// <summary>
        /// QQ认证登录 url
        /// </summary>
        internal const string QZONEOAUTH_AUTHORIZE = "http://openapi.qzone.qq.com/oauth/qzoneoauth_authorize";

        /// <summary>
        /// 请求access token url
        /// </summary>
        internal const string QZONEOAUTH_ACCESS_TOKEN = "http://openapi.qzone.qq.com/oauth/qzoneoauth_access_token";

        /// <summary>
        /// 获取用户信息地址
        /// </summary>
        internal const string GET_USER_INFO = "http://openapi.qzone.qq.com/user/get_user_info";
        #endregion

        #region 认证相关变量
        public static string AppId
        {
            get
            {
                return SessionHelper.GetSessionString("OAuth_Tencent.AppId");
            }

            set
            {
                SessionHelper.SetSession("OAuth_Tencent.AppId", value);
            }
        }

        public static string AppKey
        {
            get
            {
                return SessionHelper.GetSessionString("OAuth_Tencent.AppKey");
            }

            set
            {
                SessionHelper.SetSession("OAuth_Tencent.AppKey", value);
            }
        }
        public static string SuccessRedirectUrl
        {
            get
            {
                return SessionHelper.GetSessionString("OAuth_Tencent.SuccessRedirectUrl");
            }

            set
            {
                SessionHelper.SetSession("OAuth_Tencent.SuccessRedirectUrl", value);
            }
        }
        public static string FailRedirectUrl
        {
            get
            {
                return SessionHelper.GetSessionString("OAuth_Tencent.FailRedirectUrl");
            }

            set
            {
                SessionHelper.SetSession("OAuth_Tencent.FailRedirectUrl", value);
            }
        }

        public static string OAuth_Token
        {
            get
            {
                return SessionHelper.GetSessionString("OAuth_Tencent.OAuth_Token");
            }

            set
            {
                SessionHelper.SetSession("OAuth_Tencent.OAuth_Token", value);
            }
        }
        public static string OAuth_Token_Secret
        {
            get
            {
                return SessionHelper.GetSessionString("OAuth_Tencent.OAuth_Token_Secret");
            }

            set
            {
                SessionHelper.SetSession("OAuth_Tencent.OAuth_Token_Secret", value);
            }
        }
        public static string OpenId
        {
            get
            {
                return SessionHelper.GetSessionString("OAuth_Tencent.OpenId");
            }

            set
            {
                SessionHelper.SetSession("OAuth_Tencent.OpenId", value);
            }
        }
        #endregion

        #region 认证过程
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appkey"></param>
        /// <param name="qqcallbackurl"></param>
        /// <param name="redirecturl"></param>
        /// <param name="onFail"></param>
        public static void Login(string appid, string appkey, string qqcallbackurl, string onSuccessRedirectUrl, string onFailRedirectUrl)
        {
            OAuthHelper.AppId = appid;
            OAuthHelper.AppKey = appkey;
            OAuthHelper.SuccessRedirectUrl = onSuccessRedirectUrl;
            OAuthHelper.FailRedirectUrl = onFailRedirectUrl;

            //// 生成加密的验证数据
            int oauth_nonce = DateTime.Today.Year + DateTime.Today.Month * DateTime.Today.Day * DateTime.Now.Second + DateTime.Now.Millisecond;
            int oauth_timestamp = Convert.ToInt32((DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(Convert.ToDateTime("1970-1-1"))).TotalSeconds);

            Dictionary<string, string> keyValues = new Dictionary<string, string>();

            keyValues.Add("oauth_consumer_key", OAuthHelper.AppId);
            keyValues.Add("oauth_nonce", oauth_nonce.ToString());
            keyValues.Add("oauth_signature_method", "HMAC-SHA1");
            keyValues.Add("oauth_timestamp", oauth_timestamp.ToString());
            keyValues.Add("oauth_version", "1.0");

            string key = string.Format("{0}&", OAuthHelper.AppKey);

            string url = OAuthHelper.GenerationRequestUrl(OAuthHelper.QZONEOAUTH_REQUEST_TOKEN, keyValues, key);
            string result = OAuthHelper.RequestUrl(url);

            if (OAuthHelper.ValidationResult(result))
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(result);
                string redirectUrl = string.Format("{0}?oauth_consumer_key={1}&oauth_token={2}&oauth_callback={3}",
                    OAuthHelper.QZONEOAUTH_AUTHORIZE,
                    appid, nvc["oauth_token"],
                    HttpContext.Current.Server.UrlEncode(qqcallbackurl));

                OAuthHelper.OAuth_Token_Secret = nvc["oauth_token_secret"];
                HttpContext.Current.Response.Redirect(redirectUrl);
                return;
            }

            OAuthHelper.RedirectFailPage(result);
        }
        public static void RequestAccessToken(string appid, string appkey, string oauth_token, string oauth_token_secret, string oauth_vericode)
        {
            // 生成加密的验证数据
            int oauth_nonce = DateTime.Today.Year + DateTime.Today.Month * DateTime.Today.Day * DateTime.Now.Second + DateTime.Now.Millisecond;
            int oauth_timestamp = Convert.ToInt32((DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(Convert.ToDateTime("1970-1-1"))).TotalSeconds);

            Dictionary<string, string> keyValues = new Dictionary<string, string>();

            keyValues.Add("oauth_consumer_key", appid);
            keyValues.Add("oauth_nonce", oauth_nonce.ToString());
            keyValues.Add("oauth_signature_method", "HMAC-SHA1");
            keyValues.Add("oauth_timestamp", oauth_timestamp.ToString());
            keyValues.Add("oauth_token", oauth_token);
            keyValues.Add("oauth_vericode", oauth_vericode);
            keyValues.Add("oauth_version", "1.0");

            string key = string.Format("{0}&{1}", appkey, oauth_token_secret);

            string url = OAuthHelper.GenerationRequestUrl(OAuthHelper.QZONEOAUTH_ACCESS_TOKEN, keyValues, key);
            string result = OAuthHelper.RequestUrl(url);

            if (OAuthHelper.ValidationResult(result))
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(result);

                OAuthHelper.OAuth_Token = nvc["oauth_token"];
                OAuthHelper.OAuth_Token_Secret = nvc["oauth_token_secret"];
                OAuthHelper.OpenId = nvc["openid"];

                HttpContext.Current.Response.Redirect(OAuthHelper.SuccessRedirectUrl);
            }

            OAuthHelper.RedirectFailPage(result);
        }
        #endregion

        #region 基础共用方法
        public static bool ValidationResult(string result)
        {
            if (string.IsNullOrEmpty(result))
            {
                return false;
            }

            return !result.ToLower().Contains("error_code");
        }
        public static void RedirectFailPage(string result)
        {
            if (string.IsNullOrEmpty(OAuthHelper.FailRedirectUrl))
            {
                return;
            }

            if (!string.IsNullOrEmpty(result))
            {
                result = result.Replace("\r\n", "").Replace("\n", "");
            }

            if (OAuthHelper.FailRedirectUrl.Contains("?"))
            {
                HttpContext.Current.Response.Redirect(string.Format("{0}&{1}", OAuthHelper.FailRedirectUrl, result));
            }
            else
            {
                HttpContext.Current.Response.Redirect(string.Format("{0}?{1}", OAuthHelper.FailRedirectUrl, result));
            }
        }
        public static string RequestUrl(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.MaximumAutomaticRedirections = 3;
            request.Timeout = 10000;
            HttpWebResponse res = (HttpWebResponse)request.GetResponse();
            Stream response = res.GetResponseStream();
            StreamReader sr = new StreamReader(response);

            string result = sr.ReadToEnd();
            sr.Close();
            response.Close();

            return result;
        }
        public static string GenerationRequestUrl(string requesturl, Dictionary<string, string> keyvalues, string key)
        {
            List<string> queryStrings = new List<string>();

            foreach (var item in keyvalues)
            {
                //keyValues.Add("oauth_token", oauth_token);
                //keyValues.Add("oauth_vericode", oauth_vericode);

                //if (item.Key != "oauth_token" && item.Key != "oauth_vericode")
                //{
                queryStrings.Add(string.Format("{0}={1}", item.Key, item.Value));
                // }
            }

            string srcStr = string.Format("GET&{0}&{1}",
              Microsoft.JScript.GlobalObject.encodeURIComponent(requesturl),
              Microsoft.JScript.GlobalObject.encodeURIComponent(string.Join("&", queryStrings.ToArray())));

            string signStr = Microsoft.JScript.GlobalObject.encodeURIComponent(OAuthHelper.SignWithHMAC(srcStr, key));


            //queryStrings.Clear();
            //foreach (var item in keyvalues)
            //{
            //    queryStrings.Add(string.Format("{0}={1}", item.Key, item.Value));
            //}

            return string.Format("{0}?{1}&oauth_signature={2}", requesturl, string.Join("&", queryStrings.ToArray()), signStr);
        }
        public static string SignWithHMAC(string dataToSign, string key)
        {
            HMACSHA1 hmac = new HMACSHA1(System.Text.ASCIIEncoding.ASCII.GetBytes(key));
            CryptoStream cs = new CryptoStream(Stream.Null, hmac, CryptoStreamMode.Write);

            byte[] dataBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(dataToSign);
            cs.Write(dataBytes, 0, dataBytes.Length);
            cs.Close();

            return Convert.ToBase64String(hmac.Hash, Base64FormattingOptions.None);
        }
        #endregion

        #region 资源访问
        public static string GetUserInfo(DataFormatEnum format)
        {
            // 生成加密的验证数据
            int oauth_nonce = DateTime.Today.Year + DateTime.Today.Month * DateTime.Today.Day * DateTime.Now.Second + DateTime.Now.Millisecond;
            int oauth_timestamp = Convert.ToInt32((DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(Convert.ToDateTime("1970-1-1"))).TotalSeconds);

            Dictionary<string, string> keyValues = new Dictionary<string, string>();

            keyValues.Add("format", format.ToString().ToLower());
            keyValues.Add("oauth_consumer_key", OAuthHelper.AppId);
            keyValues.Add("oauth_nonce", oauth_nonce.ToString());
            keyValues.Add("oauth_signature_method", "HMAC-SHA1");
            keyValues.Add("oauth_timestamp", oauth_timestamp.ToString());
            keyValues.Add("oauth_token", OAuthHelper.OAuth_Token);
            keyValues.Add("oauth_version", "1.0");
            keyValues.Add("openid", OAuthHelper.OpenId);

            string key = string.Format("{0}&{1}", OAuthHelper.AppKey, OAuthHelper.OAuth_Token_Secret);

            string url = OAuthHelper.GenerationRequestUrl(OAuthHelper.GET_USER_INFO, keyValues, key);
            string result = OAuthHelper.RequestUrl(url);
            return result;
        }
        #endregion
    }
}
