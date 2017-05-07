using CommonUtils;
using System;
using System.IO;
using System.Text;

namespace Mvc4Example
{
    /// <summary>
    /// 压缩并使用Base64编码
    /// </summary>
    public class Base64Filter : Stream
    {
        private Stream deflateStream;
        public Base64Filter(Stream stream)
        {
            deflateStream = stream;
        }
        public override Boolean CanRead { get { return true; } }
        public override Boolean CanSeek { get { return true; } }
        public override Boolean CanWrite { get { return true; } }
        public override void Flush()
        {
            deflateStream.Flush();
        }
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
            Byte[] data = SevenZipSharpHelper.Compress(buffer);
            String txt = Convert.ToBase64String(data, Base64FormattingOptions.InsertLineBreaks);
            data = Encoding.UTF8.GetBytes(txt);
            deflateStream.Write(data, 0, data.Length);
        }
    }
}