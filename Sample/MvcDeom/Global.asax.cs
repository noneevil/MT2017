using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MSSQLDB;

namespace MvcDeom
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


            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }
    }
}