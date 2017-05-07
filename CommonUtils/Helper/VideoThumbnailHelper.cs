using System;
using System.Diagnostics;
using System.Web;

/*
 * 示例:
 * VideoThumbnailHelper v = new VideoThumbnailHelper();
 * v.InputFile = Server.MapPath("/test.mpeg");
 * v.OutputFile = Server.MapPath("/text.jpg");
 * v.Seconds = 10;
 * v.With = 800;
 * v.Height = 600;
 * v.Convert();
 */
namespace CommonUtils
{
    /// <summary>
    /// 生成视频缩略图
    /// </summary>
    public class VideoThumbnailHelper
    {
        /// <summary>
        /// 视频文件
        /// </summary>
        public String InputFile { get; set; }
        /// <summary>
        /// 输出图片文件
        /// </summary>
        public String OutputFile { get; set; }
        /// <summary>
        /// 截取图片宽度
        /// </summary>
        public int With { get; set; }
        /// <summary>
        /// 截取图片高度
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 截取时间(单位:秒)
        /// </summary>
        public int Seconds { get; set; }
        /// <summary>
        /// 执行转换
        /// </summary>
        public void Convert()
        {
            try
            {
                HttpServerUtility Server = HttpContext.Current.Server;
                String Arguments = String.Format(" -i {0} -y -f image2 -ss {1} -s {2}x{3} {4}", InputFile, Seconds, With, Height, OutputFile);
                ProcessStartInfo Info = new ProcessStartInfo();
                Info.FileName = Server.MapPath("~/App_Data/ffmpeg.exe");
                Info.Arguments = Arguments;
                Info.CreateNoWindow = true;
                Info.UseShellExecute = false;
                Info.RedirectStandardError = false;
                Info.RedirectStandardOutput = true;
                Info.WindowStyle = ProcessWindowStyle.Hidden;

                Process Proc = new Process();
                Proc.StartInfo = Info;
                Proc.EnableRaisingEvents = false;
                Proc.Start();
                Proc.WaitForExit();
                if (Proc.HasExited)
                {
                    HttpContext.Current.Response.Write("ok");
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }
        }
    }
}
