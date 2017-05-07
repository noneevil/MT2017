using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSite.Interface.Baidu
{
    /// <summary>
    /// Baidu OAuth2.0 请求数据错误异常类。
    /// </summary>
    public class OAuthException:Exception
    {

        private string error;

        /// <summary>
        /// 错误码；关于错误码的详细信息请参考<a href="http://dev.baidu.com/wiki/connect/index.php?title=%E7%99%BE%E5%BA%A6OAuth2.0%E9%94%99%E8%AF%AF%E5%93%8D%E5%BA%94">百度OAuth2.0错误码响应</a>一节。
        /// </summary>
        public string Error
        {
            get { return error; }
            set { error = value; }
        }


        private string error_description;

        /// <summary>
        /// 错误描述信息，用来帮助理解和解决发生的错误。
        /// </summary>
        public string Error_description
        {
            get { return error_description; }
            set { error_description = value; }
        }


    }
}
