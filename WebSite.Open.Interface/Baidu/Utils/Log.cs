using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WebSite.Interface.Baidu.Utils
{
    public class Log
    {
        static string logFile = @"D:/apidebug.txt";

        public static void Write(string log)
        {
            TextWriter writer = new StreamWriter(logFile,true);
            writer.WriteLine(DateTime.Now.ToString());
            writer.WriteLine(log);
            writer.Close();
        }

    }
}
