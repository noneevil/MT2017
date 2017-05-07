using System;
using System.Collections.Generic;
using System.Data;
using MSSQLDB;

namespace CommonUtils
{
    /****************************************************************
    /****************************************************************
        CREATE TABLE [dbo].[t_filter](
        [关键值] [nvarchar](20) NOT NULL,
        [替换值] [nvarchar](20) NOT NULL CONSTRAINT [DF_t_filter_替换值]  DEFAULT (N'**')
    ) ON [PRIMARY]
    /*****************************************************************/
    /*****************************************************************/

    /// <summary>
    /// 关键词过滤处理代码
    /// </summary>
    public class KeywordFilterHelper
    {
        private static List<KeywordFilter> _keywords = null;

        /// <summary>
        /// 检查一篇文档是否包含不良关键词
        /// </summary>
        /// <param name="document">要检查的文档内容</param>
        /// <param name="keywords">返回不良关键词</param>
        /// <returns>包含返回False,否则返回True</returns>
        public static Boolean CheckDocument(String document, out List<String> keywords)
        {
            if (_keywords == null)
            { _keywords = LoadKeywords(); }
            List<String> arrs = new List<String>();
            foreach (KeywordFilter item in _keywords)
            {
                if (document.IndexOf(item.Keyword) >= 0)
                {
                    arrs.Add(item.Keyword);
                }
            }
            keywords = arrs;
            return arrs.Count == 0;
        }

        /// <summary>
        /// 对文档做不良关键词处理并返回替换后的文档内容
        /// </summary>
        /// <param name="document">要处理的文档内容</param>
        /// <returns>返回屏蔽不良关键词后的文档内容</returns>
        public static String GetDocument(String document)
        {
            if (_keywords == null)
            { _keywords = LoadKeywords(); }
            String data = document;
            foreach (KeywordFilter item in _keywords)
            {
                if (document.IndexOf(item.Keyword) >= 0)
                {
                    data = data.Replace(item.Keyword, item.ReplaceChar);
                }
            }
            return data;
        }

        /// <summary>
        /// 加载关键词代码
        /// </summary>
        /// <returns></returns>
        public static List<KeywordFilter> LoadKeywords()
        {
            List<KeywordFilter> list = new List<KeywordFilter>();
            IDataReader dr = db.ExecuteReader("SELECT * FROM [t_filter]");
            while (dr.Read())
            {
                KeywordFilter item = new KeywordFilter();
                item.Keyword = Convert.ToString(dr["关键词"]);
                item.ReplaceChar = Convert.ToString(dr["替换值"]);
                list.Add(item);
            }
            dr.Close();
            return list;
        }

        /// <summary>
        /// 保存关键词
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static Boolean AddKeywords(KeywordFilter filter)
        {
            ExecuteObject obj = new ExecuteObject();
            obj.tableName = "t_filter";
            obj.cells.Add("关键词", filter.Keyword);
            obj.cells.Add("替换值", filter.ReplaceChar);
            obj.cmdtype = CmdType.INSERT;
            return db.ExecuteCommand(obj);
        }

        /// <summary>
        /// 删除关键词
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static Boolean DeleteKeywords(String keywords)
        {
            ExecuteObject obj = new ExecuteObject();
            obj.tableName = "t_filter";
            obj.terms.Add("关键词", keywords);
            obj.cmdtype = CmdType.DELETE;
            return db.ExecuteCommand(obj);
        }

        [Serializable]
        public class KeywordFilter
        {
            public String Keyword { get; set; }
            public String ReplaceChar { get; set; }
        }
    }
}
