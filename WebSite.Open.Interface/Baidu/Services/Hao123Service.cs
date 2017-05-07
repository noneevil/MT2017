using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSite.Interface.Baidu
{
    class Hao123Service : BaseService, IHao123Service
    {
        public Hao123Service(BaiduApiInvoker invoker)
            : base(invoker)
        { 
        
        }

        #region IHao123 成员

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

        public IHao123Service JsonFormatServer
        {
            get
            {
                this.restFormat = RestFormat.Json;
                return this;
            }
        }

        public IHao123Service XMLFormatServer
        {
            get
            {
                this.restFormat = RestFormat.Xml;
                return this;
            }
        }

        public string SaveOrder(string order_id, string title, string logo, string url, int price, int goods_num, 
            int sum_price, string summary, int expire, string addr, int uid, string mobile,
            string tn, string baiduid, int bonus)
        {
            Dictionary<string, string> appParamters = new Dictionary<string, string>();
            appParamters.Add("order_id", order_id);
            appParamters.Add("title", title);
            appParamters.Add("logo", logo);
            appParamters.Add("url", url);
            appParamters.Add("price", price.ToString());
            appParamters.Add("goods_num", goods_num.ToString());
            appParamters.Add("sum_price", sum_price.ToString());
            appParamters.Add("summary", summary);
            appParamters.Add("expire", expire.ToString());

            if (!string.IsNullOrEmpty(addr))
            {
                appParamters.Add("addr", addr);
            }

            if (uid != 0)
            {
                appParamters.Add("uid", uid.ToString());
            }

            if (!string.IsNullOrEmpty(mobile))
            {
                appParamters.Add("mobile", mobile);
            }

            if (!string.IsNullOrEmpty(tn))
            {
                appParamters.Add("tn", tn);
            }

            if (!string.IsNullOrEmpty(baiduid))
            {
                appParamters.Add("baiduid", baiduid);
            }

            appParamters.Add("bonus", bonus.ToString());

            return this.MakeApiCall("hao123/saveOrder",appParamters);
        }

        public string UpdateExpire(string order_ids, int expire)
        {
            Dictionary<string, string> appParamters = new Dictionary<string, string>();
            appParamters.Add("order_ids", order_ids);
            appParamters.Add("expire", expire.ToString());

            return this.MakeApiCall("hao123/updateExpire", appParamters);

        }

        public string UseOrder(string order_id, int used_time)
        {
            Dictionary<string, string> appParamters = new Dictionary<string, string>();
            appParamters.Add("order_id", order_id);
            appParamters.Add("used_time", used_time.ToString());

            return this.MakeApiCall("hao123/useOrder", appParamters);

        }

        #endregion
    }
}
