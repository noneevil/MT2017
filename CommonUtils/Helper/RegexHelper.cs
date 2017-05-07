using System;
using System.Text.RegularExpressions;

namespace CommonUtils
{
    public class RegexHelper
    {
        public const RegexOptions regOptions = (RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// 验证字符串是否匹配正则表达式
        /// </summary>
        /// <param name="regex">正则表达式</param>
        /// <param name="text">要验证的字符串</param>
        /// <returns></returns>
        public static Boolean IsMatch(String regex, String text)
        {
            Regex reg = new Regex(regex, regOptions);
            return reg.IsMatch(text);
        }
        /// <summary>
        /// 移出script标签
        /// </summary> 
        /// <param name="html"></param>
        /// <returns></returns>
        public static String RemoveScripts(String html)
        {
            String regex = @"<script[^><]*>.*?<\/script>";
            return Replace(regex, html, String.Empty);
        }
        /// <summary>
        /// 清除HTML中的空白符号
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static String RemoveBlank(String html)
        {
            html = Regex.Replace(html, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @" />", "/>", RegexOptions.IgnoreCase);
            return html;
        }
        /// <summary>
        /// 替换字符
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="input"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static String Replace(String regex, String input, String replacement)
        {
            if (String.IsNullOrEmpty(input)) return String.Empty;
            Regex reg = new Regex(regex, regOptions);
            return reg.Replace(input, replacement);
        }
    }
}
