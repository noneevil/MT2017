using System;
using System.Collections.Generic;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Core.Table;

namespace WebSite.Web.Debug
{
    public partial class Table_Edit : BaseAdmin
    {
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
        /// 是否是编辑模式
        /// </summary>
        protected Boolean IsEdit
        {
            get { return !String.IsNullOrEmpty(Request["id"]); }
        }
        /// <summary>
        /// 编辑表名编号
        /// </summary>
        protected Guid EditID
        {
            get
            {
                return Guid.Parse(Request["id"]);
            }
        }
        /// <summary>
        /// 表
        /// </summary>
        protected SiteTable CurrentTable;
        /// <summary>
        /// 初始化编辑数据
        /// </summary>
        protected void LoadData()
        {
            CurrentTable = SiteTable.Tables.Find(a => { return a.ID == EditID; });
            this.SetFormValue(CurrentTable);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsEdit && ChkTable)
            {
                Alert(Label1, "表已经存在！", "line1px_2");
            }
            else
            {
                if (IsEdit)
                {
                    CurrentTable = SiteTable.Tables.Find(a => { return a.ID == EditID; });
                    if (TableName.Text.ToLower() != CurrentTable.TableName.ToLower())
                    {
                        if (ChkTable)
                        {
                            Alert(Label1, "表已经存在！", "line1px_2");
                            return;
                        }
                        else
                        {
                            String sql = String.Format("EXEC sp_rename '{0}','{1}'", CurrentTable.TableName, TableName.Text);//修改表名
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
                }
                else
                {
                    CurrentTable = new SiteTable { ID = Guid.NewGuid(), Columns = new List<WebSite.Core.Table.Field>() };

                    try
                    {
                        String sql = string.Format("CREATE TABLE [{0}]([ID] [bigint] IDENTITY(1,1) NOT NULL PRIMARY KEY,[SettingsXML] [varchar] (8000) DEFAULT '')", TableName.Text);
                        db.ExecuteCommand(sql);
                    }
                    catch (Exception ex)
                    {
                        Alert(Label1, ex.Message, "line1px_2");
                        return;
                    }
                    SiteTable.Tables.Add(CurrentTable);
                }
                this.GetFormValue<SiteTable>(CurrentTable);

                SiteTable.SaveTables(SiteTable.Tables);
                Alert(Label1, "保存成功！", "line1px_3");
            }
        }
        /// <summary>
        /// 检查数据表是否存在
        /// </summary>
        /// <returns></returns>
        private Boolean ChkTable
        {
            get
            {
                return SiteTable.Tables.Find(a => { return a.TableName == TableName.Text.ToLower(); }) != null;
            }
        }
    }
}