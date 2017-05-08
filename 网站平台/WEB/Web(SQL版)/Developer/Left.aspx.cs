using System;
using System.Data;
using System.Web.UI.WebControls;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class Left : BaseAdmin
    {
        protected void Page_Load(Object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        /// <summary>
        /// 功能数据表 
        /// </summary>
        private DataTable SiteMenu;
        /// <summary>
        /// 绑定一级菜单
        /// </summary>
        protected void BindData()
        {
            String sql = String.Format("SELECT a.id,a.parentid,a.title,a.link_url,a.isopen,b.actiontype FROM [T_AccessControl] AS b INNER JOIN [T_SiteMenu] AS a ON b.node = a.id WHERE (b.TableName = 't_sitemenu') AND (b.role = {0}) AND (b.actiontype <> 0) AND (a.isenable = 1) AND (a.issystem = 0) ORDER BY a.sortid", Admin.RoleID);
            if (Admin.IsSuper)
            {
                sql = "SELECT id,parentid,title,link_url,isopen,2 as actiontype FROM [T_SiteMenu] WHERE isenable = 1 AND issystem = 0 ORDER BY sortid";
            }

            DataTable dt = db.ExecuteDataTable(sql);
            SiteMenu = dt.Clone();
            foreach (DataRow dr in dt.Rows)
            {
                ActionType value = (ActionType)Enum.Parse(typeof(ActionType), dr["actiontype"].ToString());
                if (value.HasFlag(ActionType.View))
                {
                    SiteMenu.ImportRow(dr);
                }
            }

            DataView dv = new DataView(SiteMenu);
            dv.RowFilter = "parentid=0";
            Repeater1.DataSource = dv;
            Repeater1.DataBind();
        }
        /// <summary>
        /// 绑定二级菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rpt_ItemDataBound(Object sender, RepeaterItemEventArgs e)
        {
            DataRowView data = (DataRowView)e.Item.DataItem;
            DataView dv = new DataView(SiteMenu);
            dv.RowFilter = "parentid=" + data["id"].ToString();
            Repeater _rpt = (Repeater)e.Item.FindControl("rpt");
            _rpt.DataSource = dv;
            _rpt.DataBind();
        }
    }
}