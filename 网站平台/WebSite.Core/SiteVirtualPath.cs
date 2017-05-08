using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using CommonUtils;
//http://www.cnblogs.com/zxjay/archive/2009/07/26/Xianfen_Net_VirtualPathProvider_2.html
//http://www.cnblogs.com/zxjay/archive/2008/11/13/xianfen_net_virtualpathprovider.html
namespace WebSite.Core
{
    /// <summary>
    /// 虚拟路径 
    /// </summary>
    public class SiteVirtualPath : VirtualPathProvider
    {
        /// <summary>
        /// 虚拟后台程序文件
        /// </summary>
        private static Boolean MaskDevFile = false;

        public static void AppInitialize()
        {
            MaskDevFile = Convert.ToBoolean(XmlConfig.AppSettings("MaskDevFile"));

            HostingEnvironment.RegisterVirtualPathProvider(new SiteVirtualPath());
        }
        private Boolean IsVirtualFile(String virtualPath)
        {
            return MaskDevFile & Regex.IsMatch(virtualPath, @"/developer/[\S]+?.aspx", RegexOptions.IgnoreCase);
            //return Regex.IsMatch(virtualPath, @"/virtual/[\S]+?.(ascx|aspx)", RegexOptions.IgnoreCase);
            //return Path.GetExtension(virtualPath).ToLower().Trim('.') == "vhtml";
        }
        public override Boolean FileExists(String virtualPath)
        {
            if (IsVirtualFile(virtualPath)) return true;
            return Previous.FileExists(virtualPath);
        }
        public override VirtualFile GetFile(String virtualPath)
        {
            if (IsVirtualFile(virtualPath)) return new SiteVirtualFile(virtualPath);
            return Previous.GetFile(virtualPath);
        }

        #region 文件夹
        //bool IsVirtualDirectory(String vPath)
        //{
        //    String relativePath = VirtualPathUtility.ToAppRelative(vPath);
        //    return Regex.IsMatch(relativePath, @"~/vdir\d{1}/.*", RegexOptions.IgnoreCase);
        //}
        //public override bool DirectoryExists(String virtualDir)
        //{
        //    if (IsVirtualDirectory(virtualDir))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return base.DirectoryExists(virtualDir);
        //    }
        //}
        //public override VirtualDirectory GetDirectory(String virtualDir)
        //{
        //    if (IsVirtualDirectory(virtualDir))
        //    {
        //        return new SiteVirtualDirectory(virtualDir);
        //    }
        //    else
        //    {
        //        return Previous.GetDirectory(virtualDir);
        //    }
        //}
        #endregion

        //public override String GetCacheKey(String virtualPath)
        //{
        //    if (IsVirtualFile(virtualPath)) return null;
        //    return Previous.GetCacheKey(virtualPath);
        //}
        public override CacheDependency GetCacheDependency(String virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (IsVirtualFile(virtualPath))
            {
                //return null;//返回null会导致性能问题
                String path = HttpRuntime.BinDirectory;
                return new System.Web.Caching.CacheDependency(HttpContext.Current.Server.MapPath("/App_Data/WebRes.config"));
                //return new System.Web.Caching.CacheDependency(HttpContext.Current.Server.MapPath("/App_Data/Forms.xml"));
            }
            return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }
        //public override String GetFileHash(String virtualPath, IEnumerable virtualPathDependencies)
        //{
        //    if (IsVirtualFile(virtualPath)) return null;
        //    return Previous.GetFileHash(virtualPath, virtualPathDependencies);
        //}
    }

    /// <summary>
    /// 虚拟文件
    /// </summary>
    public class SiteVirtualFile : VirtualFile
    {
        private static RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled;
        private String vPath;

        public SiteVirtualFile(String virtualPath)
            : base(virtualPath)
        {
            vPath = virtualPath;
        }

        public override Stream Open()
        {
            //using (FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(vPath), FileMode.Open, FileAccess.Read))
            //{
            //    Byte[] bytes = new Byte[fs.Length];
            //    fs.Read(bytes, 0, bytes.Length);
            //    return new MemoryStream(bytes);
            //}
            return new MemoryStream(Encoding.Default.GetBytes(vPath));


            //Regex reg = new Regex(@"virtual/(?<folder>[\S]+)?/(?<file>[\S]+)", options);
            //Match m = reg.Match(vPath);
            //if (m.Success)
            //{
            //    String file = m.Groups["file"].Value;
            //    String folder = m.Groups["folder"].Value;

            //    var Forms = TableForm.TableForms.Find(a => { return a.TableID == Guid.Parse("dcedf8b4-c72c-4fd2-b20a-571c8759fafe"); });
            //    var frm = Forms.FromCollections.Find(a => { return a.ID == Guid.Parse("4aaede23-965f-406e-9ae6-c394afea17b1"); });
            //    String text = frm.Content;
            //    return new MemoryStream(Encoding.Default.GetBytes(text));
            //}
            //else
            //{
            //    throw new Exception(vPath);
            //}
        }
    }
    /// <summary>
    /// 虚拟目录
    /// </summary>
    public class SiteVirtualDirectory : VirtualDirectory
    {
        String vDir = null;
        ArrayList children = new ArrayList();
        ArrayList dirs = new ArrayList();
        ArrayList files = new ArrayList();

        public SiteVirtualDirectory(String virtualDirectory)
            : base(virtualDirectory)
        {
            vDir = virtualDirectory;
            InitData();
        }

        private void InitData()
        {
            if (String.IsNullOrEmpty(vDir))
            {
                children.Add("vdir1");
                children.Add("vdir2");
                children.Add("vdir3");
                children.Add("vdir4");

                dirs.Add("vdir1");
                dirs.Add("vdir2");
                dirs.Add("vdir3");
                dirs.Add("vdir4");

                children.Add("vfile1.aspx");
                children.Add("vfile2.aspx");
                children.Add("vfile3.aspx");
                children.Add("vfile4.aspx");

                files.Add("vfile1.aspx");
                files.Add("vfile2.aspx");
                files.Add("vfile3.aspx");
                files.Add("vfile4.aspx");
            }
            else
            {
                children.Add("vfile1.aspx");
                children.Add("vfile2.aspx");
                children.Add("vfile3.aspx");
                children.Add("vfile4.aspx");

                files.Add("vfile1.aspx");
                files.Add("vfile2.aspx");
                files.Add("vfile3.aspx");
                files.Add("vfile4.aspx");
            }
        }

        public override IEnumerable Children
        {
            get { return children; }
        }

        public override IEnumerable Directories
        {
            get { return dirs; }
        }

        public override IEnumerable Files
        {
            get { return files; }
        }
    }
}