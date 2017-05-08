using System;
using System.ComponentModel;

namespace WebSite.Models
{
    /// <summary>
    /// 权限类型
    /// </summary>
    [Flags]
    public enum ActionType : short
    {
        /// <summary>
        /// 未设置
        /// </summary>
        [Description("未设置")]
        None = 0,
        /// <summary>
        /// 创建
        /// </summary>
        [Description("添加")]
        Create = 1,
        /// <summary>
        /// 查看或运行
        /// </summary>
        [Description("查看")]
        View = 2,
        /// <summary>
        /// 编辑
        /// </summary>
        [Description("编辑")]
        Edit = 4,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 8,
        /// <summary>
        /// 各种参数设置
        /// </summary>
        [Description("设置")]
        Setting = 16,
        /// <summary>
        /// 回复
        /// </summary>
        [Description("回复")]
        Reply = 32,
        /// <summary>
        /// 确认
        /// </summary>
        [Description("确认")]
        Confirm = 64,
        /// <summary>
        /// 取消
        /// </summary>
        [Description("取消")]
        Cancel = 128,
        /// <summary>
        /// 作废
        /// </summary>
        [Description("作废")]
        Invalid = 256,
        /// <summary>
        /// 备份
        /// </summary>
        [Description("备份")]
        BackUp = 512,
        /// <summary>
        /// 还原
        /// </summary>
        [Description("还原")]
        Restore = 1024,
        /// <summary>
        /// 安装
        /// </summary>
        [Description("安装")]
        Instal = 2048,
        /// <summary>
        /// 卸载
        /// </summary>
        [Description("卸载")]
        UnInstal = 4096,
        /// <summary>
        /// 所有权限
        /// </summary>
        [Description("所有权限")]
        ALL = Create | View | Edit | Delete | Setting | Reply | Confirm | Cancel | Invalid | BackUp | Restore | Instal | UnInstal
    }
}
