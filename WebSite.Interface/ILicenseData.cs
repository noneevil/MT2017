using System;
using Newtonsoft.Json;

namespace WebSite.Interface
{
    public struct ILicenseData
    {
        /// <summary>
        /// 企业或单位名称
        /// </summary>
        [JsonProperty("A")]
        public String CompanyName { get; set; }
        /// <summary>
        /// 网站域名
        /// </summary>
        [JsonProperty("B")]
        public String DomainName { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        [JsonProperty("C")]
        public UInt32 UnixEndTime { get; set; }
        /// <summary>
        /// 到期时间
        /// </summary>
        [JsonIgnore]
        public DateTime EndTime
        {
            get
            {
                DateTime converted = new DateTime(1970, 1, 1);
                DateTime newdatetime = converted.AddSeconds(UnixEndTime);
                return newdatetime.ToLocalTime();
            }
        }
        /// <summary>
        /// GUID
        /// </summary>
        [JsonProperty("D")]
        public Guid Guid { get; set; }
        /// <summary>
        /// 加密次数
        /// </summary>
        [JsonProperty("E")]
        public Int32 Count { get; set; }
    }
}
