using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Mvc4Example.Content.kroppr
{
    /// <summary>
    /// Kroppr 的摘要说明
    /// </summary>
    public class Kroppr : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var Request = context.Request;
            Byte[] b = new Byte[Request.InputStream.Length];
            Request.InputStream.Read(b, 0, b.Length);
            string json = Encoding.UTF8.GetString(b); 
            
            context.Response.ContentType = "application/json";
            context.Response.Write(json);

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