using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Core;

namespace WebSite.Web
{
    public partial class Member : BaseAdmin
    {
        protected void Page_Load(Object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindField(dropField, "t_members", "membername");
                BindData();
            }
        }
        private String tableName = "t_members";
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
                list.Add(dropField.SelectedValue + " LIKE '%" + key + "%'");
            }
            //起始日期
            if (!String.IsNullOrEmpty(starttime))
            {
                list.Add("joindate>='" + starttime + "'");
            }
            //结束日期
            if (!String.IsNullOrEmpty(endtime))
            {
                DateTime time = Convert.ToDateTime(endtime).AddDays(1).AddSeconds(-1);
                list.Add("joindate<='" + time.ToString() + "'");
            }

            String filter = String.Join(" AND ", list.ToArray());
            String _sql = "SELECT COUNT(id) FROM [t_members] WHERE " + filter;

            String sql = String.Format("SELECT TOP {0} * FROM [t_members] WHERE (id NOT IN (SELECT TOP {1} id FROM [t_members] WHERE {2} ORDER BY id DESC)) AND {2} ORDER BY id DESC", Pager1.PageSize, Pager1.PageSize * (Pager1.CurrentPageIndex - 1), filter);

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
            HiddenField membername = e.Item.FindControl("membername") as HiddenField;
            switch (e.CommandName)
            {
                case "del":
                    db.ExecuteCommand(String.Format("DELETE FROM [t_members] WHERE id in ({0})", ID));
                    T_LogsHelper.Append("删除会员:" + membername.Value, Admin.ID);
                    break;
                case "status":
                    ExecuteObject obj = new ExecuteObject();
                    obj.cmdtype = CmdType.UPDATE;
                    obj.tableName = "t_members";
                    obj.terms.Add("id", ID);
                    obj.cells.Add(e.CommandName, !Convert.ToBoolean(e.CommandArgument));
                    db.ExecuteCommand(obj);
                    T_LogsHelper.Append("修改会员状态:" + membername.Value, Admin.ID);
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
                ImageButton status = e.Item.FindControl("status") as ImageButton;
                Image ValidateMail = e.Item.FindControl("ValidateMail") as Image;
                Image ValidateMobile = e.Item.FindControl("ValidateMobile") as Image;

                if (Convert.ToBoolean(dv["status"])) status.ImageUrl = "images/icos/checkbox_yes.png";
                if (Convert.ToBoolean(dv["ValidateMail"]))
                {
                    ValidateMail.ToolTip = "已通过验证.";
                    ValidateMail.ImageUrl = "images/icos/email-enable.png";
                }
                if (Convert.ToBoolean(dv["ValidateMobile"]))
                {
                    ValidateMobile.ToolTip = "已通过验证.";
                    ValidateMobile.ImageUrl = "images/icos/mobile-enable.png";
                }
                
                del.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 且无法恢复!确定要删除吗?'}});return false;", dv["MemberName"]);
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
            List<String> members = new List<String>();
            foreach (RepeaterItem item in Repeater1.Items)
            {
                CheckBox chk = item.FindControl("id") as CheckBox;
                HiddenField member = item.FindControl("membername") as HiddenField;
                if (chk.Checked)
                {
                    list.Add(Convert.ToInt32(chk.Text));
                    members.Add(member.Value);
                }
            }

            if (list.Count > 0)
            {
                String id = String.Join(",", list.ToArray());
                Int32 arguments = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
                switch (arguments)
                {
                    case 0://禁用
                        db.ExecuteCommand(String.Format("UPDATE [t_members] SET status = 0 WHERE id IN({0})", id));
                        T_LogsHelper.Append("批量禁用会员:" + String.Join(",", members.ToArray()), Admin.ID);
                        break;
                    case -1://删除
                        db.ExecuteCommand(String.Format("DELETE FROM [t_members] WHERE id in ({0})", id));
                        T_LogsHelper.Append("批量删除会员:" + String.Join(",", members.ToArray()), Admin.ID);
                        break;
                    case 1://启用
                        db.ExecuteCommand(String.Format("UPDATE [t_members] SET status = 1 WHERE id IN({0})", id));
                        T_LogsHelper.Append("批量启用会员:" + String.Join(",", members.ToArray()), Admin.ID);
                        break;
                }
            }
            BindData();
        }
    }
}