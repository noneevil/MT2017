using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace CommonUtils
{
    /// <summary>
    /// Excel读取类
    /// </summary>
    public static class ExcelReader
    {
        /// <summary>
        /// 获取Excel中所有的工作表名称
        /// </summary>
        /// <param name="fpath">Excel文件</param>
        /// <returns></returns>
        public static List<String> GetSheets(String fpath)
        {
            List<String> list = new List<String>();
            String connString = GetConnString(fpath);
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();
                    DataTable table = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] { null, null, null, "TABLE" });
                    foreach (DataRow row in table.Rows)
                    {
                        list.Add(row[2].ToString());
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return list;
        }
        /// <summary>
        /// 读取Excel文件到DataTabel
        /// </summary>
        /// <param name="fpath">Excel文件绝对路径</param>
        /// <param name="sheet">工作簿名称</param>
        /// <param name="hdr">第一行是否为标题</param>
        /// <returns></returns>
        public static DataTable ReadExcel(String fpath, String sheet)
        {
            DataTable dt = new DataTable();
            String connString = GetConnString(fpath);
            OleDbDataAdapter ada = new OleDbDataAdapter(String.Format("select * from [{0}$]", sheet), connString);
            ada.Fill(dt);
            return dt;
        }
        /// <summary>
        /// 根据Excel文件自动生成相应的链接字符串
        /// </summary>
        /// <param name="fpath"></param>
        /// <returns></returns>
        private static String GetConnString(String fpath)
        {
            String ext = Path.GetExtension(fpath).ToLower();
            String connString = null;
            if (ext.ToLower() == ".xls")
            {
                connString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1'", fpath);
            }
            else
            {
                connString = String.Format("Provider=Microsoft.ace.oledb.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=YES;IMEX=1;'", fpath);
            }
            return connString;
        }
    }
}
