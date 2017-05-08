using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSSQLDB;
using Newtonsoft.Json;
using WebSite.BackgroundPages;
using WebSite.Models;
using WebSite.Interface;
using CommonUtils;

namespace WebSite.Web
{
    public partial class DataBakup : BaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                switch (Request["action"])
                {
                    case "backup":
                        BakUp();
                        break;
                    default:
                        BindData();
                        break;
                }
            }
        }
        /// <summary>
        /// 数据绑定
        /// </summary>
        protected void BindData()
        {
            String _dir = Server.MapPath("/Developer/BackUp");
            if (!Directory.Exists(_dir)) Directory.CreateDirectory(_dir);
            DirectoryInfo directory = new DirectoryInfo(_dir);
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
            switch (e.CommandName)
            {
                case "del":
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                        BindData();
                    }
                    break;
                case "save":
                    FileInfo info = new FileInfo(file);
                    long fileSize = info.Length;
                    Response.Clear();
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", "attachement;filename=" + HttpUtility.UrlPathEncode(Path.GetFileName(file)));
                    Response.AddHeader("Content-Length", fileSize.ToString());
                    Response.WriteFile(file, 0, fileSize);
                    Response.Flush();
                    Response.Close();
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
                ImageButton del = e.Item.FindControl("del") as ImageButton;
                del.OnClientClick = String.Format("javascript:dialogConfirm({{el:this,text:'将删除 {0} 且无法恢复!确定要删除吗?'}});return false;", HttpUtility.HtmlEncode(file.Name));
            }
        }
        /// <summary>
        /// 执行备份
        /// </summary>
        /// <returns></returns>
        protected void BakUp()
        {
            IJsonResult result = new IJsonResult { Status = false, Text = "备份失败!", Ico = MessageICO.Failure };
            String basedir = AppDomain.CurrentDomain.BaseDirectory;
            FileInfo backfile = new FileInfo(Server.MapPath("/Developer/BackUp/data.bak"));
            if (backfile.Exists) backfile.Delete();
            db.ExecuteReader(String.Format("BACKUP DATABASE data TO DISK = N'{0}' WITH INIT,NOUNLOAD,NAME = N'数据备份',NOSKIP,STATS = 10,NOFORMAT", backfile.FullName));

            try
            {
                ProcessStartInfo p = new ProcessStartInfo();
                p.FileName = Server.MapPath("/App_Data/Plugins/Rar.exe");
                p.Arguments = String.Format(" a {0}.rar -ep {1}", DateTime.Now.ToString("yyyy年MM月dd日"), backfile.FullName);
                p.WindowStyle = ProcessWindowStyle.Hidden;
                p.WorkingDirectory = Server.MapPath("/Developer/BackUp");
                Process process = new Process();
                process.StartInfo = p;
                process.Start();
                process.WaitForExit();
                process.Close();

                Thread.Sleep(300);
                if (backfile.Exists) backfile.Delete();
                result.Status = true;
                result.Text = "备份成功!";                
                result.Ico = MessageICO.Success;
            }
            catch (Exception ex)
            {
                result.Text = ex.Message;
            }

            Response.Write(JsonConvert.SerializeObject(result));
            Response.End();
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
                foreach (RepeaterItem item in Repeater1.Items)
                {
                    HtmlInputCheckBox chkbox = item.FindControl("id") as HtmlInputCheckBox;
                    if (chkbox.Checked)
                    {
                        FileInfo file = new FileInfo(Server.UrlDecode(chkbox.Value));
                        if (file.Exists) file.Delete();
                    }
                }
                BindData();
            }
        }
    }
}