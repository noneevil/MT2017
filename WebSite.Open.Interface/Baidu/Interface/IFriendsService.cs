using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSite.Interface.Baidu
{
    /// <summary>
    /// 好友关系类接口。
    /// </summary>
    public interface IFriendsService
    {

        /// <summary>
        /// 返回指定用户的好友资料。
        /// </summary>
        /// <param name="page_no">用于支持分页的API， 0表示第1页，默认为0。</param>
        /// <param name="page_size">用于支持分页的API，表示每页返回多少条数据，默认值500。</param>
        /// <param name="sort_type">0：添加时间排序，1：登陆时间排序，默认为0。</param>
        /// <returns>方法返回的xml或json字符串，具体返回字段参考<a href="http://dev.baidu.com/wiki/connect/index.php?title=Open_API%E6%96%87%E6%A1%A3/friends/getFriends#.E8.BF.94.E5.9B.9E.E7.BB.93.E6.9E.9C">这里</a>。</returns>
        string GetFriends(uint page_no, uint page_size, uint sort_type);

        /// <summary>
        /// 获得指定用户之间是否是好友关系。第一个数组指定一半用户，第二个数组指定另外一半，两个数组必须同样的个数，一次最多可以查20个。
        /// </summary>
        /// <param name="uids1">uids1、uids2每个uid都是以半角逗号隔开。二者个数必须相等。</param>
        /// <param name="uids2">uids1、uids2每个uid都是以半角逗号隔开。二者个数必须相等。</param>
        /// <returns>方法返回的xml或json字符串，具体返回字段参考<a href="http://dev.baidu.com/wiki/connect/index.php?title=Open_API%E6%96%87%E6%A1%A3/friends/areFriends#.E8.BF.94.E5.9B.9E.E7.BB.93.E6.9E.9C">这里</a>。</returns>
        string AreFriends(string uids1, string uids2);

        /// <summary>
        /// 获取或设置服务接口实例返回结果的格式，格式参见<see cref="RestFormat"/>。
        /// </summary>
        /// <remarks></remarks>
        RestFormat Format { get; set; }

        /// <summary>
        /// 获取返回json格式结果的IFriendsService接口实例。
        /// </summary>
        /// <remarks></remarks>
        IFriendsService JsonFormatServer { get; }

        /// <summary>
        /// 获取返回xml格式结果的IFriendsService接口实例。
        /// </summary>
        /// <remarks></remarks>
        IFriendsService XMLFormatServer { get; }
    }
}
