using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSite.Interface.Baidu
{
    class UsersService : BaseService, IUsersService
    {
        public UsersService(BaiduApiInvoker invoker)
            : base(invoker)
        {

        }

        #region IUsersService 成员

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

        public string GetLoggedInUser()
        {
            return MakeApiCall("passport/users/getLoggedInUser", null);
        }

        public string GetInfo(uint uid, string fields)
        {
            string[] allFields = new string[] { "userid", "username", "realname", "userdetail", "birthday", "age",
            "marriage","sex","blood","figure","constellation","education","trade","job","secureemail","email"};

            Dictionary<string, string> appParamters = new Dictionary<string, string>();

            if (uid != 0)
            {
                appParamters.Add("uid", uid.ToString());
            }

            if (!string.IsNullOrEmpty(fields))
            {
                string[] paramFields = fields.Split(",".ToCharArray());
                bool isAllContain = true;

                foreach (string fied in paramFields)
                {
                    if (!allFields.Contains(fied))
                    {
                        isAllContain = false;
                        break;
                    }
                }
                if (!isAllContain)
                {
                    throw new Exception("GetInfo() fields参数不合法。");
                }

                appParamters.Add("fields", fields);
            }
            return this.MakeApiCall("passport/users/getInfo", appParamters);

        }

        public string GetInfo(uint uid)
        {
            return GetInfo(uid, null);
        }

        public string GetInfo(string fields)
        {
            return GetInfo(0, fields);
        }

        public string GetInfo()
        {
            return GetInfo(0, null);
        }

        public string IsAppUser(uint uid, int appid)
        {
            Dictionary<string, string> appParamters = new Dictionary<string, string>();

            if (uid != 0)
            {
                appParamters.Add("uid", uid.ToString());
            }

            if (appid != 0)
            {
                appParamters.Add("appid", appid.ToString());
            }

            return this.MakeApiCall("passport/users/isAppUser", appParamters);

        }

        public string IsAppUser(uint uid)
        {
            return IsAppUser(uid, 0);
        }

        public string IsAppUser(int appid)
        {
            return IsAppUser(0, appid);
        }

        public string IsAppUser()
        {
            return IsAppUser(0, 0);
        }

        public string HasAppPermission(string ext_perm, uint uid)
        {
            Dictionary<string, string> appParamters = new Dictionary<string, string>();

            if (uid != 0)
            {
                appParamters.Add("uid", uid.ToString());
            }

            if (!string.IsNullOrEmpty(ext_perm))
            {
                appParamters.Add("ext_perm", ext_perm);
            }

            return this.MakeApiCall("passport/users/hasAppPermission", appParamters);
        }

        public string HasAppPermission(string ext_perm)
        {
            return HasAppPermission(ext_perm, 0);
        }

        public string HasAppPermissions(string ext_perms, uint uid)
        {
            Dictionary<string, string> appParamters = new Dictionary<string, string>();

            if (uid != 0)
            {
                appParamters.Add("uid", uid.ToString());
            }

            if (!string.IsNullOrEmpty(ext_perms))
            {
                appParamters.Add("ext_perms", ext_perms);
            }

            return this.MakeApiCall("passport/users/hasAppPermissions", appParamters);
        }

        public string HasAppPermissions(string ext_perms)
        {
            return HasAppPermissions(ext_perms, 0);
        }



        #endregion


        #region IFormatOperation 成员


        public IUsersService XMLFormatServer
        {
            get
            {
                this.restFormat = RestFormat.Xml;
                return this;
            }
        }

        public IUsersService JsonFormatServer
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
