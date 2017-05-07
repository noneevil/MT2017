using System;
using System.Xml.Serialization;

namespace CommonUtils.Discuz.Toolkit
{
    /// <summary>
    /// 区域信息类
    /// </summary>
    public class Location
    {
        [XmlElement("street")]
        public String Street;

        [XmlElement("city")]
        public String City;

        [XmlElement("state")]
        public String State;

        [XmlElement("country")]
        public String Country;

        [XmlElement("zip")]
        public String Zip;
    }
}
