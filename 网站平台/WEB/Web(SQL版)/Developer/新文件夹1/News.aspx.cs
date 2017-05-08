using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using HtmlAgilityPack;
using MSSQLDB;
using ScrapySharp.Extensions;
using WebSite.BackgroundPages;
using WebSite.Core;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class News : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindField(dropField, "t_news", "title");

                #region 权限读取

                List<Int32> nodes = new List<Int32>() { 0 };
                foreach (var item in Access.FindAll(a => { return Admin.IsSuper | ActionTypeHelper.IsView(a.ActionType); }))
                {
                    nodes.Add(item.Node);
                }
                PowerNodes = nodes;

                #endregion

                #region 绑定分类数据

                ListItem element = new ListItem("选择分类", "0");
                element.Selected = true;
                dropGroup.Items.Add(element);
                List<T_GroupEntity> treenodes = T_GroupHelper.Groups.FindAll(a => { return a.TableName == "t_news" && (Admin.IsSuper || nodes.Contains(a.ID)); });
                foreach (var item in treenodes)
                {
                    ListItem el = new ListItem(item.GroupName, item.ID.ToString());
                    el.Attributes.Add("pid", item.ParentID.ToString());
                    dropGroup.Items.Add(el);
                }

                #endregion

                BindData();
            }
        }
        /// <summary>
        /// 分类权限表
        /// </summary>
        private List<T_AccessControlEntity> Access
        {
            get
            {
                return Admin.AccessControl.FindAll(a => { return a.TableName == "t_group"; });
            }
        }
        /// <summary>
        /// 权限节点
        /// </summary>
        private List<Int32> PowerNodes
        {
            get
            {
                Object obj = ViewState["PowerNodes"];
                if (obj == null) return new List<Int32>() { 0 };
                return (List<Int32>)obj;
            }
            set
            {
                ViewState["PowerNodes"] = value;
            }
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        protected void BindData()
        {
            Int32 PageIndex = Pager1.CurrentPageIndex;
            List<String> list = new List<String>();
            String key = txtKey.Text;
            String starttime = starTime.Text;
            String endtime = endTime.Text;

            list.Add("0=0");
            //关键词
            if (!String.IsNullOrEmpty(key))
            {
                list.Add("a." + dropField.SelectedValue + " LIKE '%" + key + "%'");
            }
            //起始日期
            if (!String.IsNullOrEmpty(starttime))
            {
                list.Add("a.pubdate>='" + starttime + "'");
            }
            //结束日期
            if (!String.IsNullOrEmpty(endtime))
            {
                list.Add("a.pubdate<='" + endtime + "'");
            }
            //分类
            if (dropGroup.SelectedValue != "0")
            {
                list.Add("a.groupid=" + dropGroup.SelectedValue);
            }
            if (!Admin.IsSuper)
                list.Add("a.groupid IN(" + String.Join(",", PowerNodes) + ")");

            String filter = String.Join(" AND ", list.ToArray());
            String _sql = "SELECT COUNT(a.id) FROM [t_news] AS a left JOIN [t_group] AS b ON a.groupid = b.id WHERE " + filter;
            String sql = String.Format("SELECT TOP {0} a.id,a.groupid,a.color,a.title,a.pubdate,a.nominate,a.hotspot,a.focus,a.stick,a.status,b.groupname FROM [t_news] AS a LEFT JOIN [t_group] AS b ON a.groupid=b.id WHERE (a.ID NOT IN (SELECT TOP {1} a.id FROM [t_news] AS a WHERE {2} ORDER BY a.id DESC)) AND {2} ORDER BY a.id DESC", Pager1.PageSize, Pager1.PageSize * (PageIndex - 1), filter);

            DataTable dt = db.ExecuteDataTable(sql);
            Pager1.RecordCount = Convert.ToInt32(db.ExecuteScalar(_sql));
            Repeater1.DataSource = dt;
            Repeater1.DataBind();
        }
        /// <summary>
        /// 列表事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String ID = ((CheckBox)e.Item.FindControl("ID")).Text;
            Int32 GroupID = Convert.ToInt32(((HiddenField)e.Item.FindControl("Groupid")).Value);
            T_AccessControlEntity acl = Access.Find(a => { return a.Node == GroupID; });

            if (ActionTypeHelper.IsDelete(acl.ActionType) || ActionTypeHelper.IsSetting(acl.ActionType))
            {
                switch (e.CommandName)
                {
                    case "del":
                        DELContentImages(ID);
                        db.ExecuteCommand(String.Format("DELETE FROM [t_news] WHERE id in ({0})", ID));
                        break;
                    case "focus":
                    case "stick":
                    case "status":
                        ExecuteObject obj = new ExecuteObject();
                        obj.cmdtype = CmdType.UPDATE;
                        obj.tableName = "t_news";
                        obj.terms.Add("id", ID);
                        obj.cells.Add(e.CommandName, !Convert.ToBoolean(e.CommandArgument));
                        db.ExecuteCommand(obj);
                        break;
                }
                BindData();
            }
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
                DataRowView dv = e.Item.DataItem as DataRowView;
                Int32 ID = Convert.ToInt32(dv["id"]);
                Int32 GroupID = Convert.ToInt32(dv["GroupID"]);

                CheckBox chk = e.Item.FindControl("id") as CheckBox;
                ImageButton del = e.Item.FindControl("del") as ImageButton;
                ImageButton edit = e.Item.FindControl("edit") as ImageButton;
                ImageButton status = e.Item.FindControl("status") as ImageButton;
                ImageButton focus = e.Item.FindControl("focus") as ImageButton;
                ImageButton stick = e.Item.FindControl("stick") as ImageButton;

                if (Convert.ToBoolean(dv["status"])) status.ImageUrl = "images/icos/checkbox_yes.png";
                if (Convert.ToBoolean(dv["focus"])) focus.ImageUrl = "images/icos/checkbox_yes.png";
                if (Convert.ToBoolean(dv["stick"])) stick.ImageUrl = "images/icos/checkbox_yes.png";
                edit.OnClientClick = String.Format("javascript:location.href='News_Edit.aspx?id={0}';return false;", ID);
                del.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 且无法恢复!确定要删除吗?'}});return false;", dv["title"]);

                if (!Admin.IsSuper)
                {
                    T_AccessControlEntity acl = Access.Find(a => { return a.Node == GroupID; });
                    if (!ActionTypeHelper.IsEdit(acl.ActionType))
                    {
                        edit.Enabled = false;
                        edit.ToolTip = "无权限修改.";
                        edit.ImageUrl = "images/icos/write_disable.gif";
                    }

                    if (!ActionTypeHelper.IsDelete(acl.ActionType))
                    {
                        del.Enabled = false;
                        del.ToolTip = "无权限删除.";
                        del.ImageUrl = "images/icos/del_disabled.gif";
                    }
                    if (!ActionTypeHelper.IsSetting(acl.ActionType))
                    {
                        stick.Enabled = focus.Enabled = status.Enabled = false;
                        stick.ToolTip = focus.ToolTip = status.ToolTip = "无权限设置.";
                        if (Convert.ToBoolean(dv["status"])) status.ImageUrl = "images/icos/checkbox_disabled.png";
                        if (Convert.ToBoolean(dv["focus"])) focus.ImageUrl = "images/icos/checkbox_disabled.png";
                        if (Convert.ToBoolean(dv["stick"])) stick.ImageUrl = "images/icos/checkbox_disabled.png";
                    }
                }
            }
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
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }
        /// <summary>
        /// 批量删除 批量启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<Int32> list = new List<Int32>() { 0 };
            Int32 arg = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
            if (arg == 0 || arg == 1 || arg == -1)
            {
                foreach (RepeaterItem item in Repeater1.Items)
                {
                    CheckBox chkbox = item.FindControl("id") as CheckBox;
                    HiddenField groupid = item.FindControl("Groupid") as HiddenField;

                    if (chkbox.Checked)
                    {
                        T_AccessControlEntity acl = Access.Find(a => { return a.Node == Convert.ToInt32(groupid.Value); });
                        if (Admin.IsSuper || (ActionTypeHelper.IsSetting(acl.ActionType) || ActionTypeHelper.IsDelete(acl.ActionType)))
                        {
                            list.Add(Convert.ToInt32(chkbox.Text));
                        }
                    }
                }

                String id = String.Join(",", list.ToArray());
                switch (arg)
                {
                    case 0://禁用
                        db.ExecuteCommand(String.Format("UPDATE [t_news] SET status = 0 WHERE id IN({0})", id));
                        break;
                    case -1://删除
                        foreach (var _id in list)
                        {
                            DELContentImages(_id);
                        }
                        db.ExecuteCommand(String.Format("DELETE FROM [t_news] WHERE id in ({0})", id));
                        break;
                    case 1://启用
                        db.ExecuteCommand(String.Format("UPDATE [t_news] SET status = 1 WHERE id IN({0})", id));
                        break;
                }

                BindData();
            }
        }
        /// <summary>
        /// 删除新闻内容图片
        /// </summary>
        /// <param name="id"></param>
        private void DELContentImages(Object id)
        {
            T_NewsEntity data = db.ExecuteObject<T_NewsEntity>("SELECT * FROM [t_news] WHERE id=" + id);

            if (File.Exists(Server.MapPath(data.Attac_Url))) File.Delete(Server.MapPath(data.Attac_Url));
            if (File.Exists(Server.MapPath(data.Video_Url))) File.Delete(Server.MapPath(data.Video_Url));
            if (File.Exists(Server.MapPath(data.Image_Url))) File.Delete(Server.MapPath(data.Image_Url));

            HtmlDocument xml = new HtmlDocument();
            xml.LoadHtml(data.Content + data.Abstract);
            var nodes = xml.DocumentNode.CssSelect("img[src^='/']");
            foreach (HtmlNode n in nodes)
            {
                String src = Server.MapPath(n.Attributes["src"].Value.Trim());
                if (File.Exists(src))
                {
                    File.Delete(src);
                }
            }
        }
    }
}