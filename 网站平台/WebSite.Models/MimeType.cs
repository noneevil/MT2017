using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace WebSite.Models
{
    /// <summary>
    /// 文件编码类型
    /// </summary>
    //[XmlType("item")]
    [Serializable]
    public class MimeType
    {
        [XmlAttribute("name")]
        public String Name { get; set; }

        [XmlAttribute("value")]
        public String Value { get; set; }
        
        //[XmlAttribute("head")]
        public List<Byte> Head { get; set; }
    }
}
