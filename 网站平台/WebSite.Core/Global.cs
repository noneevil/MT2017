using System;
using System.IO;
using System.Web;
using CommonUtils;
using MSSQLDB;
using WebSite.Plugins;

namespace WebSite.Core
{
    /// <summary>
    /// 全局配置
    /// </summary>
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            #region 数据库配置

            var config = SiteParameter.Config;
            String ConnType = XmlConfig.AppSettings("ConnType");
            db.CurrDbType = config.DataType;
            db.ConnectionString = config.ConnectionString;
            //String ConnType = XmlConfig.AppSettings("ConnType");
            //db.CurrDbType = (DatabaseType)Enum.Parse(typeof(DatabaseType), ConnType, true);
            //db.ConnectionString = XmlConfig.ConnectionStrings(ConnType);

            #endregion

            #region LogNet4日志初始化

            FileInfo logConfig = new FileInfo(Server.MapPath("/App_Data/Log4Net.xml"));
            log4net.Config.XmlConfigurator.Configure(logConfig);

            #endregion

            #region 清空ViewState和Sessions数据

            SQLitePageStatePersister.ClearData();
            FilePageStatePersister.ClearFile();
            //启动定时清理任务
            TaskClear.Initialize();

            #endregion

            //注册虚拟路径
            SiteVirtualPath.AppInitialize();
            //设置7z库路径
            SevenZipSharpHelper.SetZipPath(Server.MapPath("/Bin/7z.dll"));
        }

        void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码
        }

        void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码
        }
        void Session_Start(object sender, EventArgs e)
        {
            //Session.Timeout = 0; // 在新会话启动时运行的代码
        }
        void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer 
            // 或 SQLServer，则不会引发该事件。
        }
        /// <summary>
        /// 验证检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //if (Request.IsAuthenticated)
            //{
            //    HttpCookie r = Request.Cookies["r"];
            //    if (r == null || String.IsNullOrEmpty(r.Value)) Admin.SignOut();
            //    if (EncryptHelper.MD5Upper32(CN.Keys + Utils.GetIp()) != Admin.GetData("t")) Admin.SignOut();
            //    Admin.SetData("v", EncryptHelper.MD5Upper32(CN.Keys + Utils.GetIp() + DateTime.Now.Ticks.ToString()));
            //}
        }
        /// <summary>
        /// SQL注入式攻击代码分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Application_BeginRequest(object sender, EventArgs e)
        {
            //iframe跨域与session失效问题
            Response.AddHeader("P3P", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");

            //if (string.IsNullOrEmpty(Admin.Name))
            //{
            //    if (Request.Path.ToLower() == "/webresource.axd") return;
            //    if (Request.QueryString.Count > 0)
            //    {
            //        foreach (string s in Request.QueryString.AllKeys)
            //        {
            //            if (ChkSql(Request[s]))
            //            {
            //                Response.StatusCode = 404;
            //                Response.End();
            //            }
            //        }
            //    }
            //    if (Request.Form.Count > 0)
            //    {
            //        foreach (string s in Request.Form.AllKeys)
            //        {
            //            if (s.StartsWith("__")) continue;
            //            if (ChkSql(Request[s]))
            //            {
            //                Response.StatusCode = 404;
            //                Response.End();
            //            }
            //        }
            //    }
            //}
        }
        /// <summary>
        /// SQL关键字检查
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool ChkSql(string str)
        {
            if (string.IsNullOrEmpty(str)) return false;
            str = Server.UrlDecode(str.ToLower());
            string a = "'|--|\"|replace|set|src|script|<|>|and|exec|where|like|create|insert|select|delete|update|count|chr|mid|master|truncate|char|declare|.exe|.bat|.js";
            string[] b = a.Split('|');
            foreach (string c in b)
            {
                if (str.IndexOf(c) > -1) return true;
            }
            return false;
        }
    }
}
