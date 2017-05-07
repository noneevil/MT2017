using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonUtils
{
    /// <summary>
    /// 类  名：JSONConvert
    /// </summary>
    public static class JSONConvert
    {
        #region 全局变量

        private static JSONObject _json = new JSONObject();//寄存器
        private static readonly String _SEMICOLON = "@semicolon";//分号转义符
        private static readonly String _COMMA = "@comma"; //逗号转义符

        #endregion

        #region 字符串转义
        /// <summary>
        /// 字符串转义,将双引号内的:和,分别转成_SEMICOLON和_COMMA
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static String StrEncode(String text)
        {
            MatchCollection matches = Regex.Matches(text, "\\\"[^\\\"]*\\\"");
            foreach (Match match in matches)
            {
                text = text.Replace(match.Value, match.Value.Replace(":", _SEMICOLON).Replace(",", _COMMA));
            }

            return text;
        }

        /// <summary>
        /// 字符串转义,将_SEMICOLON和_COMMA分别转成:和,
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static String StrDecode(String text)
        {
            return text.Replace(_SEMICOLON, ":").Replace(_COMMA, ",");
        }

        #endregion

        #region JSON最小单元解析

        /// <summary>
        /// 最小对象转为JSONObject
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static JSONObject DeserializeSingletonObject(String text)
        {
            JSONObject jsonObject = new JSONObject();

            MatchCollection matches = Regex.Matches(text, "(\\\"(?<key>[^\\\"]+)\\\":\\\"(?<value>[^,\\\"]*)\\\")|(\\\"(?<key>[^\\\"]+)\\\":(?<value>[^,\\\"\\}]*))");
            foreach (Match match in matches)
            {
                String value = match.Groups["value"].Value;
                jsonObject.Add(match.Groups["key"].Value, _json.ContainsKey(value) ? _json[value] : StrDecode(value));
            }

            return jsonObject;
        }

        /// <summary>
        /// 最小数组转为JSONArray
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static JSONArray DeserializeSingletonArray(String text)
        {
            JSONArray jsonArray = new JSONArray();

            MatchCollection matches = Regex.Matches(text, "(\\\"(?<value>[^,\\\"]+)\")|(?<value>[^,\\[\\]]+)");
            foreach (Match match in matches)
            {
                String value = match.Groups["value"].Value;
                jsonArray.Add(_json.ContainsKey(value) ? _json[value] : StrDecode(value));
            }

            return jsonArray;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static String Deserialize(String text)
        {
            _json.Clear();//2010-01-20 清空寄存器
            text = StrEncode(text);//转义;和,

            int count = 0;
            String key = String.Empty;
            String pattern = "(\\{[^\\[\\]\\{\\}]*\\})|(\\[[^\\[\\]\\{\\}]*\\])";

            while (Regex.IsMatch(text, pattern))
            {
                MatchCollection matches = Regex.Matches(text, pattern);
                foreach (Match match in matches)
                {
                    key = "___key" + count + "___";

                    if (match.Value.Substring(0, 1) == "{")
                        _json.Add(key, DeserializeSingletonObject(match.Value));
                    else
                        _json.Add(key, DeserializeSingletonArray(match.Value));

                    text = text.Replace(match.Value, key);

                    count++;
                }
            }
            return text;
        }

        #endregion

        #region 公共接口

        /// <summary>
        /// 反序列化JSONObject对象
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static JSONObject DeserializeObject(String text)
        {
            return _json[Deserialize(text)] as JSONObject;
        }

        /// <summary>
        /// 反序列化JSONArray对象
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static JSONArray DeserializeArray(String text)
        {
            return _json[Deserialize(text)] as JSONArray;
        }

        /// <summary>
        /// 序列化JSONObject对象
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        public static String SerializeObject(JSONObject jsonObject)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (KeyValuePair<String, Object> kvp in jsonObject)
            {
                if (kvp.Value is JSONObject)
                {
                    sb.Append(String.Format("\"{0}\":{1},", kvp.Key, SerializeObject((JSONObject)kvp.Value)));
                }
                else if (kvp.Value is JSONArray)
                {
                    sb.Append(String.Format("\"{0}\":{1},", kvp.Key, SerializeArray((JSONArray)kvp.Value)));
                }
                else if (kvp.Value is String)
                {
                    sb.Append(String.Format("\"{0}\":\"{1}\",", kvp.Key, kvp.Value));
                }
                else
                {
                    sb.Append(String.Format("\"{0}\":\"{1}\",", kvp.Key, ""));
                }
            }
            if (sb.Length > 1)
                sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 序列化JSONArray对象
        /// </summary>
        /// <param name="jsonArray"></param>
        /// <returns></returns>
        public static String SerializeArray(JSONArray jsonArray)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < jsonArray.Count; i++)
            {
                if (jsonArray[i] is JSONObject)
                {
                    sb.Append(String.Format("{0},", SerializeObject((JSONObject)jsonArray[i])));
                }
                else if (jsonArray[i] is JSONArray)
                {
                    sb.Append(String.Format("{0},", SerializeArray((JSONArray)jsonArray[i])));
                }
                else if (jsonArray[i] is String)
                {
                    sb.Append(String.Format("\"{0}\",", jsonArray[i]));
                }
                else
                {
                    sb.Append(String.Format("\"{0}\",", ""));
                }

            }
            if (sb.Length > 1)
                sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }

    /// <summary>
    /// 类  名：JSONObject
    /// 描  述：JSON对象类
    /// 编  写：
    /// 站  点：
    /// 日  期：2010-01-06
    /// 版  本：1.1.0
    /// 更新历史：
    ///     2010-01-06  继承Dictionary<TKey, TValue>代替this[]
    /// </summary>
    public class JSONObject : Dictionary<String, Object>
    { }

    /// <summary>
    /// 类  名：JSONArray
    /// 描  述：JSON数组类
    /// 编  写：
    /// 站  点：
    /// 日  期：2010-01-06
    /// 版  本：1.1.0
    /// 更新历史：
    ///     2010-01-06  继承List<T>代替this[]
    /// </summary>
    public class JSONArray : List<Object>
    { }
}