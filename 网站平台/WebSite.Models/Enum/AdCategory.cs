using System;

namespace WebSite.Models
{
    /// <summary>
    /// 广告类型
    /// </summary>
    [Flags]
    public enum AdCategory
    {
        图片 = 1,
        Flash动画 = 2,
        FLV视频 = 4
    }
}