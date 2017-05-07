using System;
using System.Collections;

namespace MSSQLDB
{
    /// <summary>
    /// 封装数据
    /// </summary>
    [Serializable]
    public class ExecuteObject
    {
        /// <summary>
        /// 表名
        /// </summary>
        public String tableName = "";
        /// <summary>
        /// 命令类型
        /// </summary>
        public CmdType cmdtype = CmdType.SELECT;
        /// <summary>
        /// 关联条件
        /// </summary>
        public Hashtable terms = new Hashtable(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 数据集合
        /// </summary>
        public Hashtable cells = new Hashtable(StringComparer.OrdinalIgnoreCase);
    }
}
