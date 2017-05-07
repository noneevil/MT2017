using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebSite.Controls
{
    public class DevelopCKEditor : System.Web.UI.HtmlControls.HtmlTextArea
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                if (Page.Items["ckeditor.js"] == null)
                {
                    HtmlGenericControl js = new HtmlGenericControl("script");
                    js.Attributes["type"] = "text/javascript";
                    js.Attributes["src"] = "/developer/Plugins/ckeditor/ckeditor.js";
                    Page.Header.Controls.Add(js);

                    js = new HtmlGenericControl("script");
                    js.Attributes["type"] = "text/javascript";
                    js.Attributes["src"] = "/developer/Plugins/ckfinder/ckfinder.js";
                    Page.Header.Controls.Add(js);

                    Page.Items["ckeditor.js"] = true;
                }
            }
            base.OnLoad(e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            string code = string.Format(
                "<script type=\"text/javascript\">CKEDITOR.replace('{0}',{{skin:'{1}',width:'{2}',height:'{3}',toolbar:'{4}'}});</script>",
                new object[] { this.ClientID, this.CssClass, this.Width, this.Height, this.ToolBarSet });
            writer.Write(code);

        }
        /// <summary>
        /// 高度
        /// </summary>
        public Unit Height
        {
            get
            {
                Object obj = ViewState["Height"];
                if (obj != null) return (Unit)obj;
                return 0;
            }
            set { ViewState["Height"] = value; }
        }
        /// <summary>
        /// 宽度
        /// </summary>
        public Unit Width
        {
            get
            {
                Object obj = ViewState["Width"];
                if (obj != null) return (Unit)obj;
                return Unit.Parse("100%");
            }
            set { ViewState["Width"] = value; }
        }
        /// <summary>
        /// 工具
        /// </summary>
        public string ToolBarSet
        {
            get
            {
                Object obj = ViewState["ToolBarSet"];
                if (obj != null) return (String)obj;
                return "Min";
            }
            set
            {
                ViewState["ToolBarSet"] = value;
            }
        }
        /// <summary>
        /// 样式
        /// </summary>
        public string CssClass
        {
            get
            {
                Object obj = ViewState["CssClass"];
                if (obj != null) return (String)obj;
                return "kama";
            }
            set
            {
                ViewState["CssClass"] = value;
            }
        }
        /// <summary>
        /// 值
        /// </summary>
        public string Text
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}
