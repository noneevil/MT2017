using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSite.Interface.Baidu
{
    /// <summary>
    /// 用户授权类接口。
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// 使用户授予的access_token和session_key过期。
        /// </summary>
        /// <returns>方法返回的xml或json文本，具体返回字段参考http://dev.baidu.com/wiki/connect/index.php?title=Open_API%E6%96%87%E6%A1%A3/passport/auth/expireSession#.E8.BF.94.E5.9B.9E.E7.BB.93.E6.9E.9C。</returns>
        string ExpireSession();

        /// <summary>
        /// 撤销用户授予第三方应用的权限。
        /// </summary>
        /// <param name="uid">用户的ID,空则默认为当前用户。</param>
        /// <returns>方法返回的xml或json文本，具体返回字段参考<a href="http://dev.baidu.com/wiki/connect/index.php?title=Open_API%E6%96%87%E6%A1%A3/passport/auth/revokeAuthorization#.E8.BF.94.E5.9B.9E.E7.BB.93.E6.9E.9C">这里</a>。</returns>
        string RevokeAuthorization(uint uid);
        /// <summary>
        /// 撤销当前用户授予第三方应用的权限。
        /// </summary>
        /// <returns>方法返回的xml或json文本，具体返回字段参考<a href="http://dev.baidu.com/wiki/connect/index.php?title=Open_API%E6%96%87%E6%A1%A3/passport/auth/revokeAuthorization#.E8.BF.94.E5.9B.9E.E7.BB.93.E6.9E.9C">这里</a>。</returns>
        string RevokeAuthorization();

        /// <summary>
        /// 撤销采用Client Credentials方式授予的权限，注意：调用该API需要通过Client Credentials方式获取access token。
        /// </summary>
        /// <returns>方法返回的xml或json文本，具体返回字段参考<a href="http://dev.baidu.com/wiki/connect/index.php?title=Open_API%E6%96%87%E6%A1%A3/passport/auth/revokeClientAuthorization#.E8.BF.94.E5.9B.9E.E7.BB.93.E6.9E.9C">这里</a>。</returns>
        string RevokeClientAuthorization();

        /// <summary>
        /// 获取或设置服务接口实例返回结果的格式，格式参见<see cref="RestFormat"/>。
        /// </summary>
        /// <remarks></remarks>
        RestFormat Format { get; set; }

        /// <summary>
        /// 获取返回json格式结果的IAuthService接口实例。
        /// </summary>
        /// <remarks></remarks>
        IAuthService JsonFormatServer { get; }

        /// <summary>
        /// 获取返回xml格式结果的IAuthService接口实例。
        /// </summary>
        /// <remarks></remarks>
        IAuthService XMLFormatServer { get; }
    }
}
