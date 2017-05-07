using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace CommonUtils
{
    public static class DataCommon<T> where T : class, new()
    {
        //public static DataTable SelCommon(String sql)
        //{
        //    Database db = DatabaseFactory.CreateDatabase();
        //    DbCommand command = db.GetSqlStringCommand(sql);
        //    return db.ExecuteDataSet(command).Tables[0];
        //}
        /// <summary>
        /// 嵌套成员赋值 不支持循环嵌套。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Object SetValue(Type type, IDataRecord data)
        {
            #region
            //Object element = Activator.CreateInstance(type);
            //for (int i = 0; i < data.FieldCount; i++)
            //{
            //    foreach (PropertyInfo item in type.GetProperties())
            //    {
            //        if (item.Name.Equals(data.GetName(i), StringComparison.OrdinalIgnoreCase))
            //        {
            //            Object value = data.GetValue(i);
            //            if (value != DBNull.Value) item.SetValue(element, value, null);
            //            break;
            //        }
            //        else if (item.PropertyType.Namespace.Equals("MvcSite.Models"))
            //        {
            //            Object instance = SetValue(item.PropertyType, data);
            //            item.SetValue(element, instance, null);
            //        }
            //    }
            //}
            //return element;
            #endregion
            Object element = Activator.CreateInstance(type);
            for (int i = 0; i < data.FieldCount; i++)
            {
                String daName = data.GetName(i);
                foreach (PropertyInfo att in type.GetProperties())
                {
                    Object value = DBNull.Value;
                    String FieldName = att.Name;
                    String NestedName = String.Format("{0}.{1}", type.Name, FieldName);
                    if (daName.Equals(NestedName, StringComparison.OrdinalIgnoreCase)) FieldName = NestedName;

                    if (daName.Equals(FieldName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = data[i];
                        if (value == DBNull.Value) continue;
                        att.SetValue(element, value, null);
                    }
                    else if (att.PropertyType.Namespace.Equals("MvcSite.Models"))
                    {
                        Object instance = SetValue(att.PropertyType, data);
                        att.SetValue(element, instance, null);
                    }
                }
            }
            return element;
        }
        public static List<T> SelCommon(String sql)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            List<T> list = new List<T>();
            IDataReader reader = db.ExecuteReader(command);
            try
            {
                while (reader.Read())
                {
                    //T local = Activator.CreateInstance<T>();
                    //for (int i = 0; i < reader.FieldCount; i++)
                    //{
                    //    PropertyInfo property = typeof(T).GetProperty(reader.GetName(i));
                    //    if (property != null && reader[i] != DBNull.Value)
                    //    {
                    //        property.SetValue(local, reader.GetValue(i), null);
                    //    }
                    //}
                    T local = (T)SetValue(typeof(T), reader);
                    list.Add(local);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                reader.Dispose();
            }
            return list;
        }
    }
}
