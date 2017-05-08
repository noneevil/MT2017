using System;

namespace WebSite.Models
{
    /// <summary>
    /// 位置信息
    /// </summary>
    [Flags]
    public enum PositionType
    {
        中 = 0,
        上 = 1,
        右 = 2,
        下 = 3,
        左 = 4
    }
}