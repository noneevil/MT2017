using System;

namespace CommonUtils.ExtendedAttributes
{
    /// <summary>
    /// 字段属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class FieldAttribute : Attribute
    {
        public FieldAttribute()
        {
            this.IsKey = false;
            this.IsSeed = false;
            this.isDefaultValue = false;
            this.isAllowEdit = true;
        }

        public FieldAttribute(String fieldName)
        {
            this.IsKey = false;
            this.IsSeed = false;
            this.isDefaultValue = false;
            this.isAllowEdit = true;
            this.FieldName = fieldName;
        }

        public FieldAttribute(String fieldName, Boolean isKey)
        {
            this.IsKey = false;
            this.IsSeed = false;
            this.isDefaultValue = false;
            this.isAllowEdit = true;
            this.FieldName = fieldName;
            this.IsKey = isKey;
        }

        public FieldAttribute(String fieldName, Boolean isKey, Boolean isSeed)
        {
            this.IsKey = false;
            this.IsSeed = false;
            this.isDefaultValue = false;
            this.isAllowEdit = true;
            this.FieldName = fieldName;
            this.IsKey = isKey;
            this.IsSeed = isSeed;
        }

        public FieldAttribute(String fieldName, Boolean isKey, String outerFieldName)
        {
            this.IsKey = false;
            this.IsSeed = false;
            this.isDefaultValue = false;
            this.isAllowEdit = true;
            this.FieldName = fieldName;
            this.IsKey = isKey;
            this.OuterFieldName = outerFieldName;
        }

        public FieldAttribute(String fieldName, Boolean isKey, String outerFieldName, Boolean isSeed)
        {
            this.IsKey = false;
            this.IsSeed = false;
            this.isDefaultValue = false;
            this.isAllowEdit = true;
            this.FieldName = fieldName;
            this.IsKey = isKey;
            this.OuterFieldName = outerFieldName;
            this.IsSeed = isSeed;
        }

        public FieldAttribute(String fieldName, Boolean isKey, String outerFieldName, Boolean isSeed, Boolean isDefaultValue)
        {
            this.IsKey = false;
            this.IsSeed = false;
            this.isAllowEdit = true;
            this.FieldName = fieldName;
            this.IsKey = isKey;
            this.OuterFieldName = outerFieldName;
            this.IsSeed = isSeed;
            this.isDefaultValue = isDefaultValue;
        }

        public FieldAttribute(String fieldName, Boolean isKey, String outerFieldName, Boolean isSeed, Boolean isDefaultValue, Boolean isAllowEdit)
        {
            this.IsKey = false;
            this.IsSeed = false;
            this.isAllowEdit = isAllowEdit;
            this.FieldName = fieldName;
            this.IsKey = isKey;
            this.OuterFieldName = outerFieldName;
            this.IsSeed = isSeed;
            this.isDefaultValue = isDefaultValue;
        }
        /// <summary>
        /// 字段名称
        /// </summary>
        public String FieldName { get; set; }
        /// <summary>
        /// 是否允许编辑
        /// </summary>
        public Boolean isAllowEdit { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public Boolean isDefaultValue { get; set; }
        /// <summary>
        /// 是否关键字段
        /// </summary>
        public Boolean IsKey { get; set; }
        /// <summary>
        /// 种子
        /// </summary>
        public Boolean IsSeed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String OuterFieldName { get; set; }
    }
}
