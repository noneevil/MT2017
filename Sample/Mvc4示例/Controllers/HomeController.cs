using CommonUtils;
using Dapper;
using MSSQLDB;
using Mvc4Example.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using MvcPaging;

//MVC API文档:http://msdn.microsoft.com/zh-cn/library/system.web.mvc(v=vs.118).aspx
namespace Mvc4Example.Controllers
{
    [HandleError]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 数据提交展示窗体
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveForm()
        {
            String sql = "SELECT * FROM [T_Province] ";//LIMIT 10
            List<T_Province> data = db.ExecuteObject<List<T_Province>>(sql);
            List<T_City> city = db.ExecuteObject<List<T_City>>("SELECT * FROM [T_City]");
            data.ForEach(a =>
            {
                a.City = city.FirstOrDefault(b =>
                {
                    return a.Province_Id == b.Province_Id;
                });
                //new T_City { City_Id = "111111", City_Name = "贵阳市", City_Letter = "G", Province_Id = "000000" };
            });
            ViewData["data"] = JsonConvert.SerializeObject(data);
            return View(data);
        }
        /// <summary>
        /// 数据压缩示例窗体
        /// </summary>
        /// <returns></returns>
        public ActionResult Compress()
        {
            return View();
        }
        /// <summary>
        /// Uploadify上传测试
        /// </summary>
        /// <returns></returns>
        public ActionResult Uploadify()
        {
            return View();
        }
        /// <summary>
        /// MVC JSON 日期格式
        /// </summary>
        /// <returns></returns>
        public ActionResult DateTime()
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            ViewData["data"] = serializer.Serialize(new { date = System.DateTime.Now });
            return View();
        }
        /// <summary>
        /// 表单验证示例
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Validate(int? cmd)
        {
            if (cmd != null) //Validate Remote 验证
            {
                //System.Threading.Thread.Sleep(1000);
                return Content("true");
            }
            return View();
        }
        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Validate(JObject obj)
        {
            return Content(JsonConvert.SerializeObject(obj), "application/json");
        }
        /// <summary>
        /// 网络收音机
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        //[Authorize]
        //[ValidateAntiForgeryToken]
        //[ValidateInput(true)]
        public ActionResult Radios(Int32? page)
        {
            int PageIndex = page ?? 1;
            int PageSize = 10;
            var list = db.ExecuteObject<List<T_RadiosEntity>>("SELECT * FROM [T_Radios]");
            int PageCount = (Int32)Math.Ceiling((Decimal)list.Count / (Decimal)PageSize);
            var data = list.Skip((PageIndex - 1) * PageSize).Take(PageSize);

            ViewData["分页字符"] = PrintPage(PageCount);
            return View(data);
        }

        #region 数据绑定

        /// <summary>
        /// 数据分页示例
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [OutputCache(Duration = 120, VaryByParam = "page", Location = OutputCacheLocation.ServerAndClient)]//使用web.config缓存配置
        public ActionResult List(Int32? page)
        {
            String sql = "SELECT COUNT(*) FROM [T_City]";//查询记录数
            Object _obj = db.ExecuteScalar(sql);

            Int32 PageCount = 0;//总页数
            Int32 PageSize = 20;//分页大小
            Int32 PageIndex = page ?? 1;
            Int32 RecordCount = Convert.ToInt32(_obj);//记录数
            var list = new List<T_City>();

            if (RecordCount > 0)
            {
                PageCount = (Int32)Math.Ceiling((Decimal)RecordCount / (Decimal)PageSize);
                if (PageIndex < 1) PageIndex = 1;
                if (PageIndex > PageCount) PageIndex = PageCount;

                sql = String.Format("SELECT * FROM [T_City] Limit {0} Offset {1}", PageSize, PageSize * (PageIndex - 1));
                list = db.ExecuteObject<List<T_City>>(sql);

                ViewData["分页字符"] = PrintPage(PageCount);
            }

            return View(list);
        }
       
        //http://blog.miniasp.com/post/2009/03/21/ASPNET-MVC-Developer-Note-Part-5-Data-Paging.aspx
        public ActionResult MvcPagingList(Int32? page)
        {
            Int32 PageSize = 20;//分页大小
            Int32 PageIndex = page.HasValue ? page.Value - 1 : 0;

            using (var d = new dbContext())
            {
                var list = d.City.AsNoTracking().ToList();
                var data = list.ToPagedList(PageIndex, PageSize);
                return View(data);
            }
            return new EmptyResult();
        }


        /// <summary>
        /// 局部视图示例
        /// </summary>
        /// <returns></returns>
        public ActionResult PartialView1()
        {
            var sql = String.Format("SELECT * FROM [T_City] Limit 5");
            var data = db.ExecuteObject<List<T_City>>(sql);
            ViewData["data"] = data;
            return View();
        }
        /// <summary>
        /// 局部视图 - 分页示例
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        //[ChildActionOnly]
        //[Compress]
        public PartialViewResult AjaxView(Int32? page, Int32? pagesize)
        {
            String sql = "SELECT COUNT(*) FROM [T_City]";//查询记录数
            Object _obj = db.ExecuteScalar(sql);

            Int32 PageCount = 0;//总页数
            Int32 PageSize = pagesize ?? 10;//分页大小
            Int32 PageIndex = page ?? 1;
            Int32 RecordCount = Convert.ToInt32(_obj);//记录数
            var list = new List<T_City>();

            if (RecordCount > 0)
            {
                PageCount = (Int32)Math.Ceiling((Decimal)RecordCount / (Decimal)PageSize);
                if (PageIndex < 1) PageIndex = 1;
                if (PageIndex > PageCount) PageIndex = PageCount;

                sql = String.Format("SELECT * FROM [T_City] Limit {0} Offset {1}", PageSize, PageSize * (PageIndex - 1));
                list = db.ExecuteObject<List<T_City>>(sql);

                ViewData["分页字符"] = PrintPage(PageCount);
            }
            return PartialView(list);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(String id)
        {
            var sql = String.Format("SELECT * FROM [T_City] WHERE city_id='" + id + "' LIMIT 1");
            var data = db.ExecuteObject<T_City>(sql);
            if (data == null) return HttpNotFound();
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_City data)
        {
            ViewData["data"] = JsonConvert.SerializeObject(data);
            return View();
        }


        #endregion

        #region

        /// <summary>
        /// 获取省份列表
        /// </summary>        
        [HttpPost]
        public ContentResult GetProvince()
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
        public ContentResult GetCity(int id)
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
        public ContentResult GetArea(int id)
        {
            String sql = String.Format("SELECT * FROM [T_Area] WHERE City_Id='{0}'", id);
            DataTable dt = db.ExecuteDataTable(sql);
            List<T_Area> list = ConvertHelper.DataTableToList<T_Area>(dt);
            return Json(list);
        }



        #endregion
    }
}
