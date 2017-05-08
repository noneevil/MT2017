using System;
using System.Collections.Generic;
using MSSQLDB;
using Newtonsoft.Json;
using WebSite.Interface;

namespace WebSite.Models
{
    /// <summary>
    /// 新闻表
    /// </summary>
    [Table("T_News")]
    [Serializable]
    public class T_NewsEntity
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        [Field("ID", IsPrimaryKey = true)]
        //[JsonProperty(PropertyName = "ID")]
        public long ID { get; set; }
        /// <summary>
        /// 标题颜色
        /// </summary>
        [Field("Color")]
        public String Color { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        [Field("GroupId")]
        public Int32 GroupId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Field("Title")]
        public String Title { get; set; }
        /// <summary>
        /// 内容摘要
        /// </summary>
        [Field("Abstract")]
        public String Abstract { get; set; }
        /// <summary>
        /// 详细内容
        /// </summary>
        [Field("Content")]
        public String Content { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        [Field("Author")]
        public String Author { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        [Field("Source")]
        public String Source { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Field("PubDate")]
        public DateTime PubDate { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        [Field("EditDate")]
        [JsonIgnore]
        public DateTime EditDate { get; set; }
        /// <summary>
        /// 是否推荐(1-是 0-否)
        /// </summary>
        [Field("IsNominate")]
        public Boolean IsNominate { get; set; }
        /// <summary>
        /// 是否热门(1-是 0-否)
        /// </summary>
        [Field("IsHotspot")]
        public Boolean IsHotspot { get; set; }
        /// <summary>
        /// 是否焦点(1-是 0-否)
        /// </summary>
        [Field("IsSlide")]
        public Boolean IsSlide { get; set; }
        /// <summary>
        /// 是否置顶(1-是 0-否)
        /// </summary>
        [Field("IsStick")]
        public Boolean IsStick { get; set; }
        /// <summary>
        /// 是否启用(1-是 0-否)
        /// </summary>
        [Field("IsEnable")]
        public Boolean IsEnable { get; set; }
        /// <summary>
        /// 是否通过审核(1-是 0-否)
        /// </summary>
        [Field("IsAudit")]
        public Boolean IsAudit { get; set; }
        /// <summary>
        /// 是否充许评论(1-是 0-否)
        /// </summary>
        [Field("IsComments")]
        public Boolean IsComments { get; set; }
        /// <summary>
        /// 浏览次数
        /// </summary>
        [Field("Click")]
        public Int32 Click { get; set; }
        /// <summary>
        /// 外部链接
        /// </summary>
        [Field("Link_Url")]
        public String Link_Url { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        [Field("Image_Url")]
        public String Image_Url { get; set; }
        /// <summary>
        /// 附件地址
        /// </summary>
        [Field("Attac_Url")]
        public String Attac_Url { get; set; }
        /// <summary>
        /// 视频地址
        /// </summary>
        [Field("Video_Url")]
        public String Video_Url { get; set; }
        /// <summary>
        /// SEO标题
        /// </summary>
        [Field("Seo_Title")]
        public String Seo_Title { get; set; }
        /// <summary>
        /// SEO关健字
        /// </summary>
        [Field("Seo_Keywords")]
        public String Seo_Keywords { get; set; }
        /// <summary>
        /// SEO描述
        /// </summary>
        [Field("Seo_Description")]
        public String Seo_Description { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Field("SortID")]
        public Int32 SortID { get; set; }
        /// <summary>
        /// 阅读权限
        /// </summary>
        [Field("ReadAccess")]
        public MemberType ReadAccess { get; set; }
        /// <summary>
        /// 关联投票ID
        /// </summary>
        [Field("VoteID")]
        public Int32 VoteID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Field("UserName")]
        public String UserName { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        [Field("SettingsXML")]
        public String SettingsXML
        {
            get
            {
                if (VirtualFields == null) VirtualFields = new List<IKeyValue>();
                return JsonConvert.SerializeObject(VirtualFields);
            }
            set
            {
                VirtualFields = JsonConvert.DeserializeObject<List<IKeyValue>>(value);
            }
        }

        /// <summary>
        /// 虚拟字段集合
        /// </summary>
        [JsonIgnore]
        [Field(IsIgnore = true)]
        public List<IKeyValue> VirtualFields { get; set; }
        /// <summary>
        /// 父节分类ID
        /// </summary>
        [Field(IsIgnore = true)]
        public int ParentID { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        [Field(IsIgnore = true)]
        public String GroupName { get; set; }
    }
}
