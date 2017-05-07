using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Mvc4Example
{
    /// <summary>
    /// JSONP Object
    /// </summary>
    public class JsonpResult : JsonResult
    {
        public JsonpResult()
        {
            base.ContentType = "application/x-javascript";
            base.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }
        public JsonpResult(Object data)
        {
            base.Data = data;
            base.ContentType = "application/x-javascript";
            base.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpRequestBase request = context.HttpContext.Request;
            HttpResponseBase response = context.HttpContext.Response;
            var callback = request.QueryString["callback"] ?? "callback";
            response.Write(callback + "(");
            response.Write(JsonConvert.SerializeObject(this.Data));
            //base.ExecuteResult(context);
            response.Write(")");
        }
    }
}
