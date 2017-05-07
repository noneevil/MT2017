using System;
using System.Collections.Generic;
using System.Text;

namespace WebSite.Interface.Tencent
{
    /// <summary>
    /// 请求处理
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// 获取用户的唯一标识，和QQ号码一一对应。
        /// </summary>
        /// <remarks>
        /// 只有登录成功后才可以获取。
        /// </remarks>
        public static string OpenId
        {
            get
            {
                return OAuthHelper.OpenId;
            }
        }

        public static void Login(string appid, string appkey, string qqcallbackurl, string onSuccessRedirectUrl, string onFailRedirectUrl)
        {
            OAuthHelper.Login(appid, appkey, qqcallbackurl, onSuccessRedirectUrl, onFailRedirectUrl);
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="format">数据类型</param>
        /// <returns>xml获知json 文本</returns>
        public static string GetUserInfo(DataFormatEnum format)
        {
            return OAuthHelper.GetUserInfo(format);
        }
    }
}
