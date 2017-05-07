using System;
using System.Diagnostics;
using System.Web;
/*  
 * 示例:
 *  FlashPaperHelper paper = new FlashPaperHelper();
 *  paper.InputFile = Server.MapPath("/02.jpg");
 *  paper.OutputFile = Server.MapPath("/test.swf");
 *  paper.Convert();
 */
namespace CommonUtils
{
    /// <summary>
    /// 结合FlashPaper虚列打印机实现将PPT/WORD/EXCEL/IMG等直接转换为PDF/SWF文件
    /// </summary>
    public class FlashPaperHelper
    {
        /// <summary>
        /// 要转换的文件
        /// </summary>
        public String InputFile { get; set; }
        /// <summary>
        /// 转换后保存的文件
        /// </summary>
        public String OutputFile { get; set; }
        /// <summary>
        /// 执行转换
        /// </summary>
        public void Convert()
        {
            HttpServerUtility Server = HttpContext.Current.Server;

            Process p = new Process();
            p.StartInfo.FileName = Server.MapPath(@"\App_Data\FlashPaper\FlashPrinter.exe");
            p.StartInfo.Arguments = InputFile + " -o " + OutputFile;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.Start();
            p.WaitForExit();
            p.Close();
        }
    }
}
