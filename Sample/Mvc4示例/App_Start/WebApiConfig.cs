using Mvc4Example.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;

namespace Mvc4Example
{
    /// <summary>
    /// 教程 http://www.asp.net/web-api/overview/formats-and-model-binding/model-validation-in-aspnet-web-api
    /// </summary>
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //清除返回XML格式数据
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            //清除返回JSON格式数据
            //config.Formatters.JsonFormatter.SupportedEncodings.Clear();

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.Formatting = Formatting.Indented;//缩进
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            json.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            json.SerializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
            json.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            json.SerializerSettings.Culture = new CultureInfo("it-IT");

            //var xml = config.Formatters.XmlFormatter;
            //xml.Indent = true;

            //API JSONP
            GlobalConfiguration.Configuration.Formatters.Insert(0, new JsonpMediaTypeFormatter());


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { controller = "Attachment", action = "Get" }
            );
        }
    }
}
