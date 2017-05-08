using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Core;

namespace WebSite.Web
{
    public partial class Comment : BaseAdmin
    {
        protected void Page_Load(Object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindField(dropField, "t_comment","title");
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
                list.Add("a.joindate>='" + starttime + "'");
            }
            //结束日期
            if (!String.IsNullOrEmpty(endtime))
            {
                DateTime time = Convert.ToDateTime(endtime).AddDays(1).AddSeconds(-1);
                list.Add("a.joindate<='" + time.ToString() + "'");
            }

            String filter = String.Join(" AND ", list.ToArray());
            String _sql = "SELECT COUNT(a.id) FROM [t_comment] AS a WHERE " + filter;

            String sql = String.Format("SELECT TOP {0} a.*,b.membername FROM [t_comment] AS a LEFT OUTER JOIN [t_members] AS b ON a.memberid=b.id WHERE (a.id NOT IN (SELECT TOP {1} a.id FROM [t_comment] a WHERE {2} ORDER BY a.id DESC)) AND {2} ORDER BY a.id DESC", Pager1.PageSize, Pager1.PageSize * (Pager1.CurrentPageIndex - 1), filter);

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
            HiddenField title = e.Item.FindControl("title") as HiddenField;
            switch (e.CommandName)
            {
                case "del":
                    db.ExecuteCommand(String.Format("DELETE FROM [t_comment] WHERE id in ({0})", ID));
                    T_LogsHelper.Append("删除评论:" + title.Value, Admin.ID);
                    break;
                case "audit":
                    ExecuteObject obj = new ExecuteObject();
                    obj.cmdtype = CmdType.UPDATE;
                    obj.tableName = "t_comment";
                    obj.terms.Add("id", ID);
                    obj.cells.Add(e.CommandName, !Convert.ToBoolean(e.CommandArgument));
                    db.ExecuteCommand(obj);
                    T_LogsHelper.Append("修改评论审核:" + title.Value, Admin.ID);
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
                Int32 ID = Convert.ToInt32(dv["id"]);

                ImageButton del = e.Item.FindControl("del") as ImageButton;
                ImageButton audit = e.Item.FindControl("audit") as ImageButton;

                HtmlInputImage detail = e.Item.FindControl("detail") as HtmlInputImage;
                detail.Attributes.Add("onclick", String.Format("javascript:dialogIFrame({{url:'Comment_Edit.aspx?id={0}',title:'详细 - {1}',width:500, height: 400}});return false;", ID, dv["title"]));

                if (Convert.ToBoolean(dv["audit"])) audit.ImageUrl = "images/icos/checkbox_yes.png";
                del.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 且无法恢复!确定要删除吗?'}});return false;", dv["Title"]);
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
            BindData();
        }
        /// <summary>
        /// 批量删除 批量启用　批量禁用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<Int32> list = new List<Int32>();
            List<String> titles = new List<String>();
            foreach (RepeaterItem item in Repeater1.Items)
            {
                CheckBox chk = item.FindControl("id") as CheckBox;
                HiddenField title = item.FindControl("title") as HiddenField;
                if (chk.Checked)
                {
                    list.Add(Convert.ToInt32(chk.Text));
                    titles.Add(title.Value);
                }
            }

            if (list.Count > 0)
            {
                String id = String.Join(",", list.ToArray());
                Int32 arguments = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
                switch (arguments)
                {
                    case 0://批量未审核
                        db.ExecuteCommand(String.Format("UPDATE [t_comment] SET audit = 0 WHERE id IN({0})", id));
                        T_LogsHelper.Append("批量未通过审核:" + String.Join(",", titles.ToArray()), Admin.ID);
                        break;
                    case -1://删除
                        db.ExecuteCommand(String.Format("DELETE FROM [t_comment] WHERE id in ({0})", id));
                        T_LogsHelper.Append("批量删除评论:" + String.Join(",", titles.ToArray()), Admin.ID);
                        break;
                    case 1://批量通过审核
                        db.ExecuteCommand(String.Format("UPDATE [t_comment] SET audit = 1 WHERE id IN({0})", id));
                        T_LogsHelper.Append("批量通过审核:" + String.Join(",", titles.ToArray()), Admin.ID);
                        break;
                }
            }
            BindData();
        }
    }
}