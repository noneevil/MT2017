using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CommonUtils
{
    public abstract class LogFile
    {
        #region SQL,JS注入式攻击代码分析

        /// <summary>
        /// 处理用户提交的请求
        /// </summary>
        public static void StartProcessRequest()
        {
            const String pattern = "\'|;|and|exec|insert|select|delete%20from|update|count|*|%|chr(|mid(|master|truncate|Char(|declare|drop%20table|from|net%20user|xp_cmdshell|/add|net%20localgroup%20administrators|Asc|Char";
            const String pattern_ex = "\'|;|and|exec|insert|select|delete%20from|update|count|chr(|mid(|master|truncate|Char(|declare|drop%20table|from|net%20user|xp_cmdshell|/add|net%20localgroup%20administrators|Asc|Char";

            #region  POST部份

            if (HttpContext.Current.Request.Form != null)
            {
                foreach (String key in HttpContext.Current.Request.Form)
                {
                    if (Regex.Match(HttpContext.Current.Request.Form[key], Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("非法操作！系统做了如下记录↓<br>");
                        sb.AppendLine("操作ＩＰ：" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                        sb.AppendLine("操作时间：" + DateTime.Now.ToString());
                        sb.AppendLine("操作页面：" + HttpContext.Current.Request.ServerVariables["URL"]);
                        sb.AppendLine("提交方式：Post");
                        sb.AppendLine("提交参数：" + key);
                        sb.AppendLine("提交数据：" + HttpContext.Current.Request.Form[key]);

                        SaveLogFile(sb.ToString());
                        return;
                    }
                }
            }

            #endregion

            #region  GET部份

            if (HttpContext.Current.Request.QueryString != null)
            {
                foreach (String key in HttpContext.Current.Request.QueryString)
                {
                    if (Regex.Match(HttpContext.Current.Request.QueryString[key], Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("非法操作！系统做了如下记录↓<br>");
                        sb.AppendLine("操作ＩＰ：" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                        sb.AppendLine("操作时间：" + DateTime.Now.ToString());
                        sb.AppendLine("操作页面：" + HttpContext.Current.Request.ServerVariables["URL"]);
                        sb.AppendLine("提交方式：Get");
                        sb.AppendLine("提交参数：" + key);
                        sb.AppendLine("提交数据：" + HttpContext.Current.Request.QueryString[key]);

                        SaveLogFile(sb.ToString());
                        return;
                    }
                }
            }

            #endregion

            #region COOKIE部份

            if (HttpContext.Current.Request.Cookies != null)
            {
                foreach (String key in HttpContext.Current.Request.Cookies)
                {
                    String value = HttpContext.Current.Request.Cookies[key].Value;
                    if (Regex.Match(value, Regex.Escape(pattern_ex), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("非法操作！系统做了如下记录↓<br>");
                        sb.AppendLine("操作ＩＰ：" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                        sb.AppendLine("操作时间：" + DateTime.Now.ToString());
                        sb.AppendLine("操作页面：" + HttpContext.Current.Request.ServerVariables["URL"]);
                        sb.AppendLine("提交方式：Cookie");
                        sb.AppendLine("操作页面：" + key);
                        sb.AppendLine("提交数据：" + value);

                        SaveLogFile(sb.ToString());
                        return;
                    }
                }
            }

            #endregion
        }


        #endregion

        public static void SaveLogFile(String logtext)
        {
            StreamWriter sw = null;
            DateTime date = DateTime.Now;
            String FileName = date.Year + "-" + date.Month;
            try
            {
                FileName = HttpContext.Current.Server.MapPath("~/Logs/" + FileName + ".log");
                #region 检测日志目录是否存在
                if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Logs")))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Logs"));
                }

                if (!File.Exists(FileName))
                    sw = File.CreateText(FileName);
                else
                {
                    sw = File.AppendText(FileName);
                }
                #endregion
                sw.WriteLine(logtext);
                sw.WriteLine("**********************************************************************************\r");
                sw.Flush();
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
        }
    }
}
