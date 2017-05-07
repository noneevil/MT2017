using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel.Activation;
using System.IO;
using SiteService.Model;
using Newtonsoft.Json;
using System.Web;
using System.Web.SessionState;
using System.Web.Profile;
using System.Web.Security;
using System.Web.Services;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

namespace SiteService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class Service : IService
    {
        #region IService 成员
        //[WebMethod(EnableSession = true)]
        //[PrincipalPermission(SecurityAction.Demand, Role = "Administrators")]
        public Object UserLogin(Stream InputStream)
        {
            //var bf = new BinaryFormatter();
            // cookieContainer = bf.Deserialize(fs) as CookieContainer;  


            User user;
            Session.Timeout = 1;
            if (Session["user"] == null)
            {
                String data = new StreamReader(InputStream).ReadToEnd();
                user = (User)JsonConvert.DeserializeObject(data, typeof(User));
                Session["user"] = user;
            }
            else
            {
                user = (User)Session["user"];
                user.SessionID = Session.SessionID;
                user.IsAuthenticated = true;
            }
            user.LastLoginTime = DateTime.Now;

            //HttpCookie cook = new HttpCookie("User", user.UserName);
            //Response.Cookies.Add(cook);
            //Response.Cookies.Remove("User");

            //FormsAuthentication.SetAuthCookie(user.UserName, false);
            //FormsAuthentication.SignOut();
            //Session.Clear();
            //Session.Abandon();

            Response.Write(JsonConvert.SerializeObject(user));

            return null;
            //return new { data = data };
            //return new { DateTime = DateTime.Now };
        }
        public string HelloWorld()
        {
            return "HelloWorld";
        }


        #endregion

        #region 服务器成员

        protected HttpRequest Request { get { return HttpContext.Current.Request; } }

        protected HttpResponse Response { get { return HttpContext.Current.Response; } }

        protected HttpServerUtility Server { get { return HttpContext.Current.Server; } }

        protected HttpSessionState Session { get { return HttpContext.Current.Session; } }

        protected ProfileBase Profile { get { return HttpContext.Current.Profile; } }

        #endregion
    }
}
