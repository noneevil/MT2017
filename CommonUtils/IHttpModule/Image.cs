using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

/*
 * web.config配置
 * 经典模式
 * <system.web>
 *      <httpModules>
 *          <add name="SiteImage" type="CommonUtils.IHttpModule.SiteImage,CommonUtils" />
 *      </httpModules>
 * </system.web>
 * 集成模式
 * <system.webServer>
 *      <modules>
 *          <add name="SiteImage" type="CommonUtils.IHttpModule.SiteImage,CommonUtils"/>
 *      </modules>
 * </system.webServer>
 */
namespace CommonUtils.IHttpModule
{
    /// <summary>
    /// 动态生成图片缩略图
    /// </summary>
    public class SiteImage : System.Web.IHttpModule
    {
        #region IHttpModule 成员

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(this.Application_BeginRequest);
        }

        private void Application_BeginRequest(Object source, EventArgs e)
        {
            String file = Request.Url.LocalPath;
            String ex = Path.GetExtension(file).ToLower();
            if (String.IsNullOrEmpty(ex) || (ex != ".jpg" && ex != ".jpeg" && ex != ".bmp" && ex != ".png")) return;

            if (!File.Exists(Server.MapPath(file)))
            {
                Regex reg = new Regex(@"(?<file>[\S]+)_(?<width>[\d]+)x(?<hiehgt>[\d]+)");//thumb_
                Match match = reg.Match(file);
                if (match.Success)
                {
                    GroupCollection group = match.Groups;
                    String basefile = Server.MapPath(reg.Replace(file, group["file"].Value));
                    Int32 width = Int32.Parse(group["width"].Value);
                    Int32 hiehgt = Int32.Parse(group["hiehgt"].Value);

                    if (File.Exists(basefile))
                    {
                        ThumImage(basefile, width, hiehgt, file);
                        Response.End();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        public void Dispose()
        {

        }
        #endregion
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="src">源文件</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="savefile">输出文件</param>
        private void ThumImage(String src, int width, int height, String savefile)
        {
            Image img = new Bitmap(src);
            if (img.Width < width) width = img.Width;
            if (img.Height < height) height = img.Height;
            Decimal cutWidth = (img.Width * height / img.Height - width);
            Decimal cutHeight = (img.Height * width / img.Width - height);
            Image thumImg;
            if (cutHeight <= cutWidth)
            {
                thumImg = new Bitmap(img, width, height + (int)cutHeight);
            }
            else
            {
                thumImg = new Bitmap(img, width + (int)cutWidth, height);
            }
            int x = -(width - thumImg.Width) / 2;
            int y = -(height - thumImg.Height) / 2;

            MemoryStream ms = new MemoryStream();
            Bitmap ThumbImg = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(ThumbImg);
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            //g.Clear(Color.White);
            g.DrawImage(thumImg, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);

            ThumbImg.Save(Server.MapPath(savefile));
            ThumbImg.Save(ms, ImageFormat.Png);
            Response.Clear();
            ms.WriteTo(Response.OutputStream);

            ms.Close();
            g.Dispose();
            img.Dispose();
            ThumbImg.Dispose();
            thumImg.Dispose();
        }

        protected HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }
        protected HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }
        protected HttpServerUtility Server
        {
            get { return HttpContext.Current.Server; }
        }
    }
}
