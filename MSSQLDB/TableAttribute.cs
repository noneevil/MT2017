using System;
using System.Data;

namespace MSSQLDB
{
    /// <summary>
    /// 数据表标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class TableAttribute : System.Attribute
    {
        private String _tableName = String.Empty;

        public TableAttribute() { }
        public TableAttribute(String tablename)
        {
            this._tableName = tablename;
        }
        /// <summary>
        /// 数据表名
        /// </summary>
        public String TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }
    }
    /// <summary>
    /// 数据表字段标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FieldAttribute : System.Attribute
    {
        public FieldAttribute() { }
        public FieldAttribute(String name)
        {
            this.Name = name;
        }

        private String _name = String.Empty;
        private Boolean _isallowedit = true;
        private Type _dbtype = typeof(SqlDbType);
        private Boolean _isvirtual = false;
        private Boolean _isseed = false;
        private Boolean _isprimarykey = false;
        private Boolean _isignore = false;
        private String _controlname = String.Empty;
        /// <summary>
        /// 对应数据库表字段名称
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 是否为主键
        /// </summary>
        public Boolean IsPrimaryKey
        {
            get { return _isprimarykey; }
            set { _isprimarykey = value; }
        }
        /// <summary>
        /// 是否为自增量字段
        /// </summary>
        public Boolean IsSeed
        {
            get { return _isseed; }
            set { _isseed = value; }
        }
        /// <summary>
        /// 是否为虚拟字段
        /// </summary>
        public Boolean IsVirtual
        {
            get { return _isvirtual; }
            set { _isvirtual = value; }
        }
        /// <summary>
        /// 是否忽略
        /// </summary>
        public Boolean IsIgnore
        {
            get { return _isignore; }
            set { _isignore = value; }
        }
        /// <summary>
        /// 数据类型
        /// </summary>
        public Type DbType
        {
            get { return _dbtype; }
            set { _dbtype = value; }
        }
        /// <summary>
        /// 允许编辑
        /// </summary>
        public Boolean isAllowEdit
        {
            get { return _isallowedit; }
            set { _isallowedit = value; }
        }
        /// <summary>
        /// 对应表单中的控件名称
        /// </summary>
        public String ControlName
        {
            get { return _controlname; }
            set { _controlname = value; }
        }
    }
    /// <summary>
    /// UserControl Request接收属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequestFieldAttribute : System.Attribute
    {
        private String _name = String.Empty;
        private Object _defaultvalue = null;

        public RequestFieldAttribute() { }
        public RequestFieldAttribute(String name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Request接收名称
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 默认值
        /// </summary>
        public Object DefaultValue
        {
            get { return _defaultvalue; }
            set { _defaultvalue = value; }
        }
    }
}
