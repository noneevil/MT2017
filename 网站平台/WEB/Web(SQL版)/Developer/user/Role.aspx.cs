using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Interface;
using WebSite.Models;

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
            String strSql = "SELECT * FROM [T_UserRole]";
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
                db.ExecuteCommand(String.Format("DELETE FROM [T_UserRole] WHERE id in ({0})", id));
                db.ExecuteCommand(String.Format("DELETE FROM [T_AccessControl] WHERE role in ({0})", id));

                AppendLogs("删除角色：" + name, LogsAction.Delete);

                CacheHelper.Delete(ISessionKeys.cache_table_accesscontrol);
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
                Int32 id = Convert.ToInt32(data["id"].ToString());
                if (id == 1 || Admin.RoleID == id)
                {
                    btndel.OnClientClick = "javascript:return false;";
                    btndel.ImageUrl = "../skin/icos/del_disabled.gif";
                }
                else
                {
                    btndel.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 且无法恢复!确定要删除吗?'}});return false;", data["name"].ToString());
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
        protected void btnCommand_Click(object sender, EventArgs e)
        {
            List<Int32> listid = new List<Int32>();
            List<String> listname = new List<String>();

            foreach (RepeaterItem item in Repeater1.Items)
            {
                CheckBox chkid = (CheckBox)item.FindControl("id");
                String name = ((HiddenField)item.FindControl("hidName")).Value;
                if (chkid.Checked)
                {
                    Int32 id = Convert.ToInt32(chkid.Text);
                    if (id == Admin.RoleID || id == 1) continue;

                    listid.Add(id);
                    listname.Add(name);
                }
            }

            if (listid.Count > 0)
            {
                String idcoll = String.Join(",", listid.ToArray());
                Int32 arg = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
                switch (arg)
                {
                    case -1://删除
                        db.ExecuteCommand(String.Format("DELETE FROM [T_UserRole] WHERE id in ({0})", idcoll));
                        db.ExecuteCommand(String.Format("DELETE FROM [T_AccessControl] WHERE role in ({0})", idcoll));
                        AppendLogs("批量删除角色：" + String.Join("；", listname.ToArray()), LogsAction.Delete);
                        break;
                }
                CacheHelper.Delete(ISessionKeys.cache_table_accesscontrol);
            }
            BindData();
        }
    }
}