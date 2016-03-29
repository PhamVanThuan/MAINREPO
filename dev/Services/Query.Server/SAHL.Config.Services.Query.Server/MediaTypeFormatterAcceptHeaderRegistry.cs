using SAHL.Services.Query.Serialiser;
using StructureMap;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using WebApi.Hal;

namespace SAHL.Config.Services.Query.Server
{
    public class MediaTypeFormatterAcceptHeaderRegistry : Registry
    {
        public MediaTypeFormatterAcceptHeaderRegistry()
        {
            var mediaTypeFormatterContainer = new Container(new MediaTypeFormatterRegistry());
            var mediaTypeFormatters = mediaTypeFormatterContainer
                .GetInstance<IDictionary<Type, MediaTypeFormatter>>(MediaTypeFormatterRegistry.MediaTypeFormatterInstanceName);

            var mediaTypeFormatterAcceptHeaderMapping = new Dictionary<string, Lazy<MediaTypeFormatter>>(StringComparer.OrdinalIgnoreCase)
            {
                { HalSerialiser.HalJsonContentType, new Lazy<MediaTypeFormatter>(() => mediaTypeFormatters[typeof(JsonHalMediaTypeFormatter)]) },
                { HalSerialiser.HalXmlContentType, new Lazy<MediaTypeFormatter>(() => mediaTypeFormatters[typeof(XmlHalMediaTypeFormatter)]) },
            };

            this.For<IHalSerialiser>()
                .Singleton()
                .Use<HalSerialiser>()
                .Ctor<IDictionary<string, Lazy<MediaTypeFormatter>>>()
                .Is(mediaTypeFormatterAcceptHeaderMapping);
        }
    }
}