using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MSSQLDB;
using WebSite.Interface;
using FastReflectionLib;

namespace CommonUtils
{
    /// <summary>
    /// WEB控件操作类
    /// </summary>
    public abstract class ControlHelper
    {
        /// <summary>
        /// 将 URL 转换为在请求客户端可用的 URL。
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static String ResolveUrl(String url)
        {
            return (new Control()).ResolveUrl(url);
        }
        /// <summary>
        /// 生成控件的HTML
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static String ControlRenderHtml(Control control)
        {
            HtmlTextWriter writer = new HtmlTextWriter(new StringWriter());
            control.RenderControl(writer);
            return writer.InnerWriter.ToString();

            //StringBuilder sb = new StringBuilder();
            //using (StringWriter sw = new StringWriter(sb))
            //{
            //    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            //    {
            //        control.RenderControl(htw);
            //    }
            //}
            //return sb.ToString();
        }
        /// <summary>
        /// 引用程序集内js资源
        /// </summary>
        /// <param name="file">文件名称</param>
        /// <param name="type">程序集类型</param>
        public static void RegisterResource(String file, Type type)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            String Template = @"<script src=""{0}"" type=""text/javascript""/></script>";
            String Url = page.ClientScript.GetWebResourceUrl(type, String.Format("Resource.{0}", file));
            LiteralControl include = new LiteralControl(String.Format(Template, Url));
            page.Header.Controls.Add(include);
        }

        #region 目录树

        #region 公用方法
        /// <summary>
        /// 选中treeview的某个节点，需要每个node的value不同
        /// </summary>
        /// <param name="sNodeValue"></param>
        public static void CheckNode(TreeView tv, String sNodeValue)
        {
            foreach (TreeNode tRoot in tv.Nodes)
            {
                if (tRoot.Value == sNodeValue)
                {
                    tRoot.Select();
                }
                else
                {
                    if (tRoot.ChildNodes != null)
                    {
                        foreach (TreeNode tChild in tRoot.ChildNodes)
                        {
                            if (tChild.Value == sNodeValue)
                                tChild.Select();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 绑定数据源到目录树控件上
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="view">目录树</param>
        /// <param name="parentId">上级ID字段</param>
        /// <param name="textMember">文本绑定字段</param>
        /// <param name="valueMember">值绑定字段</param>
        public static void BindTreeView(DataTable dt, TreeView view, String parentId, String textMember, String valueMember)
        {
            DataView dv = new DataView(dt, String.Format("{0} = 0", parentId), "", DataViewRowState.Unchanged);
            view.Nodes.Clear();
            foreach (DataRowView drv in dv)
            {
                TreeNode n = new TreeNode();
                n.Text = Convert.ToString(drv[textMember]);
                n.Value = Convert.ToString(drv[valueMember]);
                view.Nodes.Add(n);
                BindChildTreeNode(dt, n, parentId, textMember, valueMember);
            }
        }
        /// <summary>
        /// 生成树结构信息
        /// </summary>
        /// <param name="dt">要生成的表名</param>
        /// <param name="field">主键字段</param>
        /// <param name="prev_field">上级关联字段</param>
        /// <param name="captionField">标题字段</param>
        /// <returns>说明：生成后的表多了一列字段：caption,用于绑定下拉框控件</returns>
        public static DataTable GetTreeTable(DataTable dt, String field, String prev_field, String captionField)
        {
            DataTable _dt = dt.Copy();
            _dt.Columns.Add("Full_Caption", typeof(String));
            DataView dv = new DataView(_dt, prev_field + " = 0", "", DataViewRowState.Unchanged);
            foreach (DataRowView drv in dv)
            {
                if (drv == null || drv.Row == null) continue;
                drv["Full_Caption"] = "└" + drv[captionField];
                dv.Table.AcceptChanges();
                createChildMenu(Convert.ToString(drv[field]), _dt, field, prev_field, captionField, 1);
            }
            return _dt;
        }
        /// <summary>
        /// 以树形结构形式绑定列表控件
        /// </summary>
        /// <param name="ctl">要绑定的控件</param>
        /// <param name="dt">要绑定的数据源，必须为已生成树结构的数据表[GetTreeTable]</param>
        /// <param name="field">要绑定的主键字段</param>
        /// <param name="prev_field">要绑定的上级关联字段</param>
        public static void BindTreeList(ListControl ctl, DataTable dt, String field, String prev_field)
        {
            BindTreeList(ctl, "0", dt, field, prev_field);
        }
        /// <summary>
        /// 绑定分类目录树
        /// </summary>
        /// <param name="view">视图控件</param>
        /// <param name="dt">要绑定的数据表</param>
        /// <param name="field">主键字段</param>
        /// <param name="prev_field">上级关联字段</param>
        /// <param name="captionField">标题字段</param>
        public static void BindTreeView(TreeView view, DataTable dt, String field, String prev_field, String captionField)
        {
            DataView dv = new DataView(dt, prev_field + " = 0", "", DataViewRowState.Unchanged);
            view.Nodes.Clear();
            TreeNode node = new TreeNode("顶级分类", "0");
            node.ImageUrl = "/images/icons/home2.gif";
            view.Nodes.AddAt(0, node);
            foreach (DataRowView drv in dv)
            {
                String value = Convert.ToString(drv[field]);
                String caption = Convert.ToString(drv[captionField]);
                TreeNode n = new TreeNode();
                n.Text = caption;
                n.Value = value;
                n.ImageUrl = "/images/icons/folder.gif";
                n.SelectAction = TreeNodeSelectAction.SelectExpand;
                node.ChildNodes.Add(n);
                BindChildTreeNode(value, dt, field, prev_field, captionField, n);
            }
        }
        /// <summary>
        /// 设置目录树选择中节点
        /// </summary>
        /// <param name="view"></param>
        /// <param name="value"></param>
        public static void SetTreeValue(TreeView view, String value)
        {
            foreach (TreeNode node in view.Nodes)
            {
                if (node.Value == value)
                {
                    node.Select();
                    return;
                }
                if (SetTreeValue(node, value))
                {
                    return;
                }
            }
        }
        /// <summary>
        /// 根据值查找节点
        /// </summary>
        /// <param name="nodes">节点集合</param>
        /// <param name="value">节点值</param>
        /// <returns>不存在则返回null</returns>
        public static TreeNode GetTreeForValue(TreeNodeCollection nodes, String value)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Value == value)
                    return node;
                return GetTreeForValue(node.ChildNodes, value);
            }
            return null;
        }

        #endregion

        #region 私有方法

        protected static void BindChildTreeNode(DataTable dt, TreeNode node, String parentId, String textMember, String valueMember)
        {
            String id = node.Value;
            DataView dv = new DataView(dt, String.Format(
                "{0} = {1}", parentId, id), "", DataViewRowState.Unchanged);
            foreach (DataRowView drv in dv)
            {
                TreeNode nn = new TreeNode();
                nn.Text = Convert.ToString(drv[textMember]);
                nn.Value = Convert.ToString(drv[valueMember]);
                node.ChildNodes.Add(nn);
                BindChildTreeNode(dt, nn, parentId, textMember, valueMember);
            }
        }

        private static void createChildMenu(String id, DataTable dt, String field, String prev_field, String captionField, int num)
        {
            String jd = GetNodePathText(num);
            jd = CommonUtils.TextHelper.HtmlDecode(jd);
            DataView dv = new DataView(dt, prev_field + " = " + id, "", DataViewRowState.Unchanged);
            foreach (DataRowView drv in dv)
            {
                if (drv == null || drv.Row == null) continue;
                drv["Full_Caption"] = jd + drv[captionField];
                dv.Table.AcceptChanges();
                createChildMenu(Convert.ToString(drv[field]), dt, field, prev_field, captionField, num + 1);
            }
        }

        protected static void BindTreeList(ListControl ctl, String prev_id, DataTable dt, String field, String prev_field)
        {
            DataView dv = new DataView(dt, prev_field + " = " + prev_id, "", DataViewRowState.Unchanged);
            foreach (DataRowView drv in dv)
            {
                if (drv == null || drv.Row == null) continue;
                ctl.Items.Add(new ListItem(Convert.ToString(drv["Full_Caption"]), Convert.ToString(drv[field])));
                BindTreeList(ctl, Convert.ToString(drv[field]), dt, field, prev_field);
            }
        }

        protected static void BindChildTreeNode(String id, DataTable dt, String field, String prev_field, String captionField, TreeNode n)
        {
            DataView dv = new DataView(dt, prev_field + " = " + id, "", DataViewRowState.Unchanged);
            foreach (DataRowView drv in dv)
            {
                String value = Convert.ToString(drv[field]);
                String caption = Convert.ToString(drv[captionField]);
                TreeNode nn = new TreeNode();
                nn.Text = caption;
                nn.Value = value;
                Object num = dt.Compute("COUNT(" + field + ")", prev_field + "=" + value);
                if (MathHelper.GetIntValue(num) > 0)
                    nn.ImageUrl = "/images/icons/folder.gif";
                else
                    nn.ImageUrl = "/images/icons/tags.gif";
                nn.SelectAction = TreeNodeSelectAction.SelectExpand;
                n.ChildNodes.Add(nn);
                BindChildTreeNode(value, dt, field, prev_field, captionField, nn);
            }
        }

        protected static String GetNodePathText(int num)
        {
            if (num == 0) return "└";
            if (num == 1) return "&nbsp;&nbsp;├";
            if (num == 2) return "&nbsp;&nbsp;│├";
            if (num == 3) return "&nbsp;&nbsp;││├";
            if (num == 4) return "&nbsp;&nbsp;│││├";
            if (num == 5) return "&nbsp;&nbsp;││││├";
            if (num == 6) return "&nbsp;&nbsp;│││││├";
            if (num == 7) return "&nbsp;&nbsp;││││││├";
            if (num == 8) return "&nbsp;&nbsp;│││││││├";
            return "&nbsp;&nbsp;├";
        }

        private static Boolean SetTreeValue(TreeNode node, String value)
        {
            foreach (TreeNode n in node.ChildNodes)
            {
                if (n.Value == value)
                {
                    n.Select();
                    return true;
                }
                if (n.ChildNodes.Count > 0)
                {
                    Boolean b = SetTreeValue(n, value);
                    if (b) return true;
                }
            }
            return false;
        }

        #endregion

        #endregion

        #region 表单工具

        internal static Object CreateInstance(Type type)
        {
            Type makeType = type.IsGenericType ? type.GetGenericArguments()[0] : type;
            return Activator.CreateInstance(makeType);
        }
        //private static Type GetMakeType(Type baseType)
        //{
        //    Type makeType = baseType.IsGenericType ? baseType.GetGenericArguments()[0] : baseType;
        //    return makeType;
        //}
        public static void FindControl(Control ctl, List<IKeyValue> data)
        {
            foreach (Control item in ctl.Controls)
            {
                if (!String.IsNullOrEmpty(item.ID) && (item is IControls || item is ITextControl || item is HtmlInputControl))
                {
                    data.Add(new IKeyValue
                    {
                        Key = item.ID,
                        Value = GetControlValue(item)
                    });
                    continue;
                }
                if (item.HasControls())
                {
                    FindControl(item, data);
                }
            }
        }

        /// <summary>
        /// 将表单数据转换为模式(创建模式)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <returns></returns>
        //public static T GetFormValue<T>(Page page)
        //{
        //    Type makeType = GetMakeType(typeof(T));
        //    Object instance = Activator.CreateInstance(makeType);
        //    SetValue(ref instance);
        //    return (T)instance;
        //}
        /// <summary>
        /// 将表单数据转换为模式(编辑模式)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        //public static T GetFormValue<T>(Object instance)
        //{
        //    //T temp = default(T);
        //    if (instance == null)
        //    {
        //        Type makeType = GetMakeType(typeof(T));
        //        instance = Activator.CreateInstance(makeType);
        //    }
        //    SetValue(instance);
        //    return (T)instance;
        //}
        //private static void SetValue(Object instance)
        //{
        //    Page page = HttpContext.Current.CurrentHandler as Page;
        //    Type type = instance.GetType();
        //    PropertyInfo[] attributes = type.GetProperties();

        //    //List<KeyValue> fields = new List<KeyValue>();
        //    //PropertyInfo VirtualFields = type.GetProperty("VirtualFields", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        //    //if (VirtualFields != null)
        //    //{
        //    //    FindControl(page, fields);
        //    //}

        //    foreach (PropertyInfo item in attributes)
        //    {
        //        //if (item.PropertyType.IsGenericType)
        //        //{
        //        //    Object value = SetValue(item.PropertyType, page);
        //        //    Object instance = Activator.CreateInstance(item.PropertyType);
        //        //    item.PropertyType.GetMethod("Add").Invoke(instance, new[] { value });
        //        //    ConvertSetValue(element, item, instance);
        //        //}
        //        //else
        //        //{
        //        if (!item.CanWrite) continue;
        //        String name = item.Name;

        //        FieldAttribute[] attribute = (FieldAttribute[])item.GetCustomAttributes(typeof(FieldAttribute), false);
        //        if (attribute != null && attribute.Length > 0)
        //        {
        //            if (!String.IsNullOrEmpty(attribute[0].ControlName)) name = attribute[0].ControlName;
        //        }

        //        Control ctl = page.FindControl(name);
        //        if (ctl == null) continue;

        //        Object value = GetControlValue(ctl);
        //        ConvertSetValue(instance, item, value);
        //        //}
        //    }
        //}

        /// <summary>
        /// 将模型数据还原到表单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="data"></param>
        //public static void SetFormValue(Object data)
        //{
        //    Page page = HttpContext.Current.CurrentHandler as Page;
        //    Type type = data.GetType();
        //    foreach (PropertyInfo item in type.GetProperties())
        //    {
        //        String name = item.Name;
        //        FieldAttribute[] field = (FieldAttribute[])item.GetCustomAttributes(typeof(FieldAttribute), false);
        //        if (field != null && field.Length > 0)
        //        {
        //            FieldAttribute att = field[0];
        //            if (!String.IsNullOrEmpty(att.ControlName)) name = att.ControlName;
        //        }
        //        Control ctl = page.FindControl(name);
        //        if (ctl == null) continue;
        //        /*--Object value = item.GetValue(data, null);*/
        //        Object value = item.FastGetValue(data);
        //        SetControlValue(ctl, value);
        //    }
        //}
        /// <summary>
        /// 清空表单数据
        /// </summary>
        /// <param name="ctl"></param>
        //public static void ClearFromValue(Control ctl)
        //{
        //    if (ctl is IAttributeAccessor)
        //    {
        //        String val = ((IAttributeAccessor)ctl).GetAttribute("clear");
        //        if (!String.IsNullOrEmpty(val) & !Convert.ToBoolean(val)) return;
        //    }
        //    if (ctl is WebControl)
        //    {
        //        if (!((WebControl)ctl).Enabled) return;
        //    }

        //    if (ctl is IControls)
        //    {
        //        ((IControls)ctl).SetValue(null);
        //    }
        //    else if (ctl is System.Web.UI.WebControls.TextBox)
        //    {
        //        ((System.Web.UI.WebControls.TextBox)ctl).Text = "";
        //    }
        //    else if (ctl is HtmlInputText)
        //    {
        //        ((HtmlInputText)ctl).Value = "";
        //    }
        //    else if (ctl is HtmlTextArea)
        //    {
        //        ((HtmlTextArea)ctl).Value = "";
        //    }

        //    if (ctl.HasControls())
        //    {
        //        foreach (Control c in ctl.Controls)
        //        {
        //            ClearFromValue(c);
        //        }
        //    }
        //}

        /// <summary>
        /// 获取表单控件数据值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="att"></param>
        /// <param name="ctl"></param>
        internal static Object GetControlValue(Control ctl)
        {
            if (ctl is IControls)
            {
                return ((IControls)ctl).GetValue();
            }

            #region HtmlControls

            else if (ctl is HtmlInputText)
            {
                return ((HtmlInputText)ctl).Value;
            }
            else if (ctl is HtmlInputHidden)
            {
                return ((HtmlInputHidden)ctl).Value;
            }
            else if (ctl is HtmlTextArea)
            {
                return ((HtmlTextArea)ctl).Value;
            }
            else if (ctl is HtmlInputCheckBox)
            {
                return ((HtmlInputCheckBox)ctl).Checked;
            }
            else if (ctl is HtmlInputRadioButton)
            {
                return ((HtmlInputRadioButton)ctl).Checked;
            }
            else if (ctl is HtmlInputPassword)
            {
                String value = ((HtmlInputPassword)ctl).Value.Trim();
                if (String.IsNullOrEmpty(value))
                    return null;
                else
                    return EncryptHelper.MD5Upper32(value);
            }
            else if (ctl is HtmlSelect)
            {
                return ((HtmlSelect)ctl).Value;
            }
            else if (ctl is HtmlInputImage)
            {
                return ((HtmlInputImage)ctl).Value;
            }
            else if (ctl is HtmlInputFile)
            {
                return FileHelper.SaveUpFile(((HtmlInputFile)ctl).PostedFile);
            }

            #endregion

            #region WebControls

            //else if (ctl is System.Web.UI.WebControls.TextBox)
            //{
            //    System.Web.UI.WebControls.TextBox text = ((System.Web.UI.WebControls.TextBox)ctl);
            //    if (text.TextMode == TextBoxMode.Password)
            //    {
            //        if (String.IsNullOrEmpty(text.Text.Trim()))
            //            return null;
            //        else
            //            return EncryptHelper.MD5Upper32(text.Text);
            //    }
            //    else
            //    {
            //        return text.Text;
            //    }
            //}
            //else if (ctl is System.Web.UI.WebControls.CheckBox)
            //{
            //    return ((System.Web.UI.WebControls.CheckBox)ctl).Checked;
            //}
            //else if (ctl is System.Web.UI.WebControls.CheckBoxList)
            //{
            //    System.Web.UI.WebControls.CheckBoxList checkboxlist = ctl as System.Web.UI.WebControls.CheckBoxList;
            //    List<String> _value = new List<String>();
            //    foreach (ListItem item in checkboxlist.Items)
            //    {
            //        if (item.Selected) _value.Add(item.Value);
            //    }
            //    return String.Join(",", _value.ToArray());
            //}
            //else if (ctl is RadioButton)
            //{
            //    return ((RadioButton)ctl).Checked;
            //}
            //else if (ctl is System.Web.UI.WebControls.RadioButtonList)
            //{
            //    System.Web.UI.WebControls.RadioButtonList radiobuttonlist = ctl as System.Web.UI.WebControls.RadioButtonList;
            //    String _value = String.Empty;
            //    foreach (ListItem item in radiobuttonlist.Items)
            //    {
            //        if (item.Selected)
            //        {
            //            _value = item.Value;
            //            break;
            //        }
            //    }
            //    return _value;
            //}
            //else if (ctl is System.Web.UI.WebControls.ListBox)
            //{
            //    System.Web.UI.WebControls.ListBox listbox = ctl as System.Web.UI.WebControls.ListBox;
            //    if (listbox.SelectionMode == ListSelectionMode.Single)
            //    {
            //        return listbox.SelectedValue;
            //    }
            //    else
            //    {
            //        List<String> _value = new List<String>();
            //        foreach (ListItem item in listbox.Items)
            //        {
            //            if (item.Selected) _value.Add(item.Value);
            //        }
            //        return String.Join(",", _value.ToArray());
            //    }
            //}
            //else if (ctl is System.Web.UI.WebControls.DropDownList)
            //{
            //    return ((System.Web.UI.WebControls.DropDownList)ctl).SelectedValue;
            //}
            //else if (ctl is System.Web.UI.WebControls.HiddenField)
            //{
            //    return ((System.Web.UI.WebControls.HiddenField)ctl).Value;
            //}
            //else if (ctl is System.Web.UI.WebControls.FileUpload)
            //{
            //    return FileHelper.SaveUpFile(((System.Web.UI.WebControls.FileUpload)ctl).PostedFile);
            //}
            //else if (ctl is Image)
            //{
            //    return ((Image)ctl).ImageUrl;
            //}

            #endregion

            #region 自定义控件

            //else if (ctl is ColorPicker)
            //{
            //    return ((ColorPicker)ctl).Color;
            //}

            #endregion

            return null;
        }
        /// <summary>
        /// 设置表单控件数据值
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static void SetControlValue(Control ctl, Object value)
        {
            if (ctl is IControls)
            {
                IControls c = ctl as IControls;
                if (value is Enum && value.ToString() == "ALL")
                {
                    List<String> val = new List<String>();
                    foreach (String s in Enum.GetNames(value.GetType()))
                    {
                        if (s == "None") continue;
                        val.Add(s);
                    }
                    value = String.Join(",", val.ToArray());
                }
                c.SetValue(value);
            }

            #region HtmlControls

            else if (ctl is HtmlInputText)
            {
                ((HtmlInputText)ctl).Value = Convert.ToString(value);
            }
            else if (ctl is HtmlInputHidden)
            {
                ((HtmlInputHidden)ctl).Value = Convert.ToString(value);
            }
            else if (ctl is HtmlTextArea)
            {
                ((HtmlTextArea)ctl).Value = Convert.ToString(value);
            }
            else if (ctl is HtmlInputCheckBox)
            {
                ((HtmlInputCheckBox)ctl).Checked = Convert.ToBoolean(value);
            }
            else if (ctl is HtmlInputRadioButton)
            {
                ((HtmlInputRadioButton)ctl).Checked = Convert.ToBoolean(value);
            }
            else if (ctl is HtmlInputPassword)
            {

            }
            else if (ctl is HtmlSelect)
            {
                ((HtmlSelect)ctl).Value = Convert.ToString(value);
            }
            else if (ctl is HtmlInputImage)
            {
                ((HtmlInputImage)ctl).Value = Convert.ToString(value);
            }
            else if (ctl is HtmlInputFile)
            {

            }

            #endregion

            #region WebControls

            //else if (ctl is System.Web.UI.WebControls.Label)
            //{
            //    ((System.Web.UI.WebControls.Label)ctl).Text = Convert.ToString(value);
            //}
            //else if (ctl is System.Web.UI.WebControls.TextBox)
            //{
            //    System.Web.UI.WebControls.TextBox text = (System.Web.UI.WebControls.TextBox)ctl;
            //    if (text.TextMode != TextBoxMode.Password)
            //    {
            //        text.Text = Convert.ToString(value);
            //    }
            //}
            //else if (ctl is System.Web.UI.WebControls.CheckBox)
            //{
            //    ((System.Web.UI.WebControls.CheckBox)ctl).Checked = Convert.ToBoolean(value);
            //}
            //else if (ctl is System.Web.UI.WebControls.CheckBoxList)
            //{
            //    System.Web.UI.WebControls.CheckBoxList checkboxlist = ctl as System.Web.UI.WebControls.CheckBoxList;
            //    String[] split = Convert.ToString(value).Split(',');
            //    foreach (String v in split)
            //    {
            //        foreach (ListItem item in checkboxlist.Items)
            //        {
            //            if (item.Value == v)
            //            {
            //                item.Selected = true;
            //                break;
            //            }
            //        }
            //    }
            //}
            //else if (ctl is RadioButton)
            //{
            //    ((RadioButton)ctl).Checked = Convert.ToBoolean(value);
            //}
            //else if (ctl is System.Web.UI.WebControls.RadioButtonList)
            //{
            //    System.Web.UI.WebControls.RadioButtonList radiobuttonlist = ctl as System.Web.UI.WebControls.RadioButtonList;
            //    String val = Convert.ToString(value);

            //    foreach (ListItem item in radiobuttonlist.Items)
            //    {
            //        if (item.Value == val)
            //        {
            //            item.Selected = true;
            //            break;
            //        }
            //    }
            //}
            //else if (ctl is System.Web.UI.WebControls.ListBox)
            //{
            //    System.Web.UI.WebControls.ListBox listbox = ctl as System.Web.UI.WebControls.ListBox;
            //    if (listbox.SelectionMode == ListSelectionMode.Single)
            //    {
            //        listbox.SelectedValue = Convert.ToString(value);
            //    }
            //    else
            //    {
            //        String[] split = Convert.ToString(value).Split(',');
            //        foreach (String v in split)
            //        {
            //            foreach (ListItem item in listbox.Items)
            //            {
            //                if (item.Value == v)
            //                {
            //                    item.Selected = true;
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}
            //else if (ctl is System.Web.UI.WebControls.DropDownList)
            //{
            //    String _value = Convert.ToString(value);
            //    System.Web.UI.WebControls.DropDownList drop = ((System.Web.UI.WebControls.DropDownList)ctl);
            //    foreach (ListItem item in drop.Items)
            //    {
            //        if (item.Value == _value)
            //        {
            //            drop.SelectedValue = _value;
            //            break;
            //        }
            //    }
            //}
            //else if (ctl is System.Web.UI.WebControls.HiddenField)
            //{
            //    ((System.Web.UI.WebControls.HiddenField)ctl).Value = Convert.ToString(value);
            //}
            //else if (ctl is System.Web.UI.WebControls.FileUpload)
            //{

            //}
            //else if (ctl is Image)
            //{
            //    ((Image)ctl).ImageUrl = Convert.ToString(value);
            //}
            #endregion

            #region 自定义控件

            //else if (ctl is ColorPicker)
            //{
            //    ((ColorPicker)ctl).Color = Convert.ToString(value);
            //}

            #endregion
        }
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="att"></param>
        /// <param name="ctl"></param>
        internal static void ConvertSetValue(Object instance, PropertyInfo att, Object value)
        {
            if (value == null || value == DBNull.Value) return;
            Type valueType = value.GetType();

            if ((valueType == att.PropertyType))//数据类型相同
            {
                /*--att.SetValue(instance, value, null);*/
                att.FastSetValue(instance, value);
            }
            else if (att.PropertyType.IsGenericType)//泛型
            {
                //Type innertype = att.PropertyType.GetGenericTypeDefinition();
                //泛型Nullable<> 可空的值类型 如int?、long?
                if (att.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    //NullableConverter nullableConverter = new NullableConverter(att.PropertyType);
                    //att.SetValue(instance, Convert.ChangeType(value, nullableConverter.UnderlyingType), null);

                    /*--att.SetValue(instance, Convert.ChangeType(value, Nullable.GetUnderlyingType(att.PropertyType)), null);*/
                    att.FastSetValue(instance, Convert.ChangeType(value, Nullable.GetUnderlyingType(att.PropertyType)));
                }
                else
                {
                    Object list = Activator.CreateInstance(att.PropertyType);
                    MethodInfo add = att.PropertyType.GetMethod("Add");
                    MethodInfo contains = att.PropertyType.GetMethod("Contains");

                    if (valueType == typeof(String))
                    {
                        String val = Convert.ToString(value);
                        foreach (String k in val.Split(','))
                        {
                            Boolean chk = (Boolean)contains.FastInvoke(list, new Object[] { k });
                            /*--add.Invoke(list, new Object[] { k });*/
                            if (!chk) add.FastInvoke(list, new Object[] { k });
                        }
                    }
                    //else
                    //{
                    //    add.Invoke(list, new Object[] { value });
                    //}

                    /*--att.SetValue(instance, list, null);*/
                    att.FastSetValue(instance, list);
                }
            }
            else if (att.PropertyType.IsEnum)//枚举类型
            {
                if (valueType == typeof(String))
                {
                    /*--att.SetValue(instance, Enum.Parse(att.PropertyType, Convert.ToString(value), true), null);*/
                    att.FastSetValue(instance, Enum.Parse(att.PropertyType, Convert.ToString(value), true));
                }
                else
                {
                    /*--att.SetValue(instance, Enum.ToObject(att.PropertyType, value), null);*/
                    att.FastSetValue(instance, Enum.ToObject(att.PropertyType, value));
                }
            }
            else if (valueType == typeof(String) && att.PropertyType == typeof(Guid))//Guid
            {
                /*--att.SetValue(instance, new Guid(Convert.ToString(value)), null);*/
                att.FastSetValue(instance, new Guid(Convert.ToString(value)));
            }
            else if (valueType == typeof(String) && att.PropertyType == typeof(DateTime))//DateTime
            {
                DateTime time = DateTime.Now;
                DateTime.TryParse(Convert.ToString(value), out time);
                if (time.Year == 1) time = DateTime.Now;
                att.FastSetValue(instance, time);
            }
            else
            {
                if (valueType == typeof(String) && String.IsNullOrEmpty(value as String)) return;
                //数据类型不同时尝试转换
                /*--att.SetValue(instance, Convert.ChangeType(value, att.PropertyType), null);*/
                att.FastSetValue(instance, Convert.ChangeType(value, att.PropertyType));
            }
        }

        #endregion
    }

}
