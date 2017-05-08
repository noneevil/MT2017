using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class Books : BaseAdmin
    {
        protected void Page_Load(Object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindField(dropField, "t_books", "title");
                BindData();
            }
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            List<String> list = new List<String>();
            String key = txtKey.Text;
            String starttime = starTime.Text;
            String endtime = endTime.Text;

            list.Add("0=0");
            //关键词
            if (!String.IsNullOrEmpty(key))
            {
                list.Add("a." + dropField.SelectedValue + " LIKE '%" + key + "%'");
            }
            //起始日期
            if (!String.IsNullOrEmpty(starttime))
            {
                list.Add("a.booktime>='" + starttime + "'");
            }
            //结束日期
            if (!String.IsNullOrEmpty(endtime))
            {
                DateTime time = Convert.ToDateTime(endtime).AddDays(1).AddSeconds(-1);
                list.Add("a.booktime<='" + time.ToString() + "'");
            }

            String filter = String.Join(" AND ", list.ToArray());
            String sql = "SELECT COUNT(a.id) FROM [t_books] AS a WHERE " + filter;

            String strSql = String.Format("SELECT TOP {0} a.*,b.membername FROM [t_books] AS a LEFT OUTER JOIN [t_members] AS b ON a.memberid=b.id WHERE (a.id NOT IN (SELECT TOP {1} a.id FROM [t_books] a WHERE {2} ORDER BY a.id DESC)) AND {2} ORDER BY a.id DESC", Pager1.PageSize, Pager1.PageSize * (Pager1.CurrentPageIndex - 1), filter);

            DataTable dt = db.ExecuteDataTable(strSql);
            Pager1.RecordCount = Convert.ToInt32(db.ExecuteScalar(sql));
            Repeater1.DataSource = dt;
            Repeater1.DataBind();
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
            switch (e.CommandName)
            {
                case "del":
                    db.ExecuteCommand(String.Format("DELETE FROM [t_books] WHERE id in ({0})", id));
                    AppendLogs("删除留言:" + name.Value, LogsAction.Delete);
                    break;
                case "status":
                    ExecuteObject obj = new ExecuteObject();
                    obj.cmdtype = CmdType.UPDATE;
                    obj.tableName = "t_books";
                    obj.terms.Add("id", id);
                    obj.cells.Add(e.CommandName, !Convert.ToBoolean(e.CommandArgument));
                    db.ExecuteCommand(obj);
                    AppendLogs("修改留言状态:" + name.Value, LogsAction.Edit);
                    break;
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
                ImageButton btnstatus = (ImageButton)e.Item.FindControl("status");
                HtmlInputImage btndetail = (HtmlInputImage)e.Item.FindControl("detail");

                btndetail.Attributes.Add("onclick", String.Format("javascript:dialogIFrame({{url:'Books_Edit.aspx?id={0}',title:'详细 - {1}',width:630, height: 450}});return false;", id, dv["title"].ToString()));
                if (!String.IsNullOrEmpty(dv["ReContent"].ToString())) btndetail.Src = "../skin/icos/mail_open.png";

                if ((Boolean)dv["status"]) btnstatus.ImageUrl = "../skin/icos/checkbox_yes.png";
                btndel.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 且无法恢复!确定要删除吗?'}});return false;", dv["Title"].ToString());
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
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Pager1.CurrentPageIndex = 1;
            BindData();
        }
        /// <summary>
        /// 批量删除 批量启用　批量禁用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Int32 arguments = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
            foreach (RepeaterItem item in Repeater1.Items)
            {
                CheckBox chkid = (CheckBox)item.FindControl("id");
                HiddenField name = (HiddenField)item.FindControl("title");
                if (chkid.Checked)
                {
                    switch (arguments)
                    {
                        case 0://禁用
                            db.ExecuteCommand(String.Format("UPDATE [t_books] SET status = 0 WHERE id = {0}", chkid.Text));
                            AppendLogs("禁用留言:" + name.Value, LogsAction.Edit);
                            break;
                        case -1://删除
                            db.ExecuteCommand(String.Format("DELETE FROM [t_books] WHERE id = {0}", chkid.Text));
                            AppendLogs("删除留言:" + name.Value, LogsAction.Delete);
                            break;
                        case 1://启用
                            db.ExecuteCommand(String.Format("UPDATE [t_books] SET status = 1 WHERE id = {0}", chkid.Text));
                            AppendLogs("启用留言:" + name.Value, LogsAction.Edit);
                            break;
                    }
                }
            }
            BindData();
        }
    }
}