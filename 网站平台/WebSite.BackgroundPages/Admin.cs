using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using CommonUtils;
using MSSQLDB;
using WebSite.Core;
using WebSite.Interface;
using WebSite.Models;

namespace WebSite.BackgroundPages
{
    /// <summary>
    /// 后台用户及权限管理类
    /// </summary>
    public class Admin
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">登录密码</param>
        /// <returns></returns>
        public static void LoginUser(String username, String password, ref IJsonResult result)
        {
            ExecuteObject obj = new ExecuteObject();
            obj.tableName = "T_User";
            obj.cmdtype = CmdType.SELECT;
            obj.terms.Add("UserName", username);
            obj.terms.Add("UserPass", password);

            T_UserEntity data = db.ExecuteObject<T_UserEntity>(obj);
            if (data.ID > 0)
            {
                if (data.IsLock)
                {
                    result.Text = "当前账号无法登录，请与管理员联系！";
                    T_LogsHelper.Append("尝试登录管理系统.", LogsAction.Login, data);
                }
                else
                {
                    result.Status = true;
                    data.LastSignTime = DateTime.Now;
                    db.ExecuteCommand<T_UserEntity>(data, CmdType.UPDATE);
                    T_LogsHelper.Append("成功登录管理系统.", LogsAction.Login, data);
                    SetUserData(data);

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "root", DateTime.Now, DateTime.Now.AddMinutes(60), false, data.ID.ToString());
                    String authticket = FormsAuthentication.Encrypt(ticket);

                    CookieHelper.AddCookie(FormsAuthentication.FormsCookieName, authticket);

                    //HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, authticket);
                    //cookie.Expires = DateTime.Now.AddMinutes(60);
                    //cookie.HttpOnly = false;
                    //cookie.Path = FormsAuthentication.FormsCookiePath;
                    //cookie.Secure = FormsAuthentication.RequireSSL;
                    //cookie.Domain = FormsAuthentication.CookieDomain;
                    //HttpContext.Current.Response.Cookies.Set(cookie);
                    //HttpContext.Current.Response.Redirect("/Developer", true);
                }
            }
        }
        /// <summary>
        /// 设置扩展信息
        /// </summary>
        /// <param name="data"></param>
        private static void SetUserData(T_UserEntity data)
        {
            String sql = String.Format("SELECT * FROM [T_UserRole] WHERE id={0}", data.RoleID);
            data.UserRole = db.ExecuteObject<T_UserRoleEntity>(sql);

            //sql = String.Format("SELECT * FROM [T_AccessControl] WHERE role={0}", data.RoleID);
            //data.AccessControl = db.ExecuteObject<List<T_AccessControlEntity>>(sql);

            HttpContext.Current.Session[ISessionKeys.session_admin] = data;
        }
        /// <summary>
        /// 更新用户信息
        /// </summary>
        public static void Update()
        {
            T_UserEntity data = db.ExecuteObject<T_UserEntity>("SELECT * FROM [T_User] WHERE id=" + Admin.ID);
            SetUserData(data);
        }
        /// <summary>
        /// 用户数据
        /// </summary>
        public static T_UserEntity UserData
        {
            get
            {
                if (!HttpContext.Current.Request.IsAuthenticated) return null;
                Object obj = HttpContext.Current.Session[ISessionKeys.session_admin];
                if (obj == null)
                {
                    HttpCookie cookdata = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    if (cookdata == null || String.IsNullOrEmpty(cookdata.Value)) return null;
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookdata.Value);
                    if (ticket.Expired) return null;

                    Int32 ID = Convert.ToInt32(ticket.UserData);

                    ExecuteObject _obj = new ExecuteObject();
                    _obj.tableName = "T_User";
                    _obj.cmdtype = CmdType.SELECT;
                    _obj.terms.Add("ID", ID);

                    T_UserEntity data = db.ExecuteObject<T_UserEntity>(_obj);

                    SetUserData(data);
                    return data;
                }
                return (T_UserEntity)obj;
            }
        }
        /// <summary>
        /// 管理员ID
        /// </summary>
        public static Int32 ID
        {
            get { return UserData.ID; }
        }
        /// <summary>
        /// 管理员名称
        /// </summary>
        public static String UserName
        {
            get { return UserData.UserName; }
        }
        /// <summary>
        /// 用户角色
        /// </summary>
        public static String RoleName
        {
            get { return UserData.UserRole.Name; }
            set { UserData.UserRole.Name = value; }
        }
        /// <summary>
        /// 角色ID
        /// </summary>
        public static Int32 RoleID
        {
            get { return UserData.RoleID; }
            set { UserData.RoleID = value; }
        }
        /// <summary>
        /// 登录密码
        /// </summary>
        public static String Password
        {
            get { return UserData.UserPass; }
            set { UserData.UserPass = value; }
        }
        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public static Boolean IsSuper
        {
            get
            {
                return UserData.IsSuper; //false;
            }
        }
        /// <summary>
        /// 权限控制
        /// </summary>
        public static List<T_AccessControlEntity> AccessControl
        {
            get
            {
                return T_AccessControlHelper.GetRole(Admin.RoleID);
                //return UserData.AccessControl;
            }
            //set
            //{
            //    UserData.AccessControl = value;
            //}
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        public static void SignOut()
        {
            FormsAuthentication.SignOut();//清除验证信息]
            HttpContext.Current.Session.Clear();//清除Session内容
            HttpContext.Current.Session.Abandon();//取消当前会话
            HttpContext.Current.Response.Redirect("/developer/login.aspx", true);
        }
    }
}