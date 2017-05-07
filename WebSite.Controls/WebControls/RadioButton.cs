using System;
using System.ComponentModel;
using WebSite.Interface;

namespace WebSite.Controls
{
    public class RadioButton : System.Web.UI.WebControls.RadioButton, IControls
    {
        [Browsable(false)]
        public string FormatString { get; set; }

        public void SetValue(object value)
        {
            this.Checked = Convert.ToBoolean(value);
        }

        public object GetValue()
        {
            return this.Checked;
        }
    }
}
