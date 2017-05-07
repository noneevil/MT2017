using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;


//http://weblog.west-wind.com/posts/2012/Apr/02/Creating-a-JSONP-Formatter-for-ASPNET-Web-API

namespace Mvc4Example.Api
{
    /// <summary>
    /// Web Api JSONP
    /// </summary>
    public class JsonpMediaTypeFormatter : JsonMediaTypeFormatter
    {
        public string Callback { get; private set; }

        public JsonpMediaTypeFormatter(string callback = null)
        {
            this.Callback = callback;
            //SupportedMediaTypes.Add(new MediaTypeWithQualityHeaderValue("application/x-javascript"));
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            if (string.IsNullOrEmpty(this.Callback))
            {
                return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
            }
            try
            {
                this.WriteToStream(type, value, writeStream, content);
                return Task.Factory.StartNew(() =>
                {
                    new AsyncVoid();
                });

                //return Task.FromResult<AsyncVoid>(new AsyncVoid());//.net framework 4.5
            }
            catch (Exception exception)
            {
                TaskCompletionSource<AsyncVoid> source = new TaskCompletionSource<AsyncVoid>(); //
                source.SetException(exception);
                return source.Task;
            }
        }

        private void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            JsonSerializer serializer = JsonSerializer.Create();// (base.SerializerSettings);
            using (StreamWriter sw = new StreamWriter(writeStream))//, this.SupportedEncodings.First()
            {
                using (JsonTextWriter writer = new JsonTextWriter(sw) { CloseOutput = false })
                {
                    writer.WriteRaw(this.Callback + "(");
                    serializer.Serialize(writer, value);
                    writer.WriteRaw(")");
                }
            }
        }

        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, MediaTypeHeaderValue mediaType)
        {
            if (request.Method != HttpMethod.Get)
            {
                return this;
            }
            string callback;
            if (request.GetQueryNameValuePairs().ToDictionary(pair => pair.Key, pair => pair.Value).TryGetValue("callback", out callback))
            {
                return new JsonpMediaTypeFormatter(callback);
            }
            return this;
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        private struct AsyncVoid { }
    }
}