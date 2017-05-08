using CommonUtils;
using MSSQLDB;
using System;
using WebSite.BackgroundPages;
using WebSite.Interface;
using WebSite.Models;
using WebSite.UserCenter;

namespace WebSite.Web
{
    public partial class Member_Edit : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime time = DateTime.Now;
                joindate.SetValue(time);
                lastsigntime.SetValue(time);
                birthday.SetValue(time.AddYears(-20));

                foreach (MemberType item in Enum.GetValues(typeof(MemberType)))
                {
                    if (item == MemberType.所有会员 || item == MemberType.匿名用户) continue;
                    String text = item.ToString();
                    membertype.Items.Add(text);
                }

                if (IsEdit)
                {
                    LoadData();
                    membername.Enabled = false;
                    RequiredPassword.Visible = false;
                }
            }
        }
        /// <summary>
        /// 初始化编辑数据
        /// </summary>
        protected void LoadData()
        {
            String strSql = "SELECT * FROM [T_Members] WHERE id=" + EditID;
            T_MembersEntity data = db.ExecuteObject<T_MembersEntity>(strSql);
            ViewState["data"] = data;
            this.SetFormValue(data);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            T_MembersEntity data = (T_MembersEntity)ViewState["data"];
            data = this.GetFormValue<T_MembersEntity>(data);
            if (!IsEdit)
            {
                IJsonResult msg = MemberCenter.RegMember(data);
                Alert(Label1, msg.Text, msg.Css);
                if (msg.Status)
                {
                    this.ClearFromValue();
                    AppendLogs("添加会员:" + data.UserName, LogsAction.Create);
                }
            }
            else
            {
                if (db.ExecuteCommand<T_MembersEntity>(data, CmdType.UPDATE))
                {
                    AppendLogs("修改会员:" + data.UserName, LogsAction.Edit);
                    Alert(Label1, "保存成功！", "line1px_3");
                }
            }
        }
    }
}