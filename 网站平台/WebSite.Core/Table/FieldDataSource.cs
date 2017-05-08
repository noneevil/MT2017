using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace WebSite.Core.Table
{
    /// <summary>
    /// 字段数据源
    /// </summary>
    [Serializable]
    public class FieldDataSource
    {
        /// <summary>
        /// 数据源类型
        /// </summary>
        [XmlAttribute("DataSourceType")]
        public FieldDataSourceType DataSourceType { get; set; }
        [XmlElement("Layout")]
        public DataSourceLayout Layout { get; set; }

        [XmlElement("SQL")]
        public SQLDataSource SQLDataSource { get; set; }
        [XmlArray("ListItem")]
        public List<ListItemDataSource> ListItemDataSource { get; set; }

        //[XmlElement("dictionary")]
        //public SerializableDictionary<String, String> dict { get; set; }
        //[XmlArray("array")]
        //public Array array { get; set; }
        //[XmlEnum("enum")]
        //[XmlElement("enum")]
        //public Enum @enum { get; set; }
        //[XmlAnyElement("obj")]
        //public Object obj { get; set; }
    }
}
