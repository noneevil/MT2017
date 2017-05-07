using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace CommonUtils.Converter
{
    /// <summary>
    /// DataTable TO JSON转换类
    /// </summary>
    public class DataTableConverter : System.Web.Script.Serialization.JavaScriptConverter
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="type"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override Object Deserialize(IDictionary<String, Object> dictionary, Type type, System.Web.Script.Serialization.JavaScriptSerializer serializer)
        {
            if (dictionary == null) throw new ArgumentNullException("dictionary");
            if (type == typeof(DataTable))
            {
                foreach (KeyValuePair<String, Object> table in dictionary)
                {
                    DataTable dt = new DataTable(table.Key);//表名
                    ArrayList rows = (ArrayList)table.Value;
                    //列名
                    Dictionary<String, Object> row = serializer.ConvertToType<Dictionary<String, Object>>(rows[0]);
                    foreach (String item in row.Keys)
                    {
                        dt.Columns.Add(item);
                    }
                    //每行数据
                    for (int i = 0; i < rows.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        Dictionary<String, Object> dic = serializer.ConvertToType<Dictionary<String, Object>>(rows[i]);
                        foreach (KeyValuePair<String, Object> item in dic)
                        {
                            dr[item.Key] = item.Value;
                        }
                        dt.Rows.Add(dr);
                    }
                    return dt;
                }
            }
            return null;
        }

        public override IDictionary<String, Object> Serialize(Object obj, System.Web.Script.Serialization.JavaScriptSerializer serializer)
        {
            DataTable dt = obj as DataTable; //将Datatable转成Dictionary完成序列化
            Dictionary<String, Object> dict = new Dictionary<String, Object>();
            if (dt != null)
            {
                ArrayList arrList = new ArrayList();
                foreach (DataRow dr in dt.Rows)//循环每行
                {
                    Dictionary<String, Object> dic = new Dictionary<String, Object>();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        dic.Add(dc.ColumnName, dr[dc.ColumnName]);//Dic中存储列名和每列值
                    }
                    arrList.Add(dic);//ArrayList中保存各行信息
                }
                dict[dt.TableName] = arrList; //表名作为Key,ArrayList作为值
            }
            return dict;
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { return new Type[] { typeof(DataTable) }; }
        }

    }
}
