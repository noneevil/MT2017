using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Core;
using WebSite.Models;
using CommonUtils;

namespace WebSite.Web
{
    public partial class Group : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Groups = T_GroupHelper.Groups;
            if (!IsPostBack)
            {
                BindData();
            }
        }
        private String TableName
        {
            get { return Request["table"]; }
        }
        private List<T_GroupEntity> Groups;
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            //String sql = String.Format("SELECT a.*,IsNull(b.GroupName,'顶级栏目') AS ParentName FROM [t_group] AS a LEFT JOIN [t_group] AS b ON a.ParentID = b.ID WHERE a.tablename='{0}' ORDER BY a.parentid ASC,a.id", TableName);
            //Groups = db.ExecuteObject<List<T_Group>>(sql);
            var groups = Groups.FindAll(a => { return a.TableName == TableName; });
            Repeater1.BindPage<T_GroupEntity>(Pager1, Groups);
        }
        /// <summary>
        /// 列表事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String ID = ((CheckBox)e.Item.FindControl("ID")).Text;
            if (e.CommandName == "del")
            {
                T_GroupEntity data = Groups.Find(a => { return a.ID == Convert.ToInt32(ID); });
                if (data != null)
                {
                    String sql = String.Format("DELETE FROM [t_group] WHERE id in ({0})", ID);
                    db.ExecuteCommand(sql);
                    Groups.Remove(data);
                }
            }
            BindData();
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
                T_GroupEntity data = e.Item.DataItem as T_GroupEntity;
                HtmlTableCell Label1 = e.Item.FindControl("Label1") as HtmlTableCell;
                Label1.InnerText = SetGroupNav(data.ID);
                ImageButton del = e.Item.FindControl("del") as ImageButton;
                del.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 且无法恢复!确定要删除吗?'}});return false;", data.GroupName);
            }
        }
        /// <summary>
        /// 设置分类信息
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private String SetGroupNav(Int32 node)
        {
            List<String> list = new List<String>();
            T_GroupEntity data = Groups.Find(a => { return a.ID == node; });
            list.Add(data.ParentName);
            while (true)
            {
                data = Groups.Find(a => { return a.ID == data.ParentID; });
                if (data == null) break;
                list.Add(data.ParentName);
                if (data.ParentID == 0) break;
            }
            if (list.Count > 1) list.Remove("顶级栏目");
            list.Reverse();
            return String.Join("／", list.ToArray());
        }
        /// <summary>
        /// 分页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Pager1_PageChanged(object sender, EventArgs e)
        {
            BindData();
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Int32 arg = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
            if (arg == -1)
            {
                List<Int32> list = new List<Int32>();
                foreach (RepeaterItem item in Repeater1.Items)
                {
                    CheckBox chkbox = item.FindControl("id") as CheckBox;
                    if (chkbox.Checked) list.Add(Convert.ToInt32(chkbox.Text));
                }

                if (list.Count > 0)
                {
                    String id = String.Join(",", list.ToArray());
                    String sql = String.Format("DELETE FROM [t_group] WHERE id in ({0})", id);
                    db.ExecuteCommand(sql);
                    foreach (Int32 i in list)
                    {
                        T_GroupEntity data = Groups.Find(a => { return a.ID == i; });
                        Groups.Remove(data);
                    }
                }
            }
            BindData();
        }
    }
}