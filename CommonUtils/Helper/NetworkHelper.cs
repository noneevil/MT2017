using System;
using System.Runtime.InteropServices;
using CommonUtils.Enumeration;

namespace CommonUtils
{
    /// <summary>
    /// 检测网络连接状态
    /// </summary>
    public class NetworkHelper
    {
        /// <summary>
        /// 检测网络连接状态True网络连接成功，False网络未连接
        /// INTERNET_CONNECTION_MODEM             拨号上网                    1
        /// INTERNET_CONNECTION_LAN               局域网                      2                
        /// INTERNET_CONNECTION_PROXY             代理                        4
        /// INTERNET_CONNECTION_MODEM_BUSY        代理被占用                  8          
        /// INTERNET_RAS_INSTALLED                RAS安装                     16
        /// INTERNET_CONNECTION_OFFLINE           离线                        32
        /// INTERNET_CONNECTION_CONFIGURED        虽然可以联网，但当前不可用  64
        /// </summary>
        /// <param name="connectionDescription"></param>
        /// <param name="reservedValue">0 参数值必须是0</param>
        /// <returns></returns>
        [DllImport("wininet.dll")]
        private extern static Boolean InternetGetConnectedState(out int connectionDescription, int reservedValue);
        /// <summary>
        /// 检测网络连接状态True网络连接成功，False网络未连接
        /// NETWORK_ALIVE_LAN
        /// NETWORK_ALIVE_WAN
        /// </summary>
        /// <param name="connectionDescription"></param>
        /// <returns></returns>
        [DllImport("sensapi.dll")]
        private extern static Boolean IsNetworkAlive(out int connectionDescription);
        /// <summary>
        /// 使用IsNetworkAlive检测网络连接状态
        /// 可以及时反应网络连通情况，但是需要服务System Event Notification支持(系统默认自动启动该服务)
        /// </summary>
        /// <returns></returns>
        public static Boolean IsConnected()
        {
            int connectionDescription;
            return IsNetworkAlive(out connectionDescription);
        }
        /// <summary>
        /// 使用InternetGetConnectedState检测网络连接状态
        /// 此方法对网络状况不能及时反应(有几秒钟延时)
        /// </summary>
        /// <param name="connectionModel">网络连接方式</param>
        /// <returns></returns>
        public static Boolean IsConnected(out INTERNET_CONNECTION connectionModel)
        {
            int connectionDescription;
            Boolean res = InternetGetConnectedState(out connectionDescription, 0);
            connectionModel = (INTERNET_CONNECTION)connectionDescription;
            return res;
        }
    }
}
