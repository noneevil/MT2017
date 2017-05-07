using System;
using System.Web;
/*
 * web.config配置
 * 经典模式
 * <system.web>
 *      <httpModules>
 *          <add name="GlobalModule" type="WebSite.CommonUtils.IHttpModule.GlobalModule,WebSite.CommonUtils" />
 *      </httpModules>
 * </system.web>
 * 集成模式
 * <system.webServer>
 *      <modules>
 *          <add name="GlobalModule" type="WebSite.CommonUtils.IHttpModule.GlobalModule,WebSite.CommonUtils"/>
 *      </modules>
 * </system.webServer>
 */
namespace CommonUtils.IHttpModule
{
    /// <summary>
    /// 全局扩展处理
    /// </summary>
    public class GlobalModule : System.Web.IHttpModule
    {
        #region IHttpModule 成员

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        void OnPreSendRequestHeaders(Object sender, EventArgs e)
        {
            try
            {
                HttpContext.Current.Response.Headers.Set("Server", "www.wieui.cn");
            }
            catch { }
        }

        public void Dispose()
        {
            
        }

        #endregion
    }
}
