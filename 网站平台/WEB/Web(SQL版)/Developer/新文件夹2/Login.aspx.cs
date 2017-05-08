using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using CommonUtils;
using WebSite.BackgroundPages;
using WebSite.Interface;

namespace WebSite.Web
{
    public partial class Login : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Request.IsLocal) Admin.LoginUser("administrator", Util.MD5("vs2012"));
                //if (Request["cmd"] == "exit") Admin.SignOut();
                if (Admin.UserData != null) Response.Redirect("/Developer");
            }
            base.OnPreInit(e);
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(num.Value))
            {
                String cookieval = CookieHelper.GetValue(ISessionKeys.cookie_authcode);
                String authcode = EncryptHelper.MD5Upper32(num.Value.ToLower() + Utils.GetIp());
                if (authcode == cookieval)
                {
                    if (!String.IsNullOrEmpty(user.Value) && !String.IsNullOrEmpty(pwd.Value))
                    {
                        //Admin.LoginUser(user.Value, pwd.Value);
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), null, "alert('验证码错误！'); ", true);
                }
            }
        }

        /// <summary>
        /// 压缩HTML
        /// </summary>
        /// <param name="writer"></param>
        //protected override void Render(HtmlTextWriter writer)
        //{
        //    StringWriter sw = new StringWriter();
        //    base.Render(new HtmlTextWriter(sw));
        //    String html = sw.ToString();
        //    html = Regex.Replace(html, @"\s+\s", "");
        //    html = html.Replace("//<![CDATA[", "").Replace("//]]", "");
        //    writer.Write(html);
        //}
    }
}