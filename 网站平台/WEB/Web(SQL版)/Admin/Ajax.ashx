<%@ WebHandler Language="C#" Class="Ajax" %>
using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using FastReflectionLib;
using MSSQLDB;

public class Ajax : IHttpHandler, IReadOnlySessionState //IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        HttpRequest Request = context.Request;
        HttpResponse Response = context.Response;
        HttpServerUtility Server = context.Server;

        Response.Clear();
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddDays(30));
        
        String ascxpath = String.Format("/admin/ajaxview/{0}.ascx", Request.Params["action"]);
        if (File.Exists(Server.MapPath(ascxpath)))
        {
            String html;
            Page page = new Page();
            UserControl ctl = page.LoadControl(ascxpath) as UserControl;
            SetPropertyValues(ctl, context);
            page.Controls.Add(ctl);

            using (StringWriter sw = new StringWriter())
            {
                Server.Execute(page, sw, false);
                html = sw.ToString();
                //html = Regex.Replace(html, @"\s+\s", "");
            }
            
            #region 输出类型

            switch (Request["data"])
            {
                case "json":
                    Response.ContentType = "application/json";
                    break;
                case "javascript":
                    Response.ContentType = "application/javascript";
                    break;
                case "xml":
                    Response.ContentType = "text/xml";
                    break;
                case "html":
                    Response.ContentType = "text/html";
                    break;
                default:
                    Response.ContentType = "text/plain";
                    break;
            }

            #endregion
            
            Response.Write(html);
        }
    }
    /// <summary>
    /// 设置UserControl属性值
    /// </summary>
    /// <param name="control"></param>
    /// <param name="context"></param>
    private void SetPropertyValues(UserControl control, HttpContext context)
    {
        Type type = control.GetType();
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (PropertyInfo item in properties)
        {
            RequestFieldAttribute[] attribute = (RequestFieldAttribute[])item.GetCustomAttributes(typeof(RequestFieldAttribute), false);
            if (attribute != null && attribute.Length > 0)
            {
                String val = context.Request.Params[attribute[0].Name];
                Object value = val ?? attribute[0].DefaultValue;

                if (value != null)
                {
                    item.FastSetValue(control, Convert.ChangeType(value, item.PropertyType));
                }
            }
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