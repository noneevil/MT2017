using System;
using System.Web;
using System.Web.UI;

namespace CommonUtils
{
    /// <summary>
    /// ��Ϣ��ʾ������
    /// </summary>
    public class AlertHelper
    {
        /// <summary>
        /// ���һalert��Ϣ��
        /// </summary>
        /// <param name="Text"></param>
        public static void Message(String Text)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            String _text = Text.Replace("\r\n", "").Replace("'", "\"");
            ScriptManager.RegisterStartupScript(page, page.GetType(), "", "alert('" + _text + "');", true);
        }
        /// <summary>
        /// ���һ��js�ű�
        /// </summary>
        /// <param name="script"></param>
        public static void ExecuteScript(String script)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "", script, true);
        }
        /// <summary>
        /// ���һ��js�ű�����ҳ�����JQueryʱʹ�ô˷�����
        /// </summary>
        /// <param name="script"></param>
        public static void JQueryScript(String script)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "", " $(function(){\r\n" + script + "\r\n});", true);
        }
        /// <summary>
        /// ��ҳ�����JQueryʱ���ɵ�ҳ����ʾ��Ϣ
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
