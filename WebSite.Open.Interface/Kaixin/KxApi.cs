using System;
using System.Collections.Generic;
using System.Text;

namespace WebSite.Interface.Kaixin
{
    public class KxApi
    {
        public string Users_Me(string apiUrl, string access_token)
        {
            HTTPBase httpManager = new HTTPBase();
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams.Add("access_token", access_token);
            return httpManager.Get(apiUrl, queryParams);
        }
    }
}
