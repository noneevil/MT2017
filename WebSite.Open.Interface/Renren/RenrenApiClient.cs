using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSite.Interface.Renren.APIUtility;
using Newtonsoft.Json;

namespace WebSite.Interface.Renren
{
    public class RenrenApiClient
    {
        /// <summary>
        /// 获取 Authorization code
        /// 执行此方法后，将会访问callback地址，
        /// 返回需要访问的URL地址，形式如：http://www.exaple.com?code=xxxx
        /// code就为需要获得的Authorization code。
        /// </summary>
        public void GetAuthorizationCode(string ApiKey, string CallBackURL)
        {
            string authorizationUrl = APIConfig.AuthorizationURL;
            List<APIParameter> paras = new List<APIParameter>() { 
                new APIParameter("client_id", ApiKey),
                new APIParameter("response_type","code"),
                new APIParameter("redirect_uri", CallBackURL)
            };
            string requestUrl = HttpUtil.AddParametersToURL(authorizationUrl, paras);
            System.Web.HttpContext.Current.Response.Redirect(requestUrl);
        }
    }
}
