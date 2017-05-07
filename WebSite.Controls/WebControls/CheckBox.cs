using System;
using System.ComponentModel;
using WebSite.Interface;

namespace WebSite.Controls
{
    public class CheckBox : System.Web.UI.WebControls.CheckBox, IControls
    {
        [Browsable(false)]
        public String FormatString { get; set; }

        public void SetValue(Object value)
        {
            this.Checked = Convert.ToBoolean(value);
        }

        public Object GetValue()
        {
            return this.Checked;
        }
    }
}
