using System;
using System.Collections.Generic;

namespace CommonUtils.Algorithm
{
    /// <summary>
    /// 位图排序算法示例
    /// </summary>
    public class BitmapSort
    {
        /// <summary>
        /// 从0-100中随机获取无序整数集合，并对其进行排序
        /// </summary>
        /// <param name="unSortSet"></param>
        /// <param name="sortSet"></param>
        public void Sort(out HashSet<int> unSortSet, out HashSet<int> sortSet)
        {
            unSortSet = new HashSet<int>();
            sortSet = new HashSet<int>();

            //数据范围集合
            int range = 100;

            //初始化无序的的随机数集合
            Random rand = new Random();
            for (int i = 0; i < range; i++)
            {
                unSortSet.Add(rand.Next(0, range));
            }

            //使用位图排序
            Byte[] bytes = new Byte[range];
            foreach (int i in unSortSet)
            {
                bytes[i] = 1;
            }

            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 1) sortSet.Add(i);
            }
        }
    }
}
