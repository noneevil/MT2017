using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using CommonUtils;

namespace WebSite.Interface.Sina
{
    public class SinaConfig
    {
        /// <summary>
        /// 得到Sina的AppKey
        /// </summary>
        /// <returns>string AppKey</returns>
        public static string AppKey
        {
            get
            {
                return  SessionHelper.GetSessionString("OAuth_Sina.AppKey");
            }

            set
            {
                SessionHelper.SetSession("OAuth_Sina.AppKey", value);
            }
        }

        /// <summary>
        /// 得到Sina的AppSecret
        /// </summary>
        /// <returns>string AppSecret</returns>
        public static string AppSecret
        {
            get
            {
                return SessionHelper.GetSessionString("OAuth_Sina.AppSecret");
            }

            set
            {
                SessionHelper.SetSession("OAuth_Sina.AppSecret", value);
            }
        }

        /// <summary>
        /// 得到回调地址
        /// </summary>
        /// <returns></returns>
        public static string CallBackURI
        {
            get
            {
                return SessionHelper.GetSessionString("OAuth_Sina.CallBackURI");
            }

            set
            {
                SessionHelper.SetSession("OAuth_Sina.CallBackURI", value);
            }
        }

    }
}
