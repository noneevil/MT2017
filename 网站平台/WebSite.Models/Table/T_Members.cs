using System;
using MSSQLDB;

namespace WebSite.Models
{
    /// <summary>
    /// 会员
    /// </summary>
    [Table("T_Members")]
    [Serializable]
    public class T_MembersEntity
    {
        /// <summary>
        /// 分类ID
        /// </summary>		
        [Field("ID", IsPrimaryKey = true)]
        public Int32 ID { get; set; }
        /// <summary>
        /// 会员名称
        /// </summary>
        [Field("MemberName")]
        public String MemberName { get; set; }
        /// <summary>
        /// 会员类型
        /// </summary>
        [Field("MemberType")]
        public MemberType MemberType { get; set; }
        /// <summary>
        /// 会员等级
        /// </summary>
        [Field("MemberLevel")]
        public Int32 MemberLevel { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        [Field("UserName")]
        public String UserName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        [Field("Picture")]
        public String Picture { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [Field("PassWord")]
        public String PassWord { get; set; }
        /// <summary>
        /// 性别(0-女 1-男)
        /// </summary>
        [Field("Sex")]
        public Boolean Sex { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        [Field("Birthday")]
        public DateTime Birthday { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        [Field("IDCard")]
        public String IDCard { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [Field("Phone")]
        public String Phone { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        [Field("Mobile")]
        public String Mobile { get; set; }
        /// <summary>
        /// 联系邮箱
        /// </summary>
        [Field("Email")]
        public String Email { get; set; }
        /// <summary>
        /// 联系QQ
        /// </summary>
        [Field("QQ")]
        public String QQ { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        [Field("Address")]
        public String Address { get; set; }
        /// <summary>
        /// 启用状态(1 启用 0 禁用)
        /// </summary>
        [Field("Status")]
        public Boolean Status { get; set; }
        /// <summary>
        /// 注册日期
        /// </summary>
        [Field("JoinDate")]
        public DateTime JoinDate { get; set; }
        /// <summary>
        /// 最后登录日期
        /// </summary>
        [Field("LastSignTime")]
        public DateTime LastSignTime { get; set; }
        /// <summary>
        /// 是否通过邮箱验证
        /// </summary>
        [Field("ValidateMail")]
        public Boolean ValidateMail { get; set; }
        /// <summary>
        /// 是否通过手机验证
        /// </summary>
        [Field("ValidateMobile")]
        public Boolean ValidateMobile { get; set; }
    }
}