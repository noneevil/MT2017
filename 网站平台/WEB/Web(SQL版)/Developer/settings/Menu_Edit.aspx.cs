using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Core;
using WebSite.Interface;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class Menu_Edit : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                String strSql = String.Format("SELECT id,title FROM [T_SiteMenu] WHERE ParentID=0 AND id<>{0} ORDER BY id", EditID);
                parentid.DataSource = db.ExecuteDataTable(strSql);
                parentid.DataTextField = "title";
                parentid.DataValueField = "id";
                parentid.DataBind();
                parentid.Items.Insert(0, new ListItem("顶级分类", "0"));

                actiontype.DataSource = ActionTypeHelper.ActionList();
                actiontype.DataTextField = "value";
                actiontype.DataValueField = "key";
                actiontype.DataBind();

                if (IsEdit)
                {
                    LoadData();
                }
            }
        }
        /// <summary>
        /// 初始化编辑数据
        /// </summary>
        protected void LoadData()
        {
            String strSql = "SELECT * FROM [T_SiteMenu] WHERE id=" + EditID;
            T_SiteMenuEntity data = db.ExecuteObject<T_SiteMenuEntity>(strSql);
            ViewState["data"] = data;
            this.SetFormValue(data);

            if (data.ParentID != 0) isopen.Visible = false;
            if (data.ParentID == 0 || data.ID == 16) issystem.Visible = false;
            if (data.ID == 1 || data.ID == 16) isenable.Enabled = false;
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (actiontype.SelectedIndex == -1) actiontype.SelectedIndex = 0;

            T_SiteMenuEntity data = (T_SiteMenuEntity)ViewState["data"];
            data = this.GetFormValue<T_SiteMenuEntity>(data);
            CmdType cmd = IsEdit ? CmdType.UPDATE : CmdType.INSERT;
            if (!IsEdit)
            {
                data.SortID = 9999;
                data.Layer = (data.ParentID > 0) ? 2 : 1;
            }
            if (db.ExecuteCommand<T_SiteMenuEntity>(data, cmd))
            {
                FixLayer();
                if (!IsEdit)
                {
                    this.ClearFromValue();
                }
                else
                {
                    UpdateAccessControl(data);
                }
                Alert(Label1, "保存成功！", "line1px_3");

                String text = String.Format("{0}菜单：{1}.", IsEdit ? "编辑" : "添加", data.Title);
                AppendLogs(text, IsEdit ? LogsAction.Edit : LogsAction.Create);
            }
        }
        /// <summary>
        /// 修复层级数
        /// </summary>
        protected void FixLayer()
        {
            String strSql = "SELECT id,parentid,actiontype,actiontype as actiontype_2,layer,layer as layer_2,list,list as list_2 FROM [T_SiteMenu]";
            DataTable dt = db.ExecuteDataTable(strSql);
            UpdateLayer(dt, 0);
            foreach (DataRow dr in dt.Rows)
            {
                if ((Convert.ToInt32(dr["layer"]) != Convert.ToInt32(dr["layer_2"])) ||
                    (Convert.ToInt32(dr["actiontype"]) != Convert.ToInt32(dr["actiontype_2"])) ||
                    (dr["list"].ToString() != dr["list_2"].ToString()))
                {
                    ExecuteObject obj = new ExecuteObject();
                    obj.cmdtype = CmdType.UPDATE;
                    obj.tableName = "T_SiteMenu";
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
        /// 更新权限设置
        /// </summary>
        /// <param name="nodeId">权限节点</param>
        protected void UpdateAccessControl(T_SiteMenuEntity data)
        {
            String strSql = "SELECT * FROM [T_AccessControl] WHERE tablename='t_sitemenu' AND node=" + data.ID;
            List<T_AccessControlEntity> acc = db.ExecuteObject<List<T_AccessControlEntity>>(strSql);

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

            CacheHelper.Delete(ISessionKeys.cache_table_accesscontrol);
        }
    }
}