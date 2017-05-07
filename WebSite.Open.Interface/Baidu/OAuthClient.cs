using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Text;

namespace WebSite.Interface.Baidu
{

    /// <summary>
    /// 封装百度OAuth 2.0支持的使用Authentication Code，用户名和密码，公钥、密钥，Refresh Token四种获取Access Token方式。
    /// </summary>
    public class OAuthClient
    {
        /// <summary>
        /// 封装百度OAuth 2.0支持的使用Authentication Code，用户名和密码，公钥、密钥，Refresh Token四种获取Access Token方式。
        /// </summary>
        private static readonly string authorizeUrl = "https://openapi.baidu.com/oauth/2.0/authorize";
        /// <summary>
        /// 
        /// </summary>
        private static readonly string tokenUrl = "https://openapi.baidu.com/oauth/2.0/token";


        /// <summary>
        /// 获取Authorization Code。
        /// </summary>
        /// <param name="API_Key">注册应用时获得的API Key。</param>
        /// <param name="redirect_uri">授权后要回调的URI，即接受code的URI。</param>
        /// <remarks></remarks>
        public static void GetAuthorizationCode(string API_Key, string redirect_uri)
        {
            string url = string.Format("{0}?client_id={1}&response_type=code&redirect_uri={2}",
                authorizeUrl, API_Key, redirect_uri);
            HttpContext.Current.Response.Redirect(url);

        }

        /// <summary>
        /// 使用Authentication Code获取Access Token。
        /// </summary>
        /// <param name="API_Key">应用的API Key。</param>
        /// <param name="Secret_Key">'应用的Secret Key。</param>
        /// <param name="code">获得的Authorization Code。</param>
        /// <param name="redirect_uri">必须与获取Authorization Code时传递的“redirect_uri”保持一致。</param>
        /// <returns>包括Access Token等信息的OAuthMessage类型对象。</returns>
        /// <exception cref="OAuthException">OAuthException异常。</exception>
        /// <remarks></remarks>
        public static OAuthMessage GetAccessTokenByAuthorizationCode(string API_Key, string Secret_Key, string code, string redirect_uri)
        {
            string queryString = string.Format(
                "grant_type=authorization_code&code={0}&client_id={1}&client_secret={2}&redirect_uri={3}",
                code, API_Key, Secret_Key, redirect_uri);

            return AccessTokenRequest(queryString);

        }

        /// <summary>
        /// 使用应用公钥、密钥获取Access Token。
        /// </summary>
        /// <param name="API_Key">应用的API Key。</param>
        /// <param name="Secret_Key">应用的Secret Key。</param>
        /// <param name="scope">以空格分隔的权限列表，采用本方式获取Access Token时只能申请跟用户数据无关的数据访问权限。关于权限的具体信息请参考<a href="http://dev.baidu.com/wiki/connect/index.php?title=%E6%9D%83%E9%99%90%E5%88%97%E8%A1%A8">权限列表</a>。</param>
        /// <returns>包括Access Token等信息的OAuthMessage类型对象。</returns>
        /// <exception cref="OAuthException">OAuthException异常。</exception>
        /// <remarks></remarks>
        public static OAuthMessage GetAccessTokenByClientCredentials(string API_Key, string Secret_Key, string scope)
        {
            string queryString = string.Format(
                "grant_type=client_credentials&client_id={0}&client_secret={1}&scope={2}", API_Key, Secret_Key, scope);

            return AccessTokenRequest(queryString);

        }


        /// <summary>
        /// 使用用户名、密码获取Access Token。
        /// </summary>
        /// <param name="API_Key">应用的API Key。</param>
        /// <param name="Secret_Key">应用的Secret Key。</param>
        /// <param name="username">百度用户的用户名。</param>
        /// <param name="password">百度用户的密码。</param>
        /// <param name="scope">以空格分隔的权限列表，若不传递此参数，代表请求用户的默认权限。关于权限的具体信息请参考“<a href="http://dev.baidu.com/wiki/connect/index.php?title=%E6%9D%83%E9%99%90%E5%88%97%E8%A1%A8">权限列表</a>”。</param>
        /// <returns>包括Access Token等信息的OAuthMessage类型对象。</returns>
        /// <exception cref="OAuthException">OAuthException异常。</exception>
        /// <remarks></remarks>
        public static OAuthMessage GetAccessTokenByPasswordCredentials(string API_Key, string Secret_Key, string username, string password, string scope)
        {
            string queryString = string.Format(
                "grant_type=password&client_id={0}&client_secret={1}&scope={2}&username={3}&password={4}", API_Key, Secret_Key, scope, username, password);

            return AccessTokenRequest(queryString);

        }

        /// <summary>
        /// 使用Refresh Token获取Access Token。
        /// </summary>
        /// <param name="API_Key">应用的API Key。</param>
        /// <param name="Secret_Key">应用的Secret Key。</param>
        /// <param name="refresh_token">用于刷新Access Token用的Refresh Token。</param>
        /// <param name="scope">以空格分隔的权限列表，若不传递此参数，代表请求的数据访问操作权限与上次获取Access Token时一致。通过Refresh Token刷新Access Token时所要求的scope权限范围必须小于等于上次获取Access Token时授予的权限范围。关于权限的具体信息请参考“<a href="http://dev.baidu.com/wiki/connect/index.php?title=%E6%9D%83%E9%99%90%E5%88%97%E8%A1%A8">权限列表</a>”。</param>
        /// <returns>包括Access Token等信息的OAuthMessage类型对象。</returns>
        /// <exception cref="OAuthException">OAuthException异常。</exception>
        /// <remarks></remarks>
        public static OAuthMessage GetAccessTokenByRefreshToken(string API_Key, string Secret_Key, string refresh_token, string scope)
        {
            string queryString = string.Format(
                "grant_type=refresh_token&client_id={0}&client_secret={1}&scope={2}&refresh_token={3}", API_Key, Secret_Key, scope, refresh_token);

            return AccessTokenRequest(queryString);
        }


        private static OAuthMessage AccessTokenRequest(string queryString)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            OAuthMessage access_token = null;
            string jsonresult = null;

            try
            {
                jsonresult = new HttpUtils().HttpPost(tokenUrl, queryString);

            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    StreamReader reader = new StreamReader(e.Response.GetResponseStream(), Encoding.UTF8);
                    jsonresult = reader.ReadToEnd();
                }
            }
            if (jsonresult.Contains("error"))
            {
                throw js.Deserialize<OAuthException>(jsonresult);
            }

            jsonresult = jsonresult.Replace(@"\",@"\\");

            access_token = js.Deserialize<OAuthMessage>(jsonresult);

            return access_token;
        }
    }
}
