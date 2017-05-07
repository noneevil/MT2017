using System;
using System.Xml.Serialization;

namespace CommonUtils.Discuz.Toolkit
{
    public class Arg
    {
        public Arg()
        { }
        public Arg(String key, String value)
        {
            this.Key = key;
            this.Value = value;
        }
        [XmlElement("key")]
        public String Key;

        [XmlElement("value")]
        public String Value;
    }
}
