using CommonUtils;
using System;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Mvc4Example.Api
{
    /// <summary>
    /// Web Api 使用7z压缩流输出
    /// </summary>
    public class CompressionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            HttpResponseMessage Response = context.Response;
            HttpContent content = Response.Content;
            Byte[] buffer = content == null ? new Byte[0] : content.ReadAsByteArrayAsync().Result;
            buffer = SevenZipSharpHelper.Compress(buffer);

            Response.Content = new ByteArrayContent(buffer);
            //Response.Content.Headers.Add("Content-encoding", "7z");
            Response.Content.Headers.Remove("Content-Type");
            Response.Content.Headers.Add("Content-Type", "application/octet-stream");
            base.OnActionExecuted(context);
        }
    }
}