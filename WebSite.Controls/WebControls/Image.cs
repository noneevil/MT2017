using System;
using System.ComponentModel;
using WebSite.Interface;

namespace WebSite.Controls
{
    public class Image : System.Web.UI.WebControls.Image, IControls
    {
        [Browsable(false)]
        public string FormatString { get; set; }

        public void SetValue(object value)
        {
            this.ImageUrl = Convert.ToString(value);
        }

        public object GetValue()
        {
            return this.ImageUrl;
        }
    }
}
