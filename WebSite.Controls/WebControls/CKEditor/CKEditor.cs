using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebSite.Interface;

namespace WebSite.Controls
{
    /// <summary>
    /// CkEditor
    /// </summary>
    public class CKEditor : System.Web.UI.HtmlControls.HtmlTextArea, IControls
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                if (Page.FindControl("ckeditor") == null)
                {
                    HtmlGenericControl js = new HtmlGenericControl("script");
                    js.ID = "ckeditor";
                    js.Attributes["type"] = "text/javascript";
                    js.Attributes["src"] = "/developer/ckeditor/ckeditor.js";
                    Page.Header.Controls.Add(js);
                }
                if (Page.FindControl("ckfinder") == null)
                {
                    HtmlGenericControl js = new HtmlGenericControl("script");
                    js.ID = "ckfinder";
                    js.Attributes["type"] = "text/javascript";
                    js.Attributes["src"] = "/developer/ckfinder/ckfinder.js";
                    Page.Header.Controls.Add(js);
                }
            }
            base.OnLoad(e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (!DesignMode)
            {
                String code = String.Format("<script type=\"text/javascript\">CKEDITOR.replace('{0}',{{skin:'{1}',width:'{2}',height:'{3}',toolbar:'{4}'}});</script>",
                    new Object[] { 
                        this.ClientID, 
                        this.CssClass,
                        this.Width, 
                        this.Height, 
                        this.ToolBarSet 
                });
                writer.Write(code);
            }
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
        public String ToolBarSet
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
        public String CssClass
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
        public String Text
        {
            get { return base.Value; }
            set { base.Value = value; }
        }


        [Browsable(false)]
        public String FormatString { get; set; }
        public void SetValue(Object value)
        {
            base.Value = Convert.ToString(value);
        }
        public Object GetValue()
        {
            return base.Value;
        }
    }
}
