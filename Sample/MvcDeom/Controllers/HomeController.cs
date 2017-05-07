using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Mvc;
using MSSQLDB;
using CommonUtils;
using MvcSite.Models;
using Newtonsoft.Json;
using System.Drawing;
using System.Diagnostics;

namespace MvcSite.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            String sql = "SELECT * FROM T_Province INNER JOIN T_City ON T_Province.Province_Id = T_City.Province_Id WHERE T_Province.Province_Id='520000'";
            DataTable dt = db.ExecuteDataTable(sql);
            List<T_Province> list = ConvertHelper.DataTableToList<T_Province>(dt);
            //string json = SerializerHelper.ObjectToJson(dt.AsEnumerable().ToList());

            //Response.Write(json);
            //Response.End();
            ViewData["Message"] = "欢迎使用 ASP.NET MVC!";
            return View(list);
        }
        /// <summary>
        /// 返回JSON
        /// </summary>
        /// <returns></returns>
        public JsonResult Android()
        {
            //T_Product product = new T_Product()
            //{
            //    Product_Name = "产品名字",
            //    Product_Remark = "商品备注",
            //    Start_Time = Convert.ToDateTime("2013-1-5 23:58:50"),
            //    End_Time = Convert.ToDateTime("2013-1-5 23:59:00")
            //};
            //return Json(product, JsonRequestBehavior.AllowGet);

            String sql = "SELECT  a.*, b.City_Id, b.City_Name, b.City_Letter FROM T_Province AS a INNER JOIN T_City AS b ON a.Province_Id = b.Province_Id WHERE a.Province_Id='520000'";
            List<T_Province> province = db.ExecuteObject<List<T_Province>>(sql);

            return Json(province, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 页面调用
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewPage1()
        {
            String sql = "SELECT a.*, b.City_Id, b.City_Name, b.City_Letter FROM T_Province AS a INNER JOIN T_City AS b ON a.Province_Id = b.Province_Id";
            List<T_Province> province = db.ExecuteObject<List<T_Province>>(sql);
            return View(province[0]);
        }
        public ActionResult About()
        {
            return View();
        }
        /// <summary>
        /// JSON提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult SaveForm(T_City city)
        {
            Byte[] b = new Byte[Request.InputStream.Length];
            Request.InputStream.Read(b, 0, b.Length);
            string json = Encoding.UTF8.GetString(b);

            //StreamReader reader = new StreamReader(Request.InputStream);
            //string json = reader.ReadToEnd();

            //List<T_City> list = SerializerHelper.JsonToObject<List<T_City>>(json);
            List<T_City> list = JsonConvert.DeserializeObject<List<T_City>>(json);
            json = JsonConvert.SerializeObject(list);

            return Content(json, "application/json");
        }
        /// <summary>
        /// JS淡入淡出数据
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ViewUserControl1()
        {
            String sql = "SELECT id,title,pubdate,editdate,imageurl FROM T_News ORDER BY id DESC";
            List<T_News> news = db.ExecuteObject<List<T_News>>(sql);
            return PartialView(news);
        }
        /// <summary>
        /// 获取省份列表
        /// </summary>        
        //[ValidateAntiForgeryToken]
        //[ValidateInput(true)]
        //[Authorize]
        [HttpPost]
        public JsonResult GetProvince()
        {
            DataTable dt = db.ExecuteDataTable("SELECT * FROM [T_Province] ORDER BY Province_Letter");
            List<T_Province> list = ConvertHelper.DataTableToList<T_Province>(dt);// ConvertHelper<T_Province>.ConvertToList(dt);
            return Json(list);
        }
        /// <summary>
        /// 获取城市数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCity(int id)
        {
            String sql = String.Format("SELECT * FROM [T_City] WHERE Province_Id='{0}'", id);
            DataTable dt = db.ExecuteDataTable(sql);
            List<T_City> list = ConvertHelper.DataTableToList<T_City>(dt);
            return Json(list);
        }
        /// <summary>
        /// 获取地区数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetArea(int id)
        {
            String sql = String.Format("SELECT * FROM [T_Area] WHERE City_Id='{0}'", id);
            DataTable dt = db.ExecuteDataTable(sql);
            List<T_Area> list = ConvertHelper.DataTableToList<T_Area>(dt);
            return Json(list);
        }
    }
}
