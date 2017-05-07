using System;
using System.Collections;
using System.Collections.Generic;

namespace CommonUtils.Converter
{
    /// <summary>
    /// JSON日期转换类
    /// </summary>
    public class DateTimeConverter : System.Web.Script.Serialization.JavaScriptConverter
    {
        public override Object Deserialize(IDictionary<String, Object> dictionary, Type type, System.Web.Script.Serialization.JavaScriptSerializer serializer)
        {
            return new System.Web.Script.Serialization.JavaScriptSerializer().ConvertToType(dictionary, type);
        }

        public override IDictionary<String, Object> Serialize(Object obj, System.Web.Script.Serialization.JavaScriptSerializer serializer)
        {
            if (!(obj is DateTime)) return null;
            return new CustomString(((DateTime)obj).ToString("o"));
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { return new[] { typeof(DateTime) }; }
        }

        internal class CustomString : Uri, IDictionary<String, Object>
        {
            public CustomString(String str)
                : base(str, UriKind.Relative)
            {
            }

            #region IDictionary<String,Object> 成员

            void IDictionary<String, Object>.Add(String key, Object value)
            {
                throw new NotImplementedException();
            }

            Boolean IDictionary<String, Object>.ContainsKey(String key)
            {
                throw new NotImplementedException();
            }

            ICollection<String> IDictionary<String, Object>.Keys
            {
                get { throw new NotImplementedException(); }
            }

            Boolean IDictionary<String, Object>.Remove(String key)
            {
                throw new NotImplementedException();
            }

            Boolean IDictionary<String, Object>.TryGetValue(String key, out Object value)
            {
                throw new NotImplementedException();
            }

            ICollection<Object> IDictionary<String, Object>.Values
            {
                get { throw new NotImplementedException(); }
            }

            Object IDictionary<String, Object>.this[String key]
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            #endregion

            #region ICollection<KeyValuePair<String,Object>> 成员

            void ICollection<KeyValuePair<String, Object>>.Add(KeyValuePair<String, Object> item)
            {
                throw new NotImplementedException();
            }

            void ICollection<KeyValuePair<String, Object>>.Clear()
            {
                throw new NotImplementedException();
            }

            Boolean ICollection<KeyValuePair<String, Object>>.Contains(KeyValuePair<String, Object> item)
            {
                throw new NotImplementedException();
            }

            void ICollection<KeyValuePair<String, Object>>.CopyTo(KeyValuePair<String, Object>[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            int ICollection<KeyValuePair<String, Object>>.Count
            {
                get { throw new NotImplementedException(); }
            }

            Boolean ICollection<KeyValuePair<String, Object>>.IsReadOnly
            {
                get { throw new NotImplementedException(); }
            }

            Boolean ICollection<KeyValuePair<String, Object>>.Remove(KeyValuePair<String, Object> item)
            {
                throw new NotImplementedException();
            }

            #endregion

            #region IEnumerable<KeyValuePair<String,Object>> 成员

            IEnumerator<KeyValuePair<String, Object>> IEnumerable<KeyValuePair<String, Object>>.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            #endregion

            #region IEnumerable 成员

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            #endregion
        }
    }
}
