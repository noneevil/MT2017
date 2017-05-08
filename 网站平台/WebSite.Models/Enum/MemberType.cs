using System;

namespace WebSite.Models
{
    /// <summary>
    /// 会员类型
    /// </summary>
    [Flags]
    public enum MemberType
    {
        匿名用户 = 0,
        普通会员 = 1,
        银卡会员 = 2,
        金卡会员 = 4,
        钻石会员 = 8,
        所有会员 = 普通会员 | 银卡会员 | 金卡会员 | 钻石会员
    }
}
