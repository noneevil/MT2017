using System;
using System.ComponentModel;
using System.Web.Security;
using System.Web.UI.WebControls;
using WebSite.Interface;
using System.Globalization;

namespace WebSite.Controls
{
    //[ToolboxBitmap(typeof(System.Web.UI.WebControls.TextBox), "System.Web.UI.WebControls.TextBox.bmp")]
    public class TextBox : System.Web.UI.WebControls.TextBox, IControls
    {
        [Bindable(true), Category("Expand"), DefaultValue("")]
        public String FormatString
        {
            get
            {
                return Convert.ToString(ViewState["FormatString"]);
            }
            set
            {
                ViewState["FormatString"] = value;
            }
        }

        public void SetValue(Object value)
        {
            if (this.TextMode == TextBoxMode.Password)
            {
                ViewState["value"] = value;
            }
            else
            {
                if (String.IsNullOrEmpty(FormatString))
                {
                    this.Text = Convert.ToString(value);
                }
                else
                {
                    this.Text = String.Format(CultureInfo.CurrentCulture, FormatString, new Object[] { value });
                    //this.Text = String.Format(FormatString, value);
                }
            }
        }

        public Object GetValue()
        {
            String value = this.Text.Trim();
            if (this.TextMode == TextBoxMode.Password)
            {
                if (!String.IsNullOrEmpty(value))
                {
                    String encrypt = FormsAuthentication.HashPasswordForStoringInConfigFile(value, "MD5");
                    return encrypt.ToUpper();
                }
                else
                {
                    value = Convert.ToString(ViewState["value"]);
                }
            }
            return value;
        }
    }
}
