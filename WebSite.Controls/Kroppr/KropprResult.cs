using System;

namespace WebSite.Controls
{
    /// <summary>
    /// 裁剪输出信息
    /// </summary>
    public struct KropprResult
    {
        /// <summary>
        /// 状态
        /// </summary>
        public Boolean status { get; set; }
        /// <summary>
        /// 缩略图地址
        /// </summary>
        public String image { get; set; }
        /// <summary>
        /// 真实地址
        /// </summary>
        public String realimage { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public String type { get; set; }
    }
}