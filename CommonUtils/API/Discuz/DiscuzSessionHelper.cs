using System;
using CommonUtils.Discuz.Toolkit;

namespace CommonUtils.Discuz
{
    public class DiscuzSessionHelper
    {
        private static String apikey, secret, url;
        private static DiscuzSession ds;
        static DiscuzSessionHelper()
        {
            String strXmlFile = System.Web.HttpContext.Current.Server.MapPath("~/_data/config/site.config");
            XmlControl XmlTool = new XmlControl(strXmlFile);
            apikey = XmlTool.GetText("Root/ForumAPIKey");//API Key
            secret = XmlTool.GetText("Root/ForumSecret");//密钥
            url = XmlTool.GetText("Root/ForumUrl");//论坛地址
            XmlTool.Dispose();
            ds = new DiscuzSession(apikey, secret, url);
        }

        public static DiscuzSession GetSession()
        {
            return ds;
        }
    }
}
