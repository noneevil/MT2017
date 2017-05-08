using System;
using System.Xml.Serialization;
using MSSQLDB;

namespace WebSite.Core.Table
{
    /// <summary>
    /// 字段
    /// </summary>
    [Serializable]
    public class Field
    {
        /// <summary>
        /// 编号
        /// </summary>
        [XmlAttribute("ID")]
        public Guid ID { get; set; }
        /// <summary>
        /// 列名称
        /// </summary>
        [XmlAttribute("FieldName")]
        public String FieldName { get; set; }
        /// <summary>
        /// 列标识号
        /// </summary>
        [XmlAttribute("Position")]
        public Int32 Position { get; set; }
        /// <summary>
        /// 是否为虚拟字段
        /// </summary>
        [XmlAttribute("isVirtual")]
        public Boolean isVirtual { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        [XmlAttribute("DataType")]
        [FieldAttribute(ControlName = "DataTypeValue")]
        public Int32 DataType { get; set; }
        /// <summary>
        /// 字段长度
        /// </summary>
        [XmlAttribute("Length")]
        public Int32 Length { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [XmlAttribute("Description")]
        public String Description { get; set; }
        /// <summary>
        /// 控件类型
        /// </summary>
        [XmlAttribute("Control")]
        public ControlType Control { get; set; }
        /// <summary>
        /// 是否允许 NULL
        /// </summary>
        [XmlAttribute("isNullable")]
        public Boolean isNullable { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        [XmlAttribute("DefaultValue")]
        public String DefaultValue { get; set; }
        /// <summary>
        /// 数据验证正则表达式
        /// </summary>
        [XmlAttribute("Regex")]
        public String Regex { get; set; }
        /// <summary>
        /// 数据源
        /// </summary>
        [XmlElement("BindData")]
        public FieldDataSource DataSource { get; set; }
    }
}