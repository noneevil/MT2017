using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Mvc4Example
{
    /// <summary>
    /// 压缩HTML空白
    /// </summary>
    public class WhiteSpaceFilter : Stream
    {
        private Stream deflateStream;

        public WhiteSpaceFilter(Stream stream)
        {
            deflateStream = stream;
        }
        public override Boolean CanRead { get { return true; } }
        public override Boolean CanSeek { get { return true; } }
        public override Boolean CanWrite { get { return true; } }
        public override void Flush() { deflateStream.Flush(); }
        public override long Length { get { return 0; } }
        public override long Position { get; set; }
        public override int Read(Byte[] buffer, int offset, int count)
        {
            return deflateStream.Read(buffer, offset, count);
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            return deflateStream.Seek(offset, origin);
        }
        public override void SetLength(long value)
        {
            deflateStream.SetLength(value);
        }
        public override void Close()
        {
            deflateStream.Close();
        }
        public override void Write(Byte[] buffer, int offset, int count)
        {
            String html = Encoding.UTF8.GetString(buffer);
            html = Regex.Replace(html, @"\s+\s", "", RegexOptions.Compiled);
            //html = Regex.Replace(html, @" />", "/>", RegexOptions.Compiled);
            //html = Regex.Replace(html, @"\s+\s", "");
            //html = Regex.Replace(html, @"\s+/", "/");
            //html = Regex.Replace(html, @"""\s+", "\"");
            //html = html.Replace("\r\n", "");

            Byte[] data = Encoding.UTF8.GetBytes(html);
            deflateStream.Write(data, 0, data.GetLength(0));
        }
    }
}
