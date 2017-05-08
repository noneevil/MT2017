using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Core.Table;

namespace WebSite.Web.Debug
{
    public partial class Field_Edit : BaseAdmin
    {
        protected override void OnInit(EventArgs e)
        {
            CurrentTable = SiteTable.Tables.Find(a => { return a.ID == EditTableID; });
            CurrentFields = CurrentTable.Columns;

            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                @Control.DataSource = Enum.GetValues(typeof(ControlType));
                @Control.DataBind();
                @DataType.DataSource = Enum.GetValues(db.dbType);
                @DataType.DataBind();
                @DataSourceType.DataSource = Enum.GetValues(typeof(FieldDataSourceType));
                @DataSourceType.DataBind();
                @RepeatDirection.DataSource = Enum.GetValues(typeof(RepeatDirection));
                @RepeatDirection.DataBind();
                @RepeatLayout.DataSource = Enum.GetValues(typeof(RepeatLayout));
                @RepeatLayout.DataBind();

                @RepeatDirection.Items[0].Selected = true;
                @RepeatLayout.Items[0].Selected = true;

                if (IsEdit)
                {
                    LoadData();
                }
                else
                {
                    isVirtual.SelectedIndex = 1;
                }
                Control_SelectedIndexChanged(sender, e);
                DataSourceType_SelectedIndexChanged(sender, e);
            }
        }
        /// <summary>
        /// 字段
        /// </summary>
        private WebSite.Core.Table.Field CurrentField;
        /// <summary>
        /// 表
        /// </summary>
        private SiteTable CurrentTable;
        /// <summary>
        /// 表字段
        /// </summary>
        private List<WebSite.Core.Table.Field> CurrentFields;
        /// <summary>
        /// 是否是编辑模式
        /// </summary>
        protected Boolean IsEdit
        {
            get { return !String.IsNullOrEmpty(Request["id"]); }
        }
        /// <summary>
        /// 编辑字段编号
        /// </summary>
        protected Guid EditID
        {
            get
            {
                return Guid.Parse(Request["id"]);
            }
        }
        /// <summary>
        /// 编辑表名称
        /// </summary>
        protected Guid EditTableID
        {
            get
            {
                return Guid.Parse(Request["table"]);
            }
        }
        /// <summary>
        /// 初始化编辑数据
        /// </summary>
        protected void LoadData()
        {
            CurrentField = CurrentFields.Find(a => { return a.ID == EditID; });
            this.SetFormValue(CurrentField);

            if (CurrentField.DataSource != null)
            {
                //设置数据源类型
                this.SetFormValue(CurrentField.DataSource);

                if (CurrentField.DataSource.Layout != null)
                {
                    //设置布局模式
                    this.SetFormValue(CurrentField.DataSource.Layout);
                }
                if (CurrentField.DataSource.SQLDataSource != null)
                {
                    //设置SQL
                    this.SetFormValue(CurrentField.DataSource.SQLDataSource);
                }

                if (CurrentField.DataSource.ListItemDataSource != null)
                {
                    //设置列表
                    ListItem = CurrentField.DataSource.ListItemDataSource;
                    ListItemCount.Text = CurrentField.DataSource.ListItemDataSource.Count.ToString();
                    Repeater1.DataSource = CurrentField.DataSource.ListItemDataSource;
                    Repeater1.DataBind();
                }
            }

            FieldName.Enabled = false;
            isVirtual.Enabled = false;
            if (CurrentField.FieldName == "id" || CurrentField.FieldName == "settingsxml")
            {
                @DataType.Enabled = false;
            }
            @DataType.SelectedValue = Convert.ToString(Enum.Parse(db.dbType, CurrentField.DataType.ToString(), true));
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsEdit && ChkField)
            {
                Alert(Label1, "字段已经存在！", "line1px_2");
            }
            else
            {
                Int32 _length = 0;
                Int32.TryParse(@Length.Text, out _length);
                @Length.Text = _length.ToString();

                Int32 _columns = 1;
                Int32.TryParse(RepeatColumns.Text, out _columns);
                RepeatColumns.Text = _columns.ToString();

                String sql = String.Empty;
                Int32 _datatypevalue = Convert.ToInt32(Enum.Parse(db.dbType, @DataType.SelectedValue, true));
                DataTypeValue.Value = _datatypevalue.ToString();
                if (IsEdit)
                {
                    CurrentField = CurrentFields.Find(a => { return a.ID == EditID; });

                    #region 检查数据库改动

                    if (isVirtual.SelectedIndex == 1)
                    {
                        //类型类型改动
                        sql = String.Format("ALTER TABLE {0} ALTER COLUMN {1} {2}", CurrentTable.TableName, CurrentField.FieldName, @DataType.SelectedValue);
                        if (CurrentField.Length != _length && _length != 0)//检查长度
                        {
                            sql += "(" + _length + ")";
                        }
                        if (CurrentField.DataType != _datatypevalue || (CurrentField.Length != _length && _length != 0))
                        {
                            try
                            {
                                db.ExecuteCommand(sql);
                            }
                            catch (Exception ex)
                            {
                                Alert(Label1, ex.Message, "line1px_2");
                                return;
                            }
                        }
                    }

                    #endregion
                }
                else
                {
                    CurrentField = new WebSite.Core.Table.Field()
                    {
                        ID = Guid.NewGuid()
                    };
                    if (isVirtual.SelectedIndex == 1)//添加真实字段
                    {
                        sql = sql = String.Format("ALTER TABLE {0} ADD {1} {2}", CurrentTable.TableName, FieldName.Text, @DataType.SelectedValue);
                        if (_length != 0)//设置长度
                        {
                            sql += "(" + _length + ")";
                        }

                        try
                        {
                            db.ExecuteCommand(sql);
                        }
                        catch (Exception ex)
                        {
                            Alert(Label1, ex.Message, "line1px_2");
                            return;
                        }
                    }

                    CurrentFields.Add(CurrentField);
                }
                CurrentField = this.GetFormValue<WebSite.Core.Table.Field>(CurrentField);

                //数据源类型
                CurrentField.DataSource = this.GetFormValue<FieldDataSource>(CurrentField.DataSource);
                //布局模式
                CurrentField.DataSource.Layout = this.GetFormValue<DataSourceLayout>(CurrentField.DataSource.Layout);
                //SQL数据源
                CurrentField.DataSource.SQLDataSource = this.GetFormValue<SQLDataSource>(CurrentField.DataSource.SQLDataSource);
                //列表项目
                if (Repeater1.Items.Count > 0)
                {
                    CurrentField.DataSource.ListItemDataSource = new List<ListItemDataSource>();
                    foreach (RepeaterItem item in Repeater1.Items)
                    {
                        String text = ((TextBox)item.Controls[0].FindControl("t1")).Text;
                        String value = ((TextBox)item.Controls[0].FindControl("t2")).Text;
                        Boolean chk = ((CheckBox)item.Controls[0].FindControl("c1")).Checked;
                        ListItemDataSource listitem = new ListItemDataSource
                        {
                            Selected = chk,
                            Text = text,
                            Value = value
                        };
                        CurrentField.DataSource.ListItemDataSource.Add(listitem);
                    }
                }

                SiteTable.SaveTables(SiteTable.Tables);
                Alert(Label1, "保存成功！", "line1px_3");
            }
        }
        /// <summary>
        /// 检查数据表是否存在
        /// </summary>
        /// <returns></returns>
        private Boolean ChkField
        {
            get
            {
                return CurrentFields.Find(a => { return a.FieldName == FieldName.Text.ToLower(); }) != null;
            }
        }
        /// <summary>
        /// 列表项目缓存
        /// </summary>
        protected List<ListItemDataSource> ListItem
        {
            get
            {
                object obj = ViewState["listitem"];
                if (obj != null) return (List<ListItemDataSource>)obj;
                return new List<ListItemDataSource>();
            }
            set { ViewState["listitem"] = value; }
        }
        /// <summary>
        /// 设置列表项目数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SetCount_OnClick(object sender, EventArgs e)
        {
            Int32 count = 0;
            Int32.TryParse(ListItemCount.Text, out count);
            if (count == 0) return;

            List<ListItemDataSource> list = new List<ListItemDataSource>();
            for (int i = 0; i < count; i++)
            {
                if (i < Repeater1.Items.Count)
                {
                    RepeaterItem item = Repeater1.Items[i];
                    String text = ((TextBox)item.Controls[0].FindControl("t1")).Text;
                    String value = ((TextBox)item.Controls[0].FindControl("t2")).Text;
                    Boolean chk = ((CheckBox)item.Controls[0].FindControl("c1")).Checked;
                    list.Add(new ListItemDataSource
                    {
                        Selected = chk,
                        Text = text,
                        Value = value
                    });
                }
                else
                {
                    list.Add(new ListItemDataSource
                    {
                        Selected = false,
                        //Text = (i + 1).ToString(),
                        Value = (i + 1).ToString()
                    });
                }
            }
            ListItem = list;
            Repeater1.DataSource = list;
            Repeater1.DataBind();
        }
        /// <summary>
        /// 选择控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Control_SelectedIndexChanged(object sender, EventArgs e)
        {
            ControlType input = (ControlType)Enum.Parse(typeof(ControlType), @Control.SelectedValue, true);
            switch (input)
            {
                case ControlType.RadioButtonList:
                case ControlType.CheckBoxList:
                case ControlType.DropDownList:
                case ControlType.ListBox:
                    if (input == ControlType.RadioButtonList || input == ControlType.CheckBoxList)
                    {
                        tab_2.Visible = true;
                        tabs_2.Visible = true;
                    }
                    else
                    {
                        tab_2.Visible = false;
                        tabs_2.Visible = false;
                    }

                    tab_3.Visible = true;
                    tabs_3.Visible = true;
                    break;
                default:
                    tab_2.Visible = false;
                    tabs_2.Visible = false;

                    tab_3.Visible = false;
                    tabs_3.Visible = false;
                    break;
            }
        }
        /// <summary>
        /// 数据类型选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DataSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FieldDataSourceType type = (FieldDataSourceType)Enum.Parse(typeof(FieldDataSourceType), DataSourceType.SelectedValue, true);
            switch (type)
            {
                case FieldDataSourceType.SQL:
                    _customsql.Visible = true;
                    _listitems.Visible = false;

                    break;
                case FieldDataSourceType.ListItem:
                    _customsql.Visible = false;
                    _listitems.Visible = true;

                    break;
                default:

                    break;
            }
        }
    }
}