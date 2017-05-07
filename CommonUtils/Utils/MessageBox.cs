using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CommonUtils
{
    /// <summary>
    /// 显示消息提示对话框。
    /// </summary>
    public class MessageBox
    {
        /// <summary>
        /// 显示消息提示对话框
        /// </summary>
        /// <param name="msg">提示信息</param>
        public static void Show(String msg)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script type=\"javascript\">alert('" + msg.ToString() + "');</script>");
        }

        /// <summary>
        /// 控件点击 消息确认提示框
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        public static void ShowConfirm(WebControl Control, String msg)
        {
            //Control.Attributes.Add("onClick","if (!window.confirm('"+msg+"')){return false;}");
            Control.Attributes.Add("onclick", "return confirm('" + msg + "');");
        }

        /// <summary>
        /// 显示消息提示对话框，并进行页面跳转
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="url">跳转的目标URL</param>
        public static void ShowAndRedirect(String msg, String url)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            //Response.Write("<script>alert('帐户审核通过！现在去为企业充值。');window.location=\"" + pageurl + "\"</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script type=\"javascript\">alert('" + msg + "');window.location=\"" + url + "\"</script>");
        }
        /// <summary>
        /// 显示消息提示对话框，并进行页面跳转
        /// </summary>
        /// <param name="msg">提示信息</param>
        /// <param name="url">跳转的目标URL</param>
        public static void ShowAndRedirects(String msg, String url)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type=\"javascript\">");
            sb.AppendFormat("alert('{0}');", msg);
            sb.AppendFormat("top.location.href='{0}'", url);
            sb.Append("</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", sb.ToString());
        }

        /// <summary>
        /// 输出自定义脚本信息
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="script">输出脚本</param>
        public static void ResponseScript(String script)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            page.ClientScript.RegisterStartupScript(page.GetType(), "message", "<script type=\"javascript\">" + script + "</script>");
        }

    }
}
