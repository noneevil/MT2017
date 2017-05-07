using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommonUtils
{
    /// <summary>
    /// 简单反射处理类
    /// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        /// 搜索指定接口所在程序集，并返回在程序集中定义并实现了此接口的类型
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <returns></returns>
        public static Type[] GetTypes<T>()
        {
            return GetTypes(Assembly.GetAssembly(typeof(T)), typeof(T));
        }
        /// <summary>
        /// 在指定程序集中搜索指定接口返回在程序集中定义并实现了此接口的类型
        /// </summary>
        /// <param name="assembly">要搜索的程序集</param>
        /// <param name="iType">接口</param>
        /// <returns></returns>
        public static Type[] GetTypes(Assembly assembly, Type iType)
        {
            Type[] types = assembly.GetTypes().Where(t => t.IsInterface == false).ToArray();
            types = types.Where(t => t.GetInterface(iType.ToString()) != null).ToArray();
            return types;
        }
        /// <summary>
        /// 搜索指定接口所在程序集，并返回实现了指定接口和Attribute的类型
        /// </summary>
        /// <param name="iType">接口</param>
        /// <param name="attributeType">Attribute</param>
        /// <returns></returns>
        public static Type[] GetTypes(Type iType, Type attributeType)
        {
            Type[] types = GetTypes(Assembly.GetAssembly(iType), iType, attributeType);
            return types;
        }
        /// <summary>
        /// 搜索指定程序集中实现了指定接口和Attribute的类型
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="iType">接口</param>
        /// <param name="attributeType">Attribute</param>
        /// <returns></returns>
        public static Type[] GetTypes(Assembly assembly, Type iType, Type attributeType)
        {
            Type[] types = GetTypes(assembly, iType);
            types = types.Where(t => t.IsDefined(attributeType, false)).ToArray();
            return types;
        }
        /// <summary>
        /// 搜索指定接口所在程序集，并返回所有实现了此接口的实例集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetInstance<T>()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(T));
            return GetInstance<T>(assembly);
        }
        /// <summary>
        /// 搜索指定程序集，返回所有实现了指定接口的类型的实例集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static T[] GetInstance<T>(Assembly assembly)
        {
            Type[] types = GetTypes(assembly, typeof(T));
            List<T> list = new List<T>();
            foreach (Type type in types)
            {
                list.Add((T)assembly.CreateInstance(type.ToString()));
            }
            return list.ToArray();
        }

        protected static Hashtable _Assemblies;
        /// <summary>
        /// 动态调用DLL
        /// </summary>
        /// <param name="DllFileName">dll名称</param>
        /// <param name="NameSpace">命名空间</param>
        /// <param name="ClassName">类名</param>
        /// <param name="MethodName">方法名</param>
        /// <param name="ObjArrayParams">参数数组</param>
        /// <returns></returns>
        private static Object InvokeMember(String DllFileName, String NameSpace, String ClassName, String MethodName, Object[] ObjArrayParams)
        {
            try
            {
                //载入程序集 
                Assembly DllAssembly = null;
                if (String.IsNullOrEmpty(DllFileName))
                {
                    DllAssembly = GetAssembly(NameSpace);
                }
                else
                {
                    DllAssembly = Assembly.LoadFrom(DllFileName);
                }
                //查找类型
                Type DllType = DllAssembly.GetType(ClassName);
                if (DllType == null)
                {
                    DllAssembly = (Assembly)_Assemblies["mscorlib"];
                    DllType = DllAssembly.GetType(ClassName);
                }
                if (DllType == null) return null; ;

                MethodInfo[] MyMethod = DllType.GetMethods(BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
                foreach (MethodInfo item in MyMethod)
                {
                    if (item.Name.ToLower() == MethodName.ToLower())
                    {
                        if (item.IsStatic)
                        {
                            //静态方法
                            return DllType.InvokeMember(MethodName, BindingFlags.Default | BindingFlags.InvokeMethod, null, null, ObjArrayParams);
                        }
                        else
                        {
                            //动态方法
                            Object mObject = Activator.CreateInstance(DllType);
                            return DllType.InvokeMember(MethodName, BindingFlags.Default | BindingFlags.InvokeMethod, null, mObject, ObjArrayParams);
                        }
                    }
                }
                if (ObjArrayParams == null || ObjArrayParams.Length == 0)
                {
                    PropertyInfo pi = DllType.GetProperty(MethodName);
                    if (pi != null)
                    {
                        return pi.GetValue(DllType, null);
                    }
                }
                return null; ;
            }
            catch (Exception ex)
            {
                List<String> list = new List<String>();
                if (ObjArrayParams != null)
                {
                    foreach (Object item in ObjArrayParams)
                    {
                        list.Add(Convert.ToString(item));
                    }
                }
                ErrorMsg.WriteError(ex, String.Format("方法调用:({0}.{1}) 参数列表：({2})", ClassName, MethodName, String.Join(",", list.ToArray())));
                return null;
            }
        }
        /// <summary>
        /// 返回指定类的属性
        /// </summary>
        /// <param name="DllFileName"></param>
        /// <param name="NameSpace"></param>
        /// <param name="ClassName"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private static Object GetProperty(String DllFileName, String NameSpace, String ClassName, String propertyName)
        {
            try
            {
                //载入程序集 
                Assembly DllAssembly = null;
                if (String.IsNullOrEmpty(DllFileName))
                {
                    DllAssembly = GetAssembly(NameSpace);
                }
                else
                {
                    DllAssembly = Assembly.LoadFrom(DllFileName);
                }
                //查找类型
                Type DllType = DllAssembly.GetType(ClassName);

                if (DllType == null)
                {
                    DllAssembly = (Assembly)_Assemblies["mscorlib"];
                    DllType = DllAssembly.GetType(ClassName);
                }

                if (DllType == null) return null; ;

                PropertyInfo pi = DllType.GetProperty(propertyName);
                if (pi != null)
                {
                    return pi.GetValue(DllType, null);
                }
                return null; ;
            }
            catch (Exception ex)
            {
                ErrorMsg.WriteError(ex, String.Format("属性调用:{1}.{0}", ClassName, propertyName));
                return null;
            }
        }
        /// <summary>
        /// 返回指定类的属性
        /// </summary>
        /// <param name="NameSpace"></param>
        /// <param name="ClassName"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Object GetProperty(String NameSpace, String ClassName, String propertyName)
        {
            String DllName = GetDLLName(NameSpace);
            return GetProperty(DllName, NameSpace, ClassName, propertyName);
        }
        /// <summary>
        /// 返回指定类的属性
        /// </summary>
        /// <param name="ClassName"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Object GetProperty(String ClassName, String propertyName)
        {
            if (String.IsNullOrEmpty(ClassName))
            { return null; ; }
            String NameSpace = GetNameSpace(ClassName);
            if (String.IsNullOrEmpty(NameSpace))
            { return null; ; }
            //String className = GetMethodName(ClassName);
            return GetProperty(NameSpace, ClassName, propertyName);
        }
        /// <summary>
        /// 返回指定类的属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Object GetProperty(String propertyName)
        {
            if (String.IsNullOrEmpty(propertyName))
            { return null; ; }
            String ClassName = GetNameSpace(propertyName);
            if (String.IsNullOrEmpty(ClassName))
            { return null; ; }
            if (ClassName.IndexOf('.') < 0)
            {
                ClassName = GetFullClassName(ClassName);
            }
            String _propertyName = GetMethodName(propertyName);
            return GetProperty(ClassName, _propertyName);
        }
        /// <summary>
        /// 动态调用并执行方法名
        /// </summary>
        /// <param name="NameSpace"></param>
        /// <param name="ClassName"></param>
        /// <param name="MethodName"></param>
        /// <param name="ObjArrayParams"></param>
        /// <returns></returns>
        public static Object InvokeMember(String NameSpace, String ClassName, String MethodName, Object[] ObjArrayParams)
        {
            String DllName = GetDLLName(NameSpace);
            return InvokeMember(DllName, NameSpace, ClassName, MethodName, ObjArrayParams);
        }
        /// <summary>
        /// 动态调用并执行方法名
        /// </summary>
        /// <param name="ClassName"></param>
        /// <param name="MethodName"></param>
        /// <param name="ObjArrayParams"></param>
        /// <returns></returns>
        public static Object InvokeMember(String ClassName, String MethodName, Object[] ObjArrayParams)
        {
            if (String.IsNullOrEmpty(ClassName))
            { return null; ; }
            String NameSpace = GetNameSpace(ClassName);
            if (String.IsNullOrEmpty(NameSpace))
            { return null; ; }
            return InvokeMember(NameSpace, ClassName, MethodName, ObjArrayParams);
        }
        /// <summary>
        /// 动态调用并执行方法名
        /// </summary>
        /// <param name="MethodName"></param>
        /// <param name="ObjArrayParams"></param>
        /// <returns></returns>
        public static Object InvokeMember(String MethodName, Object[] ObjArrayParams)
        {
            if (String.IsNullOrEmpty(MethodName))
            { return null; ; }
            MethodName = GetFullMethodName(MethodName);
            String ClassName = GetNameSpace(MethodName);
            if (String.IsNullOrEmpty(ClassName))
            { return null; ; }
            if (ClassName.IndexOf('.') < 0)
            {
                ClassName = GetFullClassName(ClassName);
            }
            String methodName = GetMethodName(MethodName);
            return InvokeMember(ClassName, methodName, ObjArrayParams);
        }
        /// <summary>
        /// 取得指定命名空间的程序集名称
        /// </summary>
        /// <param name="NameSpace"></param>
        /// <returns></returns>
        protected static String GetDLLName(String NameSpace)
        {
            //DLL默认目录
            String DllPath = System.Web.HttpContext.Current.Server.MapPath("~") + "Bin\\";
            Hashtable ht = new Hashtable();
            //ht.Add("System", "system.dll");
            //ht.Add("System.Configuration", "system.dll");
            //ht.Add("System.Collections", "system.dll");
            //ht.Add("System.Collections.Generic", "system.dll");
            //ht.Add("System.Web", "System.Web.dll");
            //ht.Add("System.Web.UI", "System.Web.dll");
            //ht.Add("System.Web.UI.HtmlControls", "System.Web.dll");
            //ht.Add("System.Web.UI.WebControls", "System.Web.dll");
            //ht.Add("System.Data", "System.Data.dll");
            //ht.Add("System.IO", "system.dll");
            //ht.Add("System.Web.SessionState", "System.Web.dll");
            //ht.Add("CommonService", DllPath + "CommonService.dll");
            //ht.Add("CMSIDB", DllPath + "CMSIDB.dll");
            //ht.Add("SiteTemplate", DllPath + "SiteTemplate.dll");
            //ht.Add("SiteTemplate.Controls", DllPath + "SiteTemplate.dll");
            //ht.Add("SiteTemplate.ControlUtility", DllPath + "SiteTemplate.dll");
            //ht.Add("SiteTemplate.Template", DllPath + "SiteTemplate.dll");
            //ht.Add("Web.Interface", DllPath + "Interface.dll");
            //ht.Add("Hutao.Configuration", DllPath + "Hutao.dll");
            //ht.Add("Hutao.Web.Utils", DllPath + "Hutao.dll");
            //ht.Add("URLRewriter", DllPath + "URLRewriter.dll");
            //ht.Add("URLRewriter.Config", DllPath + "URLRewriter.dll");
            //ht.Add("HUTAO.Pager", DllPath + "AspNetPager.dll");
            //ht.Add("CKEditor", DllPath + "CKEditor.dll");
            //ht.Add("CKFinder", DllPath + "CKFinder.dll");
            if (ht.ContainsKey(NameSpace))
            {
                return Convert.ToString(ht[NameSpace]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 返回当前类名的命名空间部分字符串
        /// </summary>
        /// <param name="className">完整类名</param>
        /// <returns></returns>
        protected static String GetNameSpace(String className)
        {
            if (className.IndexOf('.') > 0)
            {
                int index = className.LastIndexOf('.');
                return className.Substring(0, index);
            }
            return null;
        }
        /// <summary>
        /// 返回当前完整方法全名的方法名称部分字符串
        /// </summary>
        /// <param name="className">完整方法全名</param>
        protected static String GetMethodName(String FullMethodName)
        {
            if (FullMethodName.IndexOf('.') > 0)
            {
                int index = FullMethodName.LastIndexOf('.');
                int length = FullMethodName.Length - index - 1;
                return FullMethodName.Substring(index + 1, length);
            }
            return null;
        }
        /// <summary>
        /// 返回指定命名空间的程序集名称
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        protected static Assembly GetAssembly(String nameSpace)
        {
            if (_Assemblies == null || _Assemblies.Count == 0)
            {
                Assembly[] arrs = AppDomain.CurrentDomain.GetAssemblies();
                _Assemblies = new Hashtable();
                foreach (Assembly s in arrs)
                {
                    String name = s.FullName.Substring(0, s.FullName.IndexOf(','));
                    _Assemblies.Add(name, s);
                }
            }
            String _namespace = nameSpace.ToLower();
            if (_namespace == "system" || _namespace == "system.collections"
                || _namespace == "system.io" || _namespace == "system.text"
                || _namespace == "microsoft.win32")
            {
                return (Assembly)_Assemblies["mscorlib"];
            }

            IDictionaryEnumerator ide = _Assemblies.GetEnumerator();
            while (ide.MoveNext())
            {
                String s = ide.Key.ToString().ToLower();
                if (_namespace.Contains(s))
                    return (Assembly)ide.Value;
            }
            return null;
        }
        /// <summary>
        /// 取得当前类的全部路径及类名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected static String GetFullClassName(String name)
        {
            Hashtable ht = new Hashtable();
            ht.Add("datetime", "System.DateTime");
            ht.Add("file", "System.IO.File");
            ht.Add("directory", "System.IO.Directory");
            ht.Add("math", "System.Math");
            ht.Add("encoding", "System.Text.Encoding");
            ht.Add("httputility", "System.Web.HttpUtility");
            ht.Add("String", "System.String");
            ht.Add("db", "CMSIDB.db");
            ht.Add("admins", "Web.Interface.Admins");
            ht.Add("encrypt", "CommonService.Encrypt");
            ht.Add("config", "CommonService.config");
            ht.Add("weather", "CommonService.Weather");
            ht.Add("cookie", "CommonService.Cookie");
            ht.Add("cache", "CommonService.CacheEx");
            ht.Add("validation", "CommonService.Validation");
            if (ht.Contains(name.ToLower()))
            {
                return (String)ht[name.ToLower()];
            }
            else
            {
                return name;
            }
        }
        /// <summary>
        /// 取得当前方法的全部路径及方法名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected static String GetFullMethodName(String name)
        {
            Hashtable ht = new Hashtable();
            ht.Add("substring", "CommonService.Text.GetSubString");
            ht.Add("substringex", "CommonService.Text.GetSubStringEx");
            ht.Add("md5", "CommonService.Encrypt.MD5");
            ht.Add("format", "System.String.Format");
            ht.Add("executescalar", "CMSIDB.db.ExecuteScalar");
            ht.Add("urldecode", "System.Web.HttpUtility.UrlDecode");
            ht.Add("urlencode", "System.Web.HttpUtility.UrlEncode");
            ht.Add("sha1", "CommonService.Encrypt.SHA1");
            ht.Add("encrypt", "CommonService.Encrypt");
            ht.Add("htmldecode", "CommonService.Text.HtmlDecode");
            ht.Add("htmlencode", "CommonService.Text.HtmlEncode");
            ht.Add("getsiteurl", "CommonService.Text.GetWebSiteUrl");
            ht.Add("getwebsiteurl", "CommonService.Text.GetWebSiteUrl");
            ht.Add("getpath", "CommonService.Text.GetPath");
            ht.Add("clearhtml", "CommonService.Text.ClearHTML");
            ht.Add("checksql", "CommonService.Text.CheckSQL");
            ht.Add("clearjavascript", "CommonService.Text.ClearJavaScript");
            ht.Add("getweather", "CommonService.Weather.GetWeather");
            ht.Add("getsession", "CommonService.SessionEx.GetSession");
            ht.Add("getcookie", "CommonService.Cookie.GetValue");
            ht.Add("getip", "CommonService.common.GetClientIP");
            ht.Add("getquerystring", "CommonService.common.GetQueryString");
            ht.Add("getformfield", "CommonService.common.GetFormField");
            if (ht.Contains(name.ToLower()))
            {
                return (String)ht[name.ToLower()];
            }
            else
            {
                return name;
            }
        }
    }
}
