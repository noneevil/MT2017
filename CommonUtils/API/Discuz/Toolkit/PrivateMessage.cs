using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace CommonUtils.Discuz.Toolkit
{

    public class PrivateMessage
    {
        [JsonPropertyAttribute("message_id")]
        [XmlElement("message_id")]
        public int MsgID;

        [JsonPropertyAttribute("from")]
        [XmlElement("from")]
        public String FromUser;

        [JsonPropertyAttribute("from_id")]
        [XmlElement("from_id")]
        public String FormID;

        [JsonPropertyAttribute("subject")]
        [XmlElement("subject", IsNullable = false)]
        public String Subject;

        [JsonPropertyAttribute("post_date_time")]
        [XmlElement("post_date_time")]
        public String PostDateTime;

        [JsonPropertyAttribute("message")]
        [XmlElement("message")]
        public String Message;
    }
}