using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MSSQLDB;

public partial class get_navigation_list : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = db.ExecuteDataTable("SELECT * FROM [T_Navigation] ORDER BY sort_id ASC,id ASC");
            write_navigation(dt, 0, "");
        }
    }

    private void write_navigation(DataTable oldData, int parent_id, string parent_name)
    {
        DataRow[] rows = oldData.Select("parent_id=" + parent_id);
        bool isWrite = false;
        for (int i = 0; i < rows.Length; i++)
        {
            //检查是否显示在界面上====================
            bool isActionPass = !Convert.ToBoolean(rows[i]["is_lock"]);

            //如果没有该权限则不显示
            if (!isActionPass)
            {
                if (isWrite && i == (rows.Length - 1) && parent_id > 0 && parent_name != "sys_contents")
                {
                    Response.Write("</ul>\n");
                }
                continue;
            }
            //输出开始标记
            if (i == 0 && parent_id > 0 && parent_name != "sys_contents")
            {
                isWrite = true;
                Response.Write("<ul>\n");
            }
            //以下是输出选项内容=======================
            if (int.Parse(rows[i]["class_layer"].ToString()) == 1)
            {
                Response.Write("<div class=\"list-group\" name=\"" + rows[i]["sub_title"].ToString() + "\">\n");
                if (rows[i]["name"].ToString() != "sys_contents")
                {
                    Response.Write("<h2>" + rows[i]["title"].ToString() + "<i></i></h2>\n");
                }
                //调用自身迭代
                write_navigation(oldData, int.Parse(rows[i]["id"].ToString()), rows[i]["name"].ToString());
                Response.Write("</div>\n");
            }
            else if (int.Parse(rows[i]["class_layer"].ToString()) == 2 && parent_name == "sys_contents")
            {
                Response.Write("<h2>" + rows[i]["title"].ToString() + "<i></i></h2>\n");
                //调用自身迭代
                write_navigation(oldData, int.Parse(rows[i]["id"].ToString()), rows[i]["name"].ToString());
            }
            else
            {
                Response.Write("<li>\n");
                Response.Write("<a navid=\"" + rows[i]["name"].ToString() + "\"");
                if (!string.IsNullOrEmpty(rows[i]["link_url"].ToString()))
                {
                    if (int.Parse(rows[i]["channel_id"].ToString()) > 0)
                    {
                        Response.Write(" href=\"" + rows[i]["link_url"].ToString() + "?channel_id=" + rows[i]["channel_id"].ToString() + "\" target=\"mainframe\"");
                    }
                    else
                    {
                        Response.Write(" href=\"" + rows[i]["link_url"].ToString() + "\" target=\"mainframe\"");
                    }
                }
                Response.Write(" class=\"item\">\n");
                Response.Write("<span>" + rows[i]["title"].ToString() + "</span>\n");
                Response.Write("</a>\n");
                //调用自身迭代
                write_navigation(oldData, int.Parse(rows[i]["id"].ToString()), rows[i]["name"].ToString());
                Response.Write("</li>\n");
            }
            //以上是输出选项内容=======================
            //输出结束标记
            if (i == (rows.Length - 1) && parent_id > 0 && parent_name != "sys_contents")
            {
                Response.Write("</ul>\n");
            }
        }
    }
}