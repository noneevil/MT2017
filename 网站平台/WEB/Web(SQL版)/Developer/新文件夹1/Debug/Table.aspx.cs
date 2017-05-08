using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Core.Table;

namespace WebSite.Web.Debug
{
    public partial class Table : BaseAdmin
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentTables = SiteTable.Tables;
            if (!IsPostBack)
            {
                if (CurrentTables == null || CurrentTables.Count == 0)
                {
                    SiteTable.RefreshTable();
                }
                BindData();
            }
        }
        private List<SiteTable> CurrentTables;
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            Repeater1.DataSource = CurrentTables;
            Repeater1.DataBind();
        }
        /// <summary>
        /// 列表事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String id = ((HtmlInputCheckBox)e.Item.FindControl("ID")).Value;
            if (e.CommandName == "del")
            {
                Guid tabid = Guid.Parse(id);
                var tab = CurrentTables.Find(a => { return a.ID == tabid; });
                if (tab != null)
                {
                    String sql = "DROP TABLE " + tab.TableName;
                    db.ExecuteCommand(sql);
                    CurrentTables.Remove(tab);
                    SiteTable.SaveTables(CurrentTables);
                }
                var frm = TableForm.TableForms.Find(a => { return a.TableID == tabid; });
                if (frm != null)
                {
                    TableForm.TableForms.Remove(frm);
                    TableForm.SaveForms();
                }
                this.BindData();
            }
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
                SiteTable dv = e.Item.DataItem as SiteTable;
                ImageButton del = e.Item.FindControl("del") as ImageButton;
                del.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 数据表且无法恢复!确定要删除吗?'}});return false;", dv.TableName);
            }
        }
        /// <summary>
        /// 批量删除 刷新表结构
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Int32 arguments = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
            switch (arguments)
            {
                case 1:
                    SiteTable.RefreshTable();
                    this.BindData();
                    break;
                case -1:
                    List<SiteTable> list = new List<SiteTable>();
                    foreach (RepeaterItem item in Repeater1.Items)
                    {
                        HtmlInputCheckBox chk = item.FindControl("id") as HtmlInputCheckBox;
                        if (chk.Checked)
                        {
                            var tabid = Guid.Parse(chk.Value);
                            var tab = CurrentTables.Find(a => { return a.ID == tabid; });
                            if (tab != null)
                            {
                                String sql = "DROP TABLE " + tab.TableName;
                                db.ExecuteCommand(sql);
                                CurrentTables.Remove(tab);
                            }
                            var frm = TableForm.TableForms.Find(a => { return a.TableID == tabid; });
                            if (frm != null)
                            {
                                TableForm.TableForms.Remove(frm);
                                TableForm.SaveForms();
                            }
                        }
                    }
                    SiteTable.SaveTables(CurrentTables);
                    this.BindData();
                    break;
            }
        }
    }
}