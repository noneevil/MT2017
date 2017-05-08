using System;
using System.Data;
using System.Web.UI.WebControls;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Models;
using Newtonsoft.Json;

namespace WebSite.Web
{
    public partial class User_Edit : BaseAdmin
    {
        protected void Page_Load(Object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                JoinDate.SetValue(DateTime.Now);
                RoleID.DataSource = db.ExecuteDataTable("SELECT id,name FROM [t_userrole]");
                RoleID.DataTextField = "name";
                RoleID.DataValueField = "id";
                RoleID.DataBind();
                RoleID.Items.Insert(0, new ListItem("选择角色", "0"));

                if (IsEdit)
                {
                    LoadData();

                    UserName.Enabled = false;
                    Req1.Visible = Req2.Visible = false;
                }
            }
        }
        /// <summary>
        /// 是否是编辑模式
        /// </summary>
        protected Boolean IsEdit
        {
            get { return EditID > 0; }
        }
        /// <summary>
        /// 编号ID
        /// </summary>
        protected Int32 EditID
        {
            get
            {
                Int32 outid = 0;
                Int32.TryParse(Request["id"], out outid);
                return outid;
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

            if (Admin.ID == EditID) Status.Enabled = false;
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(Object sender, EventArgs e)
        {
            if (!IsEdit && ChkUser(UserName.Text))
            {
                Alert(Label1, "用户已经存在！", "line1px_2");
            }
            else
            {
                T_UserEntity data = (T_UserEntity)ViewState["data"];
                data = this.GetFormValue<T_UserEntity>(data);
                if (!IsEdit) data.LastSignTime = data.JoinDate;
                CmdType cmd = IsEdit ? CmdType.UPDATE : CmdType.INSERT;

                if (db.ExecuteCommand<T_UserEntity>(data, cmd))
                {
                    if (!IsEdit) UserName.Text = "";
                    if (Admin.ID == EditID) Admin.Update();//如果是当前登录用户,更新用户数据
                    Alert(Label1, "保存成功！", "line1px_3");
                }
            }
        }
        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <returns></returns>
        private Boolean ChkUser(String user)
        {
            ExecuteObject obj = new ExecuteObject();
            obj.tableName = "T_User";
            obj.terms.Add("username", user.Trim());
            obj.cells.Add("id", null);
            Object val = db.ExecuteScalar(obj);
            Int32 id = Convert.ToInt32(val);
            return id > 0;
        }
    }
}