using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mvc4Example.Models
{
    /// <summary>
    /// 城市数据表
    /// </summary>
    [Serializable]
    [Table("T_City")]
    public class T_CityEntities
    {
        /// <summary>
        /// 城市编号
        /// </summary>
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("Id")]
        public Int32 City_Id { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        [JsonProperty("Name")]
        public String City_Name { get; set; }
        /// <summary>
        /// 省份编号
        /// </summary>
        [JsonProperty("Pid")]
        public Int32 Province_Id { get; set; }
        /// <summary>
        /// 拼音缩写
        /// </summary>
        [JsonProperty("Letter")]
        public String City_Letter { get; set; }
    }
}