using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TestSite.Models;

namespace TestSite.Controllers
{
    [HandleError]
    public class UserController : Controller
    {
        protected override void Initialize(RequestContext Context)
        {

            base.Initialize(Context);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(T_User user)
        {
            return View();
        }

        public ActionResult LogOn()
        {
            return View();
        }
    }
}