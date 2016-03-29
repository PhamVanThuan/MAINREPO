using System;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NSubstitute;
using SAHL.Services.Query.Controllers.Test;
using SAHL.Services.Query.Serialiser;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;
using WebApi.Hal;

namespace SAHL.Services.Query.Server.Specs.HalSerialiserSpecs
{
    public class when_serialising_a_jobject : WithFakes
    {
        Establish that = () =>
        {
            linkResolver = An<ILinkResolver>();
            representation = new TestRepresentation(linkResolver);
            representation.Id = 1;
            representation.Value = "test";

            acceptHeader = HalSerialiser.DefaultHalContentType;

            mediaTypeFormattersAcceptHeaderMapper = An<IDictionary<string, Lazy<MediaTypeFormatter>>>();

            camelCasePropertyNamesContractResolver = new CamelCasePropertyNamesContractResolver();

            serialiserSettings = new JsonSerializerSettings();
            serialiserSettings.ContractResolver = camelCasePropertyNamesContractResolver;

            jsonHalMediaTypeFormatter = new JsonHalMediaTypeFormatter();
            jsonHalMediaTypeFormatter.SerializerSettings = serialiserSettings;

            actionToReturnFormatter = () => jsonHalMediaTypeFormatter;

            lazyMediaTypeFormatterRetriever = new Lazy<MediaTypeFormatter>(actionToReturnFormatter);

            mediaTypeFormattersAcceptHeaderMapper
                .WhenToldTo(a => a[HalSerialiser.DefaultHalContentType])
                .Return(lazyMediaTypeFormatterRetriever);

            serialiser = new HalSerialiser(mediaTypeFormattersAcceptHeaderMapper);

            jsonString =
                @"{""list"":null,""_paging"":null,""totalCount"":null,""listCount"":null,""id"":1,""value"":""test"""
                    + @",""_links"":[],""_embedded"":null}";
        };

        private Because of = () =>
        {
            result = serialiser.Serialise(representation);
        };

        private It should_have_a_non_null_result = () =>
        {
            result.ShouldNotBeNull();
        };

        private It should_have_serialised_using_the_contract_settings_in_the_existing_formatter = () =>
        {
            result.ShouldEqual(jsonString); //only properties should actually lower
        };

        private static ILinkResolver linkResolver;
        private static TestRepresentation representation;
        private static IDictionary<string, Lazy<MediaTypeFormatter>> mediaTypeFormattersAcceptHeaderMapper;
        private static HalSerialiser serialiser;
        private static string acceptHeader;
        private static JsonHalMediaTypeFormatter jsonHalMediaTypeFormatter;
        private static string result;
        private static Lazy<MediaTypeFormatter> lazyMediaTypeFormatterRetriever;
        private static MediaTypeFormatter mediaTypeFormatter;
        private static JObject jsonObject;
        private static Func<MediaTypeFormatter> actionToReturnFormatter;
        private static JsonSerializerSettings serialiserSettings;
        private static IContractResolver camelCasePropertyNamesContractResolver;
        private static string jsonString;
    }
}
