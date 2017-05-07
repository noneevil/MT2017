using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Net;
using System.Web.Services.Description;
using Microsoft.CSharp;
using System.Reflection;

namespace SiteService
{
    /// <summary>
    /// 动态调用web服务
    /// </summary>
    public class WebServiceHelper
    {
        #region InvokeWebService

        public static Object InvokeWebService(String url, String methodname, Object[] args)
        {
            return InvokeWebService(url, null, methodname, args);
        }

        public static Object InvokeWebService(String url, String classname, String methodname, Object[] args)
        {
            String @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            if (String.IsNullOrEmpty(classname))
            {
                classname = WebServiceHelper.GetWsClassName(url);
            }

            try
            {
                //获取WSDL
                WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(url + "?WSDL");
                ServiceDescription sd = ServiceDescription.Read(stream);
                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                CodeNamespace cn = new CodeNamespace(@namespace);

                //生成客户端代理类代码
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                CSharpCodeProvider csc = new CSharpCodeProvider();
                ICodeCompiler icc = csc.CreateCompiler();

                //设定编译参数
                CompilerParameters import = new CompilerParameters();
                import.GenerateExecutable = false;
                import.GenerateInMemory = true;
                import.ReferencedAssemblies.Add("System.dll");
                import.ReferencedAssemblies.Add("System.XML.dll");
                import.ReferencedAssemblies.Add("System.Web.Services.dll");
                import.ReferencedAssemblies.Add("System.Data.dll");

                //编译代理类
                CompilerResults cr = icc.CompileAssemblyFromDom(import, ccu);
                if (true == cr.Errors.HasErrors)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }

                //生成代理实例，并调用方法
                Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(@namespace + "." + classname, true, true);
                Object obj = Activator.CreateInstance(t);
                MethodInfo mi = t.GetMethod(methodname);

                return mi.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }

        private static String GetWsClassName(String wsUrl)
        {
            String[] parts = wsUrl.Split('/');
            String[] pps = parts[parts.Length - 1].Split('.');

            return pps[0];
        }
        #endregion
    }
}