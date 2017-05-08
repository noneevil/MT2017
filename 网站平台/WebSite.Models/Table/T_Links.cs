using System;
using MSSQLDB;

namespace WebSite.Models
{
    /// <summary>
    /// 友情连接
    /// </summary>
    [Serializable]
    [Table("T_Links")]
    public class T_LinksEntity
    {
        /// <summary>
        /// 编号ID
        /// </summary>		
        [Field("ID", IsPrimaryKey = true)]
        public int ID { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>		
        [Field("GroupID")]
        public LinkCategory GroupID { get; set; }
        /// <summary>
        /// 连接名称
        /// </summary>		
        [Field("LinkName")]
        public String LinkName { get; set; }
        /// <summary>
        /// 连接图片
        /// </summary>		
        [Field("LinkImage")]
        public String LinkImage { get; set; }
        /// <summary>
        /// 连接地址
        /// </summary>		
        [Field("LinkUrl")]
        public String LinkUrl { get; set; }
    }
}