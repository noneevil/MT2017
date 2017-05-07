using System;

namespace CommonUtils
{
    /// <summary>
    /// 人民币大小写金额转换
    /// </summary>
    public abstract class MoneyHelper
    {
        /// <summary>
        /// 将商品金额小写转换成大写
        /// </summary>
        /// <param name="par">小写金额</param>
        /// <returns></returns>
        public static String MoneySmallToBig(String par)
        {
            String[] Scale = { "分", "角", "元", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟", "兆", "拾", "佰", "仟" };
            String[] Base = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
            String Temp = par;
            String result = null;
            int index = Temp.IndexOf(".", 0, Temp.Length);//判断是否有小数点
            if (index != -1)
            {
                Temp = Temp.Remove(Temp.IndexOf("."), 1);
                for (int i = Temp.Length; i > 0; i--)
                {
                    int Data = System.Convert.ToInt16(Temp[Temp.Length - i]);
                    result += Base[Data - 48];
                    result += Scale[i - 1];
                }
            }
            else
            {
                for (int i = Temp.Length; i > 0; i--)
                {
                    int Data = System.Convert.ToInt16(Temp[Temp.Length - i]);
                    result += Base[Data - 48];
                    result += Scale[i + 1];
                }
            }
            return result;
        }
        //零仟零佰零拾零兆零仟零佰零拾零亿零仟零佰壹拾贰万叁仟肆佰伍拾陆元    柒角捌分
        //零壹贰叁肆伍陆柒捌玖拾佰仟万亿兆京顺
        //此处两组字符串顺序完全相反
        //static String[] rightUnit = new String[] { "仟", "佰", "拾", "兆", "仟", "佰", "拾", "亿", "仟", "佰", "拾", "万", "仟", "佰", "拾", "元" };
        static String[] _LeftUnit = new String[] { "元", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟", "兆", "拾", "佰", "仟" };
        static String[] _RightUnit = new String[] { "分", "角" };
        private MoneyHelper() { }

        /// <summary>
        /// 将阿拉伯数字表示的金额以大写形式展示
        /// (最大只支持16位整数和2位小数的转换)
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static String Convert(decimal amount)
        {
            String[] amounts = amount.ToString().Split('.');
            String left = amounts[0];
            String right = null;
            if (amounts.Length == 2)
            {
                right = amounts[1];
            }

            String result = null;
            left = LeftConvert(left);
            right = RightConvert(right);
            if (!left.EndsWith("元")) left += "元";
            if (String.IsNullOrEmpty(right))
            {
                result = left + "整";
            }
            else
            {
                result = left + right;
            }
            return result;
        }
        /// <summary>
        /// 转换金额左边整数部分(最大支持16位数，超出部分会被截断)
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        private static String LeftConvert(String left)
        {
            String result = null;
            if (!String.IsNullOrEmpty(left))
            {
                if (left.Length > 16) left = left.Substring(left.Length - 16, 16);

                for (int i = 0; i < left.Length; i++)
                {
                    if (left[i] == '0') continue;
                    result += DigitToUper(left[i].ToString()) + "{" + (left.Length - 1 - i) + "}";
                }

                result = String.Format(result, _LeftUnit);
            }
            return result;
        }
        /// <summary>
        /// 转换金额右边小数部分(最大支持2位小数，超出部分会被截断)
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        private static String RightConvert(String right)
        {
            String result = null;
            if (!String.IsNullOrEmpty(right))
            {
                if (right.Length > 2) right = right.Substring(0, 2);

                for (int i = 0; i < right.Length; i++)
                {
                    if (right[i] == '0') continue;
                    result += DigitToUper(right[i].ToString()) + "{" + (right.Length - 1 - i) + "}";
                }

                result = String.Format(result, _RightUnit);
            }
            return result;
        }
        /// <summary>
        /// 将数字转换为相应的大写形式
        /// </summary>
        /// <param name="digit"></param>
        /// <returns></returns>
        private static String DigitToUper(String digit)
        {
            String result = null;
            switch (digit)
            {
                case "0":
                    result = "零";
                    break;
                case "1":
                    result = "壹";
                    break;
                case "2":
                    result = "贰";
                    break;
                case "3":
                    result = "叁";
                    break;
                case "4":
                    result = "肆";
                    break;
                case "5":
                    result = "伍";
                    break;
                case "6":
                    result = "陆";
                    break;
                case "7":
                    result = "柒";
                    break;
                case "8":
                    result = "捌";
                    break;
                case "9":
                    result = "玖";
                    break;
            }
            return result;
        }
    }
}
