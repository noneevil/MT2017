using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;

namespace CommonUtils.Discuz.Toolkit
{
    public class Util
    {
        private const String LINE = "\r\n";

        private static Dictionary<int, XmlSerializer> serializer_dict = new Dictionary<int, XmlSerializer>();

        //private DNTParam VersionParam = DNTParam.Create("v", "1.0");
        private String api_key;
        private String secret;
        private String url;
        private Boolean use_json;

        private static XmlSerializer ErrorSerializer
        {
            get
            {
                return GetSerializer(typeof(Error));
            }
        }

        public Util(String api_key, String secret, String url)
        {
            this.api_key = api_key;
            this.secret = secret;
            this.url = url;
        }

        public Boolean UseJson
        {
            get { return use_json; }
            set { use_json = value; }
        }

        internal String SharedSecret
        {
            get { return secret; }
            set { secret = value; }
        }

        internal String ApiKey
        {
            get { return api_key; }
        }

        internal String Url
        {
            get { return url; }
            set { url = value; }
        }

        public T GetResponse<T>(String method_name, params DiscuzParam[] parameters)
        {
            //String url = FormatGetUrl(method_name, parameters);
            //byte[] response_bytes = GetResponseBytes(url);

            DiscuzParam[] signed = Sign(method_name, parameters);

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < signed.Length; i++)
            {
                if (i > 0)
                    builder.Append("&");

                builder.Append(signed[i].ToEncodedString());
            }

            byte[] response_bytes = GetResponseBytes(Url, method_name, builder.ToString());


            XmlSerializer response_serializer = GetSerializer(typeof(T));
            try
            {
                T response = (T)response_serializer.Deserialize(new MemoryStream(response_bytes));
                return response;
            }
            catch
            {
                Error error = (Error)ErrorSerializer.Deserialize(new MemoryStream(response_bytes));
                throw new DiscuzException(error.ErrorCode, error.ErrorMsg);
            }
        }

        public static byte[] GetResponseBytes(String apiUrl, String method_name, String postData)
        {


            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(apiUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;
            request.Timeout = 20000;

            HttpWebResponse response = null;

            try
            {
                StreamWriter swRequestWriter = new StreamWriter(request.GetRequestStream());
                swRequestWriter.Write(postData);
                if (swRequestWriter != null)
                    swRequestWriter.Close();

                response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return Encoding.UTF8.GetBytes(reader.ReadToEnd());
                }
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }


        private String FormatGetUrl(String method_name, params DiscuzParam[] parameters)
        {
            DiscuzParam[] signed = Sign(method_name, parameters);

            StringBuilder builder = new StringBuilder(Url);

            for (int i = 0; i < signed.Length; i++)
            {
                if (i > 0)
                    builder.Append("&");

                builder.Append(signed[i].ToString());
            }

            return builder.ToString();
        }

        public static XmlSerializer GetSerializer(Type t)
        {
            int type_hash = t.GetHashCode();

            if (!serializer_dict.ContainsKey(type_hash))
                serializer_dict.Add(type_hash, new XmlSerializer(t));

            return serializer_dict[type_hash];
        }

        public DiscuzParam[] Sign(String method_name, DiscuzParam[] parameters)
        {
            List<DiscuzParam> list = new List<DiscuzParam>(parameters);
            list.Add(DiscuzParam.Create("method", method_name));
            list.Add(DiscuzParam.Create("api_key", api_key));
            list.Add(DiscuzParam.Create("call_id", DateTime.Now.Ticks));
            list.Sort();

            StringBuilder values = new StringBuilder();

            foreach (DiscuzParam param in list)
            {
                if (!String.IsNullOrEmpty(param.Value))
                    values.Append(param.ToString());
            }

            values.Append(secret);

            byte[] md5_result = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(values.ToString()));

            StringBuilder sig_builder = new StringBuilder();

            foreach (byte b in md5_result)
                sig_builder.Append(b.ToString("x2"));

            list.Add(DiscuzParam.Create("sig", sig_builder.ToString()));

            return list.ToArray();
        }

        public static int GetIntFromString(String input)
        {
            try
            {
                return int.Parse(input);
            }
            catch
            {
                return 0;
            }
        }

        public static Boolean GetBoolFromString(String input)
        {
            try
            {
                return Boolean.Parse(input);
            }
            catch
            {
                return false;
            }

        }

        public static String RemoveJsonNull(String json)
        {
            json = System.Text.RegularExpressions.Regex.Replace(json, @",""\w*"":null", String.Empty);
            json = System.Text.RegularExpressions.Regex.Replace(json, @"""\w*"":null,", String.Empty);
            json = System.Text.RegularExpressions.Regex.Replace(json, @"""\w*"":null", String.Empty);
            json = System.Text.RegularExpressions.Regex.Replace(json, @",""\w*"":0", String.Empty);
            json = System.Text.RegularExpressions.Regex.Replace(json, @"""\w*"":0,", String.Empty);
            return json;
        }

        /// <summary>
        /// 过滤HTML中的不安全标签
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static String RemoveUnsafeHtml(String content)
        {
            content = Regex.Replace(content, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"(script|frame|form|meta|behavior|style)([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return content;
        }

        /// <summary>
        /// php time()
        /// </summary>
        /// <returns></returns>
        public static long Time()
        {
            DateTime timeStamp = new DateTime(1970, 1, 1);
            return (DateTime.UtcNow.Ticks - timeStamp.Ticks) / 10000000;
        }

        /// <summary>
        /// 获取API提交的参数
        /// </summary>
        /// <param name="request">request对象</param>
        /// <returns>参数数组</returns>
        private DiscuzParam[] GetParamsFromRequest(HttpRequest request)
        {
            List<DiscuzParam> list = new List<DiscuzParam>();
            foreach (String key in request.QueryString.AllKeys)
            {
                list.Add(DiscuzParam.Create(key, request.QueryString[key]));
            }
            foreach (String key in request.Form.AllKeys)
            {
                list.Add(DiscuzParam.Create(key, request.Form[key]));
            }
            list.Sort();
            return list.ToArray();
        }

        /// <summary>
        /// 根据参数和密码生成签名字符串
        /// </summary>
        /// <param name="parameters">API参数</param>
        /// <param name="secret">密码</param>
        /// <returns>签名字符串</returns>
        private String GetSignature(DiscuzParam[] parameters, String secret)
        {
            StringBuilder values = new StringBuilder();

            foreach (DiscuzParam param in parameters)
            {
                if (param.Name == "sig" || String.IsNullOrEmpty(param.Value))
                    continue;
                values.Append(param.ToString());
            }

            values.Append(secret);

            byte[] md5_result = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(values.ToString()));

            StringBuilder sig_builder = new StringBuilder();

            foreach (byte b in md5_result)
                sig_builder.Append(b.ToString("x2"));

            return sig_builder.ToString();
        }


        /// <summary>
        /// 获取API 数据同步传递的参数
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, String> GetQueryString()
        {
            DiscuzParam[] parameters = GetParamsFromRequest(HttpContext.Current.Request);
            String sig = GetSignature(parameters, this.secret);

            Boolean samesig = false;
            Dictionary<String, String> dict = new Dictionary<String, String>();
            foreach (DiscuzParam dp in parameters)
            {
                if (dp.Name == "sig")
                {
                    if (dp.Value == sig)
                        samesig = true;
                    continue;
                }
                dict.Add(dp.Name, dp.Value);
            }

            if (samesig)
                return dict;
            return new Dictionary<String, String>();
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="isMD5Passwd"></param>
        /// <returns></returns>
        public String GetMD5(String str, Boolean isMD5Passwd)
        {
            if (isMD5Passwd) return str;
            String pwd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5").ToString();
            return pwd;
        }
    }
}
