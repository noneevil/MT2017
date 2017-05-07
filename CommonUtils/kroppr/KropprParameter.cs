using System;
using System.Drawing;

namespace CommonUtils
{
    /// <summary>
    /// 在线图片裁剪信息
    /// </summary>
    public struct KropprParameter
    {
        /// <summary>
        /// 图片文件
        /// </summary>
        public Uri image { get; set; }
        /// <summary>
        /// 缩略图质量
        /// </summary>
        public long quality { get; set; }
        /// <summary>
        /// 背景色
        /// </summary>
        public Color background { get; set; }
        /// <summary>
        /// 旋转角度
        /// </summary>
        public float rotation { get; set; }
        /// <summary>
        /// 放大缩小比例
        /// </summary>
        public float xfact { get; set; }
        /// <summary>
        /// 初始参数
        /// </summary>
        public Original original { get; set; }
        /// <summary>
        /// 偏移信息
        /// </summary>
        public Offset offset { get; set; }
        /// <summary>
        /// 裁剪参数
        /// </summary>
        public Cropper cropper { get; set; }
        /// <summary>
        /// 偏移信息
        /// </summary>
        public struct Offset
        {
            /// <summary>
            /// x偏移量
            /// </summary>
            public float x { get; set; }
            /// <summary>
            /// y偏移量
            /// </summary>
            public float y { get; set; }
        }
        /// <summary>
        /// 初始参数
        /// </summary>
        public struct Original
        {
            /// <summary>
            /// 宽度
            /// </summary>
            public int w { get; set; }
            /// <summary>
            /// 高度
            /// </summary>
            public int h { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public float x { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public float y { get; set; }
        }
        /// <summary>
        /// 裁剪参数
        /// </summary>
        public struct Cropper
        {
            /// <summary>
            /// 裁剪宽度
            /// </summary>
            public int width { get; set; }
            /// <summary>
            /// 裁剪高度
            /// </summary>
            public int height { get; set; }
            /// <summary>
            /// 距顶部距离
            /// </summary>
            public int top { get; set; }
            /// <summary>
            /// 距右边距离
            /// </summary>
            public int right { get; set; }
            /// <summary>
            /// 距底部距离
            /// </summary>
            public int bottom { get; set; }
            /// <summary>
            /// 距左距离
            /// </summary>
            public int left { get; set; }
        }
    }
}
