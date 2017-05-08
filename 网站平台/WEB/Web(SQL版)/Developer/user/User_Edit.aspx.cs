using System;
using System.Web.UI.WebControls;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class User_Edit : BaseAdmin
    {
        protected void Page_Load(Object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                JoinDate.SetValue(DateTime.Now);
                RoleID.DataSource = db.ExecuteDataTable("SELECT id,name FROM [T_UserRole]");
                RoleID.DataTextField = "name";
                RoleID.DataValueField = "id";
                RoleID.DataBind();
                RoleID.Items.Insert(0, new ListItem("选择角色", "0"));

                if (IsEdit)
                {
                    LoadData();
                    UserName.Enabled = false;
                    //Req1.Visible = Req2.Visible = false;
                }
            }
        }
        /// <summary>
        /// 初始化编辑数据
        /// </summary>
        protected void LoadData()
        {
            T_UserEntity data = db.ExecuteObject<T_UserEntity>("SELECT * FROM [T_User] WHERE id=" + EditID);
            ViewState["data"] = data;
            this.SetFormValue(data);

            if (Admin.ID == EditID) IsLock.Enabled = false;//不能禁用自己
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(Object sender, EventArgs e)
        {
            T_UserEntity data = (T_UserEntity)ViewState["data"];
            data = this.GetFormValue<T_UserEntity>(data);
            if (!IsEdit)
            {
                data.LastSignTime = data.JoinDate;
                data.Salt = TextHelper.RandomText(10);
            }

            CmdType cmd = IsEdit ? CmdType.UPDATE : CmdType.INSERT;
            if (db.ExecuteCommand<T_UserEntity>(data, cmd))
            {
                if (!IsEdit) UserName.Text = "";
                if (Admin.ID == EditID) Admin.Update();//如果是当前登录用户,更新用户数据
                Alert(Label1, "保存成功！", "line1px_3");

                String text = String.Format("{0}用户：{1}.", IsEdit ? "编辑" : "添加", data.UserName);
                AppendLogs(text, IsEdit ? LogsAction.Edit : LogsAction.Create);
            }
        }
    }
}