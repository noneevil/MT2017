using System;

namespace WebSite.Interface
{
    /// <summary>
    /// 整站变量设置
    /// </summary>
    public class ISessionKeys
    {
        /// <summary>
        /// 验证码Cookie名称
        /// </summary>
        public const String cookie_authcode = "authcode";
        /// <summary>
        /// 文件类型
        /// </summary>
        public const String cookie_filetype = "filetype";
        /// <summary>
        /// 文件显示模式
        /// </summary>
        public const String cookie_displaymode = "display_mode";

        /// <summary>
        /// 后台管理员会话名称
        /// </summary>
        public const String session_admin = "admindata";

        /// <summary>
        /// 文件类型图标缓存
        /// </summary>
        public const String cache_filesicos = "files_icos";
        /// <summary>
        /// 网站参数配置
        /// </summary>
        public const String cache_siteparameter = "siteparameter";

        /// <summary>
        /// 分类缓存表
        /// </summary>
        public const String cache_table_group = "T_Group";
        /// <summary>
        /// 权限缓存表
        /// </summary>
        public const String cache_table_accesscontrol = "T_AccessControl";
    }
}
