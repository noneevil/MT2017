using System;
using MSSQLDB;

namespace WebSite.Models
{
    /// <summary>
    /// 权限访问控制
    /// </summary>
    [Serializable]
    [Table("T_AccessControl")]
    public class T_AccessControlEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Field("ID", IsPrimaryKey = true)]
        public Int32 ID { get; set; }
        /// <summary>
        /// 关联表名
        /// </summary>
        [Field("TableName")]
        public String TableName { get; set; }
        /// <summary>
        /// 节点
        /// </summary>
        [Field("Node")]
        public Int32 Node { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        [Field("ActionType")]
        public ActionType ActionType { get; set; }
        /// <summary>
        /// 关联角色编号
        /// </summary>
        [Field("Role")]
        public Int32 Role { get; set; }
        /// <summary>
        /// 连接地址
        /// </summary>
        [Field("Link_Url")]
        public String Link_Url { get; set; }
    }
}