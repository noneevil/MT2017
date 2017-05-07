using System;
using System.IO;
using System.Linq;
using System.Text;
using CommonUtils.Enumeration;
using Ionic.Zip;

namespace CommonUtils
{
    /// <summary>
    /// Zip压缩文件辅助类(DotNetZip Library)
    /// http://dotnetzip.codeplex.com/
    /// </summary>
    public static class ZipHelper
    {
        /// <summary>
        /// 压缩指定文件或目录
        /// </summary>
        /// <param name="fileOrDirectoryName">要进行压缩的文件或目录名称</param>
        /// <returns>生成的压缩文件名</returns>
        public static String Compress(String fileOrDirectoryName)
        {
            String zipPath = _GetZipPath(fileOrDirectoryName);
            Compress(fileOrDirectoryName, zipPath);
            return zipPath;
        }
        /// <summary>
        /// 压缩指定文件或目录
        /// </summary>
        /// <param name="fileOrDirectoryName">要进行压缩的文件或目录名称</param>
        /// <param name="zipPath">生成的压缩文件路径</param>
        public static void Compress(String fileOrDirectoryName, String zipPath)
        {
            using (ZipFile zip = new ZipFile(Encoding.GetEncoding("utf-8")))
            {
                zip.AddItem(fileOrDirectoryName, "");
                zip.Save(zipPath);
            }
        }
        /// <summary>
        /// 分卷压缩指定文件或目录
        /// </summary>
        /// <param name="fileOrDirectoryName">要进行压缩的文件或目录名称</param>
        /// <param name="segmentSize">分卷大小(MB)</param>
        public static void Compress(String fileOrDirectoryName, int segmentSize)
        {
            String zipPath = _GetZipPath(fileOrDirectoryName);
            Compress(fileOrDirectoryName, zipPath, DiskSizeUnit.MB, segmentSize);
        }
        /// <summary>
        /// 分卷压缩指定文件或目录
        /// </summary>
        /// <param name="fileOrDirectoryName">要进行压缩的文件或目录名称</param>
        /// <param name="zipPath">生成的压缩文件路径</param>
        /// <param name="dataUnit">分卷数据单位</param>
        /// <param name="segmentSize">分卷大小</param>
        public static void Compress(String fileOrDirectoryName, String zipPath, DiskSizeUnit dataUnit, int segmentSize)
        {
            using (ZipFile zip = new ZipFile(Encoding.GetEncoding("utf-8")))
            {
                zip.MaxOutputSegmentSize = (int)dataUnit * segmentSize;
                zip.AddItem(fileOrDirectoryName, "");
                zip.Save(zipPath);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static String _GetZipPath(String path)
        {
            String directory = Path.GetDirectoryName(path);
            if (String.IsNullOrWhiteSpace(directory)) directory = Path.GetPathRoot(path);
            String fileName = Path.GetFileName(path);
            String zipFileName = null;
            //文件路径
            if (!String.IsNullOrWhiteSpace(fileName))
            {
                zipFileName = Path.ChangeExtension(fileName, ".zip");
            }
            else
            {
                zipFileName = directory.Split('\\').Last();
                if (String.IsNullOrEmpty(zipFileName))
                {
                    zipFileName = "未命名";
                }
                else
                {
                    directory = directory.Replace(zipFileName, "");
                }
                zipFileName += ".zip";
            }
            return Path.Combine(directory, zipFileName);
        }
    }
}
