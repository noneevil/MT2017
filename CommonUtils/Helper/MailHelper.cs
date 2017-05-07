using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace CommonUtils
{
    /// <summary>
    /// 邮件扩展类
    /// </summary>
    public class MailHelper
    {
        #region 构造函数

        public MailHelper()
        {
            IsBodyHtml = true;
            Priority = MailPriority.Normal;
            EnableSsl = false;
            Attachments = new List<String>();
        }

        public MailHelper(String smtpserver)
        {
            IsBodyHtml = true;
            SmtpServer = smtpserver;
            Priority = MailPriority.Normal;
            EnableSsl = false;
            Attachments = new List<String>();
        }

        public MailHelper(String smtpserver, String email, String username, String password)
        {
            IsBodyHtml = true;
            SmtpServer = smtpserver;
            FromEmail = email;
            UserName = username;
            Password = password;
            Priority = MailPriority.Normal;
            EnableSsl = false;
            Attachments = new List<String>();
        }

        #endregion

        #region 公用属性

        /// <summary>
        /// 邮件服务器
        /// </summary>
        public String SmtpServer { get; set; }
        /// <summary>
        /// 邮箱登录名
        /// </summary>
        public String UserName { get; set; }
        /// <summary>
        /// 邮箱登录密码
        /// </summary>
        public String Password { get; set; }
        /// <summary>
        /// 邮件标题
        /// </summary>
        public String Subject { get; set; }
        /// <summary>
        /// 表示邮件格式是否为HTML
        /// </summary>
        public Boolean IsBodyHtml { get; set; }
        /// <summary>
        /// 发件人邮件地址
        /// </summary>
        public String FromEmail { get; set; }
        /// <summary>
        /// 收件人邮件地址
        /// </summary>
        public String ToEmail { get; set; }
        /// <summary>
        /// 邮件内容
        /// </summary>
        public String Body { get; set; }
        /// <summary>
        /// 邮件附件文件名集合
        /// </summary>
        public List<String> Attachments { get; set; }
        /// <summary>
        /// 邮件级别
        /// </summary>
        public MailPriority Priority { get; set; }
        /// <summary>
        /// 启用SSL
        /// </summary>
        public Boolean EnableSsl { get; set; }

        #endregion

        #region  公用方法

        /// <summary>
        ///  发送邮件代码
        /// </summary>
        /// <returns></returns>
        public Boolean SendMail()
        {
            //服务器参数设置
            SmtpClient server = new SmtpClient();
            server.DeliveryMethod = SmtpDeliveryMethod.Network;
            server.Host = SmtpServer;
            server.EnableSsl = EnableSsl;
            server.Credentials = new NetworkCredential(UserName, Password);

            //设置收发人及发送内容参数         
            MailMessage mail = new MailMessage();
            mail.Sender = new MailAddress(FromEmail);
            mail.From = new MailAddress(FromEmail);
            mail.CC.Add(ToEmail);
            mail.Subject = Subject;
            mail.Body = Body;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            foreach (String s in Attachments)
            {
                mail.Attachments.Add(new Attachment(s));
            }
            server.Send(mail);
            return true;
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email">收件地址</param>
        /// <param name="title">邮件标题</param>
        /// <param name="body">邮件内容</param>
        /// <returns></returns>
        public Boolean SendMail(String email, String title, String body)
        {
            ToEmail = email;
            Subject = title;
            Body = body;
            return SendMail();
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email">收件地址</param>
        /// <param name="title">邮件标题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="attachment">附件文件地址列表</param>
        /// <returns></returns>
        public Boolean SendMail(String email, String title, String body, List<String> attachment)
        {
            ToEmail = email;
            Subject = title;
            Body = body;
            Attachments = attachment;
            return SendMail();
        }

        #endregion
    }
}
