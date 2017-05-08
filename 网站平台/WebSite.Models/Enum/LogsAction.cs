using System;

namespace WebSite.Models
{
    /// <summary>
    /// 日志操作类型
    /// </summary>
    [Flags]
    public enum LogsAction : short
    {
        None = 0,
        /// <summary>
        /// 登录
        /// </summary>
        Login = 1,
        /// <summary>
        /// 添加
        /// </summary>
        Create = 2,
        /// <summary>
        /// 修改
        /// </summary>
        Edit = 4,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 8,
        /// <summary>
        /// 清空
        /// </summary>
        Empty = 16,
        /// <summary>
        /// 备份
        /// </summary>
        BackUp = 32,
        /// <summary>
        /// 下载
        /// </summary>
        Download = 64,
        /// <summary>
        /// 还原
        /// </summary>
        Revert = 128,
        /// <summary>
        /// 确认
        /// </summary>
        Confirm = 256,
        /// <summary>
        /// 取消
        /// </summary>
        Cancel = 512,
        /// <summary>
        /// 作废
        /// </summary>
        Invalid = 1024,
        /// <summary>
        /// 回复
        /// </summary>
        Reply = 2048
    }
}
