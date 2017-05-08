using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebSite.BackgroundPages;
using WebSite.Core.Table;

namespace WebSite.Web.Debug
{
    public partial class Form : BaseAdmin
    {
        protected override void OnInit(EventArgs e)
        {
            CurrentTable = TableForm.TableForms.Find(a => { return a.TableID == TableID; });
            if (CurrentTable == null)
            {
                CurrentTable = new TableForm { TableID = TableID, FromCollections = new List<TableFormItem>(), TableName = TableName };
                TableForm.TableForms.Add(CurrentTable);
            }
            CurrentForms = CurrentTable.FromCollections;
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        /// <summary>
        /// 表
        /// </summary>
        private TableForm CurrentTable;
        /// <summary>
        /// 表单集合
        /// </summary>
        private List<TableFormItem> CurrentForms;
        /// <summary>
        /// 数据表编号
        /// </summary>
        protected Guid TableID
        {
            get { return Guid.Parse(Request["table"]); }
        }
        /// <summary>
        /// 表名
        /// </summary>
        protected String TableName
        {
            get { return SiteTable.Tables.Find(a => { return a.ID == TableID; }).TableName; }
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            Repeater1.DataSource = CurrentForms;
            Repeater1.DataBind();
        }
        /// <summary>
        /// 列表事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                Guid id = Guid.Parse(((HtmlInputCheckBox)e.Item.FindControl("id")).Value);
                TableFormItem frm = CurrentForms.Find(a => { return a.ID == id; });
                if (id != null)
                {
                    CurrentForms.Remove(frm);
                    TableForm.SaveForms();
                }
                this.BindData();
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Int32 arguments = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
            if (arguments == -1)
            {
                List<TableForm> list = new List<TableForm>();
                foreach (RepeaterItem item in Repeater1.Items)
                {
                    HtmlInputCheckBox chk = item.FindControl("id") as HtmlInputCheckBox;
                    if (chk.Checked)
                    {
                        var frm = CurrentForms.Find(a => { return a.ID == Guid.Parse(chk.Value); });
                        if (frm != null)
                        {
                            CurrentForms.Remove(frm);
                        }
                    }
                }
                TableForm.SaveForms();
                this.BindData();
            }
        }
    }
}