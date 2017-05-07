using System;
using System.Data;
using System.IO;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace CommonUtils
{
    /// <summary>
    /// CSV文件辅助读取类
    /// </summary>
    public static class CSVHelper
    {
        /// <summary>
        /// 读取csv文件到DataTable
        /// 默认第一行为表头
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable ReadData(String filePath)
        {
            return ReadData(filePath, true);
        }

        /// <summary>
        /// 读取csv文件到DataTable
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="header">是否有表头</param>
        /// <returns></returns>
        public static DataTable ReadData(String filePath, Boolean header)
        {
            DataTable dt = new DataTable();
            if (File.Exists(filePath))
            {
                DataRow row = null;
                String[] rowArr = null;
                using (TextFieldParser parser = new TextFieldParser(filePath, Encoding.GetEncoding("GB2312")))
                {
                    parser.Delimiters = new String[] { "\t", "," };
                    parser.TrimWhiteSpace = true;
                    while (!parser.EndOfData)
                    {
                        if (parser.LineNumber == 1)
                        {
                            rowArr = parser.ReadFields();
                            if (!header)
                            {
                                for (int i = 0; i < rowArr.Length; i++)
                                {
                                    dt.Columns.Add(i.ToString());
                                }
                                row = dt.NewRow();
                                for (int i = 0; i < dt.Columns.Count; i++)
                                {
                                    row[i] = rowArr[i];
                                }
                                dt.Rows.Add(row);
                            }
                            else
                            {
                                foreach (String col in rowArr)
                                {
                                    dt.Columns.Add(col);
                                }
                            }
                        }
                        else
                        {
                            rowArr = parser.ReadFields();
                            row = dt.NewRow();
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                row[i] = rowArr[i];
                            }
                            dt.Rows.Add(row);
                        }
                    }
                    parser.Close();
                }
            }
            else
            {
                throw new FileNotFoundException();
            }
            return dt;
        }
    }
}
