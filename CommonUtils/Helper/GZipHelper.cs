using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// Gzip压缩辅助类  
    /// </summary>
    public abstract class GZipHelper
    {
        /// <summary>
        /// 对字符串进行压缩
        /// </summary>
        /// <param name="text">待压缩的字符串</param>
        /// <returns>压缩后的Base64编码字符串</returns>
        public static String Compress(String text)
        {
            Byte[] buffer = Compress(Encoding.UTF8.GetBytes(text));
            return Convert.ToBase64String(buffer);
        }
        /// <summary>
        /// 对字符串进行解压缩
        /// </summary>
        /// <param name="text">待解压缩的Base64编码字符串</param>
        /// <returns>解压缩后的字符串</returns>
        public static String Decompress(String text)
        {
            Byte[] buffer = Decompress(Convert.FromBase64String(text));
            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// 对Byte数组进行压缩
        /// </summary>
        /// <param name="data">待压缩的Byte数组</param>
        /// <returns>压缩后的Byte数组</returns>
        public static Byte[] Compress(Byte[] data)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (GZipStream zip = new GZipStream(stream, CompressionMode.Compress))
                {
                    zip.Write(data, 0, data.Length);
                }
                return stream.ToArray();
            }
        }
        /// <summary>
        /// 对Byte数组进行解压
        /// </summary>
        /// <param name="data">待解压的Byte数组</param>
        /// <returns>解压后的Byte数组</returns>
        public static Byte[] Decompress(Byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    CopyStream(zip, stream);
                }
            }
            return stream.ToArray();
        }

        /// <summary>
        /// 使用gzip压缩指定文件(默认编码为utf-8)
        /// </summary>
        /// <param name="srcFile">待压缩的文件路径</param>
        /// <param name="dstFile">压缩后的文件路径</param>
        public static void Compress(String srcFile, String dstFile)
        {
            using (FileStream dest = new FileStream(dstFile, FileMode.Create))
            {
                using (FileStream source = File.OpenRead(srcFile))
                {
                    using (GZipStream zip = new GZipStream(dest, CompressionMode.Compress))
                    {
                        CopyStream(source, zip);
                    }
                }
            }
        }
        /// <summary>
        /// 使用gzip解压缩指定文件(默认编码为utf-8)
        /// </summary>
        /// <param name="srcFile">待解压缩的文件路径</param>
        /// <param name="dstFile">解压缩后的文件路径</param>
        public static void Decompress(String srcFile, String dstFile)
        {
            using (FileStream dest = new FileStream(dstFile, FileMode.Create))
            {
                using (FileStream source = File.OpenRead(srcFile))
                {
                    using (GZipStream zip = new GZipStream(source, CompressionMode.Decompress))
                    {
                        CopyStream(zip, dest);
                    }
                }
            }
        }

        /// <summary>
        /// 流转换
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        private static void CopyStream(Stream input, Stream output)
        {
            Byte[] Bytes = new Byte[4096];
            int n;
            while ((n = input.Read(Bytes, 0, Bytes.Length)) != 0)
            {
                output.Write(Bytes, 0, n);
            }
        }
    }
}
