using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web;

namespace WebSite.Interface.Baidu
{
    /// <summary>
    /// 接口返回格式枚举类型。
    /// </summary>
    /// <remarks></remarks>
    public enum RestFormat
    {
        /// <summary>
        /// json格式。
        /// </summary>
        Json=0,
        
        /// <summary>
        /// xml格式。
        /// </summary>
        Xml=1
    }

    class BaiduApiInvoker
    {
        private static readonly string FORMAT_XML = "xml";
        private static readonly string FORMAT_JSON = "json";
        private static readonly string restHttpBase = "http://openapi.baidu.com/rest/2.0/";
        private static readonly string restHttpsBase = "https://openapi.baidu.com/rest/2.0/";

        private string access_token;
        private string session_key;
        private string session_secret;
        private bool isHttpsRequest;

        /// <summary>
        /// 创建API的调用实例，以http方式请求API。
        /// </summary>
        /// <param name="session_key">The session_key.</param>
        /// <param name="session_secret">The session_secret.</param>
        public BaiduApiInvoker(string session_key, string session_secret)
        {
            this.session_key = session_key;
            this.session_secret = session_secret;

            this.isHttpsRequest = false;
        }

        /// <summary>
        /// 创建API的调用实例，以https方式请求API。
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        public BaiduApiInvoker(string accessToken)
        {
            this.access_token = accessToken;

            this.isHttpsRequest = true;
        }

        /// <summary>
        /// 请求rest服务器以XML数据格式返回.
        /// </summary>
        /// <param name="method">API接口.列表见http://dev.baidu.com/wiki/connect/index.php?title=Open_API_2.0_%E6%96%87%E6%A1%A3#.E7.99.BE.E5.BA.A6Open_API.E6.8E.A5.E5.8F.A3.E5.88.97.E8.A1.A8</param>
        /// <param name="appParamters">API应用级参数.</param>
        /// <returns></returns>
        public string RequstRestXml(string method,Dictionary<string, string> appParamters)
        {
            return RequstRest(method.Trim(),appParamters,FORMAT_XML);
        }

        /// <summary>
        /// 请求rest服务器以JSON数据格式返回.
        /// </summary>
        /// <param name="method">API接口.列表见http://dev.baidu.com/wiki/connect/index.php?title=Open_API_2.0_%E6%96%87%E6%A1%A3#.E7.99.BE.E5.BA.A6Open_API.E6.8E.A5.E5.8F.A3.E5.88.97.E8.A1.A8</param>
        /// <param name="appParamters">API应用级参数.</param>
        /// <returns></returns>
        public string RequstRestJson(string method, Dictionary<string, string> appParamters)
        {
            return RequstRest(method.Trim(), appParamters, FORMAT_JSON);
        }

        /// <summary>
        /// 请求REST服务器并以json或xml文本返回结果.
        /// </summary>
        /// <param name="method">API 接口</param>
        /// <param name="appParamters">API接口的应用级参数，系统级参数不用添加。</param>
        /// <param name="format">返回数据格式，json或xml.</param>
        /// <returns></returns>
        private string RequstRest(string method,IDictionary<string, string> appParamters,string format)
        {
            if (this.isHttpsRequest)
            {
                return HttpsRequstRest(method, appParamters, format);
            }
            else
            {
                return HttpRequstRest(method,appParamters,format);
            }

        }


        private string HttpsRequstRest(string method, IDictionary<string, string> appParamters, string format)
        { 
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(restHttpsBase).Append(method).Append("?access_token=").Append(
                HttpUtility.UrlEncode(this.access_token)).Append("&format=");

            if (string.Compare(FORMAT_XML, format, true) == 0)
            {
                urlBuilder.Append(FORMAT_XML);
            }
            else
            {
                urlBuilder.Append(FORMAT_JSON);
            }

            string restResponse = null;

            string queryString = appParamters==null?null:GenerateQueryString(appParamters);//构造应用级参数

            try
            {
                restResponse = new HttpUtils().HttpPost(urlBuilder.ToString(), queryString);

            }
            catch
            {
                throw;
            }

            return restResponse;
        }
        
        private string HttpRequstRest(string method, IDictionary<string, string> appParamters, string format)
        {
            if (appParamters == null)
            {
                appParamters = new Dictionary<string, string>();
            }

            appParamters.Add("session_key", this.session_key);
            appParamters.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

            if (string.Compare(FORMAT_XML, format, true) == 0)
            {
                appParamters.Add("format", FORMAT_XML);
            }
            else
            {
                appParamters.Add("format", FORMAT_JSON);
            }

            string sig = getSignature(appParamters, this.session_secret);
            appParamters.Add("sign", sig);

            string urlParamters = GenerateQueryString(appParamters);

            string result = null;
            try
            {
                result = new HttpUtils().HttpPost(restHttpBase + method, urlParamters);

            }
            catch
            {
                throw;
            }

            return result;

        }

        /// <summary>
        /// 生成HTTP请求的参数字符串
        /// </summary>
        /// <returns></returns>
        public static string GenerateQueryString(IDictionary<string, string> parameters)
        {
            StringBuilder strBuilder = new StringBuilder();
            IEnumerator<KeyValuePair<string, string>> iterator = parameters.GetEnumerator();

            //第一个参数不加“&”符号
            if(iterator.MoveNext())
            {
                string encodeKey = HttpUtility.UrlEncode(iterator.Current.Key);
                string encodeValue = HttpUtility.UrlEncode(iterator.Current.Value);
                strBuilder.Append(encodeKey).Append("=").Append(encodeValue);
            }
            //往后每个参数之前加“&”符号
            while (iterator.MoveNext())
            {
                string encodeKey = HttpUtility.UrlEncode(iterator.Current.Key);
                string encodeValue = HttpUtility.UrlEncode(iterator.Current.Value);
                strBuilder.Append("&").Append(encodeKey).Append("=").Append(encodeValue);
            }

            
            return strBuilder.ToString();
        }

        /// <summary>
        /// 计算参数签名（Baidu提供）
        /// </summary>
        /// <param name="parameters">请求参数集，所有参数必须已转换为字符串类型</param>
        /// <param name="secret">签名密钥</param>
        /// <returns>签名</returns>
        /// <remarks></remarks>
        public static string getSignature(IDictionary<string, string> parameters, string secret)
        {
            // 先将参数以其参数名的字典序升序进行排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> iterator = sortedParams.GetEnumerator();

            // 遍历排序后的字典，将所有参数按"key=value"格式拼接在一起
            StringBuilder basestring = new StringBuilder();
            while (iterator.MoveNext())
            {
                string key = iterator.Current.Key;
                string value = iterator.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    basestring.Append(key).Append("=").Append(value);
                }
            }
            basestring.Append(secret);

            // 使用MD5对待签名串求签
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(basestring.ToString()));

            // 将MD5输出的二进制结果转换为小写的十六进制
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = bytes[i].ToString("x");
                if (hex.Length == 1)
                {
                    result.Append("0");
                }
                result.Append(hex);
            }

            return result.ToString();
        }
    }
}
