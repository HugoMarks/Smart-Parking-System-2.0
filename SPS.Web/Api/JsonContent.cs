using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SPS.Web.Api
{
    public class JsonContent : HttpContent
    {
        private readonly MemoryStream Stream = new MemoryStream();

        public JsonContent(object value)
        {
            Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var writer = new JsonTextWriter(new StreamWriter(Stream)) { Formatting = Formatting.Indented };
            var serializer = new JsonSerializer();

            serializer.Serialize(writer, value);
            writer.Flush();
            Stream.Position = 0;

        }
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return Stream.CopyToAsync(stream);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = Stream.Length;
            return true;
        }
    }
}