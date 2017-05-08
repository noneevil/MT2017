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
    public partial class Group : BaseAdmin
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
        private void BindData()
        {
            String strSql = String.Format("SELECT id,tablename,groupname,parentid,actiontype,template,layer FROM [t_group] WHERE tablename='{0}' ORDER BY id", Request["table"]);
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
            String id = ((CheckBox)e.Item.FindControl("ID")).Text;
            if (e.CommandName == "del")
            {
                DataTable ds = db.ExecuteDataTable(String.Format("SELECT id,groupname FROM [T_Group] WHERE (list LIKE '%,{0},%')", id));
                foreach (DataRow dr in ds.Rows)
                {
                    String _id = dr["id"].ToString();
                    String strSql = String.Format("DELETE FROM [T_Group] WHERE WHERE id={0}", _id);
                    db.ExecuteCommand(strSql);

                    strSql = String.Format("DELECT FROM [T_AccessControl] WHERE tablename='t_group' AND node={0}", _id);
                    db.ExecuteCommand(strSql);

                    AppendLogs("删除分类:" + dr["groupname"].ToString(), LogsAction.Delete);
                }

                Cache.Remove(ISessionKeys.cache_table_group);
                Cache.Remove(ISessionKeys.cache_table_accesscontrol);
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
                Int32 layer = Convert.ToInt32(data["layer"].ToString());
                ImageButton btndel = (ImageButton)e.Item.FindControl("del");
                btndel.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除分类:“{0}”及该分类所属子分类 且无法恢复!确定要删除吗?'}});return false;", data["GroupName"].ToString());

                e.Item.SetStyleLayer(layer);
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
            Int32 arg = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
            if (arg == -1)
            {
                List<Int32> listid = new List<Int32>();
                foreach (RepeaterItem item in Repeater1.Items)
                {
                    CheckBox chkid = (CheckBox)item.FindControl("id");
                    if (chkid.Checked)
                    {
                        listid.Add(Convert.ToInt32(chkid.Text));
                    }
                }

                if (listid.Count > 0)
                {
                    List<String> filter = new List<String>();
                    foreach (Int32 id in listid)
                    {
                        filter.Add("(list LIKE '%," + id + ",%')");
                    }
                    String strSql = String.Format("SELECT id,groupname FROM [T_Group] WHERE {0}", String.Join(" OR ", filter.ToArray()));
                    DataTable ds = db.ExecuteDataTable(strSql);
                    foreach (DataRow dr in ds.Rows)
                    {
                        String _id = dr["id"].ToString();
                        strSql = String.Format("DELETE FROM [T_Group] WHERE WHERE id={0}", _id);
                        db.ExecuteCommand(strSql);

                        strSql = String.Format("DELECT FROM [T_AccessControl] WHERE tablename='t_group' AND node={0}", _id);
                        db.ExecuteCommand(strSql);

                        AppendLogs("删除分类:" + dr["groupname"].ToString(), LogsAction.Delete);
                    }

                    Cache.Remove(ISessionKeys.cache_table_group);
                    Cache.Remove(ISessionKeys.cache_table_accesscontrol);
                }
            }
            BindData();
        }
    }
}