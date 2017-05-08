using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using MSSQLDB;
using WebSite.BackgroundPages;
using CommonUtils;

namespace WebSite.Web
{
    public partial class Role : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        protected void BindData()
        {
            String sql = "SELECT * FROM [t_userrole]";
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
                db.ExecuteCommand(String.Format("DELETE FROM [t_userrole] WHERE id in ({0})", ID));
                db.ExecuteCommand(String.Format("DELETE FROM [T_AccessControl] WHERE role in ({0})", ID));
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
                ImageButton del = e.Item.FindControl("del") as ImageButton;
                Int32 ID = Convert.ToInt32(dv["id"]);
                if (ID == 1 || Admin.RoleID == ID)
                {
                    del.OnClientClick = "javascript:return false;";
                    del.ImageUrl = "images/icos/del_disabled.gif";
                }
                else
                {
                    del.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 且无法恢复!确定要删除吗?'}});return false;", dv["name"]);
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
        /// 批量删除
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
            list.Remove(Admin.RoleID);
            if (list.Count > 0)
            {
                String id = String.Join(",", list.ToArray());
                Int32 arg = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
                switch (arg)
                {
                    case -1://删除
                        db.ExecuteCommand(String.Format("DELETE FROM [t_userrole] WHERE id in ({0})", id));
                        db.ExecuteCommand(String.Format("DELETE FROM [T_AccessControl] WHERE role in ({0})", id));
                        break;
                }
            }
            BindData();
        }
    }
}