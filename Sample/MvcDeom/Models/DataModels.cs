using System;
using System.Runtime.CompilerServices;
using MSSQLDB;
using Newtonsoft.Json;

namespace MvcSite.Models
{
    /// <summary>
    /// 省份表
    /// </summary>
    [Serializable]
    [CompilerGenerated]
    [Table("T_Province")]
    public class T_Province
    {
        [Field("Province_Id")]
        [JsonProperty("ID")]
        public string Province_Id { get; set; }

        [Field("Province_Name")]
        [JsonProperty("Name")]
        public string Province_Name { get; set; }

        [Field("Province_Letter")]
        [JsonProperty("Letter")]
        public string Province_Letter { get; set; }

        public T_City City { get; set; }
        //public T_Area Area { get; set; }
    }
    /// <summary>
    /// 城市表
    /// </summary>
    [Serializable]
    [Table("T_City")]
    public class T_City
    {
        [Field("City_Id")]
        [JsonProperty(PropertyName = "ID")]
        public string City_Id { get; set; }

        [Field("City_Name")]
        [JsonProperty(PropertyName = "Name")]
        public string City_Name { get; set; }

        [Field("Province_Id")]
        [JsonProperty(PropertyName = "Pid"), JsonIgnore]
        public string Province_Id { get; set; }

        [Field("City_Letter")]
        [JsonProperty(PropertyName = "Letter")]
        public string City_Letter { get; set; }

        //public T_Area Area { get; set; }
    }
    /// <summary>
    /// 地区表
    /// </summary>
    [Table("T_Area")]
    public class T_Area
    {
        [Field("Area_Id")]
        public string Area_Id { get; set; }

        [Field("Area_Name")]
        public string Area_Name { get; set; }

        [Field("City_Id")]
        public string City_Id { get; set; }
    }

    [Table("T_Product")]
    public class T_Product
    {
        public String Product_Name { get; set; }

        public String Product_Remark { get; set; }

        public DateTime Start_Time { get; set; }

        public DateTime End_Time { get; set; }
    }
    [Table("T_News")]
    public class T_News
    {
        public Int32 ID { get; set; }
        public String Title { get; set; }
        public String Content { get; set; }
        public DateTime PubDate { get; set; }
        public DateTime EditDate { get; set; }
        public String ImageUrl { get; set; }
    }
}