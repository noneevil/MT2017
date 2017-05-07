using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SQLite;
using MySql.Data.MySqlClient;

namespace MSSQLDB
{
    public class db
    {
        private static IDataAccess _Instance;
        /// <summary>
        /// 数据接口实例
        /// </summary>
        private static IDataAccess Instance
        {
            get
            {
                if (_Instance == null)
                {
                    switch (CurrDbType)
                    {
                        case DatabaseType.SqlServer:
                            if (!String.IsNullOrEmpty(DbName))
                            {
                                _Instance = new SqlServer(DbName, IP, UID, PWD);
                            }
                            else
                            {
                                _Instance = new SqlServer(ConnectionString);
                            }
                            break;
                        case DatabaseType.ACCESS:
                        case DatabaseType.OLEDB:
                            if (!String.IsNullOrEmpty(DbName))
                            {
                                _Instance = new ACCESS(DbName, PWD);
                            }
                            else
                            {
                                _Instance = new ACCESS(ConnectionString);
                            }
                            break;
                        case DatabaseType.SqLite:
                            if (!String.IsNullOrEmpty(DbName))
                            {
                                _Instance = new SQLite(DbName, PWD);
                            }
                            else
                            {
                                _Instance = new SQLite(ConnectionString);
                            }
                            break;
                        //case DatabaseType.ORACLE:
                        //    break;
                        //case DatabaseType.ODBC:
                        //    break;
                        //case DatabaseType.MYSQL:
                        //    break;
                        default:
                            throw new Exception("不支持的数据库类型！");
                    }
                }
                return _Instance;
            }
        }

        #region 参数

        private static DatabaseType m_dbType;
        private static String m_ConnectionString = "";
        private static String m_DbName = "";
        private static String m_IP = ".";
        private static String m_PWD = "";
        private static String m_UID = "";
        /// <summary>
        /// 数据表架构信息
        /// </summary>
        public static Hashtable TableSchema = new Hashtable(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 连接字符串
        /// </summary>
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
        /// <summary>
        /// 数据库类型
        /// </summary>
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
        /// <summary>
        /// 数据库名称
        /// </summary>
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
        /// <summary>
        /// 服务器IP地址
        /// </summary>
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
        /// <summary>
        /// 连接密码
        /// </summary>
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
        /// <summary>
        /// 连接用户
        /// </summary>
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

        #endregion

        #region 返回IDataReader
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>IDataReader</returns>
        public static IDataReader ExecuteReader(String SQL)
        {
            return Instance.ExecuteReader(SQL);
        }
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>IDataReader</returns>
        public static IDataReader ExecuteReader(ExecuteObject sender)
        {
            return Instance.ExecuteReader(sender);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>IDataReader</returns>
        public static IDataReader ExecuteReader(String proceName, Hashtable Parameters)
        {
            return Instance.ExecuteReader(proceName, Parameters);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <param name="outParameters">输出参数</param>
        /// <returns>IDataReader</returns>
        public static IDataReader ExecuteReader(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            return Instance.ExecuteReader(proceName, Parameters, out outParameters);
        }
        #endregion

        #region 返回Boolean
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>Boolean</returns>
        public static Boolean ExecuteCommand(String SQL)
        {
            return Instance.ExecuteCommand(SQL);
        }
        /// <summary>
        /// 执行插入或更新实体
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="cmdtype">INSERT 或 UPDATE</param>
        /// <returns>Boolean</returns>
        public static Boolean ExecuteCommand<T>(T entity, CmdType sender)
        {
            return Instance.ExecuteCommand<T>(entity, sender);
        }
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>Boolean</returns>
        public static Boolean ExecuteCommand(ExecuteObject sender)
        {
            return Instance.ExecuteCommand(sender);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>Boolean</returns>
        public static Boolean ExecuteCommand(String proceName, Hashtable Parameters)
        {
            return Instance.ExecuteCommand(proceName, Parameters);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <param name="outParameters">输出参数</param>
        /// <returns>Boolean</returns>
        public static Boolean ExecuteCommand(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            return Instance.ExecuteCommand(proceName, Parameters, out outParameters);
        }

        #endregion

        #region 返回DataSet
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(String SQL)
        {
            return Instance.ExecuteDataSet(SQL);
        }
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(ExecuteObject sender)
        {
            return Instance.ExecuteDataSet(sender);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(String proceName, Hashtable Parameters)
        {
            return Instance.ExecuteDataSet(proceName, Parameters);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <param name="outParameters">输出参数</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            return Instance.ExecuteDataSet(proceName, Parameters, out outParameters);
        }
        #endregion

        #region 返回DataTable

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>DataTable</returns>
        public static DataTable ExecuteDataTable(String SQL)
        {
            return Instance.ExecuteDataTable(SQL);
        }
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>DataTable</returns>
        public static DataTable ExecuteDataTable(ExecuteObject sender)
        {
            return Instance.ExecuteDataTable(sender);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>DataTable</returns>
        public static DataTable ExecuteDataTable(String proceName, Hashtable Parameters)
        {
            return Instance.ExecuteDataTable(proceName, Parameters);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <param name="outParameters">输出参数</param>
        /// <returns>DataTable</returns>
        public static DataTable ExecuteDataTable(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            return Instance.ExecuteDataTable(proceName, Parameters, out outParameters);
        }

        #endregion

        #region 返回Object

        /// <summary>
        /// 执行SQL语句返回单个字段
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>Object</returns>
        public static Object ExecuteScalar(String SQL)
        {
            return Instance.ExecuteScalar(SQL);
        }
        /// <summary>
        /// 执行ExecuteObject返回单个字段
        /// </summary>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>Object</returns>
        public static Object ExecuteScalar(ExecuteObject sender)
        {
            return Instance.ExecuteScalar(sender);
        }
        /// <summary>
        /// 执行存储过程返回单个字段
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>Object</returns>
        public static Object ExecuteScalar(String proceName, Hashtable Parameters)
        {
            return Instance.ExecuteScalar(proceName, Parameters);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <param name="outParameters">输出参数</param>
        /// <returns>Object</returns>
        public static Object ExecuteScalar(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            return Instance.ExecuteScalar(proceName, Parameters, out outParameters);
        }

        #endregion

        #region 返回泛型

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="SQL">SQL语句</param>
        /// <returns>T</returns>
        public static T ExecuteObject<T>(String SQL)
        {
            return Instance.ExecuteObject<T>(SQL);
        }
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>T</returns>
        public static T ExecuteObject<T>(ExecuteObject sender)
        {
            return Instance.ExecuteObject<T>(sender);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>T</returns>
        public static T ExecuteObject<T>(String proceName, Hashtable Parameters)
        {
            return Instance.ExecuteObject<T>(proceName, Parameters);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <param name="outParameters">输出参数</param>
        /// <returns>T</returns>
        public static T ExecuteObject<T>(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            return Instance.ExecuteObject<T>(proceName, Parameters, out outParameters);
        }

        #endregion

        #region 返回JSON

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>String</returns>
        public static String ExecuteJson(String SQL)
        {
            return Instance.ExecuteJson(SQL);
        }
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>String</returns>
        public static String ExecuteJson(ExecuteObject sender)
        {
            return Instance.ExecuteJson(sender);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>String</returns>
        public static String ExecuteJson(String proceName, Hashtable Parameters)
        {
            return Instance.ExecuteJson(proceName, Parameters);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>String</returns>
        public static String ExecuteJson(String proceName, Hashtable Parameters, out Hashtable outParameters)
        {
            return Instance.ExecuteJson(proceName, Parameters, out outParameters);
        }

        #endregion

        public static Type dbType
        {
            get
            {
                //return typeof(TypeCode);
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
}
