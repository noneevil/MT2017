using System;
using System.IO.Compression;
using System.Web.Mvc;

namespace Mvc4Example
{
    /// <summary>
    /// 压缩输出流
    /// </summary>
    public class CompressionAttribute : ActionFilterAttribute
    {
        private string acceptEncoding;
        private bool Ignore;

        public CompressionAttribute()
        {
            acceptEncoding = "deflate";
        }
        public CompressionAttribute(string coding)
        {
            acceptEncoding = coding;
        }
        public CompressionAttribute(bool ignore)
        {
            Ignore = ignore;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Ignore)
            {
                var request = filterContext.HttpContext.Request;
                var response = filterContext.HttpContext.Response;
                //String acceptEncoding = request.Headers["Accept-Encoding"].ToLowerInvariant();
                //gzip压缩
                if (acceptEncoding.Contains("gzip"))
                {
                    response.AppendHeader("Content-Encoding", "gzip");
                    response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                }
                //使用7z压缩
                else if (acceptEncoding.Contains("7z"))
                {
                    response.AppendHeader("Content-Encoding", "7z");
                    response.Filter = new SevenZipFilter(response.Filter);
                }
                //使用7z压缩 并使用Base64编码
                else if (acceptEncoding.Contains("base64"))
                {
                    response.AppendHeader("Content-Encoding", "base64");
                    response.Filter = new Base64Filter(response.Filter);
                }
                //清除HTML空白
                else if (acceptEncoding.Contains("html"))
                {
                    //response.AppendHeader("Content-Encoding", "base64");
                    response.Filter = new WhiteSpaceFilter(response.Filter);
                }
                //默认压缩
                else
                {
                    response.AppendHeader("Content-Encoding", "deflate");
                    response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                }
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }
    }
}
