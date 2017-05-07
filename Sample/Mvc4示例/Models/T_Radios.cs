using MSSQLDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc4Example.Models
{
    [Table("T_Radios")]
    [Serializable]
    public class T_RadiosEntity
    {
        /// <summary>
        /// id
        /// </summary>
        [Field("id", IsPrimaryKey = true)]
        public int id { get; set; }
        /// <summary>
        /// guide_id
        /// </summary>
        [Field("guide_id")]
        public string guide_id { get; set; }
        /// <summary>
        /// genre_id
        /// </summary>
        [Field("genre_id")]
        public string genre_id { get; set; }
        /// <summary>
        /// preset_id
        /// </summary>
        [Field("preset_id")]
        public string preset_id { get; set; }
        /// <summary>
        /// now_playing_id
        /// </summary>
        [Field("now_playing_id")]
        public string now_playing_id { get; set; }
        /// <summary>
        /// text
        /// </summary>
        [Field("text")]
        public string text { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        [Field("URL")]
        public string URL { get; set; }
        /// <summary>
        /// URL2
        /// </summary>
        [Field("URL2")]
        public string URL2 { get; set; }
        /// <summary>
        /// bitrate
        /// </summary>
        [Field("bitrate")]
        public string bitrate { get; set; }
        /// <summary>
        /// reliability
        /// </summary>
        [Field("reliability")]
        public string reliability { get; set; }
        /// <summary>
        /// subtext
        /// </summary>
        [Field("subtext")]
        public string subtext { get; set; }
        /// <summary>
        /// formats
        /// </summary>
        [Field("formats")]
        public string formats { get; set; }
        /// <summary>
        /// streams
        /// </summary>
        [Field("streams")]
        public string streams { get; set; }
        /// <summary>
        /// item
        /// </summary>
        [Field("item")]
        public string item { get; set; }
        /// <summary>
        /// image
        /// </summary>
        [Field("image")]
        public string image { get; set; }
        /// <summary>
        /// has_profile
        /// </summary>
        [Field("has_profile")]
        public string has_profile { get; set; }
    }
}