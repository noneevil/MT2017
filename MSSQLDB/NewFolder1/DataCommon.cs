using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using System.Data.SqlClient;
using System.Configuration;

namespace E_Shop.EntLibData
{
    public static class DataCommon<T> where T : class, new()
    {
        /// <summary>
        /// 根据查询语句查询数据库
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>数据实体</returns>
        public static DataTable ExecuteDataTable(string sql)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand comm = db.GetSqlStringCommand(sql);
            return db.ExecuteDataSet(comm).Tables[0];
        }
        public static DataSet ExecuteDataSet(string sql)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand comm = db.GetSqlStringCommand(sql);
            return db.ExecuteDataSet(comm);
        }
        /// <summary>
        /// 针对嵌套成员作赋值处理 注意:不支持循环嵌套。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Object SetValue(Type type, IDataRecord data)
        {
            Object element = Activator.CreateInstance(type);
            for (int i = 0; i < data.FieldCount; i++)
            {
                foreach (PropertyInfo item in type.GetProperties())
                {
                    if (item.Name.Equals(data.GetName(i), StringComparison.OrdinalIgnoreCase))
                    {
                        if (data.GetValue(i) != DBNull.Value)
                            item.SetValue(element, data.GetValue(i), null);
                        break;
                    }
                    else if (item.PropertyType.Namespace.Equals("E_Shop.Model"))
                    {
                        Object instance = SetValue(item.PropertyType, data);
                        item.SetValue(element, instance, null);
                    }
                }
            }
            return element;
        }
        /// <summary>
        /// 根据查询语句查询数据库
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>数据实体</returns>
        public static List<T> SelCommon(string sql)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand comm = db.GetSqlStringCommand(sql);
            List<T> ts = new List<T>();
            using (IDataReader reader = db.ExecuteReader(comm))
            {
                while (reader.Read())
                {
                    T local = (T)SetValue(typeof(T), reader);
                    ts.Add(local);
                    #region 原有方法
                    //T instance = Activator.CreateInstance<T>();
                    //for (int i = 0; i < reader.FieldCount; i++)
                    //{
                    //    PropertyInfo pinfo = typeof(T).GetProperty(reader.GetName(i));
                    //    if (pinfo != null)
                    //    {
                    //        if (reader[i] != DBNull.Value)
                    //        {
                    //            pinfo.SetValue(instance, reader.GetValue(i), null);
                    //        }
                    //    }
                    //}
                    //ts.Add(instance);
                    #endregion
                }
            }
            return ts;
        }
        public static List<T> SelCommon2(string sql, List<SqlParameter> parameters, CommandType commandType)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            command.CommandType = commandType;
            command.Parameters.Clear();
            if (parameters != null) command.Parameters.AddRange(parameters.ToArray());

            List<T> ts = new List<T>();
            using (IDataReader reader = db.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    T local = (T)SetValue(typeof(T), reader);
                    ts.Add(local);
                }
            }
            return ts;
        }
        public static T SelSignCommon2(string sql, List<SqlParameter> parameters, CommandType commandType)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand command = db.GetSqlStringCommand(sql);
            command.CommandType = commandType;
            command.Parameters.Clear();
            if (parameters != null) command.Parameters.AddRange(parameters.ToArray());

            IDataReader reader = db.ExecuteReader(command);
            T local = null;
            if (reader.Read())
            {
                local = (T)SetValue(typeof(T), reader);
            }
            return local;
        }
        public static List<T> SelCommon(string connStr, string sql)
        {

            Database db = DatabaseFactory.CreateDatabase(connStr);
            DbCommand comm = db.GetSqlStringCommand(sql);
            List<T> ts = new List<T>();
            using (IDataReader reader = db.ExecuteReader(comm))
            {
                while (reader.Read())
                {
                    T instance = Activator.CreateInstance<T>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        PropertyInfo pinfo = typeof(T).GetProperty(reader.GetName(i));
                        if (pinfo != null)
                        {
                            if (reader[i] != DBNull.Value)
                            {
                                pinfo.SetValue(instance, reader.GetValue(i), null);
                            }
                        }
                    }
                    ts.Add(instance);
                }
            }
            return ts;
        }
        /// <summary>
        /// 参数法查询
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">字典类型参数</param>
        /// <param name="commandType">查询方法类型</param>
        /// <returns></returns>
        public static List<T> SelCommon(string sql, IDictionary<string, object> parameters, CommandType commandType)
        {
            List<T> ts = new List<T>();
            using (SqlConnection sqlConnection = DatabaseFactory.CreateDatabase().CreateConnection() as SqlConnection)
            {
                SqlCommand command = new SqlCommand(sql, sqlConnection);
                command.CommandType = commandType;
                command.Parameters.Clear();
                foreach (var parameter in parameters)
                {
                    string parameterName = parameter.Key;
                    if (!parameter.Key.StartsWith("@"))
                    {
                        parameterName = "@" + parameterName;
                    }
                    command.Parameters.Add(new SqlParameter(parameterName, parameter.Value));
                }
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }

                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        T instance = Activator.CreateInstance<T>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            PropertyInfo pinfo = typeof(T).GetProperty(reader.GetName(i));
                            if (pinfo != null)
                            {
                                if (reader[i] != DBNull.Value)
                                {
                                    pinfo.SetValue(instance, reader.GetValue(i), null);
                                }
                            }
                        }
                        ts.Add(instance);
                    }
                }
                catch
                {
                    throw new Exception();
                }
            }
            return ts;
        }
        /// <summary>
        /// 参数法查询
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">泛型参数</param>
        /// <param name="commandType">查询方法类型</param>
        /// <returns></returns>
        public static List<T> SelCommon(string sql, List<SqlParameter> parameters, CommandType commandType)
        {
            List<T> ts = new List<T>();
            using (SqlConnection sqlConnection = DatabaseFactory.CreateDatabase().CreateConnection() as SqlConnection)
            {
                SqlCommand command = new SqlCommand(sql, sqlConnection);
                command.CommandType = commandType;
                command.Parameters.Clear();

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }

                //try
                //{                
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    T instance = Activator.CreateInstance<T>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        PropertyInfo pinfo = typeof(T).GetProperty(reader.GetName(i));
                        if (pinfo != null)
                        {
                            if (reader[i] != DBNull.Value)
                            {
                                pinfo.SetValue(instance, reader.GetValue(i), null);
                            }
                        }
                    }
                    ts.Add(instance);
                }
                //}
                //catch
                //{
                //    throw new Exception();
                //}
            }
            return ts;
        }
        /// <summary>
        /// 根据查询语句查询数据库（只返回结果集的第一条记录）
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>数据实体</returns>
        public static T SelSignCommon(string sql)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand comm = db.GetSqlStringCommand(sql);
            T t = Activator.CreateInstance<T>();
            using (IDataReader reader = db.ExecuteReader(comm))
            {
                if (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        PropertyInfo pinfo = typeof(T).GetProperty(reader.GetName(i));
                        if (pinfo != null)
                        {
                            if (reader[i] != DBNull.Value)
                            {
                                pinfo.SetValue(t, reader.GetValue(i), null);
                            }
                        }
                    }
                }
            }
            return t;
        }
        /// <summary>
        /// 参数法查询单条数据（只返回结果集的第一条记录）
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">字典类型参数</param>
        /// <param name="commandType">查询方法类型</param>
        /// <returns></returns>
        public static T SelSignCommon(string sql, IDictionary<string, object> parameters, CommandType commandType)
        {
            T t = Activator.CreateInstance<T>();
            using (SqlConnection sqlConnection = DatabaseFactory.CreateDatabase().CreateConnection() as SqlConnection)
            {
                SqlCommand command = new SqlCommand(sql, sqlConnection);
                command.CommandType = commandType;
                command.Parameters.Clear();
                foreach (var parameter in parameters)
                {
                    string parameterName = parameter.Key;
                    if (!parameter.Key.StartsWith("@"))
                    {
                        parameterName = "@" + parameterName;
                    }
                    command.Parameters.Add(new SqlParameter(parameterName, parameter.Value));
                }
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }

                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            PropertyInfo pinfo = typeof(T).GetProperty(reader.GetName(i));
                            if (pinfo != null)
                            {
                                if (reader[i] != DBNull.Value)
                                {
                                    pinfo.SetValue(t, reader.GetValue(i), null);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    throw new Exception();
                }
            }
            return t;
        }

        public static T SelSignCommon(string sql, List<SqlParameter> parameters, CommandType commandType)
        {
            T t = Activator.CreateInstance<T>();
            using (SqlConnection sqlConnection = DatabaseFactory.CreateDatabase().CreateConnection() as SqlConnection)
            {
                SqlCommand command = new SqlCommand(sql, sqlConnection);
                command.CommandType = commandType;
                command.Parameters.Clear();

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }

                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            PropertyInfo pinfo = typeof(T).GetProperty(reader.GetName(i));
                            if (pinfo != null)
                            {
                                if (reader[i] != DBNull.Value)
                                {
                                    pinfo.SetValue(t, reader.GetValue(i), null);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    throw new Exception();
                }
            }
            return t;
        }

        /// <summary>
        /// 新增数据到数据库
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增数据条数</returns>
        public static object InsertCommand(T entity)
        {
            //DALBase.InvalidateCache<T>();
            Database db = DatabaseFactory.CreateDatabase();
            string tableName = null;
            TableAttribute[] attrs = (TableAttribute[])typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
            if (attrs != null && attrs.Length > 0)
                tableName = attrs[0].TableName;

            if (tableName != null)
            {
                string sqlHead = "INSERT INTO " + tableName;
                string sqlFields = "(";
                string sqlValues = "(";
                PropertyInfo[] pinfos = typeof(T).GetProperties();
                List<string> parms = new List<string>();
                int i = 0;
                foreach (PropertyInfo pinfo in pinfos)
                {
                    if (GetEmptyValue(pinfo, pinfo.GetValue(entity, null)) != null)
                    {
                        FieldAttribute[] fieldattrs = (FieldAttribute[])pinfo.GetCustomAttributes(typeof(FieldAttribute), false);
                        if (fieldattrs != null && fieldattrs.Length > 0)
                        {
                            if (!fieldattrs[0].IsSeed && fieldattrs[0].isAllowEdit && !fieldattrs[0].IsKey)
                            {
                                if (i > 0)
                                {
                                    sqlFields += " ,";
                                    sqlValues += " ,";
                                }
                                sqlFields += fieldattrs[0].FieldName;
                                sqlValues += "@" + fieldattrs[0].FieldName;
                                parms.Add(fieldattrs[0].FieldName);
                                i++;
                            }
                        }
                    }
                }
                sqlFields += ") VALUES ";
                sqlValues += ");SELECT @@Identity";
                string sql = sqlHead + sqlFields + sqlValues;
                DbCommand command = db.GetSqlStringCommand(sql);

                foreach (string s in parms)
                {
                    PropertyInfo pinfo = typeof(T).GetProperty(s);
                    db.AddInParameter(command, s, GetDBType(pinfo), pinfo.GetValue(entity, null));
                }
                return db.ExecuteScalar(command);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 新增数据到数据库(允许可空类型)
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增数据条数</returns>
        public static object InsertCommandAllowNull(T entity)
        {
            //DALBase.InvalidateCache<T>();
            Database db = DatabaseFactory.CreateDatabase();
            string tableName = null;
            TableAttribute[] attrs = (TableAttribute[])typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
            if (attrs != null && attrs.Length > 0)
                tableName = attrs[0].TableName;

            if (tableName != null)
            {
                string sqlHead = "INSERT INTO " + tableName;
                string sqlFields = "(";
                string sqlValues = "(";
                PropertyInfo[] pinfos = typeof(T).GetProperties();
                List<string> parms = new List<string>();
                int i = 0;
                foreach (PropertyInfo pinfo in pinfos)
                {
                    if (GetEmptyValue(pinfo, pinfo.GetValue(entity, null)) != null)
                    {
                        FieldAttribute[] fieldattrs = (FieldAttribute[])pinfo.GetCustomAttributes(typeof(FieldAttribute), false);
                        if (fieldattrs != null && fieldattrs.Length > 0)
                        {
                            if (!fieldattrs[0].IsSeed && fieldattrs[0].isAllowEdit)
                            {
                                if (i > 0)
                                {
                                    sqlFields += " ,";
                                    sqlValues += " ,";
                                }
                                sqlFields += fieldattrs[0].FieldName;
                                sqlValues += "@" + fieldattrs[0].FieldName;
                                parms.Add(fieldattrs[0].FieldName);
                                i++;
                            }
                        }
                    }
                }
                sqlFields += ") VALUES ";
                sqlValues += ");SELECT @@Identity";
                string sql = sqlHead + sqlFields + sqlValues;
                DbCommand command = db.GetSqlStringCommand(sql);

                foreach (string s in parms)
                {
                    PropertyInfo pinfo = typeof(T).GetProperty(s);
                    object tmpval = pinfo.GetValue(entity, null) == null ? DBNull.Value : pinfo.GetValue(entity, null);
                    db.AddInParameter(command, s, GetDBType(pinfo), tmpval);
                }
                return db.ExecuteScalar(command);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 新增数据到数据库(允许可空类型)(无自增列)
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增数据条数</returns>
        public static object InsertCommandAllowNull1(T entity)
        {
            //DALBase.InvalidateCache<T>();
            Database db = DatabaseFactory.CreateDatabase();
            string tableName = null;
            TableAttribute[] attrs = (TableAttribute[])typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
            if (attrs != null && attrs.Length > 0)
                tableName = attrs[0].TableName;

            if (tableName != null)
            {
                string sqlHead = "INSERT INTO " + tableName;
                string sqlFields = "(";
                string sqlValues = "(";
                PropertyInfo[] pinfos = typeof(T).GetProperties();
                List<string> parms = new List<string>();
                int i = 0;
                foreach (PropertyInfo pinfo in pinfos)
                {
                    //if (GetEmptyValue(pinfo, pinfo.GetValue(entity, null)) != null)
                    //{
                    FieldAttribute[] fieldattrs = (FieldAttribute[])pinfo.GetCustomAttributes(typeof(FieldAttribute), false);
                    if (fieldattrs != null && fieldattrs.Length > 0)
                    {
                        if (!fieldattrs[0].IsSeed && fieldattrs[0].isAllowEdit)
                        {
                            if (i > 0)
                            {
                                sqlFields += " ,";
                                sqlValues += " ,";
                            }
                            sqlFields += fieldattrs[0].FieldName;
                            sqlValues += "@" + fieldattrs[0].FieldName;
                            parms.Add(fieldattrs[0].FieldName);
                            i++;
                        }
                    }
                    //}
                }
                sqlFields += ") VALUES ";
                sqlValues += ")";
                string sql = sqlHead + sqlFields + sqlValues;
                DbCommand command = db.GetSqlStringCommand(sql);

                foreach (string s in parms)
                {
                    PropertyInfo pinfo = typeof(T).GetProperty(s);
                    object tmpval = pinfo.GetValue(entity, null) == null ? DBNull.Value : pinfo.GetValue(entity, null);
                    db.AddInParameter(command, s, GetDBType(pinfo), tmpval);
                }
                return db.ExecuteScalar(command);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 新增数据到数据库(无自增列)
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增数据条数</returns>
        public static int InsertCommand1(T entity)
        {
            //DALBase.InvalidateCache<T>();
            Database db = DatabaseFactory.CreateDatabase();
            string tableName = null;
            TableAttribute[] attrs = (TableAttribute[])typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
            if (attrs != null && attrs.Length > 0)
                tableName = attrs[0].TableName;

            if (tableName != null)
            {
                string sqlHead = "INSERT INTO " + tableName;
                string sqlFields = "(";
                string sqlValues = "(";
                PropertyInfo[] pinfos = typeof(T).GetProperties();
                List<string> parms = new List<string>();
                int i = 0;
                foreach (PropertyInfo pinfo in pinfos)
                {
                    if (GetEmptyValue(pinfo, pinfo.GetValue(entity, null)) != null)
                    {
                        FieldAttribute[] fieldattrs = (FieldAttribute[])pinfo.GetCustomAttributes(typeof(FieldAttribute), false);
                        if (fieldattrs != null && fieldattrs.Length > 0)
                        {
                            if (!fieldattrs[0].IsSeed && fieldattrs[0].isAllowEdit)
                            {
                                if (i > 0)
                                {
                                    sqlFields += " ,";
                                    sqlValues += " ,";
                                }
                                sqlFields += fieldattrs[0].FieldName;
                                sqlValues += "@" + fieldattrs[0].FieldName;
                                parms.Add(fieldattrs[0].FieldName);
                                i++;
                            }
                        }
                    }
                }
                sqlFields += ") VALUES ";
                sqlValues += ")";
                string sql = sqlHead + sqlFields + sqlValues;
                DbCommand command = db.GetSqlStringCommand(sql);

                foreach (string s in parms)
                {
                    PropertyInfo pinfo = typeof(T).GetProperty(s);
                    db.AddInParameter(command, s, GetDBType(pinfo), pinfo.GetValue(entity, null));
                }
                return db.ExecuteNonQuery(command);
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 根据索引键编辑数据(允许可空类型)
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>编辑数据条数</returns>
        public static int UpdateCommandAllowNull(T entity)
        {
            //DALBase.InvalidateCache<T>();
            Database db = DatabaseFactory.CreateDatabase();
            string tableName = null;
            TableAttribute[] attrs = (TableAttribute[])typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
            if (attrs != null && attrs.Length > 0)
                tableName = attrs[0].TableName;

            if (tableName != null)
            {
                string sqlSting = " UPDATE " + tableName + " SET ";
                string whereString = " ";
                PropertyInfo[] pinfos = typeof(T).GetProperties();
                List<string> parms = new List<string>();
                int i = 0;
                foreach (PropertyInfo pinfo in pinfos)
                {
                    FieldAttribute[] fieldattrs = (FieldAttribute[])pinfo.GetCustomAttributes(typeof(FieldAttribute), false);
                    if (fieldattrs != null && fieldattrs.Length > 0)
                    {
                        if (!fieldattrs[0].IsKey && fieldattrs[0].isAllowEdit)
                        {
                            if (i > 0)
                                sqlSting += " ,";
                            sqlSting += fieldattrs[0].FieldName + " = " + "@" + fieldattrs[0].FieldName;
                            parms.Add(fieldattrs[0].FieldName);
                            i++;
                        }
                        else if (fieldattrs[0].IsKey)
                        {
                            whereString += " WHERE " + fieldattrs[0].FieldName + " = " + "@" + fieldattrs[0].FieldName;
                            parms.Add(fieldattrs[0].FieldName);
                        }
                    }
                }
                string sql = sqlSting + whereString;
                DbCommand command = db.GetSqlStringCommand(sql);

                foreach (string s in parms)
                {
                    PropertyInfo pinfo = typeof(T).GetProperty(s);
                    object tmpval = pinfo.GetValue(entity, null) == null ? DBNull.Value : pinfo.GetValue(entity, null);
                    db.AddInParameter(command, s, GetDBType(pinfo), tmpval);
                }
                return db.ExecuteNonQuery(command);
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 跟新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="upFields">需更新数据字段</param>
        /// <returns>跟新数据条数</returns>
        public static int UpdateCommandAllowNull(T entity, string[] upFields)
        {
            //DALBase.InvalidateCache<T>();
            Database db = DatabaseFactory.CreateDatabase();
            string tableName = null;
            TableAttribute[] attrs = (TableAttribute[])typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
            if (attrs != null && attrs.Length > 0)
                tableName = attrs[0].TableName;

            if (tableName != null)
            {
                string sqlSting = " UPDATE " + tableName + " SET ";
                string whereString = " ";
                int i = 0;

                string whereParm = null;
                foreach (PropertyInfo pp in typeof(T).GetProperties())
                {
                    FieldAttribute[] fieldattrs = (FieldAttribute[])pp.GetCustomAttributes(typeof(FieldAttribute), false);
                    if (fieldattrs != null && fieldattrs.Length > 0)
                    {
                        if (!fieldattrs[0].IsKey && fieldattrs[0].isAllowEdit)
                        {
                            int fieldNum = (from a in upFields where a == (fieldattrs[0].FieldName) select a).Count();
                            if (fieldNum > 0)
                            {
                                PropertyInfo pinfo = typeof(T).GetProperty(fieldattrs[0].FieldName);

                                if (i > 0)
                                    sqlSting += " , ";
                                sqlSting += fieldattrs[0].FieldName + " = " + "@" + fieldattrs[0].FieldName;
                                i++;
                            }
                        }
                        else if (fieldattrs[0].IsKey)
                        {
                            whereString += " WHERE " + fieldattrs[0].FieldName + " = " + "@" + fieldattrs[0].FieldName;
                            whereParm = fieldattrs[0].FieldName;
                        }
                    }
                }

                string sql = sqlSting + whereString;
                DbCommand command = db.GetSqlStringCommand(sql);
                foreach (string s in upFields)
                {
                    PropertyInfo pinfo = typeof(T).GetProperty(s);
                    object tmpval = pinfo.GetValue(entity, null) == null ? DBNull.Value : pinfo.GetValue(entity, null);
                    db.AddInParameter(command, s, GetDBType(pinfo), tmpval);
                }
                PropertyInfo wherepinfo = typeof(T).GetProperty(whereParm);
                db.AddInParameter(command, whereParm, GetDBType(wherepinfo), wherepinfo.GetValue(entity, null));
                return db.ExecuteNonQuery(command);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 根据索引键编辑数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>编辑数据条数</returns>
        public static int UpdateCommand(T entity)
        {
            //DALBase.InvalidateCache<T>();
            Database db = DatabaseFactory.CreateDatabase();
            string tableName = null;
            TableAttribute[] attrs = (TableAttribute[])typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
            if (attrs != null && attrs.Length > 0)
                tableName = attrs[0].TableName;

            if (tableName != null)
            {
                string sqlSting = " UPDATE " + tableName + " SET ";
                string whereString = " ";
                PropertyInfo[] pinfos = typeof(T).GetProperties();
                List<string> parms = new List<string>();
                int i = 0;
                foreach (PropertyInfo pinfo in pinfos)
                {
                    FieldAttribute[] fieldattrs = (FieldAttribute[])pinfo.GetCustomAttributes(typeof(FieldAttribute), false);
                    if (fieldattrs != null && fieldattrs.Length > 0)
                    {
                        if (!fieldattrs[0].IsKey && fieldattrs[0].isAllowEdit)
                        {
                            if (i > 0)
                                sqlSting += " ,";
                            sqlSting += fieldattrs[0].FieldName + " = " + "@" + fieldattrs[0].FieldName;
                            parms.Add(fieldattrs[0].FieldName);
                            i++;
                        }
                        else if (fieldattrs[0].IsKey)
                        {
                            whereString += " WHERE " + fieldattrs[0].FieldName + " = " + "@" + fieldattrs[0].FieldName;
                            parms.Add(fieldattrs[0].FieldName);
                        }
                    }
                }
                string sql = sqlSting + whereString;
                DbCommand command = db.GetSqlStringCommand(sql);

                foreach (string s in parms)
                {
                    PropertyInfo pinfo = typeof(T).GetProperty(s);
                    object o = GetEmptyValue(pinfo, pinfo.GetValue(entity, null));
                    db.AddInParameter(command, s, GetDBType(pinfo), (o == null) ? DBNull.Value : o);
                }
                return db.ExecuteNonQuery(command);
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 跟新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="upFields">需更新数据字段</param>
        /// <returns>跟新数据条数</returns>
        public static int UpdateCommand(T entity, string[] upFields)
        {
            //DALBase.InvalidateCache<T>();
            Database db = DatabaseFactory.CreateDatabase();
            string tableName = null;
            TableAttribute[] attrs = (TableAttribute[])typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
            if (attrs != null && attrs.Length > 0)
                tableName = attrs[0].TableName;

            if (tableName != null)
            {
                string sqlSting = " UPDATE " + tableName + " SET ";
                string whereString = " ";
                int i = 0;

                string whereParm = null;
                foreach (PropertyInfo pp in typeof(T).GetProperties())
                {
                    FieldAttribute[] fieldattrs = (FieldAttribute[])pp.GetCustomAttributes(typeof(FieldAttribute), false);
                    if (fieldattrs != null && fieldattrs.Length > 0)
                    {
                        if (!fieldattrs[0].IsKey && fieldattrs[0].isAllowEdit)
                        {
                            int fieldNum = (from a in upFields where a == (fieldattrs[0].FieldName) select a).Count();
                            if (fieldNum > 0)
                            {
                                PropertyInfo pinfo = typeof(T).GetProperty(fieldattrs[0].FieldName);

                                if (i > 0)
                                    sqlSting += " , ";
                                sqlSting += fieldattrs[0].FieldName + " = " + "@" + fieldattrs[0].FieldName;
                                i++;
                            }
                        }
                        else if (fieldattrs[0].IsKey)
                        {
                            whereString += " WHERE " + fieldattrs[0].FieldName + " = " + "@" + fieldattrs[0].FieldName;
                            whereParm = fieldattrs[0].FieldName;
                        }
                    }
                }

                string sql = sqlSting + whereString;
                DbCommand command = db.GetSqlStringCommand(sql);
                foreach (string s in upFields)
                {
                    PropertyInfo pinfo = typeof(T).GetProperty(s);
                    db.AddInParameter(command, s, GetDBType(pinfo), pinfo.GetValue(entity, null));
                }
                PropertyInfo wherepinfo = typeof(T).GetProperty(whereParm);
                db.AddInParameter(command, whereParm, GetDBType(wherepinfo), wherepinfo.GetValue(entity, null));
                return db.ExecuteNonQuery(command);
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 跟新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="upFields">需更新数据字段</param>
        /// <param name="whereFields">条件字段</param>
        /// <returns>更新数据条数</returns>
        public static int UpdateCommand(T entity, string[] upFields, string[] whereFields)
        {
            //DALBase.InvalidateCache<T>();
            Database db = DatabaseFactory.CreateDatabase();
            string tableName = null;
            TableAttribute[] attrs = (TableAttribute[])typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
            if (attrs != null && attrs.Length > 0)
                tableName = attrs[0].TableName;

            if (tableName != null)
            {
                string sqlSting = " UPDATE " + tableName + " SET ";
                string whereString = " WHERE 1=1 ";
                int i = 0;
                foreach (string s in upFields)
                {
                    PropertyInfo pinfo = typeof(T).GetProperty(s);
                    FieldAttribute[] fieldattrs = (FieldAttribute[])pinfo.GetCustomAttributes(typeof(FieldAttribute), false);
                    if (fieldattrs != null && fieldattrs.Length > 0)
                    {
                        if (i > 0) sqlSting += " , ";
                        sqlSting += s + " = " + "@" + s;
                        i++;
                    }
                }
                foreach (string s in whereFields)
                {
                    PropertyInfo pinfo = typeof(T).GetProperty(s);
                    FieldAttribute[] fieldattrs = (FieldAttribute[])pinfo.GetCustomAttributes(typeof(FieldAttribute), false);
                    if (fieldattrs != null && fieldattrs.Length > 0)
                    {
                        whereString += " AND " + s + " = " + "@" + s;
                    }
                }

                string sql = sqlSting + whereString;
                DbCommand command = db.GetSqlStringCommand(sql);
                foreach (string s in upFields)
                {
                    PropertyInfo pinfo = typeof(T).GetProperty(s);
                    FieldAttribute[] fieldattrs = (FieldAttribute[])pinfo.GetCustomAttributes(typeof(FieldAttribute), false);
                    db.AddInParameter(command, s, GetDBType(pinfo), pinfo.GetValue(entity, null));
                }
                foreach (string s in whereFields)
                {
                    PropertyInfo pinfo = typeof(T).GetProperty(s);
                    FieldAttribute[] fieldattrs = (FieldAttribute[])pinfo.GetCustomAttributes(typeof(FieldAttribute), false);
                    db.AddInParameter(command, s, GetDBType(pinfo), pinfo.GetValue(entity, null));
                }
                return db.ExecuteNonQuery(command);
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 按索引键删除实体
        /// </summary>
        /// <param name="entity">包含要删除键值的实体</param>
        /// <returns></returns>
        public static int DeleteCommand(T entity)
        {
            //DALBase.InvalidateCache<T>();
            Database db = DatabaseFactory.CreateDatabase();
            string tableName = null;
            TableAttribute[] attrs = (TableAttribute[])typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
            if (attrs != null && attrs.Length > 0)
                tableName = attrs[0].TableName;

            string sql = "";
            string whereParm = "";
            if (!string.IsNullOrEmpty(tableName))
            {
                sql = " DELETE FROM " + tableName + " WHERE ";
                PropertyInfo[] pinfos = typeof(T).GetProperties();
                foreach (PropertyInfo pinfo in pinfos)
                {
                    FieldAttribute[] fieldattrs = (FieldAttribute[])pinfo.GetCustomAttributes(typeof(FieldAttribute), false);
                    if (fieldattrs != null && fieldattrs.Length > 0)
                    {
                        if (fieldattrs[0].IsKey)
                        {
                            sql += fieldattrs[0].FieldName + " = " + "@" + fieldattrs[0].FieldName;
                            whereParm = fieldattrs[0].FieldName;
                        }
                    }
                }
                DbCommand command = db.GetSqlStringCommand(sql);

                PropertyInfo wherepinfo = typeof(T).GetProperty(whereParm);
                db.AddInParameter(command, whereParm, GetDBType(wherepinfo), wherepinfo.GetValue(entity, null));

                return db.ExecuteNonQuery(command);
            }
            else
                return 0;
        }
        /// <summary>
        /// 按指定条件字段删除数据
        /// </summary>
        /// <param name="entity">包含条件字段数据的实体</param>
        /// <param name="delFields">条件字段</param>
        /// <returns></returns>
        public static int DeleteCommand(T entity, string[] delFields)
        {
            //DALBase.InvalidateCache<T>();
            Database db = DatabaseFactory.CreateDatabase();
            string tableName = null;
            TableAttribute[] attrs = (TableAttribute[])typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
            if (attrs != null && attrs.Length > 0)
                tableName = attrs[0].TableName;

            string sql = "";
            if (!string.IsNullOrEmpty(tableName))
            {
                sql = " DELETE FROM " + tableName;
                PropertyInfo[] pinfos = typeof(T).GetProperties();
                int i = 0;
                foreach (string s in delFields)
                {
                    PropertyInfo pinfo = typeof(T).GetProperty(s);
                    FieldAttribute[] fieldattrs = (FieldAttribute[])pinfo.GetCustomAttributes(typeof(FieldAttribute), false);
                    if (fieldattrs != null && fieldattrs.Length > 0)
                    {
                        sql += (i == 0) ? " WHERE " : " AND ";
                        sql += fieldattrs[0].FieldName + " = " + "@" + fieldattrs[0].FieldName;
                    }
                }
                DbCommand command = db.GetSqlStringCommand(sql);
                foreach (string s in delFields)
                {
                    PropertyInfo wherepinfo = typeof(T).GetProperty(s);
                    db.AddInParameter(command, s, GetDBType(wherepinfo), wherepinfo.GetValue(entity, null));
                }

                return db.ExecuteNonQuery(command);
            }
            else
                return 0;
        }
        /// <summary>
        /// 获取DBType
        /// </summary>
        /// <param name="pinfo"></param>
        /// <returns></returns>
        private static DbType GetDBType(PropertyInfo pinfo)
        {
            DbType dbtype = DbType.String;
            Type t = GetUnderlyingType(pinfo.PropertyType);
            switch (t.Name)
            {
                case "Int32":
                    dbtype = DbType.Int32;
                    break;
                case "String":
                    dbtype = DbType.String;
                    break;
                case "DateTime":
                    dbtype = DbType.DateTime;
                    break;
                case "Guid":
                    dbtype = DbType.Guid;
                    break;
                case "Decimal":
                    dbtype = DbType.Decimal;
                    break;
                case "Double":
                    dbtype = DbType.Double;
                    break;
                case "Single":
                    dbtype = DbType.Decimal;
                    break;
            }
            return dbtype;
        }
        private static Type GetUnderlyingType(Type t)
        {
            Type returnType = t;
            if (returnType.IsGenericType &&
                    returnType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(returnType);
                returnType = nullableConverter.UnderlyingType;
            }

            return returnType;
        }

        private static object GetEmptyValue(PropertyInfo pinfo, object value)
        {
            object o = null;
            switch (pinfo.PropertyType.Name)
            {
                case "DateTime":
                    o = (DateTime.Parse(value.ToString()) == DateTime.MinValue) ? null : value;
                    break;
                case "Guid":
                    o = (value.ToString() == "00000000-0000-0000-0000-000000000000") ? null : value;
                    break;
                case "String":
                    o = (value == null) ? null : (string.IsNullOrEmpty(value.ToString())) ? null : value;
                    break;
                default:
                    o = value;
                    break;
            }
            return o;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">缓存依赖类型</typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int DeleteEntity(string sql)
        {
            //DALBase.InvalidateCache<T>();

            Database db = DatabaseFactory.CreateDatabase();
            return db.ExecuteNonQuery(CommandType.Text, sql);
        }



        public static int AutoUpdateListEntity(IList<T> list, string upField, string[] whereFields, string prefixField, bool isPlus, string only)
        {

            //DALBase.InvalidateCache<T>();
            Database db = DatabaseFactory.CreateDatabase();
            string tableName = null;
            TableAttribute[] attrs = (TableAttribute[])typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
            if (attrs != null && attrs.Length > 0)
                tableName = attrs[0].TableName;

            if (tableName != null)
            {
                StringBuilder sb = new StringBuilder();
                PropertyInfo info = typeof(T).GetProperty(upField);

                foreach (var item in list)
                {
                    sb.Append(" UPDATE " + tableName + " SET ");
                    sb.Append(upField);
                    sb.Append(" = ");
                    sb.Append(upField);
                    sb.Append(isPlus ? " + " : " - ");
                    sb.Append(info.GetValue(item, null));
                    sb.Append(" where 1 = 1 ");
                    foreach (var where in whereFields)
                    {
                        PropertyInfo whereInfo = typeof(T).GetProperty(where);
                        sb.Append(" and ");
                        if (prefixField.Equals(where))
                        {
                            sb.Append(" CONVERT(varchar(12) ,  " + where + ", 112 ) ");
                            sb.Append(" = @" + where);
                        }
                        else
                        {
                            sb.Append(where);
                            string val = whereInfo.GetValue(item, null).ToString();
                            if (val.StartsWith("P") || val.StartsWith("R"))
                            {
                                sb.Append(" = @" + val);
                            }
                            else
                            {
                                sb.Append(" = @" + where);
                            }
                        }

                    }
                }
                DbCommand command = db.GetSqlStringCommand(sb.ToString());


                bool flag = false;
                foreach (var item in list)
                {
                    foreach (var where in whereFields)
                    {
                        PropertyInfo whereInfo = typeof(T).GetProperty(where);

                        if (where.Equals(only))
                        {
                            if (flag)
                            {
                                break;
                            }
                            flag = true;
                        }
                        if (prefixField.Equals(where))
                        {
                            db.AddInParameter(command, where, GetDBType(whereInfo), whereInfo.GetValue(item, null));
                        }
                        else
                        {
                            string val = whereInfo.GetValue(item, null).ToString();
                            if (val.StartsWith("P") || val.StartsWith("R"))
                            {
                                db.AddInParameter(command, val, GetDBType(whereInfo), whereInfo.GetValue(item, null));
                            }
                            else
                            {
                                db.AddInParameter(command, where, GetDBType(whereInfo), whereInfo.GetValue(item, null));
                            }
                        }
                    }
                }

                return db.ExecuteNonQuery(command);
            }
            return 0;
        }
        public static int AutoUpdateEntity(T entity, string[] upFields, string[] whereFields, bool isPlus)
        {

            //DALBase.InvalidateCache<T>();
            Database db = DatabaseFactory.CreateDatabase();
            string tableName = null;
            TableAttribute[] attrs = (TableAttribute[])typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
            if (attrs != null && attrs.Length > 0)
                tableName = attrs[0].TableName;

            if (tableName != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" UPDATE " + tableName + " SET ");


                for (int i = 0; i < upFields.Length; i++)
                {
                    var field = upFields[i];
                    PropertyInfo info = typeof(T).GetProperty(field);
                    if (i != 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(field);
                    sb.Append(" = ");
                    sb.Append(field);
                    sb.Append(isPlus ? " + " : " - ");
                    sb.Append(info.GetValue(entity, null) + " ");
                }
                sb.Append(" where 1 = 1 ");
                foreach (var where in whereFields)
                {
                    sb.Append(" and ");
                    sb.Append(where);
                    sb.Append(" = @" + where + " ");

                }

                DbCommand command = db.GetSqlStringCommand(sb.ToString());


                foreach (var where in whereFields)
                {
                    PropertyInfo info = typeof(T).GetProperty(where);
                    db.AddInParameter(command, where, GetDBType(info), info.GetValue(entity, null));

                }
                return db.ExecuteNonQuery(command);
            }
            return 0;
        }
    }

    public static class GetRowCount
    {
        /// <summary>
        /// 获取查询结果总数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int GetSelectRowCount(string sql)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand comm = db.GetSqlStringCommand(sql);
            object obj = db.ExecuteScalar(comm);
            return (obj == null) ? 0 : int.Parse(obj.ToString());
        }
        public static int GetSelectRowCount(string connStr, string sql)
        {
            Database db = DatabaseFactory.CreateDatabase(connStr);
            DbCommand comm = db.GetSqlStringCommand(sql);
            object obj = db.ExecuteScalar(comm);
            return (obj == null) ? 0 : int.Parse(obj.ToString());
        }
        public static int GetSelectRowCount(string sql, IDictionary<string, object> parameters, CommandType commandType)
        {
            int i = 0;
            using (SqlConnection sqlConnection = DatabaseFactory.CreateDatabase().CreateConnection() as SqlConnection)
            {
                SqlCommand command = new SqlCommand(sql, sqlConnection);
                command.CommandType = commandType;
                command.Parameters.Clear();
                foreach (var parameter in parameters)
                {
                    string parameterName = parameter.Key;
                    if (!parameter.Key.StartsWith("@"))
                    {
                        parameterName = "@" + parameterName;
                    }
                    command.Parameters.Add(new SqlParameter(parameterName, parameter.Value));
                }
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }

                object obj = command.ExecuteScalar();
                i = (obj == null) ? 0 : int.Parse(obj.ToString());
            }
            return i;
        }

        public static int GetSelectRowCount(string sql, List<SqlParameter> parameters, CommandType commandType)
        {
            int i = 0;
            using (SqlConnection sqlConnection = DatabaseFactory.CreateDatabase().CreateConnection() as SqlConnection)
            {
                SqlCommand command = new SqlCommand(sql, sqlConnection);
                command.CommandType = commandType;
                command.Parameters.Clear();

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.Open();
                }

                object obj = command.ExecuteScalar();
                i = (obj == null) ? 0 : int.Parse(obj.ToString());
            }
            return i;
        }

        public static object GetScalarObject(string sql)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand comm = db.GetSqlStringCommand(sql);
            object obj = db.ExecuteScalar(comm);
            return obj;
        }
    }
}
