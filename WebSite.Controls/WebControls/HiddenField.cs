using System;
using System.ComponentModel;
using WebSite.Interface;

namespace WebSite.Controls
{
    public class HiddenField : System.Web.UI.WebControls.HiddenField, IControls
    {
        [Browsable(false)]
        public String FormatString { get; set; }

        public void SetValue(Object value)
        {
            this.Value = Convert.ToString(value);
        }

        public Object GetValue()
        {
            return this.Value;
        }
    }
}
