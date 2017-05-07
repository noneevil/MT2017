using System;
using System.Drawing;

namespace CommonUtils
{
    public abstract class ColorHelper
    {
        private const int max = 255;
        /// <summary>
        /// 将指定的颜色转换为对应的HTML代码颜色
        /// </summary>
        /// <param name="color">颜色值</param>
        /// <returns>返回一个颜色字符串(eg.#1E3259)</returns>
        public static String ColorToString(Color color)
        {
            if (color.IsKnownColor || color.IsNamedColor || color.IsSystemColor)
            {
                return color.Name;
            }
            return ColorTranslator.ToHtml(color);
            //WebColorConverter web = new WebColorConverter();
            //return web.ConvertToString(color);
        }
        /// <summary>
        /// 将Web颜色名称转换为Color引用对象
        /// </summary>
        /// <param name="color">颜色名称</param>
        /// <returns></returns>
        public static Color StringToColor(String color)
        {
            return ColorTranslator.FromHtml(color);
            //WebColorConverter web = new WebColorConverter();
            //return (Color)web.ConvertFromString(color);
        }
        /// <summary>
        /// 生成随机颜色
        /// </summary>
        /// <returns></returns>
        public static Color RandomColor()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            int red = rand.Next(50, max);
            int green = rand.Next(50, max);
            int blue = rand.Next(50, max);
            blue = (red + green > 380) ? red + green - 380 : blue;
            blue = (blue > max) ? max : blue;

            return GetDarkerColor(Color.FromArgb(red, green, blue));
        }
        /// <summary>
        /// 获取加深颜色
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color GetDarkerColor(Color color)
        {
            int increase = new Random(Guid.NewGuid().GetHashCode()).Next(30, max); //还可以根据需要调整此处的值
            int r = Math.Abs(Math.Min(color.R - increase, max));
            int g = Math.Abs(Math.Min(color.G - increase, max));
            int b = Math.Abs(Math.Min(color.B - increase, max));

            return Color.FromArgb(r, g, b);
        }
    }
}
