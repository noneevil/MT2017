using System;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Core;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class Ad_Edit : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                groupid.DataSource = Enum.GetValues(typeof(AdCategory));
                groupid.DataBind();
                target.DataSource = Enum.GetValues(typeof(TargetCategory));
                target.DataBind();

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
            String sql = "SELECT * FROM [t_ad] WHERE id=" + EditID;
            T_AdEntity data = db.ExecuteObject<T_AdEntity>(sql);
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
            T_AdEntity data = (T_AdEntity)ViewState["data"];
            data = this.GetFormValue<T_AdEntity>(data);
            CmdType cmd = IsEdit ? CmdType.UPDATE : CmdType.INSERT;

            if (db.ExecuteCommand<T_AdEntity>(data, cmd))
            {
                if (!IsEdit)
                {
                    this.ClearFromValue();
                    T_LogsHelper.Append("添加广告:" + data.Title, Admin.ID);
                }
                else
                {
                    T_LogsHelper.Append("编辑广告:" + data.Title, Admin.ID);
                }
                Alert(Label1, "保存成功！", "line1px_3");
            }
        }
    }
}