using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using MSSQLDB;
using WebSite.BackgroundPages;
using CommonUtils;

namespace WebSite.Web
{
    public partial class Menu : BaseAdmin
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
            String sql = "SELECT a.*, IsNull(b.menuName ,'顶级分类') AS pName FROM [T_SiteMenu] AS a LEFT JOIN [T_SiteMenu] AS b ON a.[ParentID] = b.id ORDER BY a.parentid,a.sort";
            DataTable dt = db.ExecuteDataTable(sql);

            Repeater1.BindPage(Pager1, dt.DefaultView, dt.Rows.Count);
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
                db.ExecuteCommand(String.Format("DELETE FROM [t_sitemenu] WHERE id in ({0})", ID));
            }
            else if (e.CommandName == "status")
            {
                ImageButton status = e.Item.FindControl("status") as ImageButton;
                ExecuteObject obj = new ExecuteObject();
                obj.cmdtype = CmdType.UPDATE;
                obj.tableName = "t_sitemenu";
                obj.terms.Add("id", ID);
                obj.cells.Add("status", Convert.ToBoolean(status.CommandArgument));
                db.ExecuteCommand(obj);
            }
            else
            {
                DataTable ds = db.ExecuteDataTable("SELECT id,Sort FROM [t_sitemenu] WHERE ParentID=" + e.CommandArgument + " ORDER BY Sort");
                switch (e.CommandName)
                {
                    case "up":
                        for (int i = 0; i < ds.Rows.Count; i++)
                        {
                            DataRow dr = ds.Rows[i];
                            if (dr["id"].ToString() == ID)
                            {
                                int num = Convert.ToInt16(dr["Sort"].ToString());
                                if (num == 0 && i == 0) break;
                                dr["Sort"] = num - 1;
                                if (i > 0)
                                {
                                    ds.Rows[i - 1]["Sort"] = Convert.ToInt16(ds.Rows[i - 1]["Sort"].ToString()) + 1;
                                }
                                break;
                            }
                        }
                        break;
                    case "down":
                        for (int i = 0; i < ds.Rows.Count; i++)
                        {
                            DataRow dr = ds.Rows[i];
                            if (dr["id"].ToString() == ID)
                            {
                                int num = Convert.ToInt16(dr["Sort"].ToString());
                                dr["Sort"] = num + 1;
                                if (i < ds.Rows.Count - 1)
                                {
                                    ds.Rows[i + 1]["Sort"] = Convert.ToInt16(ds.Rows[i + 1]["Sort"].ToString()) - 1;
                                }
                                break;
                            }
                        }
                        break;
                }
                ds.DefaultView.Sort = "Sort asc";
                ds = ds.DefaultView.ToTable();
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    db.ExecuteCommand(String.Format("UPDATE [t_sitemenu] SET Sort={1} WHERE id ={0} ", ds.Rows[i]["id"], i));
                }
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

                if (ID == 1 || ID == 16)
                {
                    del.OnClientClick = "javascript:return false;";
                    del.ImageUrl = "images/icos/del_disabled.gif";

                    status.OnClientClick = "javascript:return false;";
                    status.ImageUrl = "images/icos/checkbox_disabled.png";
                }
                else
                {
                    Boolean chk = Convert.ToBoolean(dv["status"]);
                    status.CommandArgument = Convert.ToString(!chk);
                    if (chk) status.ImageUrl = "images/icos/checkbox_yes.png";

                    del.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 且无法恢复!确定要删除吗?'}});return false;", dv["menuname"]);
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
            List<Int32> list = new List<Int32>();
            foreach (RepeaterItem item in Repeater1.Items)
            {
                CheckBox chkbox = item.FindControl("id") as CheckBox;
                if (chkbox.Checked) list.Add(Convert.ToInt32(chkbox.Text));
            }
            list.Remove(1);
            list.Remove(16);

            if (list.Count > 0)
            {
                String id = String.Join(",", list.ToArray());
                Int32 arg = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
                switch (arg)
                {
                    case 0://禁用
                        db.ExecuteCommand(String.Format("UPDATE [t_sitemenu] SET status = 0 WHERE id IN({0})", id));
                        break;
                    case -1://删除
                        db.ExecuteCommand(String.Format("DELETE FROM [t_sitemenu] WHERE id in ({0})", id));
                        break;
                    case 1://启用
                        db.ExecuteCommand(String.Format("UPDATE [t_sitemenu] SET status = 1 WHERE id IN({0})", id));
                        break;
                }
            }
            BindData();
        }
    }
}