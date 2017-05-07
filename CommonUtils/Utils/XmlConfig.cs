using System;
using System.IO;
using System.Xml;

namespace CommonUtils
{
    public class XmlConfig
    {
        public static XmlDocument AddAppSetting(XmlDocument xmlDoc, String key, String value)
        {
            XmlNode node = xmlDoc.SelectSingleNode("//appSettings");
            if (node != null)
            {
                XmlElement element;
                XmlNode node2 = node.SelectSingleNode("//add[@key='" + key + "']");
                if (node2 != null)
                {
                    element = (XmlElement)node2;
                    element.SetAttribute("value", value);
                    return xmlDoc;
                }
                element = xmlDoc.CreateElement("add");
                element.SetAttribute("key", key);
                element.SetAttribute("value", value);
                node.AppendChild(element);
            }
            return xmlDoc;
        }

        public static XmlDocument AddConnectionStrings(XmlDocument xmlDoc, String name, String conStr, String providerName)
        {
            XmlNode node = xmlDoc.SelectSingleNode("//connectionStrings");
            if (node != null)
            {
                XmlElement element;
                XmlNode node2 = node.SelectSingleNode("//add[@name='" + name + "']");
                if (node2 != null)
                {
                    element = (XmlElement)node2;
                    element.SetAttribute("connectionString", conStr);
                    element.SetAttribute("providerName", providerName);
                    return xmlDoc;
                }
                element = xmlDoc.CreateElement("add");
                element.SetAttribute("name", name);
                element.SetAttribute("connectionString", conStr);
                element.SetAttribute("providerName", providerName);
                node.AppendChild(element);
            }
            return xmlDoc;
        }

        public static XmlDocument AddElement(XmlDocument xmlDoc, String key, String value)
        {
            XmlNode node = xmlDoc.SelectSingleNode(key);
            if (node != null)
            {
                node.InnerText = value;
                return xmlDoc;
            }
            XmlNode newChild = xmlDoc.CreateElement(key);
            newChild.InnerText = value;
            xmlDoc.AppendChild(newChild);
            return xmlDoc;
        }

        public static String AppSettings(String key)
        {
            return GetAppSetting(Load(AppDomain.CurrentDomain.BaseDirectory + @"/App_Data/WebRes.config"), key);
        }

        public static String ConnectionStrings(String key)
        {
            return GetConnectionStrings(Load(AppDomain.CurrentDomain.BaseDirectory + @"/App_Data/WebRes.config"), key);
        }

        public static XmlElement DeleteAppSetting(XmlDocument xmlDoc, String key)
        {
            XmlElement element = null;
            XmlNode node = xmlDoc.SelectSingleNode("//appSettings");
            if (node != null)
            {
                XmlNode oldChild = node.SelectSingleNode("//add[@key='" + key + "']");
                if (oldChild != null)
                {
                    element = (XmlElement)node.RemoveChild(oldChild);
                }
            }
            return element;
        }

        public static XmlElement DeleteConnectionStrings(XmlDocument xmlDoc, String name)
        {
            XmlElement element = null;
            XmlNode node = xmlDoc.SelectSingleNode("//connectionStrings");
            if (node != null)
            {
                XmlNode oldChild = node.SelectSingleNode("//add[@name='" + name + "']");
                if (oldChild != null)
                {
                    element = (XmlElement)node.RemoveChild(oldChild);
                }
            }
            return element;
        }

        public static String GetAppSetting(XmlDocument xmlDoc, String key)
        {
            XmlNode node = xmlDoc.SelectSingleNode("//appSettings");
            String str = "";
            if (node != null)
            {
                XmlNode node2 = node.SelectSingleNode("//add[@key='" + key + "']");
                if (node2 != null)
                {
                    XmlElement element = (XmlElement)node2;
                    str = element.GetAttributeNode("value").Value;
                }
            }
            return str;
        }

        public static String GetConnectionStrings(XmlDocument xmlDoc, String name)
        {
            String str = "";
            XmlNode node = xmlDoc.SelectSingleNode("//connectionStrings");
            if (node != null)
            {
                XmlNode node2 = node.SelectSingleNode("//add[@name='" + name + "']");
                if (node2 != null)
                {
                    XmlElement element = (XmlElement)node2;
                    str = element.GetAttributeNode("connectionString").Value;
                }
            }
            return str;
        }

        public static XmlDocument Load(String fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("文件 " + fileName + " 不存在!");
            }
            XmlDocument document = new XmlDocument();
            document.Load(fileName);
            return document;
        }

        public static String Save(XmlDocument xmlDoc, String fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("文件 " + fileName + " 不存在!");
            }
            try
            {
                XmlTextWriter w = new XmlTextWriter(fileName, null)
                {
                    Formatting = Formatting.Indented
                };
                xmlDoc.WriteTo(w);
                w.Flush();
                w.Close();
                return "";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }
    }
}
