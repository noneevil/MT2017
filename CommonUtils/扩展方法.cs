using System;
using System.Data;
using System.IO;
using System.Web;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Caching;
using System.Collections.Generic;
using System.Collections;

namespace CommonUtils
{
    public static class 扩展方法
    {
        /// <summary>
        /// DataTable按父节点排序
        /// </summary>
        /// <param name="dt">源表</param>
        /// <param name="ds">新表</param>
        /// <param name="parentId">父节点</param>
        public static void SortTable(this DataTable dt, DataTable ds, Int32 parentId)
        {
            DataRow[] rows = dt.Select("parentid=" + parentId);
            foreach (DataRow dr in rows)
            {
                ds.ImportRow(dr);
                SortTable(dt, ds, Int32.Parse(dr["id"].ToString()));
            }
        }
        /// <summary>
        /// 删除内容中包含的图片
        /// </summary>
        /// <param name="content"></param>
        /// <param name="files"></param>
        public static void RemoveImage(this String content, params String[] files)
        {
            HttpServerUtility Server = HttpContext.Current.Server;
            foreach (String file in files)
            {
                if (File.Exists(Server.MapPath(file))) File.Delete(Server.MapPath(file));
            }

            HtmlDocument xml = new HtmlDocument();
            xml.LoadHtml(content);
            var nodes = xml.DocumentNode.CssSelect("img[src^='/']");
            foreach (HtmlNode n in nodes)
            {
                String src = Server.MapPath(n.Attributes["src"].Value.Trim());
                if (File.Exists(src))
                {
                    File.Delete(src);
                }
            }
        }

        /// <summary>
        /// Creates a deep copy of a object.
        /// 
        /// Note: object must be serializable.
        /// 
        /// Note: this will not handle an object with events. 
        /// However a work around is described here
        /// http://www.lhotka.net/WeBlog/CommentView.aspx?guid=776f44e8-aaec-4845-b649-e0d840e6de2c
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToCopy"></param>
        /// <returns></returns>
        public static T SerializedDeepCopy<T>(this T objectToCopy)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter binary = new BinaryFormatter();

            binary.Serialize(ms, objectToCopy);
            ms.Seek(0, SeekOrigin.Begin);
            
            return (T)binary.Deserialize(ms);
        }
        /// <summary>
        /// 清除全部缓存
        /// </summary>
        /// <param name="x"></param>
        public static void Clear(this Cache x)
        {
            List<string> cacheKeys = new List<string>();
            IDictionaryEnumerator cacheEnum = x.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cacheKeys.Add(cacheEnum.Key.ToString());
            }
            foreach (string cacheKey in cacheKeys)
            {
                x.Remove(cacheKey);
            }
        }
    }
}
