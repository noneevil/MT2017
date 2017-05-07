using System;

namespace MSSQLDB
{
    /// <summary>
    /// SQL命令类型
    /// </summary>
    [Flags]
    public enum CmdType
    {
        /// <summary>
        /// 从数据库表中检索数据行和列
        /// </summary>
        SELECT,
        /// <summary>
        /// 向数据库表添加新数据行
        /// </summary>
        INSERT,
        /// <summary>
        /// 更新数据库表中的数据
        /// </summary>
        UPDATE,
        /// <summary>
        /// 从数据库表中删除数据行
        /// </summary>
        DELETE
    }
}
