using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Web.UI;

namespace WebSite.Plugins
{
    /// <summary>
    /// SQLite存储 VIEWSTATE
    /// </summary>
    public class SQLitePageStatePersister : PageStatePersister
    {
        private readonly String STATEKEY = "___VIEWSTATE";
        private const String ConnectionString = "Data Source=|DataDirectory|ASPState.db;Version=3;Cache Size=8000;Page Size=4096;Synchronous=Off;journal mode=Off;";

        public SQLitePageStatePersister(Page page) : base(page) { }

        public override void Load()
        {
            String viewstateid = Page.Request.Form[STATEKEY];
            if (!String.IsNullOrEmpty(viewstateid))
            {
                Byte[] buffer = GetStoreItem(viewstateid);

                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    BinaryFormatter format = new BinaryFormatter();
                    Pair data = (Pair)format.Deserialize(ms);
                    ViewState = data.First;
                    ControlState = data.Second;
                }
            }
        }

        public override void Save()
        {
            if (ViewState != null || ControlState != null)
            {
                Pair data = new Pair(ViewState, ControlState);

                Byte[] buffer = new Byte[8];
                using (RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider())
                {
                    rand.GetNonZeroBytes(buffer);
                }

                String postbackstate = Page.Request.Form[STATEKEY];
                String viewstateid = postbackstate ?? Page.Request.Url.GetHashCode().ToString("X2") + DateTime.Now.Ticks.ToString("X2") + String.Concat(buffer);

                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter format = new BinaryFormatter();
                    format.Serialize(ms, data);
                    CreateStoreItem(viewstateid, ms.ToArray());
                }

                Page.ClientScript.RegisterHiddenField(STATEKEY, viewstateid);
            }
        }

        #region SQLite

        /// <summary>
        /// 创建新会话
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        protected void CreateStoreItem(String id, Object value)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = "DELETE FROM [viewstate] WHERE Expires < @Expires";
                    cmd.Parameters.Add(CreateParameter("@Expires", DbType.DateTime, DateTime.Now));
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    cmd.CommandText = "SELECT COUNT(SessionId) FROM [ViewState] WHERE SessionId = @SessionId";
                    cmd.Parameters.Add(CreateParameter("@SessionId", DbType.String, 100, id));
                    Int32 reader = Convert.ToInt32(cmd.ExecuteScalar());

                    if (reader.Equals(0))
                    {
                        cmd.CommandText = "INSERT INTO [ViewState] (SessionId, Created, Expires, Timeout, SessionItems) Values(@SessionId, @Created, @Expires,@Timeout,@SessionItems)";
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE [ViewState] SET Created=@Created, Expires = @Expires, SessionItems = @SessionItems WHERE SessionId = @SessionId";
                    }

                    cmd.Parameters.Add(CreateParameter("@Created", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(CreateParameter("@Expires", DbType.DateTime, DateTime.Now.AddMinutes(30)));
                    cmd.Parameters.Add(CreateParameter("@Timeout", DbType.Int32, 30));
                    cmd.Parameters.Add(CreateParameter("@SessionItems", DbType.Binary, value));
                    cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// 获取会话
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected Byte[] GetStoreItem(String id)
        {
            Byte[] buffer;
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT SessionItems FROM [ViewState] WHERE SessionId = @SessionId";
                    cmd.Parameters.Add(SQLiteHelper.CreateParameter("@SessionId", DbType.String, 100, id));

                    buffer = (Byte[])cmd.ExecuteScalar();
                }
            }
            return buffer;
        }
        private SQLiteParameter CreateParameter(String name, DbType type, Object value)
        {
            return new SQLiteParameter
            {
                ParameterName = name,
                DbType = type,
                Value = value
            };
        }
        private SQLiteParameter CreateParameter(String name, DbType type, int size, Object value)
        {
            return new SQLiteParameter
            {
                ParameterName = name,
                DbType = type,
                Size = size,
                Value = value
            };
        }

        #endregion

        /// <summary>
        /// 清理过期VIEWSTATE会话
        /// </summary>
        public static void ClearExpiredData()
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
        /// 清空所有VIEWSTATE会话数据
        /// </summary>
        public static void ClearData()
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
