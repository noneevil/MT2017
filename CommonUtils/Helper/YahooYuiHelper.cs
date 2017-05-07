using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Yahoo.Yui.Compressor;

namespace CommonUtils
{
    /// <summary>
    /// Yahoo.Yui.Compressor 2.0
    /// </summary>
    public abstract class YahooYui
    {
        /// <summary>
        /// 压缩目录内的.js或.css文件
        /// </summary>
        /// <param name="Folder">要压缩的目录</param>
        /// <param name="Filter">文件类型 如*.js或*.css</param>
        /// <param name="Option">是否包括子目录</param>
        public static void FolderCompressor(String Folder, SearchOption Option, params String[] Filter)
        {
            DirectoryInfo dir = new DirectoryInfo(Folder);
            FolderCompressor(dir, Option, Filter);
        }
        /// <summary>
        /// 压缩目录内的.js或.css文件
        /// </summary>
        /// <param name="Folder">资源目录</param>
        /// <param name="Filter">过滤条件*.js或*.css</param>
        /// <param name="Option">是否包括子目录</param>
        public static void FolderCompressor(DirectoryInfo Folder, SearchOption Option, params String[] Filter)
        {
            List<FileInfo> files = new List<FileInfo>();
            foreach (String ex in Filter)
            {
                files.AddRange(Folder.GetFiles(ex, Option));
            }
            foreach (FileInfo file in files)
            {
                String ex = file.Extension.ToLower();
                switch (ex)
                {
                    case ".js":
                        JavaScriptCompressor(file);
                        break;
                    case ".css":
                        CssCompressor(file);
                        break;
                }
            }
        }
        /// <summary>
        /// JavaScript 压缩
        /// </summary>
        /// <param name="filePath"></param>
        public static void JavaScriptCompressor(String filePath)
        {
            FileInfo file = new FileInfo(filePath);
            JavaScriptCompressor(file);
        }
        /// <summary>
        /// JavaScript 压缩
        /// </summary>
        /// <param name="filePath"></param>
        public static void JavaScriptCompressor(FileInfo filePath)
        {
            String _file = filePath.FullName;
            if (filePath.Name.ToLower().EndsWith(".min.js")) return;
            String SaveFile = Path.ChangeExtension(_file, ".min.js");
            if (File.Exists(SaveFile))
            {
                File.Delete(SaveFile);
                //String bakfile = SaveFile + "_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".bak";
                //File.Move(SaveFile, bakfile);
            }

            File.SetAttributes(_file, FileAttributes.Normal); //取消文件只读 
            if (filePath.Extension.ToLower() == ".js")
            {
                String strContent = File.ReadAllText(_file, Encoding.Default);

                //初始化JS压缩类
                var js = new JavaScriptCompressor();
                js.CompressionType = CompressionType.Standard;//压缩类型
                js.Encoding = Encoding.UTF8;//编码
                js.IgnoreEval = false;//大小写转换
                js.ThreadCulture = CultureInfo.CurrentCulture;

                strContent = js.Compress(strContent);//压缩该js
                File.WriteAllText(SaveFile, strContent, Encoding.UTF8);
            }
        }
        /// <summary>
        /// CSS 压缩
        /// </summary>
        /// <param name="filePath"></param>
        public static void CssCompressor(String filePath)
        {
            FileInfo file = new FileInfo(filePath);
            CssCompressor(file);
        }
        /// <summary>
        /// CSS 压缩 
        /// </summary>
        /// <param name="filePath"></param>
        public static void CssCompressor(FileInfo filePath)
        {
            String _file = filePath.FullName;
            if (filePath.Name.ToLower().EndsWith(".min.css")) return;
            String SaveFile = Path.ChangeExtension(_file, ".min.css");
            if (File.Exists(SaveFile))
            {
                File.Delete(SaveFile);
                //String bakfile = SaveFile + "_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".bak";
                //File.Move(SaveFile, bakfile);
            }

            File.SetAttributes(_file, FileAttributes.Normal); //取消文件只读 
            if (filePath.Extension.ToLower() == ".css")
            {
                String strContent = File.ReadAllText(_file, Encoding.Default);

                CssCompressor css = new CssCompressor();
                css.CompressionType = CompressionType.Standard;
                css.RemoveComments = true;
                strContent = css.Compress(strContent);
                File.WriteAllText(SaveFile, strContent, Encoding.UTF8);
            }
        }
    }
}