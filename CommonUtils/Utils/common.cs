using System;
using System.Data;
using System.Web;

namespace CommonUtils
{
    /// <summary>
    /// common 的摘要说明
    /// </summary>
    public class common
    {
        /// <summary>
        /// 表示最后一次出错的信息
        /// </summary>
        public static String LastErrorString = "";
        /// <summary>
        /// 表示当前数据库类型是否为Access数据库
        /// </summary>
        protected static Boolean _IsAccess = false;
        public static Boolean IsAccess
        {
            get { return _IsAccess; }
            set { _IsAccess = value; }
        }
        /// <summary>
        /// 登录验证码
        /// </summary>
        public static String ValidateCode
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session["HUTAO_WEB_CheckCode"]);
            }
            set
            {
                HttpContext.Current.Session["HUTAO_WEB_CheckCode"] = value.ToLower();
            }
        }
        protected static DataTable m_Contenttype = null;
        /// <summary>
        /// 返回指定文件名的ContentType字符串
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static String GetContentType(String fileName)
        {
            fileName = HttpContext.Current.Server.MapPath(fileName);
            String ext = System.IO.Path.GetExtension(fileName);
            if (m_Contenttype == null)
            {
                DataSet ds = new DataSet();
                ds.ReadXml(HttpContext.Current.Server.MapPath("~/App_Data/ContentType.xml"));
                m_Contenttype = ds.Tables[0];
            }
            if (m_Contenttype == null || m_Contenttype.Rows.Count == 0) return "";
            DataRow[] drs = m_Contenttype.Select(String.Format("name='{0}'", ext));
            if (drs.Length > 0) return Convert.ToString(drs[0]["value"]);
            return "";

        }
        /// <summary>
        /// 根据一个Boolean返回一个值用于显示或隐藏HTML标记
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String GetDivVisible(Object value)
        {
            Boolean b = Convert.ToBoolean(value);
            return b ? "block" : "none";
        }
        /// <summary>
        /// 读取客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static String GetClientIP()
        {
            String result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;
        }
        /// <summary>
        /// 返回当前地址栏指定参数值
        /// </summary>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        public static String GetQueryString(String fieldname)
        {
            return HttpContext.Current.Request.QueryString[fieldname];
        }
        /// <summary>
        /// 返回当前页面接收到的表单提交指定字段值
        /// </summary>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        public static String GetFormField(String fieldname)
        {
            return HttpContext.Current.Request[fieldname];
        }
    }
}
