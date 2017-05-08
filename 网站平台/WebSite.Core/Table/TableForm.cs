using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Serialization;
using CommonUtils;

namespace WebSite.Core.Table
{
    /// <summary>
    /// 数据表表单
    /// </summary>
    [Serializable]
    [XmlType("Table")]
    public class TableForm
    {
        /// <summary>
        /// 关联表编号
        /// </summary>
        [XmlAttribute("TableID")]
        public Guid TableID { get; set; }
        /// <summary>
        /// 关联表名称
        /// </summary>
        [XmlAttribute("TableName")]
        public String TableName { get; set; }
        /// <summary>
        /// 表单集合
        /// </summary>
        [XmlElement("Form")]
        public List<TableFormItem> FromCollections { get; set; }
        /// <summary>
        /// 表单集合
        /// </summary>
        public static List<TableForm> TableForms
        {
            get
            {
                List<TableForm> data;
                if (Cache["TableForms"] == null)
                {
                    data = ConvertHelper.FileToObject<List<TableForm>>(xml);
                    Cache.Insert("TableForms", data, new CacheDependency(xml));
                }
                else
                {
                    data = (List<TableForm>)Cache["TableForms"];
                }
                return data;
            }
            set
            {
                Cache.Insert("TableForms", value, new CacheDependency(xml));
            }
        }
        /// <summary>
        /// 缓存
        /// </summary>
        private static System.Web.Caching.Cache Cache = HttpRuntime.Cache;
        /// <summary>
        /// 配置文件
        /// </summary>
        private static String xml = HttpContext.Current.Server.MapPath("/App_Data/Forms.xml");
        /// <summary>
        /// 保存配置
        /// </summary>
        public static void SaveForms()
        {
            List<TableForm> forms = TableForms;
            if (forms == null) forms = new List<TableForm>();

            forms.Sort(delegate(TableForm t1, TableForm t2)
            {
                return t1.TableName.CompareTo(t2.TableName);
            });

            ConvertHelper.ObjectToFile<List<TableForm>>(forms, xml);
        }
    }
}
