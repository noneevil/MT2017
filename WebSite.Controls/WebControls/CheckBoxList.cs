using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls;
using WebSite.Interface;
using System.Web;

namespace WebSite.Controls
{
    public class CheckBoxList : System.Web.UI.WebControls.CheckBoxList, IControls
    {
        [Browsable(false)]
        public String FormatString { get; set; }

        public void SetValue(object value)
        {
            this.SelectedIndex = -1;
            String[] split = Convert.ToString(value).Split(',');
            foreach (String v in split)
            {
                String val = v.Trim();
                foreach (ListItem item in this.Items)
                {
                    if (item.Value == val)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        public object GetValue()
        {
            List<String> value = new List<String>();
            foreach (ListItem item in this.Items)
            {
                if (item.Selected) value.Add(item.Value);
            }
            return String.Join(",", value.ToArray());
        }
    }
}
