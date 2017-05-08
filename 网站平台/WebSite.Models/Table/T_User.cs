using System;
using System.Collections.Generic;
using MSSQLDB;

namespace WebSite.Models
{
    /// <summary>
    /// 管理员表
    /// </summary>
    [Table("T_User")]
    [Serializable]
    public class T_UserEntity
    {
        /// <summary>
        /// 自增ID编号
        /// </summary>
        [Field("ID", IsPrimaryKey = true)]
        public Int32 ID { get; set; }
        /// <summary>
        /// 登录名称
        /// </summary>
        [Field("UserName")]
        public String UserName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [Field("UserPass")]
        public String UserPass { get; set; }
        /// <summary>
        /// 密码校验码
        /// </summary>
        [Field("Salt")]
        public String Salt { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        [Field("Nickname")]
        public String Nickname { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [Field("TelePhone")]
        public String TelePhone { get; set; }
        /// <summary>
        /// 角色编号
        /// </summary>
        [Field("RoleID")]
        public Int32 RoleID { get; set; }
        /// <summary>
        /// 通信邮箱
        /// </summary>
        [Field("Email")]
        public String Email { get; set; }
        /// <summary>
        /// 是否超管(1-超管 0-系管)
        /// </summary>
        [Field("IsSuper")]
        public Boolean IsSuper { get; set; }
        /// <summary>
        /// 是否锁定(0 - 默认 1 - 锁定)
        /// </summary>
        [Field("IsLock")]
        public Boolean IsLock { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [Field("JoinDate")]
        public DateTime JoinDate { get; set; }
        /// <summary>
        /// 最后登录日期
        /// </summary>
        [Field("LastSignTime")]
        public DateTime LastSignTime { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public T_UserRoleEntity UserRole { get; set; }
        /// <summary>
        /// 权限控制
        /// </summary>
        //public List<T_AccessControlEntity> AccessControl { get; set; }
    }
}
