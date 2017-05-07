using System;
using System.Web;
using System.Web.SessionState;

namespace CommonUtils
{
    /// <summary>
    /// Session操作扩展类
    /// </summary>
    public class SessionHelper : IRequiresSessionState
    {
        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static Object GetSession(String strSessionName)
        {
            if (HttpContext.Current.Session == null) return null;
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Session[strSessionName];
            }
        }
        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static String GetSessionString(String strSessionName)
        {
            if (HttpContext.Current.Session == null) return null;
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            else
            {
                return Convert.ToString(HttpContext.Current.Session[strSessionName]);
            }
        }
        /// <summary>
        /// 设置Session值
        /// </summary>
        /// <param name="strSessionName"></param>
        /// <param name="value"></param>
        public static void SetSession(String strSessionName, Object value)
        {
            HttpContext.Current.Session[strSessionName] = value;
        }
        /// <summary>
        /// 删除某个Session对象
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        public static void Delete(String strSessionName)
        {
            HttpContext.Current.Session[strSessionName] = null;
        }
    }
}