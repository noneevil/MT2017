using System;

namespace CommonUtils.ExtendedAttributes
{
    /// <summary>
    /// 表结构信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TableAttribute : Attribute
    {
        public TableAttribute()
        {

        }

        public TableAttribute(String tableName)
        {
            TableName = tableName;
        }

        public TableAttribute(String tableName, String anotherName)
        {
            TableName = tableName;
            AnotherName = anotherName;
        }

        /// <summary>
        /// 对应数据库表名
        /// </summary>
        public String TableName { get; set; }
        /// <summary>
        /// 查询时的别名
        /// </summary>
        public String AnotherName { get; set; }
    }
}
