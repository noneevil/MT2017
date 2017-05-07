using System;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace CommonUtils
{
    /// <summary>
    /// 数据验证处理类
    /// </summary>
    public abstract class ValidatorHelper
    {
        #region 数据格式判断

        /// <summary>
        /// 匹配字母,数字,下划线的组合
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsNormalChar(String str)
        {
            return !Regex.IsMatch(str, @"[^\w]", RegexOptions.Compiled);
        }
        /// <summary>
        /// 对象是否为 System.DBNull 类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Boolean IsDBNull(Object obj)
        {
            return Convert.IsDBNull(obj);
        }
        /// <summary>
        /// 验证Email地址 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsEmail(String str)
        {
            return Regex.IsMatch(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        /// <summary>
        /// 验证是否为小数 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsValidDecimal(String str)
        {
            return Regex.IsMatch(str, @"[0].\d{1,2}|[1]");
        }
        /// <summary>
        /// 验证两位小数 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsDecimal(String str)
        {
            return Regex.IsMatch(str, @"^[0-9]+(.[0-9]{2})?$");
        }
        /// <summary>
        /// 验证一年的12个月
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsMonth(String str)
        {
            return Regex.IsMatch(str, @"^(0?[[1-9]|1[0-2])$");
        }
        /// <summary>
        /// 验证一个月的31天
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsDay(String str)
        {
            return Regex.IsMatch(str, @"^((0?[1-9])|((1|2)[0-9])|30|31)$");
        }
        /// <summary>
        /// 验证是否为电话号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsValidTel(String str)
        {
            return Regex.IsMatch(str, @"(\d+-)?(\d{4}-?\d{7}|\d{3}-?\d{8}|^\d{7,8})(-\d+)?");
        }
        /// <summary>
        /// 验证年月日 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsValidDate(String str)
        {
            return Regex.IsMatch(str, @"^2\d{3}-(?:0?[1-9]|1[0-2])-(?:0?[1-9]|[1-2]\d|3[0-1])(?:0?[1-9]|1\d|2[0-3]):(?:0?[1-9]|[1-5]\d):(?:0?[1-9]|[1-5]\d)$");
        }
        /// <summary>
        /// 验证后缀名 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsValidPostfix(String str)
        {
            return Regex.IsMatch(str, @"\.(?i:gif|jpg)$");
        }
        /// <summary>
        /// 验证字符是否在4至12之间 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsValidByte(String str)
        {
            return Regex.IsMatch(str, @"^[a-z]{4,12}$");
        }
        /// <summary>
        /// 验证IP 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsValidIp(String str)
        {
            return Regex.IsMatch(str, @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
        }
        /// <summary>
        /// 验证输入汉字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>  
        public Boolean IsChinese(String str)
        {
            //return Regex.IsMatch(str, @"^[\u4e00-\u9fa5],{0,}$");
            return Regex.IsMatch(str, "[\u4e00-\u9fa5]", RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 验证输入字符串 (至少8个字符) 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public Boolean IsLength(String str)
        {
            return Regex.IsMatch(str, @"^.{8,}$");
        }
        /// <summary>
        /// 验证数字输入 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public Boolean IsNumber(String str)
        {
            return Information.IsNumeric(str);
            //return Regex.IsMatch(str, @"^[0-9]*$");
        }
        /// <summary>
        /// 验证日期输入 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public Boolean IsDate(String str)
        {
            return Information.IsDate(str);
            //return Regex.IsMatch(str, @"^[0-9]*$");
        }
        /// <summary>
        /// 验证密码长度 (6-18位)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public Boolean IsPasswLength(String str)
        {
            return Regex.IsMatch(str, @"^\d{6,18}$");
        }
        /// <summary>
        /// 验证是否只含有字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean isValidOnllyChar(String str)
        {
            return Regex.IsMatch(str, "^[A-Za-z]+$");
        }
        /// <summary>
        /// 验证是否只含有汉字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean isValidOnllyChinese(String str)
        {
            return Regex.IsMatch(str, @"^[\u4e00-\u9fa5]+$");
        }
        /// <summary>
        /// 验证是否只含有数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean isValidOnlyNumber(String str)
        {
            return Regex.IsMatch(str, "^[0-9]+$");
        }
        /// <summary>
        /// 验证是否是有效密码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean isValidPassWord(String str)
        {
            return Regex.IsMatch(str, @"^(\w){6,20}$");
        }
        /// <summary>
        /// 验证是否是有效邮编号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean isValidZip(String str)
        {
            return Regex.IsMatch(str, "^[a-z0-9 ]{3,12}$");
        }
        /// <summary>
        /// 检验字符串是否为URL地址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsURLAddress(String str)
        {
            return Regex.IsMatch(str, @"[a-zA-z]+://[^s]*");
        }
        /// <summary>
        /// 检验字符串是否为IP地址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsIPAddress(String str)
        {
            return Regex.IsMatch(str, @"d+.d+.d+.d+");
            //return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        /// <summary>
        /// 判断字符串是否为手机号或小灵通号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsMobile(String str)
        {
            return Regex.IsMatch(str, @"^((\d{11,12})|(\d{7}))$");
        }
        /// <summary>
        /// 判断一个字符串是否为Boolean类型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Boolean IsBoolean(String s)
        {
            return (s.ToLower() == "true" || s.ToLower() == "false");
        }
        /// <summary>
        /// 判断一个字符串是否为数值类型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Boolean IsNumberValue(String s)
        {
            try
            {
                Convert.ToDouble(s);
                return true;
            }
            catch { return false; }
        }
        /// <summary>
        /// 判断奇偶数
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static Boolean IsOdd(int n)
        {
            return Convert.ToBoolean(n & 1);
        }

        #endregion
    }
}
