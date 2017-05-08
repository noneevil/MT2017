using System;
using MSSQLDB;

namespace WebSite.Models
{
    /// <summary>
    /// 管理员日志
    /// </summary>
    [Table("T_Logs")]
    [Serializable]
    public class T_LogsEntity
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Field("ID", IsPrimaryKey = true)]
        public long ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [Field("UserID")]
        public Int32 UserID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Field("UserName")]
        public String UserName { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        [Field("ActionType")]
        public LogsAction ActionType { get; set; }
        /// <summary>
        /// 操作内容
        /// </summary>
        [Field("Content")]
        public String Content { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        [Field("PubDate")]
        public DateTime PubDate { get; set; }
        /// <summary>
        /// 用户IP
        /// </summary>
        [Field("UserIP")]
        public String UserIP { get; set; }
    }
}