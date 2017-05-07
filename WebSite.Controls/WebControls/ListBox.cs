using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls;
using WebSite.Interface;

namespace WebSite.Controls
{
    /// <summary>
    /// 支持持久化html属性
    /// </summary>
    public class ListBox : System.Web.UI.WebControls.ListBox, IControls
    {
        [Browsable(false)]
        public String FormatString { get; set; }

        public void SetValue(Object value)
        {
            String val = Convert.ToString(value);
            this.SelectedIndex = -1;

            if (this.SelectionMode == ListSelectionMode.Single)
            {
                foreach (ListItem item in this.Items)
                {
                    if (item.Value == val)
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
            else
            {
                String[] split = val.Split(',');
                foreach (String v in split)
                {
                    foreach (ListItem item in this.Items)
                    {
                        if (item.Value == v)
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
        }

        public Object GetValue()
        {
            if (this.SelectionMode == ListSelectionMode.Single)
            {
                return this.SelectedValue;
            }
            else
            {
                List<String> value = new List<String>();
                foreach (ListItem item in this.Items)
                {
                    if (item.Selected)
                        value.Add(item.Value);
                }
                return String.Join(",", value.ToArray());
            }
        }

        #region 持久化html属性

        protected override Object SaveViewState()
        {
            // Create an Object array with one element for the CheckBoxList's
            // ViewState contents, and one element for each ListItem in skmCheckBoxList
            Object[] state = new Object[this.Items.Count + 1];

            Object baseState = base.SaveViewState();
            state[0] = baseState;

            // Now, see if we even need to save the view state
            bool itemHasAttributes = false;
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].Attributes.Count > 0)
                {
                    itemHasAttributes = true;

                    // Create an array of the item's Attribute's keys and values
                    Object[] attribKV = new Object[this.Items[i].Attributes.Count * 2];
                    int k = 0;
                    foreach (string key in this.Items[i].Attributes.Keys)
                    {
                        attribKV[k++] = key;
                        attribKV[k++] = this.Items[i].Attributes[key];
                    }

                    state[i + 1] = attribKV;
                }
            }

            // return either baseState or state, depending on whether or not
            // any ListItems had attributes
            if (itemHasAttributes)
                return state;
            else
                return baseState;

        }
        protected override void LoadViewState(Object savedState)
        {
            if (savedState == null) return;
            // see if savedState is an Object or Object array
            if (savedState is Object[])
            {
                // we have an array of items with attributes
                Object[] state = (Object[])savedState;
                base.LoadViewState(state[0]);   // load the base state

                for (int i = 1; i < state.Length; i++)
                {
                    if (state[i] != null)
                    {
                        // Load back in the attributes
                        Object[] attribKV = (Object[])state[i];
                        for (int k = 0; k < attribKV.Length; k += 2)
                            this.Items[i - 1].Attributes.Add(attribKV[k].ToString(),
                                                           attribKV[k + 1].ToString());
                    }
                }
            }
            else
                // we have just the base state
                base.LoadViewState(savedState);

        }

        #endregion
    }
}
