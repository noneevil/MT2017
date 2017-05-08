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
        /// 初始化编辑数据
        /// </summary>
        protected void LoadData()
        {
            String strSql = "SELECT a.*,b.membername FROM [T_Books] AS a LEFT OUTER JOIN [T_Members] AS b ON a.memberid = b.id WHERE a.id=" + EditID;
            T_BooksEntity data = db.ExecuteObject<T_BooksEntity>(strSql);
            ViewState["data"] = data;
            this.SetFormValue(data);

            if (String.IsNullOrEmpty(data.ReContent))
                ReTime.Text = DateTime.Now.ToString();
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
                if (IsEdit)
                {
                    AppendLogs("回复留言:" + data.Title, LogsAction.Edit);
                }
                Alert(Label1, "回复成功！", "line1px_3");
            }
        }
    }
}