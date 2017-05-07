using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mvc4Example
{
    /// <summary>
    /// 登录验证
    /// </summary>
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (String.IsNullOrEmpty(UserId))
            {
                //方式一 跳转
                //跳转到登录页面
                filterContext.Result = new RedirectToRouteResult("", new RouteValueDictionary(new
                {
                    controller = "Member",
                    action = "LogOn"
                }));
                //方式二 结束
                //HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                base.OnAuthorization(filterContext);
            }
        }
        /// <summary>
        /// 登录用户ID
        /// </summary>
        protected String UserId
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session["admin"]);
            }
        }

        //protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        //{
        //    base.HandleUnauthorizedRequest(filterContext);
        //}
        /// <summary>
        /// 返回是否通过认证
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (String.IsNullOrEmpty(UserId))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}