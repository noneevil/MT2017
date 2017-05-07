using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebSite.Interface;

[assembly: WebResource("WebSite.Controls.WebControls.ColorPicker.SliderHandle.gif", "image/gif")]
[assembly: WebResource("WebSite.Controls.WebControls.ColorPicker.TabCenterActive.gif", "image/gif")]
[assembly: WebResource("WebSite.Controls.WebControls.ColorPicker.TabLeftActive.gif", "image/gif")]
[assembly: WebResource("WebSite.Controls.WebControls.ColorPicker.TabLeftInactive.gif", "image/gif")]
[assembly: WebResource("WebSite.Controls.WebControls.ColorPicker.TabRightActive.gif", "image/gif")]
[assembly: WebResource("WebSite.Controls.WebControls.ColorPicker.TabRightInactive.gif", "image/gif")]
[assembly: WebResource("WebSite.Controls.WebControls.ColorPicker.ColorPickerIcon.jpg", "image/jpeg")]
[assembly: WebResource("WebSite.Controls.WebControls.ColorPicker.ColorPicker.css", "text/css")]
[assembly: WebResource("WebSite.Controls.WebControls.ColorPicker.ColorPicker.js", "application/x-javascript")]
namespace WebSite.Controls
{
    /// <summary>
    /// 颜色选择控件
    /// </summary>
    //[ToolboxBitmap(typeof(ColorPicker), "Images.ColorPickerIcon.jpg"), DefaultProperty("Text")]
    [ToolboxData("<{0}:ColorPicker runat=server />")]
    public class ColorPicker : WebControl, IPostBackDataHandler, IControls
    {
        private string _color = "#000000";

        public event EventHandler ColorChanged;

        //public static string ColorToString(Color color)
        //{
        //    if ((color.IsKnownColor || color.IsNamedColor) || color.IsSystemColor)
        //    {
        //        return color.Name;
        //    }
        //    return ("#" + color.ToArgb().ToString("X").Substring(2));
        //}

        //public static System.Drawing.Color StringToColor(string colorString)
        //{
        //    if ((colorString[0] == '#') && (colorString.Length < 8))
        //    {
        //        string str = colorString.Substring(1);
        //        while (str.Length != 6)
        //        {
        //            str = "0" + str;
        //        }
        //        int red = Convert.ToInt32(str.Substring(0, 2), 0x10);
        //        int green = Convert.ToInt32(str.Substring(2, 2), 0x10);
        //        int blue = Convert.ToInt32(str.Substring(4, 2), 0x10);
        //        return System.Drawing.Color.FromArgb(red, green, blue);
        //    }
        //    return System.Drawing.Color.FromName(colorString);
        //}
        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string color = this.Color;
            string str2 = postCollection[postDataKey];
            if (!((color != null) && color.Equals(str2)))
            {
                this.Color = str2;
                return true;
            }
            return false;
        }

        protected override void LoadViewState(object savedState)
        {
            this.Color = (string)savedState;
        }

        public void OnColorChanged(EventArgs e)
        {
            if (this.ColorChanged != null)
            {
                this.ColorChanged(this, e);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.Page.ClientScript.RegisterClientScriptInclude("ColorPicker.js", GetWebResourceUrl("ColorPicker.js"));

            StringBuilder sb = new StringBuilder();
            sb.Append("var colorPicker = new ColorPicker({");
            sb.AppendFormat("FormWidgetAmountSliderHandleImage : '{0}',", GetWebResourceUrl("SliderHandle.gif"));
            sb.AppendFormat("TabRightActiveImage : '{0}',", GetWebResourceUrl("TabRightActive.gif"));
            sb.AppendFormat("TabRightInactiveImage : '{0}',", GetWebResourceUrl("TabRightInactive.gif"));

            sb.AppendFormat("TabLeftActiveImage : '{0}',", GetWebResourceUrl("TabLeftActive.gif"));
            sb.AppendFormat("TabLeftInactiveImage : '{0}',", GetWebResourceUrl("TabLeftInactive.gif"));
            sb.AppendFormat("AutoPostBack : {0},", this.AutoPostBack.ToString().ToLower());

            sb.AppendFormat("AutoPostBackReference : \"{0}\",", this.Page.ClientScript.GetPostBackEventReference(this, ""));
            sb.AppendFormat("PopupPosition : {0}", (int)this.PopupPosition);
            sb.Append("});");

            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "InitColorPicker", sb.ToString(), true);

            if (!(base.DesignMode || (this.Page.Header == null)))
            {
                this.RegisterCSSInclude(this.Page.Header);
            }
        }

        public virtual void RaisePostDataChangedEvent()
        {
            this.OnColorChanged(EventArgs.Empty);
        }

        private void RegisterCSSInclude(Control target)
        {
            bool flag = false;
            foreach (Control control in target.Controls)
            {
                if (control.ID == "ControlPickerStyle")
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                HtmlGenericControl css = new HtmlGenericControl("link");
                css.ID = "ControlPickerStyle";
                css.Attributes.Add("href", GetWebResourceUrl("ColorPicker.css"));
                css.Attributes.Add("type", "text/css");
                css.Attributes.Add("rel", "stylesheet");
                css.EnableViewState = false;
                target.Controls.Add(css);
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            PlaceHolder target = new PlaceHolder();
            if (base.DesignMode || (this.Page.Header == null))
            {
                this.RegisterCSSInclude(target);
            }
            Table child = new Table();
            child.Rows.Add(new TableRow());
            child.Rows[0].Cells.Add(new TableCell());
            child.Rows[0].Cells.Add(new TableCell());

            TextBox control = new TextBox();
            control.Width = this.Width;
            control.Height = this.Height;
            control.CssClass = this.CssClass;
            control.MaxLength = 15;
            control.ForeColor = System.Drawing.ColorTranslator.FromHtml(this.Color);
            control.Text = this.Color;
            control.ID = this.ClientID;
            child.Rows[0].Cells[0].Controls.Add(control);

            HtmlInputImage image = new HtmlInputImage();
            image.Src = GetWebResourceUrl("ColorPickerIcon.jpg");
            image.Attributes.Add("onclick", string.Format("colorPicker.ShowColorPicker(this,document.getElementById('{0}'));return false;", this.ClientID));
            HtmlGenericControl control2 = new HtmlGenericControl("div");
            control2.EnableViewState = false;
            control2.Controls.Add(image);
            control2.Attributes.CssStyle.Add(HtmlTextWriterStyle.Position, "relative");
            control2.Attributes.CssStyle.Add(HtmlTextWriterStyle.Display, "block");
            child.Rows[0].Cells[1].Controls.Add(control2);
            target.Controls.Add(child);
            target.RenderControl(output);
        }

        protected override object SaveViewState()
        {
            return this.Color;
        }

        [Localizable(true), Category("Behaviour"), DefaultValue("false"), Bindable(true)]
        public bool AutoPostBack
        {
            get
            {
                return ViewState["AutoPostBack"] == null ? false : (Boolean)ViewState["AutoPostBack"];
            }
            set
            {
                this.ViewState["AutoPostBack"] = value;
            }
        }

        [Localizable(true), Bindable(true), Category("Appearance"), DefaultValue("#000000")]
        public string Color
        {
            get
            {
                return this._color;
            }
            set
            {
                this._color = value;
            }
        }

        [Localizable(true), Category("Behaviour"), DefaultValue("0"), Bindable(true)]
        public PopupPosition PopupPosition
        {
            get
            {
                object obj = ViewState["PopupPosition"];
                if (obj != null) return (PopupPosition)obj;
                return PopupPosition.BottomRight;
            }
            set
            {
                this.ViewState["PopupPosition"] = value;
            }
        }

        /// <summary>
        /// 获取资源文件地址
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private String GetWebResourceUrl(String file)
        {
            return Page.ClientScript.GetWebResourceUrl(typeof(ColorPicker), "WebSite.Controls.WebControls.ColorPicker." + file);
        }

        [Browsable(false)]
        public string FormatString { get; set; }

        public void SetValue(object value)
        {
            this.Color = Convert.ToString(value);
        }

        public object GetValue()
        {
            return this.Color;
        }
    }
}
