
namespace CommonUtils.Enumeration
{
    /// <summary>
    /// 联网方式 
    /// </summary>
    public enum INTERNET_CONNECTION
    {
        /// <summary>
        /// 拨号上网 1
        /// </summary>
        INTERNET_CONNECTION_MODEM = 1,
        /// <summary>
        /// 局域网 2
        /// </summary>
        INTERNET_CONNECTION_LAN = 2,
        /// <summary>
        /// 代理 4
        /// </summary>
        INTERNET_CONNECTION_PROXY = 4,
        /// <summary>
        /// 代理被占用 8
        /// </summary>
        INTERNET_CONNECTION_MODEM_BUSY = 8,
        /// <summary>
        /// RAS安装 16
        /// </summary>
        INTERNET_RAS_INSTALLED = 16,
        /// <summary>
        /// 离线 32
        /// </summary>
        INTERNET_CONNECTION_OFFLINE = 32,
        /// <summary>
        /// 虽然可以联网，但当前不可用 64
        /// </summary>
        INTERNET_CONNECTION_CONFIGURED = 64

    }
}
