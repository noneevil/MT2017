using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Serialization;
using CommonUtils;
using Newtonsoft.Json;

namespace WebSite.Models
{
    /// <summary>
    /// 回复模板
    /// </summary>
    [XmlType("item")]
    [Serializable]
    public class ReplyTemplate
    {
        /// <summary>
        /// 模板名称
        /// </summary>
        [XmlAttribute]
        public String name { get; set; }
        /// <summary>
        /// 模板内容
        /// </summary>
        [XmlIgnore]
        public String Context { get; set; }
        //[XmlElement("Context")]
        [XmlText]
        [JsonIgnore]
        public XmlNode[] CDataContent
        {
            get
            {
                var dummy = new XmlDocument();
                return new XmlNode[] { dummy.CreateCDataSection(Context) };
            }
            set
            {
                if (value == null)
                {
                    Context = null;
                    return;
                }
                if (value.Length != 1)
                {
                    throw new InvalidOperationException(
                        String.Format(
                            "Invalid array length {0}", value.Length));
                }
                Context = value[0].Value;
            }
        }


        /// <summary>
        /// 静态调用
        /// </summary>
        public static List<ReplyTemplate> Config
        {
            get
            {
                List<ReplyTemplate> data;
                if (Cache["ReplyTemplate"] == null)
                {
                    data = ConvertHelper.FileToObject<List<ReplyTemplate>>(xml);
                    Cache.Insert("ReplyTemplate", data, new CacheDependency(xml));
                }
                else
                {
                    data = (List<ReplyTemplate>)Cache["ReplyTemplate"];
                }
                return data;
            }
            set
            {
                Cache.Insert("ReplyTemplate", value, new CacheDependency(xml));
            }
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        public static void SaveConfig()
        {
            List<ReplyTemplate> config = Config;
            ConvertHelper.ObjectToFile<List<ReplyTemplate>>(config, xml);
        }
        /// <summary>
        /// 配置文件
        /// </summary>
        private static String xml = HttpContext.Current.Server.MapPath("/App_Data/ReplyTemplate.xml");
        /// <summary>
        /// 缓存
        /// </summary>
        private static Cache Cache = HttpRuntime.Cache;
    }
}
