using System;
using System.Data;
using System.Web.UI.WebControls;
using MSSQLDB;
using WebSite.BackgroundPages;

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
            String sql = String.Format("SELECT a.id,a.parentid,a.menuname,a.[action],a.[open] FROM [T_AccessControl] AS b INNER JOIN [T_SiteMenu] AS a ON b.node = a.id WHERE (b.TableName = 't_sitemenu') AND (b.role = {0}) AND (b.power = 2) AND (a.status = 1) AND (a.internal = 0) ORDER BY a.Sort", Admin.RoleID);
            if (Admin.IsSuper)
            {
                sql = "SELECT menuname,id,[action],[open],parentid FROM [T_SiteMenu] WHERE status = 1 AND internal = 0 ORDER BY sort";
            }

            SiteMenu = db.ExecuteDataTable(sql);
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
            DataRowView drv = (DataRowView)e.Item.DataItem;
            DataView dv = new DataView(SiteMenu);
            dv.RowFilter = "parentid=" + drv["id"];
            Repeater _rpt = (Repeater)e.Item.FindControl("rpt");
            _rpt.DataSource = dv;
            _rpt.DataBind();
        }
    }
}