using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace WebSite.Interface.Renren.APIUtility
{
    public class APIValidation
    {
        /// <summary>
        /// 获取Access Token,
        /// 通过第一步返回的URL获得参数Code的值，就为Authorization Code
        /// </summary>
        /// <returns>返回获得的Access Token</returns>
        public string GetAccessToken(string ApiKey, string SecretKey, string CallBackURL)
        {
            string accessToken = "";
            try
            {
                if (System.Web.HttpContext.Current.Session["renren_token"] == null)
                {
                    string authorizationCode = System.Web.HttpContext.Current.Request["code"] ?? "";
                    if (authorizationCode != "")
                    {
                        List<APIParameter> paras = new List<APIParameter>() { 
                        new APIParameter("grant_type","authorization_code"),
                        new APIParameter("code",authorizationCode),
                        new APIParameter("client_id", ApiKey),
                        new APIParameter("client_secret", SecretKey),
                        new APIParameter("redirect_uri", CallBackURL)
                    };
                        string requestUrl = HttpUtil.AddParametersToURL(APIConfig.AccessURL, paras);
                        string content = new SyncHttp().HttpPost(requestUrl, "");
                        JavaScriptObject obj = (JavaScriptObject)((JavaScriptArray)JavaScriptConvert.DeserializeObject(content))[0];
                        accessToken = obj["access_token"].ToString();
                        System.Web.HttpContext.Current.Session["renren_token"] = accessToken;
                    }
                }
                else
                {
                    accessToken = System.Web.HttpContext.Current.Session["renren_token"] as string;
                }
            }
            catch
            {
                accessToken = "";
            }
            // 由于获得Json字符串通过JSON.NET获取之后，还是以字符串形式存在，形如“xxxxx”，包括双引号
            // 所以必须替换掉双引号
            accessToken = accessToken.Replace("\"", "");
            return accessToken;
        }
        public string GetTest(string ApiKey, string SecretKey, string code, string CallBackURL)
        {
            List<APIParameter> paras = new List<APIParameter>() { 
                        new APIParameter("grant_type","authorization_code"),
                        new APIParameter("code",code),
                        new APIParameter("client_id", ApiKey),
                        new APIParameter("client_secret", SecretKey),
                        new APIParameter("redirect_uri", CallBackURL)
                    };
            string requestUrl = HttpUtil.AddParametersToURL(APIConfig.AccessURL, paras);
            string content = new SyncHttp().HttpPost(requestUrl, "");
            return content;
        }
    }
}
