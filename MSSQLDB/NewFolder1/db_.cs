using System;
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace MSSQLDB
{
    public class db
    {
        private static String m_ConnectionString = "";
        private static String m_DbName = "";
        private static DatabaseType m_dbType;
        private static String m_IP = ".";
        private static String m_PWD = "";
        private static String m_UID = "";

        public static Hashtable TableItems = new Hashtable();

        public static Boolean CheckItemExist(Hashtable ht, String tablename)
        {
            String sQL = "SELECT TOP 1 * FROM [" + tablename + "]";
            String str2 = "";
            IDictionaryEnumerator enumerator = ht.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Object obj2 = str2;
                str2 = String.Concat(new Object[] { obj2, enumerator.Key, "='", enumerator.Value, "' AND " });
            }
            if (str2 != "")
            {
                str2 = str2.Substring(0, str2.Length - 4);
                sQL = sQL + " WHERE " + str2;
            }
            return (ExecuteDataSet(sQL).Tables[0].Rows.Count > 0);
        }
        public static Boolean CheckItemExist(Hashtable ht, String tablename, String DbName)
        {
            String sQL = "SELECT TOP 1 * FROM [" + tablename + "]";
            String str2 = "";
            IDictionaryEnumerator enumerator = ht.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Object obj2 = str2;
                str2 = String.Concat(new Object[] { obj2, enumerator.Key, "='", enumerator.Value, "' AND " });
            }
            if (str2 != "")
            {
                str2 = str2.Substring(0, str2.Length - 4);
                sQL = sQL + " WHERE " + str2;
            }
            return (ExecuteDataSet(sQL, DbName).Tables[0].Rows.Count > 0);
        }
        public static Boolean CheckTableName(String tablename)
        {
            DataSet set = ExecuteDataSet(String.Format(ChkSql, tablename));// ExecuteDataSet("SELECT TOP 1 * FROM [" + tablename + "]");
            if (set == null)
            {
                return false;
            }
            if (set.Tables.Count == 0)
            {
                return false;
            }
            return true;
        }
        public static Boolean CheckTableName(String tablename, String DbName)
        {
            DataSet set = ExecuteDataSet(String.Format(ChkSql, tablename), DbName);// ExecuteDataSet("SELECT TOP 1 * FROM [" + tablename + "]", DbName);
            if (set == null)
            {
                return false;
            }
            if (set.Tables.Count == 0)
            {
                return false;
            }
            return true;
        }

        public static void CreateCommand(out IDbCommand cmd, ExecuteObject sender)
        {
            CreateCommand(out cmd, sender, null);
        }
        public static void CreateCommand(out IDbCommand cmd, ExecuteObject sender, String DbName)
        {
            String str;
            DataTable tableInfo = GetTableInfo(sender.tableName);
            cmd = GetCommand(DbName);
            String str2 = "";
            String str3 = "";
            String str4 = "";
            String str5 = "";
            String str6 = "";
            String key = "";
            foreach (DataColumn column in tableInfo.Columns)
            {
                key = column.ColumnName.ToLower();
                if (sender.cells.Contains(key))
                {
                    if (sender.cmdtype != CmdType.INSERT)
                    {
                        if (sender.cmdtype == CmdType.SELECT)
                        {
                            str3 = str3 + "[" + key + "],";
                        }
                        else
                        {
                            str = str3;
                            str3 = str + "[" + key + "]=@" + key + ",";
                        }
                    }
                    else
                    {
                        str5 = str5 + "[" + key + "],";
                        str6 = str6 + "@" + key + ",";
                    }
                    if (sender.cmdtype != CmdType.SELECT)
                    {
                        cmd.Parameters.Add(GetParameter(key, sender.cells[key]));
                    }
                }
            }
            foreach (DataColumn column in tableInfo.Columns)
            {
                key = column.ColumnName.ToLower();
                if (sender.terms.Contains(key))
                {
                    str = str4;
                    str4 = str + "[" + key + "]=@" + key + " AND ";
                    cmd.Parameters.Add(GetParameter(key, sender.terms[key]));
                }
            }
            if (sender.cmdtype == CmdType.INSERT)
            {
                str3 = "(" + str5.Substring(0, str5.Length - 1) + ") VALUES (" + str6.Substring(0, str6.Length - 1) + ")";
            }
            else if (str3 != "")
            {
                str3 = str3.Substring(0, str3.Length - 1);
            }
            switch (sender.cmdtype)
            {
                case CmdType.SELECT:
                    if (str3 == "")
                    {
                        str2 = "SELECT * FROM [" + sender.tableName + "] ";
                        break;
                    }
                    str2 = "SELECT " + str3 + " FROM [" + sender.tableName + "] ";
                    break;

                case CmdType.INSERT:
                    str2 = "INSERT INTO [" + sender.tableName + "]" + str3;
                    break;

                case CmdType.UPDATE:
                    str2 = "UPDATE [" + sender.tableName + "] SET " + str3;
                    break;

                case CmdType.DELETE:
                    switch (CurrDbType)
                    {
                        case DatabaseType.ACCESS:
                        case DatabaseType.SQLite:
                            str2 = "DELETE FROM [" + sender.tableName + "]";
                            break;
                        default:
                            str2 = "DELETE [" + sender.tableName + "]";
                            break;
                    }
                    break;
            }
            if (str4 != "")
            {
                str4 = " WHERE " + str4.Substring(0, str4.Length - 4);
                str2 = str2 + str4;
            }
            cmd.CommandText = str2;
        }

        public static Boolean ExecuteCommand(ExecuteObject sender)
        {
            IDbCommand command;
            Boolean flag;
            CreateCommand(out command, sender);
            IDbTransaction transaction = command.Connection.BeginTransaction();
            command.Transaction = transaction;
            try
            {
                int num = command.ExecuteNonQuery();
                transaction.Commit();
                flag = num >= 0;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw new ApplicationException(exception.Message);
            }
            finally
            {
                if (command != null)
                {
                    transaction.Dispose();
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return flag;
        }
        public static Boolean ExecuteCommand(String SQL)
        {
            Boolean flag;
            IDbCommand command = GetCommand();
            IDbTransaction transaction = command.Connection.BeginTransaction();
            command.Transaction = transaction;
            command.CommandText = SQL;
            try
            {
                int num = command.ExecuteNonQuery();
                transaction.Commit();
                flag = num >= 0;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw new ApplicationException(exception.Message);
            }
            finally
            {
                if (command.Connection.State != ConnectionState.Closed)
                {
                    transaction.Dispose();
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return flag;
        }
        public static Boolean ExecuteCommand(ExecuteObject sender, String DbName)
        {
            IDbCommand command;
            Boolean flag;
            CreateCommand(out command, sender, DbName);
            IDbTransaction transaction = command.Connection.BeginTransaction();
            command.Transaction = transaction;
            try
            {
                int num = command.ExecuteNonQuery();
                transaction.Commit();
                flag = num >= 0;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw new ApplicationException(exception.Message);
            }
            finally
            {
                if (command != null)
                {
                    transaction.Dispose();
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return flag;
        }
        public static Boolean ExecuteCommand(String SQL, Hashtable Parameters)
        {
            Boolean flag;
            IDbCommand command = GetCommand();
            IDbTransaction transaction = command.Connection.BeginTransaction();
            command.Transaction = transaction;
            command.CommandText = SQL;
            IDictionaryEnumerator enumerator = Parameters.GetEnumerator();
            while (enumerator.MoveNext())
            {
                command.Parameters.Add(GetParameter(enumerator.Key.ToString(), enumerator.Value));
            }
            try
            {
                int num = command.ExecuteNonQuery();
                transaction.Commit();
                flag = num >= 0;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw new ApplicationException(exception.Message);
            }
            finally
            {
                if (command.Connection.State != ConnectionState.Closed)
                {
                    transaction.Dispose();
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return flag;
        }
        public static Boolean ExecuteCommand(String SQL, String DbName)
        {
            Boolean flag;
            IDbCommand command = GetCommand(DbName);
            IDbTransaction transaction = command.Connection.BeginTransaction();
            command.Transaction = transaction;
            command.CommandText = SQL;
            try
            {
                int num = command.ExecuteNonQuery();
                transaction.Commit();
                flag = num >= 0;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw new ApplicationException(exception.Message);
            }
            finally
            {
                if (command.Connection.State != ConnectionState.Closed)
                {
                    transaction.Dispose();
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return flag;
        }
        public static Boolean ExecuteCommand(String SQL, Hashtable Parameters, String DbName)
        {
            Boolean flag;
            IDbCommand command = GetCommand(DbName);
            IDbTransaction transaction = command.Connection.BeginTransaction();
            command.Transaction = transaction;
            command.CommandText = SQL;
            IDictionaryEnumerator enumerator = Parameters.GetEnumerator();
            while (enumerator.MoveNext())
            {
                command.Parameters.Add(GetParameter(enumerator.Key.ToString(), enumerator.Value));
            }
            try
            {
                int num = command.ExecuteNonQuery();
                transaction.Commit();
                flag = num >= 0;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw new ApplicationException(exception.Message);
            }
            finally
            {
                if (command.Connection.State != ConnectionState.Closed)
                {
                    transaction.Dispose();
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return flag;
        }

        public static DataSet ExecuteDataSet(ExecuteObject sender)
        {
            IDbCommand command;
            DataSet set;
            CreateCommand(out command, sender);
            try
            {
                IDbDataAdapter dataAdapter = GetDataAdapter();
                dataAdapter.SelectCommand = command;
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                set = dataSet;
            }
            finally
            {
                if (command.Connection.State != ConnectionState.Closed)
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return set;
        }
        public static DataSet ExecuteDataSet(String SQL)
        {
            DataSet set;
            IDbCommand command = GetCommand();
            try
            {
                command.CommandText = SQL;
                IDbDataAdapter dataAdapter = GetDataAdapter();
                dataAdapter.SelectCommand = command;
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                set = dataSet;
            }
            finally
            {
                if (command != null)
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return set;
        }
        public static DataSet ExecuteDataSet(ExecuteObject sender, String DbName)
        {
            IDbCommand command;
            DataSet set;
            CreateCommand(out command, sender, DbName);
            try
            {
                IDbDataAdapter dataAdapter = GetDataAdapter();
                dataAdapter.SelectCommand = command;
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                set = dataSet;
            }
            finally
            {
                if (command.Connection.State != ConnectionState.Closed)
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return set;
        }
        public static DataSet ExecuteDataSet(String SQL, Hashtable Parameters)
        {
            DataSet set;
            IDbCommand command = GetCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = SQL;
            IDictionaryEnumerator enumerator = Parameters.GetEnumerator();
            while (enumerator.MoveNext())
            {
                command.Parameters.Add(GetParameter(enumerator.Key.ToString(), enumerator.Value));
            }
            try
            {
                IDbDataAdapter dataAdapter = GetDataAdapter();
                dataAdapter.SelectCommand = command;
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                set = dataSet;
            }
            finally
            {
                if (command != null)
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return set;
        }
        public static DataSet ExecuteDataSet(String SQL, String DbName)
        {
            DataSet set;
            IDbCommand command = GetCommand(DbName);
            try
            {
                command.CommandText = SQL;
                IDbDataAdapter dataAdapter = GetDataAdapter();
                dataAdapter.SelectCommand = command;
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                set = dataSet;
            }
            finally
            {
                if (command != null)
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return set;
        }
        public static DataSet ExecuteDataSet(String SQL, Hashtable Parameters, String DbName)
        {
            DataSet set;
            IDbCommand command = GetCommand(DbName);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = SQL;
            IDictionaryEnumerator enumerator = Parameters.GetEnumerator();
            while (enumerator.MoveNext())
            {
                command.Parameters.Add(GetParameter(enumerator.Key.ToString(), enumerator.Value));
            }
            try
            {
                IDbDataAdapter dataAdapter = GetDataAdapter();
                dataAdapter.SelectCommand = command;
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                set = dataSet;
            }
            finally
            {
                if (command != null)
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return set;
        }

        public static DataTable ExecuteDataTable(ExecuteObject sender)
        {
            return ExecuteDataSet(sender).Tables[0];
        }
        public static DataTable ExecuteDataTable(String SQL)
        {
            return ExecuteDataSet(SQL).Tables[0];
        }
        public static DataTable ExecuteDataTable(ExecuteObject sender, String DbName)
        {
            return ExecuteDataSet(sender, DbName).Tables[0];
        }
        public static DataTable ExecuteDataTable(String SQL, Hashtable Parameters)
        {
            return ExecuteDataSet(SQL, Parameters).Tables[0];
        }
        public static DataTable ExecuteDataTable(String SQL, String DbName)
        {
            return ExecuteDataSet(SQL, DbName).Tables[0];
        }
        public static DataTable ExecuteDataTable(String SQL, Hashtable Parameters, String DbName)
        {
            return ExecuteDataSet(SQL, Parameters, DbName).Tables[0];
        }

        public static Boolean ExecutePROCEDURE(String proceName, Hashtable Fields)
        {
            Boolean flag;
            IDbCommand command = GetCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = proceName;
            IDictionaryEnumerator enumerator = Fields.GetEnumerator();
            while (enumerator.MoveNext())
            {
                command.Parameters.Add(GetParameter(enumerator.Key.ToString(), enumerator.Value));
            }
            try
            {
                flag = command.ExecuteNonQuery() >= 0;
            }
            catch (Exception exception)
            {
                throw new ApplicationException(exception.Message);
            }
            finally
            {
                if (command != null)
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return flag;
        }
        public static Boolean ExecutePROCEDURE(String proceName, Hashtable Fields, String DbName)
        {
            Boolean flag;
            IDbCommand command = GetCommand(DbName);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = proceName;
            IDictionaryEnumerator enumerator = Fields.GetEnumerator();
            while (enumerator.MoveNext())
            {
                command.Parameters.Add(GetParameter(enumerator.Key.ToString(), enumerator.Value));
            }
            try
            {
                flag = command.ExecuteNonQuery() >= 0;
            }
            catch (Exception exception)
            {
                throw new ApplicationException(exception.Message);
            }
            finally
            {
                if (command != null)
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return flag;
        }

        public static IDataReader ExecuteReader(ExecuteObject sender)
        {
            IDbCommand command;
            IDataReader reader;
            CreateCommand(out command, sender);
            try
            {
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception exception)
            {
                throw new ApplicationException(exception.Message);
            }
            finally
            {
                if (command != null)
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return reader;
        }
        public static IDataReader ExecuteReader(String SQL)
        {
            IDataReader reader;
            IDbCommand command = GetCommand();
            command.CommandText = SQL;
            try
            {
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception exception)
            {
                throw new ApplicationException(exception.Message);
            }
            finally
            {
                if (command != null)
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return reader;
        }
        public static IDataReader ExecuteReader(ExecuteObject sender, String DbName)
        {
            IDbCommand command;
            IDataReader reader;
            CreateCommand(out command, sender, DbName);
            try
            {
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception exception)
            {
                throw new ApplicationException(exception.Message);
            }
            finally
            {
                if (command != null)
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return reader;
        }
        public static IDataReader ExecuteReader(String SQL, Hashtable Parameters)
        {
            IDataReader reader;
            IDbCommand command = GetCommand();
            command.CommandText = SQL;
            IDictionaryEnumerator enumerator = Parameters.GetEnumerator();
            while (enumerator.MoveNext())
            {
                command.Parameters.Add(GetParameter(enumerator.Key.ToString(), enumerator.Value));
            }
            try
            {
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception exception)
            {
                throw new ApplicationException(exception.Message);
            }
            finally
            {
                if (command != null)
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return reader;
        }
        public static IDataReader ExecuteReader(String SQL, String DbName)
        {
            IDataReader reader;
            IDbCommand command = GetCommand(DbName);
            command.CommandText = SQL;
            try
            {
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception exception)
            {
                throw new ApplicationException(exception.Message);
            }
            finally
            {
                if (command != null)
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return reader;
        }
        public static IDataReader ExecuteReader(String SQL, Hashtable Parameters, String DbName)
        {
            IDataReader reader;
            IDbCommand command = GetCommand(DbName);
            command.CommandText = SQL;
            IDictionaryEnumerator enumerator = Parameters.GetEnumerator();
            while (enumerator.MoveNext())
            {
                command.Parameters.Add(GetParameter(enumerator.Key.ToString(), enumerator.Value));
            }
            try
            {
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception exception)
            {
                throw new ApplicationException(exception.Message);
            }
            finally
            {
                if (command != null)
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return reader;
        }

        public static String ExecuteScalar(String SQL)
        {
            String str;
            IDbCommand command = GetCommand();
            command.CommandText = SQL;
            try
            {
                Object obj2 = command.ExecuteScalar();
                if (obj2 == null)
                {
                    return null;
                }
                str = Convert.ToString(obj2);
            }
            finally
            {
                if (command != null)
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return str;
        }
        public static String ExecuteScalar(String proceName, Hashtable Fields)
        {
            String str;
            IDbCommand command = GetCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = proceName;
            IDictionaryEnumerator enumerator = Fields.GetEnumerator();
            while (enumerator.MoveNext())
            {
                command.Parameters.Add(GetParameter(enumerator.Key.ToString(), enumerator.Value));
            }
            try
            {
                str = Convert.ToString(command.ExecuteScalar());
            }
            finally
            {
                if (command != null)
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return str;
        }
        public static String ExecuteScalar(String SQL, String DbName)
        {
            String str;
            IDbCommand command = GetCommand(DbName);
            command.CommandText = SQL;
            try
            {
                Object obj2 = command.ExecuteScalar();
                if (obj2 == null)
                {
                    return null;
                }
                str = Convert.ToString(obj2);
            }
            finally
            {
                if (command != null)
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return str;
        }
        public static String ExecuteScalar(String proceName, Hashtable Fields, String DbName)
        {
            String str;
            IDbCommand command = GetCommand(DbName);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = proceName;
            IDictionaryEnumerator enumerator = Fields.GetEnumerator();
            while (enumerator.MoveNext())
            {
                command.Parameters.Add(GetParameter(enumerator.Key.ToString(), enumerator.Value));
            }
            try
            {
                str = Convert.ToString(command.ExecuteScalar());
            }
            finally
            {
                if (command != null)
                {
                    command.Connection.Close();
                    command.Dispose();
                }
            }
            return str;
        }

        private static IDbCommand GetCommand()
        {
            return GetCommand(null);
        }
        private static IDbCommand GetCommand(String DbName)
        {
            IDbCommand command;
            IDbConnection connection = GetConnection(DbName);
            switch (CurrDbType)
            {
                case DatabaseType.MSSQLSERVER:
                    command = new SqlCommand();
                    break;

                case DatabaseType.ACCESS:
                case DatabaseType.OLEDB:
                    command = new OleDbCommand();
                    break;

                case DatabaseType.ORACLE:
                    command = new OracleCommand();
                    break;

                case DatabaseType.ODBC:
                    command = new OdbcCommand();
                    break;

                case DatabaseType.SQLite:
                    command = new SQLiteCommand();
                    break;
                default:
                    command = new OleDbCommand();
                    break;
            }
            command.Connection = connection;
            connection.Open();
            return command;
        }
        private static IDbConnection GetConnection(String DbName)
        {
            IDbConnection connection;
            switch (CurrDbType)
            {
                case DatabaseType.MSSQLSERVER:
                    connection = new SqlConnection();
                    break;

                case DatabaseType.ACCESS:
                case DatabaseType.OLEDB:
                    connection = new OleDbConnection();
                    break;

                case DatabaseType.ORACLE:
                    connection = new OracleConnection();
                    break;

                case DatabaseType.ODBC:
                    connection = new OdbcConnection();
                    break;

                case DatabaseType.SQLite:
                    connection = new SQLiteConnection();
                    break;

                default:
                    connection = new OleDbConnection();
                    break;
            }
            if (String.IsNullOrEmpty(DbName))
            {
                DbName = db.DbName;
            }
            if ((DbName == null) || (DbName == ""))
            {
                connection.ConnectionString = ConnectionString;
                return connection;
            }

            connection.ConnectionString = GetConnString(DbName);
            return connection;
        }
        private static String GetConnString(String DbName)
        {
            switch (CurrDbType)
            {
                case DatabaseType.MSSQLSERVER:
                    return ("Data Source=" + IP + ";Initial Catalog=" + DbName + ";Integrated Security=True;Uid=" + UID + ";PWD=" + PWD);

                case DatabaseType.MYSQL:
                case DatabaseType.OLEDB:
                    return DbName;

                case DatabaseType.ACCESS:
                    return ("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DbName + ";Persist Security Info=True");

                case DatabaseType.ORACLE:
                    return DbName;

                case DatabaseType.ODBC:
                    return DbName;

                case DatabaseType.SQLite:
                    return DbName;
            }
            return DbName;
        }

        private static IDbDataAdapter GetDataAdapter()
        {
            switch (CurrDbType)
            {
                case DatabaseType.MSSQLSERVER:
                    return new SqlDataAdapter();

                case DatabaseType.ACCESS:
                case DatabaseType.OLEDB:
                    return new OleDbDataAdapter();

                case DatabaseType.ORACLE:
                    return new OracleDataAdapter();

                case DatabaseType.ODBC:
                    return new OdbcDataAdapter();
                case DatabaseType.SQLite:
                    return new SQLiteDataAdapter();
            }
            return new OleDbDataAdapter();
        }
        private static OleDbParameter GetOleDbParameter(String Name, Object value)
        {
            OleDbParameter parameter = new OleDbParameter();
            Type type = value.GetType();
            if (type == typeof(DateTime))
            {
                parameter.OleDbType = OleDbType.Date;
            }
            else if ((type == typeof(Boolean)) || (type == typeof(Boolean)))
            {
                parameter.OleDbType = OleDbType.Boolean;
            }
            else if ((type == typeof(int)) || (type == typeof(short)))
            {
                parameter.OleDbType = OleDbType.Integer;
            }
            else if (type == typeof(long))
            {
                parameter.OleDbType = OleDbType.BigInt;
            }
            else if (type == typeof(decimal))
            {
                parameter.OleDbType = OleDbType.VarNumeric;
            }
            else if ((type == typeof(float)) || (type == typeof(double)))
            {
                parameter.OleDbType = OleDbType.Double;
            }
            else if ((type == typeof(String)) || (type == typeof(String)))
            {
                parameter.OleDbType = OleDbType.VarWChar;
            }
            else
            {
                parameter.OleDbType = OleDbType.Binary;
            }
            parameter.ParameterName = "@" + Name;
            parameter.Value = value;
            return parameter;
        }
        private static IDataParameter GetParameter(String Name, Object value)
        {
            switch (CurrDbType)
            {
                case DatabaseType.SQLite:
                    return new SQLiteParameter("@" + Name, value);
                case DatabaseType.MSSQLSERVER:
                    return new SqlParameter("@" + Name, value);

                case DatabaseType.ACCESS:
                case DatabaseType.OLEDB:
                    return GetOleDbParameter(Name, value);

                case DatabaseType.ORACLE:
                    return new OracleParameter("@" + Name, value);

                case DatabaseType.ODBC:
                    return new OdbcParameter("@" + Name, value);
            }
            return new OleDbParameter("@" + Name, value);
        }

        public static DataTable GetTableInfo(String tablename)
        {
            if (TableItems.Contains(tablename.ToLower()))
            {
                return (DataTable)TableItems[tablename.ToLower()];
            }
            DataTable table = ExecuteDataSet(String.Format(ChkSql, tablename)).Tables[0];// ExecuteDataSet("SELECT TOP 1 * FROM [" + tablename + "]").Tables[0];
            TableItems.Add(tablename.ToLower(), table.Clone());
            return table;
        }
        public static DataTable GetTableInfo(String tablename, String DbName)
        {
            if (TableItems.Contains(tablename.ToLower()))
            {
                return (DataTable)TableItems[tablename.ToLower()];
            }
            DataTable table = ExecuteDataSet(String.Format(ChkSql, tablename), DbName).Tables[0];// ExecuteDataSet("SELECT TOP 1 * FROM [" + tablename + "]", DbName).Tables[0];
            TableItems.Add(tablename.ToLower(), table.Clone());
            return table;
        }

        public static String ConnectionString
        {
            get
            {
                return m_ConnectionString;
            }
            set
            {
                m_ConnectionString = value;
            }
        }
        public static DatabaseType CurrDbType
        {
            get
            {
                return m_dbType;
            }
            set
            {
                m_dbType = value;
            }
        }
        public static String DbName
        {
            get
            {
                return m_DbName;
            }
            set
            {
                m_DbName = value;
            }
        }
        public static String IP
        {
            get
            {
                return m_IP;
            }
            set
            {
                m_IP = value;
            }
        }
        public static String PWD
        {
            get
            {
                return m_PWD;
            }
            set
            {
                m_PWD = value;
            }
        }
        public static String UID
        {
            get
            {
                return m_UID;
            }
            set
            {
                m_UID = value;
            }
        }
        private static String ChkSql
        {
            get
            {
                switch (CurrDbType)
                {
                    case DatabaseType.SQLite:
                        return "SELECT * FROM [{0}] LIMIT 1";
                    default:
                        return "SELECT TOP 1 * FROM [{0}]";
                }
            }
        }
    }
}