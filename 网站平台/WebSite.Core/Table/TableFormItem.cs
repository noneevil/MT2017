using System;
using System.Xml;
using System.Xml.Serialization;

namespace WebSite.Core.Table
{
    /// <summary>
    /// 表单
    /// </summary>
    [Serializable]
    public class TableFormItem
    {
        /// <summary>
        /// 表单编号
        /// </summary>
        [XmlAttribute("ID")]
        public Guid ID { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        [XmlAttribute("Preferred")]
        public Boolean Preferred { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [XmlAttribute("Description")]
        public String Description { get; set; }
        /// <summary>
        /// 表单内容
        /// </summary>
        [XmlIgnore]
        public String Content { get; set; }
        [XmlText]
        public XmlNode[] CDataContent
        {
            get
            {
                var dummy = new XmlDocument();
                return new XmlNode[] { dummy.CreateCDataSection(Content) };
            }
            set
            {
                if (value == null)
                {
                    Content = null;
                    return;
                }

                if (value.Length != 1)
                {
                    throw new InvalidOperationException(
                        String.Format(
                            "Invalid array length {0}", value.Length));
                }

                Content = value[0].Value;
            }
        }
    }
}
