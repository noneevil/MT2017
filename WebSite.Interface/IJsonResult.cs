using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebSite.Interface
{
    /// <summary>
    /// JSON消息内容实体
    /// </summary>
    public struct IJsonResult
    {
        /// <summary>
        /// 处理状态
        /// </summary>
        public Boolean Status { get; set; }
        /// <summary>
        /// 返回内容
        /// </summary>
        public String Text { get; set; }
        /// <summary>
        /// 样式
        /// </summary>
        public String Css { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public MessageICO Ico { get; set; }
        /// <summary>
        /// 传递数据
        /// </summary>
        public Object Data { get; set; }


        public String info { get; set; }
        public String status { get; set; }
    }
    /// <summary>
    /// 对话框图标
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MessageICO
    {
        /// <summary>
        /// 错误
        /// </summary>
        Failure,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 提示
        /// </summary>
        Prompt,
        /// <summary>
        /// 询问
        /// </summary>
        Request,
        /// <summary>
        /// 加载中
        /// </summary>
        Loading
    }
}
