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
    /// 分类表操作类
    /// </summary>
    public class T_GroupHelper
    {
        #region 静态调用

        /// <summary>
        /// 分类表缓存数据
        /// </summary>
        public static List<T_GroupEntity> Groups
        {
            get
            {
                Cache cache = HttpContext.Current.Cache;
                List<T_GroupEntity> data;
                if (cache[ISessionKeys.cache_table_group] == null)
                {
                    String sql = String.Format("SELECT a.*,IsNull(b.GroupName,'顶级分类') AS ParentName FROM [t_group] AS a LEFT JOIN [t_group] AS b ON a.ParentID = b.ID ORDER BY a.parentid ASC,a.id");
                    data = db.ExecuteObject<List<T_GroupEntity>>(sql);
                    HttpContext.Current.Cache[ISessionKeys.cache_table_group] = data;
                }
                else
                {
                    data = (List<T_GroupEntity>)cache[ISessionKeys.cache_table_group];
                }
                return data;
            }
            //set
            //{
            //    HttpContext.Current.Cache[ISiteKeys.cache_table_group] = value;
            //}
        }

        #endregion
    }
}
