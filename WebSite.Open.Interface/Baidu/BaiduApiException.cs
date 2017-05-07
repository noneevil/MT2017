using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSite.Interface.Baidu
{

    /// <summary>
    /// Open API调用的异常响应结果。
    /// </summary>
    /// <remarks></remarks>
    public class BaiduApiException:Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="BaiduApiException"/> class.
        /// </summary>
        /// <param name="error_code">错误码，查看所有错误码定义请参考<a href="http://dev.baidu.com/wiki/connect/index.php?title=%E7%99%BE%E5%BA%A6Open_API%E9%94%99%E8%AF%AF%E7%A0%81%E5%AE%9A%E4%B9%89">常见错误码说明</a>。</param>
        /// <param name="error_msg">对调用失败原因的描述。</param>
        /// <remarks></remarks>
        public BaiduApiException(string error_code, string error_msg)
            : base(error_msg)
        {
            this.Error_code = error_code;
            this.Error_msg = error_msg;
        }

        /// <summary>
        /// 初始化 <see cref="BaiduApiException"/> 类的新实例。
        /// </summary>
        /// <remarks></remarks>
        public BaiduApiException()
        { 
        
        }

        private string error_code;

        /// <summary>
        /// 获取或设置错误码，查看所有错误码定义请参考常见<a href="http://dev.baidu.com/wiki/connect/index.php?title=%E7%99%BE%E5%BA%A6Open_API%E9%94%99%E8%AF%AF%E7%A0%81%E5%AE%9A%E4%B9%89">常见错误码说明</a>。
        /// </summary>
        public string Error_code
        {
            get { return error_code; }
            set { error_code = value; }
        }


        private string error_msg;

        /// <summary>
        /// 获取或设置调用失败原因的描述。
        /// </summary>
        public string Error_msg
        {
            get { return error_msg; }
            set { error_msg = value; }
        }
    }
}
