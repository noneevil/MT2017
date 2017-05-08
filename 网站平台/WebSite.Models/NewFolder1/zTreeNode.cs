using System;
using Newtonsoft.Json;

namespace WebSite.Models
{
    /// <summary>
    /// zTree菜单
    /// </summary>
    [Serializable]
    public class zTreeNode
    {
        /// <summary>
        /// 编号
        /// </summary>
        [JsonProperty("id")]
        public Int32 ID { get; set; }
        /// <summary>
        /// 父节点编号
        /// </summary>
        [JsonProperty("pId")]
        public Int32 ParentID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("name")]
        public String GroupName { get; set; }
    }
}
