using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class Logs : BaseAdmin
    {
        protected void Page_Load(Object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dropGroup.DataSource = Enum.GetValues(typeof(LogsAction));
                dropGroup.DataBind();
                dropGroup.Items.Insert(0, new ListItem("操作类型", ""));

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

            //操作类型
            if (dropGroup.SelectedValue != "")
            {
                Int32 value = (Int32)((LogsAction)Enum.Parse(typeof(LogsAction), dropGroup.SelectedValue, true));
                list.Add("actiontype=" + value);
            }
            //起始日期
            if (!String.IsNullOrEmpty(starttime))
            {
                list.Add("pubdate>='" + starttime + "'");
            }
            //结束日期
            if (!String.IsNullOrEmpty(endtime))
            {
                DateTime time = Convert.ToDateTime(endtime).AddDays(1).AddSeconds(-1);
                list.Add("pubdate<='" + time.ToString() + "'");
            }
            String filter = String.Join(" AND ", list.ToArray());
            String sql = "SELECT count(id) FROM [T_Logs] WHERE " + filter;
            String strSql = String.Format("SELECT TOP {0} * FROM [T_Logs] WHERE (id NOT IN (SELECT TOP {1} id FROM [T_Logs] WHERE {2} ORDER BY id DESC)) AND {2} ORDER BY id DESC", Pager1.PageSize, Pager1.PageSize * (Pager1.CurrentPageIndex - 1), filter);

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
            if (e.CommandName == "del")
            {
                //db.ExecuteCommand(String.Format("DELETE FROM [T_Logs] WHERE id = {0}", id));
                //AppendLogs("删除操作日志.", LogsAction.Delete);
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
                ImageButton btndel = (ImageButton)e.Item.FindControl("del");
                btndel.OnClientClick = "javascript:dialogConfirm({el:this,text:'将删除操作日志且无法恢复!确定要删除吗?'});return false;";
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
        protected void btnCommand_Click(object sender, EventArgs e)
        {
            Int32 arg = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
            if (arg == -2)
            {
                db.ExecuteCommand("DELETE FROM [T_Logs]");
                AppendLogs("清空操作日志.", LogsAction.Empty);
            }
            else if (arg == -1)
            {
                List<Int32> listid = new List<Int32>();
                foreach (RepeaterItem item in Repeater1.Items)
                {
                    CheckBox chkid = (CheckBox)item.FindControl("id");
                    if (chkid.Checked)
                        listid.Add(Convert.ToInt32(chkid.Text));
                }

                if (listid.Count > 0)
                {
                    String idcoll = String.Join(",", listid.ToArray());
                    db.ExecuteCommand(String.Format("DELETE FROM [T_Logs] WHERE id in ({0})", idcoll));
                    AppendLogs("批量操作日志.", LogsAction.Delete);
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