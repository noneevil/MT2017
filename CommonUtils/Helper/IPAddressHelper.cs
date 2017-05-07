using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// IP地址辅助类
    /// </summary>
    public class IPAddressHelper
    {
        /// <summary>
        /// 检查是否在同一IP段内
        /// 示例: IPAddressHelper.MatchCIDR("192.168.30.0/24","192.168.30.255")
        /// </summary>
        /// <param name="subnet">网段</param>
        /// <param name="IPAddr">要检查的IP地址</param>
        /// <returns></returns>
        public static bool MatchCIDR(String subnet, String IPAddr)
        {
            String[] scidr = subnet.Split(new Char[] { '/' }, 2);
            uint cidriplong = IPAddressToLong(scidr[0]);
            int sub = 32 - int.Parse(scidr[1]);
            cidriplong >>= sub;
            uint iplong = IPAddressToLong(IPAddr);
            iplong >>= sub;
            if (cidriplong == iplong) return true;
            return false;
        }

        private static uint IPAddressToLong(String IPAddr)
        {
            IPAddress oIP = IPAddress.Parse(IPAddr);
            Byte[] byteIP = oIP.GetAddressBytes();
            uint ip = (uint)byteIP[0] << 24;
            ip += (uint)byteIP[1] << 16;
            ip += (uint)byteIP[2] << 8;
            ip += (uint)byteIP[3];
            return ip;
        }
    }
}
