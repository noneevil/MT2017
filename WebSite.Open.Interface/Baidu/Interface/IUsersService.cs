using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSite.Interface.Baidu
{
    /// <summary>
    /// 用户信息类接口。
    /// </summary>
    public interface IUsersService
    {

        /// <summary>
        /// 获取当前登录用户的用户uid和用户名。
        /// </summary>
        /// <returns></returns>
        string GetLoggedInUser();

        /// <summary>
        /// 返回指定用户的用户资料。
        /// </summary>
        /// <param name="uid">用户id, 为空则表示当前登录用户</param>
        /// <param name="fields">期望获得的用户资料的字段列表，用逗号隔开，如: sex,age,realname，可用的值参考<a href="http://dev.baidu.com/wiki/connect/index.php?title=Open_API%E6%96%87%E6%A1%A3/passport/users/getInfo#.E8.BF.94.E5.9B.9E.E7.BB.93.E6.9E.9C">这里</a>，为空则获取userid, username, realname。</param>
        /// <returns>根据fields字段，返回相应的信息。支持返回的字段参考<a href="http://dev.baidu.com/wiki/connect/index.php?title=Open_API%E6%96%87%E6%A1%A3/passport/users/getInfo#.E8.BF.94.E5.9B.9E.E7.BB.93.E6.9E.9C">这里</a>。</returns>
        string GetInfo(uint uid, string fields);
        /// <summary>
        /// Gets the info.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        string GetInfo(uint uid);
        /// <summary>
        /// 获取当前登录用户的用户资料。
        /// </summary>
        /// <param name="fields">期望获得的用户资料的字段列表，用逗号隔开，如: sex,age,realname，可用的值参考<a href="http://dev.baidu.com/wiki/connect/index.php?title=Open_API%E6%96%87%E6%A1%A3/passport/users/getInfo#.E8.BF.94.E5.9B.9E.E7.BB.93.E6.9E.9C">这里</a>，为空则获取userid, username, realname。</param>
        /// <returns>返回指定字段的信息。</returns>
        /// <remarks></remarks>
        string GetInfo(string fields);

        /// <summary>
        /// 获取当前登录用户的用户资料。
        /// </summary>
        /// <returns>返回包含userid, username, realname字段的信息。</returns>
        /// <remarks></remarks>
        string GetInfo();

        /// <summary>
        /// 判定用户是否已经为应用授权。
        /// </summary>
        /// <param name="uid">用户uid,为0则默认是当前用户。</param>
        /// <param name="appid">应用的appid,为0则默认是当前应用。</param>
        /// <returns>
        /// 包含result字段的xml或json文本，1表示已经授权该应用，0表示没有授权该应用。
        /// </returns>
        string IsAppUser(uint uid,int appid);

        /// <summary>
        /// 判定用户是否已经为当前应用授权。
        /// </summary>
        /// <param name="uid">用户uid。</param>
        /// <remarks></remarks>
        /// <returns>
        /// 包含result字段的xml或json文本，1表示已经授权该应用，0表示没有授权该应用。
        /// </returns>
        string IsAppUser(uint uid);

        /// <summary>
        /// 判定用户是否已经为指定应用授权。
        /// </summary>
        /// <param name="appid">应用的appid。</param>
        /// <remarks></remarks>
        /// /// <returns>
        /// 包含result字段的xml或json文本，1表示已经授权该应用，0表示没有授权该应用。
        /// </returns>
        string IsAppUser(int appid);

        /// <summary>
        /// 判定当前用户是否已经为指定应用授权。
        /// </summary>
        /// <remarks></remarks>
        /// /// <returns>
        /// 包含result字段的xml或json文本，1表示已经授权该应用，0表示没有授权该应用。
        /// </returns>
        string IsAppUser();

        /// <summary>
        /// 根据用户id以及在百度的相应的操作权限(单个权限，例如接收email等)来判断用户是否可以进行此操作。
        /// </summary>
        /// <param name="ext_perm">单个权限，例如接收email等，具体权限请查看<a href="http://dev.baidu.com/wiki/connect/index.php?title=%E6%9D%83%E9%99%90%E5%88%97%E8%A1%A8">权限列表</a>。</param>
        /// <param name="uid">用户uid，为0则默认是当前用户。</param>
        /// <returns>
        /// 包含result字段的xml或json文本，1表示当前功能有操作权限，0表示当前功能没有操作权限。.
        /// </returns>
        string HasAppPermission(string ext_perm, uint uid);

        /// <summary>
        /// 判断当前用户是否可以进行权限(单个权限，例如接收email等)操作。
        /// </summary>
        /// <param name="ext_perm">单个权限，例如接收email等，具体权限请查看<a href="http://dev.baidu.com/wiki/connect/index.php?title=%E6%9D%83%E9%99%90%E5%88%97%E8%A1%A8">权限列表</a>。</param>
        /// <remarks></remarks>
        /// <returns>
        /// 包含result字段的xml或json文本，1表示当前功能有操作权限，0表示当前功能没有操作权限。.
        /// </returns>
        string HasAppPermission(string ext_perm);

        /// <summary>
        /// 根据用户id以及在百度的相应的操作权限(可以是多个权限半角逗号隔开)来判断用户是否可以进行此操作。
        /// </summary>
        /// <param name="ext_perms">多个权限半角逗号隔开，例如basic,email等，具体权限请查看<a href="http://dev.baidu.com/wiki/connect/index.php?title=%E6%9D%83%E9%99%90%E5%88%97%E8%A1%A8">权限列表</a>。</param>
        /// <param name="uid">用户uid，为0则默认是当前用户。</param>
        /// <returns>包含对应权限字段的xml或json文本，1表示当前功能有操作权限，0表示当前功能没有操作权限。.</returns>
        string HasAppPermissions(string ext_perms, uint uid);


        /// <summary>
        /// 根据当前用户id以及在百度的相应的操作权限(可以是多个权限半角逗号隔开)来判断该用户是否可以进行此操作。
        /// </summary>
        /// <param name="ext_perms">多个权限半角逗号隔开，例如basic,email等，具体权限请查看<a href="http://dev.baidu.com/wiki/connect/index.php?title=%E6%9D%83%E9%99%90%E5%88%97%E8%A1%A8">权限列表</a>。</param>
        /// <returns>包含对应权限字段的xml或json文本，1表示当前功能有操作权限，0表示当前功能没有操作权限。.</returns>
        string HasAppPermissions(string ext_perms);

        /// <summary>
        /// 获取或设置服务实例返回结果的格式，格式参见<see cref="RestFormat"/>。
        /// </summary>
        /// <remarks></remarks>
        RestFormat Format { get; set; }

        /// <summary>
        /// 获取返回json格式结果的IUsersService接口实例。
        /// </summary>
        /// <remarks></remarks>
        IUsersService JsonFormatServer { get; }
        /// <summary>
        /// 获取返回xml格式结果的IUsersService接口实例。
        /// </summary>
        /// <remarks></remarks>
        IUsersService XMLFormatServer { get; }
    }
}
