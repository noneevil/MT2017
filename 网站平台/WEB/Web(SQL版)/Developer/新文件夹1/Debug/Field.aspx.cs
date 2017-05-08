using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Core.Table;

namespace WebSite.Web.Debug
{
    public partial class Field : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentTable = SiteTable.Tables.Find(a => { return a.ID == TableID; });
            TableName = CurrentTable.TableName;
            CurrentFields = CurrentTable.Columns;
            if (!IsPostBack)
            {
                if (CurrentFields == null || CurrentFields.Count == 0)
                {
                    SiteTable.RefreshField(CurrentTable);
                }
                BindData();
            }
        }
        /// <summary>
        /// 表
        /// </summary>
        private SiteTable CurrentTable;
        /// <summary>
        /// 表字段
        /// </summary>
        private List<WebSite.Core.Table.Field> CurrentFields;
        /// <summary>
        /// 数据表名
        /// </summary>
        protected String TableName;
        /// <summary>
        /// 表编号
        /// </summary>
        protected Guid TableID
        {
            get { return Guid.Parse(Request["table"]); }
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            Repeater1.DataSource = CurrentFields;
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
                var field = CurrentFields.Find(a => { return a.ID == Guid.Parse(id); });
                if (field != null)
                {
                    if (!field.isVirtual)
                    {
                        String sql = String.Format("ALTER TABLE {0} DROP COLUMN {1}", TableName, field.FieldName);
                        db.ExecuteCommand(sql);
                    }
                    CurrentFields.Remove(field);
                    SiteTable.SaveTables(SiteTable.Tables);
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
                WebSite.Core.Table.Field field = e.Item.DataItem as WebSite.Core.Table.Field;
                String name = field.FieldName.ToLower();
                ImageButton del = e.Item.FindControl("del") as ImageButton;
                if (name == "id" || name == "settingsxml")
                {
                    del.OnClientClick = "javascript:return false;";
                    del.ImageUrl = "../images/icos/del_disabled.gif";
                }
                else
                {
                    del.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 字段且无法恢复!确定要删除吗?'}});return false;", field.FieldName);
                }
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
                    SiteTable.RefreshField(CurrentTable);
                    this.BindData();
                    break;
                case -1:
                    List<SiteTable> list = new List<SiteTable>();
                    foreach (RepeaterItem item in Repeater1.Items)
                    {
                        HtmlInputCheckBox chk = item.FindControl("id") as HtmlInputCheckBox;
                        if (chk.Checked)
                        {
                            var field = CurrentFields.Find(a => { return a.ID == Guid.Parse(chk.Value); });
                            if (field.FieldName == "id" || field.FieldName == "settingsxml") continue;
                            if (field != null)
                            {
                                if (!field.isVirtual)
                                {
                                    String sql = String.Format("ALTER TABLE {0} DROP COLUMN {1}", TableName, field.FieldName);
                                    db.ExecuteCommand(sql);
                                }
                                CurrentFields.Remove(field);
                            }
                        }
                    }
                    SiteTable.SaveTables(SiteTable.Tables);
                    this.BindData();
                    break;
            }
        }
    }
}