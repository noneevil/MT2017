using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Reflection;
using FastReflectionLib;

namespace MSSQLDB
{
    public class SqlServerCe : DBUtils, IDataAccess
    {
        private String ConnectionString;
        /// <summary>
        /// Sql Server Compact 4.0
        /// </summary>
        /// <param name="constr">sql连接字符串</param>
        public SqlServerCe(String constr)
        {
            this.ConnectionString = constr;
        }
        /// <summary>
        /// Sql Server Compact 4.0
        /// </summary>
        /// <param name="DbName">数据库名称</param>
        /// <param name="IP">数据库IP</param>
        /// <param name="UID">连接用户</param>
        /// <param name="PWD">连接密码</param>
        public SqlServerCe(String DbName, String IP, String UID, String PWD)
        {
            if (String.IsNullOrEmpty(DbName) || String.IsNullOrEmpty(IP) || String.IsNullOrEmpty(UID) || String.IsNullOrEmpty(PWD))
            {
                throw new Exception("数据库连接参数错误！");
            }
            this.ConnectionString = String.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;Uid={2};PWD={3}", IP, DbName, UID, PWD);
        }

        #region

        private void CreateCommand(SqlCeCommand cmd, ExecuteObject sender)
        {
            String sqlStr = String.Empty;
            String sqlField = String.Empty;
            String sqlWhere = String.Empty;
            String sqlParam = String.Empty;
            String sqlValue = String.Empty;
            Dictionary<String, Type> schema = GetTableSchema(sender.tableName);

            #region 构造字段

            foreach (String key in schema.Keys)
            {
                if (sender.cells.Contains(key))
                {
                    if (sender.cmdtype == CmdType.INSERT)
                    {
                        sqlParam = sqlParam + "[" + key + "],";
                        sqlValue = sqlValue + "@" + key + ",";
                    }
                    else if (sender.cmdtype == CmdType.SELECT)
                    {
                        sqlField = sqlField + "[" + key + "],";
                    }
                    else
                    {
                        sqlField = sqlField + "[" + key + "]=@" + key + ",";
                    }

                    if (sender.cmdtype != CmdType.SELECT && sender.cmdtype != CmdType.DELETE)
                    {
                        cmd.Parameters.Add(new SqlCeParameter(key, sender.cells[key]));
                    }
                }
            }
            if (sender.cmdtype == CmdType.INSERT)
            {
                sqlField = "(" + sqlParam.Substring(0, sqlParam.Length - 1) + ") VALUES (" + sqlValue.Substring(0, sqlValue.Length - 1) + ")";
            }
            else if (sqlField != "")
            {
                sqlField = sqlField.Substring(0, sqlField.Length - 1);
            }

            #endregion

            #region 构造条件

            if (sender.cmdtype != CmdType.INSERT)
            {
                foreach (String key in schema.Keys)
                {
                    String _key = key;
                    if (sender.terms.Contains(_key))
                    {
                        if (cmd.Parameters.Contains(_key)) _key += "_2";
                        sqlWhere = sqlWhere + "[" + key + "]=@" + _key + " AND ";
                        cmd.Parameters.Add(new SqlCeParameter(_key, sender.terms[key]));
                    }
                }
            }

            #endregion

            switch (sender.cmdtype)
            {
                case CmdType.SELECT:
                    if (sqlField == "")
                    {
                        sqlStr = "SELECT * FROM [" + sender.tableName + "] ";
                    }
                    else
                    {
                        sqlStr = "SELECT " + sqlField + " FROM [" + sender.tableName + "] ";
                    }
                    break;

                case CmdType.INSERT:
                    sqlStr = "INSERT INTO [" + sender.tableName + "]" + sqlField;
                    break;

                case CmdType.UPDATE:
                    sqlStr = "UPDATE [" + sender.tableName + "] SET " + sqlField;
                    break;

                case CmdType.DELETE:
                    sqlStr = "DELETE FROM [" + sender.tableName + "]";
                    break;
            }
            if (sqlWhere != "")
            {
                sqlWhere = " WHERE " + sqlWhere.Substring(0, sqlWhere.Length - 4);
                sqlStr = sqlStr + sqlWhere;
            }
            cmd.CommandText = sqlStr;
        }
        /// <summary>
        /// 获取数据表架构信息
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <returns>String[]</returns>
        public Dictionary<String, Type> GetTableSchema(String tablename)
        {
            String sql = String.Format("SELECT TOP 1 * FROM [{0}]", tablename);
            return GetTableSchema(tablename, sql);
        }

        #endregion

        #region 返回 DataSet

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>DataSets</returns>
        public DataSet ExecuteDataSet(String SQL)
        {
            DataSet set;

            using (SqlCeConnection connection = new SqlCeConnection(this.ConnectionString))
            {
                using (SqlCeCommand command = new SqlCeCommand())
                {
                    command.CommandText = SQL;
                    command.Connection = connection;
                    connection.Open();

                    try
                    {
                        using (SqlCeDataAdapter adapter = new SqlCeDataAdapter(command))
                        {
                            set = new DataSet();
                            adapter.Fill(set);
                        }
                    }
                    finally { }
                }
            }
            return set;
        }
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <param name="sender">ExecuteObject对象</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(ExecuteObject sender)
        {
            DataSet set;

            using (SqlCeConnection connection = new SqlCeConnection(this.ConnectionString))
            {
                using (SqlCeCommand command = new SqlCeCommand())
                {
                    CreateCommand(command, sender);
                    command.Connection = connection;
                    connection.Open();

                    try
                    {
                        using (SqlCeDataAdapter adapter = new SqlCeDataAdapter(command))
                        {
                            set = new DataSet();
                            adapter.Fill(set);
                        }
                    }
                    finally { }
                }
            }
            return set;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(String proceName, Hashtable Parameters)
        {
            DataSet set;

            using (SqlCeConnection connection = new SqlCeConnection(this.ConnectionString))
            {
                using (SqlCeCommand command = new SqlCeCommand())
                {
                    command.Connection = connection;
                    command.CommandText = proceName;
                    command.CommandType = CommandType.StoredProcedure;
                    IDictionaryEnumerator enumerator = Parameters.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        command.Parameters.Add(new SqlCeParameter(enumerator.Key.ToString(), enumerator.Value));
                    }
                    connection.Open();

                    try
                    {
                        using (SqlCeDataAdapter adapter = new SqlCeDataAdapter(command))
                        {
                            set = new DataSet();
                            adapter.Fill(set);
                        }
                    }
                    finally { }
                }
            }
            return set;
        }

        public DataSet ExecuteDataSet(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 返回 Boolean

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>Boolean</returns>
        public Boolean ExecuteCommand(String SQL)
        {
            Boolean flag;

            using (SqlCeConnection connection = new SqlCeConnection(this.ConnectionString))
            {
                using (SqlCeCommand command = new SqlCeCommand())
                {
                    command.CommandText = SQL;
                    command.Connection = connection;
                    connection.Open();

                    using (SqlCeTransaction transaction = command.Connection.BeginTransaction())
                    {
                        try
                        {
                            command.Transaction = transaction;
                            int num = command.ExecuteNonQuery();
                            transaction.Commit();
                            flag = num >= 0;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new ApplicationException(ex.Message);
                        }
                        finally { }
                    }
                }
            }
            return flag;
        }
        /// <summary>
        /// 执行插入或更新实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="cmdtype">INSERT 或 UPDATE</param>
        /// <returns>Boolean</returns>
        public Boolean ExecuteCommand<T>(T entity, CmdType cmdtype)
        {
            if (cmdtype != CmdType.INSERT && cmdtype != CmdType.UPDATE)
                throw new Exception("只支持INSERT 或 UPDATE命令.");
            Boolean flag = false;

            #region 转换为List对象

            Type baseType = typeof(T);
            Type makeType = baseType.IsGenericType ? baseType.GetGenericArguments()[0] : baseType;
            Type makelist = typeof(List<>).MakeGenericType(makeType);
            Object list;
            if (baseType.IsGenericType)
            {
                list = entity;
                //MethodInfo add = makelist.GetMethod("Add");
                //MethodInfo _getenumerator = baseType.GetMethod("GetEnumerator");
                //IEnumerator _items = _getenumerator.Invoke(entity, null) as IEnumerator;
                //while (_items.MoveNext())
                //{
                //    add.Invoke(list, new Object[] { _items.Current });
                //}
            }
            else
            {
                list = Activator.CreateInstance(makelist);
                /*--makelist.GetMethod("Add").Invoke(list, new Object[] { entity });*/
                makelist.GetMethod("Add").FastInvoke(list, new Object[] { entity });
            }

            #endregion

            String tableName = String.Empty;
            TableAttribute[] _atttable = (TableAttribute[])makeType.GetCustomAttributes(typeof(TableAttribute), false);
            if (_atttable != null && _atttable.Length > 0) tableName = _atttable[0].TableName;
            if (String.IsNullOrEmpty(tableName)) return false;
            PropertyInfo[] properties = makeType.GetProperties();

            using (SqlCeConnection connection = new SqlCeConnection(this.ConnectionString))
            {
                connection.Open();

                MethodInfo enumerator = makelist.GetMethod("GetEnumerator");
                /*--IEnumerator items = enumerator.Invoke(list, null) as IEnumerator;*/
                IEnumerator items = enumerator.FastInvoke(list, null) as IEnumerator;
                while (items.MoveNext())
                {
                    Object item = items.Current;

                    String sqlStr = String.Empty;
                    String sqlField = String.Empty;
                    String sqlWhere = String.Empty;
                    String sqlParam = String.Empty;
                    String sqlValue = String.Empty;

                    using (SqlCeCommand command = new SqlCeCommand())
                    {
                        #region 构造SQL语句

                        foreach (PropertyInfo field in properties)
                        {
                            if (!field.CanRead) continue;
                            /*--Object value = field.GetValue(item, null);*/
                            Object value = field.FastGetValue(item);
                            if (value == null || value == DBNull.Value) continue;
                            FieldAttribute[] attfield = (FieldAttribute[])field.GetCustomAttributes(typeof(FieldAttribute), false);
                            if (attfield != null && attfield.Length > 0)
                            {
                                FieldAttribute att = attfield[0];
                                if (att.IsSeed || att.IsVirtual || att.IsIgnore) continue;
                                if (att.IsPrimaryKey)
                                {
                                    if (cmdtype == CmdType.UPDATE)
                                    {
                                        sqlWhere = sqlWhere + "[" + att.Name + "]=@" + att.Name + " AND ";
                                        command.Parameters.Add(new SqlCeParameter(att.Name, value));
                                    }
                                }
                                else if (att.isAllowEdit)
                                {
                                    if (cmdtype == CmdType.INSERT)
                                    {
                                        sqlParam = sqlParam + "[" + att.Name + "],";
                                        sqlValue = sqlValue + "@" + att.Name + ",";
                                    }
                                    else if (cmdtype == CmdType.UPDATE)
                                    {
                                        sqlField = sqlField + "[" + att.Name + "]=@" + att.Name + ",";
                                    }
                                    command.Parameters.Add(new SqlCeParameter(att.Name, value));
                                }
                            }
                        }

                        if (cmdtype == CmdType.INSERT)
                        {
                            sqlStr = "INSERT INTO [" + tableName + "] (" + sqlParam.Substring(0, sqlParam.Length - 1) + ") VALUES (" + sqlValue.Substring(0, sqlValue.Length - 1) + ")";
                        }
                        else if (cmdtype == CmdType.UPDATE)
                        {
                            sqlStr = "UPDATE [" + tableName + "] SET " + sqlField.Substring(0, sqlField.Length - 1);
                        }

                        if (cmdtype == CmdType.UPDATE && sqlWhere != "")
                        {
                            sqlWhere = " WHERE " + sqlWhere.Substring(0, sqlWhere.Length - 4);
                            sqlStr = sqlStr + sqlWhere;
                        }

                        #endregion

                        command.CommandText = sqlStr;
                        command.Connection = connection;
                        using (SqlCeTransaction transaction = command.Connection.BeginTransaction())
                        {
                            try
                            {
                                command.Transaction = transaction;
                                int num = command.ExecuteNonQuery();
                                transaction.Commit();
                                flag = num >= 0;
                                if (!flag) break;
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw new ApplicationException(ex.Message);
                            }
                            finally { }
                        }
                    }
                }
            }
            return flag;
        }
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>Boolean</returns>
        public Boolean ExecuteCommand(ExecuteObject sender)
        {
            Boolean flag;

            using (SqlCeConnection connection = new SqlCeConnection(this.ConnectionString))
            {
                using (SqlCeCommand command = new SqlCeCommand())
                {
                    CreateCommand(command, sender);
                    command.Connection = connection;
                    connection.Open();

                    using (SqlCeTransaction transaction = command.Connection.BeginTransaction())
                    {
                        try
                        {
                            command.Transaction = transaction;
                            int num = command.ExecuteNonQuery();
                            transaction.Commit();
                            flag = num >= 0;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new ApplicationException(ex.Message);
                        }
                        finally { }
                    }
                }
            }
            return flag;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>Boolean</returns>
        public Boolean ExecuteCommand(String proceName, Hashtable Parameters)
        {
            Boolean flag;

            using (SqlCeConnection connection = new SqlCeConnection(this.ConnectionString))
            {
                using (SqlCeCommand command = new SqlCeCommand())
                {
                    command.Connection = connection;
                    command.CommandText = proceName;
                    command.CommandType = CommandType.StoredProcedure;
                    IDictionaryEnumerator enumerator = Parameters.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        command.Parameters.Add(new SqlCeParameter(enumerator.Key.ToString(), enumerator.Value));
                    }
                    connection.Open();

                    using (SqlCeTransaction transaction = command.Connection.BeginTransaction())
                    {
                        try
                        {
                            command.Transaction = transaction;
                            int num = command.ExecuteNonQuery();
                            transaction.Commit();
                            flag = num >= 0;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new ApplicationException(ex.Message);
                        }
                        finally { }
                    }
                }
            }
            return flag;
        }

        public Boolean ExecuteCommand(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region 返回DataTable

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(String SQL)
        {
            return ExecuteDataSet(SQL).Tables[0];
        }
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(ExecuteObject sender)
        {
            return ExecuteDataSet(sender).Tables[0];
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(String proceName, Hashtable Parameters)
        {
            return ExecuteDataSet(proceName, Parameters).Tables[0];
        }

        public DataTable ExecuteDataTable(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 返回IDataReader

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>IDataReader</returns>
        public IDataReader ExecuteReader(String SQL)
        {
            SqlCeDataReader reader;

            using (SqlCeCommand command = new SqlCeCommand())
            {
                SqlCeConnection connection = new SqlCeConnection(this.ConnectionString);
                command.CommandText = SQL;
                command.Connection = connection;
                connection.Open();

                try
                {
                    reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                }
                finally { }
            }
            return reader;
        }
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>IDataReader</returns>
        public IDataReader ExecuteReader(ExecuteObject sender)
        {
            SqlCeDataReader reader;

            using (SqlCeCommand command = new SqlCeCommand())
            {
                SqlCeConnection connection = new SqlCeConnection(this.ConnectionString);
                CreateCommand(command, sender);
                command.Connection = connection;
                connection.Open();

                try
                {
                    reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                }
                finally { }
            }
            return reader;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>IDataReader</returns>
        public IDataReader ExecuteReader(String proceName, Hashtable Parameters)
        {
            SqlCeDataReader reader;

            using (SqlCeCommand command = new SqlCeCommand())
            {
                SqlCeConnection connection = new SqlCeConnection(this.ConnectionString);
                command.Connection = connection;
                command.CommandText = proceName;
                command.CommandType = CommandType.StoredProcedure;
                IDictionaryEnumerator enumerator = Parameters.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    command.Parameters.Add(new SqlCeParameter(enumerator.Key.ToString(), enumerator.Value));
                }
                connection.Open();

                try
                {
                    reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                }
                finally { }
            }
            return reader;
        }
        public IDataReader ExecuteReader(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 返回单个字段值

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>Object</returns>
        public Object ExecuteScalar(String SQL)
        {
            Object obj;

            using (SqlCeConnection connection = new SqlCeConnection(this.ConnectionString))
            {
                using (SqlCeCommand command = new SqlCeCommand())
                {
                    command.Connection = connection;
                    command.CommandText = SQL;
                    connection.Open();

                    try
                    {
                        obj = command.ExecuteScalar();
                    }
                    finally { }
                }
            }
            return obj;
        }
        public Object ExecuteScalar(ExecuteObject sender)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>Object</returns>
        public Object ExecuteScalar(String proceName, Hashtable Parameters)
        {
            Object obj;
            using (SqlCeConnection connection = new SqlCeConnection(this.ConnectionString))
            {
                using (SqlCeCommand command = new SqlCeCommand())
                {
                    command.Connection = connection;
                    command.CommandText = proceName;
                    command.CommandType = CommandType.StoredProcedure;
                    IDictionaryEnumerator enumerator = Parameters.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        command.Parameters.Add(new SqlCeParameter(enumerator.Key.ToString(), enumerator.Value));
                    }
                    connection.Open();

                    try
                    {
                        obj = command.ExecuteScalar();
                    }
                    finally { }
                }
            }
            return obj;
        }

        public Object ExecuteScalar(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 返回泛型数据

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="SQL">SQL语句</param>
        /// <returns>泛型</returns>
        public T ExecuteObject<T>(String SQL)
        {
            T result = Activator.CreateInstance<T>();
            using (IDataReader reader = ExecuteReader(SQL))
            {
                SetValue<T>(reader, ref result);
            }
            return result;
        }
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>泛型</returns>
        public T ExecuteObject<T>(ExecuteObject sender)
        {
            T result = Activator.CreateInstance<T>();
            using (IDataReader reader = ExecuteReader(sender))
            {
                SetValue<T>(reader, ref result);
            }
            return result;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>泛型</returns>
        public T ExecuteObject<T>(String proceName, Hashtable Parameters)
        {
            T result = Activator.CreateInstance<T>();
            using (IDataReader reader = ExecuteReader(proceName, Parameters))
            {
                SetValue<T>(reader, ref result);
            }
            return result;
        }

        public T ExecuteObject<T>(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 返回JSON字符串

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>Json字符串</returns>
        public String ExecuteJson(String SQL)
        {
            String json = String.Empty;
            using (IDataReader reader = ExecuteReader(SQL))
            {
                ConvertToJson(reader, ref json);
            }
            return json;
        }
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>Json字符串</returns>
        public String ExecuteJson(ExecuteObject sender)
        {
            String json = String.Empty;
            using (IDataReader reader = ExecuteReader(sender))
            {
                ConvertToJson(reader, ref json);
            }
            return json;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>Json字符串</returns>
        public String ExecuteJson(String proceName, Hashtable Parameters)
        {
            String json = String.Empty;
            using (IDataReader reader = ExecuteReader(proceName, Parameters))
            {
                ConvertToJson(reader, ref json);
            }
            return json;
        }

        public String ExecuteJson(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
