using System;

namespace WebSite.Core.Table
{
    /// <summary>
    /// 字段数据源类型
    /// </summary>
    [Serializable]
    public enum FieldDataSourceType
    {
        /// <summary>
        /// SQL语句
        /// </summary>
        SQL = 0,
        /// <summary>
        /// 列表
        /// </summary>
        ListItem = 1
        ///// <summary>
        ///// 字典
        ///// </summary>
        //Dictionary = 2,
        ///// <summary>
        ///// 数组
        ///// </summary>
        //Array = 3,
        ///// <summary>
        ///// 枚举
        ///// </summary>
        //Enum = 4,
        ///// <summary>
        ///// 未知
        ///// </summary>
        //Object = 5
    }
}
