using System;
using MSSQLDB;

namespace WebSite.Models
{
    /// <summary>
    /// 留言数据表
    /// </summary>
    [Table("T_Books")]
    [Serializable]
    public class T_BooksEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Field("ID", IsPrimaryKey = true)]
        public Int32 ID { get; set; }
        /// <summary>
        /// 留言标题
        /// </summary>
        [Field("Title")]
        public String Title { get; set; }
        /// <summary>
        /// 留言内容
        /// </summary>
        [Field("Content")]
        public String Content { get; set; }
        /// <summary>
        /// 留言时间
        /// </summary>
        [Field("BookTime")]
        public DateTime BookTime { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>
        [Field("ReContent")]
        public String ReContent { get; set; }
        /// <summary>
        /// 回复时间
        /// </summary>
        [Field("ReTime")]
        public DateTime ReTime { get; set; }
        /// <summary>
        /// 审核状态(1 - 通过 0 - 未通过)
        /// </summary>
        [Field("Status")]
        public Boolean Status { get; set; }
        /// <summary>
        /// 会员编号
        /// </summary>
        [Field("MemberID")]
        public Int32 MemberID { get; set; }
        /// <summary>
        /// 会员名称
        /// </summary>
        [Field(IsIgnore = true)]
        public String MemberName { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        [Field("UserName")]
        public String UserName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [Field("Telephone")]
        public String Telephone { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        [Field("IpAddress")]
        public String IpAddress { get; set; }
    }
}
