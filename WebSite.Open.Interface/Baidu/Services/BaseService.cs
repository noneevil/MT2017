using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web.Script.Serialization;

namespace WebSite.Interface.Baidu
{

    class BaseService
    {
        protected BaiduApiInvoker invoker;

        protected RestFormat restFormat = RestFormat.Json;

        public BaseService(BaiduApiInvoker invoker)
        {
            this.invoker = invoker;
        }

        protected BaiduApiInvoker Invoker
        {
            get { return invoker; }
            set { invoker = value; }
        }

        protected BaiduApiException GenerateApiException(string errorString)
        {
            BaiduApiException exception = null;

            string error_code =null;
            string error_msg = null;

            if (this.restFormat == RestFormat.Xml)
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(errorString);

                XmlNode codeNode = xml.SelectSingleNode("//error_code");
                if (codeNode != null)
                    error_code = codeNode.InnerText;

                XmlNode msgNode = xml.SelectSingleNode("//error_msg");
                if (msgNode != null)
                    error_msg = msgNode.InnerText;

                exception = new BaiduApiException(error_code, error_msg);
            }
            else
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                exception = js.Deserialize<BaiduApiException>(errorString);
            
            }

            return exception;
        
        }

        protected string MakeApiCall(string method,Dictionary<string,string> parameters)
        {
            string resultStr = null;

            if (restFormat == RestFormat.Xml)
            {
                resultStr = this.Invoker.RequstRestXml(method, parameters);

            }
            else
            {
                resultStr = this.Invoker.RequstRestJson(method, parameters);
            }

            if (resultStr.Contains("error_code"))
            {
                throw GenerateApiException(resultStr);
            }

            return resultStr;

        }
    }
}
