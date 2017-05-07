using System;
using System.CodeDom.Compiler;
using System.Text;
using System.Web;
using Microsoft.CSharp;

namespace CommonUtils
{
    /// <summary>
    /// 动态编译DLL
    /// </summary>
    public abstract class CodeCompiled
    {
        public static void CompileAssemblyFromSource(StringBuilder code, String[] Assemblies, String SaveFile)
        {
            CSharpCodeProvider csharp = new CSharpCodeProvider();
            CompilerParameters param = new CompilerParameters();
            param.ReferencedAssemblies.Add("System.dll");
            param.ReferencedAssemblies.Add("System.Xml.dll");
            param.ReferencedAssemblies.Add("System.Web.dll");
            param.ReferencedAssemblies.Add("System.Data.dll");
            foreach (String name in Assemblies)
            {
                param.ReferencedAssemblies.Add(name);
            }
            param.GenerateExecutable = false;
            param.OutputAssembly = SaveFile;

            code.Insert(0, "using System;\r");
            code.Insert(0, "using System.Web;\r");
            code.Insert(0, "using System.Text;\r");
            code.Insert(0, "using System.Data;\r");
            code.Insert(0, "using System.IO;\r");
            code.Insert(0, "using System.Xml;\r");
            code.Insert(0, "using System.ComponentModel;\r");

            CompilerResults cr = csharp.CompileAssemblyFromSource(param, code.ToString());

            if (cr.Errors.HasErrors)
            {
                StringBuilder error = new StringBuilder();
                error.Append("<div class=\"errlist\">");
                error.AppendFormat("<span>{0}处错误：</span>", cr.Errors.Count);
                error.Append("<ul>");
                for (int x = 0; x < cr.Errors.Count; x++)
                {
                    error.AppendFormat("<li>{0} {1}</li>", cr.Errors[x].Line, cr.Errors[x].ErrorText);
                }
                error.Append("</ul></div>");
                HttpContext.Current.Response.Write(error);
            }
        }
        public static void CompileAssemblyFromFile(String[] CodeFiles, String[] Assemblies, String SaveFile)
        {
            CSharpCodeProvider csharp = new CSharpCodeProvider();
            CompilerParameters param = new CompilerParameters();
            param.ReferencedAssemblies.Add("System.dll");
            param.ReferencedAssemblies.Add("System.Xml.dll");
            param.ReferencedAssemblies.Add("System.Web.dll");
            param.ReferencedAssemblies.Add("System.Data.dll");
            foreach (String name in Assemblies)
            {
                param.ReferencedAssemblies.Add(name);
            }
            param.GenerateExecutable = false;
            param.OutputAssembly = SaveFile;

            CompilerResults cr = csharp.CompileAssemblyFromFile(param, CodeFiles);

            if (cr.Errors.HasErrors)
            {
                StringBuilder error = new StringBuilder();
                error.Append("<div class=\"errlist\">");
                error.AppendFormat("<span>{0}处错误：</span>", cr.Errors.Count);
                error.Append("<ul>");
                for (int x = 0; x < cr.Errors.Count; x++)
                {
                    error.AppendFormat("<li>{0} {1}</li>", cr.Errors[x].Line, cr.Errors[x].ErrorText);
                }
                error.Append("</ul></div>");
                //HttpContext.Current.Response.Write(error);
            }
        }
    }
}
