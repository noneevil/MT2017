using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace CommonUtils
{
    /// <summary>
    /// 文件操作扩展类
    /// </summary>
    public class FileHelper
    {
        #region Win32API 文件移动到回收站

        private const int FO_DELETE = 0x3;
        private const ushort FOF_NOCONFIRMATION = 0x10;
        private const ushort FOF_ALLOWUNDO = 0x40;

        [DllImport("shell32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int SHFileOperation([In, Out] _SHFILEOPSTRUCT str);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public class _SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            public UInt32 wFunc;
            public String pFrom;
            public String pTo;
            public UInt16 fFlags;
            public Int32 fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public String lpszProgressTitle;
        }

        #endregion
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int Delete(String path)
        {
            _SHFILEOPSTRUCT pm = new _SHFILEOPSTRUCT();
            pm.wFunc = FO_DELETE;
            pm.pFrom = path + '\0';
            pm.pTo = null;
            pm.fFlags = FOF_ALLOWUNDO | FOF_NOCONFIRMATION;
            return SHFileOperation(pm);
        }
        /// <summary>
        /// 获取文件编码
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Encoding GetEncoding(String path)
        {
            FileInfo fileInfo = new FileInfo(path);
            IdentifyEncoding identitfy = new IdentifyEncoding();
            return Encoding.GetEncoding(identitfy.GetEncodingName(fileInfo));
        }
        /// <summary>
        /// 文件大小格式化
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static String FileSizeToStr(long size)
        {
            size = Math.Abs(size);
            if (size < 1024)
            {
                return size + "B";
            }
            else if (size / 1024 < 1024)
            {
                return Math.Round((size / (Double)1024), 2) + "K";
            }
            else if ((size / 1024) < 1024 * 1000)
            {
                return Math.Round((size / (Double)(1024 * 1024)), 2) + "M";
            }
            else
            {
                return Math.Round((size / (Double)(1024 * 1024 * 1024)), 2) + "G";
            }
        }
        /// <summary>
        /// 创建文件所需文件夹
        /// </summary>
        /// <param name="path">文件或文件夹路径</param>
        /// <returns></returns>
        public static String CreatePath(String path)
        {
            try
            {
                if (String.IsNullOrEmpty(Path.GetExtension(path))) path += @"\";
                String filepath = Path.GetDirectoryName(path);
                String filename = Path.GetFileName(path);
                filepath = filepath.TrimEnd(new Char[] { '/', '\\' }) + @"\";
                return Directory.CreateDirectory(filepath).FullName + filename;
            }
            catch
            {
                return String.Empty;
            }
        }
        /// <summary>
        /// 检查文件夹是否存在,如果存在返回新文件夹路径
        /// </summary>
        /// <param name="folder"></param>
        public static void CheckFolder(ref String folder)
        {
            DirectoryInfo newfolder = new DirectoryInfo(folder);
            CheckFolder(ref newfolder);
            folder = newfolder.FullName;
        }
        /// <summary>
        /// 检查文件夹是否存在,如果存在返回新文件夹路径
        /// </summary>
        /// <param name="folder"></param>
        public static void CheckFolder(ref DirectoryInfo folder)
        {
            if (folder.Exists)
            {
                int i = 0;
                while (true)
                {
                    i++;
                    String newfolder = Path.Combine(folder.Parent.FullName, folder.Name + "-" + i.ToString("00"));
                    if (!Directory.Exists(newfolder))
                    {
                        //CreatePath(newfolder);
                        folder = new DirectoryInfo(newfolder);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 检查文件是否存在,如果存在返回新文件路径
        /// </summary>
        /// <param name="file"></param>
        public static void CheckFile(ref String file)
        {
            FileInfo newfile = new FileInfo(file);
            CheckFile(ref newfile);
            file = newfile.FullName;
        }
        /// <summary>
        /// 检查文件是否存在,如果存在返回新文件路径
        /// </summary>
        /// <param name="file"></param>
        public static void CheckFile(ref FileInfo file)
        {
            if (file.Exists)
            {
                int i = 0;
                String outfilename = Path.GetFileNameWithoutExtension(file.Name) + "_";
                while (true)
                {
                    i++;
                    String newfile = Path.Combine(file.DirectoryName, outfilename + i.ToString("00") + file.Extension);
                    if (!File.Exists(newfile))
                    {
                        file = new FileInfo(newfile);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 判断文件是否正在使用中
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Boolean IsFileLocked(String file)
        {
            Boolean use = true;
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None);
                use = false;
                fs.Close();
            }
            catch { }
            return use;
        }
        private static String applicationRoot;// = null;
        public static String ApplicationRoot
        {
            get
            {
                //Build the relative Application Path
                if (applicationRoot == null)
                {
                    HttpRequest request = HttpContext.Current.Request;
                    applicationRoot = (request.ApplicationPath == "/") ? String.Empty : request.ApplicationPath;
                }
                return applicationRoot;
            }
        }
        public static String ApplicationFullPath
        {
            get
            {
                HttpRequest request = HttpContext.Current.Request;
                String fullpath = "http://" + request.Url.Host + request.ApplicationPath;
                return fullpath;
            }
        }
        public static String ApplicationPhysicalPath
        {
            get { return HttpContext.Current.Request.PhysicalApplicationPath; }
        }
        public static String ApplicationRootPath(String value)
        {
            return WebPathCombine(ApplicationRoot, value);
        }
        public static String ApplicationRootPath(params String[] values)
        {
            List<String> fullValues = new List<String>(values);
            fullValues.Insert(0, ApplicationRoot);
            return WebPathCombine((String[])fullValues.ToArray());
        }
        public static String WebPathCombine(params String[] values)
        {
            String separator = "/";
            String doubleSeparator = separator + separator;
            if (values == null) throw new NullReferenceException("路径不能为空!");
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                if (i != 0) s.Append(separator);
                if (values[i] != null && values[i].Length > 0) s.Append(values[i]);
            }
            String result = s.ToString();
            while (result.IndexOf(doubleSeparator) > -1)
            {
                result = result.Replace(doubleSeparator, separator);
            }
            result = result.Replace(":/", "://");
            return result;
        }
        /// <summary>
        /// 用于表示下载文件的ContentType格式的类型集合。
        /// </summary>
        protected static DataSet m_ContentType = null;
        /// <summary>
        /// 获取文件MIME类型
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <returns></returns>
        public static String GetContentType(String FileName)
        {
            String ex = Path.GetExtension(FileName);
            if (String.IsNullOrEmpty(ex)) return "";
            if (m_ContentType == null)
            {
                m_ContentType = new DataSet();
                m_ContentType.ReadXml(HttpContext.Current.Server.MapPath("/App_Data/ContentType.xml"));
            }
            DataTable dt = m_ContentType.Tables[0];
            DataRow[] drs = dt.Select(String.Format("name='{0}'", ex.ToLower()));
            if (drs.Length == 0) return "application/octet-stream";
            return Convert.ToString(drs[0]["value"]);
        }
        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="sourceDirName">原文件夹</param>
        /// <param name="destDirName">目标文件夹</param>
        /// <param name="copySubDirs">是否复制子目录</param>
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        /// <summary>
        /// 保存上传文件并返回保存文件路径
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static String SaveUpFile(HttpPostedFile file)
        {
            String result = String.Empty;
            if (file.ContentLength > 0)
            {
                HttpServerUtility Server = HttpContext.Current.Server;
                String ex = Path.GetExtension(file.FileName).ToLower().TrimStart('.');
                String SaveFolder = Path.Combine(Server.MapPath("/UpFiles/"), DateTime.Now.ToString("yyyy-MM-dd"));

                Int32 count = Directory.CreateDirectory(SaveFolder).GetFiles().Length + 1;
                String SavePath = Path.Combine(SaveFolder, DateTime.Now.ToString("yyyyMMdd") + count.ToString("00000") + "." + ex);
                file.SaveAs(SavePath);

                result = "/" + new Uri(Server.MapPath("/")).MakeRelativeUri(new Uri(SavePath)).ToString();
            }
            return result;
        }

        ////去掉7z文件头信息
        //using (FileStream zipfile = new FileStream(Server.MapPath("/1.7z"), FileMode.Open, FileAccess.Read))
        //{
        //    zipfile.Position = 6;
        //    Byte[] bytes = new Byte[zipfile.Length - 6];
        //    zipfile.Read(bytes, 0, bytes.Length);
        //    using (BinaryWriter bw = new BinaryWriter(new FileStream(Server.MapPath("/a.7z"), FileMode.OpenOrCreate, FileAccess.Write)))
        //    {
        //        bw.Write(bytes);
        //    }
        //    #region 读取文件头信息
        //    //using (BinaryReader read = new BinaryReader(zipfile))
        //    //{
        //    //    Byte[] buffer = read.ReadBytes(6);
        //    //    for (int i = 0; i < buffer.Length; i++)
        //    //    {
        //    //        Response.Write("0x" + buffer[i].ToString("x") + ",");
        //    //    }
        //    //}
        //    #endregion
        //}

        ////还原文件头信息
        //Byte[] head = new Byte[] { 0x37, 0x7a, 0xbc, 0xaf, 0x27, 0x1c }; //37 7A BC AF 27 1C //55, 122, 188, 175, 39, 28
        //using (FileStream zipfile = new FileStream(Server.MapPath("/a.7z"), FileMode.Open, FileAccess.ReadWrite))
        //{
        //    Byte[] buffer = new Byte[zipfile.Length];
        //    zipfile.Read(buffer, 0, buffer.Length);
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        ms.Write(head, 0, head.Length);
        //        ms.Write(buffer, 0, buffer.Length);
        //        zipfile.Seek(0, SeekOrigin.Begin);
        //        zipfile.Write(ms.ToArray(), 0, head.Length + buffer.Length);
        //    }
        //}
    }
}
