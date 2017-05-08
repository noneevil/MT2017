using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Core;
using WebSite.Core.Table;
using WebSite.Interface;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class Group_Edit : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
                if (IsEdit)
                {
                    LoadData();
                }
            }
        }
        /// <summary>
        /// 树形节点
        /// </summary>
        private List<T_GroupEntity> TreeNodes
        {
            get
            {
                String sql = String.Format("SELECT id,groupname,parentid FROM [T_Group] WHERE tablename='{0}'", tableName);
                List<T_GroupEntity> nodes = db.ExecuteObject<List<T_GroupEntity>>(sql);

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

            //权限资源
            actiontype.DataSource = ActionTypeHelper.ActionList();
            actiontype.DataTextField = "value";
            actiontype.DataValueField = "key";
            actiontype.DataBind();
            actiontype.SelectedIndex = 0;

            //绑定字段数据
            var table = SiteTable.Tables.Find(a => { return a.TableName == tableName; });
            foreach (var item in table.Columns)
            {
                String _name = String.IsNullOrEmpty(item.Description) ? item.FieldName : item.Description;
                @Field.Items.Add(new ListItem(_name, item.FieldName));
            }

            //绑定树形分类
            foreach (var item in TreeNodes)
            {
                ListItem node = new ListItem(item.GroupName, item.ID.ToString());
                node.Attributes.Add("pid", item.ParentID.ToString());
                ParentID.Items.Add(node);
            }
            var root = new ListItem("顶级分类", "0");
            root.Selected = true;
            ParentID.Items.Insert(0, root);
        }
        /// <summary>
        /// 表名
        /// </summary>
        protected String tableName
        {
            get { return Request["table"]; }
        }
        /// <summary>
        /// 加载编辑数据
        /// </summary>
        protected void LoadData()
        {
            String sql = "SELECT a.*,IsNull(b.groupname,'顶级分类') AS parentname FROM [t_group] AS a LEFT JOIN [t_group] AS b ON a.parentid = b.id WHERE a.id=" + EditID;
            T_GroupEntity data = db.ExecuteObject<T_GroupEntity>(sql);
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
            if (actiontype.SelectedIndex == -1) actiontype.SelectedIndex = 0;

            T_GroupEntity data = (T_GroupEntity)ViewState["data"];
            data = this.GetFormValue<T_GroupEntity>(data);
            CmdType cmd = IsEdit ? CmdType.UPDATE : CmdType.INSERT;

            if (db.ExecuteCommand<T_GroupEntity>(data, cmd))
            {
                FixLayer();
                if (!IsEdit)
                {
                    GroupName.Text = "";
                }
                else
                {
                    UpdateAccessControl(data);
                }

                Cache.Remove(ISessionKeys.cache_table_group);
                Cache.Remove(ISessionKeys.cache_table_accesscontrol);

                Alert(Label1, "保存成功！", "line1px_3");

                String text = String.Format("{0}分类：{1}.", IsEdit ? "编辑" : "添加", data.GroupName);
                AppendLogs(text, IsEdit ? LogsAction.Edit : LogsAction.Create);
            }
        }
        /// <summary>
        /// 更新权限设置
        /// </summary>
        /// <param name="nodeId">权限节点</param>
        protected void UpdateAccessControl(T_GroupEntity data)
        {
            String sql = "SELECT * FROM [T_AccessControl] WHERE tablename='t_group' AND node=" + data.ID;
            List<T_AccessControlEntity> acc = db.ExecuteObject<List<T_AccessControlEntity>>(sql);

            foreach (T_AccessControlEntity node in acc)
            {
                List<ActionType> items = ActionTypeHelper.GetValueItem(node.ActionType);
                foreach (ActionType n in items)
                {
                    if (!data.ActionType.HasFlag(n))
                    {
                        node.ActionType = ActionTypeHelper.RemoveACLoptions(node.ActionType, n);
                    }
                }
            }

            db.ExecuteCommand<List<T_AccessControlEntity>>(acc, CmdType.UPDATE);
        }
        /// <summary>
        /// 修复层级数
        /// </summary>
        protected void FixLayer()
        {
            String sql = "SELECT id,parentid,actiontype,actiontype as actiontype_2,layer,layer as layer_2,list,list as list_2 FROM [T_Group]";
            DataTable dt = db.ExecuteDataTable(sql);
            UpdateLayer(dt, 0);
            foreach (DataRow dr in dt.Rows)
            {
                if ((Convert.ToInt32(dr["layer"]) != Convert.ToInt32(dr["layer_2"])) ||
                    (Convert.ToInt32(dr["actiontype"]) != Convert.ToInt32(dr["actiontype_2"]) ||
                    (dr["list"].ToString() != dr["list_2"].ToString())))
                {
                    ExecuteObject obj = new ExecuteObject();
                    obj.cmdtype = CmdType.UPDATE;
                    obj.tableName = "T_Group";
                    obj.terms.Add("ID", dr["id"]);
                    obj.cells.Add("list", dr["list"]);
                    obj.cells.Add("layer", dr["layer"]);
                    obj.cells.Add("actiontype", dr["actiontype"]);
                    db.ExecuteCommand(obj);
                }
            }
        }
        private Int32 layer = 0;
        /// <summary>
        /// 更新层级关系
        /// </summary>
        private void UpdateLayer(DataTable dt, Int32 ParentID)
        {
            layer++;
            foreach (DataRow dr in dt.Select("parentid=" + ParentID))
            {
                String id = dr["id"].ToString();
                List<String> idlist = new List<String>();
                GetList(dt, id, idlist);

                DataRow[] childs = dt.Select("parentid=" + id);
                if (childs.Length > 0) dr["actiontype"] = ActionType.View;

                dr["layer"] = layer;
                dr["list"] = "," + String.Join(",", idlist.ToArray()) + ",";
                UpdateLayer(dt, int.Parse(id));
            }
            layer--;
        }
        /// <summary>
        /// 获取所有父节点ID
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="id"></param>
        /// <param name="IdList"></param>
        private void GetList(DataTable dt, String id, List<String> IdList)
        {
            foreach (DataRow dr in dt.Select("id=" + id))
            {
                IdList.Insert(0, dr["id"].ToString());
                GetList(dt, dr["parentid"].ToString(), IdList);
            }
        }
        /// <summary>
        /// 分类选择 继承选择分类设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ParentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            String sql = "SELECT * FROM [t_group] WHERE id=" + ParentID.SelectedValue;
            T_GroupEntity data = db.ExecuteObject<T_GroupEntity>(sql);

            //继承选择分类属性
            @Template.SelectedValue = data.Template.ToString();
            Layer.Value = (data.Layer + 1).ToString();

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