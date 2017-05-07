using Mvc4Example.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc4Example
{
    /// <summary>
    /// 异常捕获
    /// </summary>
    public class MvcHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var file = filterContext.HttpContext.Server.MapPath("/Request.txt");
            filterContext.HttpContext.Request.SaveAs(file, true);
            var header = System.IO.File.ReadAllText(file);
            System.IO.File.Delete(file);

            //构造返回数据
            var data = new IResult
            {
                data = filterContext.Exception,
                header = header
            };
            filterContext.Result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(data),
                ContentType = "application/json"
            };

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 200;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

            base.OnException(filterContext);
        }
    }
}