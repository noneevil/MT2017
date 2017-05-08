using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using MSSQLDB;
using WebSite.BackgroundPages;
using WebSite.Core;
using WebSite.Models;
using CommonUtils;

namespace WebSite.Web
{
    public partial class Ad : BaseAdmin
    {
        protected void Page_Load(Object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            String sql = "SELECT * FROM [t_ad] ORDER BY id DESC";
            DataTable dt = db.ExecuteDataTable(sql);
            Repeater1.BindPage(Pager1, dt.DefaultView, dt.Rows.Count);
        }
        /// <summary>
        /// 广告类型
        /// </summary>
        /// <returns></returns>
        protected Boolean isFlv()
        {
            AdCategory data = (AdCategory)Enum.Parse(typeof(AdCategory), Eval("groupid").ToString(), true);
            return data == AdCategory.FLV视频;
        }
        /// <summary>
        /// 列表事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String ID = ((CheckBox)e.Item.FindControl("ID")).Text;
            HiddenField title = e.Item.FindControl("title") as HiddenField;
            if (e.CommandName == "del")
            {
                db.ExecuteCommand(String.Format("DELETE FROM [t_ad] WHERE id in ({0})", ID));
                T_LogsHelper.Append("删除广告:" + title.Value, Admin.ID);
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
                DataRowView dv = e.Item.DataItem as DataRowView;
                Int32 ID = Convert.ToInt32(dv["id"]);

                ImageButton del = e.Item.FindControl("del") as ImageButton;
                del.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 且无法恢复!确定要删除吗?'}});return false;", dv["title"]);

                AdCategory adtype = (AdCategory)Enum.Parse(typeof(AdCategory), dv["groupid"].ToString(), true);
                PlaceHolder placeholder1 = e.Item.FindControl("PlaceHolder1") as PlaceHolder;
                PlaceHolder placeholder2 = e.Item.FindControl("PlaceHolder2") as PlaceHolder;
                if (adtype == AdCategory.FLV视频)
                {
                    placeholder1.Visible = false;
                    placeholder2.Visible = true;
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
        /// 批量删除 批量启用　批量禁用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<Int32> list = new List<Int32>();
            List<String> ads = new List<String>();
            foreach (RepeaterItem item in Repeater1.Items)
            {
                CheckBox chk = item.FindControl("id") as CheckBox;
                HiddenField ad = item.FindControl("title") as HiddenField;
                if (chk.Checked)
                {
                    list.Add(Convert.ToInt32(chk.Text));
                    ads.Add(ad.Value);
                }
            }

            if (list.Count > 0)
            {
                String id = String.Join(",", list.ToArray());
                Int32 arg = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
                if (arg == -1)
                {
                    db.ExecuteCommand(String.Format("DELETE FROM [t_ad] WHERE id in ({0})", id));
                    T_LogsHelper.Append("批量删除广告:" + String.Join(",", ads.ToArray()), Admin.ID);
                }
            }

            BindData();
        }
    }
}