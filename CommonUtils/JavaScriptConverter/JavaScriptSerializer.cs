using CommonUtils.Converter;

namespace CommonUtils
{
    /// <summary>
    /// JavaScriptSerializer序列化扩展处理类
    /// </summary>
    public class JavaScriptSerializer : System.Web.Script.Serialization.JavaScriptSerializer
    {
        /// <summary>
        /// JavaScriptSerializer序列化扩展处理类
        /// </summary>
        public JavaScriptSerializer()
        {
            RegisterConverters(new System.Web.Script.Serialization.JavaScriptConverter[]
            { 
                //new DataTableConverter(), 
                //new DataSetConverter(),
                new DataRowConverter(),
                new DateTimeConverter() 
            });
        }
    }
}