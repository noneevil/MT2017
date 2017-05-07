
using System;
using System.Globalization;

namespace CommonUtils
{
    /// <summary>
    /// 日期扩展类
    /// </summary>
    public abstract class DateHelper
    {
        /// <summary>
        /// 返回中文日期 格式:2013年2月28日 星期四 农历正月十九 
        /// </summary>
        /// <returns></returns>
        public static String ChinaDate()
        {
            ChineseLunisolarCalendar l = new ChineseLunisolarCalendar();
            DateTime dt = DateTime.Today; //转换当日的日期
            //dt = new DateTime(2006, 1,29);//农历2006年大年初一（测试用），也可指定日期转换
            String[] aMonth = { "", "正月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "冬月", "腊月", "腊月" };
            //a10表示日期的十位!
            String[] a10 = { "初", "十", "廿", "卅" };
            String[] aDigi = { "Ｏ", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
            String sYear = "", sYearArab = "", sMonth = "", sDay = "", sDay10 = "", sDay1 = "", sLuniSolarDate = "";
            int iYear, iMonth, iDay;
            iYear = l.GetYear(dt);
            iMonth = l.GetMonth(dt);
            iDay = l.GetDayOfMonth(dt);
            //Format Year
            sYearArab = iYear.ToString();
            for (int i = 0; i < sYearArab.Length; i++)
            {
                sYear += aDigi[Convert.ToInt16(sYearArab.Substring(i, 1))];
            }

            //Format Month
            int iLeapMonth = l.GetLeapMonth(iYear); //获取闰月

            /*
             * 闰月可以出现在一年的任何月份之后。
             * 例如，GetMonth 方法返回一个介于 1 到 13 之间的数字来表示与指定日期关联的月份。
             * 如果在一年的八月和九月之间有一个闰月，则 GetMonth 方法为八月返回 8，为闰八月返回 9，为九月返回 10。
             */

            if (iLeapMonth > 0 && iMonth <= iLeapMonth)
            {
                //for (int i = iLeapMonth + 1; i < 13; i++) aMonth[i] = aMonth[i - 1];
                aMonth[iLeapMonth] = "闰" + aMonth[iLeapMonth - 1];
                sMonth = aMonth[l.GetMonth(dt)];
            }
            else if (iLeapMonth > 0 && iMonth > iLeapMonth)
            {
                sMonth = aMonth[l.GetMonth(dt) - 1];
            }
            else
            {
                sMonth = aMonth[l.GetMonth(dt)];
            }
            //Format Day
            sDay10 = a10[iDay / 10];
            sDay1 = aDigi[(iDay % 10)];
            sDay = sDay10 + sDay1;

            if (iDay == 10) sDay = "初十";
            if (iDay == 20) sDay = "二十";
            if (iDay == 30) sDay = "三十";

            //Format Lunar Date
            //sLuniSolarDate = dt.Year+"年"+dt.Month+"月"+dt.Day+"日 "+Weeks(dt.DayOfWeek.ToString())+" 农历" + sYear + "年" + sMonth + sDay;
            sLuniSolarDate = dt.Year + "年" + dt.Month + "月" + dt.Day + "日 " + ChinaWeek() + " 农历" + sMonth + sDay;
            return sLuniSolarDate;

        }
        /// <summary>
        /// 转换星期为中文
        /// </summary>
        /// <returns></returns>
        public static String ChinaWeek()
        {
            String[] week = new String[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            return week[(int)DateTime.Now.DayOfWeek];
        }
        /// <summary>
        /// Unix时间转DateTime
        /// </summary>
        /// <param name="d">Unix时间</param>
        /// <returns>DateTime</returns>
        public static DateTime UnixToDateTime(Double value)
        {
            DateTime converted = new DateTime(1970, 1, 1);
            DateTime newdatetime = converted.AddSeconds(value);
            return newdatetime.ToLocalTime();
        }
        /// <summary>
        /// DateTime时间格式转换为Unix时间格式
        /// </summary>
        /// <param name="value">时间</param>
        /// <returns>Unix时间</returns>
        public static UInt32 DateTimeToUnix(DateTime value)
        {
            TimeSpan span = (value - new DateTime(1970, 1, 1).ToLocalTime());
            return Convert.ToUInt32(span.TotalSeconds);
        }

        /// <summary>
        /// 取得日期字符串
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="Format">D/d/T/t</param>
        /// <returns></returns>
        public static String GetDateString(Object datetime, String Format)
        {
            try
            {
                switch (Format)
                {
                    case "D":
                        return Convert.ToDateTime(datetime).ToLongDateString();
                    case "d":
                        return Convert.ToDateTime(datetime).ToShortDateString();
                    case "T":
                        return Convert.ToDateTime(datetime).ToLongTimeString();
                    case "t":
                        return Convert.ToDateTime(datetime).ToShortTimeString();
                    default:
                        return Convert.ToDateTime(datetime).ToString(Format);
                }
            }
            catch
            {
                return "无效日期";
            }
        }
        /// <summary>
        /// 判断字符是否是日期
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsDateTime(String str)
        {
            DateTime value;
            return DateTime.TryParse(str, out value);
        }
        /// <summary>
        /// 把秒转换成分钟
        /// </summary>
        /// <returns></returns>
        public static int SecondToMinute(int Second)
        {
            Decimal mm = (Decimal)((Decimal)Second / (Decimal)60);
            return Convert.ToInt32(Math.Ceiling(mm));
        }
        /// <summary>
        /// 返回某年某月最后一天
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>日</returns>
        public static int GetMonthLastDate(int year, int month)
        {
            DateTime lastDay = new DateTime(year, month, new GregorianCalendar().GetDaysInMonth(year, month));
            int Day = lastDay.Day;
            return Day;
        }
        /// <summary>
        /// 返回时间差
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <returns></returns>
        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            try
            {
                //TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                //TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                //TimeSpan ts = ts1.Subtract(ts2).Duration();
                TimeSpan ts = DateTime2 - DateTime1;
                if (ts.Days >= 1)
                {
                    dateDiff = DateTime1.Month.ToString() + "月" + DateTime1.Day.ToString() + "日";
                }
                else
                {
                    if (ts.Hours > 1)
                    {
                        dateDiff = ts.Hours.ToString() + "小时前";
                    }
                    else
                    {
                        dateDiff = ts.Minutes.ToString() + "分钟前";
                    }
                }
            }
            catch
            { }
            return dateDiff;
        }
    }
}
