using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Mvc4Example
{
    /*
     * 
     *  实现Jobject Jarray 模型绑定
     *  
     *  方式1. MVC项目 Global.asax Application_Start事件中 添加以下项
     *  
     *  ModelBinders.Binders.Add(typeof(JObject), new JObjectModelBinder());
     *  ModelBinders.Binders.Add(typeof(JArray), new JObjectModelBinder()); 
     *  
     *  方式2. 实现JSON 文本到 Model 转换
     *  替换默认适配器
     *  ModelBinders.Binders.DefaultBinder = new JsonModelBinder();
     *  
     */
    public class JsonModelBinder : DefaultModelBinder //IModelBinder
    {
        public override object BindModel(ControllerContext context, ModelBindingContext bindingContext)
        {
            HttpRequestBase Request = context.RequestContext.HttpContext.Request;
            String ContentType = Request.ContentType;
            if ((bindingContext.ModelType == typeof(JObject))
                || (bindingContext.ModelType == typeof(JArray))
                || ContentType.StartsWith("text/json", StringComparison.OrdinalIgnoreCase)
                || ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            {
                Byte[] buffer = new Byte[Request.InputStream.Length];
                Request.InputStream.Read(buffer, 0, buffer.Length);
                String json = Encoding.UTF8.GetString(buffer);

                return JsonConvert.DeserializeObject(json, bindingContext.ModelType);

                //var stream = context.RequestContext.HttpContext.Request.InputStream;
                //stream.Seek(0, SeekOrigin.Begin);
                //string json = new StreamReader(stream).ReadToEnd();
                //return JsonConvert.DeserializeObject<dynamic>(json);
            }

            return base.BindModel(context, bindingContext);
        }
    }
}