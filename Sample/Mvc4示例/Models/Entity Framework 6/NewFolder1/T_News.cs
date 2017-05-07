using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mvc4Example.Models
{
    /// <summary>
    /// 新闻内容
    /// </summary>
    [Serializable]
    [Table("T_News")]
    public class T_NewsEntities
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("ID")]
        public long ID { get; set; }
        /// <summary>
        /// 标题颜色
        /// </summary>
        [JsonProperty("color")]
        public string color { get; set; }
        /// <summary>
        /// 分类编号
        /// </summary>
        [JsonProperty("GroupId")]
        public long GroupId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("Title")]
        public string Title { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        [JsonProperty("subject")]
        public string subject { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        [JsonProperty("Abstract")]
        public string Abstract { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [JsonProperty("Content")]
        public string Content { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        [JsonProperty("Author")]
        public string Author { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        [JsonProperty("Source")]
        public string Source { get; set; }
        /// <summary>
        /// 发布日期
        /// </summary>
        [JsonProperty("PubDate")]
        public DateTime PubDate { get; set; }
        /// <summary>
        /// 编辑日期
        /// </summary>
        [JsonProperty("EditDate")]
        public DateTime EditDate { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        [JsonProperty("ImageUrl")]
        public string ImageUrl { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        [JsonProperty("AttacUrl")]
        public string AttacUrl { get; set; }
        /// <summary>
        /// 视频
        /// </summary>
        [JsonProperty("VideoUrl")]
        public string VideoUrl { get; set; }
        /// <summary>
        /// 网站地址
        /// </summary>
        [JsonProperty("Url")]
        public string Url { get; set; }
        /// <summary>
        /// 推荐
        /// </summary>
        [JsonProperty("nominate")]
        public bool nominate { get; set; }
        /// <summary>
        /// 热门
        /// </summary>
        [JsonProperty("hotspot")]
        public bool hotspot { get; set; }
        /// <summary>
        /// 焦点
        /// </summary>
        [JsonProperty("focus")]
        public bool focus { get; set; }
        /// <summary>
        /// 置顶
        /// </summary>
        [JsonProperty("Stick")]
        public bool Stick { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [JsonProperty("Status")]
        public bool Status { get; set; }
        /// <summary>
        /// 审核
        /// </summary>
        [JsonProperty("audit")]
        public bool audit { get; set; }
        /// <summary>
        /// 允许评论
        /// </summary>
        [JsonProperty("isComments")]
        public bool isComments { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        [JsonProperty("viewnum")]
        public long viewnum { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [JsonProperty("Sort")]
        public long Sort { get; set; }
        /// <summary>
        /// 封面
        /// </summary>
        [JsonProperty("Covers")]
        public string Covers { get; set; }
        /// <summary>
        /// Works
        /// </summary>
        [JsonProperty("Works")]
        public string Works { get; set; }
        /// <summary>
        /// Activity
        /// </summary>
        [JsonProperty("Activity")]
        public string Activity { get; set; }
        /// <summary>
        /// Albums
        /// </summary>
        [JsonProperty("Albums")]
        public string Albums { get; set; }
    }
}