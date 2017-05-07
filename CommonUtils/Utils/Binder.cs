  using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace CommonUtils
{
    /// <summary>
    /// 数据绑定
    /// </summary>
    public abstract class Binder
    {
        private static readonly Char[] expressionPartSeparator = new Char[] { '.' };
        private static readonly Char[] indexExprEndChars = new Char[] { ']', ')' };
        private static readonly Char[] indexExprStartChars = new Char[] { '[', '(' };

        /// <summary>
        /// 在运行时计算数据绑定表达式。
        /// </summary>
        /// <param name="container"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Object Eval(Object container, String expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            expression = expression.Trim();
            if (expression.Length == 0)
            {
                throw new ArgumentNullException("expression");
            }
            if (container == null)
            {
                return null;
            }
            String[] expressionParts = expression.Split(expressionPartSeparator);
            return Eval(container, expressionParts);
        }

        /// <summary>
        /// 在运行时计算数据绑定表达式，并将结果格式化为要在请求浏览器中显示的文本。
        /// </summary>
        /// <param name="container"></param>
        /// <param name="expression"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static String Eval(Object container, String expression, String format)
        {
            Object obj2 = Eval(container, expression);
            if ((obj2 == null) || (obj2 == DBNull.Value))
            {
                return String.Empty;
            }
            if (String.IsNullOrEmpty(format))
            {
                return obj2.ToString();
            }
            return String.Format(format, obj2);
        }

        /// <summary>
        /// 取字符串长度
        /// </summary>
        /// <param name="container"></param>
        /// <param name="expression"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static String Eval(Object container, String expression, int length)
        {
            return Eval(container, expression, length, String.Empty);
        }

        /// <summary>
        /// 取字符串长度结尾加符号
        /// </summary>
        /// <param name="container"></param>
        /// <param name="expression"></param>
        /// <param name="length"></param>
        /// <param name="ellipsis"></param>
        /// <returns></returns>
        public static String Eval(Object container, String expression, int length, String ellipsis)
        {
            Object obj2 = Eval(container, expression);
            if ((obj2 == null) || (obj2 == DBNull.Value)) return String.Empty;
            String _str = obj2.ToString();
            if (_str.Length > length) return _str.Substring(0, length) + ellipsis;
            return _str;
        }

        /// <summary>
        /// 清除HTML标记
        /// </summary>
        /// <param name="container"></param>
        /// <param name="expression"></param>
        /// <param name="clear"></param>
        /// <returns></returns>
        public static String Eval(Object container, String expression, Boolean clear)
        {
            Object obj2 = Eval(container, expression);
            if ((obj2 == null) || (obj2 == DBNull.Value)) return String.Empty;
            String _str = Regex.Replace(obj2.ToString(), "(<[^>]*>)", String.Empty);
            return _str;
        }

        /// <summary>
        /// 取字符串长度并清除HTML标记
        /// </summary>
        /// <param name="container"></param>
        /// <param name="expression"></param>
        /// <param name="clear"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static String Eval(Object container, String expression, Boolean clear, int length)
        {
            return Eval(container, expression, clear, length, String.Empty);
        }

        /// <summary>
        /// 取字符串长度结尾加符号并清除HTML标记
        /// </summary>
        /// <param name="container"></param>
        /// <param name="expression"></param>
        /// <param name="clear"></param>
        /// <param name="length"></param>
        /// <param name="ellipsis"></param>
        /// <returns></returns>
        public static String Eval(Object container, String expression, Boolean clear, int length, String ellipsis)
        {
            String _str = Eval(container, expression, clear);
            if (_str.Length > length) return _str.Substring(0, length) + ellipsis;
            return _str;
        }

        private static Object Eval(Object container, String[] expressionParts)
        {
            Object propertyValue = container;
            for (int i = 0; (i < expressionParts.Length) && (propertyValue != null); i++)
            {
                String propName = expressionParts[i];
                if (propName.IndexOfAny(indexExprStartChars) < 0)
                {
                    propertyValue = GetPropertyValue(propertyValue, propName);
                }
                else
                {
                    propertyValue = GetIndexedPropertyValue(propertyValue, propName);
                }
            }
            return propertyValue;
        }

        public static Object GetIndexedPropertyValue(Object container, String expr)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            if (String.IsNullOrEmpty(expr))
            {
                throw new ArgumentNullException("expr");
            }
            Object obj2 = null;
            Boolean flag = false;
            int length = expr.IndexOfAny(indexExprStartChars);
            int num2 = expr.IndexOfAny(indexExprEndChars, length + 1);
            if (((length < 0) || (num2 < 0)) || (num2 == (length + 1)))
            {
                throw new ArgumentException("DataBinder_Invalid_Indexed_Expr");
                //throw new ArgumentException(System.Web.SR.GetString("DataBinder_Invalid_Indexed_Expr", new Object[] { expr }));
            }
            String propName = null;
            Object obj3 = null;
            String s = expr.Substring(length + 1, (num2 - length) - 1).Trim();
            if (length != 0)
            {
                propName = expr.Substring(0, length);
            }
            if (s.Length != 0)
            {
                if (((s[0] == '"') && (s[s.Length - 1] == '"')) || ((s[0] == '\'') && (s[s.Length - 1] == '\'')))
                {
                    obj3 = s.Substring(1, s.Length - 2);
                }
                else if (Char.IsDigit(s[0]))
                {
                    int num3;
                    flag = int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out num3);
                    if (flag)
                    {
                        obj3 = num3;
                    }
                    else
                    {
                        obj3 = s;
                    }
                }
                else
                {
                    obj3 = s;
                }
            }
            if (obj3 == null)
            {
                throw new ArgumentException("DataBinder_Invalid_Indexed_Expr");
                //throw new ArgumentException(System.Web.SR.GetString("DataBinder_Invalid_Indexed_Expr", new Object[] { expr }));
            }
            Object propertyValue = null;
            if ((propName != null) && (propName.Length != 0))
            {
                propertyValue = GetPropertyValue(container, propName);
            }
            else
            {
                propertyValue = container;
            }
            if (propertyValue == null)
            {
                return obj2;
            }
            Array array = propertyValue as Array;
            if ((array != null) && flag)
            {
                return array.GetValue((int)obj3);
            }
            if ((propertyValue is IList) && flag)
            {
                return ((IList)propertyValue)[(int)obj3];
            }
            PropertyInfo info = propertyValue.GetType().GetProperty("Item", BindingFlags.Public | BindingFlags.Instance, null, null, new Type[] { obj3.GetType() }, null);
            if (info == null)
            {
                throw new ArgumentException("DataBinder_No_Indexed_Accessor");
                //throw new ArgumentException(System.Web.SR.GetString("DataBinder_No_Indexed_Accessor", new Object[] { propertyValue.GetType().FullName }));
            }
            return info.GetValue(propertyValue, new Object[] { obj3 });
        }
        public static Object GetPropertyValue(Object container, String propName)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            if (String.IsNullOrEmpty(propName))
            {
                throw new ArgumentNullException("propName");
            }
            PropertyDescriptor descriptor = TypeDescriptor.GetProperties(container).Find(propName, true);
            if (descriptor == null)
            {
                descriptor = TypeDescriptor.GetProperties(container).Find("settingsxml", true);
                if (descriptor != null)
                {
                    //propName = propName.ToLower();
                    //List<CustomFieldValue> list = CustomFieldsManage.GetFieldsData(descriptor.GetValue(container));
                    //foreach (CustomFieldValue item in list)
                    //{
                    //    if (item.FieldName == null) continue;
                    //    if (item.FieldName.ToLower().Equals(propName)) return item.FieldValue;
                    //}
                    String val = new HttpRequest(String.Empty, "http://localhost", descriptor.GetValue(container).ToString())[propName];
                    if (val == null) return String.Format("{0}!", propName);
                    val = val.Replace("_equals_", "=").Replace("_and_", "&").Replace("_question_", "?").Replace("_quote_", "'");
                    //val = Regex.Replace(val, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
                    return val;
                }
                return String.Format("{0}!", propName);

                //throw new HttpException(System.Web.SR.GetString("DataBinder_Prop_Not_Found", new Object[] { container.GetType().FullName, propName }));
            }
            return descriptor.GetValue(container);
        }

        public static Object GetDataItem(Object container)
        {
            Boolean flag;
            return GetDataItem(container, out flag);
        }
        public static Object GetDataItem(Object container, out Boolean foundDataItem)
        {
            if (container == null)
            {
                foundDataItem = false;
                return null;
            }
            IDataItemContainer container2 = container as IDataItemContainer;
            if (container2 != null)
            {
                foundDataItem = true;
                return container2.DataItem;
            }
            String name = "DataItem";
            PropertyInfo property = container.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property == null)
            {
                foundDataItem = false;
                return null;
            }
            foundDataItem = true;
            return property.GetValue(container, null);
        }
        public static String GetIndexedPropertyValue(Object container, String propName, String format)
        {
            Object indexedPropertyValue = GetIndexedPropertyValue(container, propName);
            if ((indexedPropertyValue == null) || (indexedPropertyValue == DBNull.Value))
            {
                return String.Empty;
            }
            if (String.IsNullOrEmpty(format))
            {
                return indexedPropertyValue.ToString();
            }
            return String.Format(format, indexedPropertyValue);
        }
        public static String GetPropertyValue(Object container, String propName, String format)
        {
            Object propertyValue = GetPropertyValue(container, propName);
            if ((propertyValue == null) || (propertyValue == DBNull.Value))
            {
                return String.Empty;
            }
            if (String.IsNullOrEmpty(format))
            {
                return propertyValue.ToString();
            }
            return String.Format(format, propertyValue);
        }
        internal static Boolean IsNull(Object value)
        {
            if ((value != null) && !Convert.IsDBNull(value))
            {
                return false;
            }
            return true;
        }
    }
}
