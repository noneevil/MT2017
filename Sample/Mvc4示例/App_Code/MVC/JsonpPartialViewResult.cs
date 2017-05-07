using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Mvc4Example
{
    /// <summary>
    /// JSONP 局部视图
    /// </summary>
    public class JsonpPartialViewResult : PartialViewResult
    {
        public JsonpPartialViewResult() : this(null, null) { }

        public JsonpPartialViewResult(object model) : this(null, model) { }

        public JsonpPartialViewResult(string viewName) : this(viewName, null) { }

        public JsonpPartialViewResult(string viewName, object model)
        {
            if (model != null) ViewData.Model = model;

            ViewName = viewName;
            ViewEngineCollection = ViewEngineCollection;
        }

        protected override ViewEngineResult FindView(ControllerContext context)
        {
            foreach (var key in context.Controller.ViewData.Keys)
            {
                ViewData.Add(key, context.Controller.ViewData[key]);
            }

            TempData = context.Controller.TempData;

            if (Model != null)
            {
                ViewData.Model = Model;
            }

            return base.FindView(context);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (String.IsNullOrEmpty(ViewName))
            {
                ViewName = context.RouteData.GetRequiredString("action");
            }

            ViewEngineResult result = null;
            if (View == null)
            {
                result = FindView(context);
                View = result.View;
            }

            using (StringWriter writer = new StringWriter())
            {
                ViewContext viewContext = new ViewContext(context, View, ViewData, TempData, writer);
                View.Render(viewContext, writer);
                if (result != null)
                {
                    result.ViewEngine.ReleaseView(context, View);
                }
                var html = writer.GetStringBuilder().ToString();
                //html = Regex.Replace(html, @"\s+\s", "", RegexOptions.Compiled);

                HttpRequestBase request = context.HttpContext.Request;
                HttpResponseBase response = context.HttpContext.Response;
                //response.Write(html);

                response.ContentType = "application/x-javascript";
                var callback = request.QueryString["callback"] ?? "callback";
                //response.Write(string.Format("{0}(\"{1}\");", callback, HttpUtility.JavaScriptStringEncode(html)));
                response.Write(string.Format("{0}({1});", callback, JsonConvert.SerializeObject(html)));
            }
        }
    }
}
