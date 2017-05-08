using System;

namespace WebSite.Models
{
    /// <summary>
    /// 站点模板类型
    /// </summary>
    [Flags]
    public enum SiteTemplate : byte
    {
        空白模板 = 0,
        新闻列表 = 1,
        图片列表 = 2,
        视频列表 = 4,
        下载列表 = 8,
        索引首页 = 16,
        单页内容 = 32
    }
}