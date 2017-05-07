using System;

namespace CommonUtils.Helper
{
    public abstract class IntHelper
    {
        /// <summary>
        /// 获得单位数,非整除时取整后加一
        /// </summary>
        /// <param name="countNum">总数量</param>
        /// <param name="PageSize">每单位数量</param>
        /// <returns></returns>
        public static int GetPageCount(int countNum, int PageSize)
        {
            if (countNum == 0)
            {
                return 1;
            }
            else
            {
                return countNum % PageSize == 0 ? countNum / PageSize : countNum / PageSize + 1;
            }
        }
        /// <summary>
        /// 选比较大的值
        /// </summary>
        /// <param name="int1"></param>
        /// <param name="int2"></param>
        /// <returns></returns>
        public static int Max(int int1, int int2)
        {
            return int1 > int2 ? int1 : int2;
        }
        /// <summary>
        /// 选比较小的值
        /// </summary>
        /// <param name="int1"></param>
        /// <param name="int2"></param>
        /// <returns></returns>
        public static int Min(int int1, int int2)
        {
            return int1 < int2 ? int1 : int2;
        }
        /// <summary>
        /// double型整除
        /// </summary>
        /// <param name="x">被除数</param>
        /// <param name="y">除数</param>
        /// <param name="ending">是否四舍五入</param>
        /// <returns></returns>
        public static int ExactlyDivisible(Double x, Double y, Boolean ending)
        {
            Double result = x / y;
            if (!ending)
                return Convert.ToInt32(result);
            else
                return Convert.ToInt32(result - x % y / y);
        }
    }
}
