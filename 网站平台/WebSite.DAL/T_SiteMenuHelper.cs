using System;
using System.Data;
using MSSQLDB;

namespace WebSite.DAL
{
    public class T_SiteMenuHelper
    {
        /// <summary>
        /// 返回所有菜单
        /// </summary>
        /// <returns></returns>
        public static DataTable AllSiteMenu()
        {
            String sql = "SELECT a.*, IsNull(b.menuName ,'顶级分类') AS pName FROM [T_SiteMenu] AS a LEFT JOIN [T_SiteMenu] AS b ON a.[ParentID] = b.id ORDER BY a.parentid,a.sort";
            return db.ExecuteDataTable(sql);
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        public static Boolean DeleteMenu(String id)
        {
            String sql = String.Format("DELETE FROM [t_sitemenu] WHERE id in ({0})", id);
            return db.ExecuteCommand(sql);
        }
    }
}
