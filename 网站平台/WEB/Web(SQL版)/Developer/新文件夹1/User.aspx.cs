using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using CommonUtils;

namespace WebSite.Web
{
    public partial class User : BaseAdmin
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
            String sql = "SELECT a.*,b.name FROM [T_User] AS a LEFT JOIN [t_userrole] AS b ON a.roleid = b.id";
            if (!Admin.IsSuper) sql += " WHERE a.Super=0";

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
            Int32 ID = Convert.ToInt32(((CheckBox)e.Item.FindControl("ID")).Text);
            if (e.CommandName == "del")
            {
                db.ExecuteCommand(String.Format("DELETE FROM [T_User] WHERE id in ({0})", ID));
            }
            else if (e.CommandName == "status")
            {
                ImageButton status = e.Item.FindControl("status") as ImageButton;
                ExecuteObject obj = new ExecuteObject();
                obj.cmdtype = CmdType.UPDATE;
                obj.tableName = "T_User";
                obj.terms.Add("id", ID);
                obj.cells.Add("status", Convert.ToBoolean(status.CommandArgument));
                db.ExecuteCommand(obj);
            }
            else if (e.CommandName == "reset")
            {
                String pwd = TextHelper.RandomText(8).ToLower();
                ExecuteObject obj = new ExecuteObject();
                obj.cmdtype = CmdType.UPDATE;
                obj.tableName = "T_User";
                obj.terms.Add("id", ID);
                obj.cells.Add("password", EncryptHelper.MD5Upper32(pwd));
                db.ExecuteCommand(obj);
                Alert("重置成功！<br/>请牢记密码：" + pwd, "success");
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
                ImageButton status = e.Item.FindControl("status") as ImageButton;
                ImageButton reset = e.Item.FindControl("reset") as ImageButton;

                if (Admin.ID == Convert.ToInt32(dv["id"]))
                {
                    del.ImageUrl = "images/icos/del_disabled.gif";
                    del.OnClientClick = "javascript:return false;";

                    status.ImageUrl = "images/icos/checkbox_disabled.png";
                    status.OnClientClick = "javascript:return false;";

                    reset.ImageUrl = "images/icos/lock_disable.gif";
                    reset.OnClientClick = "javascript:return false;";
                }
                else
                {
                    Boolean chk = Convert.ToBoolean(dv["status"]);
                    status.CommandArgument = Convert.ToString(!chk);
                    if (chk) status.ImageUrl = "images/icos/checkbox_yes.png";
                    del.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 且无法恢复!确定要删除吗?'}});return false;", dv["UserName"]);
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
                CheckBox chk = item.FindControl("id") as CheckBox;
                if (chk.Checked) list.Add(Convert.ToInt32(chk.Text));
            }
            list.Remove(Admin.ID);
            if (list.Count > 0)
            {
                String id = String.Join(",", list.ToArray());
                Int32 arg = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
                switch (arg)
                {
                    case 0://禁用
                        db.ExecuteCommand(String.Format("UPDATE [T_User] SET status = 0 WHERE id IN({0})", id));
                        break;
                    case -1://删除
                        db.ExecuteCommand(String.Format("DELETE FROM [T_User] WHERE id in ({0})", id));
                        break;
                    case 1://启用
                        db.ExecuteCommand(String.Format("UPDATE [T_User] SET status = 1 WHERE id IN({0})", id));
                        break;
                }
            }
            BindData();
        }
    }
}