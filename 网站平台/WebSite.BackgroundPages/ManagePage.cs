using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using MSSQLDB;
using WebSite.Core;
using WebSite.Plugins;

namespace WebSite.BackgroundPages
{
    public class ManagePage : System.Web.UI.Page
    {
        public ManagePage()
        {

        }

        #region ViewState

        protected override PageStatePersister PageStatePersister
        {
            get
            {
                //switch (SiteParameter.Config.ViewStateMode)
                //{
                    //case ViewStateModes.File:
                    //    return new FilePageStatePersister(this);
                    //case ViewStateModes.SQLite:
                        return new SQLitePageStatePersister(this);
                    //case ViewStateModes.Session:
                    //    return new SessionPageStatePersister(this);
                    //case ViewStateModes.Compress:
                    //    return new CompressPageStatePersister(this);
                    //default:
                    //    return new HiddenFieldPageStatePersister(this);
                //}
            }
        }

        #endregion


        /// <summary>
        /// 是否是编辑模式
        /// </summary>
        protected virtual Boolean IsEdit
        {
            get { return EditID > 0; }
        }
        /// <summary>
        /// 编辑ID
        /// </summary>
        protected virtual Int32 EditID
        {
            get
            {
                Int32 outid = 0;
                Int32.TryParse(Request["id"], out outid);
                return outid;
            }
        }


        /// <summary>
        /// 写入管理日志
        /// </summary>
        /// <param name="action_type"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool AddAdminLog(string action_type, string remark)
        {
            //if (siteConfig.logstatus > 0)
            //{
            ExecuteObject obj = new ExecuteObject();
            obj.cmdtype = CmdType.INSERT;
            obj.tableName = "T_Manager_log";
            obj.cells.Add("user_id", null);
            obj.cells.Add("user_name", null);
            obj.cells.Add("action_type", action_type);
            obj.cells.Add("remark", remark);
            obj.cells.Add("user_ip", null);
            obj.cells.Add("add_time", DateTime.Now);
            if (db.ExecuteCommand(obj))
            {
                return true;
            }
            //}
            return false;
        }

        #region JS提示============================================
        /// <summary>
        /// 添加编辑删除提示
        /// </summary>
        /// <param name="msgtitle">提示文字</param>
        /// <param name="url">返回地址</param>
        /// <param name="msgcss">CSS样式</param>
        protected void JscriptMsg(string msgtitle, string url, string msgcss)
        {
            string msbox = "parent.jsprint(\"" + msgtitle + "\", \"" + url + "\", \"" + msgcss + "\")";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "JsPrint", msbox, true);
        }
        /// <summary>
        /// 带回传函数的添加编辑删除提示
        /// </summary>
        /// <param name="msgtitle">提示文字</param>
        /// <param name="url">返回地址</param>
        /// <param name="msgcss">CSS样式</param>
        /// <param name="callback">JS回调函数</param>
        protected void JscriptMsg(string msgtitle, string url, string msgcss, string callback)
        {
            string msbox = "parent.jsprint(\"" + msgtitle + "\", \"" + url + "\", \"" + msgcss + "\", " + callback + ")";
            ClientScript.RegisterClientScriptBlock(Page.GetType(), "JsPrint", msbox, true);
        }
        #endregion
    }
}
