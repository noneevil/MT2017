using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSite.Interface.Baidu
{
    class FriendsService : BaseService, IFriendsService
    {
        public FriendsService(BaiduApiInvoker invoker)
            : base(invoker)
        {

        }

        #region IFriendsService 成员

        public string GetFriends(uint page_no, uint page_size, uint sort_type)
        {
            Dictionary<string, string> appParamters = new Dictionary<string, string>();

            appParamters.Add("page_no", page_no.ToString());
            appParamters.Add("page_size", page_size.ToString());
            appParamters.Add("sort_type", sort_type.ToString());

            return this.MakeApiCall("friends/getFriends", appParamters);
        }

        public string AreFriends(string uids1, string uids2)
        {
            Dictionary<string, string> appParamters = new Dictionary<string, string>();

            appParamters.Add("uids1", uids1);
            appParamters.Add("uids2", uids2);

            return this.MakeApiCall("friends/areFriends", appParamters);
        }

        #endregion


        #region IFormatOperation 成员

        public RestFormat Format
        {
            get
            {
                return this.restFormat;
            }
            set
            {
                this.restFormat = value;
            }
        }

        public IFriendsService XMLFormatServer
        {
            get
            {
                this.restFormat = RestFormat.Xml;
                return this;
            }
        }

        public IFriendsService JsonFormatServer
        {
            get
            {
                this.restFormat = RestFormat.Json;
                return this;
            }
        }

        #endregion
    }
}
