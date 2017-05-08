using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace WebSite.Web
{
    public partial class Thumbnail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "image/jpeg";
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(30));

            var src = Request["file"].ToLower();
            if (src.ToLower().StartsWith("/upfiles/thumb"))
            {
                Response.Redirect(src, true);
            }
            else
            {
                String file = Server.MapPath(Request["file"]);
                Int32 width = Convert.ToInt32(Request["w"]);
                Int32 height = Convert.ToInt32(Request["h"]);
                BuildThumImage(file, width, height);
            }
        }
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="src">源文件</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        private void BuildThumImage(String src, Int32 width, Int32 height)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo format = encoders[1];//0-bmp 1-jpg 2-gif 3-tif 4-png
            EncoderParameters ep = new EncoderParameters(1);
            ep.Param[0] = new EncoderParameter(Encoder.Quality, 80L);

            String _savefile = String.Format("/UpFiles/Thumb/{0}_{1}x{2}.jpg", Request.Url.GetHashCode(), width, height);
            String savefile = Server.MapPath(_savefile);
            if (File.Exists(savefile))
            {
                Response.Redirect(_savefile, true);
                //Image img = new Bitmap(savefile);
                //img.Save(Response.OutputStream, format, ep);
                //img.Dispose();
            }
            else
            {
                Int32 left = 0, top = 0;
                Image srcimg = new Bitmap(src);
                Bitmap thumbimg = new Bitmap(width, height);
                if (srcimg.Width > width || srcimg.Height > height)
                {
                    Decimal cutwidth = srcimg.Width * height / srcimg.Height - width;
                    Decimal cutheight = srcimg.Height * width / srcimg.Width - height;
                    if (cutheight <= cutwidth)
                    {
                        srcimg = new Bitmap(srcimg, width, height + (int)cutheight);
                    }
                    else
                    {
                        srcimg = new Bitmap(srcimg, width + (int)cutwidth, height);
                    }
                }

                left = -(width - srcimg.Width) / 2;
                top = -(height - srcimg.Height) / 2;

                using (Graphics g = Graphics.FromImage(thumbimg))
                {
                    g.InterpolationMode = InterpolationMode.High;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.Clear(Color.White);
                    g.DrawImage(srcimg, new Rectangle(0, 0, width, height), new Rectangle(left, top, width, height), GraphicsUnit.Pixel);

                    thumbimg.Save(savefile, format, ep);
                    //thumbimg.Save(Response.OutputStream, format, ep);
                    Response.WriteFile(savefile);
                }

                thumbimg.Dispose();
                srcimg.Dispose();
            }
        }
    }
}