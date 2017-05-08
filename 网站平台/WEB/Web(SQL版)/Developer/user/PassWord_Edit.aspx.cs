using System;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class PassWord_Edit : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Label1.CssClass = "line1px_2";
            if (String.IsNullOrEmpty(txtOldpwd.Text))
            {
                Label1.Text = "原始密码不能为空！";
            }
            else if (EncryptHelper.MD5Upper32(txtOldpwd.Text) != Admin.Password)
            {
                Label1.Text = "原始密码输入错误！";
            }
            else if (String.IsNullOrEmpty(txtNewpwd.Text))
            {
                Label1.Text = "新密码不能为空！";
            }
            else if (String.IsNullOrEmpty(txtRepwd.Text))
            {
                Label1.Text = "确认新密码不能为空！";
            }
            else if (txtNewpwd.Text != txtRepwd.Text)
            {
                Label1.Text = "新密码两次输入不匹配！";
            }
            else
            {
                String pwd = EncryptHelper.MD5Upper32(txtRepwd.Text);
                ExecuteObject obj = new ExecuteObject();
                obj.tableName = "T_User";
                obj.cmdtype = CmdType.UPDATE;
                obj.terms.Add("ID", Admin.ID);
                obj.cells.Add("UserPass", pwd);

                if (db.ExecuteCommand(obj))
                {
                    Admin.Password = pwd;

                    Alert(Label1, "密码修改成功！", "line1px_3");

                    AppendLogs("修改登录密码!", LogsAction.Edit);
                }
            }
        }
    }
}