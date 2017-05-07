using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Mvc4Example
{
    /// <summary>
    /// Web Api 异常捕获接口
    /// </summary>
    public class WebApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            //更改输出异常信息内容,返回 410
            context.Response = new HttpResponseMessage()
            { 
                StatusCode = HttpStatusCode.Gone,
                Content = new StringContent(context.Exception.Message)
            };

            base.OnException(context);
        }
    }
}