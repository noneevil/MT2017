using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSite.Controls
{
    /// <summary>
    /// 日期选择框
    /// </summary>
    public class DatePicker : TextBox
    {
        protected override void OnInit(EventArgs e)
        {
            //if (!DesignMode)
            //{
            //if (Page.Items["__datepicker"] == null)
            //{
            //LiteralControl css = new LiteralControl(string.Format("<link href=\"{0}\" type=\"text/css\" rel=\"stylesheet\"/>",
            //    Page.ClientScript.GetWebResourceUrl(GetType(), "SiteControl.Resources.datepicker.css")));
            //Page.Header.Controls.Add(css);

            //HtmlGenericControl js = new HtmlGenericControl("script");
            //js.Attributes["type"] = "text/javascript";
            //js.Attributes["src"] = Page.ClientScript.GetWebResourceUrl(GetType(), "SiteControl.Resources.mootools-more.js");
            //Page.Header.Controls.Add(js);

            //js = new HtmlGenericControl("script");
            //js.Attributes["type"] = "text/javascript";
            //js.Attributes["src"] = Page.ClientScript.GetWebResourceUrl(GetType(), "SiteControl.Resources.datepicker.js");
            //Page.Header.Controls.Add(js);

            //Page.Items["__datepicker"] = true;

            //}
            //}
            this.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            base.OnInit(e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.ShowMode == DateMode.Date)
                this.Style.Add("background", "url(/developer/images/ico/datepicker.gif) no-repeat right");
            else
                this.Style.Add("background", "url(/developer/images/ico/datetime.gif) no-repeat right");
            if (!DesignMode)
            {
                writer.Write("<div style=\"position:relative;z-index:1;float:left;\">");

                this.TextMode = TextBoxMode.SingleLine;
                this.ReadOnly = true;
                this.Wrap = false;
                base.Render(writer);

                writer.Write("<script type=\"text/javascript\">");
                writer.Write("window.addEvent('domready', function() {");
                if (ShowMode == DateMode.Date)
                {
                    writer.Write(string.Format("new Picker.Date('{0}', {{format: '%Y-%m-%d',pickerClass: '{1}' }});", this.ClientID, CssPicker));
                }
                else
                {
                    writer.Write(string.Format("new Picker.Date('{0}', {{format: '%Y-%m-%d %H:%M',timePicker: true,pickerClass: '{1}' }});", this.ClientID, CssPicker));
                }
                writer.Write("});</script>");
                writer.Write("</div>");
            }
            else
            {
                this.Text = this.ClientID;
                this.Style.Add("border", "1px solid #ADD2DA");
                base.Render(writer);
            }
        }
        /// <summary>
        /// 控件外观
        /// </summary>
        public string CssPicker
        {
            get { return ViewState["CssPicker"] == null ? "datepicker" : (String)ViewState["CssPicker"]; }
            set { ViewState["CssPicker"] = value; }
        }
        public DateMode ShowMode
        {
            get
            {
                object obj = ViewState["ShowMode"];
                if (obj != null) return (DateMode)obj;
                return DateMode.Date;
            }
            set { ViewState["ShowMode"] = value; }
        }
        /// <summary>
        /// 控件模式
        /// </summary>
        public enum DateMode
        {
            /// <summary>
            /// 日期
            /// </summary>
            Date,
            /// <summary>
            /// 日期时间
            /// </summary>
            Time
        }
    }
}