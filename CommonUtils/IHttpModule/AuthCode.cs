using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

/*
 * 经典模式
 * <system.web>
 *  <httpHandlers>
      <add path="AuthCode.axd" verb="*" type="CommonUtils.IHttpModule.AuthCode,CommonUtils"/>
    </httpHandlers>
 * </system.web>
 * 集成模式
 * <system.webServer>
 *  <handlers>
 *   <add name="AuthCode" path="AuthCode.axd" verb="*" type="CommonUtils.IHttpModule.AuthCode,CommonUtils"/>
 *  </handlers>
 * </system.webServer>
 */
namespace CommonUtils.IHttpModule
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class AuthCode : System.Web.IHttpHandler
    {
        #region IHttpHandler 成员

        public void ProcessRequest(System.Web.HttpContext context)
        {
            String code = TextHelper.RandomText(5);
            //context.Session["Site.AuthCode"] = code;

            HttpCookie cookie = new HttpCookie("Site.AuthCode", EncryptHelper.MD5Lower32(code.ToLower()));
            HttpContext.Current.Response.Cookies.Add(cookie);

            CreateCheckCodeImage(code, context);
        }

        public Boolean IsReusable
        {
            get { return true; }
        }

        #endregion

        /// <summary>
        /// 生成图片
        /// </summary>
        /// <param name="checkCode">验证码字串</param>
        /// <param name="context"></param>
        private void CreateCheckCodeImage(String checkCode, System.Web.HttpContext context)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();

                //清空图片背景色
                //g.Clear(Color.White);
                g.Clear(Color.FromArgb(23, 70, 113));
                //画图片的背景噪音线
                //for (int i = 0; i < 5; i++)
                //{
                //int x1 = random.Next(image.Width);
                //int x2 = random.Next(image.Width);
                //int y1 = random.Next(image.Height);
                //int y2 = random.Next(image.Height);

                //g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                //}

                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                //LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.White, Color.White, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);

                //画图片的前景噪音点
                //for (int i = 0; i < 20; i++)
                //{
                //    int x = random.Next(image.Width);
                //    int y = random.Next(image.Height);
                //    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                //}

                //画图片的边框线
                //g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Png);
                context.Response.ClearContent();
                context.Response.ContentType = "image/Png";
                context.Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

    }
}
