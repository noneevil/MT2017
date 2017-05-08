using System;
using System.Xml.Serialization;

namespace WebSite.Core.Table
{
    /// <summary>
    /// 列表项数据源
    /// </summary>
    [Serializable]
    [XmlType("item")]
    public class ListItemDataSource
    {
        /// <summary>
        /// 是否选中
        /// </summary>
        [XmlAttribute("Selected")]
        public Boolean Selected { get; set; }
        /// <summary>
        /// 文本
        /// </summary>
        [XmlAttribute("Text")]
        public String Text { get; set; }
        /// <summary>
        /// 数据值
        /// </summary>
        [XmlAttribute("Value")]
        public String Value { get; set; }
    }
}
