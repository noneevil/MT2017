using System.Xml.Serialization;
using Newtonsoft.Json;

namespace CommonUtils.Discuz.Toolkit
{
    public class Notification
    {
        [JsonPropertyAttribute("unread")]
        [XmlElement("unread", IsNullable = false)]
        public int Unread;

        [JsonPropertyAttribute("most_recent")]
        [XmlElement("most_recent", IsNullable = false)]
        public int MostRecent;
    }
}
