using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace WebSite.Interface.Renren.APIUtility
{
    public class APIConfig
    {
        public APIConfig()
        {
        }
        public static string Format
        {
            get { return "json"; }
        }
        public static string AuthorizationURL
        {
            get { return "https://graph.renren.com/oauth/authorize"; }
        }
        public static string AccessURL
        {
            get { return "https://graph.renren.com/oauth/token"; }
        }
        public static string SessionURL
        {
            get { return "https://graph.renren.com/renren_api/session_key"; }
        }
        public static string RenRenAPIURL
        {
            get { return "http://api.renren.com/restserver.do"; }
        }
    }
}
