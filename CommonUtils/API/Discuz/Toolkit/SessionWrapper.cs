using System.Xml.Serialization;

namespace CommonUtils.Discuz.Toolkit
{
    public class SessionWrapper
    {
        [XmlIgnore()]
        internal DiscuzSession Session;
    }
}
