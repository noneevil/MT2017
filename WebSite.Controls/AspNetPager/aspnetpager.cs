using System;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: WebResource("WebSite.Controls.AspNetPager.ANPScript.js", "text/javascript")]
[assembly: WebResource("WebSite.Controls.AspNetPager.Repeater.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("WebSite.Controls.AspNetPager.Repeater_1.gif", "image/gif")]
[assembly: WebResource("WebSite.Controls.AspNetPager.Repeater_2.gif", "image/gif")]
[assembly: WebResource("WebSite.Controls.AspNetPager.Repeater_3.gif", "image/gif")]
namespace WebSite.Controls
{
    #region AspNetPager Server Control

    /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Class[@name="AspNetPager"]/*'/>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [DefaultProperty("PageSize")]
    [DefaultEvent("PageChanged")]
    [ParseChildren(false)]
    [PersistChildren(false)]
    [ANPDescription("desc_AspNetPager")]
    [Designer(typeof(PagerDesigner))]
    [ToolboxData("<{0}:AspNetPager runat=server></{0}:AspNetPager>")]
    [ToolboxBitmap(typeof(AspNetPager), "AspNetPager.bmp")]
    public partial class AspNetPager : Panel, INamingContainer, IPostBackEventHandler, IPostBackDataHandler
    {

        #region Control Rendering Logic

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="OnInit"]/*'/>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (null != CloneFrom && string.Empty != CloneFrom.Trim())
            {
                AspNetPager ctrl = Parent.FindControl(CloneFrom) as AspNetPager;
                if (null == ctrl)
                {
                    string errStr = SR.GetString("def_CloneFromTypeError");
                    throw new ArgumentException(errStr.Replace("%controlID%", CloneFrom), "CloneFrom");
                }
                if (null != ctrl.cloneFrom && this == ctrl.cloneFrom)
                {
                    string errStr = SR.GetString("def_RecursiveCloneFrom");
                    throw new ArgumentException(errStr, "CloneFrom");
                }
                cloneFrom = ctrl;
                CssClass = cloneFrom.CssClass;
                Width = cloneFrom.Width;
                Height = cloneFrom.Height;
                HorizontalAlign = cloneFrom.HorizontalAlign;
                BackColor = cloneFrom.BackColor;
                BackImageUrl = cloneFrom.BackImageUrl;
                BorderColor = cloneFrom.BorderColor;
                BorderStyle = cloneFrom.BorderStyle;
                BorderWidth = cloneFrom.BorderWidth;
                Font.CopyFrom(cloneFrom.Font);
                ForeColor = cloneFrom.ForeColor;
                EnableViewState = cloneFrom.EnableViewState;
                Enabled = cloneFrom.Enabled;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="OnLoad"]/*'/>
        protected override void OnLoad(EventArgs e)
        {
            if (UrlPaging)
            {
                currentUrl = Page.Request.Path;
                queryString = Page.Request.ServerVariables["Query_String"];
                if (!string.IsNullOrEmpty(queryString) && queryString.StartsWith("?")) //mono <v2.8 compatible
                    queryString = queryString.TrimStart('?');
                if (!Page.IsPostBack && cloneFrom == null)
                {
                    int index;
                    int.TryParse(Page.Request.QueryString[UrlPageIndexName], out index);
                    if (index <= 0)
                        index = 1;
                    else if (ReverseUrlPageIndex)
                        index = PageCount - index + 1;
                    PageChangingEventArgs args = new PageChangingEventArgs(index);
                    OnPageChanging(args);
                }
            }
            else
            {
                inputPageIndex = Page.Request.Form[UniqueID + "_input"];
            }
            base.OnLoad(e);
            if ((UrlPaging || (!UrlPaging && PageIndexBoxType == PageIndexBoxType.TextBox)) && (ShowPageIndexBox == ShowPageIndexBox.Always || (ShowPageIndexBox == ShowPageIndexBox.Auto && PageCount >= ShowBoxThreshold)))
            {
                HttpContext.Current.Items[scriptRegItemName] = true;
            }
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="OnPreRender"]/*'/>
        //protected override void OnPreRender(EventArgs e)
        //{
        //    Page.ClientScript.RegisterClientScriptResource(this.GetType(), "WebSite.Controls.AspNetPager.ANPScript.js");
        //        base.OnPreRender(e);
        //}

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="AddAttributesToRender"]/*'/>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (Page != null && !UrlPaging)
                Page.VerifyRenderingInServerForm(this);
            const string isANPScriptRegistered = "isANPScriptRegistered";
            if (!DesignMode && HttpContext.Current.Items[scriptRegItemName] != null && HttpContext.Current.Items[isANPScriptRegistered] == null)
            {
                writer.Write("<script type=\"text/javascript\" src=\"");
                writer.Write(Page.ClientScript.GetWebResourceUrl(this.GetType(), "WebSite.Controls.AspNetPager.ANPScript.js"));
                writer.WriteLine("\"></script>");
                HttpContext.Current.Items[isANPScriptRegistered] = true;
            }
            base.AddAttributesToRender(writer);
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="RenderBeginTag"]/*'/>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            bool showPager = (PageCount > 1 || (PageCount <= 1 && AlwaysShow));
            //writer.WriteLine();
            //writer.Write("<!-- ");
            //writer.Write(SR.GetString("def_CopyrightText"));
            //writer.WriteLine(" -->");
            if (!showPager)
            {
                //writer.Write("<!--");
                //writer.Write(SR.GetString("def_AutoHideInfo"));
                //writer.Write("-->");
            }
            else
                base.RenderBeginTag(writer);
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="RenderEndTag"]/*'/>
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            if (PageCount > 1 || (PageCount <= 1 && AlwaysShow))
                base.RenderEndTag(writer);
            //writer.WriteLine();
            //writer.Write("<!-- ");
            //writer.Write(SR.GetString("def_CopyrightText"));
            //writer.WriteLine(" -->");
            //writer.WriteLine();
        }


        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Method[@name="RenderContents"]/*'/>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (PageCount <= 1 && !AlwaysShow)
                return;
            writer.Indent = 0;
            if (ShowCustomInfoSection != ShowCustomInfoSection.Never)
            {
                if (LayoutType == LayoutType.Table)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, Style.Value);
                    if (Height != Unit.Empty)
                        writer.AddStyleAttribute(HtmlTextWriterStyle.Height, Height.ToString());
                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                    writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                    writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                    writer.RenderBeginTag(HtmlTextWriterTag.Table); //<table>
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr); //<tr>
                }
                if (ShowCustomInfoSection == ShowCustomInfoSection.Left)
                {
                    RenderCustomInfoSection(writer);
                    RenderNavigationSection(writer);
                }
                else
                {
                    RenderNavigationSection(writer);
                    RenderCustomInfoSection(writer);
                }
                if (LayoutType == LayoutType.Table)
                {
                    writer.RenderEndTag(); //</tr>
                    writer.RenderEndTag(); //</table>
                }
            }
            else
                RenderPagingElements(writer);
        }


        #endregion


    }

    #endregion


    #region PageChangingEventHandler Delegate
    /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Delegate[@name="PageChangingEventHandler"]/*'/>
    public delegate void PageChangingEventHandler(object src, PageChangingEventArgs e);

    #endregion

}