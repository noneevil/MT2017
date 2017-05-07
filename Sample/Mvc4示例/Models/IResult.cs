using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc4Example.Models
{
    /// <summary>
    /// 返回数据模型结构
    /// </summary>
    public struct IResult
    {
        /// <summary>
        /// 数据载体
        /// </summary>
        public Object data { get; set; }
        /// <summary>
        /// 请求头信息
        /// </summary>
        public String header { get; set; }
    }
}