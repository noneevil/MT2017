using System;
using System.Web;

namespace CommonUtils
{
    public abstract class Utils
    {
        /// <summary>
        /// 客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static String GetIp()
        {
            String IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];//可以透过代理服务器
            if (String.IsNullOrEmpty(IP)) IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return IP;
        }
    }
}
