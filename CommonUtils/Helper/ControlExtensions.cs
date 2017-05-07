using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using FastReflectionLib;
using MSSQLDB;
using System.Web.UI.WebControls;
using WebSite.Interface;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Data;

namespace CommonUtils
{
    /// <summary>
    /// 控件扩展方法
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// 设置控件属性值
        /// </summary>
        /// <param name="control"></param>
        /// <param name="context"></param>
        public static void SetPropertyValues(this Control control, HttpContext context)
        {
            Type type = control.GetType();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (PropertyInfo item in properties)
            {
                RequestFieldAttribute[] attribute = (RequestFieldAttribute[])item.GetCustomAttributes(typeof(RequestFieldAttribute), false);
                if (attribute != null && attribute.Length > 0)
                {
                    String val = context.Request.Params[attribute[0].Name];
                    Object value = val ?? attribute[0].DefaultValue;

                    if (value != null)
                    {
                        item.FastSetValue(control, Convert.ChangeType(value, item.PropertyType));
                    }
                }
            }
        }
        /// <summary>
        /// 将模型数据还原到表单
        /// </summary>
        /// <param name="control"></param>
        /// <param name="data"></param>
        public static void SetFormValue(this Control control, Object data)
        {
            Type type = data.GetType();
            Page page = HttpContext.Current.CurrentHandler as Page;

            foreach (PropertyInfo item in type.GetProperties())
            {
                if (!item.CanRead) continue;
                String name = item.Name;
                FieldAttribute[] attributes = (FieldAttribute[])item.GetCustomAttributes(typeof(FieldAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    FieldAttribute att = attributes[0];
                    if (!String.IsNullOrEmpty(att.ControlName)) name = att.ControlName;
                }
                Control ctl = page.FindControl(name);
                if (ctl == null) continue;
                /*--Object value = item.GetValue(data, null);*/
                Object value = item.FastGetValue(data);
                ControlHelper.SetControlValue(ctl, value);
            }
        }
        /// <summary>
        /// 将表单数据转换为模型(编辑模式)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static T GetFormValue<T>(this Control control, T instance)
        {
            Type type = typeof(T);
            Page page = HttpContext.Current.CurrentHandler as Page;
            //instance = instance ?? ControlHelper.CreateInstance(type);
            if (instance == null) instance = (T)ControlHelper.CreateInstance(type);

            foreach (PropertyInfo item in type.GetProperties())
            {
                if (!item.CanWrite) continue;
                String name = item.Name;
                FieldAttribute[] attributes = (FieldAttribute[])item.GetCustomAttributes(typeof(FieldAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    FieldAttribute att = attributes[0];
                    if (!String.IsNullOrEmpty(att.ControlName)) name = att.ControlName;
                }

                Control ctl = page.FindControl(name);
                if (ctl == null) continue;

                Object value = ControlHelper.GetControlValue(ctl);
                ControlHelper.ConvertSetValue(instance, item, value);
            }
            return instance;
        }
        /// <summary>
        /// 清空表单数据
        /// </summary>
        /// <param name="ctl"></param>
        public static void ClearFromValue(this Control ctl)
        {
            if (ctl is IAttributeAccessor)
            {
                String val = ((IAttributeAccessor)ctl).GetAttribute("clear");
                if (!String.IsNullOrEmpty(val) & !Convert.ToBoolean(val)) return;
            }
            if (ctl is WebControl)
            {
                if (!((WebControl)ctl).Enabled) return;
            }

            if (ctl is IControls)
            {
                ((IControls)ctl).SetValue(null);
            }
            else if (ctl is System.Web.UI.WebControls.TextBox)
            {
                ((System.Web.UI.WebControls.TextBox)ctl).Text = "";
            }
            else if (ctl is HtmlInputText)
            {
                ((HtmlInputText)ctl).Value = "";
            }
            else if (ctl is HtmlTextArea)
            {
                ((HtmlTextArea)ctl).Value = "";
            }

            if (ctl.HasControls())
            {
                foreach (Control c in ctl.Controls)
                {
                    ClearFromValue(c);
                }
            }
        }

        /// <summary>
        /// 美化列表样式
        /// </summary>
        /// <param name="Item"></param>
        public static void SetStyleLayer(this RepeaterItem item, Int32 layer)
        {
            //DataRowView data = item.DataItem as DataRowView;

            #region 美化

            //Int32 layer = Convert.ToInt32(data["layer"].ToString());
            Literal LitFirst = (Literal)item.FindControl("LitLayer");

            String LitStyle = "<span style=\"display:inline-block;width:{0}px;\"></span>{1}{2}";
            String LitImg1 = "<span class=\"folder-open\"></span>";
            String LitImg2 = "<span class=\"folder-line\"></span>";

            if (layer == 1)
            {
                LitFirst.Text = LitImg1;
            }
            else
            {
                LitFirst.Text = String.Format(LitStyle, (layer - 1) * 24, LitImg2, LitImg1);
            }

            #endregion
        }

        /// <summary>
        /// 注册调用服务器端脚本
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static String CallPostBack(this Control ctl, String cmd)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            return page.ClientScript.GetPostBackEventReference(ctl, cmd);
        }
        /// <summary>
        /// 分页绑定
        /// </summary>
        /// <param name="rpt">绑定控件</param>
        /// <param name="pager">分页控件</param>
        /// <param name="dt">数据源</param>
        /// <param name="RecordCount">总记录数</param>
        public static void BindPage(this Repeater rpt, WebSite.Controls.AspNetPager pager, IEnumerable data, Int32 RecordCount)
        {
            //pager.PageSize = PageSize;
            pager.RecordCount = RecordCount;

            PagedDataSource ps = new PagedDataSource();
            ps.AllowPaging = true;
            ps.PageSize = pager.PageSize;
            ps.CurrentPageIndex = pager.CurrentPageIndex - 1;
            ps.DataSource = data;

            rpt.DataSource = ps;
            rpt.DataBind();
        }
        public static void BindPage<T>(this Repeater rpt, WebSite.Controls.AspNetPager pager, List<T> data)
        {
            //pager.PageSize = PageSize;
            pager.RecordCount = data.Count;

            PagedDataSource ps = new PagedDataSource();
            ps.AllowPaging = true;
            ps.PageSize = pager.PageSize;
            ps.CurrentPageIndex = pager.CurrentPageIndex - 1;
            ps.DataSource = data;

            rpt.DataSource = ps;
            rpt.DataBind();
        }
    }
}
