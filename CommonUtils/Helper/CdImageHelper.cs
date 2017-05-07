using System;
using System.Diagnostics;
using System.Web;
/*
 * 示例:
 * CdImageHelper cd = new CdImageHelper();
 * cd.InputFolder = Server.MapPath("/skin");
 * cd.OutputFile = Server.MapPath("/test.iso");
 * cd.CDLabel = "光盘卷标";
 * cd.Convert();
 */
namespace CommonUtils
{
    /// <summary>
    /// 将文件夹打包成ISO格式
    /// </summary>
    public class CdImageHelper
    {
        /// <summary>
        /// 输入的文件夹路径
        /// </summary>
        public String InputFolder { get; set; }
        /// <summary>
        /// ISO文件的输出路径
        /// </summary>
        public String OutputFile { get; set; }
        /// <summary>
        /// 光盘卷标
        /// </summary>
        public String CDLabel { get; set; }
        /// <summary>
        /// 执行操作
        /// </summary>
        public void Convert()
        {
            HttpServerUtility Server = HttpContext.Current.Server;
            DateTime time = DateTime.Now.AddHours(-8);
            String Arguments = String.Format("-l{0} -t{1} -h -o -m -j1 \"{2}\" \"{3}\"", CDLabel, time.ToString("MM/dd/yyyy,HH:mm:ss"), InputFolder, OutputFile);
           
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.FileName = Server.MapPath("~/App_Data/Plugins/cdimage.exe");
            Info.Arguments = Arguments;
            Info.CreateNoWindow = true;
            Info.UseShellExecute = false;
            Info.RedirectStandardError = false;
            Info.RedirectStandardOutput = true;
            Info.WindowStyle = ProcessWindowStyle.Hidden;

            Process p = new Process();
            p.StartInfo = Info;
            p.Start();
            p.WaitForExit();
            p.Close();
        }
    }
}
