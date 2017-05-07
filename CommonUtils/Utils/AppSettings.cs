using System;
using System.Web.Configuration;

namespace CommonUtils
{
    /// <summary>
    /// web.config 配置节点处理类
    /// </summary>
    public class AppSettings
    {
        public static String GetSetting(String name)
        {
            return WebConfigurationManager.AppSettings[name];
        }
        public static void RemoveSetting(String name)
        {
            WebConfigurationManager.AppSettings.Remove(name);
        }
        public static void AddSetting(String name, String value)
        {
            WebConfigurationManager.AppSettings.Add(name, value);
        }
    }
}
