using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc4Example.Models
{
    /// <summary>
    /// 区县数据表
    /// </summary>
    [Serializable]
    [Table("T_Area")]
    public class T_AreaEntities
    {
        /// <summary>
        /// 区县编号
        /// </summary>
        [Key]
        //[JsonProperty("Id")]
        public Int32 Area_Id { get; set; }
        /// <summary>
        /// 区县名称
        /// </summary>
        //[JsonProperty("Name")]
        public String Area_Name { get; set; }
        /// <summary>
        /// 城市编号
        /// </summary>
        //[JsonProperty("Id")]
        public Int32 City_Id { get; set; }
    }
}