using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSSQLDB;
using WebSite.Core;
using WebSite.Core.Table;
using WebSite.Models;
using WebSite.Plugins;

namespace WebSite.BackgroundPages
{
    /// <summary>
    /// 后台管理页面基类
    /// </summary>
    public class BaseAdmin : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            //if (Admin.UserData == null) Admin.SignOut();
            if (!Request.IsAuthenticated) Admin.SignOut();
            if (!IsCanView)
            {
                Response.Redirect("/developer/skin/error.html");
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                Page.ClientScript.GetPostBackEventReference(this, null);
                base.OnPreInit(e);
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            //LiteralControl include = new LiteralControl("<link href=\"/developer/images/css64.css\" rel=\"stylesheet\" type=\"text/css\" />");
            //switch (Request.Browser.MSDomVersion.Major)
            //{
            //    case 6:
            //    case 7:
            //        include = new LiteralControl("<link href=\"/developer/images/cssSprites.css\" rel=\"stylesheet\" type=\"text/css\" />");
            //        break;
            //}
            //Header.Controls.AddAt(0, include);

            base.OnPreRender(e);
        }
        /// <summary>
        /// 压缩输出HTML
        /// </summary>
        /// <param name="writer"></param>
        //protected override void Render(HtmlTextWriter writer)
        //{
        //    StringWriter sw = new StringWriter();
        //    base.Render(new HtmlTextWriter(sw));
        //    String html = sw.ToString();

        //    Hashtable ht = new Hashtable();
        //    MatchCollection matche = Regex.Matches(html, @"<textarea[\s\S]+?>(?<txt>[\s\S]*?)</textarea>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //    for (int i = 0; i < matche.Count; i++)
        //    {
        //        Match m = matche[i];
        //        String txt = m.Groups["txt"].Value;
        //        if (String.IsNullOrEmpty(txt)) continue;
        //        ht.Add(i, txt);
        //        html = html.Replace(txt, String.Format("</store-{0}/>", i));
        //    }
        //    html = Regex.Replace(html, @"\s+\s", "");
        //    html = Regex.Replace(html, @"\s+/", "/");
        //    html = Regex.Replace(html, @"""\s+", "\"");
        //    html = html.Replace("//<![CDATA[", "").Replace("//]]>", "");
        //    for (int i = 0; i < ht.Count; i++)
        //    {
        //        html = html.Replace(String.Format("</store-{0}/>", i), ht[i].ToString());
        //    }

        //    writer.Write(html);
        //}
        /// <summary>
        /// ViewState
        /// </summary>
        protected override PageStatePersister PageStatePersister
        {
            get
            {
                switch (SiteParameter.Config.ViewStateMode)
                {
                    case ViewStateType.File:
                        return new FilePageStatePersister(this);
                    case ViewStateType.SQLite:
                        return new SQLitePageStatePersister(this);
                    case ViewStateType.Session:
                        return new SessionPageStatePersister(this);
                    case ViewStateType.Compress:
                        return new CompressPageStatePersister(this);
                    default:
                        return new HiddenFieldPageStatePersister(this);
                }
            }
        }
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
        /// 记录日志
        /// </summary>
        /// <param name="text"></param>
        /// <param name="action"></param>
        protected void AppendLogs(String text, LogsAction action)
        {
            T_LogsHelper.Append(text, action, Admin.UserData);
        }

        #region 页面权限

        /// <summary>
        /// 获取页面权限
        /// </summary>
        private T_AccessControlEntity GetPageAction
        {
            get
            {
                T_AccessControlEntity acc = (T_AccessControlEntity)ViewState["pageaction"];
                if (acc == null)
                {
                    String url = Request.RawUrl.ToLower();
                    if (url.IndexOf("?") > 0) url = url.Substring(0, url.IndexOf("?"));

                    T_AccessControlEntity action = Admin.AccessControl.Find(a =>
                    {
                        return a.TableName == "t_sitemenu" && !String.IsNullOrEmpty(a.Link_Url) && url.EndsWith(a.Link_Url);
                    });
                    acc = action ?? new T_AccessControlEntity() { ActionType = ActionType.ALL };
                    ViewState["pageaction"] = acc;
                }
                return acc;
            }
        }
        /// <summary>
        /// 是否拥有页面编辑权限
        /// </summary>
        protected Boolean IsCanEdit
        {
            get
            {
                return ActionTypeHelper.IsEdit(GetPageAction.ActionType);// || Admin.IsSuper;
            }
        }
        /// <summary>
        /// 是否拥有页面添加权限
        /// </summary>
        protected Boolean IsCanCreat
        {
            get
            {
                return ActionTypeHelper.IsCreat(GetPageAction.ActionType);// || Admin.IsSuper;
            }
        }
        /// <summary>
        /// 是否拥有页面删除权限
        /// </summary>
        protected Boolean IsCanDelete
        {
            get
            {
                return ActionTypeHelper.IsDelete(GetPageAction.ActionType);// || Admin.IsSuper;
            }
        }
        /// <summary>
        /// 是否拥有页面设置权限
        /// </summary>
        protected Boolean IsCanSetting
        {
            get
            {
                return ActionTypeHelper.IsSetting(GetPageAction.ActionType);// || Admin.IsSuper;
            }
        }
        /// <summary>
        /// 是否拥有页面查看权限
        /// </summary>
        protected Boolean IsCanView
        {
            get
            {
                return ActionTypeHelper.IsView(GetPageAction.ActionType);// || Admin.IsSuper;
            }
        }

        #endregion
        /// <summary>
        /// 绑定表字段信息
        /// </summary>
        /// <param name="dropField">下拉控件</param>
        /// <param name="tableName">表名</param>
        /// <param name="defaultField">默认选中字段</param>
        protected void BindField(DropDownList dropField, String tableName, String defaultField)
        {
            var tab = SiteTable.Tables.Find(a => { return a.TableName == tableName; });
            foreach (var item in tab.Columns)
            {
                SqlDbType datatype = (SqlDbType)Enum.Parse(db.dbType, item.DataType.ToString(), true);
                switch (datatype)
                {
                    case SqlDbType.BigInt:
                    case SqlDbType.Binary:
                    case SqlDbType.Bit:
                    case SqlDbType.Image:
                    case SqlDbType.Money:
                    case SqlDbType.VarBinary:
                        break;
                    default:
                        String _name = String.IsNullOrEmpty(item.Description) ? item.FieldName : item.Description;
                        dropField.Items.Add(new ListItem(_name, item.FieldName));
                        break;
                }
            }
            dropField.SelectedValue = defaultField;
        }

        #region 提示功能
        /// <summary>
        /// js提示框
        /// </summary>
        /// <param name="txt"></param>
        protected void Alert(String txt)
        {
            Header.Controls.Add(new LiteralControl(String.Format("<script type=\"text/javascript\">alert('{0}');</script>", txt)));
        }
        /// <summary>
        /// js提示框之后刷新页面
        /// </summary>
        /// <param name="txt"></param>
        protected void Alert(String txt, bool refresh)
        {
            Header.Controls.Add(new LiteralControl(String.Format("<script type=\"text/javascript\">alert('{0}');location.href=location.href;</script>", txt)));
        }
        /// <summary>
        /// 自定义提示
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="txt"></param>
        /// <param name="css"></param>
        protected void Alert(Control ctl, String txt, String css)
        {
            if (ctl is HtmlContainerControl)
            {
                HtmlContainerControl c = ctl as HtmlContainerControl;
                c.InnerText = txt;
                c.Attributes.Add("class", css);
            }
            else if (ctl is ITextControl)
            {
                WebControl c = ctl as WebControl;
                c.CssClass = css;
                ITextControl itxt = ctl as ITextControl;
                itxt.Text = txt;
            }
        }
        /// <summary>
        /// 弹出消息提示
        /// </summary>
        /// <param name="text"></param>
        /// <param name="css"></param>
        protected void Alert(String text, String css)
        {
            LiteralControl js = new LiteralControl();
            js.Text = String.Format("<script type=\"text/javascript\">jQuery(function (){{dialogMessage('{0}','{1}');}})</script>", text, css);
            Header.Controls.Add(js);
        }
        #endregion
    }
}
