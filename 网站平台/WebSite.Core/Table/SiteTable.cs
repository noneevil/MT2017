using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Serialization;
using CommonUtils;
using MSSQLDB;

namespace WebSite.Core.Table
{
    /// <summary>
    /// 数据表
    /// </summary>
    [Serializable]
    [XmlType("Table")]
    public class SiteTable
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        [XmlAttribute("ID")]
        public Guid ID { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        [XmlAttribute("TableName")]
        public String TableName { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        [XmlAttribute("Description")]
        public String Description { get; set; }
        /// <summary>
        /// 字段集合
        /// </summary>
        [XmlElement("Field")]
        public List<Field> Columns { get; set; }




        /// <summary>
        /// 数据库表架构信息
        /// </summary>
        public static List<SiteTable> Tables
        {
            get
            {
                List<SiteTable> data;
                if (Cache["SiteTables"] == null)
                {
                    data = ConvertHelper.FileToObject<List<SiteTable>>(xml);
                    Cache.Insert("SiteTables", data, new CacheDependency(xml));
                }
                else
                {
                    data = (List<SiteTable>)Cache["SiteTables"];
                }
                return data;
            }
            set
            {
                Cache.Insert("SiteTables", value, new CacheDependency(xml));
            }
        }
        /// <summary>
        /// 缓存
        /// </summary>
        private static System.Web.Caching.Cache Cache = HttpRuntime.Cache;
        /// <summary>
        /// 配置文件
        /// </summary>
        private static String xml = HttpContext.Current.Server.MapPath("/App_Data/Table.xml");
        /// <summary>
        /// 刷新数据库
        /// </summary>
        public static void RefreshTable()
        {
            List<SiteTable> tables = (List<SiteTable>)Cache["SiteTables"];
            if (tables == null) tables = new List<SiteTable>();

            using (SqlConnection connection = new SqlConnection(db.ConnectionString))
            {
                connection.Open();
                DataTable dt = connection.GetSchema("Tables", new String[] { null, null, null, "BASE TABLE" });
                dt.DefaultView.Sort = "table_name ASC";
                dt = dt.DefaultView.ToTable();
                List<String> list = new List<String>();
                foreach (DataRow dr in dt.Rows)
                {
                    String tabname = Convert.ToString(dr["TABLE_NAME"]).ToLower();
                    if (tabname == "dtproperties") continue;
                    list.Add(tabname);
                    SiteTable table = tables.Find(a => { return a.TableName == tabname; });
                    if (table == null)
                    {
                        table = new SiteTable
                        {
                            ID = Guid.NewGuid(),
                            TableName = tabname,
                            Description = "",
                            Columns = new List<Field>()
                        };
                        tables.Add(table);
                    }
                    RefreshField(connection, table);
                }

                #region 检查是否存在

                List<SiteTable> notExist = new List<SiteTable>();
                foreach (SiteTable item in tables)
                {
                    if (!list.Contains(item.TableName))
                    {
                        notExist.Add(item);
                    }
                }
                foreach (SiteTable item in notExist)
                {
                    tables.Remove(item);
                }

                #endregion

                SaveTables(tables);
            }
        }
        /// <summary>
        /// 刷新字段
        /// </summary>
        public static void RefreshField(SqlConnection connection, SiteTable table)
        {
            #region 获取字段信息

            DataTable ds = connection.GetSchema("Columns", new String[] { null, null, table.TableName, null });
            ds.DefaultView.Sort = "ordinal_position ASC";
            ds = ds.DefaultView.ToTable();
            List<String> list = new List<String>();

            foreach (DataRow r in ds.Rows)
            {
                String name = r["column_name"].ToString().ToLower();
                list.Add(name);
                Field field = table.Columns.Find(a => { return a.FieldName == name; });
                if (field == null)
                {
                    field = new Field
                    {
                        ID = Guid.NewGuid(),
                        FieldName = name,
                        //Regex = "",
                        //Description = "",
                        isVirtual = false,
                        Control = ControlType.Text,
                        Position = Convert.ToInt32(r["ordinal_position"])
                    };
                    table.Columns.Add(field);
                }
                Object length = r["CHARACTER_MAXIMUM_LENGTH"];

                field.Length = length == DBNull.Value ? 0 : Convert.ToInt32(length);
                field.DefaultValue = Convert.ToString(r["column_default"]).Trim(new Char[] { '(', ')', '\'', '\"' });
                field.DataType = (Int32)Enum.Parse(db.dbType, r["data_type"].ToString(), true);
                field.isNullable = Convert.ToString(r["is_nullable"]).Equals("yes", StringComparison.OrdinalIgnoreCase);
            }
            #endregion

            #region 检查是否存在

            List<Field> notExist = new List<Field>();
            foreach (Field item in table.Columns)
            {
                if (!list.Contains(item.FieldName) && !item.isVirtual)
                {
                    notExist.Add(item);
                }
            }
            foreach (Field item in notExist)
            {
                table.Columns.Remove(item);
            }

            #endregion
        }
        /// <summary>
        /// 刷新单张表字段
        /// </summary>
        /// <param name="table"></param>
        public static void RefreshField(SiteTable table)
        {
            using (SqlConnection connection = new SqlConnection(db.ConnectionString))
            {
                connection.Open();
                RefreshField(connection, table);
                SaveTables(Tables);
            }
        }
        /// <summary>
        /// 保存配置
        /// </summary>
        public static void SaveTables(List<SiteTable> tables)
        {
            //List<SiteTable> tables = (List<SiteTable>)Cache["SiteTables"];
            if (tables == null) tables = new List<SiteTable>();

            tables.Sort(delegate(SiteTable t1, SiteTable t2)
            {
                return t1.TableName.CompareTo(t2.TableName);
            });
            tables.ForEach(a =>
            {
                a.Columns.Sort(delegate(Field t1, Field t2)
                {
                    return t1.FieldName.CompareTo(t2.FieldName);
                });
            });
            ConvertHelper.ObjectToFile<List<SiteTable>>(tables, xml);
        }
    }
}