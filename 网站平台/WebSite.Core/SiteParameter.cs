using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Serialization;
using CommonUtils;
using MSSQLDB;
using WebSite.Interface;
using WebSite.Models;

namespace WebSite.Core
{
    /// <summary>
    /// 站点参数配置
    /// </summary>
    [Serializable]
    //[XmlType("Root")]
    public class SiteParameter
    {
        /// <summary>
        /// 替换字符
        /// </summary>
        public String SubStitute { get; set; }
        /// <summary>
        /// 会员注册审核
        /// </summary>
        public Boolean AuditReg { get; set; }
        /// <summary>
        /// 留言审核
        /// </summary>
        public Boolean AuditBook { get; set; }
        /// <summary>
        /// 邮件服务器
        /// </summary>
        public String Smtp { get; set; }
        /// <summary>
        /// 邮件地址
        /// </summary>
        public String Email { get; set; }
        /// <summary>
        /// 邮箱用户
        /// </summary>
        public String MailUser { get; set; }
        /// <summary>
        /// 邮箱密码
        /// </summary>
        public String MailPass { get; set; }
        /// <summary>
        /// 记录日志
        /// </summary>
        public Boolean RecordLog { get; set; }
        /// <summary>
        /// 后台文件虚拟
        /// </summary>
        public Boolean VirtualDeveloper { get; set; }
        /// <summary>
        /// 私钥
        /// </summary>
        public String Keys { get; set; }
        /// <summary>
        /// 授权密钥
        /// </summary>
        public Guid LicenseKey { get; set; }
        /// <summary>
        /// 存储视图方式
        /// </summary>
        public ViewStateType ViewStateMode { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public List<Language> Languages { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        [XmlIgnore]
        public DatabaseType DataType
        {
            get
            {
                var data = DataTypeOptions.Find(a => { return a.Selected == true; });
                if (data == null) return DatabaseType.SqlServer;
                return (DatabaseType)Enum.Parse(typeof(DatabaseType), data.Name, true);
            }
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        [XmlIgnore]
        public String ConnectionString
        {
            get
            {
                var data = DataTypeOptions.Find(a => { return a.Selected == true; });
                if (data == null) return String.Empty;
                return data.ConnectionString;
            }
        }
        /// <summary>
        /// 数据库连接选项
        /// </summary>
        [XmlArrayItem("item")]
        public List<WebSiteDataType> DataTypeOptions { get; set; }
        /// <summary>
        /// 过滤关键字
        /// </summary>
        //[XmlIgnore]
        //public String BadKeywords { get; set; }
        //[XmlElement("BadKeywords")]
        //[XmlIgnore]
        //public XmlNode[] CDataContent
        //{
        //    get
        //    {
        //        var dummy = new XmlDocument();
        //        return new XmlNode[] { dummy.CreateCDataSection(BadKeywords) };
        //    }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            BadKeywords = null;
        //            return;
        //        }
        //        if (value.Length != 1)
        //        {
        //            throw new InvalidOperationException(
        //                String.Format(
        //                    "Invalid array length {0}", value.Length));
        //        }
        //        BadKeywords = value[0].Value;
        //    }
        //}
        /// <summary>
        /// 过滤关键字
        /// </summary>
        [XmlArray("BadKeywords"), XmlArrayItem("item")]
        public List<String> BadKeywords { get; set; }
        /// <summary>
        /// SQL过滤关键字
        /// </summary>
        [XmlArray("SqlFilterWord"), XmlArrayItem("item")]
        public List<String> SqlFilterWord { get; set; }
        /// <summary>
        /// 文件类型编码
        /// </summary>
        [XmlArray("MimeType"), XmlArrayItem("item")]
        public List<MimeType> MimeType { get; set; }

        /// <summary>
        /// 静态调用
        /// </summary>
        public static SiteParameter Config
        {
            get
            {
                SiteParameter data;
                if (Cache[ISessionKeys.cache_siteparameter] == null)
                {
                    data = ConvertHelper.FileToObject<SiteParameter>(xml);
                    Cache.Insert(ISessionKeys.cache_siteparameter, data, new CacheDependency(xml));
                }
                else
                {
                    data = (SiteParameter)Cache[ISessionKeys.cache_siteparameter];
                }
                return data;
            }
            set
            {
                Cache.Insert(ISessionKeys.cache_siteparameter, value, new CacheDependency(xml));
            }
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        public static void SaveConfig()
        {
            SiteParameter config = Config;
            config.Languages.Sort(delegate(Language t1, Language t2)
            {
                return t1.language.CompareTo(t2.language);
            });

            ConvertHelper.ObjectToFile<SiteParameter>(config, xml);
        }
        /// <summary>
        /// 配置文件
        /// </summary>
        private static String xml = HttpContext.Current.Server.MapPath("/App_Data/Param.xml");
        /// <summary>
        /// 缓存
        /// </summary>
        private static Cache Cache = HttpRuntime.Cache;

        #region 语言

        public static Language CN
        {
            get { return Config.Languages.Find(a => { return a.language == "CN"; }); }
        }
        public static Language TW
        {
            get { return Config.Languages.Find(a => { return a.language == "TW"; }); }
        }
        public static Language US
        {
            get { return Config.Languages.Find(a => { return a.language == "US"; }); }
        }
        #endregion
    }
    /// <summary>
    /// 站点语言
    /// </summary>
    [Serializable]
    [XmlType("item")]
    public class Language
    {
        /// <summary>
        /// 站点语言
        /// </summary>
        [XmlAttribute("Language")]
        public String language { get; set; }
        /// <summary>
        /// 站点名称
        /// </summary>
        public String SiteName { get; set; }
        /// <summary>
        /// 站点域名
        /// </summary>
        public String SiteURL { get; set; }
        /// <summary>
        /// SEO关键字
        /// </summary>
        public String KeyWords { get; set; }
        /// <summary>
        /// SEO介绍信息
        /// </summary>
        public String Description { get; set; }
        /// <summary>
        /// 版权信息
        /// </summary>
        [XmlIgnore]
        public String Copyright { get; set; }
        [XmlElement("Copyright")]
        public XmlNode[] CDataContent
        {
            get
            {
                var dummy = new XmlDocument();
                return new XmlNode[] { dummy.CreateCDataSection(Copyright) };
            }
            set
            {
                if (value == null)
                {
                    Copyright = null;
                    return;
                }

                if (value.Length != 1)
                {
                    throw new InvalidOperationException(String.Format("Invalid array length {0}", value.Length));
                }

                Copyright = value[0].Value;
            }
        }
    }
    /// <summary>
    /// 站点数据库
    /// </summary>
    [Serializable]
    //[XmlType("item")]
    public class WebSiteDataType
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        [XmlAttribute("Name")]
        public String Name { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        [XmlAttribute("ConnectionString")]
        public String ConnectionString { get; set; }
        /// <summary>
        /// 提供库名全称
        /// </summary>
        [XmlAttribute("ProviderName")]
        public String ProviderName { get; set; }
        /// <summary>
        /// 默认选中
        /// </summary>
        [XmlAttribute("Selected")]
        public Boolean Selected { get; set; }
    }
}
