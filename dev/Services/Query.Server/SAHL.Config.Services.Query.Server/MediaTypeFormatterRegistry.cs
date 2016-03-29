using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StructureMap.Configuration.DSL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using StructureMap.Configuration.DSL;
using WebApi.Hal;

namespace SAHL.Config.Services.Query.Server
{
    public class MediaTypeFormatterRegistry : Registry
    {
        public static string MediaTypeFormatterInstanceName = "HalMediaTypeFormatters";

        public MediaTypeFormatterRegistry()
        {
            var jsonHalMediaTypeFormatter = new JsonHalMediaTypeFormatter();
            var halSettings = jsonHalMediaTypeFormatter.SerializerSettings;
            halSettings.NullValueHandling = NullValueHandling.Ignore;
            halSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var xmlHalMediaTypeFormatter = new XmlHalMediaTypeFormatter();

            var xmlMediaTypeFormatter = new XmlMediaTypeFormatter();

            var mediaTypeFormatters = new List<MediaTypeFormatter>
            {
                jsonHalMediaTypeFormatter,
                xmlHalMediaTypeFormatter,
                xmlMediaTypeFormatter, //for errors
            };

            IDictionary<Type, MediaTypeFormatter> indexedMediaTypeFormatters = mediaTypeFormatters.ToDictionary(a => a.GetType(), a => a);

            this.For<IDictionary<Type, MediaTypeFormatter>>()
                .Singleton()
                .Use(indexedMediaTypeFormatters)
                .Named(MediaTypeFormatterInstanceName);
        }
    }
}