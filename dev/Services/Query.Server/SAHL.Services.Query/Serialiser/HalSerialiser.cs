using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApi.Hal;

namespace SAHL.Services.Query.Serialiser
{
    public class HalSerialiser : IHalSerialiser
    {
        public const string DefaultHalContentType = HalJsonContentType;
        public const string HalJsonContentType = "application/hal+json";
        public const string HalXmlContentType = "application/hal+xml";

        private readonly IDictionary<string, Lazy<MediaTypeFormatter>> mediaTypeFormattersAcceptHeaderMapper;

        public HalSerialiser(IDictionary<string, Lazy<MediaTypeFormatter>> mediaTypeFormattersAcceptHeaderMapper)
        {
            this.mediaTypeFormattersAcceptHeaderMapper = mediaTypeFormattersAcceptHeaderMapper;
        }

        public string Serialise(Representation item, string acceptHeader = null)
        {
            var retriever = GetMediaTypeFormatterRetriever(acceptHeader);
            var formatter = retriever.Value;

            var stringContent = new StringContent(string.Empty);
            using (var stream = new MemoryStream())
            {
                formatter.WriteToStreamAsync(item.GetType(), item, stream, stringContent, null).Wait();
                stream.Seek(0L, SeekOrigin.Begin);
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private Lazy<MediaTypeFormatter> GetMediaTypeFormatterRetriever(string acceptHeader)
        {
            Lazy<MediaTypeFormatter> retriever;
            if (acceptHeader == null || !this.mediaTypeFormattersAcceptHeaderMapper.TryGetValue(acceptHeader, out retriever))
            {
                retriever = this.mediaTypeFormattersAcceptHeaderMapper[DefaultHalContentType];
            }
            return retriever;
        }
    }
}
