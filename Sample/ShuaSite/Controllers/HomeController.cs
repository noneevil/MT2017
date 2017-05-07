using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
using MSSQLDB;
using System.Data.SQLite;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using HtmlAgilityPack;
using System.Collections;
using CommonUtils;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Text.RegularExpressions;
using TestSite.Models;
using Newtonsoft.Json;
using System.Net;

namespace TestSite.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DateTime now = DateTime.Now;
            Random r = new Random();
            Stopwatch w = new Stopwatch();
            w.Start();

            #region 查询数据
            //DataTable ds = db.ExecuteDataTable("SELECT * FROM T_SiteUrl");
            //foreach (DataRow dr in ds.Rows)
            //{
            //    Response.Write(dr["jointime"] + "\t");
            //    Response.Flush();
            //}
            #endregion

            #region 创建表
            //            String sql = @"CREATE TABLE [T_SiteUrl] (
            //                                                    [ID] INTEGER  PRIMARY KEY NOT NULL,
            //                                                    [Url] NVARCHAR(255)  NOT NULL,
            //                                                    [UserID] INTEGER  NOT NULL,
            //                                                    [JoinTime] TIMESTAMP default (datetime('now', 'localtime')),
            //                                                    [Content] TEXT  NULL
            //                                                    )";
            //            db.ExecuteCommand(sql);
            #endregion

            #region 插入数据
            ////StreamReader sr = new StreamReader(Server.MapPath("/App_Data/db.txt"));
            ////String html = sr.ReadToEnd();
            ////sr.Close();

            //for (int i = 0; i <= 100; i++)
            //{
            //    String u = "http://www.baidu.com/s?wd=" + r.Next(1, 100000000);

            //    ExecuteObject obj = new ExecuteObject();
            //    obj.tableName = "t_siteurl";
            //    obj.cmdtype = CmdType.INSERT;
            //    //obj.terms.Add("id", dr["id"]);

            //    obj.cells.Add("url", u);
            //    obj.cells.Add("userid", 100);
            //    //obj.cells.Add("content", html);
            //    db.ExecuteCommand(obj);

            //    Response.Write(i + "\t");
            //    Response.Flush();
            //}
            #endregion

            #region 更新数据
            //DataTable ds = db.ExecuteDataTable("SELECT * FROM T_SiteUrl");
            //foreach (DataRow dr in ds.Rows)
            //{
            //    String u = String.Format("http://www.google.com.hk/search?q={0} site:www.wieui.com", list[r.Next(0, list.Count)]);

            //    ExecuteObject obj = new ExecuteObject();
            //    obj.tableName = "t_siteurl";
            //    obj.cmdtype = CmdType.UPDATE;
            //    obj.terms.Add("id", dr["id"]);

            //    obj.cells.Add("url", u);
            //    db.ExecuteCommand(obj);

            //    Response.Write(dr["id"] + "\t");
            //    Response.Flush();
            //}
            #endregion

            w.Stop();

            //Response.Write(String.Format("<hr/>{0}分{1}秒{2}毫秒", w.Elapsed.Minutes, w.Elapsed.Seconds, w.Elapsed.Milliseconds));
            //Response.End();

            return View();
        }
        public int GetSum(int? i_A, int? i_B)
        {
            int i_a = i_A ?? 1;
            int i_b = i_B ?? 2;
            return i_a + i_b;
        }
        //http://seo.zix.cc/
        //http://tool.lusongsong.com/seo/
        [Compress]
        public ActionResult SEO(SEO data)
        {
            if (!String.IsNullOrEmpty(Request["SiteUrl"]))
            {
                try
                {
                    Int32 PageCount = 0;
                    Int32 PageSize = 10;
                    Int32 PageIndex = data.Page;
                    Int32 RecordCount = 0;

                    Uri uri = new Uri(data.SiteUrl);
                    String _RecordCount = (String)db.ExecuteScalar("SELECT COUNT(id) FROM [t_submitsite]");
                    Int32.TryParse(_RecordCount, out RecordCount);

                    PageCount = (Int32)Math.Ceiling((Decimal)RecordCount / (Decimal)PageSize);

                    if (PageIndex < 1) PageIndex = 1;
                    if (PageIndex > PageCount) PageIndex = PageCount;

                    String Url = String.Format("/{0}/{1}/{2}/{3}?SiteUrl={4}",
                        RouteData.Values["action"], "{0}", data.auto, data.time, HttpUtility.UrlEncode(Request["SiteUrl"]));

                    String sql = String.Format("SELECT * FROM [t_submitsite] LIMIT {0} OFFSET {1}", PageSize, PageSize * (PageIndex - 1));
                    DataTable ds = db.ExecuteDataTable(sql);
                    foreach (DataRow dr in ds.Rows)
                    {
                        String _url = String.Format(dr["url"].ToString(), uri.Host);
                        dr["url"] = _url;
                    }

                    ViewData["NextPage"] = String.Format(Url, PageIndex + 1);
                    ViewData["Host"] = uri.Scheme + "://" + uri.Host;
                    ViewData["Table"] = ds;
                    ViewData["PageString"] = PrintPage(Url, "black2", PageCount, PageIndex);
                }
                catch
                {
                    Response.Write("错误的域名!");
                    Response.End();
                }
            }
            return View(data);
        }

        //[Authorize]
        public ActionResult Alexa()
        {
            if (Session["RefreshTime"] != null)
            {
                TimeSpan s = DateTime.Now - (DateTime)Session["RefreshTime"];
                if (s.Seconds < 10)
                {
                    Response.Write("刷新过快！请正确刷站！");
                    Response.End();
                }
            }

            Session["RefreshTime"] = DateTime.Now;
            String url = (String)db.ExecuteScalar("SELECT url FROM T_SiteUrl ORDER BY random() LIMIT 1");
            ViewData["url"] = url;
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [NonAction]
        private String PrintPage(String Url, String css, Int32 PageCount, Int32 PageIndex)
        {
            String str = String.Empty;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<div class=\"{0}\">", css);

            if (PageCount > 1)
            {
                if (PageIndex == 1)//第一页
                {
                    sb.Append("<span class=\"disabled\">&#171;</span><span class=\"disabled\">&#139;</span>");
                }
                else
                {
                    sb.AppendFormat("<a href=\"{0}\" title=\"首页\">&#171;</a><a href=\"{1}\" title=\"上一页\">&#139;</a>", String.Format(Url, 1), String.Format(Url, PageIndex - 1));
                }
                //***************************************************************************************
                int nPage2 = PageIndex; //前进页面
                int nPage3 = PageIndex - 6;//后退页面
                if (PageCount > 10 && PageIndex > 6)//输出页数
                {
                    for (int j = 1; j <= 5; j++)
                    {
                        nPage3 = nPage3 + 1;
                        sb.AppendFormat("<a href=\"{0}\">{1}</a>", String.Format(Url, nPage3), nPage3);
                    }
                    for (int i = 1; i <= 5; i++)
                    {
                        if (nPage2 > PageCount) break;
                        if (nPage2 == PageIndex) //当前页
                        {
                            sb.AppendFormat("<span class=\"current\">{0}</span>", nPage2);

                        }
                        else
                        {
                            sb.AppendFormat("<a href=\"{0}\">{1}</a>", String.Format(Url, nPage2), nPage2);
                        }
                        nPage2 = nPage2 + 1;
                    }
                }
                else
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        if (i > PageCount) break;
                        if (i == PageIndex) //当前页
                        {
                            sb.AppendFormat("<span class=\"current\">{0}</span>", i);
                        }
                        else
                        {
                            sb.AppendFormat("<a href=\"{0}\">{1}</a>", String.Format(Url, i), i);
                        }
                    }
                }
                //***************************************************************************************
                if (PageIndex == PageCount)//最后一页
                {
                    sb.Append("<span class=\"disabled\">&#155;</span><span class=\"disabled\">&#187;</span>");
                }
                else
                {
                    sb.AppendFormat("<a href=\"{0}\" title=\"下一页\">&#155;</a><a href=\"{1}\" title=\"尾页\">&#187;</a>", String.Format(Url, PageIndex + 1), String.Format(Url, PageCount));
                }
            }

            //sb.AppendFormat("<b>共&nbsp;{0}&nbsp;条记录&nbsp;&nbsp;当前{1}/{2}页</b></div>", RecordCount, PageIndex, PageCount);
            //sb.AppendFormat("<b>{1}/{2}页</b></div>", RecordCount, PageIndex, PageCount);
            sb.AppendLine("</div>");

            return sb.ToString();
        }
    }
    /// <summary>
    /// 客户端缓存设置
    /// </summary>
    public class CacheAttribute : ActionFilterAttribute
    {
        public CacheAttribute()
        {
            Day = 7;
            Cacheability = HttpCacheability.Private;
        }
        /// <summary>
        /// 缓存时间(单位：天)
        /// </summary>
        public Double Day { get; set; }
        public HttpCacheability Cacheability { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (Day <= 0) return;
            HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
            TimeSpan Duration = TimeSpan.FromDays(Day);
            cache.SetExpires(DateTime.Now.Add(Duration));
            cache.SetMaxAge(Duration);
            cache.SetLastModified(DateTime.Now);
            cache.SetCacheability(Cacheability);
            base.OnActionExecuted(filterContext);
        }
    }
    /// <summary>
    /// 压缩HTML
    /// </summary>
    public class CompressAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;
            //response.Write("AAAAAAAAAAAAAAA");
            response.Filter = new WhiteSpaceFilter(response.Filter, s =>
            {
                //([\r\n])[\s]+
                //s = Regex.Replace(s, @"\s+(?=<)|\s+$|(?<=>)\s+", "");
                s = Regex.Replace(s, @"([\r\n])[\s]+", "");

                //single-line doctype must be preserved      
                //var firstEndBracketPosition = s.IndexOf(">");
                //if (firstEndBracketPosition >= 0)
                //{
                //    s = s.Remove(firstEndBracketPosition, 1);
                //    s = s.Insert(firstEndBracketPosition, ">");
                //}
                return s;
            });
        }
    }
    public class WhiteSpaceFilter : Stream
    {
        private Stream _shrink;
        private Func<String, String> _filter;

        public WhiteSpaceFilter(Stream shrink, Func<String, String> filter)
        {
            _shrink = shrink;
            _filter = filter;
        }
        public override Boolean CanRead { get { return true; } }
        public override Boolean CanSeek { get { return true; } }
        public override Boolean CanWrite { get { return true; } }
        public override void Flush() { _shrink.Flush(); }
        public override long Length { get { return 0; } }
        public override long Position { get; set; }
        public override int Read(Byte[] buffer, int offset, int count)
        {
            return _shrink.Read(buffer, offset, count);
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _shrink.Seek(offset, origin);
        }
        public override void SetLength(long value)
        {
            _shrink.SetLength(value);
        }
        public override void Close()
        {
            _shrink.Close();
        }
        public override void Write(Byte[] buffer, int offset, int count)
        {
            // capture the data and convert to String
            Byte[] data = new Byte[count];
            Buffer.BlockCopy(buffer, offset, data, 0, count);
            String s = Encoding.UTF8.GetString(buffer);
            // filter the String
            s = _filter(s);
            // write the data to stream
            Byte[] outdata = Encoding.UTF8.GetBytes(s);
            _shrink.Write(outdata, 0, outdata.GetLength(0));
        }
    }

}
