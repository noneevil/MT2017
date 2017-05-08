using System;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

namespace WebSite.Core.Table
{
    /// <summary>
    /// 布局设置 
    /// </summary>
    [Serializable]
    public class DataSourceLayout
    {
        /// <summary>
        /// 显示列数
        /// </summary>
        [XmlAttribute("Columns")]
        public Int32 RepeatColumns { get; set; }
        /// <summary>
        /// 显示方向
        /// </summary>
        [XmlAttribute("Direction")]
        public RepeatDirection RepeatDirection { get; set; }
        /// <summary>
        /// 布局模式
        /// </summary>
        [XmlAttribute("Layout")]
        public RepeatLayout RepeatLayout { get; set; }
    }
}
