using System;
using System.Web;
using System.Web.Security;
using CommonUtils;
using MSSQLDB;
using Newtonsoft.Json;
using WebSite.Core;
using WebSite.Interface;
using WebSite.Models;

namespace WebSite.UserCenter
{
    /// <summary>
    /// 会员中心辅助类
    /// </summary>
    public class MemberCenter
    {
        /// <summary>
        /// 检查会员名是否注册
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static Boolean CheckUser(String username)
        {
            ExecuteObject obj = new ExecuteObject();
            obj.tableName = "T_Members";
            obj.cells.Add("id", null);
            obj.terms.Add("MemberName", username);
            Int32 id = Convert.ToInt32(db.ExecuteScalar(obj));
            return id == 0;
        }
        /// <summary>
        /// 检查注册邮箱
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        public static Boolean CheckEmail(String mail)
        {
            ExecuteObject obj = new ExecuteObject();
            obj.tableName = "T_Members";
            obj.cells.Add("id", null);
            obj.terms.Add("Email", mail);
            Int32 id = Convert.ToInt32(db.ExecuteScalar(obj));
            return id == 0;
        }
        /// <summary>
        /// 检查注册手机
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static Boolean CheckMobile(String mobile)
        {
            ExecuteObject obj = new ExecuteObject();
            obj.tableName = "T_Members";
            obj.cells.Add("id", null);
            obj.terms.Add("Mobile", mobile);
            Int32 id = Convert.ToInt32(db.ExecuteScalar(obj));
            return id == 0;
        }
        /// <summary>
        /// 检查注册坐机
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static Boolean CheckPhone(String phone)
        {
            ExecuteObject obj = new ExecuteObject();
            obj.tableName = "T_Members";
            obj.cells.Add("id", null);
            obj.terms.Add("Phone", phone);
            Int32 id = Convert.ToInt32(db.ExecuteScalar(obj));
            return id == 0;
        }
        /// <summary>
        /// 检查注册QQ号
        /// </summary>
        /// <param name="qq"></param>
        /// <returns></returns>
        public static Boolean CheckQQ(String qq)
        {
            ExecuteObject obj = new ExecuteObject();
            obj.tableName = "T_Members";
            obj.cells.Add("id", null);
            obj.terms.Add("QQ", qq);
            Int32 id = Convert.ToInt32(db.ExecuteScalar(obj));
            return id == 0;
        }
        /// <summary>
        /// 检查注册身份证
        /// </summary>
        /// <param name="idcard"></param>
        /// <returns></returns>
        public static Boolean CheckIDCard(String idcard)
        {
            ExecuteObject obj = new ExecuteObject();
            obj.tableName = "T_Members";
            obj.cells.Add("id", null);
            obj.terms.Add("IDCard", idcard);
            Int32 id = Convert.ToInt32(db.ExecuteScalar(obj));
            return id == 0;
        }

        /// <summary>
        /// 会员登录信息
        /// </summary>
        public static T_MembersEntity Member
        {
            get
            {
                Object obj = HttpContext.Current.Session["Member_UserData"];

                if (obj == null)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["member"];
                    if (cookie == null || String.IsNullOrEmpty(cookie.Value)) return null;
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    if (ticket.Expired) return null;

                    Int32 ID = Convert.ToInt32(ticket.UserData);
                    ExecuteObject _obj = new ExecuteObject();
                    _obj.tableName = "T_Members";
                    _obj.cmdtype = CmdType.SELECT;
                    _obj.terms.Add("ID", ID);
                    T_MembersEntity data = db.ExecuteObject<T_MembersEntity>(_obj);

                    HttpContext.Current.Session["Member_UserData"] = data;
                    return data;
                }
                return (T_MembersEntity)obj;
            }
            set
            {
                HttpContext.Current.Session["Member_UserData"] = value;
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "member", DateTime.Now, DateTime.Now.AddMinutes(30), false, value.ID.ToString());
                String authTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie("member", authTicket);
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }
        /// <summary>
        /// 会员登录
        /// </summary>
        /// <param name="membername"></param>
        /// <param name="password"></param>
        public static String Login(String membername, String password)
        {
            IJsonResult msg = new IJsonResult { Status = false, Text = "用户名或密码输入错误！" };

            ExecuteObject obj = new ExecuteObject();
            obj.tableName = "T_Members";
            obj.cmdtype = CmdType.SELECT;
            obj.terms.Add("MemberName", membername);
            obj.terms.Add("PassWord", EncryptHelper.MD5Upper32(password));

            T_MembersEntity data = db.ExecuteObject<T_MembersEntity>(obj);
            if (!String.IsNullOrEmpty(data.MemberName))
            {
                if (!data.Status)
                {
                    msg.Text = "当前账号无法登录，请与管理员联系！";
                    T_LogsHelper.Append("会员:" + data.MemberName + "尝试登录会员中心,登录失败!", data.ID);
                }
                else
                {
                    msg.Text = null;
                    msg.Status = true;

                    data.LastSignTime = DateTime.Now;
                    db.ExecuteCommand<T_MembersEntity>(data, CmdType.UPDATE);
                    T_LogsHelper.Append("会员:" + data.MemberName + "成功登录会员中心!", data.ID);
                    Member = data;
                }
            }
            return JsonConvert.SerializeObject(msg);
        }
        /// <summary>
        /// 会员注册
        /// </summary>
        /// <param name="data"></param>
        public static IJsonResult RegMember(T_MembersEntity data)
        {
            IJsonResult msg = new IJsonResult { Status = false, Text = "", Css = "line1px_2" };
            if (String.IsNullOrEmpty(data.MemberName))
            {
                msg.Text = "登录名称不能为空!！";
            }
            else if (!MemberCenter.CheckUser(data.MemberName))
            {
                msg.Text = "会员名称已注册！";
            }
            else if (!String.IsNullOrEmpty(data.Email) && !MemberCenter.CheckEmail(data.Email))
            {
                msg.Text = "邮箱已注册！";
            }
            else if (!String.IsNullOrEmpty(data.QQ) && !MemberCenter.CheckQQ(data.QQ))
            {
                msg.Text = "QQ号已注册！";
            }
            else
            {
                data.JoinDate = DateTime.Now;
                data.LastSignTime = DateTime.Now;
                if (db.ExecuteCommand<T_MembersEntity>(data, CmdType.INSERT))
                {
                    msg.Status = true;
                    msg.Css = "line1px_3";
                    msg.Text = "注册成功！";
                }
            }
            return msg;
        }
        /// <summary>
        /// 退出
        /// </summary>
        public static void SignOut()
        {
            CookieHelper.Delete("member");
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }
    }
}
