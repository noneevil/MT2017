using System;
using System.ServiceModel.Activation;
using System.Web.Routing;
using System.Web.Security;
using System.Web;
using System.Web.ApplicationServices;

namespace SiteService
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.Add(new ServiceRoute("api", new WebServiceHostFactory(), typeof(Service)));
            //AuthenticationService.CreatingCookie += new EventHandler<CreatingCookieEventArgs>(AuthenticationService_CreatingCookie);
        }
        void AuthenticationService_CreatingCookie(object sender, CreatingCookieEventArgs e)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                                1,
                                                e.UserName,
                                                DateTime.Now,
                                                DateTime.Now.AddMinutes(30),
                                                e.IsPersistent,
                                                e.CustomCredential,
                                                FormsAuthentication.FormsCookiePath);

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.Expires = DateTime.Now.AddMinutes(30);
            HttpContext.Current.Response.Cookies.Add(cookie);
            e.CookieIsSet = true;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 30;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}