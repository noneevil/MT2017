using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Security;
using Newtonsoft.Json;
using SevenZip;
using WebSite.Interface;

namespace WebSite.Plugins
{
    public class LicenseHelper
    {
        /// <summary>
        /// 静态调用
        /// </summary>
        public static ILicenseData License
        {
            get
            {
                String LicenseFile = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data/license.txt");
                ILicenseData data;
                var Cache = HttpRuntime.Cache;
                if (Cache["License"] == null)
                {
                    using (FileStream file = new FileStream(LicenseFile, FileMode.Open, FileAccess.Read))
                    {
                        data = DecryptLicense(file);
                    }

                    Cache.Insert("License", data, new CacheDependency(LicenseFile));
                }
                else
                {
                    data = (ILicenseData)Cache["License"];
                }
                return data;
            }
        }
        /// <summary>
        /// 生成授权文件
        /// </summary>
        /// <param name="data"></param>
        public static void CreateLicense(ILicenseData data)
        {
            Int32 count = data.Count;
            String json = JsonConvert.SerializeObject(data);
            Byte[] buffer = Encoding.UTF8.GetBytes(json);

            #region 7z压缩加密

            Byte[] zipBuffer = Encoding.UTF8.GetBytes(Math.Pow(3, count / 2).ToString("0"));
            String zipPassword = MD5Upper32(String.Join("&", zipBuffer));

            SevenZipCompressor zip = new SevenZipCompressor()
            {
                DefaultItemName = "license",
                EncryptHeaders = true,
                FastCompression = true,
                ScanOnlyWritable = true,
                CompressionLevel = CompressionLevel.High,
                CompressionMethod = CompressionMethod.Lzma2
            };

            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    zip.CompressStream(stream, ms);
                    //zip.CompressStream(stream, ms, zipPassword);

                    ms.Position = 6;//去掉7z头文件信息
                    buffer = new Byte[ms.Length - ms.Position];
                    ms.Read(buffer, 0, buffer.Length);
                }
            }

            #endregion

            Array.Reverse(buffer, 0, buffer.Length);//反转流

            #region DES在次加密

            for (int i = 1; i <= count; i++)
            {
                String[] arr = new String[5];
                for (int j = 2; j < 7; j++)
                {
                    String v1 = Math.Pow(j, i).ToString("0");
                    arr[j - 2] = v1;
                }
                String key = MD5Upper32(String.Join("#", arr));
                buffer = EncryptDES(buffer, key);
            }

            #endregion

            #region 文件尾写入加密次数

            Byte[] byt = new Byte[] { (Byte)count };
            buffer = buffer.Concat(byt).ToArray();

            #endregion

            #region 输出文件

            String base64 = Convert.ToBase64String(buffer, Base64FormattingOptions.InsertLineBreaks);

            Byte[] head = new Byte[256];
            StringBuilder sb = new StringBuilder(256);
            sb.AppendLine("Warning: Please don't delete or modify this file.");
            sb.AppendLine("Have any questions, please contact the author.");
            sb.AppendLine();
            sb.AppendLine("author:Jin Jia You Email:412541529@qq.com");
            Encoding.UTF8.GetBytes(sb.ToString()).CopyTo(head, 0);

            HttpResponse Response = HttpContext.Current.Response;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            Response.AddHeader("Content-Disposition", "attachment; filename=license.txt");

            using (StreamWriter sw = new StreamWriter(Response.OutputStream, Encoding.UTF8))
            {
                sw.Write(Encoding.UTF8.GetString(head));
                sw.WriteLine();
                sw.Write(base64);
            }

            Response.Flush();
            Response.End();

            #endregion
        }
        /// <summary>
        /// 解密授权文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static ILicenseData DecryptLicense(Stream file)
        {
            Int32 count = 0;
            Byte[] buffer = new Byte[] { };

            file.Position = 256 + 4;
            using (BinaryReader br = new BinaryReader(file, Encoding.UTF8))
            {
                Byte[] byt = br.ReadBytes((int)(file.Length - file.Position));
                String base64 = Encoding.UTF8.GetString(byt);
                byt = Convert.FromBase64String(base64);

                count = byt[byt.Length - 1];//读取加密次数
                buffer = new Byte[byt.Length - 1];//读取加密正文
                Array.Copy(byt, buffer, buffer.Length);
            }

            #region DES解密

            for (int i = count; i >= 1; i--)
            {
                String[] arr = new String[5];
                for (int j = 2; j < 7; j++)
                {
                    String v1 = Math.Pow(j, i).ToString("0");
                    arr[j - 2] = v1;
                }
                String key = MD5Upper32(String.Join("#", arr));
                buffer = DecryptDES(buffer, key);
            }

            #endregion

            Array.Reverse(buffer, 0, buffer.Length);

            #region 7z解密

            Byte[] zipBuffer = Encoding.UTF8.GetBytes(Math.Pow(3, count / 2).ToString("0"));
            String zipPassword = MD5Upper32(String.Join("&", zipBuffer));

            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(new Byte[] { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C }, 0, 6);
                stream.Write(buffer, 0, buffer.Length);
                using (SevenZipExtractor zip = new SevenZipExtractor(stream, zipPassword))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        zip.ExtractFile(0, ms);
                        buffer = ms.ToArray();
                    }
                }
            }

            #endregion

            String json = Encoding.UTF8.GetString(buffer);
            ILicenseData data = JsonConvert.DeserializeObject<ILicenseData>(json);
            return data;
        }
        /// <summary>
        /// 解密授权文件
        /// </summary>
        /// <returns></returns>
        //public static ILicenseData DecryptLicense()
        //{
        //    ILicenseData data;
        //    String licensefile = HttpContext.Current.Server.MapPath("/App_Data/license.txt");
        //    using (FileStream file = new FileStream(licensefile, FileMode.Open, FileAccess.Read))
        //    {
        //        data = DecryptLicense(file);
        //    }
        //    return data;
        //}
        /// <summary>
        /// 密钥向量
        /// </summary>
        private static Byte[] IV = { 0xE9, 0x9D, 0xB3, 0xE5, 0xAE, 0xB6, 0xE5, 0x8F, 0x8B };
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="inputbuffer">待加密流</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密后Byte[]</returns>
        private static Byte[] EncryptDES(Byte[] inputbuffer, String encryptKey)
        {
            Byte[] Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            Byte[] outputbuffer = { };

            using (DESCryptoServiceProvider dcsp = new DESCryptoServiceProvider())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, dcsp.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                    {
                        cs.Write(inputbuffer, 0, inputbuffer.Length);
                        cs.FlushFinalBlock();
                        outputbuffer = ms.ToArray();
                    }
                }
            }
            return outputbuffer;
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="inputbuffer">待解密流</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密后Byte[]</returns>
        private static Byte[] DecryptDES(Byte[] inputbuffer, string decryptKey)
        {
            Byte[] Key = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
            Byte[] outputbuffer = { };
            using (DESCryptoServiceProvider dcsp = new DESCryptoServiceProvider())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, dcsp.CreateDecryptor(Key, IV), CryptoStreamMode.Write))
                    {
                        cs.Write(inputbuffer, 0, inputbuffer.Length);
                        cs.FlushFinalBlock();
                        outputbuffer = ms.ToArray();
                    }
                }
            }
            return outputbuffer;
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string MD5Upper32(String text)
        {
            String result = FormsAuthentication.HashPasswordForStoringInConfigFile(text, FormsAuthPasswordFormat.MD5.ToString());
            return result.ToUpper();
        }
    }
}
