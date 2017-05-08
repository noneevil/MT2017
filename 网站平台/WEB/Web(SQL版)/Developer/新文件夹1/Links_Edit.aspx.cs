using System;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Core;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class Links_Edit : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                groupid.DataSource = Enum.GetValues(typeof(LinkCategory));
                groupid.DataBind();

                if (IsEdit)
                {
                    LoadData();
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
            String sql = "SELECT * FROM [t_links] WHERE id=" + EditID;
            T_LinksEntity data = db.ExecuteObject<T_LinksEntity>(sql);
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
            T_LinksEntity data = (T_LinksEntity)ViewState["data"];
            data = this.GetFormValue<T_LinksEntity>(data);
            CmdType cmd = IsEdit ? CmdType.UPDATE : CmdType.INSERT;

            if (db.ExecuteCommand<T_LinksEntity>(data, cmd))
            {
                if (!IsEdit)
                {
                    this.ClearFromValue();
                    T_LogsHelper.Append("添加友情连接:" + data.LinkName, Admin.ID);
                }
                else
                {
                    T_LogsHelper.Append("编辑友情连接:" + data.LinkName, Admin.ID);
                }
                Alert(Label1, "保存成功！", "line1px_3");
            }
        }
    }
}