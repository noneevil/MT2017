using System;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class Books_Edit : BaseAdmin
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
            String sql = "SELECT a.*,b.membername FROM [t_books] AS a LEFT OUTER JOIN [t_members] AS b ON a.memberid = b.id WHERE a.id=" + EditID;
            T_BooksEntity data = db.ExecuteObject<T_BooksEntity>(sql);
            ViewState["data"] = data;
            this.SetFormValue(data);
            if (String.IsNullOrEmpty(data.ReContent)) ReTime.Text = DateTime.Now.ToString();
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            T_BooksEntity data = (T_BooksEntity)ViewState["data"];
            data = this.GetFormValue<T_BooksEntity>(data);
            data.ReTime = DateTime.Now;
            CmdType cmd = IsEdit ? CmdType.UPDATE : CmdType.INSERT;
            
            if (db.ExecuteCommand<T_BooksEntity>(data, cmd))
            {
                Alert(Label1, "回复成功！", "line1px_3");
            }
        }
    }
}