using System;
using MSSQLDB;

namespace WebSite.Models
{
    /// <summary>
    /// 评论表
    /// </summary>
    [Table("T_Comment")]
    [Serializable]
    public class T_CommentEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Field("ID", IsPrimaryKey = true)]
        public Int32 ID { get; set; }
        /// <summary>
        /// 关联编号
        /// </summary>
        [Field("LinkID")]
        public Int32 LinkID { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        [Field("UserName")]
        public String UserName { get; set; }
        /// <summary>
        /// 评论分类
        /// </summary>
        [Field("GroupID")]
        public Int32 GroupID { get; set; }
        /// <summary>
        /// 会员编号
        /// </summary>
        [Field("MemberID")]
        public Int32 MemberID { get; set; }
        /// <summary>
        /// 评论标题
        /// </summary>
        [Field("Title")]
        public String Title { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        [Field("Content")]
        public String Content { get; set; }
        /// <summary>
        /// 审核(0-未通过 1-通过)
        /// </summary>
        [Field("Audit")]
        public Boolean Audit { get; set; }
        /// <summary>
        /// 支持
        /// </summary>
        [Field("Plus")]
        public Int32 Plus { get; set; }
        /// <summary>
        /// 反对
        /// </summary>
        [Field("Minus")]
        public Int32 Minus { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        [Field("ClientIP")]
        public String ClientIP { get; set; }
        /// <summary>
        /// 评论日期
        /// </summary>
        [Field("JoinDate")]
        public DateTime JoinDate { get; set; }
        


        /// <summary>
        /// 会员名称
        /// </summary>
        [Field(IsIgnore = true)]
        public String MemberName { get; set; }
    }
}
