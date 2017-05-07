using System.Web;
using System.Web.Mvc;

namespace Mvc4Example
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            //filters.Add(new MvcHandleErrorAttribute());//异常捕获
            //filters.Add(new ApiExceptionLogsFilter());

            //用户登录认证
            //config.MessageHandlers.Add(new UserAuthorizeHandler());
            //登录认证
            //filters.Add(new UserAuthorizeAttribute());
            //filters.Add(new WebApiExceptionFilter());
        }
    }
}