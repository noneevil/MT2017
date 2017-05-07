using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using WebSite.Interface;

namespace WebSite.Controls
{
    public class DropDownList : System.Web.UI.WebControls.DropDownList, IControls
    {
        [Browsable(false)]
        public string FormatString { get; set; }

        public void SetValue(object value)
        {
            String val = Convert.ToString(value);
            this.SelectedIndex = -1;
            foreach (ListItem item in this.Items)
            {
                if (item.Value == val)
                {
                    item.Selected = true;
                    break;
                }
            }
        }

        public object GetValue()
        {
            return this.SelectedValue;
        }
    }
}
