using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using CommonUtils;
using WebSite.Interface;

namespace WebSite.Web
{
    public partial class AuthCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ClearContent();
            Response.ContentType = "image/png";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            String code = CheckCode();
            String val = EncryptHelper.MD5Upper32(code.ToLower() + Utils.GetIp());

            CookieHelper.AddCookie(ISessionKeys.cookie_authcode, val);
            //HttpCookie cookie = new HttpCookie(IKeys.cookie_authcode, val);
            //cookie.Expires = DateTime.Now.AddMinutes(5);
            //Response.Cookies.Add(cookie);

            CreateImage(code);
        }
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        private String CheckCode()
        {
            int number;
            Char txt;
            String textcode = String.Empty;
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                    txt = (Char)('0' + (Char)(number % 10));
                else
                    txt = (Char)('A' + (Char)(number % 26));
                textcode += txt.ToString();
            }
            return textcode;
        }
        /// <summary>
        /// 生成图片
        /// </summary>
        /// <param name="checkCode">验证码字串</param>
        private void CreateImage(String checkCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);

            //生成随机生成器
            Random random = new Random();

            //清空图片背景色
            //g.Clear(Color.White);
            //g.Clear(Color.FromArgb(23, 70, 113));
            //画图片的背景噪音线
            //for (int i = 0; i < 25; i++)
            //{
            //    int x1 = random.Next(image.Width);
            //    int x2 = random.Next(image.Width);
            //    int y1 = random.Next(image.Height);
            //    int y2 = random.Next(image.Height);

            //    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            //}

            Font font = new Font("Arial", 12, FontStyle.Bold | FontStyle.Italic);
            LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.White, Color.White, 1.2f, true);

            Graphics g = Graphics.FromImage(image);
            g.DrawString(checkCode, font, brush, 2, 2);

            //画图片的前景噪音点
            //for (int i = 0; i < 100; i++)
            //{
            //    int x = random.Next(image.Width);
            //    int y = random.Next(image.Height);
            //    image.SetPixel(x, y, Color.Silver);
            //}

            //画图片的边框线
            //g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                g.Dispose();
                image.Dispose();

                Response.BinaryWrite(ms.ToArray());
            }
        }
    }
}