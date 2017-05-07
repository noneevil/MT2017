using System;
using Newtonsoft.Json;

namespace CommonUtils.Discuz.Toolkit
{
    public class Reply
    {
        [JsonPropertyAttribute("tid")]
        public int Tid;

        [JsonPropertyAttribute("fid")]
        public int Fid;

        [JsonPropertyAttribute("message")]
        public String Message;


        [JsonPropertyAttribute("title")]
        public String Title;
    }
}
