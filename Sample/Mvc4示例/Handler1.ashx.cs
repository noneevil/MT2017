using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.SessionState;
using System.Text;

/// <summary>
/// Handler1 的摘要说明
/// </summary>
public class Handler1 : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        HttpRequest Request = context.Request;
        HttpResponse Response = context.Response;
        HttpServerUtility Server = context.Server;
        HttpSessionState Session = context.Session;

        //Request.SaveAs(Server.MapPath("/Handler1.txt"), true);

        //Response.Write("oooooooooook");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
