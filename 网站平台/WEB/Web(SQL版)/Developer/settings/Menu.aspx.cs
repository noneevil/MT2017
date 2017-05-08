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
            String strSql = "SELECT * FROM [T_SiteMenu] ORDER BY sortid";
            DataTable dt = db.ExecuteDataTable(strSql);
            DataTable ds = dt.Clone();
            dt.SortTable(ds, 0);

            Repeater1.DataSource = ds;
            Repeater1.DataBind();
        }
        /// <summary>
        /// 列表事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Int32 id = Convert.ToInt32(((CheckBox)e.Item.FindControl("ID")).Text);
            String title = ((HiddenField)e.Item.FindControl("hidTitle")).Value;
            String strSql = String.Empty;
            if (e.CommandName == "del")
            {
                strSql = String.Format("SELECT id,title FROM [T_SiteMenu] WHERE (list LIKE '%,{0},%')", id);
                DataTable ds = db.ExecuteDataTable(strSql);
                foreach (DataRow dr in ds.Rows)
                {
                    String _id = dr["id"].ToString();
                    strSql = String.Format("DELETE FROM [T_SiteMenu] WHERE id = {0}", id);
                    db.ExecuteCommand(strSql);

                    strSql = String.Format("DELECT FROM [T_AccessControl] WHERE tablename='t_sitemenu' AND node={0}", id);
                    db.ExecuteCommand(strSql);

                    AppendLogs("删除菜单:" + dr["title"].ToString(), LogsAction.Delete);
                }
            }
            else if (e.CommandName == "enable")
            {
                ImageButton btnenable = (ImageButton)e.Item.FindControl("IsEnable");
                ExecuteObject obj = new ExecuteObject();
                obj.cmdtype = CmdType.UPDATE;
                obj.tableName = "T_SiteMenu";
                obj.terms.Add("id", id);
                obj.cells.Add("isenable", btnenable.CommandArgument);
                db.ExecuteCommand(obj);

                AppendLogs("修改菜单状态：" + title, LogsAction.Edit);
            }
            else
            {
                strSql = "SELECT id,sortid FROM [T_SiteMenu] WHERE parentid=" + e.CommandArgument + " ORDER BY sortid";
                DataTable ds = db.ExecuteDataTable(strSql);
                switch (e.CommandName)
                {
                    case "up":
                        {
                            for (Int32 i = 0; i < ds.Rows.Count; i++)
                            {
                                DataRow dr = ds.Rows[i];
                                if (Convert.ToInt32(dr["id"].ToString()) == id)
                                {
                                    Int32 num = Convert.ToInt32(dr["sortid"].ToString());
                                    if (num == 0 && i == 0) break;
                                    dr["sortid"] = num - 1;
                                    if (i > 0)
                                    {
                                        ds.Rows[i - 1]["sortid"] = Convert.ToInt32(ds.Rows[i - 1]["sortid"].ToString()) + 1;
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    case "down":
                        {
                            for (Int32 i = 0; i < ds.Rows.Count; i++)
                            {
                                DataRow dr = ds.Rows[i];
                                if (Convert.ToInt32(dr["id"].ToString()) == id)
                                {
                                    Int32 num = Convert.ToInt32(dr["sortid"].ToString());
                                    dr["sortid"] = num + 1;
                                    if (i < ds.Rows.Count - 1)
                                    {
                                        ds.Rows[i + 1]["sortid"] = Convert.ToInt16(ds.Rows[i + 1]["sortid"].ToString()) - 1;
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                }
                ds.DefaultView.Sort = "sortid asc";
                ds = ds.DefaultView.ToTable();
                for (Int32 i = 0; i < ds.Rows.Count; i++)
                {
                    ExecuteObject obj = new ExecuteObject();
                    obj.cmdtype = CmdType.UPDATE;
                    obj.tableName = "T_SiteMenu";
                    obj.terms.Add("id", ds.Rows[i]["id"]);
                    obj.cells.Add("sortid", i);
                    db.ExecuteCommand(obj);
                }

                AppendLogs("修改菜单排序：" + title, LogsAction.Edit);
            }

            CacheHelper.Delete(ISessionKeys.cache_table_accesscontrol);

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
                Int32 id = Convert.ToInt32(data["id"].ToString());
                Int32 layer = Convert.ToInt32(data["layer"].ToString());
                Literal liturl = (Literal)e.Item.FindControl("LitLinkUrl");
                ImageButton btndel = (ImageButton)e.Item.FindControl("del");
                ImageButton btnenable = (ImageButton)e.Item.FindControl("IsEnable");

                e.Item.SetStyleLayer(layer);

                if (id == 1 || id == 16)
                {
                    btndel.OnClientClick = "javascript:return false;";
                    btndel.ImageUrl = "../skin/icos/del_disabled.gif";

                    btnenable.OnClientClick = "javascript:return false;";
                    btnenable.ImageUrl = "../skin/icos/checkbox_disabled.png";
                }
                else
                {
                    Boolean val = Convert.ToBoolean(data["IsEnable"]);
                    btnenable.CommandArgument = val ? "0" : "1";
                    if (val) btnenable.ImageUrl = "../skin/icos/checkbox_yes.png";

                    btndel.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除功能:“{0}”及所属子功能且无法恢复!确定要删除吗?'}});return false;", data["title"].ToString());
                }
            }
        }
        /// <summary>
        /// 批量删除 批量启用　批量禁用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCommand_Click(object sender, EventArgs e)
        {
            List<Int32> listid = new List<Int32>();
            List<String> listtitle = new List<String>();
            foreach (RepeaterItem item in Repeater1.Items)
            {
                CheckBox chkid = (CheckBox)item.FindControl("id");
                if (chkid.Checked)
                {
                    String title = ((HiddenField)item.FindControl("hidTitle")).Value;
                    listtitle.Add(title);
                    listid.Add(Convert.ToInt32(chkid.Text));
                }
            }
            listid.Remove(1);
            listid.Remove(16);

            if (listid.Count > 0)
            {
                String idcoll = String.Join(",", listid.ToArray());
                String titlecoll = String.Join("；", listtitle.ToArray());
                Int32 arg = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
                switch (arg)
                {
                    case 0://禁用
                        db.ExecuteCommand(String.Format("UPDATE [T_SiteMenu] SET isenable = 0 WHERE id IN({0})", idcoll));
                        AppendLogs("禁用菜单：" + titlecoll, LogsAction.Edit);
                        break;
                    case -1://删除
                        {
                            List<String> filter = new List<String>();
                            foreach (Int32 id in listid)
                            {
                                filter.Add("(list LIKE '%," + id + ",%')");
                            }
                            String strSql = String.Format("SELECT id,title FROM [T_SiteMenu] WHERE {0}", String.Join(" OR ", filter.ToArray()));
                            DataTable ds = db.ExecuteDataTable(strSql);
                            foreach (DataRow dr in ds.Rows)
                            {
                                String _id = dr["id"].ToString();
                                strSql = String.Format("DELETE FROM [T_SiteMenu] WHERE WHERE id={0}", _id);
                                db.ExecuteCommand(strSql);

                                strSql = String.Format("DELECT FROM [T_AccessControl] WHERE tablename='t_group' AND node={0}", _id);
                                db.ExecuteCommand(strSql);

                                AppendLogs("删除菜单:" + dr["title"].ToString(), LogsAction.Delete);
                            }
                        }
                        break;
                    case 1://启用
                        db.ExecuteCommand(String.Format("UPDATE [T_SiteMenu] SET isenable = 1 WHERE id IN({0})", idcoll));
                        AppendLogs("启用菜单：" + titlecoll, LogsAction.Edit);
                        break;
                }
                CacheHelper.Delete(ISessionKeys.cache_table_accesscontrol);
            }
            BindData();
        }
    }
}