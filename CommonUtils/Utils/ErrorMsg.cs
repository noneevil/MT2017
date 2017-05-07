using System;
using System.IO;
using System.Text;
using System.Web;

namespace CommonUtils
{
    public class ErrorMsg
    {
        public static Boolean isDebug = true;
        /// <summary>
        /// 输出错误代码
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="SQL"></param>
        public static void WriteError(Exception ex, String SQL)
        {
            if (!isDebug) return;
            String fileName = HttpContext.Current.Server.MapPath("~/log/" + DateTime.Now.ToString("yyyyMMdd") + ".log");
            String path = HttpContext.Current.Server.MapPath("~/log/");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("时间：" + DateTime.Now.ToString());
            sb.AppendLine("Message：" + ex.Message);
            sb.AppendLine("Source：" + ex.Source);
            sb.AppendLine("StackTrace：" + ex.StackTrace);
            sb.AppendLine("SQL：" + SQL);
            sb.AppendLine("==========================================================================================");
            sb.AppendLine(" ");
            File.AppendAllText(fileName, sb.ToString());
        }

        /// <summary>
        /// 输出错误代码
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="SQL"></param>
        public static void WriteSQL(String SQL)
        {
            if (!isDebug) return;
            String fileName = HttpContext.Current.Server.MapPath("~/log/SQL_" + DateTime.Now.ToString("yyyyMMdd") + ".log");
            String path = HttpContext.Current.Server.MapPath("~/log/");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("时间：" + DateTime.Now.ToString());
            sb.AppendLine("SQL：" + SQL);
            sb.AppendLine("==========================================================================================");
            sb.AppendLine(" ");
            File.AppendAllText(fileName, sb.ToString());
        }
    }
}
