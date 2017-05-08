using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using WebSite.Models;
using System.ComponentModel;
using System.Reflection;
using MSSQLDB;
using System.Security;
using System.Runtime.InteropServices;
//http://msdn.microsoft.com/library/6a71f45d.aspx //C# Operators
//http://www.cnblogs.com/shanyou/archive/2006/11/16/562816.html C# Enum设计和使用的相关技巧 
//http://www.dotblogs.com.tw/chhuang/archive/2008/04/26/3514.aspx
//http://msdn.microsoft.com/en-us/library/system.flagsattribute.aspx
public partial class test : System.Web.UI.Page
{
    //ACLOptions.Create | ACLOptions.Delete | ACLOptions.Create 相加

    protected void Page_Load(object sender, EventArgs e)
    {
        SecureString s = new SecureString();
        s.AppendChar('1');
        s.AppendChar('2');
        s.AppendChar('3');
        Response.Write(Marshal.PtrToStringUni(Marshal.SecureStringToBSTR(s)));
        TextBox1.SetValue(DateTime.Now);
    }

    protected void AjaxHandler() {
         string ajax_target = Request.Form["ajax_target"];
         if (string.IsNullOrEmpty(ajax_target)) return;
 
        Control target = FindControl(ajax_target);
         if (target == null) return;
 
        MethodInfo method = target.GetType().GetMethod("IAjax");
         if (method == null) return;
 
        Response.Cache.SetNoStore();
 
        Response.Write("(");
         method.Invoke(target, null);
         Response.Write(")");
         Response.End();
     }
}