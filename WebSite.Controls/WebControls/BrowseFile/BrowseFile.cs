using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;

[assembly: WebResource("WebSite.Controls.WebControls.BrowseFile.CkFinder.html", "text/html")]
[assembly: WebResource("WebSite.Controls.WebControls.BrowseFile.CkFinder.js", "application/x-javascript", PerformSubstitution = true)]
namespace WebSite.Controls
{
    public enum CkFinderResourceType
    {
        文档文件,
        图片文件,
        Flash动画,
        音乐视频,
        广告文件
    }
    /// <summary>
    /// CkFinder 文件上传控件
    /// </summary>
    [ToolboxData("<{0}:BrowseFile runat=server />")]
    public class BrowseFile : WebSite.Controls.TextBox
    {
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!DesignMode)
            {
                if (Page.Items["__BrowseFile"] == null)
                {
                    String iframeid = "_" + this.ClientID;
                    Page.Items["__BrowseFile"] = iframeid;
                    //HtmlGenericControl iframe = new HtmlGenericControl("script");
                    //iframe.Attributes["type"] = "text/javascript";
                    //iframe.Attributes["src"] = Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.WebControls.BrowseFile.CkFinder.js");

                    HtmlGenericControl iframe = new HtmlGenericControl("iframe");
                    iframe.Attributes["src"] = Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.WebControls.BrowseFile.CkFinder.html");
                    iframe.Attributes["id"] = iframeid;
                    iframe.Attributes["name"] = iframeid;
                    iframe.Attributes["style"] = "display:none;";
                    iframe.Attributes["frameborder"] = "0";
                    iframe.RenderControl(writer);
                }
            }
            //this.ReadOnly = true;
            base.Render(writer);

            writer.Write(String.Format("<a style=\"padding-left:5px\" href=\"javascript:\" onclick=\"window.frames['{4}'].BrowseFile({{ResourceType:'{0}',ClientID:'{1}',Multiple:{2},Folder:'{0}:/{3}/'}});\">",
                this.ResourceType,
                this.ClientID,
                this.SelectMultiple.ToString().ToLower(),
                DateTime.Now.ToString("yyyy-MM-dd"),
                Page.Items["__BrowseFile"]));
            writer.Write("浏览");
            writer.Write("</a>");

        }
        [Bindable(true), Category("参数设置"), Description("多文件选择."), DefaultValue(0)]
        public Boolean SelectMultiple
        {
            get
            {
                return Convert.ToBoolean(ViewState["selectmultiple"]);
            }
            set { ViewState["selectmultiple"] = value; }
        }
        [Bindable(true), Category("参数设置"), Description("资源类型."), DefaultValue(0)]
        public CkFinderResourceType ResourceType
        {
            get
            {
                Object obj = ViewState["resourcetype"];
                if (obj != null) return (CkFinderResourceType)obj;
                return CkFinderResourceType.图片文件;
            }
            set { ViewState["resourcetype"] = value; }
        }
    }
}
