using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using CommonUtils.Enumeration;

namespace CommonUtils
{
    /// <summary>
    /// FTP辅助操作类
    /// 只支持基本的文件上传、下载、目录递归创建、文件目录列表获取
    /// </summary>
    public class FtpHelper
    {
        #region 属性
        /// <summary>
        /// 获取或设置用户名
        /// </summary>
        public String UserName { get; set; }
        /// <summary>
        /// 获取或设置密码
        /// </summary>
        public String Password { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        public String ErrorMsg { get; set; }
        /// <summary>
        /// Exception
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public FtpStatusCode StatusCode { get; set; }
        /// <summary>
        /// 状态描述
        /// </summary>
        public String StatusDescription { get; set; }
        /// <summary>
        /// 获取或设置FTP服务器地址
        /// </summary>
        public Uri Uri { get; set; }
        /// <summary>
        /// 获取或者是读取文件、目录列表时所使用的编码，默认为UTF-8 
        /// </summary>
        public Encoding Encode { get; set; }

        #endregion

        #region 检测网络是否畅通
        /// <summary>
        /// 检测网络是否畅通
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static Boolean CheckServer(IPAddress ip)
        {
            Ping ping = new Ping();
            if (ping.Send(ip).Status == IPStatus.Success)
            {
                ping.Dispose();
                return true;
            }
            ping.Dispose();
            return false;
        }
        #endregion

        #region 构造函数
        public FtpHelper(Uri uri, String username, String password)
        {
            this.Uri = uri;
            this.UserName = username;
            this.Password = password;
            this.Encode = Encoding.GetEncoding("utf-8");
        }
        #endregion

        #region 建立连接
        /// <summary>
        /// 建立FTP链接,返回请求对象
        /// </summary>
        /// <param name="uri">FTP地址</param>
        /// <param name="method">操作命令(WebRequestMethods.Ftp)</param>
        /// <returns></returns>
        private FtpWebRequest CreateRequest(Uri uri, String method)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Credentials = new NetworkCredential(this.UserName, this.Password);//指定登录ftp服务器的用户名和密码。
                request.KeepAlive = false;//指定连接是应该关闭还是在请求完成之后关闭，默认为true
                request.UsePassive = true;//指定使用被动模式，默认为true
                request.UseBinary = true;//指示服务器要传输的是二进制数据.false,指示数据为文本。默认值为true
                request.EnableSsl = false;//如果控制和数据传输是加密的,则为true.否则为false.默认值为 false
                request.Method = method;
                return request;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 建立FTP链接,返回响应对象
        /// </summary>
        /// <param name="uri">FTP地址</param>
        /// <param name="method">操作命令(WebRequestMethods.Ftp)</param>
        /// <returns></returns>
        private FtpWebResponse CreateResponse(Uri uri, String method)
        {
            try
            {
                FtpWebRequest request = CreateRequest(uri, method);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                this.StatusCode = response.StatusCode;
                this.StatusDescription = response.StatusDescription;
                return response;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = ex.Response as FtpWebResponse;
                if (response != null)
                {
                    this.StatusCode = response.StatusCode;
                    this.StatusDescription = response.StatusDescription;
                }
                throw ex;
            }
        }
        #endregion

        #region 上传文件
        /// <summary>
        /// 上传文件到FTP服务器,若文件已存在自动覆盖
        /// 本方法不会自动创建远程路径的目录
        /// </summary>
        /// <param name="localFilePath">本地带有完整路径的文件名</param>
        /// <param name="remoteFilePath">要在FTP服务器上面保存完整文件名</param>
        public Boolean UploadFile(String localFilePath, String remoteFilePath)
        {
            return UploadFile(localFilePath, remoteFilePath, false);
        }
        /// <summary>
        /// 上传文件到FTP服务器,若文件已存在自动覆盖
        /// </summary>
        /// <param name="localFilePath">本地带有完整路径的文件名</param>
        /// <param name="remoteFilePath">要在FTP服务器上面保存完整文件名</param>
        /// <param name="autoCreateDirectory">是否自动递归创建文件目录</param>
        /// <returns></returns>
        public Boolean UploadFile(String localFilePath, String remoteFilePath, Boolean autoCreateDirectory)
        {
            try
            {
                //自动递归创建目录
                if (autoCreateDirectory)
                {
                    if (!CreateDirectory(Path.GetDirectoryName(remoteFilePath)))
                    {
                        //递归创建目录失败，返回false
                        return false;
                    }
                }
                FileInfo fileInf = new FileInfo(localFilePath);
                if (!fileInf.Exists)
                {
                    throw new FileNotFoundException(String.Format("本地文件不存在:{0}!", localFilePath));
                }

                FtpWebRequest request = CreateRequest(new Uri(this.Uri + remoteFilePath), WebRequestMethods.Ftp.UploadFile);

                request.ContentLength = fileInf.Length;

                int contentLen = 0;
                //缓冲2kb
                Byte[] buff = new Byte[2048];
                using (FileStream fs = fileInf.OpenRead())
                {
                    using (Stream stream = request.GetRequestStream())
                    {
                        while ((contentLen = fs.Read(buff, 0, buff.Length)) > 0)
                        {
                            stream.Write(buff, 0, contentLen);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                this.Exception = ex;
                this.ErrorMsg = ex.Message;
            }
            return false;
        }
        #endregion

        #region 下载文件
        /// <summary>
        /// 从FTP服务器下载文件
        /// </summary>
        /// <param name="remoteFilePath">远程完整文件名</param>
        /// <param name="localFilePath">本地带有完整路径的文件名</param>
        public Boolean DownloadFile(String remoteFilePath, String localFilePath)
        {
            try
            {
                String localDirector = Path.GetDirectoryName(localFilePath);
                if (!Directory.Exists(localDirector))
                {
                    Directory.CreateDirectory(localDirector);
                }

                FtpWebResponse response = CreateResponse(new Uri(this.Uri + remoteFilePath), WebRequestMethods.Ftp.DownloadFile);
                Byte[] buffer = new Byte[2048];
                int bytesCount = 0;
                Stream stream = response.GetResponseStream();
                using (FileStream fs = new FileStream(localFilePath, FileMode.Create))
                {
                    while ((bytesCount = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fs.Write(buffer, 0, bytesCount);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                this.Exception = ex;
                this.ErrorMsg = ex.Message;
            }
            return false;
        }
        #endregion

        #region 移动、重命名文件
        /// <summary>
        /// 移动远程文件文件
        /// </summary>
        /// <param name="remoteFileName">远程文件名</param>
        /// <param name="newFileName">新文件名</param>
        /// <returns></returns>
        public Boolean MoveFile(String remoteFileName, String newFileName)
        {
            return ReName(remoteFileName, newFileName);
        }
        /// <summary>
        /// 重命名远程文件
        /// </summary>
        /// <param name="remoteFileName">远程文件名</param>
        /// <param name="newFileName">新文件名</param>
        /// <returns></returns>
        public Boolean ReName(String remoteFileName, String newFileName)
        {
            try
            {
                if (remoteFileName != newFileName)
                {
                    FtpWebRequest request = CreateRequest(new Uri(this.Uri + remoteFileName), WebRequestMethods.Ftp.Rename);
                    request.RenameTo = newFileName;
                    request.GetResponse();
                }
                return true;
            }
            catch (WebException ex)
            {
                this.ErrorMsg = ex.Message;
                this.Exception = ex;
                FtpWebResponse response = ex.Response as FtpWebResponse;
                if (response != null)
                {
                    this.StatusCode = response.StatusCode;
                    this.StatusDescription = response.StatusDescription;
                }
            }
            return false;
        }
        #endregion

        #region 删除文件
        /// <summary>
        /// 删除远程文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>成功返回True，否则返回False</returns>
        public Boolean DeleteFile(String fileName)
        {
            try
            {
                CreateResponse(new Uri(this.Uri + fileName), WebRequestMethods.Ftp.DeleteFile);
                return true;
            }
            catch (Exception ex)
            {
                this.Exception = ex;
                this.ErrorMsg = ex.Message;
            }
            return false;
        }
        #endregion

        #region 递归创建目录
        /// <summary>
        /// 递归创建目录，在创建目录前不进行目录是否已存在检测
        /// </summary>
        /// <param name="remoteDirectory"></param>
        public Boolean CreateDirectory(String remoteDirectory)
        {
            return CreateDirectory(remoteDirectory, false);
        }

        /// <summary>
        /// 在FTP服务器递归创建目录
        /// </summary>
        /// <param name="remoteDirectory">要创建的目录</param>
        /// <param name="autoCheckExist">创建目录前是否进行目录是否存在检测</param>
        /// <returns></returns>
        public Boolean CreateDirectory(String remoteDirectory, Boolean autoCheckExist)
        {
            try
            {
                String parentDirector = "/";
                if (!String.IsNullOrEmpty(remoteDirectory))
                {
                    remoteDirectory = remoteDirectory.Replace("\\", "/");
                    String[] directors = remoteDirectory.Split(new Char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (String director in directors)
                    {
                        if (!parentDirector.EndsWith("/")) parentDirector += "/";
                        if (autoCheckExist)
                        {
                            if (!DirectoryExist(parentDirector, director))
                                CreateResponse(new Uri(this.Uri + parentDirector + director), WebRequestMethods.Ftp.MakeDirectory);
                        }
                        else
                        {
                            try
                            {
                                CreateResponse(new Uri(this.Uri + parentDirector + director), WebRequestMethods.Ftp.MakeDirectory);
                            }
                            catch (WebException ex)
                            {
                                if (this.StatusCode != FtpStatusCode.ActionNotTakenFileUnavailable)
                                {
                                    throw ex;
                                }
                            }
                        }
                        parentDirector += director;
                    }
                }
                return true;
            }
            catch (WebException ex)
            {
                this.Exception = ex;
                this.ErrorMsg = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// 检测指定目录下是否存在指定的目录名
        /// </summary>
        /// <param name="parentDirector"></param>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        private Boolean DirectoryExist(String parentDirector, String directoryName)
        {
            List<FileStruct> list = GetFileAndDirectoryList(parentDirector);
            foreach (FileStruct fstruct in list)
            {
                if (fstruct.IsDirectory && fstruct.Name == directoryName)
                {
                    return true;
                }
            }
            return false;
        }


        #endregion

        #region 检测文件是否已存在
        /// <summary>
        /// 检测FTP服务器上是否存在指定文件
        /// 中文文件名若存在无法正确检测现在有肯能是编码问题所致
        /// 请调用this.Encode进行文件编码设置，默认为UTF-8，一般改为GB2312就能正确识别
        /// </summary>
        /// <param name="remoteFilePath"></param>
        /// <returns></returns>
        public Boolean FileExist(String remoteFilePath)
        {
            List<FileStruct> list = GetFileAndDirectoryList(Path.GetDirectoryName(remoteFilePath));
            foreach (FileStruct fstruct in list)
            {
                if (!fstruct.IsDirectory && fstruct.Name == Path.GetFileName(remoteFilePath))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 目录、文件列表
        /// <summary>
        /// 获取FTP服务器上指定目录下的所有文件和目录
        /// 若获取的中文文件、目录名优乱码现象
        /// 请调用this.Encode进行文件编码设置，默认为UTF-8，一般改为GB2312就能正确识别
        /// </summary>
        /// <param name="direcotry"></param>
        /// <returns></returns>
        public List<FileStruct> GetFileAndDirectoryList(String direcotry)
        {
            try
            {
                List<FileStruct> list = new List<FileStruct>();
                String str = null;
                //WebRequestMethods.Ftp.ListDirectoryDetails可以列出所有的文件和目录列表
                //WebRequestMethods.Ftp.ListDirectory只能列出目录的文件列表
                FtpWebResponse response = CreateResponse(new Uri(this.Uri.ToString() + direcotry), WebRequestMethods.Ftp.ListDirectoryDetails);
                Stream stream = response.GetResponseStream();

                using (StreamReader sr = new StreamReader(stream, this.Encode))
                {
                    str = sr.ReadToEnd();
                }
                String[] fileList = str.Split(new Char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                EFileListFormat format = JudgeFileListFormat(fileList);
                if (!String.IsNullOrEmpty(str) && format != EFileListFormat.Unknown)
                {
                    list = ParseFileStruct(fileList, format);
                }
                return list;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 解析文件列表信息返回文件列表
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="format">文件列表格式</param>
        /// <returns></returns>
        private List<FileStruct> ParseFileStruct(String[] fileList, EFileListFormat format)
        {
            List<FileStruct> list = new List<FileStruct>();
            if (format == EFileListFormat.UnixFormat)
            {
                foreach (String info in fileList)
                {
                    FileStruct fstuct = new FileStruct();
                    fstuct.Origin = info.Trim();
                    fstuct.OriginArr = fstuct.Origin.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (fstuct.OriginArr.Length == 9)
                    {
                        fstuct.Flags = fstuct.OriginArr[0];
                        fstuct.IsDirectory = (fstuct.Flags[0] == 'd');
                        fstuct.Owner = fstuct.OriginArr[2];
                        fstuct.Group = fstuct.OriginArr[3];
                        fstuct.Size = Convert.ToInt32(fstuct.OriginArr[4]);
                        if (fstuct.OriginArr[7].Contains(":"))
                        {
                            fstuct.OriginArr[7] = DateTime.Now.Year + " " + fstuct.OriginArr[7];
                        }
                        fstuct.UpdateTime = DateTime.Parse(String.Format("{0} {1} {2}", fstuct.OriginArr[5], fstuct.OriginArr[6], fstuct.OriginArr[7]));
                        fstuct.Name = fstuct.OriginArr[8];
                        if (fstuct.Name != "." && fstuct.Name != "..")
                        {
                            list.Add(fstuct);
                        }
                    }

                }
            }
            else if (format == EFileListFormat.WindowsFormat)
            {
                foreach (String info in fileList)
                {
                    FileStruct fstuct = new FileStruct();
                    fstuct.Origin = info.Trim();
                    fstuct.OriginArr = fstuct.Origin.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (fstuct.OriginArr.Length == 4)
                    {
                        DateTimeFormatInfo usDate = new CultureInfo("en-US", false).DateTimeFormat;
                        usDate.ShortTimePattern = "t";
                        fstuct.UpdateTime = DateTime.Parse(fstuct.OriginArr[0] + " " + fstuct.OriginArr[1], usDate);

                        fstuct.IsDirectory = (fstuct.OriginArr[2] == "<DIR>");
                        if (!fstuct.IsDirectory)
                        {
                            fstuct.Size = Convert.ToInt32(fstuct.OriginArr[2]);
                        }
                        fstuct.Name = fstuct.OriginArr[3];
                        if (fstuct.Name != "." && fstuct.Name != "..")
                        {
                            list.Add(fstuct);
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 判断文件列表的方式Window方式还是Unix方式
        /// </summary>
        /// <param name="fileList">文件信息列表</param>
        /// <returns></returns>
        private EFileListFormat JudgeFileListFormat(String[] fileList)
        {
            foreach (String str in fileList)
            {
                if (str.Length > 10 && Regex.IsMatch(str.Substring(0, 10), "(-|d)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)"))
                {
                    return EFileListFormat.UnixFormat;
                }
                else if (str.Length > 8 && Regex.IsMatch(str.Substring(0, 8), "[0-9][0-9]-[0-9][0-9]-[0-9][0-9]"))
                {
                    return EFileListFormat.WindowsFormat;
                }
            }
            return EFileListFormat.Unknown;
        }

        private FileStruct ParseFileStructFromWindowsStyleRecord(String Record)
        {
            FileStruct f = new FileStruct();
            String processstr = Record.Trim();
            String dateStr = processstr.Substring(0, 8);
            processstr = (processstr.Substring(8, processstr.Length - 8)).Trim();
            String timeStr = processstr.Substring(0, 7);
            processstr = (processstr.Substring(7, processstr.Length - 7)).Trim();
            DateTimeFormatInfo myDTFI = new CultureInfo("en-US", false).DateTimeFormat;
            myDTFI.ShortTimePattern = "t";
            f.UpdateTime = DateTime.Parse(dateStr + " " + timeStr, myDTFI);
            if (processstr.Substring(0, 5) == "<DIR>")
            {
                f.IsDirectory = true;
                processstr = (processstr.Substring(5, processstr.Length - 5)).Trim();
            }
            else
            {
                String[] strs = processstr.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);   // true);
                processstr = strs[1];
                f.IsDirectory = false;
            }
            f.Name = processstr;
            return f;
        }
        #endregion

        #region 文件结构

        public struct FileStruct
        {
            public String Origin { get; set; }
            public String[] OriginArr { get; set; }
            public String Flags { get; set; }
            /// <summary>
            /// 所有者
            /// </summary>
            public String Owner { get; set; }
            public String Group { get; set; }
            /// <summary>
            /// 是否为目录
            /// </summary>
            public Boolean IsDirectory { get; set; }
            /// <summary>
            /// 文件或目录更新时间
            /// </summary>
            public DateTime UpdateTime { get; set; }
            /// <summary>
            /// 文件或目录名称
            /// </summary>
            public String Name { get; set; }
            /// <summary>
            /// 文件大小(目录始终为0)
            /// </summary>
            public int Size { get; set; }
        }

        #endregion
    }
}