using System;

namespace WebSite.Interface
{
    /// <summary>
    /// 控件数据接口
    /// </summary>
    public interface IControls
    {
        /// <summary>
        /// 关联字段名称
        /// </summary>
        //String LinkProperty { get; set; }
        /// <summary>
        /// 关联数据表名
        /// </summary>
        //String LinkTableName { get; set; }
        /// <summary>
        /// 是否通过服务器验证默认为true
        /// </summary>
        //Boolean IsValid { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        //DbType DataType { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        //TypeCode TypeCode { get; set; }
        /// <summary>
        /// 只读标记
        /// </summary>
        //Boolean IsReadOnly { get; set; }
        /// <summary>
        /// 是否客户端验证
        /// </summary>
        //Boolean IsClientValidation { get; set; }
        /// <summary>
        /// 是否允许空值
        /// </summary>
        //Boolean IsNull { get; set; }
        /// <summary>
        /// 是否是主键
        /// </summary>
        //Boolean IsPrimaryKey { get; set; }
        /// <summary>
        /// 客户端验证脚本
        /// </summary>
        //String ClientValidationFunctionString { get; set; }
        /// <summary>
        /// 服务端验证
        /// </summary>
        /// <returns></returns>
        //Boolean Validate();
        /// <summary>
        /// 数据显示格式
        /// </summary>
        String FormatString { get; set; }
        /// <summary>
        /// 设置控件值
        /// </summary>
        /// <param name="value"></param>
        void SetValue(Object value);
        /// <summary>
        /// 获取控件值
        /// </summary>
        /// <returns></returns>
        Object GetValue();
    }

    //public interface IDataTextBox : IDataControl
    //{
    //    //String Text { get; set; }
    //    //String DataFormatString { get; set; }
    //    //int MaxLength { get; set; }
    //}

    //public interface IDataCheckBox : IDataControl
    //{
    //    string Value { get; set; }
    //    bool Checked { get; set; }
    //    string Text { get; set; }
    //    event EventHandler CheckedChanged;
    //}
}