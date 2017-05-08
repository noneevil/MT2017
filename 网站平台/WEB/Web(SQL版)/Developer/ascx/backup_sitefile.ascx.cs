using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using WebSite.BackgroundPages;
using WebSite.Core;
using WebSite.Interface;
using WebSite.Models;

public partial class backup_sitefile : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IJsonResult result = new IJsonResult { Text = "打包失败!", Ico = MessageICO.Failure };
            try
            {
                int i = 0;
                String backfile = "/网站打包[" + DateTime.Now.ToString("yyyy-MM-dd") + "].rar";
                while (File.Exists(Server.MapPath(backfile)))
                {
                    i++;
                    backfile = "/网站打包[" + DateTime.Now.ToString("yyyy-MM-dd") + "]-" + i.ToString("00") + ".rar";
                }
                String rarfile = Server.MapPath(backfile);

                ProcessStartInfo p = new ProcessStartInfo();
                p.FileName = Server.MapPath("/App_Data/Plugins/Rar.exe");
                p.Arguments = String.Format(" a {0} -r -m5", rarfile);
                p.WindowStyle = ProcessWindowStyle.Hidden;
                p.WorkingDirectory = Server.MapPath("/");
                Process process = new Process();
                process.StartInfo = p;
                process.Start();
                process.WaitForExit();
                process.Close();

                Thread.Sleep(300);
                result.Text = "打包成功,文件已存放在网站根目录下!";
                result.Ico = MessageICO.Success;
                result.Status = true;
                result.Data = backfile;

                T_LogsHelper.Append("打包网站：" + backfile, LogsAction.Delete, Admin.UserData);
            }
            catch (Exception ex)
            {
                result.Text = ex.Message;
            }

            Response.Write(JsonConvert.SerializeObject(result));
        }
    }
}