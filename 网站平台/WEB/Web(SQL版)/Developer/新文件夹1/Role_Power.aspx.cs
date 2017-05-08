using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Core;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class Role_Power : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        /// <summary>
        /// 用户权限列表
        /// </summary>
        private List<T_AccessControlEntity> data;
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
        /// 绑定数据
        /// </summary>
        protected void BindData()
        {
            //角色已分配权限
            String sql = "SELECT * FROM [T_AccessControl] WHERE role=" + EditID;
            data = db.ExecuteObject<List<T_AccessControlEntity>>(sql);

            //功能权限
            sql = "SELECT a.id,a.menuname,a.action, IsNull(b.menuName ,'顶级分类') AS pname FROM [T_SiteMenu] AS a LEFT JOIN [T_SiteMenu] AS b ON a.[ParentID] = b.id ORDER BY a.parentid,a.sort";
            DataTable dt = db.ExecuteDataTable(sql);
            Repeater1.DataSource = dt;
            Repeater1.DataBind();

            //新闻权限
            sql = "SELECT a.id,a.groupname,IsNull(b.GroupName ,'顶级分类') AS pname FROM [T_Group] AS a LEFT JOIN [T_Group] AS b ON a.[ParentID] = b.id ORDER BY a.parentid";
            dt = db.ExecuteDataTable(sql);
            Repeater2.DataSource = dt;
            Repeater2.DataBind();
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<T_AccessControlEntity> list = new List<T_AccessControlEntity>();

            #region 功能权限

            foreach (RepeaterItem item in Repeater1.Items)
            {
                HiddenField id = item.FindControl("id") as HiddenField;
                HiddenField url = item.FindControl("url") as HiddenField;
                CheckBox chk = item.FindControl("CheckBox1") as CheckBox;
                ActionType isview = chk.Checked ? ActionType.View : ActionType.None;
                String strUrl = Path.GetFileName(url.Value).ToLower();
                if (strUrl.IndexOf("?") > 0) strUrl = strUrl.Substring(0, strUrl.IndexOf("?"));
                list.Add(new T_AccessControlEntity
                {
                    TableName = "t_sitemenu",
                    Node = Convert.ToInt32(id.Value),
                    Link_Url = strUrl,
                    ActionType = isview,
                    Role = EditID
                });
            }

            #endregion

            #region 新闻权限

            foreach (RepeaterItem item in Repeater2.Items)
            {
                ActionType power = ActionType.None;
                HiddenField id = item.FindControl("id") as HiddenField;

                if (((CheckBox)item.FindControl("CheckBox1")).Checked) power = power | ActionType.Create;
                if (((CheckBox)item.FindControl("CheckBox2")).Checked) power = power | ActionType.View;
                if (((CheckBox)item.FindControl("CheckBox3")).Checked) power = power | ActionType.Edit;
                if (((CheckBox)item.FindControl("CheckBox4")).Checked) power = power | ActionType.Delete;
                if (((CheckBox)item.FindControl("CheckBox5")).Checked) power = power | ActionType.Setting;

                list.Add(new T_AccessControlEntity
                {
                    Role = EditID,
                    TableName = "t_group",
                    Node = Convert.ToInt32(id.Value),
                    ActionType = power
                });
            }

            #endregion

            db.ExecuteCommand("DELETE FROM [T_AccessControl] WHERE role=" + EditID);
            if (db.ExecuteCommand<List<T_AccessControlEntity>>(list, CmdType.INSERT))
            {
                if (Admin.RoleID == EditID) Admin.Update();
                Alert(Label1, "保存成功!", "line1px_3");
            }
        }
        /// <summary>
        /// 功能权限绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dv = e.Item.DataItem as DataRowView;
                Int32 id = Convert.ToInt32(dv["id"]);
                T_AccessControlEntity acl = data.Find(a => { return a.Node == id && a.TableName == "t_sitemenu"; });
                if (acl != null)
                {
                    ((CheckBox)e.Item.FindControl("CheckBox1")).Checked = ActionTypeHelper.IsView(acl.ActionType);
                }
            }
        }
        /// <summary>
        /// 新闻权限绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Repeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dv = e.Item.DataItem as DataRowView;
                Int32 id = Convert.ToInt32(dv["id"]);
                T_AccessControlEntity acl = data.Find(a => { return a.Node == id && a.TableName == "t_group"; });
                if (acl != null)
                {
                    ((CheckBox)e.Item.FindControl("CheckBox1")).Checked = ActionTypeHelper.IsCreat(acl.ActionType);
                    ((CheckBox)e.Item.FindControl("CheckBox2")).Checked = ActionTypeHelper.IsView(acl.ActionType);
                    ((CheckBox)e.Item.FindControl("CheckBox3")).Checked = ActionTypeHelper.IsEdit(acl.ActionType);
                    ((CheckBox)e.Item.FindControl("CheckBox4")).Checked = ActionTypeHelper.IsDelete(acl.ActionType);
                    ((CheckBox)e.Item.FindControl("CheckBox5")).Checked = ActionTypeHelper.IsSetting(acl.ActionType);
                }
            }
        }
    }
}