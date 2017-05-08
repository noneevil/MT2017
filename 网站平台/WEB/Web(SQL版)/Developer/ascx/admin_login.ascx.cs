using System;
using CommonUtils;
using MSSQLDB;
using Newtonsoft.Json;
using WebSite.BackgroundPages;
using WebSite.Interface;

public partial class admin_login : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        IJsonResult result = new IJsonResult();
        if (!String.IsNullOrEmpty(AuthCode))
        {
            AuthCode = EncryptHelper.MD5Upper32(AuthCode.ToLower() + Utils.GetIp());
            if (AuthCode != CookieHelper.GetValue("AuthCode"))
            {
                result.Text = "验证码输入错误！";
            }
            else if (!String.IsNullOrEmpty(UserName) && !String.IsNullOrEmpty(UserPass))
            {
                result.Text = "用户名或密码输入错误！";
                Admin.LoginUser(UserName, UserPass, ref result);
            }
        }
        String json = JsonConvert.SerializeObject(result);
        Response.Write(json);
    }

    /// <summary>
    /// 用户名
    /// </summary>
    [RequestField(Name = "user")]
    protected String UserName { get; set; }
    /// <summary>
    /// 登录密码
    /// </summary>
    [RequestField(Name = "pwd")]
    protected String UserPass { get; set; }
    /// <summary>
    /// 验证码
    /// </summary>
    [RequestField(Name = "num")]
    protected String AuthCode { get; set; }
}