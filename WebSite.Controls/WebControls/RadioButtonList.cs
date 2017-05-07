using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using WebSite.Interface;

namespace WebSite.Controls
{
    public class RadioButtonList : System.Web.UI.WebControls.RadioButtonList, IControls
    {
        [Browsable(false)]
        public string FormatString { get; set; }

        public void SetValue(object value)
        {
            this.SelectedIndex = -1;
            if (value is Boolean) value = ((Boolean)value) ? 1 : 0;
            String val = Convert.ToString(value);
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
            switch (this.SelectedValue)
            {
                case "1":
                    return 1;
                case "0":
                    return 0;
                default:
                    return this.SelectedValue;
            }
        }
    }
}
