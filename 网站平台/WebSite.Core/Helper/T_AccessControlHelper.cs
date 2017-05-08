using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using MSSQLDB;
using WebSite.Interface;
using WebSite.Models;

namespace WebSite.Core
{
    /// <summary>
    /// 权限表操作类
    /// </summary>
    public class T_AccessControlHelper
    {
        #region 静态调用

        /// <summary>
        /// 角色权限表缓存数据
        /// </summary>
        public static List<T_AccessControlEntity> AccessControl
        {
            get
            {
                Cache cache = HttpContext.Current.Cache;
                List<T_AccessControlEntity> data;
                if (cache[ISessionKeys.cache_table_accesscontrol] == null)
                {
                    String sql = String.Format("SELECT * FROM [T_AccessControl]");
                    data = db.ExecuteObject<List<T_AccessControlEntity>>(sql);
                    HttpContext.Current.Cache[ISessionKeys.cache_table_accesscontrol] = data;
                }
                else
                {
                    data = (List<T_AccessControlEntity>)cache[ISessionKeys.cache_table_accesscontrol];
                }
                return data;
            }
            //set
            //{
            //    HttpContext.Current.Cache[ISiteKeys.cache_table_accesscontrol] = value;
            //}
        }
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static List<T_AccessControlEntity> GetRole(Int32 roleId)
        {
            return AccessControl.FindAll(a => { return a.Role == roleId; });
        }

        #endregion
    }
}
