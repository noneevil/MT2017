using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using WebSite.BackgroundPages;

namespace WebSite.Web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Request.IsLocal) Admin.LoginUser("administrator", Util.MD5("vs2012"));
                if (Request["action"] == "exit")
                    Admin.SignOut();
                if (Admin.UserData != null)
                    Response.Redirect("/Developer");
            }
        }
        /// <summary>
        /// 压缩HTML
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            StringWriter sw = new StringWriter();
            base.Render(new HtmlTextWriter(sw));
            String html = sw.ToString();
            html = Regex.Replace(html, @"\s+\s", "");
            writer.Write(html);
        }
    }
}