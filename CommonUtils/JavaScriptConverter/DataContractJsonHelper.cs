using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// DataContractJsonSerializer 扩展类
    /// </summary>
    public abstract class DataContractJsonHelper
    {
        /// <summary>
        /// JSON序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String Serialize(Object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                String json = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return json;
            }
        }
        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Object Deserialize(String json, Type type)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);
                Object obj = serializer.ReadObject(ms);
                ms.Close();
                return obj;
            }
        }
        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(String json)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                T obj = (T)serializer.ReadObject(ms);
                ms.Close();
                return obj;
            }
        }
    }
}
