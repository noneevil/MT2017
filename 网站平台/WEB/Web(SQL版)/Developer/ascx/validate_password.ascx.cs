using System;
using CommonUtils;
using MSSQLDB;
using Newtonsoft.Json;
using WebSite.BackgroundPages;
using WebSite.Interface;

public partial class validate_password : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        IJsonResult result = new IJsonResult { Text = " " };

        if (EncryptHelper.MD5Upper32(PassWord) != Admin.Password)
        {
            result.Text = "原始密码输入错误！";
        }
        else
        {
            result.Status = true;
        }
        Response.Write(JsonConvert.SerializeObject(result));
    }

    /// <summary>
    /// 注册用户名称
    /// </summary>
    [RequestField(Name = "param")]
    protected String PassWord { get; set; }
}