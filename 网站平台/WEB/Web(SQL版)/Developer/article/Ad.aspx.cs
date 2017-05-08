using System;
using System.Data;
using System.Web.UI.WebControls;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class Ad : BaseAdmin
    {
        protected void Page_Load(Object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            String strSql = "SELECT * FROM [t_ad] ORDER BY id DESC";
            DataTable dt = db.ExecuteDataTable(strSql);
            Repeater1.BindPage(Pager1, dt.DefaultView, dt.Rows.Count);
        }
        /// <summary>
        /// 列表事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String id = ((CheckBox)e.Item.FindControl("ID")).Text;
            HiddenField name = (HiddenField)e.Item.FindControl("title");
            if (e.CommandName == "del")
            {
                db.ExecuteCommand(String.Format("DELETE FROM [T_Ad] WHERE id in = {0}", id));
                AppendLogs("删除广告:" + name.Value, LogsAction.Delete);
            }
            BindData();
        }
        /// <summary>
        /// 控件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dv = e.Item.DataItem as DataRowView;
                Int32 id = Convert.ToInt32(dv["id"].ToString());
                ImageButton btndel = (ImageButton)e.Item.FindControl("del");

                btndel.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 且无法恢复!确定要删除吗?'}});return false;", dv["title"].ToString());

                AdCategory adtype = (AdCategory)Enum.Parse(typeof(AdCategory), dv["groupid"].ToString(), true);
                PlaceHolder holder1 = (PlaceHolder)e.Item.FindControl("PlaceHolder1");
                PlaceHolder holder2 = (PlaceHolder)e.Item.FindControl("PlaceHolder2");
                if (adtype == AdCategory.FLV视频)
                {
                    holder1.Visible = false;
                    holder2.Visible = true;
                }
            }
        }
        /// <summary>
        /// 分页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Pager1_PageChanged(object sender, EventArgs e)
        {
            BindData();
        }
        /// <summary>
        /// 批量删除 批量启用　批量禁用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Int32 arg = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
            foreach (RepeaterItem item in Repeater1.Items)
            {
                CheckBox chkid = (CheckBox)item.FindControl("id");
                HiddenField name = (HiddenField)item.FindControl("title");
                if (chkid.Checked)
                {
                    if (arg == -1)
                    {
                        db.ExecuteCommand(String.Format("DELETE FROM [T_Ad] WHERE id in = {0}", chkid.Text));
                        AppendLogs("删除广告:" + name.Value, LogsAction.Delete);
                    }
                }
            }
            BindData();
        }
    }
}