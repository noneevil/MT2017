using System;

namespace WebSite.Models
{
    /// <summary>
    /// 链接打开方式
    /// </summary>
    [Flags]
    public enum TargetCategory
    {
        _blank = 1,
        _parent = 2,
        _search = 4,
        _self = 8,
        _top = 16
    }
}