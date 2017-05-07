using System;
using System.Data;
using System.Web;
using CommonUtils;
using MSSQLDB;

/// <summary>
/// WapSessionID 的摘要说明
/// </summary>
public class WapSession
{
    protected static String GetSessionID()
    {
        return String.IsNullOrEmpty(
        HttpContext.Current.Request.QueryString["sid"]) ?
             HttpContext.Current.Session.SessionID : HttpContext.Current.Request.QueryString["sid"];
    }

    /// <summary>
    /// 处理Session
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public static String GetSessionURI(String uri)
    {
        HttpContext curr = HttpContext.Current;
        String sid = HttpContext.Current.Session.SessionID;
        if (uri.ToLower().IndexOf("?sid=") <= 0
            && uri.ToLower().IndexOf("&sid=") <= 0)
        {
            return uri + (uri.IndexOf("?") > 0 ? "&" : "?")
                + "sid=" + curr.Request.QueryString["sid"];
        }
        else
        {
            return uri;
        }
    }

    /// <summary>
    /// 处理Session
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public static String GetSessionURI(String uri, String sid)
    {
        if (uri.ToLower().IndexOf("?sid=") <= 0
            && uri.ToLower().IndexOf("&sid=") <= 0)
        {
            return uri + (uri.IndexOf("?") > 0 ? "&" : "?")
                + "sid=" + sid;
        }
        else
        {
            return uri;
        }
    }

    /// <summary>
    /// 判断手机用户是否登录，根据页面URL中sid参数判断
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Boolean CheckUserIsLogin(String key)
    {
        if (String.IsNullOrEmpty(key)) return false;
        try
        {
            String s = EncryptHelper.DecryptString(key);
            if (!(s.IndexOf(":") > 0)) return false;
            String[] arrs = s.Split(':');
            if (arrs.Length == 2 && arrs[0].Length > 0 && arrs[1] == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 取得登录用户ID
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static String GetUserID(String key)
    {
        if (String.IsNullOrEmpty(key)) return "0";
        try
        {
            String s = EncryptHelper.DecryptString(key);
            if (!(s.IndexOf(":") > 0)) return "0";
            String[] arrs = s.Split(':');
            if (arrs.Length == 2 && arrs[0].Length > 0 && arrs[1] == "true")
            {
                return arrs[0];
            }
            else
            {
                return "0";
            }
        }
        catch
        {
            return "0";
        }
    }

    /// <summary>
    /// 读取Session数据
    /// </summary>
    /// <param name="sessionid"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Object GetSessionValue(String sessionid, String key, Type type)
    {
        ClearSessionValue();
        String sid = TextHelper.CheckSQL(sessionid);
        String _key = TextHelper.CheckSQL(key);
        DataTable dt = db.ExecuteDataTable(String.Format(
            "SELECT session_value FROM [T_session] WHERE sessionid ='{0}' AND session_key = '{1}'", sid, _key));
        if (dt != null && dt.Rows.Count > 0)
        {
            db.ExecuteCommand(String.Format(
            "UPDATE [T_session] SET session_time = '{1}' WHERE sessionid ='{0}'",
            sid, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            if (dt.Rows[0][0] == DBNull.Value)
                return null;
            else
            {
                if (type == typeof(DataTable))
                {
                    String arrs = Convert.ToString(dt.Rows[0][0]);
                    return ConvertHelper.StringToObject(arrs, type);
                }
                else
                {
                    return dt.Rows[0][0];
                }
            }
        }
        else
        {
            return null;
        }
    }

    public static Object GetSessionValue(String sessionid, String key)
    {
        return GetSessionValue(sessionid, key, null);
    }

    public static Object GetSessionValue(String key)
    {
        return GetSessionValue(GetSessionID(), key);
    }

    public static Object GetSessionValue(String key, Type type)
    {
        return GetSessionValue(GetSessionID(), key, type);
    }

    /// <summary>
    /// 设置Session数据
    /// </summary>
    /// <param name="sessionid"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Boolean SetSessionValue(String sessionid, String key, Object value)
    {
        String sid = TextHelper.CheckSQL(sessionid);
        String _key = TextHelper.CheckSQL(key);
        String num = (String)db.ExecuteScalar(String.Format(
            "SELECT COUNT(id) FROM [T_session] WHERE sessionid ='{0}' AND session_key = '{1}'", sid, _key));
        ExecuteObject obj = new ExecuteObject();
        obj.tableName = "T_session";
        if (num == "0")
        {
            obj.cells.Add("sessionid", sessionid);
            obj.cells.Add("session_key", key);
            obj.cells.Add("pub_date", DateTime.Now);
            obj.cmdtype = CmdType.INSERT;
        }
        else
        {
            obj.terms.Add("sessionid", sessionid);
            obj.terms.Add("session_key", key);
            obj.cmdtype = CmdType.UPDATE;
        }
        if (value is DataTable)
        {
            DataTable dt = ((DataTable)value);
            dt.TableName = key;
            obj.cells.Add("session_value", ConvertHelper.ObjectToString(dt));
        }
        else
        {
            obj.cells.Add("session_value", value);
        }
        if (value == null)
        {
            obj.cmdtype = CmdType.DELETE;
            obj.cells.Clear();
        }
        return db.ExecuteCommand(obj);
    }

    public static Boolean SetSessionValue(String key, Object value)
    {
        return SetSessionValue(GetSessionID(), key, value);
    }

    /// <summary>
    /// 清除超时Session数据
    /// </summary>
    public static void ClearSessionValue()
    {
        int timeout = 30;
        if (HttpContext.Current != null)
            timeout = HttpContext.Current.Session.Timeout;
        DateTime tm = DateTime.Now.AddMinutes(-timeout);
        db.ExecuteCommand(String.Format("DELETE  [T_session] WHERE session_time < '{0}'", tm.ToString("yyyy-MM-dd HH:mm:ss")));
    }

}
