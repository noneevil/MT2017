using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSite.BackgroundPages;
using WebSite.Models;
using System.Data;
using MSSQLDB;
using System.Web.UI.HtmlControls;

public partial class Admin_TreeNode : BaseAdmin
{
    protected void Page_Load(object sender, EventArgs e)
    {
        data = db.ExecuteDataTable("select * from t_group");
        //DataView dv = new DataView(data);
        //dv.RowFilter = "parentid=0";
        //Repeater1.DataSource = dv;
        //Repeater1.DataBind();
        BindTreeNode(0, PlaceHolder1);
    }
    /// <summary>
    /// 层级
    /// </summary>
    private Int32 level;
    /// <summary>
    /// 数据源
    /// </summary>
    private DataTable data;

    private void BindTreeNode(Int32 parentid, Control ctl)
    {
        DataView view = new DataView(data);
        view.RowFilter = "parentid=" + parentid;
        foreach (DataRowView dv in view)
        {
            Int32 id = Convert.ToInt32(dv["id"]);
            HtmlGenericControl li = new HtmlGenericControl("li");
            li.Attributes.Add("class", "level" + level);

            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes.Add("class", "button level" + level + " switch root_open");

            HtmlGenericControl a = new HtmlGenericControl("a");
            a.Attributes.Add("class", "level" + level);

            HtmlGenericControl a_span_ico = new HtmlGenericControl("span");
            a_span_ico.Attributes.Add("class", "button ico_open");

            HtmlGenericControl a_span_text = new HtmlGenericControl("span");
            a_span_text.InnerText = dv["groupname"].ToString();

            a.Controls.Add(a_span_ico);
            a.Controls.Add(a_span_text);
            //span.Controls.Add(a);
            li.Controls.Add(span);
            li.Controls.Add(a);
            ctl.Controls.Add(li);

            DataView view2 = new DataView(data);
            view2.RowFilter = "parentid=" + id;
            if (view2.Count > 0)
            {
                HtmlGenericControl ul = new HtmlGenericControl("ul");
                ul.Attributes.Add("class", "level" + level);
                li.Controls.Add(ul);
                BindTreeNode(id, ul);
            }
        }
        level++;
    }
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Repeater1_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView dv = e.Item.DataItem as DataRowView;
            HtmlGenericControl li = new HtmlGenericControl("li");
            li.Attributes.Add("class", "level" + level);

            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes.Add("class", "button level" + level + " switch root_open");

            HtmlGenericControl a = new HtmlGenericControl("a");
            a.Attributes.Add("class", "level" + level);

            HtmlGenericControl a_span_ico = new HtmlGenericControl("span");
            a_span_ico.Attributes.Add("class", "button ico_open");

            HtmlGenericControl a_span_text = new HtmlGenericControl("span");
            a_span_text.InnerText = dv["groupname"].ToString();



            a.Controls.Add(a_span_ico);
            a.Controls.Add(a_span_text);
            //span.Controls.Add(a);
            li.Controls.Add(span);
            li.Controls.Add(a);
            e.Item.Controls.Add(li);
        }
    }

    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

    }
}