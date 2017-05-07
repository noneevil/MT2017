using MSSQLDB;
using Mvc4Example.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Mvc4Example.Controllers
{
    /// <summary>
    /// 数据压缩示例
    /// </summary>
    public class CompressController : BaseController
    {
        /// <summary>
        /// 清除HTML空白
        /// </summary>
        /// <returns></returns>
        [Compression("html")]
        public ActionResult clearHtmlCompress()
        {
            var sql = "SELECT * FROM [T_City]";
            var data = db.ExecuteObject<List<T_City>>(sql);
            return View("AjaxView", data);
        }
        /// <summary>
        /// 7z压缩后编辑成base64字符串
        /// </summary>
        /// <returns></returns>
        [Compression("base64")]
        public ActionResult base64Compress()
        {
            var sql = "SELECT * FROM [T_Radios]";
            var data = db.ExecuteObject<List<T_RadiosEntity>>(sql);
            return Content(JsonConvert.SerializeObject(data), "application/octet-stream");
        }
        /// <summary>
        /// 7z压缩
        /// </summary>
        /// <returns></returns>
        [Compression("7z")]
        public ActionResult z7Compress()
        {
            var sql = "SELECT * FROM [T_Radios]";
            var data = db.ExecuteObject<List<T_RadiosEntity>>(sql);
            return Content(JsonConvert.SerializeObject(data), "application/octet-stream");
        }
    }
}
