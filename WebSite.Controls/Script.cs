using System;
using System.Web.UI;

namespace WebSite.Controls
{
    /// <summary>
    /// JS脚本注册组件
    /// </summary>
    [ToolboxData("<{0}:Script runat=server />")]
    public class Script : System.Web.UI.HtmlControls.HtmlGenericControl
    {
        public Script(String tag)
            : base("script")
        {
        }

        protected override void RenderAttributes(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, this.Src);
            base.RenderAttributes(writer);
        }
        /// <summary>
        /// js文件路径
        /// </summary>
        public virtual String Src
        {
            get
            {
                String path = (String)this.ViewState["Src"];
                if (path == null) return String.Empty;
                return base.ResolveUrl(path);
            }
            set { this.ViewState["Src"] = value; }
        }
    }
}
