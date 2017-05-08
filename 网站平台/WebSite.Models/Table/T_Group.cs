using System;
using MSSQLDB;

namespace WebSite.Models
{
    /// <summary>
    /// 无限级分类表
    /// </summary>
    [Table("T_Group")]
    [Serializable]
    public class T_GroupEntity
    {
        /// <summary>
        /// 分类ID
        /// </summary>		
        [Field("ID", IsPrimaryKey = true)]
        public int ID { get; set; }
        /// <summary>
        /// 父节点ID
        /// </summary>		
        [Field("ParentID")]
        public int ParentID { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>		
        [Field("GroupName")]
        public String GroupName { get; set; }
        /// <summary>
        /// 关联表名
        /// </summary>		
        [Field("TableName")]
        public String TableName { get; set; }
        /// <summary>
        /// 模板类型
        /// </summary>
        [Field("Template")]
        public SiteTemplate Template { get; set; }
        /// <summary>
        /// 使用字段
        /// </summary>
        [Field("Field")]
        public String Field { get; set; }
        /// <summary>
        /// 拥有权限
        /// </summary>
        [Field("ActionType")]
        public ActionType ActionType { get; set; }
        /// <summary>
        /// 类别深度
        /// </summary>
        [Field("Layer")]
        public Int32 Layer { get; set; }
        /// <summary>
        /// 类别ID列表(逗号分隔开)
        /// </summary>
        [Field("List")]
        public String List { get; set; }
        /// <summary>
        /// 排序数字
        /// </summary>
        [Field("SortID")]
        public Int32 SortID { get; set; }
        /// <summary>
        /// URL跳转地址
        /// </summary>
        [Field("Link_Url")]
        public String Link_Url { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        [Field("Image_Url")]
        public String Image_Url { get; set; }
        /// <summary>
        /// 备注说明
        /// </summary>
        [Field("Content")]
        public String Content { get; set; }
        /// <summary>
        /// SEO标题
        /// </summary>
        [Field("Seo_Title")]
        public String Seo_Title { get; set; }
        /// <summary>
        /// SEO关健字
        /// </summary>
        [Field("Seo_Keyword")]
        public String Seo_Keyword { get; set; }
        /// <summary>
        /// SEO描述
        /// </summary>
        [Field("Seo_Description")]
        public String Seo_Description { get; set; }


        /// <summary>
        /// 父节分类名称
        /// </summary>
        [Field(IsIgnore = true)]
        public String ParentName { get; set; }
    }
}