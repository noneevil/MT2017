using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

[assembly: WebResource("WebSite.Controls.Repeater.Repeater.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("WebSite.Controls.Repeater.Repeater.js", "text/javascript", PerformSubstitution = true)]
[assembly: WebResource("WebSite.Controls.Repeater.Repeater_1.gif", "image/gif")]
[assembly: WebResource("WebSite.Controls.Repeater.Repeater_2.gif", "image/gif")]
[assembly: WebResource("WebSite.Controls.Repeater.Repeater_3.gif", "image/gif")]
namespace WebSite.Controls
{
    public class Repeater : System.Web.UI.WebControls.Repeater
    {
        #region 数据绑定初始化

        protected override void OnInit(EventArgs e)
        {
            if (!DesignMode)
            {
                if (Page.FindControl("repeatercss") == null)
                {
                    HtmlGenericControl css = new HtmlGenericControl("link");
                    css.ID = "repeatercss";
                    css.Attributes.Add("href", this.Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.Repeater.Repeater.css"));
                    css.Attributes.Add("type", "text/css");
                    css.Attributes.Add("rel", "stylesheet");
                    Page.Header.Controls.Add(css);
                }
            }
            base.OnInit(e);
        }
        //protected override void OnLoad(EventArgs e)
        //{
            //if (!DesignMode)
            //{
            //    HtmlContainerControl attachto = null;
            //    if (!String.IsNullOrEmpty(attachTo)) attachto = (HtmlContainerControl)Page.FindControl(attachTo);
            //    if (attachto != null) attachto.InnerHtml = PrintPage();
            //}
        //    base.OnLoad(e);
        //}
        /// <summary>
        /// 设置数据源
        /// </summary>
        public override Object DataSource
        {
            get { return base.DataSource; }
            set
            {
                if (!DesignMode)
                {
                    //String cookname = Path.GetFileName(Request.Url.AbsolutePath).ToLower();
                    //NameValueCollection Query = new NameValueCollection(Request.QueryString);
                    //Query.Remove(PageString);
                    //Query.Remove(null);
                    //Query.Remove("");
                    //foreach (String s in Query.AllKeys)
                    //{
                    //    cookname += String.Format("{0}={1}", s, Query[s]);
                    //}
                    //cookname = EncryptHelper.MD5Lower32(cookname.ToLower());
                    ///*初始页面时还原上次访问的页面*/
                    ////String cookname = Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath).ToLower() + PageString;

                    String cookname = Request.Url.GetHashCode().ToString();
                    if (String.IsNullOrEmpty(Request[PageString]))
                    {
                        HttpCookie cook = Request.Cookies[cookname];
                        if (cook != null) PageIndex = int.Parse(cook.Value);
                    }
                    else
                    {
                        HttpCookie cook = new HttpCookie(cookname, Request[PageString]);
                        Response.Cookies.Add(cook);
                    }
                }
                PagedDataSource dt = new PagedDataSource();
                dt.DataSource = ConvertDataSource(value);
                RecordCount = dt.DataSourceCount;
                dt.AllowPaging = true;
                dt.PageSize = PageSize;
                dt.CurrentPageIndex = PageIndex - 1;
                base.DataSource = dt;
            }
        }
        /// <summary>
        /// 数据类型转换
        /// </summary>
        /// <param name="source">数据</param>
        /// <returns></returns>
        private IEnumerable ConvertDataSource(Object source)
        {
            if (source is IEnumerable)
                return (IEnumerable)source;
            else if (source is IList)
                return (IEnumerable)source;
            else if (source is DataView)
                return (IEnumerable)source;
            else if (source is DataSet)
                return (IEnumerable)(((DataSet)source).Tables[0].DefaultView);
            else if (source is DataTable)
                return (IEnumerable)(((DataTable)source).DefaultView);
            else
                return null;
        }

        #endregion

        #region Url输出页面代码

        /// <summary>
        /// 打印分页
        /// </summary>
        /// <returns></returns>
        protected String PrintPage()
        {
            String Href = GetUrl();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<div class=\"{0}\"", CssClass);
            if (Float != FloatCss.none)
            {
                sb.AppendFormat(" style=\"float:{0}\"", Float.ToString());
            }
            sb.Append(">");
            if (PageCount > 1)
            {
                if (PageIndex == 1)//第一页
                {
                    sb.Append("<span class=\"disabled\">&#171;</span><span class=\"disabled\">&#139;</span>");
                }
                else
                {
                    sb.AppendFormat("<a href=\"{0}\" title=\"首页\">&#171;</a><a href=\"{1}\" title=\"上一页\">&#139;</a>", String.Format(Href, 1), String.Format(Href, (PageIndex - 1)));
                }
                int nPage2 = PageIndex; //前进页面
                int nPage3 = PageIndex - 6;//后退页面
                if (PageCount > 10 && PageIndex > 6)//输出页数
                {
                    for (int j = 1; j <= 5; j++)
                    {
                        nPage3 = nPage3 + 1;
                        sb.AppendFormat("<a href=\"{0}\">{1}</a>", String.Format(Href, nPage3), nPage3);
                    }
                    for (int i = 1; i <= 5; i++)
                    {
                        if (nPage2 > PageCount) break;
                        if (nPage2 == PageIndex) //当前页
                        {
                            sb.AppendFormat("<span class=\"current\">{0}</span>", nPage2);
                        }
                        else
                        {
                            sb.AppendFormat("<a href=\"{0}\">{1}</a>", String.Format(Href, nPage2), nPage2);
                        }
                        nPage2 = nPage2 + 1;
                    }
                }
                else
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        if (i > PageCount) break;
                        if (i == PageIndex) //当前页
                        {
                            sb.AppendFormat("<span class=\"current\">{0}</span>", i);
                        }
                        else
                        {
                            sb.AppendFormat("<a href=\"{0}\">{1}</a>", String.Format(Href, i), i);
                        }
                    }
                }
                if (PageIndex == PageCount)//最后一页
                {
                    sb.Append("<span class=\"disabled\">&#155;</span><span class=\"disabled\">&#187;</span>");
                }
                else
                {
                    sb.AppendFormat("<a href=\"{0}\" title=\"下一页\">&#155;</a><a href=\"{1}\" title=\"尾页\">&#187;</a>", String.Format(Href, (PageIndex + 1)), String.Format(Href, PageCount));
                }
            }
            sb.AppendFormat("共有<span style=\"color:Red;font-weight:bold;\">{0}</span>条信息，<span style=\"color:Red;font-weight:bold;\">{1}</span>/<b>{2}</b>页，每页<b>{3}</b>条信息", RecordCount, PageIndex, PageCount, PageSize);
            sb.AppendLine("</div>");
            return sb.ToString();
        }
        /// <summary>
        /// 设计视图效果
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (DesignMode)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("<link href=\"{0}\" type=\"text/css\" rel=\"stylesheet\"/>",
                Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.Repeater.Repeater.css"));
                sb.AppendFormat("<div class=\"{0}\">", CssClass);
                sb.AppendLine("<a href=\"#\" title=\"首页\">&#171;</a><a href=\"#\" title=\"上一页\">&#139;</a>");
                for (int i = 1; i <= 5; i++)
                {
                    if (i == 3)
                    {
                        sb.AppendFormat("<span class=\"current\">{0}</span>", i);
                    }
                    else
                    {
                        sb.AppendFormat("<a href=\"#\">{0}</a>", i);
                    }
                }
                sb.AppendLine("<a href=\"#\" title=\"下一页\">&#155;</a><a href=\"#\" title=\"尾页\">&#187;</a>");
                writer.Write(sb.ToString());
            }
            else
            {
                writer.Write(PrintPage());
            }
        }
        /// <summary>
        /// 处理连接参数信息
        /// </summary>
        /// <returns></returns>
        private String GetUrl()
        {
            String baseQuery = String.Empty;
            StringBuilder url = new StringBuilder();
            NameValueCollection Query = new NameValueCollection(Request.QueryString);
            Query.Remove(PageString);
            Query.Remove(null);
            Query.Remove("");
            int length = Query.AllKeys.Length;
            url.Append(Request.Url.AbsolutePath);
            if (length > 0)
            {
                String[] parms = new String[length];
                for (int i = 0; i < length; i++)
                {
                    parms[i] = String.Format("{0}={1}", Query.AllKeys[i], Query[Query.AllKeys[i]]);
                }
                baseQuery = String.Join("&", parms);
                url.AppendFormat(String.Format("?{0}", baseQuery));
                url.AppendFormat("&{0}={1}", PageString, "{0}");
                return url.ToString();
            }
            return url.AppendFormat("?{0}={1}", PageString, "{0}").ToString();
        }

        #endregion

        #region 相关配置参数

        /// <summary>
        /// 当前页索引
        /// </summary>
        protected int PageIndex
        {
            get
            {
                if (DesignMode) return 1;
                int index = 1;
                if (ViewState["PageIndex"] == null)
                {
                    index = String.IsNullOrEmpty(Request[PageString]) ? 1 : int.Parse(Request[PageString]);
                }
                else
                {
                    index = (int)(ViewState["PageIndex"]);
                }
                if (index > PageCount) index = PageCount;
                if (index < 1) index = 1;
                return index;
            }
            set { ViewState["PageIndex"] = value; }
        }
        /// <summary>
        /// 总记录数
        /// </summary>
        protected int RecordCount
        {
            get { return ViewState["RecordCount"] == null ? 0 : (int)ViewState["RecordCount"]; }
            set { ViewState["RecordCount"] = value; }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        protected int PageCount
        {
            get
            {
                return (int)Math.Ceiling((double)RecordCount / (double)PageSize);
            }
        }
        /// <summary>
        /// 分页QueryString字符
        /// </summary>
        [Category("分页属性"), DefaultValue(""), Description("分页QueryString字符")]
        public String PageString
        {
            get { return (ViewState["PageString"] == null) ? "page" : (String)(ViewState["PageString"]); }
            set { ViewState["PageString"] = value; }
        }
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        [Category("分页属性"), DefaultValue(0), Description("分页大小")]
        public int PageSize
        {
            get { return (ViewState["PageSize"] == null) ? 10 : (int)(ViewState["PageSize"]); }
            set { ViewState["PageSize"] = value; }
        }
        /// <summary>
        /// 设置显示分页控件
        /// </summary>
        [Category("分页属性"), TypeConverter(typeof(IDConverter)), Description("分页显示控件")]
        public String attachTo
        {
            set { ViewState["attachTo"] = value; }
            get { return (String)ViewState["attachTo"]; }
        }
        [Category("分页属性"), DefaultValue(-1), Description("分页样式")]
        public RepeaterPageCss CssClass
        {
            get { return ViewState["CssClass"] == null ? RepeaterPageCss.black2 : (RepeaterPageCss)ViewState["CssClass"]; }
            set { ViewState["CssClass"] = value; }
        }
        [Category("分页属性"), DefaultValue(-1), Description("浮动设置")]
        public FloatCss Float
        {
            get { return ViewState["Float"] == null ? FloatCss.left : (FloatCss)ViewState["Float"]; }
            set { ViewState["Float"] = value; }
        }
        #endregion
        /// <summary>
        /// 控件筛选
        /// </summary>
        public sealed class IDConverter : ControlIDConverter
        {
            protected override bool FilterControl(Control control)
            {
                if (control is HtmlContainerControl)
                {
                    return true;
                }
                return false;
            }
        }
        protected HttpRequest Request
        {
            get { return Page.Request; }
        }
        protected HttpResponse Response
        {
            get { return Page.Response; }
        }
    }
}
