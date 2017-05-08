using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Models;

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
            String strSql = "SELECT a.*,b.name FROM [T_User] AS a LEFT JOIN [T_UserRole] AS b ON a.roleid = b.id";
            if (!Admin.IsSuper)
                strSql += " WHERE a.issuper=0";

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
            Int32 id = Convert.ToInt32(((CheckBox)e.Item.FindControl("ID")).Text);
            String name = ((HiddenField)e.Item.FindControl("hidName")).Value;
            if (e.CommandName == "del")
            {
                String sql = String.Format("DELETE FROM [T_User] WHERE id = {0}", id);
                db.ExecuteCommand(sql);

                AppendLogs("删除用户：" + name, LogsAction.Delete);
            }
            else if (e.CommandName == "lock")
            {
                ImageButton btnlock = (ImageButton)e.Item.FindControl("islock");
                ExecuteObject obj = new ExecuteObject();
                obj.cmdtype = CmdType.UPDATE;
                obj.tableName = "T_User";
                obj.terms.Add("id", id);
                obj.cells.Add("islock", btnlock.CommandArgument);
                db.ExecuteCommand(obj);

                AppendLogs("修改用户状态：" + name, LogsAction.Edit);
            }
            else if (e.CommandName == "reset")
            {
                String pwd = TextHelper.RandomText(8).ToLower();
                ExecuteObject obj = new ExecuteObject();
                obj.cmdtype = CmdType.UPDATE;
                obj.tableName = "T_User";
                obj.terms.Add("id", id);
                obj.cells.Add("userpass", EncryptHelper.MD5Upper32(pwd));
                db.ExecuteCommand(obj);
                Alert("重置成功！<br/>请牢记密码：" + pwd, "success");

                AppendLogs("重置用户密码：" + name, LogsAction.Edit);
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
                DataRowView data = e.Item.DataItem as DataRowView;
                ImageButton btndel = (ImageButton)e.Item.FindControl("del");
                ImageButton btnlock = (ImageButton)e.Item.FindControl("islock");
                ImageButton btnreset = (ImageButton)e.Item.FindControl("reset");

                if (Admin.ID == Convert.ToInt32(data["id"].ToString()))
                {
                    btndel.ImageUrl = "../skin/icos/del_disabled.gif";
                    btndel.OnClientClick = "javascript:return false;";

                    btnlock.OnClientClick = "javascript:return false;";

                    btnreset.ImageUrl = "../skin/icos/lock_disable.gif";
                    btnreset.OnClientClick = "javascript:return false;";
                }
                else
                {
                    Boolean val = Convert.ToBoolean(data["islock"]);
                    btnlock.CommandArgument = val ? "0" : "1";
                    if (val) btnlock.ImageUrl = "../skin/icos/checkbox_yes.png";
                    btndel.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 且无法恢复!确定要删除吗?'}});return false;", data["UserName"].ToString());
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
        protected void btnCommand_Click(object sender, EventArgs e)
        {
            List<Int32> listid = new List<Int32>();
            List<String> listname = new List<String>();
            foreach (RepeaterItem item in Repeater1.Items)
            {
                CheckBox chkid = (CheckBox)item.FindControl("ID");
                if (chkid.Checked)
                {
                    Int32 id = Convert.ToInt32(chkid.Text);
                    if (id != Admin.ID)
                    {
                        String name = ((HiddenField)item.FindControl("hidName")).Value;
                        listid.Add(id);
                        listname.Add(name);
                    }
                }
            }
            if (listid.Count > 0)
            {
                String idcoll = String.Join(",", listid.ToArray());
                String namecoll = String.Join("；", listname.ToArray());
                Int32 arg = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
                switch (arg)
                {
                    case 0://禁用
                        db.ExecuteCommand(String.Format("UPDATE [T_User] SET islock = 1 WHERE id IN({0})", idcoll));
                        AppendLogs("批量禁用用户：" + namecoll, LogsAction.Edit);
                        break;
                    case -1://删除
                        db.ExecuteCommand(String.Format("DELETE FROM [T_User] WHERE id in ({0})", idcoll));
                        AppendLogs("批量删除用户：" + namecoll, LogsAction.Delete);
                        break;
                    case 1://启用
                        db.ExecuteCommand(String.Format("UPDATE [T_User] SET islock = 0 WHERE id IN({0})", idcoll));
                        AppendLogs("批量启用用户：" + namecoll, LogsAction.Edit);
                        break;
                }
            }
            BindData();
        }
    }
}