using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonUtils;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class News : BaseAdmin
    {
        protected override void OnPreInit(EventArgs e)
        {
            GroupAccessControl = Admin.AccessControl.FindAll(a =>
            {
                return a.TableName == "t_group" && a.ActionType.HasFlag(ActionType.View);
            });
            base.OnPreInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindField(dropField, "t_news", "title");

                #region 获取分类数据

                List<Int32> listnodes = new List<Int32>() { 0 };
                String strSql = String.Format("SELECT a.id,a.groupname,a.parentid,b.actiontype FROM [T_Group] a INNER JOIN [T_AccessControl] b ON a.id = b.node WHERE a.tablename='t_news' AND b.tablename = 't_group' AND b.actiontype<>0 AND b.role={0}", Admin.RoleID);
                DataTable dt = db.ExecuteDataTable(strSql);
                dt.PrimaryKey = new DataColumn[] { dt.Columns["id"] };
                foreach (DataRow dr in dt.Rows)
                {
                    if (!Admin.IsSuper)
                    {
                        if (!((ActionType)dr["actiontype"]).HasFlag(ActionType.View)) continue;
                        //父节点权限检查
                        Boolean flag = false;
                        Int32 parentid = (Int32)dr["parentid"];
                        while (true)
                        {
                            if (parentid == 0) break;
                            DataRow row = dt.Rows.Find(parentid);
                            if (row == null || !((ActionType)row["actiontype"]).HasFlag(ActionType.View))
                            {
                                //无权限
                                flag = true;
                                break;
                            }
                            parentid = (Int32)row["parentid"];
                        }
                        if (flag) continue;
                    }
                    listnodes.Add((Int32)dr["id"]);
                    ListItem node = new ListItem(dr["groupname"].ToString(), dr["id"].ToString());
                    node.Attributes.Add("pid", dr["parentid"].ToString());
                    dropGroup.Items.Add(node);
                }
                ViewNodes = listnodes;
                //绑定分类
                ListItem root = new ListItem("选择分类", "0");
                root.Selected = true;
                dropGroup.Items.Insert(0, root);

                #endregion

                BindData();
            }
        }
        /// <summary>
        /// 分类权限表
        /// </summary>
        private List<T_AccessControlEntity> GroupAccessControl;
        /// <summary>
        /// 拥有权限节点
        /// </summary>
        private List<Int32> ViewNodes
        {
            get
            {
                Object obj = ViewState["ViewNodes"];
                if (obj == null) return new List<Int32>() { 0 };
                return (List<Int32>)obj;
            }
            set
            {
                ViewState["ViewNodes"] = value;
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
            else if (!Admin.IsSuper)
            {
                list.Add("a.groupid IN(" + String.Join(",", ViewNodes) + ")");
            }

            String filter = String.Join(" AND ", list.ToArray());

            String sql = "SELECT COUNT(a.id) FROM [t_news] AS a left JOIN [t_group] AS b ON a.groupid = b.id WHERE " + filter;
            String strSql = String.Format("SELECT TOP {0} a.id,a.groupid,a.color,a.title,a.pubdate,a.isnominate,a.ishotspot,a.isslide,a.isstick,a.isenable,a.isaudit,a.iscomments,a.sortid,a.readaccess,a.voteid,a.click,b.groupname FROM [t_news] AS a LEFT JOIN [t_group] AS b ON a.groupid=b.id WHERE (a.id NOT IN (SELECT TOP {1} a.id FROM [t_news] AS a WHERE {2} ORDER BY a.id DESC)) AND {2} ORDER BY a.id DESC", Pager1.PageSize, Pager1.PageSize * (PageIndex - 1), filter);

            DataTable dt = db.ExecuteDataTable(strSql);
            Pager1.RecordCount = (Int32)(db.ExecuteScalar(sql));
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
            String id = ((CheckBox)e.Item.FindControl("ID")).Text;
            Int32 groupid = Convert.ToInt32(((HiddenField)e.Item.FindControl("hidGroupid")).Value);
            String title = ((HiddenField)e.Item.FindControl("hidTitle")).Value;
            T_AccessControlEntity acl = GroupAccessControl.Find(a => { return a.Node == groupid; });
            switch (e.CommandName)
            {
                case "del":
                    if (Admin.IsSuper || acl.ActionType.HasFlag(ActionType.Delete))
                    {
                        RemoveContent(id);
                    }
                    break;
                case "iscomments":
                case "isstick":
                case "isnominate":
                case "ishotspot":
                case "isslide":
                case "isaudit":
                case "isenable":
                    if (Admin.IsSuper || acl.ActionType.HasFlag(ActionType.Setting))
                    {
                        ExecuteObject obj = new ExecuteObject();
                        obj.cmdtype = CmdType.UPDATE;
                        obj.tableName = "T_News";
                        obj.terms.Add("id", id);
                        obj.cells.Add(e.CommandName, !Convert.ToBoolean(e.CommandArgument));
                        db.ExecuteCommand(obj);

                        AppendLogs(((LinkButton)e.CommandSource).ToolTip + ":" + title, LogsAction.Edit);
                    }
                    break;
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
                DataRowView data = e.Item.DataItem as DataRowView;
                Int32 id = Convert.ToInt32(data["id"].ToString());
                Int32 groupid = Convert.ToInt32(data["GroupID"].ToString());

                ImageButton btndel = (ImageButton)e.Item.FindControl("del");
                ImageButton btnedit = (ImageButton)e.Item.FindControl("edit");

                btnedit.OnClientClick = String.Format("javascript:location.href='News_Edit.aspx?id={0}';return false;", id);
                btndel.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 且无法恢复!确定要删除吗?'}});return false;", data["title"].ToString());

                if (!Admin.IsSuper)
                {
                    T_AccessControlEntity acl = GroupAccessControl.Find(a => { return a.Node == groupid; });
                    if (!acl.ActionType.HasFlag(ActionType.Edit))
                    {
                        btnedit.Enabled = false;
                        btnedit.ToolTip = "无权限修改.";
                        btnedit.ImageUrl = "../skin/icos/write_disable.gif";
                    }

                    if (!acl.ActionType.HasFlag(ActionType.Delete))
                    {
                        btndel.Enabled = false;
                        btndel.ToolTip = "无权限删除.";
                        btndel.ImageUrl = "../skin/icos/del_disabled.gif";
                    }

                    if (!acl.ActionType.HasFlag(ActionType.Setting))
                    {
                        foreach (Control ctl in e.Item.Controls)
                        {
                            if (ctl is LinkButton)
                            {
                                LinkButton c = ((LinkButton)ctl);
                                c.Enabled = false;
                                c.ToolTip = "无权限操作.";
                            }
                        }
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
            Pager1.CurrentPageIndex = 1;
            BindData();
        }
        /// <summary>
        /// 批量删除 批量启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCommand_Click(object sender, EventArgs e)
        {
            List<String> listid = new List<String>();
            List<String> listtitle = new List<String>();

            Int32 arg = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
            if (arg == 0 || arg == 1 || arg == -1)
            {
                foreach (RepeaterItem item in Repeater1.Items)
                {
                    CheckBox chkid = (CheckBox)item.FindControl("id");
                    HiddenField groupid = (HiddenField)item.FindControl("hidGroupid");
                    String title = ((HiddenField)item.FindControl("hidTitle")).Value;

                    if (chkid.Checked)
                    {
                        //检查权限
                        T_AccessControlEntity acl = GroupAccessControl.Find(a => { return a.Node == Convert.ToInt32(groupid.Value); });
                        if (Admin.IsSuper || (acl.ActionType.HasFlag(ActionType.Setting) || acl.ActionType.HasFlag(ActionType.Delete)))
                        {
                            listid.Add(chkid.Text);
                            listtitle.Add(title);
                        }
                    }
                }

                if (listid.Count > 0)
                {
                    String idcoll = String.Join(",", listid.ToArray());
                    String titlecoll = String.Join(",", listtitle.ToArray());
                    switch (arg)
                    {
                        case 0://禁用
                            db.ExecuteCommand(String.Format("UPDATE [T_News] SET isenable = 0 WHERE id IN({0})", idcoll));
                            AppendLogs("批量禁用内容:" + titlecoll, LogsAction.Edit);
                            break;
                        case -1://删除
                            foreach (var _id in listid)
                            {
                                RemoveContent(_id);
                            }
                            break;
                        case 1://启用
                            db.ExecuteCommand(String.Format("UPDATE [T_News] SET isenable = 1 WHERE id IN({0})", idcoll));
                            AppendLogs("批量启用内容:" + titlecoll, LogsAction.Edit);
                            break;
                    }

                    BindData();
                }
            }
        }
        /// <summary>
        /// 删除图片和内容记录
        /// </summary>
        /// <param name="id"></param>
        private void RemoveContent(String id)
        {
            var data = db.ExecuteObject<T_NewsEntity>("SELECT * FROM [T_News] WHERE id=" + id);
            String html = data.Content + data.Abstract;
            html.RemoveImage(data.Attac_Url, data.Video_Url, data.Image_Url);

            db.ExecuteCommand(String.Format("DELETE FROM [T_News] WHERE id = {0}", data.ID));
            AppendLogs("删除内容:" + data.Title, LogsAction.Delete);
        }
    }
}