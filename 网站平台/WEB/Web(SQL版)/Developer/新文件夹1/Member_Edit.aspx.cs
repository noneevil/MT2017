using System;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Models;
using WebSite.UserCenter;
using WebSite.Interface;

namespace WebSite.Web
{
    public partial class Member_Edit : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                membertype.DataSource = Enum.GetValues(typeof(MemberType));
                membertype.DataBind();

                if (IsEdit)
                {
                    LoadData();
                    membername.Enabled = false;
                    RequiredPassword.Visible = false;
                }
                else
                {
                    sex.SelectedIndex = 1;
                    DateTime time = DateTime.Now;
                    joindate.SetValue(time);
                    lastsigntime.SetValue(time);
                    birthday.SetValue(time.AddYears(-20));
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
        /// 编辑ID
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
            String sql = "SELECT * FROM [t_members] WHERE id=" + EditID;
            T_MembersEntity data = db.ExecuteObject<T_MembersEntity>(sql);
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
                if (msg.Status) this.ClearFromValue();
            }
            else
            {
                if (db.ExecuteCommand<T_MembersEntity>(data, CmdType.UPDATE))
                {
                    Alert(Label1, "保存成功！", "line1px_3");
                }
            }
        }
    }
}