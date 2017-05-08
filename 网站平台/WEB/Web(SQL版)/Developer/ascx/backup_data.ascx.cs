using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using MSSQLDB;
using Newtonsoft.Json;
using WebSite.BackgroundPages;
using WebSite.Core;
using WebSite.Interface;
using WebSite.Models;

public partial class backup_data : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IJsonResult result = new IJsonResult { Text = "备份失败!", Ico = MessageICO.Failure };
            String basedir = AppDomain.CurrentDomain.BaseDirectory;
            FileInfo backfile = new FileInfo(Server.MapPath("/Developer/BackUp/data.bak"));
            if (backfile.Exists) backfile.Delete();

            try
            {
                String sql = String.Format("BACKUP DATABASE data TO DISK = N'{0}' WITH INIT,NOUNLOAD,NAME = N'数据备份',NOSKIP,STATS = 10,NOFORMAT", backfile.FullName);
                db.ExecuteReader(sql);

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

                backfile = new FileInfo(Server.MapPath("/Developer/BackUp/data.bak"));
                if (backfile.Exists) backfile.Delete();

                result.Status = true;
                result.Text = "备份成功!";
                result.Ico = MessageICO.Success;

                T_LogsHelper.Append("备份数据库：" + DateTime.Now.ToString("yyyy年MM月dd日") + ".rar", LogsAction.BackUp, Admin.UserData);
            }
            catch (Exception ex)
            {
                result.Text = ex.Message;
            }

            String json = JsonConvert.SerializeObject(result);
            Response.Write(json);
        }
    }
}