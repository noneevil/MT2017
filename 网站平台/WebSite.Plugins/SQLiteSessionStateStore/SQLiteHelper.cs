using System;
using System.Data;
using System.Data.SQLite;

namespace WebSite.Plugins
{
    class SQLiteHelper
    {
        public static IDbDataParameter CreateParameter(String name, DbType type, Object value)
        {
            return new SQLiteParameter
            {
                ParameterName = name,
                DbType = type,
                Value = value
            };
        }
        public static IDbDataParameter CreateParameter(String name, DbType type, int size, Object value)
        {
            return new SQLiteParameter
            {
                ParameterName = name,
                DbType = type,
                Size = size,
                Value = value
            };
        }
    }
}
