using System;
using MSSQLDB;

namespace WebSite.Models
{
    /// <summary>
    /// 广告
    /// </summary>
    [Table("T_Ad")]
    [Serializable]
    public class T_AdEntity
    {
        /// <summary>
        /// 广告编号
        /// </summary>
        [Field("ID", IsPrimaryKey = true)]
        public int ID { get; set; }
        /// <summary>
        /// 广告分类
        /// </summary>
        [Field("GroupID")]
        public AdCategory GroupID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Field("Title")]
        public String Title { get; set; }
        /// <summary>
        /// 资源地址
        /// </summary>
        [Field("LinkSrc")]
        public String LinkSrc { get; set; }
        /// <summary>
        /// 广告宽度
        /// </summary>
        [Field("Width")]
        public int Width { get; set; }
        /// <summary>
        /// 广告高度
        /// </summary>
        [Field("Height")]
        public int Height { get; set; }
        /// <summary>
        /// 打开方式
        /// </summary>
        [Field("Target")]
        public TargetCategory Target { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        [Field("LinkUrl")]
        public String LinkUrl { get; set; }
        /// <summary>
        /// 点击次数
        /// </summary>
        [Field("Hits")]
        public int Hits { get; set; }
        /// <summary>
        /// 展示次数
        /// </summary>
        [Field("ShowNum")]
        public int ShowNum { get; set; }
    }
}