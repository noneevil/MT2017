using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using CommonUtils.Enumeration;

namespace CommonUtils
{
    /// <summary>
    /// 图片处理类
    /// 主要功能：生成缩略图，生成图片水印
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// 缩略图。
        /// </summary>
        /// <param name="image">要缩略的图片</param>
        /// <param name="size">要缩放的尺寸</param>
        /// <returns>返回已经缩放的图片。</returns>
        public static Bitmap Thumbnail(String fileName, Size size)
        {
            Bitmap image = new Bitmap(fileName);
            if (!size.IsEmpty && !image.Size.IsEmpty && !size.Equals(image.Size))
            {
                //先取一个宽比例。
                Double scale = (Double)image.Width / (Double)size.Width;
                //缩略模式
                if (image.Height > image.Width)
                {

                    scale = (Double)image.Height / (Double)size.Height;
                }
                SizeF newszie = new SizeF((float)(image.Width / scale), (float)(image.Height / scale));
                Bitmap result = new Bitmap((int)newszie.Width, (int)newszie.Height);
                using (Graphics g = Graphics.FromImage(result))
                {
                    g.FillRectangle(Brushes.White, new Rectangle(new Point(0, 0), size));
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingMode = CompositingMode.SourceOver;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    g.DrawImage(image, 0, 0, newszie.Width, newszie.Height);
                    image.Dispose();
                }
                return result;
            }
            else
                return image;
        }
        /// <summary>
        /// 保存图片。
        /// </summary>
        /// <param name="image">要保存的图片</param>
        /// <param name="quality">品质（1L~100L之间，数值越大品质越好）</param>
        /// <param name="filename">保存路径</param>
        public static void SaveIamge(Bitmap image, long quality, String filename)
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
        /// <summary>
        /// 保存图片。
        /// </summary>
        /// <param name="stream">要保存的流</param>
        /// <param name="quality">品质（1L~100L之间，数值越大品质越好）</param>
        /// <param name="filename">保存路径</param>
        public static void SaveIamge(Stream stream, long quality, String filename)
        {
            using (Bitmap bmpTemp = new Bitmap(stream))
            {
                SaveIamge(bmpTemp, quality, filename);
            }
        }
        /// <summary>
        /// 将指定图片文件加上水印
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="wType">水印类型</param>
        /// <param name="wLocation">水印位置</param>
        /// <param name="Text">水印文字或图片地址。wType为“文字水印”时则为文字内容，否则则为水印图片地址</param>
        public static void WatermarkImage(String filename, WatermarkType wType, WatermarkLocation wLocation, Font font, Color color, String Text)
        {
            String _fileName = HttpContext.Current.Server.MapPath(filename);
            if (File.Exists(_fileName))
            {  //创建一个bitmap类型的bmp变量来读取文件。
                Bitmap bmp = new Bitmap(_fileName);
                //新建第二个bitmap类型的bmp2变量，我这里是根据我的程序需要设置的。
                Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height, bmp.PixelFormat);
                //将第一个bmp拷贝到bmp2中
                Graphics draw = Graphics.FromImage(bmp2);
                draw.DrawImage(bmp, 0, 0);
                draw.Dispose();
                bmp.Dispose();//释放bmp文件资源

                bmp2 = GetWatermarkImage(bmp2, wType, wLocation, font, color, Text);
                bmp2.Save(_fileName);
            }
        }
        /// <summary>
        /// 生成水印图片
        /// </summary>
        /// <param name="bmp">原图</param>
        /// <param name="wType">水印类型</param>
        /// <param name="wLocation">水印位置</param>
        /// <param name="Text">水印文字或图片地址。wType为“文字水印”时则为文字内容，否则则为水印图片地址</param>
        /// <returns></returns>
        protected static Bitmap GetWatermarkImage(Bitmap bmp, WatermarkType wType, WatermarkLocation wLocation, Font font, Color color, String Text)
        {
            Bitmap _bmp = bmp;
            using (Graphics g = Graphics.FromImage(_bmp))
            {
                int w = _bmp.Width;                     //原图宽度
                int h = _bmp.Height;                    //原图高度
                SizeF s = g.MeasureString(Text, font);  //文字所占大小
                Image img = null;
                if (wType == WatermarkType.图片水印)
                {
                    String fileName = HttpContext.Current.Server.MapPath(Text);
                    if (File.Exists(fileName))
                    {
                        img = Image.FromFile(fileName);
                        if (img.Width > w || img.Height > h)
                        {
                            g.Dispose();
                            return _bmp;
                        }
                        s = new SizeF(img.Width, img.Height);
                    }
                    else
                    {
                        g.Dispose();
                        return _bmp;
                    }
                }
                else
                {
                    if (s.Width > w || s.Height > h) return _bmp;
                }
                Point p = new Point(0, 0);
                switch (wLocation)
                {
                    case WatermarkLocation.上左:
                        p = new Point(5, 5);
                        break;
                    case WatermarkLocation.上中:
                        p = new Point((w - (int)s.Width) / 2, 5);
                        break;
                    case WatermarkLocation.上右:
                        p = new Point((w - (int)s.Width) - 5, 5);
                        break;
                    case WatermarkLocation.中左:
                        p = new Point(5, (h - (int)s.Height) / 2);
                        break;
                    case WatermarkLocation.中中:
                        p = new Point((w - (int)s.Width) / 2, (h - (int)s.Height) / 2);
                        break;
                    case WatermarkLocation.中右:
                        p = new Point((w - (int)s.Width) - 5, (h - (int)s.Height) / 2);
                        break;
                    case WatermarkLocation.下左:
                        p = new Point(5, (h - (int)s.Height) - 5);
                        break;
                    case WatermarkLocation.下中:
                        p = new Point((w - (int)s.Width) / 2, (h - (int)s.Height) - 5);
                        break;
                    case WatermarkLocation.下右:
                        p = new Point((w - (int)s.Width) - 5, (h - (int)s.Height) - 5);
                        break;
                }
                g.CompositingMode = CompositingMode.SourceOver;
                g.CompositingQuality = CompositingQuality.HighQuality;
                if (wType == WatermarkType.图片水印)
                {
                    float[][] ptsArray ={ 
                        new float[] {1, 0, 0, 0, 0},
                        new float[] {0, 1, 0, 0, 0},
                        new float[] {0, 0, 1, 0, 0},
                        new float[] {0, 0, 0, 0.5f, 0}, //注意：此处为0.5f，图像为半透明
                        new float[] {0, 0, 0, 0, 1}
                    };
                    ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
                    ImageAttributes imgAttributes = new ImageAttributes();
                    //设置图像的颜色属性
                    imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default,
                    ColorAdjustType.Bitmap);

                    g.DrawImage(img, new Rectangle(p.X, p.Y, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttributes);

                    img.Dispose();
                }
                else
                {
                    g.DrawString(Text, font, new SolidBrush(color), p);
                }

            }
            return _bmp;
        }
    }
}
