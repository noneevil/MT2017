using System;
using System.Xml.Serialization;

namespace WebSite.Core.Table
{
    /// <summary>
    /// SQL数据源
    /// </summary>
    [Serializable]
    public class SQLDataSource
    {
        /// <summary>
        /// SQL语句
        /// </summary>
        [XmlAttribute("sql")]
        public String SQL { get; set; }
        /// <summary>
        /// 文本字段
        /// </summary>
        [XmlAttribute("Text")]
        public String TextField { get; set; }
        /// <summary>
        /// 值字段
        /// </summary>
        [XmlAttribute("Value")]
        public String ValueField { get; set; }
    }
}