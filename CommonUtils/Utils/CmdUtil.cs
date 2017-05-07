using System;
using System.Diagnostics;
using System.Web;

namespace CommonUtils
{
    /// <summary>
    /// 执行cmd.exe命令
    /// </summary>
    public class CmdUtil
    {
        /// <summary>
        /// 执行cmd.exe命令
        /// </summary>
        /// <param name="commandText">命令文本</param>
        /// <returns>命令输出文本</returns>
        public static String ExeCommand(String commandText)
        {
            return ExeCommand(new String[] { commandText });
        }
        /// <summary>
        /// 执行多条cmd.exe命令
        /// </summary>
        /// <param name="commandTexts">命令文本数组</param>
        /// <returns>命令输出文本</returns>
        public static String ExeCommand(String[] commandTexts)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            String strOutput = null;
            try
            {
                p.Start();
                foreach (String item in commandTexts)
                {
                    p.StandardInput.WriteLine(item);
                }
                p.StandardInput.WriteLine("exit");
                strOutput = p.StandardOutput.ReadToEnd();
                //strOutput = Encoding.UTF8.GetString(Encoding.Default.GetBytes(strOutput));
                p.WaitForExit();
                p.Close();
            }
            catch (Exception e)
            {
                strOutput = e.Message;
            }
            return strOutput;
        }
        /// <summary>
        /// 启动外部Windows应用程序，隐藏程序界面
        /// </summary>
        /// <param name="appName">应用程序路径名称</param>
        /// <returns>启动外部Windows应用程序，隐藏程序界面</returns>
        public static Boolean StartApp(String appName)
        {
            return StartApp(appName, ProcessWindowStyle.Hidden);
        }
        /// <summary>
        /// 启动外部应用程序
        /// </summary>
        /// <param name="appName">应用程序路径名称</param>
        /// <param name="style">进程窗口模式</param>
        /// <returns>true表示成功，false表示失败</returns>
        public static Boolean StartApp(String appName, ProcessWindowStyle style)
        {
            return StartApp(appName, null, style);
        }
        /// <summary>
        /// 启动外部应用程序，隐藏程序界面
        /// </summary>
        /// <param name="appName">应用程序路径名称</param>
        /// <param name="arguments">启动参数</param>
        /// <returns>true表示成功，false表示失败</returns>
        public static Boolean StartApp(String appName, String arguments)
        {
            return StartApp(appName, arguments, ProcessWindowStyle.Hidden);
        }
        /// <summary>
        /// 启动外部应用程序
        /// </summary>
        /// <param name="appName">应用程序路径名称</param>
        /// <param name="arguments">启动参数</param>
        /// <param name="style">进程窗口模式</param>
        /// <returns>true表示成功，false表示失败</returns>
        public static Boolean StartApp(String appName, String arguments, ProcessWindowStyle style)
        {
            Boolean blnRst = false;
            Process p = new Process();
            p.StartInfo.FileName = appName;
            p.StartInfo.WindowStyle = style;
            p.StartInfo.Arguments = arguments;
            try
            {
                p.Start();
                p.WaitForExit();
                p.Close();
                blnRst = true;
            }
            catch
            {
            }
            return blnRst;
        }


        #region WINRAR压缩解压

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="fileOrfolder">要压缩的文件目录</param>
        /// <param name="destFileName">要生成的压缩文件名</param>
        public static void Rar(String fileOrfolder, String destFileName)
        {
            ExeCommand(HttpContext.Current.Server.MapPath("~/App_Data/rar.exe")
                + " a \"" + destFileName + "\" \"" + fileOrfolder + "\" -ep1");
        }
        /// <summary>
        /// 解压缩文件
        /// </summary>
        /// <param name="sourceFileName">要解压缩的文件名</param>
        /// <param name="destFolder">解压缩目录</param>
        public static void UnRar(String sourceFileName, String destFolder)
        {
            ExeCommand(HttpContext.Current.Server.MapPath("~/App_Data/rar.exe")
                + " x \"" + sourceFileName + "\" \"" + destFolder + "\" -o+");
            //Arguments = "x -t -o+ -p- " + " " + file + " " + unPath;           
        }

        #endregion
    }
}
