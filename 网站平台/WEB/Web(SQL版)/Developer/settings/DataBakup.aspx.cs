using System;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CommonUtils;
using WebSite.BackgroundPages;
using WebSite.Models;

namespace WebSite.Web
{
    public partial class DataBakup : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        /// <summary>
        /// 数据绑定
        /// </summary>
        protected void BindData()
        {
            String backdir = Server.MapPath("/Developer/BackUp");
            if (!Directory.Exists(backdir)) Directory.CreateDirectory(backdir);
            DirectoryInfo directory = new DirectoryInfo(backdir);
            FileInfo[] files = directory.GetFiles("*.rar");

            Repeater1.BindPage(Pager1, files, files.Length);
        }
        /// <summary>
        /// 列表事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            String file = Server.UrlDecode(((HtmlInputCheckBox)e.Item.FindControl("ID")).Value);
            String _file = "/" + HttpUtility.UrlDecode(new Uri(Server.MapPath("/")).MakeRelativeUri(new Uri(file)).ToString());
            switch (e.CommandName)
            {
                case "del":
                    {
                        if (File.Exists(file))
                        {
                            File.Delete(file);

                            AppendLogs("删除数据库备份文件：" + _file, LogsAction.Delete);

                            BindData();
                        }
                    }
                    break;
                case "save":
                    {
                        FileInfo info = new FileInfo(file);
                        long fileSize = info.Length;
                        Response.Clear();
                        Response.ContentType = "application/octet-stream";
                        Response.AddHeader("Content-Disposition", "attachement;filename=" + HttpUtility.UrlPathEncode(Path.GetFileName(file)));
                        Response.AddHeader("Content-Length", fileSize.ToString());
                        Response.WriteFile(file, 0, fileSize);

                        AppendLogs("下载数据库备份文件：" + _file, LogsAction.Download);

                        Response.Flush();
                        Response.Close();
                    }
                    break;
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
                FileSystemInfo file = e.Item.DataItem as FileSystemInfo;
                ImageButton btndel = (ImageButton)e.Item.FindControl("del");
                btndel.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 且无法恢复!确定要删除吗?'}});return false;", HttpUtility.HtmlEncode(file.Name));
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
        /// 批量删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCommand_Click(object sender, EventArgs e)
        {
            Int32 arg = Convert.ToInt32(Request.Form["__EVENTARGUMENT"]);
            if (arg == -1)
            {
                foreach (RepeaterItem item in Repeater1.Items)
                {
                    HtmlInputCheckBox chkid = item.FindControl("ID") as HtmlInputCheckBox;
                    if (chkid.Checked)
                    {
                        FileInfo file = new FileInfo(Server.UrlDecode(chkid.Value));
                        if (file.Exists)
                        {
                            file.Delete();

                            String _file = "/" + HttpUtility.UrlDecode(new Uri(Server.MapPath("/")).MakeRelativeUri(new Uri(file.FullName)).ToString());
                            AppendLogs("删除数据库备份文件：" + HttpUtility.UrlDecode(_file), LogsAction.Delete);
                        }
                    }
                }
                BindData();
            }
        }
    }
}