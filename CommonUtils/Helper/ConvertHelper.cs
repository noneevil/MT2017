using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic;

namespace CommonUtils
{
    /// <summary>
    /// 数据类型转换处理类
    /// </summary>
    public abstract class ConvertHelper
    {
        /// <summary>
        /// 将对象序列化为Base64字符串
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>Base64字符串</returns>
        public static String ObjectToBase64(Object obj)
        {
            return Convert.ToBase64String(ObjectToBytes(obj));
        }
        /// <summary>
        /// 将Base64字符串反序列化为对象
        /// </summary>
        /// <param name="scrStr">要反序列化的字符串</param>
        /// <param name="objType">对象类型</param>
        /// <returns></returns>
        public static Object Base64ToObject(String scrStr, Type objType)
        {
            return BytesToObject(Convert.FromBase64String(scrStr), objType);
        }
        /// <summary>
        /// 对象序列化，得到字符串
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <returns></returns>
        public static String ObjectToString(Object obj)
        {
            return Encoding.Default.GetString(ObjectToBytes(obj));
        }
        /// <summary>
        /// 字符串反序列化，得到对象
        /// </summary>
        /// <param name="scrStr">要反序列化的字符串</param>
        /// <param name="objType">对象类型</param>
        /// <returns></returns>
        public static Object StringToObject(String scrStr, Type objType)
        {
            return BytesToObject(Encoding.Default.GetBytes(scrStr), objType);
        }
        /// <summary>
        /// Base64编码字符串转图像
        /// </summary>
        /// <param name="imgString"></param>
        /// <returns></returns>
        public static System.Drawing.Image StringToImage(String imgString)
        {
            if (String.IsNullOrEmpty(imgString)) return null;
            try
            {
                //String[] imgarr = imgString.Split(new Char[] { ',' });
                //Byte[] buffer = Array.ConvertAll<String, Byte>(imgarr, delegate(String s) { return Byte.Parse(s); });
                //Byte[] buffer = Array.ConvertAll<String, Byte>(imgarr, s => Byte.Parse(s));
                Byte[] buffer = Convert.FromBase64String(imgString);
                MemoryStream ms = new MemoryStream(buffer);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                return image;
            }
            catch { return null; }
        }
        /// <summary>
        /// 图像转Base64编码字符串
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static String ImageToString(System.Drawing.Image img)
        {
            ImageConverter converter = new ImageConverter();
            Byte[] buffer = (Byte[])converter.ConvertTo(img, typeof(Byte[]));
            String result = Convert.ToBase64String(buffer);
            //String result = String.Join(",", Array.ConvertAll(buffer, (Converter<Byte, String>)Convert.ToString));
            return result;
        }
        /// <summary>
        /// 将对象序列化为字节数组
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Byte[] ObjectToBytes(Object obj)
        {
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(ms, Encoding.Default);
            xs.Serialize(writer, obj);
            writer.Close();
            return ms.ToArray();
        }
        /// <summary>
        /// 从字节数组反序列化得到对象
        /// </summary>
        /// <param name="objBytes"></param>
        /// <returns></returns>
        public static Object BytesToObject(Byte[] objBytes, Type objType)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(objType);
                MemoryStream ms = new MemoryStream(objBytes);
                StreamReader sr = new StreamReader(ms, Encoding.Default);
                return xs.Deserialize(sr);
            }
            catch
            {
                return null;
            }
        }
        ///   <summary>   
        ///   序列化对象   
        ///   </summary>   
        ///   <param   name="data">要序列化的对象</param>   
        ///   <returns>返回存放序列化后的数据缓冲区</returns>   
        public static Byte[] Serialize(Object data)
        {
            Byte[] buffer;
            BinaryFormatter binary = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                binary.Serialize(ms, data);
                buffer = ms.GetBuffer();
            }
            return buffer;
        }
        ///   <summary>   
        ///   反序列化对象   
        ///   </summary>   
        ///   <param   name="data">数据缓冲区</param>   
        ///   <returns>对象</returns>   
        public static Object Deserialize(Byte[] data)
        {
            Object resutl;
            BinaryFormatter binary = new BinaryFormatter();
            using (MemoryStream rems = new MemoryStream(data))
            {
                resutl = binary.Deserialize(rems);
            }
            data = null;
            return resutl;
        }
        /// <summary>
        /// 将对象序列化为XML
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="objType">圣像类型</param>
        /// <returns>返回XML</returns>
        public static String ObjectToXml(Object obj, Type objType)
        {
            StringBuilder result = new StringBuilder();
            StringWriter sw = new StringWriter(result);
            try
            {
                XmlSerializer xml = new XmlSerializer(objType);
                xml.Serialize(sw, obj);
            }
            finally
            {
                sw.Close();
            }
            return result.ToString();
        }
        /// <summary>
        /// 将对象序列化为XML
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objType"></param>
        /// <param name="xmlroot"></param>
        /// <returns></returns>
        public static String ObjectToXml(Object obj, Type objType, String xmlroot)
        {
            StringBuilder result = new StringBuilder();
            StringWriter sw = new StringWriter(result);
            try
            {
                XmlSerializer xml = new XmlSerializer(objType, new XmlRootAttribute(xmlroot));
                xml.Serialize(sw, obj);
            }
            finally
            {
                sw.Close();
            }
            return result.ToString();
        }
        /// <summary>
        /// 将xml反系列化为Object
        /// </summary>
        /// <param name="xml">读取xml</param>
        /// <returns></returns>
        public static Object XmlToObject(String xml, Type objType)
        {
            XmlSerializer xs = new XmlSerializer(objType);
            Stream stream = new MemoryStream(Encoding.Default.GetBytes(xml));
            Object obj = xs.Deserialize(stream);
            return obj;
        }
        /// <summary>
        /// 将xml反系列化为Object
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="objType"></param>
        /// <param name="xmlroot">xml根节点</param>
        /// <returns></returns>
        public static Object XmlToObject(String xml, Type objType, String xmlroot)
        {
            XmlSerializer xs = new XmlSerializer(objType, new XmlRootAttribute(xmlroot));
            Stream stream = new MemoryStream(Encoding.Default.GetBytes(xml));
            Object obj = xs.Deserialize(stream);
            return obj;
        }
        /// <summary>
        /// 将对象存为文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="configFile"></param>
        public static void ObjectToFile<T>(Object obj, String configFile)
        {
            if (obj == null) obj = (T)Activator.CreateInstance(typeof(T));
            using (FileStream fs = new FileStream(configFile, FileMode.OpenOrCreate))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute("Root"));
                fs.SetLength(0);
                serializer.Serialize(fs, obj);
            }

            //using (FileStream stream = new FileStream(configFile, FileMode.OpenOrCreate))
            //{
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        BinaryFormatter binary = new BinaryFormatter();
            //        binary.Serialize(ms, obj);

            //        Byte[] buffer = GZipHelper.Compress(ms.ToArray());
            //        //Byte[] buffer = SevenZipSharpHelper.Compress(ms.ToArray());
            //        Array.Reverse(buffer, 0, buffer.Length);
            //        stream.SetLength(0);
            //        stream.Write(buffer, 0, buffer.Length);
            //    }
            //}
        }
        /// <summary>
        /// 将文件转为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configFile"></param>
        /// <returns></returns>
        public static T FileToObject<T>(String configFile)
        {
            using (FileStream stream = new FileStream(configFile, FileMode.OpenOrCreate))
            {
                try
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T), new XmlRootAttribute("Root"));
                    return (T)xs.Deserialize(stream);
                }
                catch
                {
                    return (T)Activator.CreateInstance(typeof(T));
                }
            }
            //using (FileStream stream = new FileStream(configFile, FileMode.OpenOrCreate))
            //{
            //    try
            //    {
            //        Byte[] buffer = new Byte[stream.Length];
            //        stream.Read(buffer, 0, (Int32)stream.Length);
            //        Array.Reverse(buffer, 0, buffer.Length);

            //        buffer = GZipHelper.Decompress(buffer);
            //        //buffer = SevenZipSharpHelper.Decompress(buffer); 
            //        using (MemoryStream ms = new MemoryStream(buffer))
            //        {
            //            BinaryFormatter binary = new BinaryFormatter();
            //            return (T)binary.Deserialize(ms);
            //        }
            //    }
            //    catch
            //    {
            //        return (T)Activator.CreateInstance(typeof(T));
            //    }
            //}
        }
        /// <summary>
        /// 序列化DataTable
        /// </summary>
        /// <param name="dt">包含数据的DataTable</param>
        /// <returns>序列化的DataTable</returns>
        public static String DataTableToXml(DataTable dt, String dtname)
        {
            dt.TableName = dtname;
            StringBuilder sb = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(sb);
            XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
            serializer.Serialize(writer, dt);
            writer.Close();
            return sb.ToString();
        }
        /// <summary>
        /// 反序列化DataTable
        /// </summary>
        /// <param name="xml">序列化的DataTable</param>
        /// <returns>DataTable</returns>
        public static DataTable XmlToDataTable(String xml)
        {
            StringReader sr = new StringReader(xml);
            XmlReader reader = XmlReader.Create(sr);
            XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
            DataTable dt = serializer.Deserialize(reader) as DataTable;
            return dt;
        }
        /// <summary>
        /// 将对象转换为 JSON 字符串。
        /// </summary>
        /// <param name="obj">序列化的对象</param>
        /// <returns>序列化的 JSON 字符串</returns>
        public static String ObjectToJson(Object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (obj.GetType() == typeof(DataTable))
            {
                return serializer.Serialize(((DataTable)obj).AsEnumerable().ToList());
            }
            else if (obj.GetType() == typeof(DataSet))
            {
                return serializer.Serialize(((DataSet)obj).Tables[0].AsEnumerable().ToList());
            }
            else
            {
                return serializer.Serialize(obj);
            }
        }
        /// <summary>
        /// 将 JSON 格式字符串转换为指定类型的对象
        /// </summary>
        /// <param name="json">要反序列化的 JSON 字符串</param>
        /// <param name="objType">对象的类型</param>
        /// <returns>反序列化的对象。</returns>
        public static Object JsonToObject(String json, Type objType)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize(json, objType);
        }
        /// <summary>
        /// 将 JSON 字符串转换为 T 类型的对象。
        /// </summary>
        /// <typeparam name="T">所生成对象的类型</typeparam>
        /// <param name="json">要进行反序列化的 JSON 字符串</param>
        /// <returns>反序列化的对象</returns>
        public static T JsonToObject<T>(String json)
        {
            if (String.IsNullOrEmpty(json)) return default(T);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(json);
        }
        /// <summary>
        /// 将 DataTable 转为模型 List
        /// </summary>
        /// <typeparam name="T">所生成对象的类型</typeparam>
        /// <param name="dt">数据源</param>
        /// <returns>反序列化的对象</returns>
        public static List<T> DataTableToList<T>(DataTable dt)
        {
            List<T> list = new List<T>();
            Type type = typeof(T);

            foreach (DataRow dr in dt.Rows)
            {
                T t = (T)SetValue(type, dr);
                list.Add(t);
            }
            return list;
        }
        /// <summary>
        /// 嵌套成员赋值
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="dr">DataRow 数据源</param>
        /// <returns></returns>
        private static Object SetValue(Type type, DataRow dr)
        {
            Object obj = Activator.CreateInstance(type);
            DataColumnCollection dc = dr.Table.Columns;

            foreach (PropertyInfo att in type.GetProperties())
            {
                Object value = DBNull.Value;
                String fieldname = att.Name;
                String nestedname = String.Format("{0}.{1}", type.Name, fieldname);
                if (dc.Contains(nestedname)) fieldname = nestedname;

                if (dc.Contains(fieldname))
                {
                    value = dr[fieldname];
                    if (value == DBNull.Value) continue;
                    att.SetValue(obj, value, null);
                    //dc.Remove(FieldName);
                    dr[fieldname] = null;
                }
                else if (att.PropertyType.Namespace.Equals("MvcSite.Models"))
                {
                    Object instance = SetValue(att.PropertyType, dr);
                    att.SetValue(obj, instance, null);
                }
            }
            return obj;
        }
        /// <summary>
        /// 将字符串转换为简体中文
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String ToSimplifiedChinese(String text)
        {
            return Strings.StrConv(text, VbStrConv.SimplifiedChinese, 0);
        }
        /// <summary>
        /// 将字符串转换为繁体中文
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String ToTraditionalChinese(String text)
        {
            return Strings.StrConv(text, VbStrConv.TraditionalChinese, 0);
        }
        /// <summary>
        /// 全角符号转半角
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String ToNarrow(String text)
        {
            return Strings.StrConv(text, VbStrConv.Narrow, 0);
        }
        /// <summary>
        /// 半角符号转全角
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String ToWide(String text)
        {
            return Strings.StrConv(text, VbStrConv.Wide, 0);
        }
        /// <summary>
        /// 将指定的字符串转换为首字母大写
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String ToTitleCase(String text)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text);
        }
    }
}