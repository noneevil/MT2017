using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvcDeom.Controllers
{
    public class OpenApiController : ApiController
    {
        /// <summary>
        /// 令牌
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Token()
        {
            string cookieToken, formToken;
            System.Web.Helpers.AntiForgery.GetTokens(null, out cookieToken, out formToken);
            return cookieToken + ":" + formToken;
        }

        //http://www.asp.net/web-api/overview/security/preventing-cross-site-request-forgery-(csrf)-attacks

        /// <summary>
        /// 令牌验证
        /// </summary>
        /// <param name="request"></param>
        void ValidateRequestHeader(HttpRequestMessage request)
        {
            string cookieToken = "";
            string formToken = "";

            IEnumerable<string> tokenHeaders;
            if (request.Headers.TryGetValues("Token", out tokenHeaders))
            {
                string[] tokens = tokenHeaders.First().Split(':');
                if (tokens.Length == 2)
                {
                    cookieToken = tokens[0].Trim();
                    formToken = tokens[1].Trim();
                }
            }
            System.Web.Helpers.AntiForgery.Validate(cookieToken, formToken);
        }
    }
}
