using System;

namespace WebSite.Interface
{
    /// <summary>
    /// 键值对
    /// </summary>
    [Serializable]
    public struct IKeyValue
    {
        /// <summary>
        /// 名称
        /// </summary>
        public String Key { get; set; }
        /// <summary>
        /// 数据值
        /// </summary>
        public Object Value { get; set; }
    }
}
