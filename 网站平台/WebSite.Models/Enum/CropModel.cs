using System;

namespace WebSite.Models
{
    /// <summary>
    /// 缩略图裁切模式
    /// </summary>
    [Flags]
    public enum CropModel
    {
        /// <summary>
        /// 按比例缩放
        /// </summary>
        Scale = 0,
        /// <summary>
        /// 强制缩放(会变形)
        /// </summary>
        Zoom = 1,
        /// <summary>
        /// 裁剪
        /// </summary>
        Cut = 2
    }
}