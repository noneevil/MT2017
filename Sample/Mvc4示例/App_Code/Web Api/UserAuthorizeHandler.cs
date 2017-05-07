using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Mvc4Example.Api
{
    /// <summary>
    /// 登录验证
    /// </summary>
    public class UserAuthorizeHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Int32? Id = UserId;
            if (Id != null)
            {
                //用户已经登录,设置当前处理线程用户及角色信息
                GenericIdentity gid = new GenericIdentity(Id.Value.ToString(), UserType.ToString());
                GenericPrincipal principal = new GenericPrincipal(gid, null);
                Thread.CurrentPrincipal = principal;
                //if (HttpContext.Current != null) HttpContext.Current.User = principal;

                return base.SendAsync(request, cancellationToken);
            }
            else
            {
                //未登录返回 401
                var status = new { status = false, text = "未登录." };
                var respText = JsonConvert.SerializeObject(status);
                var response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent(respText, Encoding.UTF8, "application/json")
                };
                var task = new TaskCompletionSource<HttpResponseMessage>();
                task.SetResult(response);
                return task.Task;
            }
            //return base.SendAsync(request, cancellationToken);
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        protected int? UserId
        {
            get
            {
                Object obj = HttpContext.Current.Session["UserId"];
                if (obj != null) return Convert.ToInt32(obj);
                return null;
            }
        }
        /// <summary>
        /// 用户类型
        /// </summary>
        protected int UserType
        {
            get
            {
                Object obj = HttpContext.Current.Session["UserType"];
                if (obj != null) return Convert.ToInt32(obj);
                return 0;
            }
        }
    }
}