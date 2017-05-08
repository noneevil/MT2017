using System;
using System.Data;
using System.Data.SQLite;

namespace WebSite.Plugins
{
    public class ViewStateStore
    {
        private const String ConnectionString = "Data Source=|DataDirectory|ASPState.db;Version=3;Cache Size=8000;Page Size=4096;Synchronous=Off;journal mode=Off;";

        /// <summary>
        /// 创建新会话
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void CreateStoreItem(String id, String value)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(SessionId) FROM [ViewState] WHERE SessionId = @SessionId";
                    cmd.Parameters.Add(SQLiteHelper.CreateParameter("@SessionId", DbType.String, 100, id));
                    Int32 reader = Convert.ToInt32(cmd.ExecuteScalar());

                    if (reader.Equals(0))
                    {
                        cmd.CommandText = "INSERT INTO [ViewState] (SessionId, Created, Expires, Timeout, SessionItems) Values(@SessionId, @Created, @Expires,@Timeout,@SessionItems)";
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE [ViewState] SET Created=@Created, Expires = @Expires, SessionItems = @SessionItems WHERE SessionId = @SessionId";
                    }

                    cmd.Parameters.Add(SQLiteHelper.CreateParameter("@Created", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(SQLiteHelper.CreateParameter("@Expires", DbType.DateTime, DateTime.Now.AddMinutes(30)));
                    cmd.Parameters.Add(SQLiteHelper.CreateParameter("@Timeout", DbType.Int32, 30));
                    cmd.Parameters.Add(SQLiteHelper.CreateParameter("@SessionItems", DbType.String, value.Length, value));
                    cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// 获取会话
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public String GetStoreItem(String id)
        {
            String SessionItems = String.Empty;
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT SessionItems FROM [ViewState] WHERE SessionId = @SessionId";
                    cmd.Parameters.Add(SQLiteHelper.CreateParameter("@SessionId", DbType.String, 100, id));

                    SessionItems = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            return SessionItems;
        }
        /// <summary>
        /// 清理过期会话
        /// </summary>
        public static void CleanUpExpiredData()
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "DELETE FROM [viewstate] WHERE Expires < @Expires";
                    cmd.Parameters.Add(SQLiteHelper.CreateParameter("@Expires", DbType.DateTime, DateTime.Now));
                    cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// 清空会话表数据
        /// </summary>
        public static void EmptyTable()
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                String sql = "DELETE FROM [ViewState];";
                sql += "DELETE FROM [Sessions];";
                sql += "VACUUM";
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
