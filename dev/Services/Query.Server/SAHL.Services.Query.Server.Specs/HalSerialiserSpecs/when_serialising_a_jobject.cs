using System;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
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
    public class when_serialising_a_representation : WithFakes
    {
        Establish that = () =>
        {
            linkResolver = An<ILinkResolver>();
            representation = new TestRepresentation(linkResolver);

            acceptHeader = HalSerialiser.DefaultHalContentType;

            mediaTypeFormattersAcceptHeaderMapper = An<IDictionary<string, Lazy<MediaTypeFormatter>>>();

            jsonHalMediaTypeFormatter = An<JsonHalMediaTypeFormatter>();

            lazyMediaTypeFormatterRetriever = new Lazy<MediaTypeFormatter>(() => jsonHalMediaTypeFormatter);

            mediaTypeFormattersAcceptHeaderMapper
                .WhenToldTo(a => a[HalSerialiser.DefaultHalContentType])
                .Return(lazyMediaTypeFormatterRetriever);

            serialiser = new HalSerialiser(mediaTypeFormattersAcceptHeaderMapper);
        };


        private Because of = () =>
        {
            result = serialiser.Serialise(representation, acceptHeader);
        };

        private It should_have_a_non_null_result = () =>
        {
            result.ShouldNotBeNull();
        };

        private It should_have_called_write_on_the_media_formatter = () =>
        {
            jsonHalMediaTypeFormatter
                .WasToldTo(a => a.WriteToStreamAsync(representation.GetType(), representation, Arg.Any<MemoryStream>(), Arg.Any<StringContent>(), null))
                .OnlyOnce();
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
    }
}
