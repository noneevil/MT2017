using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TestSite.Models
{
    public class T_SiteUrl
    {
        /// <summary>
        /// 网址编号
        /// </summary>
        [DisplayName("网址编号")]
        public Int32 ID { get; set; }
        /// <summary>
        /// 网站地址
        /// </summary>
        [Required]
        [DisplayName("网站地址")]
        [DataType(DataType.Url)]
        public String Url { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        [DisplayName("用户编号")]
        public Int32 UserID { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DisplayName("创建日期")]
        public DateTime JoinTime { get; set; }
    }
}