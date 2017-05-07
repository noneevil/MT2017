using System;
using System.Collections.Generic;
using System.Data;

namespace CommonUtils.Converter
{
    public class DataSetConverter : System.Web.Script.Serialization.JavaScriptConverter
    {
        public override Object Deserialize(IDictionary<String, Object> dictionary, Type type, System.Web.Script.Serialization.JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<String, Object> Serialize(Object obj, System.Web.Script.Serialization.JavaScriptSerializer serializer)
        {
            DataSet ds = obj as DataSet;
            IDictionary<String, Object> result = new Dictionary<String, Object>();
            if (ds != null)
            {
                DataTable[] array = new DataTable[ds.Tables.Count];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = ds.Tables[i];
                }
                result["Tables"] = array;
            }
            return result;
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { return new Type[] { typeof(DataSet) }; }
        }
    }
}
