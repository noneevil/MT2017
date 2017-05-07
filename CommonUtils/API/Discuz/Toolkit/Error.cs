using System;
using System.Xml.Serialization;

namespace CommonUtils.Discuz.Toolkit
{
    [XmlRoot("error_response", Namespace = "http://nt.discuz.net/api/", IsNullable = false)]
    public class Error
    {
        [XmlElement("error_code")]
        public int ErrorCode;

        [XmlElement("error_msg")]
        public String ErrorMsg;

        [XmlElement("request_args", IsNullable = false)]
        public ArgResponse Args;

    }

}
