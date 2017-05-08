using System.ComponentModel;

namespace WebSite.Models
{
    /// <summary>
    /// 会话视图存储模式
    /// </summary>
    public enum ViewStateType
    {
        [Description("默认")]
        HiddenField = 0,
        [Description("Session")]
        Session = 2,
        [Description("压缩传输")]
        Compress = 4,
        [Description("文件存储")]
        File = 8,
        [Description("SQLite数据库存储")]
        SQLite = 16
    }
}
