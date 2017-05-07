using System;
using System.Collections.Generic;
using System.Data;

namespace CommonUtils.Converter
{
    public class DataRowConverter : System.Web.Script.Serialization.JavaScriptConverter
    {
        public override Object Deserialize(IDictionary<String, Object> dictionary, Type type, System.Web.Script.Serialization.JavaScriptSerializer serializer)
        {
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();
            if (dictionary != null)
            {
                foreach (KeyValuePair<String, Object> item in dictionary)
                {
                    dt.Columns.Add(item.Key);
                    dr[item.Key] = item.Value;
                }
            }
            return dr;
        }

        public override IDictionary<String, Object> Serialize(Object obj, System.Web.Script.Serialization.JavaScriptSerializer serializer)
        {
            DataRow dr = obj as DataRow;
            Dictionary<String, Object> dict = new Dictionary<String, Object>();
            if (dr != null)
            {
                foreach (DataColumn column in dr.Table.Columns)
                {
                    dict[column.ColumnName] = dr[column];
                }
            }
            return dict;
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { return new Type[] { typeof(DataRow) }; }
        }
    }
}
