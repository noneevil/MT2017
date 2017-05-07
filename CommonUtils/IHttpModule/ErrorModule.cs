using System;
using System.Web;
using log4net;
/*
 * web.config配置
 * 经典模式
 * <system.web>
 *      <httpModules>
 *          <add name="ErrorModule" type="WebSite.CommonUtils.IHttpModule.ErrorModule,WebSite.CommonUtils" />
 *      </httpModules>
 * </system.web>
 * 集成模式
 * <system.webServer>
 *      <modules>
 *          <add name="ErrorModule" type="WebSite.CommonUtils.IHttpModule.ErrorModule,WebSite.CommonUtils"/>
 *      </modules>
 * </system.webServer>
 */
namespace CommonUtils.IHttpModule
{
    public class ErrorModule : System.Web.IHttpModule
    {
        public void Init(System.Web.HttpApplication context)
        {
            context.Error += new EventHandler(OnError);
        }

        public void OnError(Object sender, EventArgs e)
        {
            HttpContext Context = HttpContext.Current;
            HttpResponse Response = Context.Response;
            HttpRequest Request = Context.Request;
            HttpServerUtility Server = Context.Server;

            #region 记录错误日志

            Exception ex = Server.GetLastError();
            String msg = String.Format("\r\n\tIP:{0}\r\n\t页面:{1}\r\n\t消息:{2}\r\n\t类型:{3}\r\n\t用户:{4}",
                                    Request.UserHostAddress,
                                    Request.Url.AbsoluteUri,
                                    ex.Message,
                                    ex.GetType(),
                                    Request.UserAgent);
            ILog logger = LogManager.GetLogger("Error");
            logger.Debug(msg);

            #endregion

            Server.ClearError();
            Response.Clear();
            Response.StatusCode = 410;
            Response.ContentType = "text/html";
            Response.TrySkipIisCustomErrors = true;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Context.ApplicationInstance.CompleteRequest();

        }

        public void Dispose()
        {

        }
    }
}
