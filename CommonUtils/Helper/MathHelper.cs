using System;

namespace CommonUtils
{
    /// <summary>
    /// 数值转换处理扩展类
    /// </summary>
    public class MathHelper
    {
        /// <summary>
        /// 将一字符串转换为整形数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetIntValue(Object value)
        {
            try
            {
                return (int)Convert.ToDouble(value);
            }
            catch { return 0; }
        }
        /// <summary>
        /// 将一字符串转换为整形数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long GetInt64(Object value)
        {
            try
            {
                return (long)Convert.ToDouble(value);
            }
            catch { return 0; }
        }
        /// <summary>
        /// 将一字符串转换为整形数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short GetInt16(Object value)
        {
            try
            {
                return (short)Convert.ToDouble(value);
            }
            catch { return 0; }
        }
        /// <summary>
        /// 将一字符串转换为双精度数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double GetDoubleValue(Object value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch { return 0; }
        }
        /// <summary>
        /// 将一字符串转换为Decimal数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Decimal GetFloatValue(Object value)
        {
            try
            {
                return (decimal)Convert.ToDouble(value);
            }
            catch { return 0; }
        }
        /// <summary>
        /// 将一字符串转换为日期时间类型 转换失败则返回:“1900-01-01"
        /// </summary>
        /// <param name="value">要转换的对象</param>
        /// <returns></returns>
        public static DateTime GetDateTimeValue(Object value)
        {
            try
            {
                return Convert.ToDateTime(value);
            }
            catch { return DateTime.Now; }
        }
        /// <summary>
        /// 将一个字符串转换为Boolean类型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Boolean GetBoolean(String s)
        {
            if (String.IsNullOrEmpty(s) || s == "0") return false;
            String _s = s.ToLower();
            int i = 0;
            int.TryParse(_s, out i);
            if (i != 0) return true;
            return (s.ToLower() == "true");
        }
        /// <summary>
        /// 将Object转为Boolean类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Boolean GetBoolean(Object data)
        {
            if (data is Boolean) return (Boolean)data;
            return GetBoolean(Convert.ToString(data));
        }
        
    }
}