using System;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class Role_Edit : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (IsEdit)
                {
                    LoadData();
                }
            }
        }
        /// <summary>
        /// 初始化编辑数据
        /// </summary>
        protected void LoadData()
        {
            String strSql = "SELECT * FROM [T_UserRole] WHERE id=" + EditID;
            T_UserRoleEntity data = db.ExecuteObject<T_UserRoleEntity>(strSql);
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
            T_UserRoleEntity data = (T_UserRoleEntity)ViewState["data"];
            data = this.GetFormValue<T_UserRoleEntity>(data);
            CmdType cmd = IsEdit ? CmdType.UPDATE : CmdType.INSERT;
            if (db.ExecuteCommand<T_UserRoleEntity>(data, cmd))
            {
                if (!IsEdit)
                {
                    this.ClearFromValue();
                }

                if (EditID == Admin.RoleID)
                {
                    Admin.Update();
                }
                Alert(Label1, "保存成功！", "line1px_3");

                String text = String.Format("{0}角色：{1}.", IsEdit ? "编辑" : "添加", data.Name);
                AppendLogs(text, IsEdit ? LogsAction.Edit : LogsAction.Create);
            }
        }
    }
}