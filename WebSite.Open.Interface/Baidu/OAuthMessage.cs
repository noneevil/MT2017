using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSite.Interface.Baidu
{

    /// <summary>
    /// 请求OAuth服务返回包括Access Token等消息类型。
    /// </summary>
    public class OAuthMessage
    {

        private int expires_in;

        /// <summary>
        /// Access Token的有效期，以秒为单位。
        /// </summary>
        public int Expires_in
        {
            get { return expires_in; }
            set { expires_in = value; }
        }


        private string access_token;

        /// <summary>
        /// 要获取的Access Token。
        /// </summary>
        public string Access_token
        {
            get { return access_token; }
            set { access_token = value; }
        }


        private string session_secret;

        /// <summary>
        /// 基于http调用Open API时计算参数签名用的签名密钥。
        /// </summary>
        public string Session_secret
        {
            get { return session_secret; }
            set { session_secret = value; }
        }


        private string session_key;

        /// <summary>
        /// 基于http调用Open API时所需要的Session Key，其有效期与Access Token一致。
        /// </summary>
        public string Session_key
        {
            get { return session_key; }
            set { session_key = value; }
        }


        private string scope;

        /// <summary>
        /// Access Token最终的访问范围，即用户实际授予的权限列表（用户在授权页面时，有可能会取消掉某些请求的权限），关于权限的具体信息参考<a href="http://dev.baidu.com/wiki/connect/index.php?title=%E6%9D%83%E9%99%90%E5%88%97%E8%A1%A8">权限列表</a>一节。
        /// </summary>
        public string Scope
        {
            get { return scope; }
            set { scope = value; }
        }

    }
}
