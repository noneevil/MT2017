using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using WebSite.Models;

namespace WebSite.Core
{
    /// <summary>
    /// 权限控制辅助类
    /// </summary>
    public class ActionTypeHelper
    {
        public static Dictionary<String, String> ActionList()
        {
            Dictionary<String, String> dic = new Dictionary<String, String>();
            foreach (FieldInfo field in typeof(ActionType).GetFields())
            {
                //if (field.Name == "None") continue;
                Object[] atts = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (atts.Length > 0)
                {
                    DescriptionAttribute att = (DescriptionAttribute)atts[0];
                    if (att == null) continue;
                    dic.Add(field.Name, att.Description);// field.Name + "(" + att.Description + ")");
                }
            }
            return dic;
        }

        /// <summary>
        /// 是否拥有创建权限
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean IsCreat(ActionType value)
        {
            return (value & ActionType.Create) == ActionType.Create;
        }
        /// <summary>
        /// 是否拥有查看权限
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean IsView(ActionType value)
        {
            return (value & ActionType.View) == ActionType.View;
        }
        /// <summary>
        /// 是否拥有编辑修改权限
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean IsEdit(ActionType value)
        {
            return (value & ActionType.Edit) == ActionType.Edit;
        }
        /// <summary>
        /// 是否拥有删除权限
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean IsDelete(ActionType value)
        {
            return (value & ActionType.Delete) == ActionType.Delete;
        }
        /// <summary>
        /// 是否拥有开启各种设置权限
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean IsSetting(ActionType value)
        {
            return (value & ActionType.Setting) == ActionType.Setting;
        }
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="oldacl"></param>
        /// <param name="newacl"></param>
        /// <returns></returns>
        public static ActionType AddACLOptions(ActionType oldacl, ActionType newacl)
        {
            return oldacl | newacl;
        }
        /// <summary>
        /// 移出权限
        /// </summary>
        /// <param name="acl"></param>
        /// <param name="remove"></param>
        /// <returns></returns>
        public static ActionType RemoveACLoptions(ActionType acl, ActionType remove)
        {
            return (acl & (ActionType.ALL ^ remove));
        }
        /// <summary>
        /// 获取值所包含的项
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<ActionType> GetValueItem(ActionType value)
        {
            List<ActionType> list = new List<ActionType>();
            foreach (ActionType item in Enum.GetValues(typeof(ActionType)))
            {
                if ((value & item) == item)
                {
                    list.Add(item);
                }
            }
            return list;
        }
    }
}
