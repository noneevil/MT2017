
namespace CommonUtils.Algorithm
{
    /// <summary>
    /// C#冒泡法排序
    /// </summary>
    public class BubbleSort
    {
        public static void Sort(int[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                for (int j = 0; j < list.Length - i - 1; j++)
                {
                    if (list[j] > list[j + 1])
                    {
                        int Temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = Temp;
                    }
                }
            }
        }
    }
}
