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
        private List<T_AccessControlEntity> ActionTypeList;
        /// <summary>
        /// 获取权限名称
        /// </summary>
        private Dictionary<String, String> ActionTypeNames;
        /// <summary>
        /// 获取ActionType枚举值
        /// </summary>
        private Array ActionTypeValues;
        /// <summary>
        /// 绑定数据
        /// </summary>
        protected void BindData()
        {
            ActionTypeNames = ActionTypeHelper.ActionList();
            ActionTypeValues = Enum.GetValues(typeof(ActionType));

            //角色已分配权限
            String strSql = "SELECT * FROM [T_AccessControl] WHERE role=" + EditID;
            ActionTypeList = db.ExecuteObject<List<T_AccessControlEntity>>(strSql);

            //功能权限
            strSql = "SELECT id,parentid,layer,title,actiontype,link_url FROM [T_SiteMenu] ORDER BY sortid";
            DataTable dt = db.ExecuteDataTable(strSql);
            DataTable ds = dt.Clone();
            dt.SortTable(ds, 0);
            Repeater1.DataSource = ds;
            Repeater1.DataBind();

            //新闻权限
            strSql = "SELECT id,parentid,layer,groupname,actiontype FROM [T_Group] WHERE tablename='t_news' ORDER BY id";
            dt = db.ExecuteDataTable(strSql);
            ds = dt.Clone();
            dt.SortTable(ds, 0);
            Repeater2.DataSource = ds;
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

            //功能权限
            foreach (RepeaterItem item in Repeater1.Items)
            {
                list.Add(GetItemPower(item, "t_sitemenu"));
            }

            //新闻分类权限
            foreach (RepeaterItem item in Repeater2.Items)
            {
                list.Add(GetItemPower(item, "t_group"));
            }

            db.ExecuteCommand("DELETE FROM [T_AccessControl] WHERE role=" + EditID);
            if (db.ExecuteCommand<List<T_AccessControlEntity>>(list, CmdType.INSERT))
            {
                if (Admin.RoleID == EditID) Admin.Update();

                CacheHelper.Delete(ISessionKeys.cache_table_accesscontrol);

                Alert(Label1, "保存成功!", "line1px_3");

                //日志
                T_UserRoleEntity role = db.ExecuteObject<T_UserRoleEntity>("SELECT * FROM [T_UserRole] WHERE id=" + EditID);
                AppendLogs("修改角色:" + role.Name + "权限！", LogsAction.Edit);
            }
        }
        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="rtpItem"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        private T_AccessControlEntity GetItemPower(RepeaterItem rtpItem, String tablename)
        {
            HiddenField id = (HiddenField)rtpItem.FindControl("id");
            HiddenField hidurl = (HiddenField)rtpItem.FindControl("link_url");
            CheckBoxList chkaction = (CheckBoxList)rtpItem.FindControl("cbaction");

            //权限
            ActionType action = ActionType.None;
            foreach (ListItem n in chkaction.Items)
            {
                if (n.Selected)
                {
                    ActionType value = (ActionType)Enum.Parse(typeof(ActionType), n.Value);
                    action = action | value;
                }
            }

            T_AccessControlEntity acc = new T_AccessControlEntity
            {
                Link_Url = "",
                Role = EditID,
                ActionType = action,
                TableName = tablename,
                Node = Convert.ToInt32(id.Value)
            };
            String strUrl = hidurl.Value.ToLower().Trim();
            if (strUrl.IndexOf("?") > 0) strUrl = strUrl.Substring(0, strUrl.IndexOf("?"));
            acc.Link_Url = strUrl;
            return acc;
        }
        /// <summary>
        /// 绑定权限及美化列表
        /// </summary>
        /// <param name="rtpItem"></param>
        /// <param name="tablename"></param>
        private void BindItemPower(RepeaterItem rtpItem, String tablename)
        {
            DataRowView dv = rtpItem.DataItem as DataRowView;
            Int32 id = Convert.ToInt32(dv["id"].ToString());
            Int32 layer = Convert.ToInt32(dv["layer"].ToString());
            ActionType action = (ActionType)dv["actiontype"];
            CheckBoxList chkaction = (CheckBoxList)rtpItem.FindControl("cbaction");
            T_AccessControlEntity acl = ActionTypeList.Find(a => { return a.Node == id && a.TableName == tablename; });

            rtpItem.SetStyleLayer(layer);

            //绑定权限
            foreach (ActionType item in ActionTypeValues)
            {
                if (!action.HasFlag(item) || item == ActionType.None || item == ActionType.ALL) continue;

                String text = item.ToString();
                ListItem node = new ListItem(ActionTypeNames[text], text);
                if (acl != null && acl.ActionType.HasFlag(item))
                {
                    node.Selected = true;
                }
                chkaction.Items.Add(node);
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
                BindItemPower(e.Item, "t_sitemenu");
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
                BindItemPower(e.Item, "t_group");
            }
        }
    }
}