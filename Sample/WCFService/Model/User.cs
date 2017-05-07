using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteService.Model
{
    [Serializable]
    public class User
    {
        public String UserName { get; set; }

        public String PassWord { get; set; }

        public DateTime LastLoginTime { get; set; }

        public String SessionID { get; set; }

        public Boolean IsAuthenticated { get; set; }
    }
}