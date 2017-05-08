<%@ WebHandler Language="C#" Class="WebSite.Web.Ajax" %>
using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using FastReflectionLib;
using MSSQLDB;
using CommonUtils;

namespace WebSite.Web
{
    public class Ajax : IHttpHandler, IReadOnlySessionState //IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            HttpServerUtility Server = context.Server;
            
            Response.Clear();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.Now.AddDays(30));
            
            String action = Request.Params["action"] ?? Request.Headers["action"];
            String ascxpath = String.Format("/developer/ascx/{0}.ascx", action);
            if (File.Exists(Server.MapPath(ascxpath)))
            {
                String html = String.Empty;
                Page page = new Page();
                UserControl ctl = page.LoadControl(ascxpath) as UserControl;
                ctl.SetPropertyValues(context);
                page.Controls.Add(ctl);

                using (StringWriter sw = new StringWriter())
                {
                    Server.Execute(page, sw, false);
                    html = sw.ToString();
                    //html = Regex.Replace(html, @"\s+\s", "");
                }

                #region 输出类型

                String accept = Request.Headers["Accept"].ToLower();
                if (accept.Contains("application/xml"))
                {
                    Response.ContentType = "text/xml";
                }
                else if (accept.Contains("text/plain"))
                {
                    Response.ContentType = "text/plain";
                }
                else if (accept.Contains("application/json"))
                {
                    Response.ContentType = "application/json";
                }
                else if (accept.Contains("application/javascript"))
                {
                    Response.ContentType = "application/x-javascript";
                }
                else
                {
                    Response.ContentType = "text/html";
                }
                //switch (Request["data"])
                //{
                //    case "json":
                //        Response.ContentType = "application/json";
                //        break;
                //    case "javascript":
                //        Response.ContentType = "application/javascript";
                //        break;
                //    case "xml":
                //        Response.ContentType = "text/xml";
                //        break;
                //    case "html":
                //        Response.ContentType = "text/html";
                //        break;
                //    default:
                //        Response.ContentType = "text/plain";
                //        break;
                //}

                #endregion
                
                Response.Write(html);
            }
        }
        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}