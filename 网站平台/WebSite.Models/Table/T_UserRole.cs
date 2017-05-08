using System;
using MSSQLDB;

namespace WebSite.Models
{
    /// <summary>
    /// 用户角色表
    /// </summary>
    [Table("T_UserRole")]
    [Serializable]
    public class T_UserRoleEntity
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [Field("ID", IsPrimaryKey = true)]
        public int ID { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [Field("Name")]
        public String Name { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        [Field("Remark")]
        public String Remark { get; set; }
        /// <summary>
        /// 是否系统默认(1-是 0-否)
        /// </summary>
        [Field("IsSystem")]
        public Boolean IsSystem { get; set; }
    }
}
