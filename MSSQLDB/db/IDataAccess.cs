using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MSSQLDB
{
    public interface IDataAccess
    {
        Dictionary<String, Type> GetTableSchema(String tablename);

        #region 返回IDataReader
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>IDataReader</returns>
        IDataReader ExecuteReader(String SQL);
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>IDataReader</returns>
        IDataReader ExecuteReader(ExecuteObject sender);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>IDataReader</returns>
        IDataReader ExecuteReader(String proceName, Hashtable Parameters);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <param name="outParameters">返回存储过程输出参数</param>
        /// <returns></returns>
        IDataReader ExecuteReader(String proceName, Hashtable Parameters, out Hashtable outParameters);

        #endregion

        #region 返回Boolean
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>Boolean</returns>
        Boolean ExecuteCommand(String SQL);
        /// <summary>
        /// 执行插入或更新实体
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="cmdtype">INSERT 或 UPDATE</param>
        /// <returns>Boolean</returns>
        Boolean ExecuteCommand<T>(T entity, CmdType cmdtype);
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>Boolean</returns>
        Boolean ExecuteCommand(ExecuteObject sender);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>Boolean</returns>
        Boolean ExecuteCommand(String proceName, Hashtable Parameters);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <param name="outParameters">返回存储过程输出参数</param>
        /// <returns></returns>
        Boolean ExecuteCommand(String proceName, Hashtable Parameters, out Hashtable outParameters);

        #endregion

        #region 返回DataSet
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>DataSet</returns>
        DataSet ExecuteDataSet(String SQL);
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>DataSet</returns>
        DataSet ExecuteDataSet(ExecuteObject sender);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>DataSet</returns>
        DataSet ExecuteDataSet(String proceName, Hashtable Parameters);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <param name="outParameters">返回存储过程输出参数</param>
        /// <returns>DataSet</returns>
        DataSet ExecuteDataSet(String proceName, Hashtable Parameters, out Hashtable outParameters);

        #endregion

        #region 返回DataTable
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>DataTable</returns>
        DataTable ExecuteDataTable(String SQL);
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>DataTable</returns>
        DataTable ExecuteDataTable(ExecuteObject sender);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>DataTable</returns>
        DataTable ExecuteDataTable(String proceName, Hashtable Parameters);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <param name="outParameters">返回存储过程输出参数</param>
        /// <returns>DataTable</returns>
        DataTable ExecuteDataTable(String proceName, Hashtable Parameters, out Hashtable outParameters);

        #endregion

        #region 返回Object
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>Object</returns>
        Object ExecuteScalar(String SQL);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>Object</returns>
        Object ExecuteScalar(String proceName, Hashtable Parameters);
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>DataTable</returns>
        Object ExecuteScalar(ExecuteObject sender);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <param name="outParameters">返回存储过程输出参数</param>
        /// <returns>Object</returns>
        Object ExecuteScalar(String proceName, Hashtable Parameters, out Hashtable outParameters);

        #endregion

        #region 返回泛型
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="SQL">SQL语句</param>
        /// <returns>T</returns>
        T ExecuteObject<T>(String SQL);
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>T</returns>
        T ExecuteObject<T>(ExecuteObject sender);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>T</returns>
        T ExecuteObject<T>(String proceName, Hashtable Parameters);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <param name="outParameters">返回存储过程输出参数</param>
        /// <returns>T</returns>
        T ExecuteObject<T>(String proceName, Hashtable Parameters, out Hashtable outParameters);

        #endregion

        #region 返回Json字符串
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="SQL">SQL语句</param>
        /// <returns>String</returns>
        String ExecuteJson(String SQL);
        /// <summary>
        /// 执行ExecuteObject
        /// </summary>
        /// <param name="sender">ExecuteObject</param>
        /// <returns>String</returns>
        String ExecuteJson(ExecuteObject sender);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <returns>String</returns>
        String ExecuteJson(String proceName, Hashtable Parameters);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="proceName">存储过程名称</param>
        /// <param name="Parameters">存储过程参数</param>
        /// <param name="outParameters">返回存储过程输出参数</param>
        /// <returns>String</returns>
        String ExecuteJson(String proceName, Hashtable Parameters, out Hashtable outParameters);

        #endregion

    }
}
