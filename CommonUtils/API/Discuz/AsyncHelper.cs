using System;
using System.IO;
using System.Web;

namespace CommonUtils.Discuz
{
    public class AsyncHelper
    {
        public static Boolean WriteAsyncLog(String action)
        {
            //HttpContext.Current.Request.PhysicalApplicationPath
            try
            {
                String actionString = String.Format("DataTime:{0}--Action:{1}\r\n", DateTime.Now.ToString(), action);
                String fileName = HttpContext.Current.Request.PhysicalApplicationPath + "\\asynclog.txt";
                StreamWriter sw;

                if (File.Exists(fileName))
                {
                    sw = File.AppendText(fileName);

                }
                else
                {
                    sw = File.CreateText(fileName);
                }
                sw.Write(actionString);
                sw.Flush();
                sw.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
