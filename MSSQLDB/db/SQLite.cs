using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using FastReflectionLib;

namespace MSSQLDB
{
    /// <summary>
    /// SQLite数据库
    /// </summary>
    public class SQLite : DBUtils, IDataAccess
    {
        private String ConnectionString;
        /// <summary>
        /// SQLite 数据库
        /// </summary>
        /// <param name="constr">sql连接字符串</param>
        public SQLite(String constr)
        {
            this.ConnectionString = constr;
        }
        /// <summary>
        /// SQLite 数据库
        /// </summary>
        /// <param name="DbPath">数据库名称</param>
        /// <param name="PWD">连接密码</param>
        public SQLite(String DbPath, String PWD)
        {
            if (String.IsNullOrEmpty(DbPath))
            {
                throw new Exception("数据库连接参数错误！");
            }
            this.ConnectionString = String.Format("Data Source={0};Version=3;Cache Size=8000;Page Size=4096;Synchronous=Off;journal mode=Off;", DbPath);
        }

        #region

        private void CreateCommand(SQLiteCommand cmd, ExecuteObject sender)
        {
            String sqlStr = String.Empty;
            String sqlField = String.Empty;
            String sqlWhere = String.Empty;
            String sqlParam = String.Empty;
            String sqlValue = String.Empty;
            Dictionary<String, Type> schema = GetTableSchema(sender.tableName);

            //#region 字段名称转小写
            //Hashtable cells = new Hashtable();
            //foreach (String k in sender.cells.Keys)
            //{
            //    cells.Add(k.ToLower(), sender.cells[k]);
            //}
            //Hashtable terms = new Hashtable();
            //foreach (String k in sender.terms.Keys)
            //{
            //    terms.Add(k.ToLower(), sender.terms[k]);
            //}
            //#endregion

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
                        cmd.Parameters.Add(new SQLiteParameter(key, sender.cells[key]));
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
                        cmd.Parameters.Add(new SQLiteParameter(_key, sender.terms[key]));
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

        public Dictionary<String, Type> GetTableSchema(String tablename)
        {
            String sql = String.Format("SELECT * FROM [{0}] LIMIT 1", tablename);
            return GetTableSchema(tablename, sql);
        }

        #endregion

        #region IDataReader

        public IDataReader ExecuteReader(String SQL)
        {
            SQLiteDataReader reader;
            using (SQLiteCommand command = new SQLiteCommand())
            {
                SQLiteConnection connection = new SQLiteConnection(this.ConnectionString);
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

        public IDataReader ExecuteReader(ExecuteObject sender)
        {
            SQLiteDataReader reader;

            using (SQLiteCommand command = new SQLiteCommand())
            {
                SQLiteConnection connection = new SQLiteConnection(this.ConnectionString);
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

        public IDataReader ExecuteReader(String proceName, Hashtable Parameters)
        {
            throw new NotImplementedException();
        }

        public IDataReader ExecuteReader(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Boolean

        public Boolean ExecuteCommand(String SQL)
        {
            Boolean flag;

            using (SQLiteConnection connection = new SQLiteConnection(this.ConnectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand())
                {
                    command.CommandText = SQL;
                    command.Connection = connection;
                    connection.Open();

                    using (SQLiteTransaction transaction = command.Connection.BeginTransaction())
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

            using (SQLiteConnection connection = new SQLiteConnection(this.ConnectionString))
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

                    using (SQLiteCommand command = new SQLiteCommand())
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
                                        command.Parameters.Add(new SQLiteParameter(att.Name, value));
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
                                    command.Parameters.Add(new SQLiteParameter(att.Name, value));
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
                        using (SQLiteTransaction transaction = command.Connection.BeginTransaction())
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

        public Boolean ExecuteCommand(ExecuteObject sender)
        {
            Boolean flag;

            using (SQLiteConnection connection = new SQLiteConnection(this.ConnectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand())
                {
                    CreateCommand(command, sender);
                    command.Connection = connection;
                    connection.Open();

                    using (SQLiteTransaction transaction = command.Connection.BeginTransaction())
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

        public Boolean ExecuteCommand(String proceName, Hashtable Parameters)
        {
            throw new NotImplementedException();
        }

        public Boolean ExecuteCommand(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region DataSet

        public DataSet ExecuteDataSet(String SQL)
        {
            DataSet set;

            using (SQLiteConnection connection = new SQLiteConnection(this.ConnectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand())
                {
                    command.CommandText = SQL;
                    command.Connection = connection;
                    connection.Open();

                    try
                    {
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
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

        public DataSet ExecuteDataSet(ExecuteObject sender)
        {
            DataSet set;

            using (SQLiteConnection connection = new SQLiteConnection(this.ConnectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand())
                {
                    CreateCommand(command, sender);
                    command.Connection = connection;
                    connection.Open();

                    try
                    {
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
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

        public DataSet ExecuteDataSet(String proceName, Hashtable Parameters)
        {
            throw new NotImplementedException();
        }

        public DataSet ExecuteDataSet(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region DataTable

        public DataTable ExecuteDataTable(String SQL)
        {
            return ExecuteDataSet(SQL).Tables[0];
        }

        public DataTable ExecuteDataTable(ExecuteObject sender)
        {
            return ExecuteDataSet(sender).Tables[0];
        }

        public DataTable ExecuteDataTable(String proceName, Hashtable Parameters)
        {
            throw new NotImplementedException();
        }

        public DataTable ExecuteDataTable(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Object

        public Object ExecuteScalar(String SQL)
        {
            Object obj;

            using (SQLiteConnection connection = new SQLiteConnection(this.ConnectionString))
            {
                using (SQLiteCommand command = new SQLiteCommand())
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

        public Object ExecuteScalar(String proceName, Hashtable Parameters)
        {
            throw new NotImplementedException();
        }

        public Object ExecuteScalar(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 泛型

        public T ExecuteObject<T>(String SQL)
        {
            T result = Activator.CreateInstance<T>();
            using (IDataReader reader = ExecuteReader(SQL))
            {
                SetValue<T>(reader, ref result);
            }
            return result;
        }

        public T ExecuteObject<T>(ExecuteObject sender)
        {
            T result = Activator.CreateInstance<T>();
            using (IDataReader reader = ExecuteReader(sender))
            {
                SetValue<T>(reader, ref result);
            }
            return result;
        }

        public T ExecuteObject<T>(String proceName, Hashtable Parameters)
        {
            throw new NotImplementedException();
        }

        public T ExecuteObject<T>(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region JSON

        public String ExecuteJson(String SQL)
        {
            String json = String.Empty;
            using (IDataReader reader = ExecuteReader(SQL))
            {
                ConvertToJson(reader, ref json);
            }
            return json;
        }

        public String ExecuteJson(ExecuteObject sender)
        {
            String json = String.Empty;
            using (IDataReader reader = ExecuteReader(sender))
            {
                ConvertToJson(reader, ref json);
            }
            return json;
        }

        public String ExecuteJson(String proceName, Hashtable Parameters)
        {
            throw new NotImplementedException();
        }

        public String ExecuteJson(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
