using System;
using MSSQLDB;

namespace WebSite.Models
{
    [Table("T_SiteMenu")]
    [Serializable]
    public partial class T_SiteMenuEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Field("ID", IsPrimaryKey = true)]
        public Int32 ID { get; set; }
        /// <summary>
        /// 父节点编号
        /// </summary>
        [Field("ParentID")]
        public Int32 ParentID { get; set; }
        /// <summary>
        /// 标题名称
        /// </summary>
        [Field("Title")]
        public String Title { get; set; }
        /// <summary>
        /// 副标题
        /// </summary>
        [Field("Sub_Title")]
        public String Sub_Title { get; set; }
        /// <summary>
        /// 连接地址
        /// </summary>
        [Field("Link_Url")]
        public String Link_Url { get; set; }
        /// <summary>
        /// 拥有权限列表选项
        /// </summary>
        [Field("ActionType")]
        public ActionType ActionType { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Field("SortID")]
        public Int32 SortID { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        [Field("Layer")]
        public Int32 Layer { get; set; }
        /// <summary>
        /// 类别ID列表(逗号分隔开)
        /// </summary>
        [Field("List")]
        public String List { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Field("Remark")]
        public String Remark { get; set; }
        /// <summary>
        /// 启用状态(1-启用 0-禁用)
        /// </summary>
        [Field("IsEnable")]
        public Boolean IsEnable { get; set; }
        /// <summary>
        /// 默认是否展开(0 - 否 1 - 是)(只对父节点有效)
        /// </summary>
        [Field("IsOpen")]
        public Boolean IsOpen { get; set; }
        /// <summary>
        /// 系统默认(0-否 1-是)
        /// </summary>
        [Field("IsSystem")]
        public Boolean IsSystem { get; set; }
    }
}
