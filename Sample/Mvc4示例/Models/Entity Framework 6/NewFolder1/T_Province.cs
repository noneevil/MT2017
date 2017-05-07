using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mvc4Example.Models
{
    /// <summary>
    /// 省份数据表
    /// </summary>
    [Serializable]
    [Table("T_Province")]
    public class T_ProvinceEntities
    {
        /// <summary>
        /// 省份编号
        /// </summary>
        [Key]//主键
        [JsonProperty("Id")]
        public Int32 Province_Id { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        [JsonProperty("Name")]
        public String Province_Name { get; set; }
        /// <summary>
        /// 拼音缩写
        /// </summary>
        [JsonProperty("Letter")]
        public String Province_Letter { get; set; }

        [ForeignKey("Province_Id")]//外键
        public List<T_CityEntities> Citys { get; set; }
    }
}