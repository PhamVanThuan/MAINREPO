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
    public class when_serialising_a_representation_with_an_accept_header_that_has_not_been_set_up : WithFakes
    {
        Establish that = () =>
        {
            linkResolver = An<ILinkResolver>();
            representation = new TestRepresentation(linkResolver);

            acceptHeader = "Banana";

            defaultMediaTypeFormatter = new JsonHalMediaTypeFormatter();

            lazyMediaTypeFormatterRetriever = new Lazy<MediaTypeFormatter>(() => defaultMediaTypeFormatter);

            mediaTypeFormattersAcceptHeaderMapper = new Dictionary<string, Lazy<MediaTypeFormatter>>
            {
                { HalSerialiser.DefaultHalContentType, lazyMediaTypeFormatterRetriever },
            };

            jsonHalMediaTypeFormatter = An<JsonHalMediaTypeFormatter>();

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

        private It should_have_retrieved_the_default_media_type_formatter = () =>
        {
            ReferenceEquals(lazyMediaTypeFormatterRetriever.Value, defaultMediaTypeFormatter).ShouldBeTrue();
        };

        private static ILinkResolver linkResolver;
        private static TestRepresentation representation;
        private static Dictionary<string, Lazy<MediaTypeFormatter>> mediaTypeFormattersAcceptHeaderMapper;
        private static HalSerialiser serialiser;
        private static string acceptHeader;
        private static JsonHalMediaTypeFormatter jsonHalMediaTypeFormatter;
        private static string result;
        private static Lazy<MediaTypeFormatter> lazyMediaTypeFormatterRetriever;
        private static JsonHalMediaTypeFormatter defaultMediaTypeFormatter;
    }
}
