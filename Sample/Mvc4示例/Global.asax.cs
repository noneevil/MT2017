using MSSQLDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json.Linq;
using CommonUtils;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using Newtonsoft.Json.Converters;

namespace Mvc4Example
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //db.CurrDbType = DatabaseType.ACCESS;
            //db.DbName = Server.MapPath("~/App_Data/data.mdb");
            db.CurrDbType = DatabaseType.SqLite;
            db.ConnectionString = "Data Source=|DataDirectory|data.db;Version=3;Pooling=True;Max Pool Size=100;Cache Size=8000;Page Size=4096;Synchronous=Off;Journal Mode=Off;";

            #region LogNet4日志初始化

            FileInfo logConfig = new FileInfo(Server.MapPath("/App_Data/Log4Net.xml"));
            log4net.Config.XmlConfigurator.Configure(logConfig);

            #endregion

            //7z压缩
            SevenZipSharpHelper.SetZipPath(HttpRuntime.BinDirectory + "7z.dll");

            //Removing X-AspNetMvc-Version
            MvcHandler.DisableMvcResponseHeader = true;

            // using Newtonsoft.Json 
            // POST JSON String 动态类型 字段别名
            //ModelBinders.Binders.Add(typeof(JObject), new JsonModelBinder());
            //ModelBinders.Binders.Add(typeof(JArray), new JsonModelBinder());
            ModelBinders.Binders.DefaultBinder = new JsonModelBinder();

            // Newtonsoft.Json POST JSON String 实体模型
            //ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().FirstOrDefault());//删除MVC默认适配器
            //ValueProviderFactories.Factories.Add(new JsonNetValueProviderFactory());//加入JSON.NET 解析适配器

            // MVC POST JSON String 字段实名
            //ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());//MVC默认适配器 (注:此处可不用手动添加,默认MVC已经加入)

            //MVC JSON.NET 默认参数设置
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
#if DEBUG
                Formatting = Formatting.Indented,
#else
                Formatting = Formatting.None,
#endif
                Culture = new CultureInfo("it-IT")
                {
                    NumberFormat = new NumberFormatInfo
                    {
                        NumberDecimalDigits = 2,
                        CurrencyDecimalDigits = 2
                    },
                    DateTimeFormat = new DateTimeFormatInfo
                    {
                        TimeSeparator = ":"
                    }
                },
                //NullValueHandling = NullValueHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                Converters = { new StringEnumConverter() }
            };

            //路由调试
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_AuthenticateRequest(Object sender, EventArgs e)
        {

        }

        void Application_BeginRequest(Object sender, EventArgs e)
        {

        }
    }
}