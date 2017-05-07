using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace CommonUtils.Discuz.Toolkit
{
    public class Post
    {
        [JsonPropertyAttribute("pid")]
        [XmlElement("pid")]
        public int Pid;

        [JsonPropertyAttribute("layer")]
        [XmlElement("layer")]
        public int Layer;

        [JsonPropertyAttribute("poster_id")]
        [XmlElement("poster_id")]
        public int PosterId;

        [JsonPropertyAttribute("poster_name")]
        [XmlElement("poster_name")]
        public String PosterName;

        [JsonPropertyAttribute("title")]
        [XmlElement("title")]
        public String Title;

        [JsonPropertyAttribute("message")]
        [XmlElement("message")]
        public String Message;

        [JsonPropertyAttribute("post_date_time")]
        [XmlElement("post_date_time")]
        public String PostDateTime;

        [JsonPropertyAttribute("invisible")]
        [XmlElement("invisible")]
        public int Invisible;

        [JsonPropertyAttribute("rate")]
        [XmlElement("rate")]
        public int Rate;

        [JsonPropertyAttribute("rate_times")]
        [XmlElement("rate_times")]
        public int RateTimes;

        [JsonPropertyAttribute("use_signature")]
        [XmlElement("use_signature")]
        public int UseSignature;

        [JsonPropertyAttribute("poster_email")]
        [XmlElement("poster_email")]
        public String PosterEmail;

        [JsonPropertyAttribute("poster_show_email")]
        [XmlElement("poster_show_email")]
        public int PosterShowEmail;

        [JsonPropertyAttribute("poster_avator")]
        [XmlElement("poster_avator")]
        public String PosterAvator;

        [JsonPropertyAttribute("poster_avator_width")]
        [XmlElement("poster_avator_width")]
        public int PosterAvatorWidth;

        [JsonPropertyAttribute("poster_avator_height")]
        [XmlElement("poster_avator_height")]
        public int PosterAvatorHeight;

        [JsonPropertyAttribute("poster_signature")]
        [XmlElement("poster_signature")]
        public String PosterSignature;

        [JsonPropertyAttribute("poster_location")]
        [XmlElement("poster_location")]
        public String PosterLocation;

        [JsonPropertyAttribute("ad_index")]
        [XmlElement("ad_index")]
        public int AdIndex;
    }
}
