using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSite.Interface.Baidu
{

    class AuthService:BaseService,IAuthService
    {
        public AuthService(BaiduApiInvoker invoker)
            : base(invoker)
        { 
        
        }

        #region IAuthService 成员

        public string ExpireSession()
        {
            return this.MakeApiCall("passport/auth/expireSession", null);
        }

        public string RevokeAuthorization(uint uid)
        {
            Dictionary<string, string> appParamters = new Dictionary<string, string>();
            if (uid != 0)
            {
                appParamters.Add("uid", uid.ToString());
            }

            return this.MakeApiCall("passport/auth/revokeAuthorization", appParamters);
        }

        public string RevokeAuthorization()
        {
            return RevokeAuthorization(0);
        }

        public string RevokeClientAuthorization()
        {
            return this.MakeApiCall("passport/auth/revokeClientAuthorization",null);
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

        public IAuthService XMLFormatServer
        {
            get
            {
                this.restFormat = RestFormat.Xml;
                return this;
            }
        }

        public IAuthService JsonFormatServer
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
