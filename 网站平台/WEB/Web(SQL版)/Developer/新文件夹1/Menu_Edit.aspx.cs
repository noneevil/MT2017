using System;
using System.Web.UI.WebControls;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class Menu_Edit : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                String sql = String.Format("SELECT id,menuname FROM [t_sitemenu] WHERE ParentID=0 AND id<>{0} ORDER BY id", EditID);
                parentid.DataSource = db.ExecuteDataTable(sql);
                parentid.DataTextField = "menuname";
                parentid.DataValueField = "id";
                parentid.DataBind();
                parentid.Items.Insert(0, new ListItem("顶级分类", "0"));

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
            String sql = "SELECT * FROM [t_sitemenu] WHERE id=" + EditID;
            T_SiteMenuEntity data = db.ExecuteObject<T_SiteMenuEntity>(sql);
            ViewState["data"] = data;
            this.SetFormValue(data);
            if (data.ParentID == 0 || data.ID == 16) @internal.Visible = false;
            if (data.ParentID != 0) open.Visible = false;
            if (data.ID == 1 || data.ID == 16) status.Enabled = false;
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            T_SiteMenuEntity data = (T_SiteMenuEntity)ViewState["data"];
            data = this.GetFormValue<T_SiteMenuEntity>(data);
            CmdType cmd = IsEdit ? CmdType.UPDATE : CmdType.INSERT;
            if (!IsEdit) data.SortID = 9999;
            if (db.ExecuteCommand<T_SiteMenuEntity>(data, cmd))
            {
                if (!IsEdit) this.ClearFromValue();
                Alert(Label1, "保存成功！", "line1px_3");
            }
        }
    }
}