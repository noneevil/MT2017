using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

[assembly: WebResource("WebSite.Controls.save2local.save2local.swf", "application/x-shockwave-flash")]
[assembly: WebResource("WebSite.Controls.save2local.save2local.js", "application/x-javascript", PerformSubstitution = true)]
[assembly: WebResource("WebSite.Controls.Resources.stop_icon.gif", "image/gif")]
namespace WebSite.Controls
{
    public class Save2local : Control
    {
        protected override void OnInit(EventArgs e)
        {
            if (!DesignMode)
            {
                if (Page.FindControl("save2local") == null)
                {
                    HtmlGenericControl js = new HtmlGenericControl("script");
                    js.ID = "save2local";
                    js.Attributes["type"] = "text/javascript";
                    js.Attributes["src"] = Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.save2local.save2local.js");
                    Page.Header.Controls.Add(js);
                }
            }
            
            base.OnInit(e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                writer.Write(this.ClientID);
            }
            else
            {
                writer.Write(string.Format("<span id=\"{0}_tag\">", this.ClientID));
                writer.Write("<script type=\"text/javascript\">");
                //writer.Write("var {0}_movie;\r", this.ClientID);
                //writer.Write("window.addEvent(\"domready\", function() {");
                writer.Write(string.Format("var Save2local=new save2local('{0}?r=0.{1}', {{id:'{2}',container:'{2}_tag'}});",
                new object[] {
                        Page.ClientScript.GetWebResourceUrl(GetType(), "WebSite.Controls.save2local.save2local.swf"),
                        DateTime.Now.Ticks,
                        this.ClientID
                }));
                //writer.Write("});</script>");
                //writer.Write("</span>");
                writer.WriteEndTag("script");
                writer.WriteEndTag("span");
            }
        }
    }
}
