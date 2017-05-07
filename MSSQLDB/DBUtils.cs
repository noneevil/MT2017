using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SQLite;
using System.Reflection;
using FastReflectionLib;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace MSSQLDB
{
    public abstract class DBUtils
    {
        /// <summary>
        /// 反射设置数据
        /// </summary>
        /// <typeparam name="T">支持 <T> 或 List<T></typeparam>
        /// <param name="reader">IDataReader</param>
        /// <returns></returns>
        public void SetValue<T>(IDataReader reader, ref T result)
        {
            Type baseType = result.GetType();
            Type makeType = baseType.IsGenericType ? baseType.GetGenericArguments()[0] : baseType;
            while (reader.Read())
            {
                Object instance = Activator.CreateInstance(makeType);
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Object value = reader.GetValue(i);
                    if (value == null || value == DBNull.Value) continue;
                    PropertyInfo att = makeType.GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (att == null || !att.CanWrite) continue;

                    if (value.GetType() == att.PropertyType)//数据类型相同
                    {
                        /*--att.SetValue(instance, value, null);*/
                        att.FastSetValue(instance, value);
                    }
                    else if (att.PropertyType.IsEnum)//枚举类型
                    {
                        att.FastSetValue(instance, Enum.ToObject(att.PropertyType, value));
                    }
                    else if (att.PropertyType.IsGenericType)
                    {
                        Type generictype = att.PropertyType.GetGenericTypeDefinition();//泛型Nullable<> 可空的值类型 如int?、long?
                        if (generictype == typeof(Nullable<>))
                        {
                            /*--att.SetValue(instance, Convert.ChangeType(value, Nullable.GetUnderlyingType(att.PropertyType)), null);*/
                            att.FastSetValue(instance, Convert.ChangeType(value, Nullable.GetUnderlyingType(att.PropertyType)));
                        }
                    }
                    else
                    {
                        /*att.SetValue(instance, Convert.ChangeType(value, att.PropertyType), null); //数据类型不同时尝试转换*/
                        att.FastSetValue(instance, Convert.ChangeType(value, att.PropertyType));
                    }
                }
                if (baseType.IsGenericType)
                {
                    /*--baseType.GetMethod("Add").Invoke(result, new[] { instance });*/
                    baseType.GetMethod("Add").FastInvoke(result, new[] { instance });
                }
                else
                {
                    result = (T)instance;
                    break;
                }
            }
        }
        /// <summary>
        /// 转成JSON字符
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <param name="json">JSON</param>
        public void ConvertToJson(IDataReader reader, ref String json)
        {
            List<Dictionary<String, Object>> dict = new List<Dictionary<String, Object>>();
            while (reader.Read())
            {
                Dictionary<String, Object> data = new Dictionary<String, Object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    data.Add(reader.GetName(i), reader.GetValue(i));
                }
                dict.Add(data);
            }
            if (dict.Count == 1)
                json = JsonConvert.SerializeObject(dict[0]);
            else
                json = JsonConvert.SerializeObject(dict);
        }
        /// <summary>
        /// 获取数据表架构
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="sql"></param>
        public Dictionary<String, Type> GetTableSchema(String tablename, String sql)
        {
            //tablename = tablename.ToLower();
            if (db.TableSchema.Contains(tablename))
            {
                return (Dictionary<String, Type>)db.TableSchema[tablename];
            }
            else
            {
                DataTable table = db.ExecuteDataSet(sql).Tables[0];
                Dictionary<String, Type> dict = new Dictionary<String, Type>();
                foreach (DataColumn item in table.Columns)
                {
                    dict.Add(item.ColumnName.ToLower(), null);
                    //dict.Add(item.ColumnName.ToLower(), item.DataType);
                }
                lock (db.TableSchema)
                {
                    if (!db.TableSchema.Contains(tablename))
                        db.TableSchema.Add(tablename, dict);
                }
                return dict;
            }
        }
        public Type dbType()
        {
            switch (db.CurrDbType)
            {
                case DatabaseType.OLEDB:
                case DatabaseType.ACCESS:
                    return typeof(OleDbType);
                case DatabaseType.SqLite:
                    return typeof(TypeAffinity);
                case DatabaseType.ORACLE:
                    return typeof(OracleType);
                case DatabaseType.SqlServer:
                    return typeof(SqlDbType);
                case DatabaseType.SqlServerCe:
                    return typeof(SqlDbType);
                case DatabaseType.MYSQL:
                    return typeof(MySqlDbType);
                default:
                    return typeof(DbType);
            }
        }
    }
}
