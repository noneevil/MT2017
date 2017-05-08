using System;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class Comment_Edit : BaseAdmin
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
            String sql = "SELECT a.*, b.membername FROM [T_Comment] AS a INNER JOIN [T_Members] AS b ON a.memberid = b.id WHERE a.id =" + EditID;
            T_CommentEntity data = db.ExecuteObject<T_CommentEntity>(sql);
            //ViewState["data"] = data;
            this.SetFormValue(data);
        }
    }
}