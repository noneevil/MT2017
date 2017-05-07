using System;
using System.IO;
using System.Web;
/*
 * 经典模式
 * <system.web>
 *  <httpHandlers>
      <add path="Upload.axd" verb="*" type="CommonUtils.IHttpModule.Upload,CommonUtils"/>
    </httpHandlers>
 * </system.web>
 * 集成模式
 * <system.webServer>
 *  <handlers>
 *   <add name="Upload" path="Upload.axd" verb="*" type="CommonUtils.IHttpModule.Upload,CommonUtils"/>
 *  </handlers>
 * </system.webServer>
 */
namespace CommonUtils.IHttpModule
{
    /// <summary>
    /// 文件上传接口
    /// </summary>
    public class Upload : System.Web.IHttpHandler
    {
        #region IHttpHandler 成员

        public void ProcessRequest(System.Web.HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            HttpServerUtility Server = context.Server;

            for (int i = 0; i < Request.Files.Count; i++)
            {
                //HttpPostedFile file = Request.Files[i];
                //if (file.ContentLength == 0) continue;

                //String ex = Path.GetExtension(file.FileName).ToLower();
                //String Folder = Path.Combine(Server.MapPath("/UpFiles/"), ex.Substring(1), DateTime.Now.ToString("yyyyMMdd"));
                //FileHelper.CreatePath(Folder);

                //Int32 count = new DirectoryInfo(Folder).GetFiles().Length + 1;
                //String filename = Path.Combine(Folder, DateTime.Now.ToString("yyyyMMddHHmmss") + count.ToString("00000") + ex);
                //file.SaveAs(filename);

                //String outfile = "/" + new Uri(Server.MapPath("/")).MakeRelativeUri(new Uri(filename)).ToString();
                //Response.StatusCode = 200;
                //Response.Write(outfile);

                HttpPostedFile file = Request.Files[i];
                if (file.ContentLength == 0) continue;
                String ex = Path.GetExtension(file.FileName).ToLower().TrimStart('.');
                String Folder = String.Empty;
                String filename = String.Empty;

                if (String.IsNullOrEmpty(Request["dir"]))
                {
                    Folder = Path.Combine(Server.MapPath("/UpFiles/"), DateTime.Now.ToString("yyyy-MM-dd"));
                    FileHelper.CreatePath(Folder);

                    Int32 count = new DirectoryInfo(Folder).GetFiles().Length + 1;
                    filename = Path.Combine(Folder, DateTime.Now.ToString("yyyyMMdd") + count.ToString("00000") + "." + ex);
                    file.SaveAs(filename);
                }
                else
                {
                    Folder = Server.MapPath(HttpUtility.UrlDecode(Request["dir"]));

                    filename = Path.Combine(Folder, Path.GetFileNameWithoutExtension(file.FileName) + "." + ex);
                    int j = 0;
                    while (File.Exists(filename))
                    {
                        j++;
                        filename = Path.Combine(Folder, Path.GetFileNameWithoutExtension(file.FileName) + "-" + j.ToString("00") + "." + ex);
                    }
                    file.SaveAs(filename);
                }
                String outfile = "/" + new Uri(Server.MapPath("/")).MakeRelativeUri(new Uri(filename)).ToString();
                Response.StatusCode = 200;
                Response.Write(outfile);
            }
        }

        public Boolean IsReusable
        {
            get { return true; }
        }
        #endregion
    }
}
