using System;
using CommonUtils;
using MSSQLDB;
using WebSite.Models;

namespace WebSite.Core
{
    public class T_LogsHelper
    {
        /// <summary>
        /// 追加日志
        /// </summary>
        /// <param name="content">日志内容</param>
        /// <param name="UserID">管理员编号</param>
        public static void Append(String content, Int32 UserID)
        {
            T_LogsEntity log = new T_LogsEntity();
            log.UserID = UserID;
            log.Content = content;
            log.PubDate = DateTime.Now;
            log.ActionType = 0;

            db.ExecuteCommand<T_LogsEntity>(log, CmdType.INSERT);
        }
        /// <summary>
        /// 添加日志记录
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="action">操作类型</param>
        /// <param name="user">用户</param>
        public static void Append(String content, LogsAction action, T_UserEntity user)
        {
            if (SiteParameter.Config.RecordLog)
            {
                T_LogsEntity log = new T_LogsEntity();
                log.UserID = user.ID;
                log.UserName = user.UserName;
                log.Content = content;
                log.PubDate = DateTime.Now;
                log.ActionType = action;
                log.UserIP = Utils.GetIp();

                db.ExecuteCommand<T_LogsEntity>(log, CmdType.INSERT);
            }
        }
    }
}
