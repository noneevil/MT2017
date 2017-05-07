using System;
using System.Web;
using System.Web.UI;

namespace CommonUtils
{
    /// <summary>
    /// 消息提示框处理类
    /// </summary>
    public class AlertHelper
    {
        /// <summary>
        /// 输出一alert消息框
        /// </summary>
        /// <param name="Text"></param>
        public static void Message(String Text)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            String _text = Text.Replace("\r\n", "").Replace("'", "\"");
            ScriptManager.RegisterStartupScript(page, page.GetType(), "", "alert('" + _text + "');", true);
        }
        /// <summary>
        /// 输出一段js脚本
        /// </summary>
        /// <param name="script"></param>
        public static void ExecuteScript(String script)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "", script, true);
        }
        /// <summary>
        /// 输出一段js脚本（当页面包含JQuery时使用此方法）
        /// </summary>
        /// <param name="script"></param>
        public static void JQueryScript(String script)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "", " $(function(){\r\n" + script + "\r\n});", true);
        }
        /// <summary>
        /// 当页面包含JQuery时生成的页面提示信息
        /// </summary>
        /// <param name="Text"></param>
        public static void JQueryMessage(String Text)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            String _text = Text.Replace("\r\n", "").Replace("'", "\"");
            ScriptManager.RegisterStartupScript(page, page.GetType(), "", " $(function(){\r\nalert('" + _text + "');\r\n});", true);
        }
    }
}
