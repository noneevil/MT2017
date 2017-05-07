using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MSSQLDB;
using CommonUtils;

namespace TestSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("SEO", "seo/{page}/{auto}/{time}",
                new { controller = "Home", action = "seo", page = 1, auto = true, time = 30 });

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}", // 带有参数的 URL
                new { controller = "Home", action = "Index" } // 参数默认值
            );
        }

        protected void Application_Start()
        {
            db.CurrDbType = DatabaseType.SqLite;
            db.DbName = XmlConfig.ConnectionStrings("SQLite");

            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }

        protected void Application_PreSendRequestHeaders(Object source, EventArgs e)
        {
            //Response.Headers["Server"] = "www.ings.cn";
            //Response.Headers.Remove("X-AspNetMvc-Version");
        }
    }
}