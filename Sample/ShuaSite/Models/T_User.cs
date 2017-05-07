using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace TestSite.Models
{
    public class T_User
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        [DisplayName("用户编号")]
        public Int32 ID { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        [Required]
        [DisplayName("用户名称")]
        public String UserName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        [Required]
        [DisplayName("登录密码")]
        [DataType(DataType.Password)]
        public String PassWord { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        [Required]
        [DisplayName("确认密码")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// 剩余流量点数
        /// </summary>
        [DisplayName("剩余流量点数")]
        public Int32 PlusScore { get; set; }
        /// <summary>
        /// 累计流量点数
        /// </summary>
        [DisplayName("累计流量点数")]
        public Int32 CountScore { get; set; }
        /// <summary>
        /// 获赠积分
        /// </summary>
        [DisplayName("获赠积分")]
        public Int32 GiftScore { get; set; }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        [Required]
        [DisplayName("电子邮件地址")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        [DisplayName("用户状态")]
        public Boolean Status { get; set; }
        /// <summary>
        /// 网站名称
        /// </summary>
        [Required]
        [DisplayName("网站名称")]
        public String SiteName { get; set; }
        /// <summary>
        /// 网站LOGO
        /// </summary>
        [DisplayName("网站LOGO")]
        [DataType(DataType.ImageUrl)]
        public String SiteLogo { get; set; }
        /// <summary>
        /// 网站地址
        /// </summary>
        [Required]
        [DisplayName("网站地址")]
        [DataType(DataType.Url)]
        public String SiteUrl { get; set; }
        /// <summary>
        /// 0-积分累积模式 1-流量交换模式d
        /// </summary>
        [DisplayName("交换模式")]
        public Boolean Mode { get; set; }
        /// <summary>
        /// 注册日期
        /// </summary>
        [DisplayName("注册日期")]
        [DataType(DataType.DateTime)]
        public DateTime JoinTime { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        [DisplayName("最后登录时间")]
        [DataType(DataType.DateTime)]
        public DateTime LastLoginTime { get; set; }
    }
}