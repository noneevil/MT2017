using MSSQLDB;
using Mvc4Example.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace Mvc4Example.Controllers
{
    /// <summary>
    /// 数据传递方式
    /// </summary>
    public class SaveFormController : BaseController
    {
        #region 简单业务应用方案
        /// <summary>
        /// 集合类数据
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <param name="array3"></param>
        /// <param name="array4"></param>
        /// <param name="array5"></param>
        /// <param name="array6"></param>
        /// <param name="array7"></param>
        /// <param name="array8"></param>
        /// <returns></returns>
        public ActionResult SaveForm0(int[] array1, ICollection<int[]> array2, IEnumerable<String[]> array3, Byte[] array4, List<int> array5, Dictionary<string, int> array6, IList<byte> array7, KeyValuePair<string, string> array8)
        {
            List<object> data = new List<object>();
            data.Add(new { type = typeof(int[]).FullName, key = "array1", value = array1 });
            data.Add(new { type = typeof(ICollection<int[]>).FullName, key = "array2", value = array2 });
            data.Add(new { type = typeof(IEnumerable<String[]>).FullName, key = "array3", value = array3 });
            data.Add(new { type = typeof(Byte[]).FullName, key = "array4", value = array4 });
            data.Add(new { type = typeof(List<int>).FullName, key = "array5", value = array5 });
            data.Add(new { type = typeof(Dictionary<string, int>).FullName, key = "array6", value = array6 });
            data.Add(new { type = typeof(IList<byte>).FullName, key = "array7", value = array7 });
            data.Add(new { type = typeof(KeyValuePair<string, string>).FullName, key = "array8", value = array8 });

            //构造返回数据
            var result = new IResult
            {
                data = data,
                header = RequestHeader
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// MVC 默认接收参数1
        /// 使用实体作为Action参数传入，前提是提交的表单元素名称与实体属性名称一一对应
        /// 传递方式:GET/POST
        /// GET-不支持嵌套实体赋值 POST-支持嵌套实体赋值
        /// </summary>
        /// <param name="data">实体模型</param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult SaveForm1(T_Province data)
        {
            //构造返回数据
            var result = new IResult
            {
                data = data,
                header = RequestHeader
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// MVC JSON Strin 2 Model
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult SaveForm1_2(T_Province data)
        {
            //构造返回数据
            var result = new IResult
            {
                data = data,
                header = RequestHeader
            };
            return Json(result);
        }
        /// <summary>
        /// MVC 默认接收参数2
        /// Action参数名与表单元素name值一一对应
        /// 传递方式:GET/POST
        /// </summary>
        /// <param name="id">此处加问号表示可选择参数</param>
        /// <param name="name"></param>
        /// <param name="letter"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult SaveForm2(int? id, String name, String letter)
        {
            //实例化模型,并赋值
            T_Province data = new T_Province()
            {
                Province_Id = Convert.ToString(id),
                Province_Name = name,
                Province_Letter = letter
            };
            //构造返回数据
            var result = new IResult
            {
                data = data,
                header = RequestHeader
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// MVC 默认接收参数3 
        /// 从MVC封装的FormCollection容器中读取
        /// 传递方式:POST
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult SaveForm3(FormCollection form)
        {
            //实例化模型,并赋值
            T_Province data = new T_Province()
            {
                Province_Id = form.Get("id"),
                Province_Name = form.Get("name"),
                Province_Letter = form.Get("letter")
            };
            //构造返回数据
            var result = new IResult
            {
                data = data,
                header = RequestHeader
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// MVC 默认接收参数4
        /// 使用传统的Request请求取值
        /// 传递方式:GET/POST
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult SaveForm4()
        {
            //实例化模型,并赋值
            T_Province data = new T_Province()
            {
                Province_Id = Request["id"],
                Province_Name = Request["name"],
                Province_Letter = Request["letter"]
            };
            //构造返回数据
            var result = new IResult
            {
                data = data,
                header = RequestHeader
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 复杂业务应用方案

        /// <summary>
        /// List<T> 泛型数组接收
        /// 传递方式:GET/POST
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult SaveForm5(List<T_Province> data)
        {
            //构造返回数据
            var result = new IResult
            {
                data = data,
                header = RequestHeader
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 以数据流方式取得传递JSON字符,转换为业务模型
        /// 传递方式:POST
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //[Compression]
        public ActionResult SaveForm6()
        {
            //读取post数据流 方式一
            Byte[] buffer = new Byte[Request.InputStream.Length];
            Request.InputStream.Read(buffer, 0, buffer.Length);
            String jsonText = Encoding.UTF8.GetString(buffer);

            //读取post数据流 方式二
            //string jsonText = string.Empty;
            //using (StreamReader reader = new StreamReader(Request.InputStream, Encoding.UTF8))
            //{
            //    jsonText = reader.ReadToEnd();
            //}

            //使用Json.Net框架将JSON字符 转换实体模型
            List<T_Province> data = JsonConvert.DeserializeObject<List<T_Province>>(jsonText);

            //构造返回数据
            var result = new IResult
            {
                data = data,
                header = RequestHeader
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// JObject 动态模型接收
        /// 传递方式:POST
        /// Global.asax > Application_Start > ModelBinders.Binders.Add(typeof(JObject), new JsonModelBinder());
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult SaveForm7(JObject obj)
        {
            T_Province data = obj.ToObject<T_Province>();
            //构造返回数据
            var result = new IResult
            {
                data = data,
                header = RequestHeader
            };
            return Json(result);
        }
        /// <summary>
        /// JArray 动态接收
        /// 传递方式:POST
        /// Global.asax > Application_Start > ModelBinders.Binders.Add(typeof(JArray), new JsonModelBinder());
        /// </summary>
        /// <param name="arrobj"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult SaveForm8(JArray arrobj)
        {
            List<T_Province> data = arrobj.ToObject<List<T_Province>>();
            //构造返回数据
            var result = new IResult
            {
                data = data,
                header = RequestHeader
            };
            return Json(result);
        }
        /// <summary>
        /// JSON.NET JSON String 2 Model
        /// Global.asax > Application_Start > ModelBinders.Binders.DefaultBinder = new JsonModelBinder();
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult SaveForm8_2(T_Province[] data)
        {
            //构造返回数据
            var result = new IResult
            {
                data = data,
                header = RequestHeader
            };
            return Json(result);
        }

        #endregion

        #region JSONP

        /// <summary>
        /// JSONP 返回Object对象
        /// 传递方式:GET
        /// </summary>
        /// <param name="id"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public JsonpResult JsonpObject(T_City city, String id)
        {
            String sql = "SELECT * FROM [T_City] LIMIT 10";// WHERE City_Id='" + city.City_Id + "'
            List<T_City> data = db.ExecuteObject<List<T_City>>(sql);

            //构造返回数据
            var result = new IResult
            {
                data = data,
                header = RequestHeader
            };

            return new JsonpResult(result);
        }
        /// <summary>
        /// JSONP 分部视图
        /// 传递方式:GET
        /// </summary>
        /// <param name="city"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonpPartialViewResult JsonpPartialView1(String id)
        {
            ViewData["Request"] = RequestHeader;
            ViewData["测试"] = DateTime.Now.ToLongDateString();
            String sql = "SELECT * FROM [T_City] LIMIT 10";// WHERE City_Id='" + city.City_Id + "'
            List<T_City> data = db.ExecuteObject<List<T_City>>(sql);
            return new JsonpPartialViewResult(data);
            //JsonpResult(new { data = data, request = GetRequest() });
        }

        #endregion
    }
}
