using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Core;

namespace WebSite.Web
{
    public partial class Logs : BaseAdmin
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
            List<String> list = new List<String>();
            String starttime = starTime.Text;
            String endtime = endTime.Text;
            list.Add("0=0");

            //起始日期
            if (!String.IsNullOrEmpty(starttime))
            {
                list.Add("a.pubdate>='" + starttime + "'");
            }
            //结束日期
            if (!String.IsNullOrEmpty(endtime))
            {
                DateTime time = Convert.ToDateTime(endtime).AddDays(1).AddSeconds(-1);
                list.Add("a.pubdate<='" + time.ToString() + "'");
            }
            String filter = String.Join(" AND ", list.ToArray());
            String _sql = "SELECT count(id) FROM [T_Logs]";
            String sql = String.Format("SELECT TOP {0} a.*, b.username FROM [T_Logs] AS a LEFT JOIN [T_User] AS b ON a.[userid] = b.id WHERE (a.id NOT IN (SELECT TOP {1} a.id FROM [T_Logs] AS a WHERE {2} ORDER BY a.id DESC)) AND {2} ORDER BY a.id DESC", Pager1.PageSize, Pager1.PageSize * (Pager1.CurrentPageIndex - 1), filter);

            DataTable dt = db.ExecuteDataTable(sql);
            Pager1.RecordCount = Convert.ToInt32(db.ExecuteScalar(_sql));
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
            String ID = ((CheckBox)e.Item.FindControl("ID")).Text;
            if (e.CommandName == "del")
            {
                db.ExecuteCommand(String.Format("DELETE FROM [T_Logs] WHERE id in ({0})", ID));
                T_LogsHelper.Append("删除操作日志.", Admin.ID);
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
                ImageButton del = e.Item.FindControl("del") as ImageButton;
                del.OnClientClick = "javascript:dialogConfirm({{el:this,text:'将删除操作日志且无法恢复!确定要删除吗?'}});return false;";
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
            if (arg == -2)
            {
                db.ExecuteCommand("DELETE FROM [t_logs]");
                T_LogsHelper.Append("清空操作日志.", Admin.ID);
            }
            else if (arg == -1)
            {
                List<Int32> list = new List<Int32>();
                foreach (RepeaterItem item in Repeater1.Items)
                {
                    CheckBox chkbox = item.FindControl("id") as CheckBox;
                    if (chkbox.Checked) list.Add(Convert.ToInt32(chkbox.Text));
                }

                if (list.Count > 0)
                {
                    String id = String.Join(",", list.ToArray());
                    db.ExecuteCommand(String.Format("DELETE FROM [t_logs] WHERE id in ({0})", id));
                    T_LogsHelper.Append("批量操作日志.", Admin.ID);
                }
            }
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
    }
}