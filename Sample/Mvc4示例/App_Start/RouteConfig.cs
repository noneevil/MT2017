using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mvc4Example
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //ajax分页示例
            routes.MapRoute(
                name: "AjaxView",
                url: "AjaxView/{page}.html",
                defaults: new { controller = "Home", action = "AjaxView", page = 1 }
            );
            //列表分页
            routes.MapRoute(
                name: "List",
                url: "List/{page}.html",
                defaults: new { controller = "Home", action = "List", page = 1 }
            );
            routes.MapRoute(
                name: "Edit",
                url: "Edit/{id}.html",
                defaults: new { controller = "Home", action = "Edit" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}