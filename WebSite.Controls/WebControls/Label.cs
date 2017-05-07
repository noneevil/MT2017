using System;
using System.ComponentModel;
using WebSite.Interface;
using System.Globalization;

namespace WebSite.Controls
{
    public class Label : System.Web.UI.WebControls.Label, IControls
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

        public Object GetValue()
        {
            String value = this.Text.Trim();
            return value;
        }
    }
}
