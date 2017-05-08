using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using CommonUtils;
using Newtonsoft.Json;
using WebSite.BackgroundPages;
using WebSite.Models;
using WebSite.Interface;

namespace WebSite.Web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Request.IsLocal) Admin.LoginUser("administrator", Util.MD5("vs2012"));
                switch (Request["cmd"])
                {
                    case "login":
                        goLogin();
                        break;
                    case "exit":
                        Admin.SignOut();
                        break;
                }
                if (Admin.UserData != null) Response.Redirect("/Developer");
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        private void goLogin()
        {
            if (!String.IsNullOrEmpty(Request["num"]))
            {
                String authcode = Request["num"].ToLower() + Utils.GetIp();
                if (EncryptHelper.MD5Upper32(authcode) != CookieHelper.GetValue("AuthCode"))
                {
                    IJsonResult msg = new IJsonResult { Status = false, Text = "验证码输入错误！" };
                    Response.Write(JsonConvert.SerializeObject(msg));
                }
                else if (!String.IsNullOrEmpty(Request["user"]) && !String.IsNullOrEmpty(Request["pwd"]))
                {
                    //Admin.LoginUser(Request["user"], Request["pwd"]);
                }
            }
            Response.End();
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