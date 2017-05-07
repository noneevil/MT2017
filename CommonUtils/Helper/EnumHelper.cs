using System;
using System.Data;
using System.Data.OleDb;

namespace CommonUtils
{
    /// <summary>
    /// 枚举扩展类
    /// </summary>
    public abstract class EnumHelper
    {
        /// <summary>
        /// 值转枚举类型
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="value">值</param>
        /// <param name="defaultType">默认值</param>
        /// <returns></returns>
        public static Object ToEnum(Type enumType, String value, Object defaultType)
        {
            if (Enum.IsDefined(enumType, value))
            {
                return Enum.Parse(enumType, value, false);
            }
            return defaultType;
        }
        /// <summary>
        /// 枚举类型转字符
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static String EnumToString(Enum enumType)
        {
            return enumType.ToString();
        }
        /// <summary>
        /// 将整数转换为枚举成员
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Object ToEnum(Type enumType, int index)
        {
            return Enum.ToObject(enumType, index);
        }
        /// <summary>
        /// SQL Server 数据类型
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static SqlDbType ToSqlDbType(String typeStr)
        {
            return (SqlDbType)ToEnum(typeof(SqlDbType), typeStr, SqlDbType.VarChar);
        }
        /// <summary>
        /// Access 数据类型
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public static OleDbType ToOleDbType(String typeStr)
        {
            return (OleDbType)ToEnum(typeof(OleDbType), typeStr, OleDbType.VarChar);
        }
    }
}
