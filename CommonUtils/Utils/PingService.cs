using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace CommonUtils
{
    /// <summary>
    /// Ping 服务
    ///http://zhanzhang.baidu.com/tools/ping
    ///http://www.google.cn/intl/zh-CN/help/blogsearch/pinging_API.html
    /// http://www.mypagerank.net/service_pingservice_index
    /// https://lanbing.org/rsspingurl.cgi
    /// </summary>
    public class PingService
    {
        public static void Send(String Url)
        {
            Uri Uri = new Uri(Url);

            HttpResponse Response = HttpContext.Current.Response;
            HttpServerUtility Server = HttpContext.Current.Server;

            //List<PingPoint> list = new List<PingPoint>();
            //list.Add(new PingPoint { RPC = "http://ping.baidu.com/ping/RPC2", MethodName = "weblogUpdates.extendedPing", NodePath = "/methodResponse/params/param/value/int", Success = "0", Failure = "1" });
            //list.Add(new PingPoint { RPC = "http://blogsearch.google.com/ping/RPC2", MethodName = "weblogUpdates.extendedPing", NodePath = "/methodResponse/params/param/value/struct/member/value/boolean", Success = "0", Failure = "1" });
            //XmlSerializer serializer = new XmlSerializer(typeof(List<PingPoint>), new XmlRootAttribute("Service"));
            //FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("/App_Data/PingService.xml"), FileMode.Create, FileAccess.Write);
            //serializer.Serialize(fs, list);
            //fs.Close();

            String file = Server.MapPath("/App_Data/PingService.xml");
            StreamReader sr = new StreamReader(file);
            XmlSerializer serializer = new XmlSerializer(typeof(List<PingPoint>), new XmlRootAttribute("Service"));
            List<PingPoint> list = serializer.Deserialize(sr) as List<PingPoint>;
            sr.Close();

            foreach (PingPoint site in list)
            {
                Execute(Uri, site);
            }
            Response.Write("<br/>\r");
            Response.Flush();
        }

        protected static void Execute(Uri uri, PingPoint site)
        {
            String result = String.Empty;
            MemoryStream ms = new MemoryStream();
            Uri siteUrl = new Uri(site.RPC);
            HttpResponse Response = HttpContext.Current.Response;

            using (XmlTextWriter data = new XmlTextWriter(ms, Encoding.UTF8))
            {
                data.Formatting = System.Xml.Formatting.Indented;
                data.Indentation = 3;

                data.WriteStartDocument();
                data.WriteStartElement("methodCall");
                data.WriteElementString("methodName", site.MethodName);
                data.WriteStartElement("params");

                data.WriteStartElement("param");
                data.WriteStartElement("value");
                data.WriteElementString("String", uri.Scheme + "://" + uri.Host);
                data.WriteEndElement();
                data.WriteEndElement();

                data.WriteStartElement("param");
                data.WriteStartElement("value");
                data.WriteElementString("String", uri.AbsoluteUri);
                data.WriteEndElement();
                data.WriteEndElement();

                data.WriteEndElement();
                data.WriteEndElement();
            }
            //Response.Write(Encoding.UTF8.GetString(ms.ToArray()));
            //Response.End();

            using (System.Net.WebClient web = new System.Net.WebClient())
            {
                try
                {
                    Byte[] byt = web.UploadData(siteUrl.AbsoluteUri, ms.ToArray());
                    if (web.ResponseHeaders["Content-Type"].IndexOf("text/xml") > -1)
                    {
                        result = Encoding.UTF8.GetString(byt);
                        if (!String.IsNullOrEmpty(result))
                        {
                            XmlDocument xml = new XmlDocument();
                            xml.LoadXml(result);
                            XmlNode node = xml.SelectSingleNode(site.NodePath);
                            String txt = node.InnerXml;
                            Response.Write(String.Format("{0}={1}\t\t", siteUrl.Host, txt));
                        }
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Response is HttpWebResponse)
                    {
                        HttpStatusCode statuscode = ((HttpWebResponse)ex.Response).StatusCode;
                        Response.Write(String.Format("{0}={1}\t\t", siteUrl.Host, statuscode));
                    }
                }
                finally
                {
                    ms.Dispose();
                    web.Dispose();
                }
            }
        }
    }

    /// <summary>
    /// Ping 服务请求节点
    /// </summary>
    [Serializable]
    [XmlType("item")]
    public class PingPoint
    {
        /// <summary>
        /// RPC端点
        /// </summary>
        public String RPC { get; set; }
        /// <summary>
        /// 调用方法名
        /// </summary>
        public String MethodName { get; set; }
        /// <summary>
        /// 返回节点Xpath
        /// </summary>
        public String NodePath { get; set; }
        /// <summary>
        /// 执行成功值
        /// </summary>
        public String Success { get; set; }
        /// <summary>
        /// 执行失败值
        /// </summary>
        public String Failure { get; set; }
    }
}
