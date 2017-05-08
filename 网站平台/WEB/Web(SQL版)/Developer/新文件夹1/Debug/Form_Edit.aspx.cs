using System;
using System.Collections.Generic;
using CommonUtils;
using WebSite.BackgroundPages;
using WebSite.Core.Table;

namespace WebSite.Web.Debug
{
    public partial class Form_Edit : BaseAdmin
    {
        protected override void OnInit(EventArgs e)
        {
            CurrentTable = TableForm.TableForms.Find(a => { return a.TableID == TableID; });
            CurrentForms = CurrentTable.FromCollections;
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (IsEdit)
                {
                    LoadData();
                }
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
        /// 表单
        /// </summary>
        private TableFormItem CurrentForm;
        /// <summary>
        /// 表编号
        /// </summary>
        protected Guid TableID
        {
            get { return Guid.Parse(Request["table"]); }
        }
        /// <summary>
        /// 是否是编辑模式
        /// </summary>
        protected Boolean IsEdit
        {
            get { return !String.IsNullOrEmpty(Request["id"]); }
        }
        /// <summary>
        /// 表单编号
        /// </summary>
        protected Guid EditID
        {
            get
            {
                return Guid.Parse(Request["id"]);
            }
        }
        /// <summary>
        /// 加载编辑数据
        /// </summary>
        protected void LoadData()
        {
            CurrentForm = CurrentForms.Find(a => { return a.ID == EditID; });
            this.SetFormValue(CurrentForm);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(Object sender, EventArgs e)
        {
            //HtmlDocument _xml = new HtmlDocument();
            //_xml.LoadHtml(Content.Text);
            //HtmlNode form = _xml.DocumentNode.SelectSingleNode("//form");
            //if (form == null)
            //{
            //    Alert(Label1, "没有发现表单!", "line1px_2");
            //    return;
            //}
            //HtmlAttributeCollection att = form.Attributes;
            //if (att["name"] == null || String.IsNullOrEmpty(att["name"].Value))
            //{
            //    Alert(Label1, "表单名字未设置!", "line1px_2");
            //    return;
            //}
            //String name = att["name"].Value;

            if (IsEdit)
            {
                CurrentForm = CurrentForms.Find(a => { return a.ID == EditID; });
            }
            else
            {
                CurrentForm = new TableFormItem { ID = Guid.NewGuid() };
                CurrentForms.Add(CurrentForm);
            }
            this.GetFormValue<TableFormItem>(CurrentForm);
            TableForm.SaveForms();
            Alert(Label1, "保存成功!", "line1px_3");
        }
    }
}