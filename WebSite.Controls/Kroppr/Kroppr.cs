using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;

namespace WebSite.Controls
{
    public class Kroppr : IHttpHandler
    {
        #region IHttpHandler 成员

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            HttpServerUtility Server = context.Server;

            String data = String.Empty;
            using (StreamReader sr = new StreamReader(Request.InputStream))
            {
                data = sr.ReadToEnd();
            }
            KropprParameter k = JsonConvert.DeserializeObject<KropprParameter>(data);

            KropprResult result = new KropprResult()
            {
                type = "jpg",
                image = k.image.LocalPath// "/kroppr/img/" + Guid.NewGuid().ToString() + ".jpg"
            };

            Int32 zoomWidth = (Int32)(k.original.w * k.xfact);
            Int32 zoomHeight = (Int32)(k.original.h * k.xfact);
            GraphicsPath p = new GraphicsPath();
            p.AddRectangle(new RectangleF(0, 0, zoomWidth, zoomHeight));
            Matrix m = new Matrix();
            m.Rotate(k.rotation % 360);
            RectangleF rct = p.GetBounds(m);
            m.Dispose();
            p.Dispose();

            Bitmap initimg = new Bitmap(Server.MapPath(k.image.LocalPath));
            Bitmap zoomimg = new Bitmap(zoomWidth, zoomHeight);
            using (Graphics g = Graphics.FromImage(zoomimg))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.High;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawImage(initimg, new Rectangle(0, 0, zoomWidth, zoomHeight), new Rectangle(0, 0, initimg.Width, initimg.Height), GraphicsUnit.Pixel);
            }

            Bitmap angleimg = new Bitmap((Int32)rct.Width, (Int32)rct.Height);
            using (Graphics g = Graphics.FromImage(angleimg))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.High;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.TranslateTransform(-rct.X, -rct.Y);
                g.RotateTransform(k.rotation % 360);
                g.DrawImage(zoomimg, 0, 0);
            }
            initimg.Dispose();

            Bitmap Canvas = new Bitmap(k.original.w, k.original.h);
            using (Graphics g = Graphics.FromImage(Canvas))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.High;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawImageUnscaled(angleimg, (Int32)k.offset.x - (angleimg.Width - zoomWidth) / 2, (Int32)k.offset.y - (angleimg.Height - zoomHeight) / 2);
            }

            Bitmap Thumb = new Bitmap(k.cropper.width, k.cropper.height);
            using (Graphics g = Graphics.FromImage(Thumb))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.High;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawImage(Canvas, -k.cropper.left, -k.cropper.top);
            }

            ImageCodecInfo Jpeg = ImageCodecInfo.GetImageEncoders().First(a => { return a.MimeType == "image/jpeg"; });
            EncoderParameters ep = new EncoderParameters(1);
            ep.Param[0] = new EncoderParameter(Encoder.Quality, k.quality);

            Thumb.Save(Server.MapPath(result.image), Jpeg, ep);

            ep.Dispose();
            
            zoomimg.Dispose();
            angleimg.Dispose();
            Canvas.Dispose();
            Thumb.Dispose();

            result.status = true;
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(result));
        }

        public Boolean IsReusable
        {
            get { return false; }
        }

        #endregion
        /// <summary>
        /// 底片效果
        /// </summary>
        /// <param name="img"></param>
        void ReverseColor(Bitmap img)
        {
            Color pixel;
            for (Int32 x = 1; x < img.Width; x++)
            {
                for (Int32 y = 1; y < img.Height; y++)
                {
                    Int32 r, g, b;
                    pixel = img.GetPixel(x, y);
                    r = 255 - pixel.R;
                    g = 255 - pixel.G;
                    b = 255 - pixel.B;
                    img.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
        }
        /// <summary>
        /// 浮雕效果
        /// </summary>
        /// <param name="img"></param>
        void Relief(Bitmap img)
        {
            Color pixel1, pixel2;
            for (Int32 x = 0; x < img.Width - 1; x++)
            {
                for (Int32 y = 0; y < img.Height - 1; y++)
                {
                    Int32 r = 0, g = 0, b = 0;
                    pixel1 = img.GetPixel(x, y);
                    pixel2 = img.GetPixel(x + 1, y + 1);
                    r = Math.Abs(pixel1.R - pixel2.R + 128);
                    g = Math.Abs(pixel1.G - pixel2.G + 128);
                    b = Math.Abs(pixel1.B - pixel2.B + 128);
                    if (r > 255) r = 255;
                    if (r < 0) r = 0;
                    if (g > 255) g = 255;
                    if (g < 0) g = 0;
                    if (b > 255) b = 255;
                    if (b < 0) b = 0;
                    img.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
        }
        /// <summary>
        /// 黑白效果
        /// </summary>
        /// <param name="img"></param>
        void Gray(Bitmap img)
        {
            Color pixel;
            for (Int32 x = 0; x < img.Width; x++)
            {
                for (Int32 y = 0; y < img.Height; y++)
                {
                    pixel = img.GetPixel(x, y);
                    Int32 r, g, b, Result = 0;
                    r = pixel.R;
                    g = pixel.G;
                    b = pixel.B;                        //实例程序以加权平均值法产生黑白图像     
                    Int32 iType = 2;
                    switch (iType)
                    {
                        case 0://平均值法        
                            Result = ((r + g + b) / 3);
                            break;
                        case 1://最大值法     
                            Result = r > g ? r : g;
                            Result = Result > b ? Result : b;
                            break;
                        case 2://加权平均值法   
                            Result = ((Int32)(0.7 * r) + (Int32)(0.2 * g) + (Int32)(0.1 * b));
                            break;
                    }
                    img.SetPixel(x, y, Color.FromArgb(Result, Result, Result));
                }
            }
        }
        /// <summary>
        /// 柔化效果
        /// </summary>
        /// <param name="img"></param>
        void Soften(Bitmap img)
        {
            Color pixel;
            //高斯模板            
            Int32[] Gauss = { 1, 2, 1, 2, 4, 2, 1, 2, 1 };
            for (Int32 x = 1; x < img.Width - 1; x++)
            {
                for (Int32 y = 1; y < img.Height - 1; y++)
                {
                    Int32 r = 0, g = 0, b = 0;
                    Int32 Index = 0;
                    for (Int32 col = -1; col <= 1; col++)
                    {
                        for (Int32 row = -1; row <= 1; row++)
                        {
                            pixel = img.GetPixel(x + row, y + col);
                            r += pixel.R * Gauss[Index]; g += pixel.G * Gauss[Index];
                            b += pixel.B * Gauss[Index]; Index++;
                        }
                    }
                    r /= 16;
                    g /= 16;
                    b /= 16;                        //处理颜色值溢出      
                    r = r > 255 ? 255 : r; r = r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g; g = g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b; b = b < 0 ? 0 : b;
                    img.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
                }
            }
        }
        /// <summary>
        /// 锐化效果
        /// </summary>
        /// <param name="img"></param>
        void Sharpen(Bitmap img)
        {
            Color pixel;                //拉普拉斯模板    
            Int32[] Laplacian = { -1, -1, -1, -1, 9, -1, -1, -1, -1 };
            for (Int32 x = 1; x < img.Width - 1; x++)
            {
                for (Int32 y = 1; y < img.Height - 1; y++)
                {
                    Int32 r = 0, g = 0, b = 0;
                    Int32 Index = 0;
                    for (Int32 col = -1; col <= 1; col++)
                    {
                        for (Int32 row = -1; row <= 1; row++)
                        {
                            pixel = img.GetPixel(x + row, y + col);
                            r += pixel.R * Laplacian[Index];
                            g += pixel.G * Laplacian[Index];
                            b += pixel.B * Laplacian[Index];
                            Index++;
                        }
                    }
                    //处理颜色值溢出                   
                    r = r > 255 ? 255 : r;
                    r = r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b;
                    b = b < 0 ? 0 : b;
                    img.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
                }
            }
        }
        /// <summary>
        /// 雾化效果
        /// </summary>
        /// <param name="img"></param>
        void Atomization(Bitmap img)
        {
            Color pixel;
            for (Int32 x = 1; x < img.Width - 1; x++)
            {
                for (Int32 y = 1; y < img.Height - 1; y++)
                {
                    Random r = new Random();
                    Int32 k = r.Next(123456);                        //像素块大小   
                    Int32 dx = x + k % 19; Int32 dy = y + k % 19;
                    if (dx >= img.Width) dx = img.Width - 1;
                    if (dy >= img.Height) dy = img.Height - 1;
                    pixel = img.GetPixel(dx, dy);
                    img.SetPixel(x, y, pixel);
                }
            }
        }
        /// <summary>
        /// 保存图片。
        /// </summary>
        /// <param name="image">要保存的图片</param>
        /// <param name="quality">品质（1L~100L之间，数值越大品质越好）</param>
        /// <param name="filename">保存路径</param>
        void SaveIamge(Bitmap image, long quality, String filename)
        {
            using (EncoderParameters encoderParams = new EncoderParameters(1))
            {
                using (EncoderParameter parameter = (encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality)))
                {
                    ImageCodecInfo encoder = null;
                    //取得扩展名
                    String ext = Path.GetExtension(filename);
                    if (String.IsNullOrEmpty(ext))
                        ext = ".jpg";
                    //根据扩展名得到解码、编码器
                    foreach (ImageCodecInfo codecInfo in ImageCodecInfo.GetImageEncoders())
                    {
                        if (Regex.IsMatch(codecInfo.FilenameExtension, String.Format(@"(;|^)\*\{0}(;|)", ext), RegexOptions.IgnoreCase))
                        {
                            encoder = codecInfo;
                            break;
                        }
                    }
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));
                    image.Save(filename, encoder, encoderParams);
                }
            }
        }
    }
}
