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
    /// 
    /// </summary>
    [Serializable]
    [Table("T_Radios")]
    public class T_RadiosEntities
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        [JsonProperty("id")]
        public Int32 id { get; set; }
        /// <summary>
        /// guide_id
        /// </summary>
        [JsonProperty("id")]
        public String guide_id { get; set; }
        /// <summary>
        /// genre_id
        /// </summary>
        [JsonProperty("id")]
        public String genre_id { get; set; }
        /// <summary>
        /// preset_id
        /// </summary>
        [JsonProperty("id")]
        public String preset_id { get; set; }
        /// <summary>
        /// now_playing_id
        /// </summary>
        [JsonProperty("playing_id")]
        public String now_playing_id { get; set; }
        /// <summary>
        /// text
        /// </summary>
        [JsonProperty("text")]
        public String text { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        [JsonProperty("URL")]
        public String URL { get; set; }
        /// <summary>
        /// URL2
        /// </summary>
        [JsonProperty("URL2")]
        public String URL2 { get; set; }
        /// <summary>
        /// bitrate
        /// </summary>
        [JsonProperty("bitrate")]
        public String bitrate { get; set; }
        /// <summary>
        /// reliability
        /// </summary>
        [JsonProperty("reliability")]
        public String reliability { get; set; }
        /// <summary>
        /// subtext
        /// </summary>
        [JsonProperty("subtext")]
        public String subtext { get; set; }
        /// <summary>
        /// formats
        /// </summary>
        [JsonProperty("formats")]
        public String formats { get; set; }
        /// <summary>
        /// streams
        /// </summary>
        [JsonProperty("streams")]
        public String streams { get; set; }
        /// <summary>
        /// item
        /// </summary>
        [JsonProperty("item")]
        public String item { get; set; }
        /// <summary>
        /// image
        /// </summary>
        [JsonProperty("image")]
        public String image { get; set; }
        /// <summary>
        /// has_profile
        /// </summary>
        [JsonProperty("profile")]
        public String has_profile { get; set; }
    }
}