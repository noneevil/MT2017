using System;
using System.Collections.Generic;
using System.Text;

namespace WebSite.Interface.Sina
{
    public class SinaUser
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 微博名称
        /// </summary>
        public string screen_name { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 微博名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string location { get; set; }
        /// <summary>
        /// 一句话介绍
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 博客地址
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string profile_image_url { get; set; }
        /// <summary>
        /// 微博地址
        /// </summary>
        public string domain { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string gender { get; set; }
        /// <summary>
        /// 粉丝数
        /// </summary>
        public int followers_count { get; set; }
        /// <summary>
        /// 关注数
        /// </summary>
        public int friends_count { get; set; }
        /// <summary>
        /// 微博数
        /// </summary>
        public int statuses_count { get; set; }
        /// <summary>
        /// 收藏数
        /// </summary>
        public int favourites_count { get; set; }
        public bool following { get; set; }
    }

    public class SinaStatus
    { }

    public class SinaComment
    { }
}
