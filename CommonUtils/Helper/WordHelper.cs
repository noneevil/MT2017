using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Web;
using HtmlAgilityPack;
using Microsoft.Office.Interop.Word;
/*
 * 示例:
 *  WordHelper word = new WordHelper();
 *  word.InputFile = Server.MapPath("/test.doc");
 *  word.SaveFile = Server.MapPath("/test.PDF");
 *  word.SaveFormat = WdSaveFormat.wdFormatPDF;
 *  word.Convert();
 */
namespace CommonUtils
{
    /// <summary>
    /// Word文档处理类
    /// 引用COM组件:Microsoft Word 12.0 Object Library
    /// </summary>
    public class WordHelper
    {
        /// <summary>
        /// Doc文件
        /// </summary>
        public String InputFile { get; set; }
        /// <summary>
        /// 保存文件
        /// </summary>
        public String SaveFile { get; set; }
        /// <summary>
        /// 转换类型
        /// </summary>
        public WdSaveFormat SaveFormat { get; set; }
        /// <summary>
        /// 临时文件
        /// </summary>
        protected FileInfo TempFile { get; set; }
        /// <summary>
        /// 执行转换
        /// </summary>
        public void Convert()
        {
            ApplicationClass word = new ApplicationClass();
            Type wordType = word.GetType();
            Documents docs = word.Documents;

            Type docsType = docs.GetType();
            Document document = (Document)docsType.InvokeMember("Open", BindingFlags.InvokeMethod, null, docs, new Object[] { InputFile, true, true });
            Type doctype = document.GetType();

            TempFile = new FileInfo(Server.MapPath("/UpFiles/Temp/temp") + Path.GetExtension(SaveFile));
            FileHelper.CreatePath(TempFile.FullName);

            #region
            /*
                * 下面是Microsoft Word 9 Object Library的写法，如果是10，可能写成：
                * docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod,
                * null, doc, new Object[]{saveFileName, Word.WdSaveFormat.wdFormatFilteredHTML});
                * 其它格式：
                * wdFormatHTML
                * wdFormatDocument
                * wdFormatDOSText
                * wdFormatDOSTextLineBreaks
                * wdFormatEncodedText
                * wdFormatRTF
                * wdFormatTemplate
                * wdFormatText
                * wdFormatTextLineBreaks
                * wdFormatUnicodeText
            */
            #endregion

            doctype.InvokeMember("SaveAs", BindingFlags.InvokeMethod, null, document, new Object[] { TempFile.FullName, SaveFormat });
            wordType.InvokeMember("Quit", BindingFlags.InvokeMethod, null, word, null);

            if (SaveFormat != WdSaveFormat.wdFormatFilteredHTML)
            {
                File.Copy(TempFile.FullName, SaveFile);
                ClearTemp();
            }
        }
        /// <summary>
        /// 清除Word Html标记
        /// </summary>
        public String ConvertToHtml()
        {
            this.SaveFormat = WdSaveFormat.wdFormatFilteredHTML;
            this.Convert();
            if (!TempFile.Exists) return String.Empty;
            while (FileHelper.IsFileLocked(TempFile.FullName))
            {
                System.Threading.Thread.Sleep(300);
            }

            HtmlDocument xml = new HtmlDocument();
            xml.Load(TempFile.FullName);
            //清除Word样式
            HtmlNodeCollection tempnodes = xml.DocumentNode.SelectNodes("//*[@class|@style|@lang]");
            if (tempnodes != null)
            {
                foreach (HtmlNode node in tempnodes)
                {
                    HtmlNode n = node.Clone();
                    HtmlAttributeCollection atts = n.Attributes;
                    foreach (HtmlAttribute att in atts)
                    {
                        switch (att.Name)
                        {
                            case "lang":
                            case "class":
                            case "style":
                                node.Attributes.Remove(att.Name);
                                break;
                        }
                    }
                }
            }
            //处理文档图片
            HtmlNodeCollection images = xml.DocumentNode.SelectNodes("//img[@src and not(starts-with(@src,'http'))]");
            if (images != null)
            {
                foreach (HtmlNode img in images)
                {
                    String imgfile = Path.Combine(TempFile.DirectoryName, img.Attributes["src"].Value.Trim());
                    if (File.Exists(imgfile))
                    {
                        String ex = Path.GetExtension(imgfile).ToLower();
                        String destfolder = Server.MapPath(String.Format("/UpFiles/{0}/{1}/", ex.Substring(1), DateTime.Now.ToString("yyyyMMdd")));
                        FileHelper.CreatePath(destfolder);

                        Int32 count = new DirectoryInfo(destfolder).GetFiles().Length + 1;
                        String destfile = Path.Combine(destfolder, DateTime.Now.ToString("yyyyMMddHHmmss") + count.ToString("00000") + ex);
                        File.Move(imgfile, destfile);

                        String src = "/" + new Uri(Server.MapPath("/")).MakeRelativeUri(new Uri(destfile)).ToString();
                        img.Attributes["src"].Value = src;
                    }
                }
            }
            String html = xml.DocumentNode.SelectSingleNode("//body").InnerHtml;
            ClearTemp();
            return html;
        }
        /// <summary>
        /// 执行转换
        /// </summary>
        public String ConvertToString()
        {
            ApplicationClass word = new ApplicationClass();
            Type wordType = word.GetType();
            Documents docs = word.Documents;

            Type docsType = docs.GetType();
            Document document = (Document)docsType.InvokeMember("Open", BindingFlags.InvokeMethod, null, docs, new Object[] { InputFile, true, true });
            Type doctype = document.GetType();

            TempFile = new FileInfo(Server.MapPath("/UpFiles/Temp/temp_" + Path.GetFileNameWithoutExtension(InputFile) + ".html"));
            FileHelper.CreatePath(TempFile.FullName);

            #region
            /*
                * 下面是Microsoft Word 9 Object Library的写法，如果是10，可能写成：
                * docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod,
                * null, doc, new Object[]{saveFileName, Word.WdSaveFormat.wdFormatFilteredHTML});
                * 其它格式：
                * wdFormatHTML
                * wdFormatDocument
                * wdFormatDOSText
                * wdFormatDOSTextLineBreaks
                * wdFormatEncodedText
                * wdFormatRTF
                * wdFormatTemplate
                * wdFormatText
                * wdFormatTextLineBreaks
                * wdFormatUnicodeText
            */
            #endregion

            doctype.InvokeMember("SaveAs", BindingFlags.InvokeMethod, null, document, new Object[] 
            { 
                TempFile.FullName, WdSaveFormat.wdFormatFilteredHTML 
            });
            wordType.InvokeMember("Quit", BindingFlags.InvokeMethod, null, word, null);

            if (!TempFile.Exists) return String.Empty;
            while (FileHelper.IsFileLocked(TempFile.FullName))
            {
                Thread.Sleep(100);
            }
            HtmlDocument xml = new HtmlDocument();
            xml.Load(TempFile.FullName);
            //清除Word样式
            HtmlNodeCollection tempnodes = xml.DocumentNode.SelectNodes("//*[@class|@style|@lang]");
            if (tempnodes != null)
            {
                foreach (HtmlNode node in tempnodes)
                {
                    HtmlNode n = node.Clone();
                    HtmlAttributeCollection atts = n.Attributes;
                    foreach (HtmlAttribute att in atts)
                    {
                        switch (att.Name)
                        {
                            case "lang":
                            case "class":
                            case "style":
                                node.Attributes.Remove(att.Name);
                                break;
                        }
                    }
                }
            }
            //处理文档图片
            HtmlNodeCollection images = xml.DocumentNode.SelectNodes("//img[@src and not(starts-with(@src,'http'))]");
            if (images != null)
            {
                foreach (HtmlNode img in images)
                {
                    String imgfile = Path.Combine(TempFile.DirectoryName, img.Attributes["src"].Value.Trim());
                    if (File.Exists(imgfile))
                    {
                        String ex = Path.GetExtension(imgfile).ToLower();
                        String destfolder = Server.MapPath(String.Format("/UpFiles/{0}/{1}/", ex.Substring(1), DateTime.Now.ToString("yyyyMMdd")));
                        FileHelper.CreatePath(destfolder);

                        Int32 count = new DirectoryInfo(destfolder).GetFiles().Length + 1;
                        String destfile = Path.Combine(destfolder, DateTime.Now.ToString("yyyyMMddHHmmss") + count.ToString("00000") + ex);
                        File.Move(imgfile, destfile);

                        String src = "/" + new Uri(Server.MapPath("/")).MakeRelativeUri(new Uri(destfile)).ToString();
                        img.Attributes["src"].Value = src;
                    }
                }
            }
            String html = xml.DocumentNode.SelectSingleNode("//body").InnerHtml;
            try
            {
                File.Delete(InputFile);
                TempFile.Delete();
                TempFile.Directory.Delete();
            }
            catch { }
            return html;
        }
        /// <summary>
        /// 清理临时文件
        /// </summary>
        protected void ClearTemp()
        {
            while (FileHelper.IsFileLocked(TempFile.FullName))
            {
                System.Threading.Thread.Sleep(300);
            }
            foreach (FileInfo file in TempFile.Directory.GetFiles("*.*", SearchOption.AllDirectories))
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in TempFile.Directory.GetDirectories("*.*", SearchOption.AllDirectories))
            {
                dir.Delete();
            }
        }
        protected HttpResponse Response
        {
            get
            {
                return HttpContext.Current.Response;
            }
        }
        protected HttpServerUtility Server
        {
            get
            {
                return HttpContext.Current.Server;
            }
        }
    }
}
