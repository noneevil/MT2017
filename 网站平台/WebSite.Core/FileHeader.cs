using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Xml.Serialization;
using CommonUtils;

namespace WebSite.Core
{
    /// <summary>
    /// 文件头结构信息
    /// </summary>
    [Serializable]
    [XmlType("item")]
    public class FileHeader
    {
        /// <summary>
        /// 文件后辍名
        /// </summary>
        [XmlAttribute("extension")]
        public String Extension { get; set; }
        /// <summary>
        /// 文件头结构
        /// </summary>
        [XmlAttribute("header")]
        public List<Byte> Header { get; set; }

        /// <summary>
        /// 静态调用
        /// </summary>
        public static List<FileHeader> Headers
        {
            get
            {
                List<FileHeader> data;
                if (Cache["FileHeader"] == null)
                {
                    data = ConvertHelper.FileToObject<List<FileHeader>>(xml);
                    Cache.Insert("FileHeader", data, new CacheDependency(xml));
                }
                else
                {
                    data = (List<FileHeader>)Cache["FileHeader"];
                }
                return data;
            }
            set
            {
                Cache.Insert("FileHeader", value, new CacheDependency(xml));
            }
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        public static void SaveHeaders()
        {
            List<FileHeader> data = Headers;
            ConvertHelper.ObjectToFile<List<FileHeader>>(data, xml);
        }
        /// <summary>
        /// 配置文件
        /// </summary>
        private static String xml = HttpContext.Current.Server.MapPath("/App_Data/FileHeader.xml");
        /// <summary>
        /// 缓存
        /// </summary>
        private static Cache Cache = HttpRuntime.Cache;
    }
}