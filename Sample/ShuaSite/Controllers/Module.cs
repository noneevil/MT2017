using System;
using System.Drawing;
using System.IO;
using System.Web;

/// <summary>
/// .shtml页面转向模块
/// </summary>
public class ShtmlModule : IHttpModule
{
    public ShtmlModule()
    {

    }

    #region IHttpModule 成员

    public void Init(HttpApplication context)
    {
        context.BeginRequest += new EventHandler(this.Application_BeginRequest);
        context.PreSendRequestHeaders += new EventHandler(this.Application_PreSendRequestHeaders);
    }
    private void Application_PreSendRequestHeaders(Object source, EventArgs e)
    {
        HttpApplication app = source as HttpApplication;
        //app.Response.Cache.SetExpires(DateTime.Now.AddDays(1));
        //app.Response.Cache.SetLastModified(DateTime.Now);
        //app.Response.Headers.Remove("etag");
        app.Response.Headers["Server"] = "www.wieui.com";
    }
    private void Application_BeginRequest(Object source, EventArgs e)
    {
        HttpApplication app = source as HttpApplication;
        HttpResponse Response = app.Response;
        HttpRequest Request = app.Request;
        HttpServerUtility Server = app.Server;
        string url = Request.Url.LocalPath;

        if (url.Contains(".shtml"))
        {
            Response.Clear();
            url = url.Replace(".shtml", ".html");
            if (File.Exists(Server.MapPath(url)))
            {
                Response.StatusCode = 301;
                Response.AddHeader("Location", "http://www.wieui.com" + url);
                Response.End();
            }
            else
            {
                //Image img = new Bitmap(600, 300);
                //Graphics g = Graphics.FromImage(img);
                //g.Clear(Color.White);
                //Font f = new Font("Chiller", 120);
                //Brush b = new SolidBrush(Color.Red);
                //g.DrawString("404", f, b, 150, 70);
                //MemoryStream ms = new MemoryStream();
                //img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                //ms.Close();
                //img.Dispose();
                //g.Dispose();
                //Response.StatusCode = 404;
                //Response.ContentType = "image/gif";
                //Response.BinaryWrite(ms.ToArray());
                Response.StatusCode = 404;
                Response.End();
            }
        }
        else if (Request.Url.Host == "zx7.vicp.net")
        {
            Response.Clear();
            Response.StatusCode = 301;
            Response.AddHeader("Location", "http://www.wieui.com" + url);
            Response.End();
        }
    }

    public void Dispose()
    {

    }
    #endregion
}
