using Mvc4Example.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mvc4Example.Controllers
{
    /// <summary>
    /// 基控制器
    /// </summary>
    //[MvcHandleError]//异常捕获方式一
    public class BaseController : Controller
    {
        /// <summary>
        /// 重载初始化方法
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }
        /// <summary>
        /// 异常捕获方式二
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            //构造返回数据
            //var data = new IResult
            //{
            //    data = filterContext.Exception.Message,
            //    header = RequestHeader
            //};
            //filterContext.Result = new ContentResult
            //{
            //    Content = JsonConvert.SerializeObject(data),
            //    ContentType = "application/json"
            //};
            base.OnException(filterContext);
        }

#if DEBUG
        /// <summary>
        /// 获取请求头信息
        /// </summary>
        protected String RequestHeader
        {
            get
            {
                var file = Server.MapPath("/Request.txt");
                Request.SaveAs(file, true);
                var txt = System.IO.File.ReadAllText(file);
                System.IO.File.Delete(file);
                return txt;
            }
        }
#endif
        protected new ContentResult Json(object data)
        {
            return Content(JsonConvert.SerializeObject(data), "application/json");
        }
        protected new ContentResult Json(object data, JsonRequestBehavior behavior)
        {
            return Content(JsonConvert.SerializeObject(data), "application/json");
        }

        /// <summary>
        /// 打印分页
        /// </summary>
        /// <param name="PageCount">总页数</param>
        /// <returns></returns>
        [NonAction]
        protected String PrintPage(Int32 PageCount)
        {
            StringBuilder sb = new StringBuilder(1024);

            if (PageCount > 1)
            {
                var host = String.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host, Request.Url.Port == 80 ? "" : ":" + Request.Url.Port);

                Object _Key = (RouteData.Values["pagekey"] ?? Request.QueryString["pagekey"]) ?? "page";
                var key = Convert.ToString(_Key);
                Object _Index = RouteData.Values[key] ?? Request.QueryString[key];
                Int32 PageIndex = Convert.ToInt32(_Index);
                String actionName = RouteData.GetRequiredString("action");

                RouteValueDictionary QueryParams = new RouteValueDictionary();
                if (Request.QueryString.HasKeys())
                {
                    foreach (String k in Request.QueryString.AllKeys)
                    {
                        if (String.IsNullOrEmpty(k)) continue;
                        QueryParams.Add(k, Request.QueryString[k]);
                    }
                }

                if (PageIndex < 1) PageIndex = 1;
                if (PageIndex > PageCount) PageIndex = PageCount;

                sb.Append("<div class=\"black2\">");
                if (PageIndex == 1)//第一页
                {
                    sb.Append("<span class=\"disabled\">第一页</span><span class=\"disabled\">上一页</span>");
                }
                else
                {
                    QueryParams[key] = 1;
                    sb.AppendFormat("<a href=\"{0}{1}\" title=\"首页\">第一页</a>", host, Url.Action(actionName, QueryParams));
                    QueryParams[key] = PageIndex - 1;
                    sb.AppendFormat("<a href=\"{0}{1}\" title=\"上一页\">上一页</a>", host, Url.Action(actionName, QueryParams));
                }

                int startpage = PageIndex > 6 ? PageIndex - 5 : 1;
                if (startpage > (PageCount - 9)) startpage = PageCount - 9;
                if (startpage < 1) startpage = 1;
                for (int i = 0; i < 10; i++)
                {
                    if (startpage > PageCount) break;
                    if (startpage == PageIndex) //当前页
                    {
                        sb.AppendFormat("<span class=\"current\">{0}</span>", startpage);
                    }
                    else
                    {
                        QueryParams[key] = startpage;
                        sb.AppendFormat("<a href=\"{0}{1}\">{2}</a>", host, Url.Action(actionName, QueryParams), startpage);
                    }
                    startpage++;
                }

                if (PageIndex == PageCount)//最后一页
                {
                    sb.Append("<span class=\"disabled\">下一页</span><span class=\"disabled\">尾页</span>");
                }
                else
                {
                    QueryParams[key] = PageIndex + 1;
                    sb.AppendFormat("<a href=\"{0}{1}\" title=\"下一页\">下一页</a>", host, Url.Action(actionName, QueryParams));
                    QueryParams[key] = PageCount;
                    sb.AppendFormat("<a href=\"{0}{1}\" title=\"尾页\">尾页</a>", host, Url.Action(actionName, QueryParams));
                }
                sb.AppendLine("</div>");
            }
            return sb.ToString();
        }

        #region 废弃代码

        /// <summary>
        /// 打印分页(旧)
        /// </summary>
        /// <param name="PageCount"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        //[NonAction]
        //protected String PrintPage2(Int32 PageCount)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    if (PageCount > 1)
        //    {
        //        sb.Append("<div class=\"black2\">");
        //        Int32 PageIndex = Convert.ToInt32(RouteData.Values["page"]);
        //        String actionName = RouteData.Values["action"].ToString();
        //        UrlHelper url = new UrlHelper(ControllerContext.RequestContext);

        //        Dictionary<String, String> dic = new Dictionary<String, String>();
        //        foreach (String k in Request.QueryString.AllKeys)
        //        {
        //            if (String.IsNullOrEmpty(k)) continue;
        //            dic.Add(k, Request.QueryString[k]);
        //        }
        //        dynamic Query = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(dic));

        //        if (PageIndex == 0 && !String.IsNullOrEmpty(Request.QueryString["page"]))
        //        {
        //            PageIndex = Convert.ToInt32(Request.QueryString["page"]);
        //        }
        //        if (PageIndex < 1) PageIndex = 1;
        //        if (PageIndex > PageCount) PageIndex = PageCount;
        //        //sb.Append("<div class=\"black2\">");
        //        if (PageIndex == 1)//第一页
        //        {
        //            sb.Append("<span class=\"disabled\">第一页</span><span class=\"disabled\">上一页</span>");
        //        }
        //        else
        //        {
        //            Query.page = 1;
        //            sb.AppendFormat("<a href=\"{0}\" title=\"首页\">第一页</a>", url.Action(actionName, Query));
        //            Query.page = PageIndex - 1;
        //            sb.AppendFormat("<a href=\"{0}\" title=\"上一页\">上一页</a>", url.Action(actionName, Query));
        //        }
        //        //***************************************************************************************
        //        int nPage2 = PageIndex; //前进页面
        //        int nPage3 = PageIndex - 6;//后退页面
        //        if (PageCount > 10 && PageIndex > 6)//输出页数
        //        {
        //            for (int j = 1; j <= 5; j++)
        //            {
        //                nPage3 = nPage3 + 1;
        //                Query.page = nPage3;
        //                sb.AppendFormat("<a href=\"{0}\">{1}</a>", url.Action(actionName, Query), nPage3);
        //            }
        //            for (int i = 1; i <= 5; i++)
        //            {
        //                if (nPage2 > PageCount) break;
        //                if (nPage2 == PageIndex) //当前页
        //                {
        //                    sb.AppendFormat("<span class=\"current\">{0}</span>", nPage2);
        //                }
        //                else
        //                {
        //                    Query.page = nPage2;
        //                    sb.AppendFormat("<a href=\"{0}\">{1}</a>", url.Action(actionName, Query), nPage2);
        //                }
        //                nPage2 = nPage2 + 1;
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 1; i <= 10; i++)
        //            {
        //                if (i > PageCount) break;
        //                if (i == PageIndex) //当前页
        //                {
        //                    sb.AppendFormat("<span class=\"current\">{0}</span>", i);
        //                }
        //                else
        //                {
        //                    Query.page = i;
        //                    sb.AppendFormat("<a href=\"{0}\">{1}</a>", url.Action(actionName, Query), i);
        //                }
        //            }
        //        }
        //        //***************************************************************************************
        //        if (PageIndex == PageCount)//最后一页
        //        {
        //            sb.Append("<span class=\"disabled\">下一页</span><span class=\"disabled\">尾页</span>");
        //        }
        //        else
        //        {
        //            Query.page = PageIndex + 1;
        //            sb.AppendFormat("<a href=\"{0}\" title=\"下一页\">下一页</a>", url.Action(actionName, Query));
        //            Query.page = PageCount;
        //            sb.AppendFormat("<a href=\"{0}\" title=\"尾页\">尾页</a>", url.Action(actionName, Query));
        //        }
        //        sb.AppendLine("</div>");
        //    }
        //    return sb.ToString().Replace("-1.shtml", ".shtml");
        //}
       
        #endregion
    }
}
