using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Core;
using WebSite.Core.Table;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class Group_Edit : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Groups = T_GroupHelper.Groups;
            if (!IsPostBack)
            {
                BindData();
                if (IsEdit)
                {
                    LoadData();
                }
            }
        }
        private List<T_GroupEntity> Groups;
        /// <summary>
        /// 树形节点
        /// </summary>
        private List<T_GroupEntity> TreeNodes
        {
            get
            {
                List<T_GroupEntity> nodes = Groups.FindAll(a => { return a.TableName == tableName; });

                if (IsEdit)
                {
                    #region 节点检查 编辑时移出自身及子节点

                    while (true)
                    {
                        List<T_GroupEntity> remove = new List<T_GroupEntity>();
                        foreach (var item in nodes)
                        {
                            if (item.ID == EditID)
                            {
                                remove.Add(item);
                                continue;
                            }
                            if (item.ParentID == 0) continue;
                            T_GroupEntity temp = nodes.Find(a => { return a.ID == item.ParentID; });
                            if (temp == null) remove.Add(item);
                        }
                        if (remove.Count == 0) break;

                        foreach (var item in remove)
                        {
                            nodes.Remove(item);
                        }
                    }

                    #endregion
                }

                return nodes;
            }
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            @Template.DataSource = Enum.GetValues(typeof(SiteTemplate));
            @Template.DataBind();
            @TableName.Text = tableName;

            #region 绑定字段数据

            var table = SiteTable.Tables.Find(a => { return a.TableName == tableName; });
            foreach (var item in table.Columns)
            {
                String _name = String.IsNullOrEmpty(item.Description) ? item.FieldName : item.Description;
                @Field.Items.Add(new ListItem(_name, item.FieldName));
            }

            #endregion

            #region 绑定树形分类

            foreach (var item in TreeNodes)
            {
                ListItem el = new ListItem(item.GroupName, item.ID.ToString());
                el.Attributes.Add("pid", item.ParentID.ToString());
                ParentID.Items.Add(el);
            }
            var root = new ListItem("顶级分类", "0");
            root.Selected = true;
            ParentID.Items.Insert(0, root);

            #endregion
        }
        /// <summary>
        /// 表名
        /// </summary>
        protected String tableName
        {
            get { return Request["table"]; }
        }
        /// <summary>
        /// 是否是编辑模式
        /// </summary>
        protected Boolean IsEdit
        {
            get { return EditID > 0; }
        }
        /// <summary>
        /// 编辑ID
        /// </summary>
        protected Int32 EditID
        {
            get
            {
                Int32 outid = 0;
                Int32.TryParse(Request["id"], out outid);
                return outid;
            }
        }
        /// <summary>
        /// 加载编辑数据
        /// </summary>
        protected void LoadData()
        {
            T_GroupEntity data = Groups.Find(a => { return a.ID == EditID; });
            ViewState["data"] = data;
            this.SetFormValue(data);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            T_GroupEntity data = (T_GroupEntity)ViewState["data"];
            data = this.GetFormValue<T_GroupEntity>(data);
            CmdType cmd = IsEdit ? CmdType.UPDATE : CmdType.INSERT;
            if (db.ExecuteCommand<T_GroupEntity>(data, cmd))
            {
                if (!IsEdit) GroupName.Text = "";
                Alert(Label1, "保存成功！", "line1px_3");
            }
            Cache.Remove("T_Group");
        }
        /// <summary>
        /// 分类选择 继承选择分类设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ParentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            T_GroupEntity data = T_GroupHelper.Groups.Find(a => { return a.ID == Convert.ToInt32(ParentID.SelectedValue); });
            if (data != null)
            {
                //继承选择分类属性
                @Template.SelectedValue = data.Template.ToString();

                Field.SelectedIndex = -1;
                if (!String.IsNullOrEmpty(data.Field))
                {
                    foreach (String k in data.Field.Split(','))
                    {
                        foreach (ListItem item in Field.Items)
                        {
                            if (item.Value == k)
                            {
                                item.Selected = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}