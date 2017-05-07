using System;
using System.IO;
using System.Text;
using SevenZip;

namespace CommonUtils
{
    public abstract class SevenZipSharpHelper
    {
        /// <summary>
        /// 设置7z DLL路径
        /// </summary>
        /// <param name="zipdllPath"></param>
        public static void SetZipPath(String zipdllPath)
        {
            //SevenZipBase.SetLibraryPath(@"C:\Program Files\7z.dll");
            SevenZipBase.SetLibraryPath(zipdllPath);
        }

        /// <summary>
        /// 对字符串进行压缩
        /// </summary>
        /// <param name="text">待压缩的字符串</param>
        /// <returns>压缩后的Base64编码字符串</returns>
        public static String Compress(String text)
        {
            return Compress(text, null);
        }
        /// <summary>
        /// 对字符串进行压缩
        /// </summary>
        /// <param name="text">待压缩的字符串</param>
        /// <param name="password">压缩密码</param>
        /// <returns>压缩后的Base64编码字符串</returns>
        public static String Compress(String text, String password)
        {
            Byte[] data = Encoding.UTF8.GetBytes(text);
            Byte[] buffer = Compress(data, password);
            return Convert.ToBase64String(buffer);
        }
        /// <summary>
        /// 对Byte数组进行压缩
        /// </summary>
        /// <param name="data">待压缩的Byte数组</param>
        /// <returns>压缩后的Byte数组</returns>
        public static Byte[] Compress(Byte[] data)
        {
            return Compress(data, null);
        }
        /// <summary>
        /// 对Byte数组进行压缩
        /// </summary>
        /// <param name="data">待压缩的Byte数组</param>
        /// <param name="password">压缩密码</param>
        /// <returns>压缩后的Byte数组</returns>
        public static Byte[] Compress(Byte[] data, String password)
        {
            SevenZipCompressor zip = new SevenZipCompressor();
            zip.CompressionMethod = CompressionMethod.Lzma;
            zip.CompressionLevel = CompressionLevel.High;
            zip.ScanOnlyWritable = true;
            //zip.DefaultItemName = "T";

            using (MemoryStream stream = new MemoryStream(data))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    if (!String.IsNullOrEmpty(password))
                    {
                        zip.CompressStream(stream, ms, password);
                    }
                    else
                    {
                        zip.CompressStream(stream, ms);
                    }
                    return ms.ToArray();
                }
            }
        }



        /// <summary>
        /// 对字符串进行解压缩
        /// </summary>
        /// <param name="text">待解压缩的Base64编码字符串</param>
        /// <returns>解压缩后的字符串</returns>
        public static String Decompress(String text)
        {
            Byte[] data = Convert.FromBase64String(text);
            Byte[] buffer = Decompress(data, null);
            return Encoding.UTF8.GetString(buffer);
        }
        /// <summary>
        /// 对Byte数组进行解压
        /// </summary>
        /// <param name="data">待解压的Byte数组</param>
        /// <returns>解压后的Byte数组</returns>
        public static Byte[] Decompress(Byte[] data)
        {
            return Decompress(data, null);
        }
        /// <summary>
        /// 对Byte数组进行解压
        /// </summary>
        /// <param name="data">待解压的Byte数组</param>
        /// <param name="password">解压密码</param>
        /// <returns>解压后的Byte数组</returns>
        public static Byte[] Decompress(Byte[] data, String password)
        {
            if (password == null) password = "";
            using (MemoryStream stream = new MemoryStream(data))
            {
                using (SevenZipExtractor zip = new SevenZipExtractor(stream))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        zip.ExtractFile(0, ms);
                        return ms.ToArray();
                    }
                }
            }
        }



        /// <summary>
        /// 压缩指定文件
        /// </summary>
        /// <param name="srcFile">待压缩的文件路径</param>
        /// <param name="dstFile">压缩后的文件路径</param>
        public static void CompressFile(String dstFile, String srcFile)
        {
            String[] files = new String[] { srcFile };
            CompressFile(dstFile, files);
        }
        /// <summary>
        /// 压缩指定文件
        /// </summary>
        /// <param name="srcFiles">待压缩的文件路径</param>
        /// <param name="dstFile">压缩后的文件路径</param>
        public static void CompressFile(String dstFile, params String[] srcFiles)
        {
            SevenZipCompressor zip = new SevenZipCompressor();
            zip.ArchiveFormat = OutArchiveFormat.SevenZip;
            zip.CompressionMethod = CompressionMethod.Lzma2;
            zip.CompressionMode = CompressionMode.Create;
            zip.CompressionLevel = CompressionLevel.High;
            zip.CompressFiles(dstFile, srcFiles);
        }
        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="dstFile">压缩后的文件路径</param>
        /// <param name="directory">待压缩的文件夹路径</param>
        public static void CompressFolder(String dstFile, String directory)
        {
            SevenZipCompressor zip = new SevenZipCompressor();
            zip.ArchiveFormat = OutArchiveFormat.SevenZip;
            zip.CompressionMethod = CompressionMethod.Lzma2;
            zip.CompressionMode = CompressionMode.Create;
            zip.CompressionLevel = CompressionLevel.High;
            zip.CompressDirectory(directory, dstFile);
        }
        /// <summary>
        /// 解压缩指定文件
        /// </summary>
        /// <param name="srcFile">待解压缩的文件路径</param>
        /// <param name="directory">解压缩后的文件路径</param>
        public static void Decompress(String srcFile, String directory)
        {
            using (SevenZipExtractor zip = new SevenZipExtractor(srcFile))
            {
                zip.ExtractArchive(directory);
            }
        }
    }
}