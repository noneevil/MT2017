using Mvc4Example.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityFramework.Extensions;

namespace Mvc4Example.Controllers
{
    /// <summary>
    /// 实体框架示例 http://boytnt.blog.51cto.com/966121/977382
    /// </summary>
    public class EF6Controller : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Object data = null;
            using (var db = new dbContext())
            {
                #region 单表查询

                //data = db.Province.Count();//查询记录数量 - 无关联条件
                //data = db.Province.Count(a => a.Province_Letter.Contains("H"));//查询记录数量 - 模糊条件

                //data = db.Province.ToList();//所有数据
                //data = db.City.Where(a => a.City_Name.Contains("州") & a.City_Letter == "A").ToList();//多条件查询

                //data = db.Province.OrderBy(a => a.Province_Letter).ToList();//升序
                //data = db.Province.OrderByDescending(a => a.Province_Letter).ToList();//降序

                //组合条件查询
                //var query = db.Province.Select(a => a);
                //query = query.Where(a => a.Province_Letter == "X" || a.Province_Letter == "H");
                //query = query.Where(a => a.Province_Name.Contains("省"));
                //data = query.ToList();

                //分页
                //var PageSize = 5;
                //var PageIndex = 3;
                //data = db.City.Select(a => new
                //{
                //    Id = a.City_Id,         //
                //    Name = a.City_Name,     //设置查询字段 映射到匿名类型
                //    Letter = a.City_Letter  //
                //})
                //.OrderByDescending(a => a.Id)
                //.Skip((PageIndex - 1) * PageSize)
                //.Take(PageSize)
                //.ToList();


                //LEFT JOIN
                //data = db.Province.Select(a => new
                //{
                //    Pid = a.Province_Id,
                //    PName = a.Province_Name,
                //    a.Citys
                //}).ToList();

                //INNER JOIN 
                //data = db.City.Select(a => new
                //{
                //    Id = a.City_Id,
                //    Name = a.City_Name,
                //    PId = a.Province_Id,
                //    PName = db.Province.FirstOrDefault(g => g.Province_Id == a.Province_Id).Province_Name
                //}).ToList();

                //查询主表数据同时查询子表数据
                //data = db.Province.Include(x => x.Citys).ToList();

                //事务
                //using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                //{
                //    try
                //    {
                //        db.City.Add(new T_CityEntities { });
                //        db.SaveChanges();
                //    }
                //    catch (Exception ex)
                //    {
                //        dbTran.Rollback();
                //    }
                //}

                //分类
                //data = db.Group.Include(a => a.Childs).Where(x => x.ParentID == 0).ToList();

                #endregion

                #region 数据增删改

                //更新单个字段值
                //var obj = new T_CityEntities { City_Id = 110100, City_Name = "城市名称", City_Letter = "X" };
                //db.City.Attach(obj);
                //db.Entry(obj).Property(x => x.City_Name).IsModified = true;
                //db.SaveChanges();

                //更新整个实体
                //var obj = new T_CityEntities { City_Id = 110100, City_Name = "新城市名称", City_Letter = "X" };
                //db.City.Attach(obj);
                ////db.Entry(obj).State = EntityState.Modified;
                ////db.Entry<T_CityEntities>(obj).State = EntityState.Modified;
                //db.SaveChanges();

                //批量更新 EntityFramework.Extended
                //db.City.Where(x => x.Province_Id == 110000).Update(c => new T_CityEntities { City_Name = "批量更新" });

                //添加数据 
                //非自增主键设置元标记:[DatabaseGenerated(DatabaseGeneratedOption.None)]
                //var obj = new T_CityEntities { City_Id = 900000, City_Name = "新城市", City_Letter = "X", Province_Id = 0 };
                //db.City.Add(obj);
                ////db.Entry(obj).State = EntityState.Added;
                ////db.Entry<T_CityEntities>(obj).State = EntityState.Added;
                //db.SaveChanges();

                //删除数据
                //var obj = new T_CityEntities { City_Id = 900000 };
                //db.City.Remove(obj);
                ////db.Entry(obj).State = EntityState.Deleted;
                ////db.Entry<T_NewsEntities>(obj).State = EntityState.Deleted;
                //db.SaveChanges();

                //批量删除
                //var items = db.City.Where(x => x.Province_Id == 650000).ToList();
                //db.City.RemoveRange(items);
                ////db.City.Where(x => x.Province_Id == 650000).Delete(); //EntityFramework.Extended
                //db.SaveChanges();

                #endregion
                //一对多取第一条子表记录(:>MS Sql2005以上)
                //data = db.Area.Select(a => a.Area_Name).Distinct().Select(b => db.Area.FirstOrDefault(c => c.Area_Name == b)).ToList();

                //data = db.Province.ToList();//全部数据
                //data = (db.News.ta.Select(100));//主键查询

                //简单查询
                //data = db.News.SqlQuery("SELECT * FROM [t_news] ORDER BY id DESC").ToList();//SQL
                //data = db.News.Where(a => a.ID == 100 || a.ID == 99).OrderBy(a => a.ID).ToList();//Func形式 
                //data = (from a in db.News where a.ID == 100 || a.ID == 99 orderby a.ID select a).ToList();//Linq

                //查询部分字段
                //var d1 = db.News.Where(a => a.ID > 1).Select(a => new { a = a.ID, b = a.Title }).ToList();
                //return Content(JsonConvert.SerializeObject(d1));

                //查询单一记录：
                //var d2 = db.News.FirstOrDefault(a => a.ID == 100);
                //return Content(JsonConvert.SerializeObject(d2));

                //LEFT JOIN 连接查询
                //var d3 = db.City.Select(a => new
                //{
                //    a.City_Id,
                //    a.City_Name,
                //    a.City_Letter,
                //    a.Province_Id,
                //    Province_Name = db.Province.FirstOrDefault(b => b.Province_Id == a.Province_Id).Province_Name
                //}).ToList();
                //return Content(JsonConvert.SerializeObject(d3));

                //INNER JOIN 连接查询
                //var d4 = db.News.Where(c => c.ID > 0).OrderBy(c => c.ID).Skip(20).Take(3).ToList();
                //return Content(JsonConvert.SerializeObject(d4));

            }
            return Content(JsonConvert.SerializeObject(data));
            return View(data);
        }

        //public ActionResult Edit()
        //{
        //    return View();
        //}

        //public ActionResult Edit(T_NewsEntities data)
        //{
        //    return View();
        //}
    }
}
