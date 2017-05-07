using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSite.Interface.Baidu
{
    /// <summary>
    /// 封装了百度Open API 2.0的所有调用接口，支持http和https方式调用。
    /// </summary>
    /// <remarks></remarks>
    public class BaiduApiClient
    {
        BaiduApiInvoker invoker;

        IUsersService userService;
        IAuthService authService;
        IFriendsService friendsService;
        IHao123Service hao123Service;

        /// <summary>
        /// 获取团购类接口调用实例。
        /// </summary>
        /// <remarks></remarks>
        public IHao123Service Hao123Service
        {
            get { return hao123Service; }
            set { hao123Service = value; }
        }

        /// <summary>
        /// 获取好友关系类接口调用实例。
        /// </summary>
        /// <remarks></remarks>
        public IFriendsService FriendsService
        {
            get { return friendsService; }
        }

        /// <summary>
        /// 获取用户授权类接口调用实例。
        /// </summary>
        /// <remarks></remarks>
        public IAuthService AuthService
        {
            get { return authService; }
        }

        /// <summary>
        /// 获取用户信息类接口调用实例。
        /// </summary>
        /// <remarks></remarks>
        public IUsersService UserService
        {
            get { return userService; }
        }

        /// <summary>
        /// 初始化<see cref="BaiduApiClient"/> 类的新实例，使用https方式请求数据。
        /// </summary>
        /// <param name="accessToken">授权码，其值必须是通过OAuth2.0协议换取access token时所拿到的access_token参数值。</param>
        public BaiduApiClient(string accessToken)
        {
            invoker = new BaiduApiInvoker(accessToken);
            InitService();
        }

        /// <summary>
        /// 初始化<see cref="BaiduApiClient"/> 类的新实例，使用http方式请求数据。
        /// </summary>
        /// <param name="session_key">授权码，其值必须是通过OAuth2.0协议换取access token时所拿到的session_key参数值。</param>
        /// <param name="session_secret">应用在之前通过百度OAuth2.0服务获取Access Token的过程中所拿到的session_secret参数值。</param>
        public BaiduApiClient(string session_key, string session_secret)
        {
            invoker = new BaiduApiInvoker(session_key,session_secret);
            InitService();
        }
        
        private void InitService()
        {
            if (invoker != null)
            {
                userService = new UsersService(invoker);
                authService = new AuthService(invoker);
                friendsService = new FriendsService(invoker);
                hao123Service = new Hao123Service(invoker);
            }
        }


    }
}
